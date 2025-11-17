# Gestión de Perfiles y Permisos - Configuración del Sistema

## Descripción General

Este documento especifica los casos de uso para la **configuración de la arquitectura de seguridad** del sistema: creación de roles y asignación de permisos. Estas funciones son responsabilidad exclusiva del **Administrador** y se realizan desde el formulario **Gestión de Permisos**.

### Conceptos Clave

**Rol (Familia):** Agrupación de permisos que define las capacidades de un tipo de usuario. Los roles se crean como Familias con el prefijo "ROL_" en su nombre.

**Permiso (Patente):** Autorización individual para acceder a una funcionalidad específica del sistema (formulario, menú, reporte, etc.).

**Relación Rol-Permiso:** Un rol puede tener múltiples permisos asignados. Los permisos se asignan directamente al rol y todos los usuarios con ese rol heredan automáticamente esos permisos.

---

## CU-015: Crear Rol

**Actor Principal:** Administrador
**Precondiciones:**
- El usuario tiene permisos de administrador
- El usuario ha accedido a "Gestión de Permisos"

**Postcondiciones:**
- Se ha creado un nuevo rol en el sistema
- El rol está disponible para asignar permisos
- El rol está disponible para asignar a usuarios

### Flujo Principal:

1. El administrador accede al formulario "Gestión de Permisos"
2. El administrador selecciona la pestaña "Gestión de Roles"
3. El administrador hace clic en el botón "Crear Rol"
4. El sistema muestra un cuadro de diálogo solicitando el nombre del nuevo rol
5. El administrador ingresa el nombre del rol (sin el prefijo "ROL_")
   - Ejemplo: ingresa "Ayudante" (el sistema lo convertirá a "ROL_Ayudante")
6. El administrador confirma
7. El sistema valida el nombre del rol (UC-015.1)
8. El sistema agrega automáticamente el prefijo "ROL_" si no lo tiene
9. El sistema crea la nueva Familia (rol) con un GUID único
10. El sistema guarda el rol en la base de datos (tabla `Familia`)
11. El sistema registra la operación en la bitácora de seguridad con:
    - Módulo: "Permisos"
    - Acción: "Creación de rol"
    - Detalle: Nombre completo del rol creado
    - Gravedad: "Alto"
12. El sistema muestra mensaje de éxito
13. El sistema actualiza la lista de roles en el ComboBox
14. El sistema selecciona automáticamente el nuevo rol creado

### Flujos Alternativos:

**5a. Usuario cancela o deja el nombre vacío:**
- El sistema no realiza cambios
- Fin del caso de uso

**7a. Nombre inválido (menos de 3 caracteres):**
- El sistema muestra mensaje "El nombre del rol debe tener al menos 3 caracteres"
- Retorna al paso 4

**7b. Rol duplicado:**
- El sistema muestra mensaje "Ya existe un rol con el nombre 'ROL_[nombre]'"
- Retorna al paso 4

**9a. Error al guardar en base de datos:**
- El sistema captura la excepción
- El sistema registra el error en la bitácora de seguridad con gravedad "Alto"
- El sistema muestra mensaje de error al usuario
- Fin del caso de uso

### Información Específica:

- **Formulario:** `View/UI/WinUi/Administración/gestionPermisos.cs`
- **Evento botón:** `BtnCrearRol_Click` (línea 391)
- **Clase BLL:** `Model/Services/BLL/FamiliaBLL.cs`
- **Método:** `CrearRol(string nombreRol)` (línea 18)
- **Validaciones:**
  - Nombre no vacío (línea 23)
  - Longitud mínima 3 caracteres (línea 29)
  - No duplicado (línea 41)
- **Tabla:** `Familia` (base de datos `SeguridadBiblioteca`)
- **Campos:**
  - `IdFamilia`: UNIQUEIDENTIFIER (GUID generado)
  - `Nombre`: NVARCHAR(100) con prefijo "ROL_"
  - `Descripcion`: NVARCHAR(500) (opcional)
- **Patrón:** Repository Pattern
- **Bitácora:** Se registra con gravedad "Alto" como cambio crítico de seguridad

---

## CU-015.1: Validar Nombre de Rol

**Actor Principal:** Sistema
**Precondiciones:** Se ha ingresado un nombre para el nuevo rol

### Flujo Principal:

1. El sistema valida que el nombre no esté vacío o sea solo espacios en blanco
2. El sistema valida que el nombre tenga al menos 3 caracteres (después de quitar espacios)
3. El sistema construye el nombre completo:
   - Si el nombre ya comienza con "ROL_", lo usa tal cual
   - Si no, agrega el prefijo "ROL_" al nombre
4. El sistema obtiene todos los roles existentes de la base de datos
5. El sistema compara el nombre completo con los nombres existentes (ignorando mayúsculas/minúsculas)
6. Si no existe duplicado, retorna verdadero
7. Si existe duplicado, lanza excepción `ValidacionException`

### Información Específica:

- **Validación vacío:** Línea 23 de `FamiliaBLL.cs`
- **Validación longitud:** Línea 29 de `FamiliaBLL.cs`
- **Agregar prefijo:** Líneas 35-37 de `FamiliaBLL.cs`
- **Validación duplicado:** Líneas 40-44 de `FamiliaBLL.cs`
- **Comparación:** `StringComparison.OrdinalIgnoreCase` (insensible a mayúsculas)

---

## CU-016: Asignar Permisos a Rol

**Actor Principal:** Administrador
**Precondiciones:**
- Existe al menos un rol en el sistema
- Existen permisos (patentes) disponibles en el sistema

**Postcondiciones:**
- Los permisos del rol han sido actualizados
- Todos los usuarios con ese rol heredan automáticamente los nuevos permisos
- Los usuarios con sesión activa son notificados del cambio (patrón Observer)

### Flujo Principal:

1. El administrador accede al formulario "Gestión de Permisos"
2. El administrador selecciona la pestaña "Gestión de Roles"
3. El sistema carga todos los roles disponibles en el ComboBox
4. El sistema selecciona por defecto el rol "Administrador" (si existe)
5. El administrador selecciona un rol del ComboBox
6. El sistema carga los permisos actuales del rol (UC-016.1)
7. El sistema muestra todos los permisos disponibles en un CheckedListBox:
   - Permisos de menú principal
   - Permisos de reportes
   - Permisos de bitácoras
   - Permisos administrativos (Backup, etc.)
8. El sistema marca con check los permisos que el rol ya tiene
9. El administrador marca o desmarca permisos según necesite
10. El administrador hace clic en "Guardar Cambios"
11. El sistema obtiene la lista de permisos marcados
12. El sistema compara con los permisos anteriores para determinar (UC-016.2):
    - Permisos agregados (nuevos marcados)
    - Permisos quitados (previamente marcados, ahora desmarcados)
13. El sistema actualiza las relaciones en la tabla `FamiliaPatente`:
    - Elimina relaciones de permisos quitados
    - Inserta relaciones de permisos agregados
14. El sistema registra el cambio en la bitácora de seguridad con:
    - Módulo: "Permisos"
    - Acción: "Modificación de permisos de rol"
    - Detalle: Lista de permisos agregados y quitados
    - Gravedad: "Alto"
15. El sistema notifica a todos los usuarios que los permisos han cambiado (UC-016.3)
16. El sistema muestra mensaje de éxito: "Los usuarios afectados verán los cambios actualizados automáticamente"

### Flujos Alternativos:

**5a. No hay rol seleccionado al hacer clic en "Guardar":**
- El sistema muestra mensaje "Seleccione un rol"
- No realiza cambios
- Fin del caso de uso

**13a. Error al actualizar permisos:**
- El sistema captura la excepción
- El sistema registra el error en bitácora con gravedad "Alto"
- El sistema muestra mensaje de error
- El sistema revierte los cambios (rollback)
- Fin del caso de uso

### Información Específica:

- **Evento ComboBox:** `CboRoles_SelectedIndexChanged` (línea 348)
- **Evento botón:** `BtnGuardarRol_Click` (línea 535)
- **Clase BLL:** `FamiliaBLL.cs`
- **Método actualizar:** `ActualizarPatentesDeRol(Guid idFamilia, List<Patente> patentes)` (línea 133)
- **Métodos auxiliares:**
  - `AsignarPatenteAFamilia(Guid idFamilia, Guid idPatente)` (línea 215)
  - `QuitarPatenteDeFamilia(Guid idFamilia, Guid idPatente)` (línea 237)
- **Tabla relación:** `FamiliaPatente` (base de datos `SeguridadBiblioteca`)
- **Campos:**
  - `idFamilia`: UNIQUEIDENTIFIER (FK → Familia.IdFamilia)
  - `idPatente`: UNIQUEIDENTIFIER (FK → Patente.IdPatente)
  - PRIMARY KEY compuesta (idFamilia, idPatente)
- **Patrón Observer:** `PermissionManager.Instance.NotifyAllUsersPermissionsChanged()` (línea 580)

---

## CU-016.1: Cargar Permisos del Rol

**Actor Principal:** Sistema
**Precondiciones:** Se ha seleccionado un rol

### Flujo Principal:

1. El sistema obtiene el ID del rol seleccionado
2. El sistema consulta la tabla `FamiliaPatente` para obtener relaciones del rol
3. Para cada relación encontrada:
   - Obtiene la patente correspondiente de la tabla `Patente`
   - Agrega la patente a la lista de permisos del rol
4. El sistema retorna la lista de permisos directos del rol (no recursivos)
5. El sistema recorre todos los ítems del CheckedListBox
6. Para cada permiso en la lista:
   - Si está en los permisos del rol, marca el check
   - Si no está, desmarca el check

### Información Específica:

- **Método carga:** `CargarPatentesDelRol(Familia rol)` (línea 361 de `gestionPermisos.cs`)
- **Método BLL:** `ObtenerPatentesDirectasDeFamilia(Guid idFamilia)` (línea 180 de `FamiliaBLL.cs`)
- **Repositorio:** `FamiliaPatenteRepository.Current.GetChildrenRelations(familia)`
- **Nota:** Solo carga permisos directos, no permisos heredados de familias hijas (si las hubiera)

---

## CU-016.2: Comparar Cambios en Permisos

**Actor Principal:** Sistema
**Precondiciones:** El administrador ha modificado los permisos y ha hecho clic en "Guardar"

### Flujo Principal:

1. El sistema obtiene los permisos anteriores del rol (antes del cambio)
2. El sistema obtiene los permisos seleccionados actualmente (CheckedItems)
3. El sistema identifica permisos agregados:
   - Permisos que están en la selección actual
   - Pero NO estaban en los permisos anteriores
4. El sistema identifica permisos quitados:
   - Permisos que estaban en los permisos anteriores
   - Pero NO están en la selección actual
5. El sistema ejecuta las operaciones de actualización:
   - Para cada permiso quitado: `QuitarPatenteDeFamilia(idFamilia, idPatente)`
   - Para cada permiso agregado: `AsignarPatenteAFamilia(idFamilia, idPatente)`
6. El sistema construye el detalle para la bitácora:
   - Lista nombres de permisos agregados
   - Lista nombres de permisos quitados

### Información Específica:

- **Ubicación:** Líneas 546-568 de `gestionPermisos.cs`
- **LINQ:**
  - Agregados: `patentesSeleccionadas.Where(p => !patentesAnteriores.Any(...))`
  - Quitados: `patentesAnteriores.Where(pa => !patentesSeleccionadas.Any(...))`
- **Formato detalle:** `string.Join(", ", lista.Select(p => p.MenuItemName))`

---

## CU-016.3: Notificar Cambios de Permisos (Observer Pattern)

**Actor Principal:** Sistema
**Precondiciones:** Los permisos de un rol han sido modificados

### Flujo Principal:

1. El sistema obtiene la instancia del `PermissionManager` (Singleton)
2. El sistema llama a `NotifyAllUsersPermissionsChanged()`
3. El `PermissionManager` dispara el evento `PermissionsChanged` a todos los observers registrados
4. Cada formulario abierto en sesiones activas recibe la notificación
5. Cada formulario actualiza su interfaz según los nuevos permisos:
   - Habilita/deshabilita controles
   - Muestra/oculta elementos de menú
   - Actualiza permisos en caché del usuario

### Información Específica:

- **Clase:** `Services.Services.PermissionManager`
- **Método:** `NotifyAllUsersPermissionsChanged()` (línea 580 de `gestionPermisos.cs`)
- **Patrón:** Observer Pattern
- **Alcance:** Notifica a TODOS los usuarios con sesión activa
- **Propósito:** Aplicar cambios de permisos en tiempo real sin necesidad de reiniciar sesión
- **Formularios afectados:** Todos los que implementan `IPermissionObserver`

---

## CU-017: Eliminar Rol

**Actor Principal:** Administrador
**Precondiciones:**
- Existe al menos un rol en el sistema
- El rol a eliminar no tiene usuarios asignados

**Postcondiciones:**
- El rol ha sido eliminado del sistema
- Todas las relaciones `FamiliaPatente` del rol han sido eliminadas
- El rol ya no está disponible para asignar a usuarios

### Flujo Principal:

1. El administrador accede al formulario "Gestión de Permisos"
2. El administrador selecciona la pestaña "Gestión de Roles"
3. El administrador selecciona el rol a eliminar del ComboBox
4. El administrador hace clic en el botón "Eliminar Rol"
5. El sistema valida que haya un rol seleccionado
6. El sistema valida que el rol no tenga usuarios asignados (UC-017.1)
7. El sistema muestra diálogo de confirmación:
   - "¿Está seguro de que desea eliminar el rol '[nombre]'?"
   - "Esta acción no se puede deshacer."
8. El administrador confirma la eliminación
9. El sistema guarda el nombre del rol para la bitácora
10. El sistema elimina todas las relaciones `FamiliaPatente` del rol:
    - Obtiene todas las relaciones del rol
    - Elimina cada relación de la tabla `FamiliaPatente`
11. El sistema elimina el rol de la tabla `Familia`
12. El sistema registra la operación en la bitácora de seguridad con:
    - Módulo: "Permisos"
    - Acción: "Eliminación de rol"
    - Detalle: Nombre del rol eliminado
    - Gravedad: "Alto"
13. El sistema muestra mensaje de éxito
14. El sistema limpia la selección actual
15. El sistema actualiza la lista de roles en el ComboBox

### Flujos Alternativos:

**5a. No hay rol seleccionado:**
- El sistema muestra mensaje "Seleccione un rol"
- No realiza cambios
- Fin del caso de uso

**6a. El rol tiene usuarios asignados:**
- El sistema muestra mensaje "No se puede eliminar el rol '[nombre]' porque tiene usuarios asignados"
- No realiza cambios
- Fin del caso de uso

**8a. Usuario cancela la eliminación:**
- El sistema no realiza cambios
- Fin del caso de uso

**10a. Error al eliminar:**
- El sistema captura la excepción
- El sistema registra el error en bitácora con gravedad "Alto"
- El sistema muestra mensaje de error
- El sistema intenta rollback si es posible
- Fin del caso de uso

### Información Específica:

- **Evento botón:** `BtnEliminarRol_Click` (línea 460 de `gestionPermisos.cs`)
- **Clase BLL:** `FamiliaBLL.cs`
- **Método:** `EliminarRol(Guid idFamilia)` (línea 75)
- **Validaciones:**
  - Rol existe y es válido (línea 80-88)
  - No tiene usuarios asignados (línea 92-99)
- **Proceso de eliminación:**
  1. Obtener relaciones `FamiliaPatente` (línea 102)
  2. Eliminar cada relación (líneas 105-113)
  3. Eliminar el rol (línea 116)
- **Repositorios:**
  - `FamiliaRepository.Current`
  - `FamiliaPatenteRepository.Current`
  - `UsuarioFamiliaRepository.Current`
- **Diálogo confirmación:** `MessageBox.Show` con `MessageBoxButtons.YesNo` (línea 472)

---

## CU-017.1: Validar Rol sin Usuarios Asignados

**Actor Principal:** Sistema
**Precondiciones:** Se intenta eliminar un rol

### Flujo Principal:

1. El sistema obtiene el ID del rol a eliminar
2. El sistema consulta la tabla `UsuarioFamilia` buscando relaciones con ese rol:
   ```sql
   SELECT * FROM UsuarioFamilia WHERE idFamilia = @idFamilia
   ```
3. El sistema cuenta el número de resultados
4. Si el conteo es 0 (cero usuarios):
   - Retorna verdadero (puede eliminar)
5. Si el conteo es mayor a 0:
   - Lanza excepción `ValidacionException` con mensaje descriptivo
   - Indica cuántos usuarios tienen el rol (opcional)

### Información Específica:

- **Ubicación:** Líneas 92-99 de `FamiliaBLL.cs`
- **Método:** `UsuarioFamiliaRepository.Current.SelectAll().Where(uf => uf.idFamilia == idFamilia)`
- **Validación:** Método `.Any()` retorna true si hay al menos un usuario
- **Propósito:** Prevenir eliminación de roles en uso que dejaría usuarios sin permisos

---

## CU-018: Consultar Permisos de Rol

**Actor Principal:** Administrador
**Precondiciones:** Existe al menos un rol en el sistema

**Postcondiciones:** Se muestran los permisos del rol seleccionado

### Flujo Principal:

1. El administrador accede al formulario "Gestión de Permisos"
2. El administrador selecciona la pestaña "Gestión de Roles"
3. El sistema carga todos los roles en el ComboBox con traducción al idioma actual
4. El administrador selecciona un rol
5. El sistema carga los permisos del rol (UC-016.1)
6. El sistema muestra en el CheckedListBox todos los permisos disponibles:
   - Organizados por categoría (Menú, Reportes, Bitácoras, Administrativos)
   - Ordenados por prioridad y nombre
   - Traducidos al idioma actual del usuario
7. El sistema marca con check los permisos que el rol tiene asignados
8. El administrador puede visualizar qué permisos tiene cada rol

### Información Específica:

- **Evento:** `CboRoles_SelectedIndexChanged` (línea 348 de `gestionPermisos.cs`)
- **Carga roles:** `CargarRoles()` (línea 112)
- **Traducción:** `TraducirNombreRol(string nombreRol)` (línea 161)
  - Administrador → `LanguageManager.Translate("rol_administrador")`
  - Bibliotecario → `LanguageManager.Translate("rol_bibliotecario")`
  - Docente → `LanguageManager.Translate("rol_docente")`
- **Carga permisos:** `CargarPatentesDisponibles()` (línea 215)
- **Filtrado permisos:** Solo muestra permisos relevantes (líneas 227-233):
  - `FormName == "menu"` (menú principal)
  - `FormName.Contains("reporte")` (reportes)
  - `FormName.Contains("bitacora")` (bitácoras)
  - `FormName == "renovarPrestamo"` o `"FrmGestionBackup"` (funciones especiales)
- **Ordenamiento:** Por categoría (línea 237), luego por orden y nombre
- **Traducción permisos:** `ObtenerClaveTraduccionPermiso(string nombrePermiso)` (línea 314)
  - Convierte "Consultar Material" → "permiso_consultar_material"
  - Busca traducción en el diccionario de idiomas

---

## Flujo de Configuración de Seguridad

```
1. CREAR ROLES
   Administrador crea rol "ROL_Docente"
        ↓
   Se guarda en tabla Familia
        ↓
   Rol disponible en sistema

2. ASIGNAR PERMISOS A ROL
   Administrador selecciona "ROL_Docente"
        ↓
   Marca permisos:
     ✓ Consultar Material
     ✓ Registrar Préstamo
     ✓ Registrar Devolución
     ✓ Reporte de Préstamos
        ↓
   Sistema guarda en FamiliaPatente:
     - (idRol, idPatente_ConsultarMaterial)
     - (idRol, idPatente_RegistrarPrestamo)
     - (idRol, idPatente_RegistrarDevolucion)
     - (idRol, idPatente_ReportePrestamos)
        ↓
   Rol configurado con permisos

3. ASIGNAR ROL A USUARIO (en otro formulario)
   Administrador asigna "ROL_Docente" a Juan
        ↓
   Juan hereda automáticamente todos los permisos del rol
        ↓
   Juan puede:
     - Consultar Material ✓
     - Registrar Préstamo ✓
     - Registrar Devolución ✓
     - Ver Reporte de Préstamos ✓

4. MODIFICAR PERMISOS DEL ROL
   Administrador agrega permiso "Renovar Préstamo" a "ROL_Docente"
        ↓
   Sistema notifica a todos los usuarios con ese rol
        ↓
   Juan (y todos los Docentes) obtienen el nuevo permiso automáticamente
   Sin necesidad de cerrar sesión
```

---

## Estructura de Datos

### Tabla Familia (Roles)
```sql
CREATE TABLE Familia (
    IdFamilia UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(500)
)

-- Ejemplo de roles:
INSERT INTO Familia (IdFamilia, Nombre, Descripcion) VALUES
(NEWID(), 'ROL_Administrador', 'Rol de Administrador - Control total del sistema'),
(NEWID(), 'ROL_Docente', 'Rol de Docente - Gestión de préstamos y devoluciones'),
(NEWID(), 'ROL_Bibliotecario', 'Rol de Bibliotecario - Gestión de materiales y préstamos'),
(NEWID(), 'ROL_Ayudante', 'Rol de Ayudante - Consulta de información')
```

### Tabla Patente (Permisos)
```sql
CREATE TABLE Patente (
    IdPatente UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FormName NVARCHAR(100) NOT NULL,      -- Nombre del formulario o funcionalidad
    MenuItemName NVARCHAR(100),            -- Texto del menú
    Orden INT,                              -- Orden de visualización
    Descripcion NVARCHAR(500)              -- Descripción del permiso
)

-- Ejemplo de permisos:
INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion) VALUES
(NEWID(), 'menu', 'Consultar Material', 1, 'Permiso para ver el catálogo de materiales'),
(NEWID(), 'menu', 'Registrar Préstamo', 2, 'Permiso para registrar préstamos'),
(NEWID(), 'menu', 'Registrar Devolución', 3, 'Permiso para registrar devoluciones'),
(NEWID(), 'reportePrestamos', 'Reporte de Préstamos', 1, 'Permiso para ver reporte de préstamos'),
(NEWID(), 'FrmGestionBackup', 'Backup y Restore', 1, 'Permiso para hacer respaldos de la BD')
```

### Tabla FamiliaPatente (Relación Rol-Permiso)
```sql
CREATE TABLE FamiliaPatente (
    idFamilia UNIQUEIDENTIFIER NOT NULL,
    idPatente UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (idFamilia, idPatente),
    FOREIGN KEY (idFamilia) REFERENCES Familia(IdFamilia) ON DELETE CASCADE,
    FOREIGN KEY (idPatente) REFERENCES Patente(IdPatente) ON DELETE CASCADE
)

-- Ejemplo: Asignar permisos al rol Docente
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolDocente, @IdPatenteConsultarMaterial),
(@IdRolDocente, @IdPatenteRegistrarPrestamo),
(@IdRolDocente, @IdPatenteRegistrarDevolucion),
(@IdRolDocente, @IdPatenteReportePrestamos)
```

---

## Patrones de Diseño

### 1. Repository Pattern
**Implementación:**
- `FamiliaRepository`: CRUD de roles
- `PatenteRepository`: CRUD de permisos
- `FamiliaPatenteRepository`: CRUD de relaciones rol-permiso

**Beneficio:** Abstracción del acceso a datos, facilita pruebas y mantenimiento.

### 2. Observer Pattern
**Implementación:**
- `PermissionManager` (Singleton)
- Método `NotifyAllUsersPermissionsChanged()`
- Formularios implementan `IPermissionObserver`

**Beneficio:** Actualización en tiempo real de permisos sin reiniciar sesión.

### 3. Singleton Pattern
**Implementación:**
- `PermissionManager.Instance`
- `FamiliaRepository.Current`
- `PatenteRepository.Current`

**Beneficio:** Única instancia compartida, gestión centralizada de estado.

### 4. Composite Pattern
**Implementación:**
- `Component` (abstracto)
- `Familia` (composite)
- `Patente` (leaf)

**Beneficio:** Permite crear jerarquías de permisos (aunque en este caso los roles solo tienen un nivel).

---

## Notas de Implementación

### Multiidioma
- Los nombres de roles se traducen dinámicamente según el idioma del usuario
- Los nombres de permisos también se traducen
- El ComboBox muestra nombres traducidos pero internamente usa GUIDs
- Idiomas soportados: Español (es-AR), Inglés (en-GB)

### Seguridad
- Todas las operaciones se registran en bitácora con gravedad "Alto"
- Se valida que el usuario tenga permisos de administrador
- No se pueden eliminar roles con usuarios asignados
- Los cambios se aplican inmediatamente usando Observer Pattern

### Interfaz de Usuario
- **Pestaña "Gestión de Roles":**
  - ComboBox para seleccionar rol
  - CheckedListBox para marcar/desmarcar permisos
  - Botones: "Crear Rol", "Eliminar Rol", "Guardar Cambios"
- **Permisos organizados por categoría:**
  1. Menú principal (prioridad 1)
  2. Reportes (prioridad 2)
  3. Bitácoras (prioridad 3)
  4. Funciones administrativas (prioridad 4)

### Auditoría
- **Creación de rol:** Se registra con nombre completo del rol
- **Modificación de permisos:** Se registran permisos agregados y quitados
- **Eliminación de rol:** Se registra con nombre del rol eliminado
- Todos los registros incluyen: usuario que realizó la acción, fecha/hora, gravedad "Alto"

---

**Última actualización:** 16 de Noviembre de 2025
**Versión:** 1.0
**Formulario:** Gestión de Permisos - Pestaña "Gestión de Roles"
