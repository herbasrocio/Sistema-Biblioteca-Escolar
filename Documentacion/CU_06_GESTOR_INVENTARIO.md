# CU 06 - GESTOR DE INVENTARIO (EJEMPLARES)

## Descripción General

El Gestor de Inventario permite administrar las copias físicas (ejemplares) de cada material del catálogo. Cada ejemplar representa un objeto físico individual con su propio estado, ubicación y código de barras.

### Conceptos Clave

**Ejemplar:** Copia física individual de un material. Cada libro, revista o DVD físico es un ejemplar.

**Estados de Ejemplar:**
- **Disponible:** Puede ser prestado
- **Prestado:** Actualmente en préstamo
- **En Reparación:** Temporalmente no disponible por mantenimiento
- **No Disponible:** Fuera de circulación (dañado, extraviado)

**Relación:** Un Material puede tener múltiples Ejemplares (1:N)

---

## Casos de Uso del Módulo

- **CU-06.1:** Agregar Ejemplar
- **CU-06.2:** Editar Ejemplar
- **CU-06.3:** Eliminar Ejemplar

---

## CU-06.1: Agregar Ejemplar

**Actor Principal:** Bibliotecario, Administrador

**Precondiciones:**
- El usuario tiene permisos para gestionar ejemplares
- Existe un material al cual asignar el ejemplar

**Postcondiciones:**
- Se ha creado un nuevo ejemplar físico
- Se ha incrementado la cantidad total del material
- Se ha actualizado la cantidad disponible del material

### Flujo Principal:

1. El usuario accede a "Gestionar Ejemplares" desde un material específico
2. El sistema muestra el formulario de gestión de ejemplares
3. El sistema carga y muestra:
   - Datos del material (título, autor)
   - Lista de ejemplares existentes
4. El usuario hace clic en "Agregar Ejemplar"
5. El sistema muestra formulario para nuevo ejemplar
6. El usuario ingresa:
   - **Número de ejemplar** (generado automáticamente como siguiente número disponible)
   - **Código de ejemplar/barras** (opcional, debe ser único)
   - **Ubicación física** (ej: "Estante A3, Nivel 2")
   - **Estado inicial** (por defecto: Disponible)
   - **Observaciones** (opcional)
7. El usuario confirma
8. El sistema valida los datos (CU-06.1.1):
   - El material asociado existe
   - Número de ejemplar > 0
   - Número de ejemplar no duplicado para este material
   - Código de barras único (si se ingresó)
9. El sistema genera GUID para el ejemplar
10. El sistema establece fecha de registro = fecha actual
11. El sistema guarda el ejemplar en la base de datos
12. El sistema actualiza automáticamente las cantidades del material:
    - CantidadTotal = count de ejemplares
    - CantidadDisponible = count de ejemplares en estado "Disponible"
13. El sistema registra la operación en bitácora de operaciones
14. El sistema actualiza la grilla de ejemplares
15. El sistema muestra mensaje de éxito

### Flujos Alternativos:

**8a. Número de ejemplar duplicado:**
- El sistema muestra "Ya existe un ejemplar con el número [N] para este material"
- Retorna al paso 6

**8b. Código de barras duplicado:**
- El sistema muestra "El código de barras ya está en uso por otro ejemplar"
- Retorna al paso 6

**8c. Material no existe:**
- El sistema muestra "El material asociado no existe"
- Fin del caso de uso

### Información Específica:

- **Formulario:** `View/UI/WinUi/Administración/GestionarEjemplares.cs`
- **Evento botón:** `BtnAgregar_Click` (línea 42)
- **Clase BLL:** `Model/BLL/EjemplarBLL.cs`
- **Método principal:** `GuardarEjemplar(Ejemplar ejemplar)` (línea 74)
- **Método actualización:** `ActualizarCantidadesMaterial(Guid idMaterial)` (línea 221)
- **Validaciones:**
  - IdMaterial debe existir (línea 77-82)
  - NumeroEjemplar > 0 (línea 84-85)
  - NumeroEjemplar no duplicado (línea 88-90)
  - CodigoEjemplar único (línea 93-98)
- **Tabla:** `Ejemplar` (base de datos `NegocioBiblioteca`)
- **Campos:**
  - `IdEjemplar`: UNIQUEIDENTIFIER (GUID generado automáticamente)
  - `IdMaterial`: UNIQUEIDENTIFIER (FK → Material.IdMaterial)
  - `NumeroEjemplar`: INT (secuencial por material: 1, 2, 3...)
  - `CodigoEjemplar`: NVARCHAR(50) (código de barras, único, opcional)
  - `Estado`: NVARCHAR(20) (enum: Disponible, Prestado, EnReparacion, NoDisponible)
  - `Ubicacion`: NVARCHAR(100) (ubicación física)
  - `Observaciones`: NVARCHAR(MAX) (opcional)
  - `FechaRegistro`: DATETIME (generada automáticamente)
  - `Activo`: BIT (default: true)

### Notas Importantes:

- El número de ejemplar es secuencial por material (no global)
- El código de barras es opcional pero si se usa debe ser único en todo el sistema
- El sistema recalcula automáticamente CantidadTotal y CantidadDisponible del material
- Estado por defecto es "Disponible"

---

## CU-06.1.1: Validar Datos de Ejemplar

**Actor Principal:** Sistema

**Precondiciones:** Se han ingresado datos para un nuevo ejemplar

### Flujo Principal:

1. El sistema valida que IdMaterial no sea Guid.Empty
2. El sistema obtiene el material asociado y verifica que exista
3. El sistema valida que NumeroEjemplar sea mayor a 0
4. El sistema obtiene todos los ejemplares existentes del material
5. El sistema verifica que el NumeroEjemplar no esté duplicado
6. Si se ingresó CodigoEjemplar:
   - El sistema busca ejemplares con ese código
   - Si encuentra otro ejemplar con ese código, lanza excepción
7. Si todas las validaciones pasan, retorna verdadero

### Información Específica:

- **Ubicación:** Líneas 76-98 de `EjemplarBLL.cs`
- **Excepciones:** `Exception` con mensaje descriptivo

---

## CU-06.2: Editar Ejemplar

**Actor Principal:** Bibliotecario, Administrador

**Precondiciones:**
- El ejemplar existe en el sistema
- El usuario tiene permisos para editar ejemplares

**Postcondiciones:**
- Los datos del ejemplar han sido actualizados
- Si cambió el estado, se registra en el historial
- Se actualizan las cantidades del material

### Flujo Principal:

1. El usuario visualiza la lista de ejemplares de un material
2. El usuario selecciona un ejemplar de la grilla
3. El usuario hace clic en "Editar"
4. El sistema carga los datos actuales del ejemplar
5. El sistema muestra formulario de edición con:
   - Número de ejemplar (solo lectura, no editable)
   - Código de ejemplar/barras
   - Ubicación física
   - Estado (ComboBox con opciones: Disponible, Prestado, En Reparación, No Disponible)
   - Observaciones
6. El usuario modifica los campos deseados
7. **Si el usuario cambia el estado:**
   - El sistema solicita motivo del cambio (opcional)
   - El sistema registra el cambio en HistorialEstadoEjemplar (CU-06.2.1)
8. El usuario confirma los cambios
9. El sistema valida los datos:
   - IdEjemplar válido
   - NumeroEjemplar > 0
   - CodigoEjemplar único (si cambió)
10. El sistema actualiza el ejemplar en la base de datos
11. El sistema actualiza automáticamente las cantidades del material
12. El sistema registra la operación en bitácora
13. El sistema actualiza la grilla
14. El sistema muestra mensaje de éxito

### Flujos Alternativos:

**9a. Código de barras duplicado:**
- El sistema muestra "El código de barras ya está en uso por otro ejemplar"
- Retorna al paso 6

**2a. Ejemplar en estado Prestado y se intenta cambiar ubicación:**
- El sistema permite el cambio pero muestra advertencia
- Continúa normalmente

### Información Específica:

- **Evento botón:** `BtnEditar_Click` (línea 43 de `GestionarEjemplares.cs`)
- **Método actualización:** `ActualizarEjemplar(Ejemplar ejemplar)` (línea 109 de `EjemplarBLL.cs`)
- **Método cambio estado:** `CambiarEstado(Guid idEjemplar, EstadoMaterial nuevoEstado, Guid? idUsuario, string motivo)` (línea 135)
- **Validaciones:**
  - IdEjemplar no vacío (línea 112)
  - NumeroEjemplar > 0 (línea 115)
  - CodigoEjemplar único (línea 119-124)
- **Campos editables:** CodigoEjemplar, Ubicacion, Estado, Observaciones
- **Campos NO editables:** IdEjemplar, IdMaterial, NumeroEjemplar (asignados en creación)

### Cambio de Estado

Cuando se cambia el estado del ejemplar:

**Restricciones por estado:**
- **Disponible → Prestado:** Solo mediante proceso de préstamo (CU-08.1)
- **Prestado → Disponible:** Solo mediante proceso de devolución (CU-08.2)
- **Cualquiera → En Reparación:** Permitido (requiere motivo)
- **Cualquiera → No Disponible:** Permitido (requiere motivo)
- **En Reparación → Disponible:** Permitido (ejemplar reparado)
- **No Disponible → Disponible:** Permitido (ejemplar recuperado)

**Impacto en cantidades:**
- Solo ejemplares en estado "Disponible" cuentan para CantidadDisponible
- Todos los ejemplares activos cuentan para CantidadTotal

---

## CU-06.2.1: Registrar Cambio de Estado en Historial

**Actor Principal:** Sistema

**Precondiciones:** El estado del ejemplar está cambiando

### Flujo Principal:

1. El sistema obtiene el ejemplar actual
2. El sistema guarda el estado anterior
3. El sistema compara estado anterior con nuevo estado
4. Si son diferentes:
   - El sistema actualiza el estado del ejemplar
   - El sistema crea registro en tabla `HistorialEstadoEjemplar`:
     - IdEjemplar
     - EstadoAnterior
     - EstadoNuevo
     - IdUsuario (quien realizó el cambio)
     - Motivo (razón del cambio)
     - TipoCambio: Manual (vs. Automático por préstamo/devolución)
     - FechaCambio: GETDATE()
   - El sistema guarda el registro de historial
5. El sistema actualiza las cantidades del material

### Información Específica:

- **Método:** `CambiarEstado()` líneas 135-166 de `EjemplarBLL.cs`
- **Tabla historial:** `HistorialEstadoEjemplar`
- **Propósito:** Auditoría completa de cambios de estado
- **Tipos de cambio:**
  - Manual: Usuario cambió estado manualmente
  - Automatico: Sistema cambió estado por préstamo/devolución

---

## CU-06.3: Eliminar Ejemplar

**Actor Principal:** Administrador

**Precondiciones:**
- El ejemplar existe
- El ejemplar NO está en estado "Prestado"
- El usuario tiene permisos de administrador

**Postcondiciones:**
- El ejemplar ha sido eliminado (borrado lógico: Activo = false)
- Se decrementan las cantidades del material
- Se registra la operación en bitácora

### Flujo Principal:

1. El usuario visualiza la lista de ejemplares
2. El usuario selecciona un ejemplar
3. El usuario hace clic en "Eliminar"
4. El sistema valida que el ejemplar no esté prestado (CU-06.3.1)
5. El sistema muestra diálogo de confirmación:
   - "¿Está seguro de eliminar el ejemplar #[número]?"
   - "Esta acción no se puede deshacer"
6. El usuario confirma
7. El sistema guarda el IdMaterial para actualización posterior
8. El sistema realiza borrado lógico:
   - Establece campo `Activo = false`
   - NO elimina físicamente el registro (preserva para auditoría)
9. El sistema actualiza automáticamente las cantidades del material:
   - CantidadTotal = count de ejemplares activos
   - CantidadDisponible = count de ejemplares activos y disponibles
10. El sistema registra en bitácora de operaciones con:
    - Módulo: "Inventario"
    - Acción: "Eliminación de ejemplar"
    - Detalle: Material, número de ejemplar, motivo
11. El sistema actualiza la grilla (no muestra ejemplares inactivos)
12. El sistema muestra mensaje de éxito

### Flujos Alternativos:

**4a. Ejemplar está prestado:**
- El sistema muestra "No se puede eliminar un ejemplar que está prestado"
- Debe registrarse la devolución primero
- Fin del caso de uso

**6a. Usuario cancela:**
- No se realizan cambios
- Fin del caso de uso

### Información Específica:

- **Evento botón:** `BtnEliminar_Click` (línea 45 de `GestionarEjemplares.cs`)
- **Método:** `EliminarEjemplar(Ejemplar ejemplar)` (línea 171 de `EjemplarBLL.cs`)
- **Validación crítica:** Líneas 174-175 - verifica que Estado != Prestado
- **Tipo de borrado:** Lógico (Activo = false)
- **Razón borrado lógico:**
  - Preserva historial de préstamos
  - Mantiene integridad referencial
  - Permite auditoría completa
  - Posibilita recuperación si fue error
- **Actualización cantidades:** Línea 180-181

---

## CU-06.3.1: Validar Ejemplar No Prestado

**Actor Principal:** Sistema

**Precondiciones:** Se intenta eliminar un ejemplar

### Flujo Principal:

1. El sistema obtiene el ejemplar a eliminar
2. El sistema verifica el campo Estado
3. Si Estado == EstadoMaterial.Prestado:
   - Lanza excepción con mensaje descriptivo
   - Aborta la eliminación
4. Si Estado != Prestado:
   - Permite continuar con la eliminación

### Información Específica:

- **Validación:** Líneas 174-175 de `EjemplarBLL.cs`
- **Lógica:** No se puede eliminar si está en manos de un alumno
- **Proceso correcto:**
  1. Registrar devolución (cambia estado a Disponible)
  2. Luego eliminar ejemplar

---

## Flujos Relacionados con Otros Módulos

### Con CU-05 (Gestor de Catálogo):

```
Material creado
    ↓
Agregar primer ejemplar (CU-06.1)
    ↓
Material.CantidadTotal = 1
Material.CantidadDisponible = 1
```

### Con CU-08 (Préstamos):

```
Registrar Préstamo (CU-08.1)
    ↓
Sistema llama: EjemplarBLL.PrestarEjemplar(idEjemplar)
    ↓
Ejemplar.Estado = Prestado
    ↓
Material.CantidadDisponible se reduce automáticamente
```

### Con Devoluciones:

```
Registrar Devolución (CU-08.2)
    ↓
Sistema llama: EjemplarBLL.DevolverEjemplar(idEjemplar)
    ↓
Ejemplar.Estado = Disponible
    ↓
Material.CantidadDisponible se incrementa automáticamente
```

---

## Métodos Auxiliares Importantes

### CrearEjemplaresParaMaterial (Creación masiva)

**Propósito:** Crear múltiples ejemplares automáticamente para un material nuevo

**Flujo:**
1. Recibe idMaterial y cantidad
2. Obtiene número del último ejemplar existente
3. Crea N ejemplares con numeración secuencial
4. Todos con estado Disponible
5. Actualiza cantidades del material

**Uso típico:** Al registrar un material nuevo, crear automáticamente los ejemplares

**Información Específica:**
- **Método:** `CrearEjemplaresParaMaterial(Guid idMaterial, int cantidad)` (línea 187)

---

### ActualizarCantidadesMaterial (Recálculo automático)

**Propósito:** Recalcular CantidadTotal y CantidadDisponible basándose en los ejemplares

**Flujo:**
1. Obtiene todos los ejemplares activos del material
2. Cuenta total de ejemplares activos
3. Cuenta ejemplares en estado Disponible
4. Actualiza Material.CantidadTotal
5. Actualiza Material.CantidadDisponible
6. Guarda cambios en Material

**Cuándo se ejecuta:**
- Al agregar ejemplar
- Al editar ejemplar (cambio de estado)
- Al eliminar ejemplar
- Al prestar ejemplar
- Al devolver ejemplar

**Información Específica:**
- **Método:** Privado, líneas 221-238 de `EjemplarBLL.cs`
- **Llamado automáticamente:** No requiere invocación manual

---

## Estructura de Datos

### Tabla Ejemplar

```sql
CREATE TABLE Ejemplar (
    IdEjemplar UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdMaterial UNIQUEIDENTIFIER NOT NULL,
    NumeroEjemplar INT NOT NULL,
    CodigoEjemplar NVARCHAR(50) NULL UNIQUE,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Disponible',
    Ubicacion NVARCHAR(100) NULL,
    Observaciones NVARCHAR(MAX) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Ejemplar_Material FOREIGN KEY (IdMaterial)
        REFERENCES Material(IdMaterial),
    CONSTRAINT UQ_Ejemplar_Material_Numero UNIQUE (IdMaterial, NumeroEjemplar),
    CONSTRAINT CK_Ejemplar_NumeroPositivo CHECK (NumeroEjemplar > 0)
)

-- Índices para optimización
CREATE INDEX IX_Ejemplar_IdMaterial ON Ejemplar(IdMaterial)
CREATE INDEX IX_Ejemplar_Estado ON Ejemplar(Estado)
CREATE INDEX IX_Ejemplar_CodigoEjemplar ON Ejemplar(CodigoEjemplar)
```

### Tabla HistorialEstadoEjemplar

```sql
CREATE TABLE HistorialEstadoEjemplar (
    IdHistorial INT IDENTITY(1,1) PRIMARY KEY,
    IdEjemplar UNIQUEIDENTIFIER NOT NULL,
    EstadoAnterior NVARCHAR(20) NOT NULL,
    EstadoNuevo NVARCHAR(20) NOT NULL,
    IdUsuario UNIQUEIDENTIFIER NULL,
    Motivo NVARCHAR(500) NULL,
    TipoCambio NVARCHAR(20) NOT NULL, -- 'Manual' o 'Automatico'
    FechaCambio DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_HistorialEstado_Ejemplar FOREIGN KEY (IdEjemplar)
        REFERENCES Ejemplar(IdEjemplar)
)

-- Índice para consultas de historial
CREATE INDEX IX_HistorialEstado_IdEjemplar ON HistorialEstadoEjemplar(IdEjemplar)
CREATE INDEX IX_HistorialEstado_FechaCambio ON HistorialEstadoEjemplar(FechaCambio DESC)
```

---

## Enum EstadoMaterial

```csharp
public enum EstadoMaterial
{
    Disponible,      // Puede ser prestado
    Prestado,        // Actualmente en préstamo
    EnReparacion,    // En mantenimiento
    NoDisponible     // Fuera de circulación
}
```

**Ubicación:** `Model/DomainModel/Enums/EstadoMaterial.cs`

---

## Resumen de Permisos

| Caso de Uso | Administrador | Bibliotecario | Docente |
|-------------|---------------|---------------|---------|
| CU-06.1 Agregar Ejemplar | ✓ | ✓ | ✗ |
| CU-06.2 Editar Ejemplar | ✓ | ✓ | ✗ |
| CU-06.3 Eliminar Ejemplar | ✓ | ✗ | ✗ |
| Consultar Inventario | ✓ | ✓ | ✓ (solo lectura) |
| Cambiar Estado Manual | ✓ | ✓ | ✗ |

---

## Notas de Implementación

### Borrado Lógico vs Físico

El sistema usa **borrado lógico** (Activo = false) en lugar de DELETE por:

✅ **Preserva historial:** Mantiene registros de préstamos pasados
✅ **Integridad referencial:** No rompe relaciones con Prestamo
✅ **Auditoría:** Permite rastrear ejemplares eliminados
✅ **Recuperación:** Posible reactivar si fue error
✅ **Estadísticas:** Mantiene datos históricos precisos

### Actualización Automática de Cantidades

El sistema **NO requiere** actualización manual de cantidades:

- `CantidadTotal` se calcula contando ejemplares activos
- `CantidadDisponible` se calcula contando ejemplares activos en estado Disponible
- Se actualiza automáticamente después de cada operación CRUD

### Historial de Estados

Cada cambio de estado se registra para:

- Auditoría completa de movimientos
- Identificar quién y cuándo cambió el estado
- Diferenciar cambios manuales vs automáticos
- Analizar patrones de uso y daños

---

**Última actualización:** 16 de Noviembre de 2025
**Versión:** 1.0
**Framework:** .NET Framework 4.7.2
**Patrón:** Repository Pattern + Unit of Work
