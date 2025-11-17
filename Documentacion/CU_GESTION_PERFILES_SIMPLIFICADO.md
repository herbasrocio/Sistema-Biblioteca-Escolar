# Gestión de Perfiles y Permisos - Versión Simplificada

## Conceptos Clave

### Roles del Sistema
El sistema maneja roles predefinidos que se asignan a los usuarios:

- **ROL_Administrador:** Control total del sistema, gestión de usuarios y configuración
- **ROL_Docente:** Gestión de préstamos, devoluciones y consultas de materiales
- **ROL_Bibliotecario:** (rol personalizable según necesidad institucional)
- **ROL_Ayudante:** (rol personalizable según necesidad institucional)

### Asignación Automática de Roles
**El sistema asigna automáticamente el rol seleccionado cuando se crea o modifica un usuario.** No es necesario realizar pasos adicionales de asignación de permisos; el rol incluye todos los permisos necesarios para las funciones del usuario.

### Arquitectura de Permisos
El sistema implementa el patrón **Composite** para la gestión de permisos:

```
Usuario
  └── Familia (Rol)
        ├── Patente 1 (Permiso individual)
        ├── Patente 2 (Permiso individual)
        ├── Familia Hija (Grupo de permisos)
        │     ├── Patente 3
        │     └── Patente 4
        └── ...
```

---

## CU-015: Crear Usuario con Rol

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos de administrador
**Postcondiciones:** Se ha creado un nuevo usuario con su rol asignado automáticamente

### Flujo Principal:
1. El administrador accede a "Gestión de Usuarios"
2. El administrador selecciona "Nuevo Usuario"
3. El sistema muestra el formulario de creación de usuario
4. El administrador ingresa los datos requeridos:
   - Nombre de usuario (único en el sistema)
   - Email (formato válido: usuario@dominio.com)
   - Contraseña (mínimo 6 caracteres)
   - **Rol** (selección de lista desplegable):
     - Administrador
     - Docente
     - Bibliotecario
     - Ayudante
5. El administrador confirma la creación
6. El sistema valida los datos ingresados (UC-015.1)
7. El sistema crea el nuevo usuario
8. **El sistema asigna automáticamente el rol seleccionado al usuario** mediante la relación UsuarioFamilia
9. El sistema hashea la contraseña con SHA-256
10. El sistema calcula el DVH (Dígito Verificador Horizontal) del usuario
11. El sistema registra la operación en la bitácora de seguridad con gravedad "Alto"
12. El sistema muestra mensaje de éxito
13. El sistema actualiza la lista de usuarios

### Flujos Alternativos:

**6a. Datos inválidos:**
- El sistema muestra mensaje específico del error (campo vacío, email inválido, contraseña corta)
- Retorna al paso 4

**6b. Usuario duplicado:**
- El sistema muestra mensaje "Ya existe un usuario con ese nombre"
- Retorna al paso 4

**6c. Rol no seleccionado:**
- El sistema muestra mensaje "Debe seleccionar un rol"
- Retorna al paso 4

### Información Específica:
- **Formulario:** `View/UI/WinUi/Administración/gestionUsuarios.cs`
- **Clase BLL:** `Model/Services/BLL/UsuarioBLL.cs`
- **Método principal:** `CrearUsuario(string nombre, string email, string password, Guid idFamiliaRol)` (línea 79)
- **Asignación automática:** `AsignarFamilia(nuevoUsuario.IdUsuario, idFamiliaRol)` (línea 120)
- **Tabla principal:** `Usuario` (base de datos SeguridadBiblioteca)
- **Tabla relación:** `UsuarioFamilia`
- **Bitácora:** Se registra en `BitacoraSeguridad` con módulo "Usuarios", acción "Creación de usuario"

---

## CU-015.1: Validar Datos de Usuario

**Actor Principal:** Sistema
**Precondiciones:** Se han ingresado datos de nuevo usuario

### Flujo Principal:
1. El sistema valida que el nombre no esté vacío
2. El sistema valida que la contraseña no esté vacía
3. El sistema valida que la contraseña tenga al menos 6 caracteres
4. El sistema valida el formato del email usando expresión regular
5. El sistema verifica que el rol seleccionado exista en la base de datos
6. El sistema verifica que el rol seleccionado sea una Familia válida (propiedad `EsRol = true`)
7. El sistema verifica que el nombre de usuario no esté duplicado
8. Si todas las validaciones pasan, retorna verdadero

### Información Específica:
- **Validaciones:** Líneas 84-104 de `UsuarioBLL.cs`
- **Regex email:** `^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$` (línea 605 de `gestionUsuarios.cs`)
- **Excepciones:**
  - `ValidacionException`: Para errores de validación de datos
  - `UsuarioNoEncontradoException`: Si no se encuentra el rol

---

## CU-016: Modificar Usuario y Cambiar Rol

**Actor Principal:** Administrador
**Precondiciones:** Existe al menos un usuario en el sistema
**Postcondiciones:** El usuario ha sido actualizado con los nuevos datos y rol

### Flujo Principal:
1. El administrador accede a "Gestión de Usuarios"
2. El administrador selecciona un usuario de la lista o lo busca por nombre
3. El administrador selecciona "Modificar"
4. El sistema carga los datos actuales del usuario en el formulario
5. El sistema muestra la contraseña como placeholder (••••••••) por seguridad
6. El administrador modifica los campos deseados:
   - Nombre de usuario
   - Email
   - **Contraseña** (opcional - requiere confirmación para cambiar)
   - **Rol** (selección de lista desplegable)
7. Si el administrador hace clic en el campo contraseña:
   - El sistema pregunta si desea cambiar la contraseña
   - Si confirma, habilita el campo para ingresar nueva contraseña
   - Si no confirma, mantiene la contraseña actual
8. El administrador confirma los cambios
9. El sistema valida los nuevos datos
10. El sistema actualiza el registro del usuario
11. Si la contraseña cambió:
    - El sistema hashea la nueva contraseña con SHA-256
    - El sistema actualiza el campo `Clave`
12. **Si el rol cambió:**
    - El sistema quita el rol anterior del usuario
    - **El sistema asigna automáticamente el nuevo rol**
    - El sistema registra el cambio de rol en la bitácora
13. El sistema recalcula el DVH del usuario (UC-016.1)
14. El sistema registra la operación en la bitácora de seguridad
15. El sistema muestra mensaje de éxito
16. El sistema actualiza la lista de usuarios

### Flujos Alternativos:

**2a. Usuario no encontrado:**
- El sistema muestra mensaje "Usuario no encontrado"
- Fin del caso de uso

**9a. Datos inválidos:**
- El sistema muestra mensaje de error específico
- Retorna al paso 6

**9b. Nuevo nombre duplicado:**
- El sistema muestra mensaje "Ya existe otro usuario con ese nombre"
- Retorna al paso 6

### Información Específica:
- **Clase:** `Services.BLL.UsuarioBLL`
- **Método principal:** `ActualizarUsuario(Guid idUsuario, string nombre, string email, string password, Guid? idFamiliaRol)` (línea 137)
- **Cambio de rol:** `CambiarRol(Guid idUsuario, Guid idNuevaFamiliaRol)` (líneas 195-225)
- **Proceso de cambio:**
  1. Obtiene familias actuales del usuario
  2. Quita todas las familias que son roles (`Where(f => f.EsRol)`)
  3. Asigna la nueva familia de rol
- **Placeholder contraseña:** `••••••••` (constante `PLACEHOLDER_PASSWORD` línea 24 de `gestionUsuarios.cs`)

---

## CU-016.1: Recalcular DVH de Usuario

**Actor Principal:** Sistema
**Precondiciones:** Los datos críticos del usuario han cambiado

### Flujo Principal:
1. El sistema concatena los campos críticos del usuario: `Nombre + Password`
2. El sistema aplica el algoritmo de hash SHA-256 sobre la concatenación
3. El sistema convierte el resultado a string hexadecimal
4. El sistema actualiza el campo `HashDH` en el registro del usuario
5. El sistema guarda los cambios en la base de datos

### Información Específica:
- **Clase:** `Services.Services.CryptographyService`
- **Método hash:** `HashPassword(string textPlainPass)`
- **Algoritmo:** SHA-256
- **Encoding:** Unicode (UTF-16) para compatibilidad con SQL Server NVARCHAR
- **Propósito:** Detectar manipulación no autorizada de datos críticos
- **Campo:** `Usuario.HashDH`

### Nota de Seguridad:
El DVH (Dígito Verificador Horizontal) es una medida de seguridad que permite detectar si los datos del usuario fueron modificados directamente en la base de datos sin pasar por la aplicación. En el login, el sistema recalcula el DVH y lo compara con el almacenado; si no coinciden, lanza una `IntegridadException`.

---

## CU-017: Consultar Usuarios y Roles

**Actor Principal:** Administrador, Bibliotecario
**Precondiciones:** El usuario tiene permisos de consulta de usuarios
**Postcondiciones:** Se muestran los usuarios del sistema con sus roles

### Flujo Principal:
1. El usuario accede a "Gestión de Usuarios"
2. El sistema carga todos los usuarios del sistema
3. El sistema obtiene el rol de cada usuario mediante `Usuario.ObtenerFamiliaRol()`
4. El sistema traduce el nombre del rol al idioma actual del usuario
5. El sistema muestra los datos en una grilla con las siguientes columnas:
   - Nombre de usuario
   - Email
   - Rol (traducido: "Administrador", "Docente", etc.)
6. El usuario puede seleccionar una fila para ver los detalles
7. Al seleccionar un usuario, el sistema muestra:
   - Nombre
   - Email
   - Rol asignado
   - Estado (Activo/Inactivo)

### Información Específica:
- **Formulario:** `gestionUsuarios.cs`
- **Método carga:** `ObtenerTodosLosUsuarios()` (línea 15 de `UsuarioBLL.cs`)
- **Método obtener rol:** `Usuario.ObtenerFamiliaRol()` (DomainModel)
- **Traducción de roles:** Método `TraducirNombreRol(string nombreRol)` (líneas 121-137 de `gestionUsuarios.cs`)
- **Mapeo de traducciones:**
  - "administrador" → `LanguageManager.Translate("rol_administrador")`
  - "bibliotecario" → `LanguageManager.Translate("rol_bibliotecario")`
  - "docente" → `LanguageManager.Translate("rol_docente")`
  - "ayudante" → `LanguageManager.Translate("rol_ayudante")`
- **DataGridView:** Configurado con estilo visual personalizado (colores, fuentes)

---

## CU-018: Verificar Permiso de Usuario

**Actor Principal:** Sistema
**Precondiciones:** Un usuario autenticado intenta acceder a un recurso protegido
**Postcondiciones:** Se determina si el usuario tiene permiso para acceder

### Flujo Principal:
1. El sistema recibe una solicitud de verificación de permiso:
   - Usuario actual (objeto `Usuario`)
   - Nombre del formulario o patente a verificar (string)
2. El sistema obtiene el rol (Familia) del usuario mediante `Usuario.ObtenerFamiliaRol()`
3. El sistema busca la patente en los permisos directos del usuario
4. Si no la encuentra directamente, busca en la jerarquía del rol:
   - Recorre la estructura Composite del rol (Familia)
   - Busca recursivamente en familias hijas
   - Busca en las patentes de cada familia
5. Si encuentra la patente requerida en cualquier nivel, retorna `true`
6. Si no la encuentra después de recorrer toda la jerarquía, retorna `false`
7. El sistema permite o deniega el acceso según el resultado

### Flujos Alternativos:

**2a. Usuario sin rol asignado:**
- El sistema retorna `false`
- El acceso es denegado

### Información Específica:
- **Clase:** `Services.DomainModel.Security.Composite.Usuario`
- **Método:** `TienePermiso(string patente)` (implementa búsqueda recursiva)
- **Método auxiliar:** `TieneRol(string nombreRol)` - verifica si el usuario tiene un rol específico
- **Patrón de diseño:** Composite Pattern
- **Estructura jerárquica:**
  ```
  Usuario
    └── Familia (ROL_Administrador)
          ├── Patente: frmGestionUsuarios
          ├── Patente: frmGestionPermisos
          ├── Familia: Gestión de Usuarios
          │     ├── Patente: Alta de Usuario
          │     ├── Patente: Baja de Usuario
          │     └── Patente: Modificar Usuario
          └── Familia: Configuración
                ├── Patente: frmBackup
                └── Patente: frmVisorLogs
  ```
- **Búsqueda:** Profundidad primero (DFS - Depth First Search)
- **Uso típico:**
  ```csharp
  if (SessionManager.CurrentUser.TienePermiso("frmGestionUsuarios"))
  {
      // Permitir acceso al formulario
  }
  ```

---

## CU-019: Eliminar Usuario

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos de administrador
**Postcondiciones:** El usuario y todas sus relaciones han sido eliminados

### Flujo Principal:
1. El administrador accede a "Gestión de Usuarios"
2. El administrador selecciona un usuario de la lista
3. El administrador selecciona "Eliminar"
4. El sistema valida que el usuario seleccionado no sea el administrador logueado (UC-019.1)
5. El sistema muestra diálogo de confirmación:
   - "¿Está seguro que desea eliminar el usuario '[nombre]'?"
6. El administrador confirma la eliminación
7. El sistema guarda temporalmente los datos del usuario para la bitácora:
   - Nombre
   - Email
   - Rol
8. El sistema elimina en orden:
   - Relaciones usuario-familia (tabla `UsuarioFamilia`)
   - Relaciones usuario-patente si existen (tabla `UsuarioPatente`)
   - Registro del usuario (tabla `Usuario`)
9. El sistema registra la operación en la bitácora de seguridad con:
   - Módulo: "Usuarios"
   - Acción: "Eliminación de usuario"
   - Detalle: Nombre, email y rol del usuario eliminado
   - Gravedad: "Alto"
10. El sistema muestra mensaje de éxito
11. El sistema actualiza la lista de usuarios

### Flujos Alternativos:

**4a. Intenta eliminar su propio usuario:**
- El sistema muestra mensaje "No puede eliminar su propio usuario"
- El sistema no permite continuar
- Fin del caso de uso

**6a. Usuario cancela:**
- El sistema no realiza cambios
- Fin del caso de uso

**8a. Error al eliminar (usuario referenciado en bitácora):**
- El sistema captura la excepción
- El sistema registra el error en la bitácora de seguridad
- El sistema muestra mensaje de error descriptivo
- Fin del caso de uso

### Información Específica:
- **Clase:** `Services.BLL.UsuarioBLL`
- **Método:** `EliminarUsuario(Guid idUsuario)` (línea 230)
- **Validación auto-eliminación:** Línea 513 de `gestionUsuarios.cs`
- **Confirmación:** `MessageBox.Show` con `MessageBoxButtons.YesNo`
- **Integridad referencial:** Las relaciones UsuarioFamilia y UsuarioPatente se eliminan automáticamente por CASCADE DELETE en la base de datos
- **Bitácora:** Se preservan los registros de bitácora del usuario eliminado para auditoría

---

## CU-019.1: Validar Eliminación de Usuario

**Actor Principal:** Sistema
**Precondiciones:** Se intenta eliminar un usuario

### Flujo Principal:
1. El sistema obtiene el ID del usuario a eliminar
2. El sistema obtiene el ID del usuario actualmente logueado
3. El sistema compara ambos IDs
4. Si son diferentes, retorna `true` (puede eliminar)
5. Si son iguales, retorna `false` (no puede eliminar)

### Información Específica:
- **Validación:** `_usuarioSeleccionado.IdUsuario == _usuarioLogueado.IdUsuario`
- **Ubicación:** Línea 513 de `gestionUsuarios.cs`
- **Propósito:** Prevenir que un administrador se elimine a sí mismo y quede sin acceso al sistema

---

## CU-020: Buscar Usuario

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos de gestión de usuarios
**Postcondiciones:** Se muestra el usuario encontrado

### Flujo Principal:
1. El administrador accede a "Gestión de Usuarios"
2. El administrador ingresa el nombre de usuario a buscar en el campo de búsqueda
3. El administrador hace clic en "Buscar"
4. El sistema valida que se haya ingresado un nombre
5. El sistema busca el usuario por nombre exacto
6. El sistema carga los datos del usuario encontrado:
   - Nombre
   - Email
   - Rol
   - Estado (Activo/Inactivo)
7. El sistema muestra los datos en el formulario
8. El sistema muestra mensaje "Usuario '[nombre]' encontrado"

### Flujos Alternativos:

**4a. Campo de búsqueda vacío:**
- El sistema muestra mensaje "Ingrese un nombre de usuario para buscar"
- Fin del caso de uso

**5a. Usuario no encontrado:**
- El sistema muestra mensaje "Usuario no encontrado"
- El sistema limpia los campos del formulario
- Fin del caso de uso

### Información Específica:
- **Método:** `ObtenerUsuarioPorNombre(string nombre)` (línea 52 de `UsuarioBLL.cs`)
- **Evento botón:** `BtnBuscar_Click` (línea 268 de `gestionUsuarios.cs`)
- **Búsqueda:** Exacta, sensible a mayúsculas/minúsculas
- **Repositorio:** `UsuarioRepository.Current.SelectOneByName(nombre)`

---

## Resumen de Flujos de Asignación de Roles

### Creación de Usuario
```
Usuario nuevo
    ↓
Seleccionar rol del ComboBox
    ↓
CrearUsuario(nombre, email, password, idFamiliaRol)
    ↓
AsignarFamilia(idUsuario, idFamiliaRol) [AUTOMÁTICO]
    ↓
INSERT INTO UsuarioFamilia (idUsuario, idFamilia)
    ↓
Usuario creado con rol asignado
```

### Modificación de Rol
```
Usuario existente con ROL_A
    ↓
Seleccionar nuevo ROL_B
    ↓
ActualizarUsuario(..., idNuevoRol)
    ↓
CambiarRol(idUsuario, idNuevoRol)
    ↓
QuitarFamilia(idUsuario, ROL_A)
    ↓
AsignarFamilia(idUsuario, ROL_B) [AUTOMÁTICO]
    ↓
Usuario actualizado con nuevo rol
```

### Verificación de Permisos
```
Usuario intenta acceder a frmGestionUsuarios
    ↓
Sistema: TienePermiso("frmGestionUsuarios")
    ↓
Usuario.ObtenerFamiliaRol() → ROL_Administrador
    ↓
Búsqueda recursiva en estructura Composite
    ↓
¿Encontró patente frmGestionUsuarios? → SÍ
    ↓
Acceso PERMITIDO
```

---

## Patrones de Diseño Utilizados

### 1. Composite Pattern
**Ubicación:** `Services.DomainModel.Security.Composite`

**Estructura:**
- `Component` (abstracto): Clase base para permisos
- `Familia` (composite): Contiene otros componentes (patentes o familias)
- `Patente` (leaf): Permiso individual, no contiene otros componentes
- `Usuario`: Contiene una lista de `Component`

**Beneficio:** Permite tratar patentes individuales y grupos de permisos (familias) de manera uniforme.

### 2. Repository Pattern
**Ubicación:** `Services.DAL.Implementations`

**Implementaciones:**
- `UsuarioRepository`: CRUD de usuarios
- `FamiliaRepository`: CRUD de familias (roles)
- `PatenteRepository`: CRUD de patentes
- `UsuarioFamiliaRepository`: Relaciones usuario-rol
- `UsuarioPatenteRepository`: Relaciones usuario-patente

**Beneficio:** Abstrae el acceso a datos y facilita el mantenimiento.

### 3. Unit of Work Pattern
**Ubicación:** `Services.DAL.UnitOfWork`

**Uso:** Agrupa múltiples operaciones de repositorio en una transacción atómica.

### 4. Singleton Pattern
**Ubicación:** Repositorios (`Current` property)

**Beneficio:** Garantiza una única instancia de cada repositorio.

---

## Tablas de Base de Datos Involucradas

### SeguridadBiblioteca.Usuario
```sql
IdUsuario UNIQUEIDENTIFIER PRIMARY KEY
Nombre NVARCHAR(100) NOT NULL UNIQUE
Email NVARCHAR(200)
Password NVARCHAR(100) -- texto plano (no usado en login)
Clave NVARCHAR(200) -- hash SHA-256
Activo BIT DEFAULT 1
IdiomaPreferido NVARCHAR(10) DEFAULT 'es-AR'
HashDH NVARCHAR(200) -- Dígito Verificador Horizontal
FechaCreacion DATETIME DEFAULT GETDATE()
FechaUltimoAcceso DATETIME
```

### SeguridadBiblioteca.Familia
```sql
IdFamilia UNIQUEIDENTIFIER PRIMARY KEY
Nombre NVARCHAR(100) NOT NULL UNIQUE
Descripcion NVARCHAR(500)
-- Nota: Los roles se identifican por prefijo "ROL_" en el nombre
```

### SeguridadBiblioteca.UsuarioFamilia
```sql
idUsuario UNIQUEIDENTIFIER FOREIGN KEY → Usuario.IdUsuario
idFamilia UNIQUEIDENTIFIER FOREIGN KEY → Familia.IdFamilia
PRIMARY KEY (idUsuario, idFamilia)
```

### SeguridadBiblioteca.Patente
```sql
IdPatente UNIQUEIDENTIFIER PRIMARY KEY
FormName NVARCHAR(100) -- Nombre del formulario
MenuItemName NVARCHAR(100)
Orden INT
Descripcion NVARCHAR(500)
```

### SeguridadBiblioteca.FamiliaPatente
```sql
idFamilia UNIQUEIDENTIFIER FOREIGN KEY → Familia.IdFamilia
idPatente UNIQUEIDENTIFIER FOREIGN KEY → Patente.IdPatente
PRIMARY KEY (idFamilia, idPatente)
```

### SeguridadBiblioteca.BitacoraSeguridad
```sql
IdBitacora INT IDENTITY PRIMARY KEY
IdUsuario UNIQUEIDENTIFIER FOREIGN KEY → Usuario.IdUsuario
NombreUsuario NVARCHAR(100)
TipoEvento NVARCHAR(50) -- Error, Seguridad, CambioCritico
Modulo NVARCHAR(100)
Accion NVARCHAR(200)
Detalle NVARCHAR(MAX)
Gravedad NVARCHAR(20) -- Bajo, Medio, Alto
Fecha DATETIME DEFAULT GETDATE()
DireccionIP NVARCHAR(50)
```

---

## Notas de Implementación

### Seguridad
1. **Contraseñas:** Se almacenan hasheadas con SHA-256, nunca en texto plano
2. **DVH:** Dígito Verificador Horizontal para detectar manipulación de datos
3. **Bitácora:** Todos los cambios de usuarios y roles se registran con gravedad "Alto"
4. **Validaciones:** Email con regex, contraseña mínima 6 caracteres

### Multiidioma
- Los nombres de roles se traducen dinámicamente según el idioma del usuario
- Traducciones disponibles: Español (es-AR), Inglés (en-GB)
- El ComboBox de roles muestra nombres traducidos pero internamente usa los IDs

### Interfaz de Usuario
- Placeholder de contraseña (••••••••) en modo edición por seguridad
- Confirmación requerida para cambiar contraseña en modo edición
- Validación de email en tiempo real con feedback visual
- DataGridView con estilo personalizado y colores alternados

### Auditoría
- Creación de usuario: Se registra con nombre, email y rol asignado
- Modificación: Se registra qué campos cambiaron (nombre, email, contraseña, rol)
- Cambio de rol: Se registra el rol anterior y el nuevo
- Eliminación: Se preservan los datos del usuario eliminado en el registro de bitácora

---

**Última actualización:** 16 de Noviembre de 2025
**Versión:** Simplificada para reflejar asignación automática de roles
