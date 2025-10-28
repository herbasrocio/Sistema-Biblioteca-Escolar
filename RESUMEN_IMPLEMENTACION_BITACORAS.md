# Sistema de Bitácoras - Implementación Completa

## Fecha: 2025-10-28
## Autor: Sistema Biblioteca Escolar

---

## 1. RESUMEN EJECUTIVO

Se ha implementado un **sistema de bitácoras dual** para el Sistema Biblioteca Escolar, dividido en dos módulos independientes con diferentes niveles de acceso:

### Bitácoras Implementadas:

1. **BitacoraAdmin** (Solo Administradores)
   - Base de datos: `SeguridadBiblioteca`
   - Registra: Errores del sistema, eventos de seguridad, cambios críticos
   - Acceso: Solo usuarios con permiso `consultarBitacoraAdmin`

2. **BitacoraBibliotecario** (Bibliotecarios y Administradores)
   - Base de datos: `NegocioBiblioteca`
   - Registra: Operaciones de negocio (préstamos, devoluciones, consultas, etc.)
   - Acceso: Usuarios con permiso `consultarBitacoraBibliotecario`

---

## 2. ARQUITECTURA IMPLEMENTADA

### 2.1 Estructura de Capas

```
View (UI Layer)
  └── WinForms (pendiente implementación)
       ├── ConsultarBitacoraAdmin.cs
       └── ConsultarBitacoraBibliotecario.cs

Model (Business Layer)
  ├── BLL/
  │    ├── BitacoraAdminBLL.cs ✅
  │    └── BitacoraBibliotecarioBLL.cs ✅
  │
  ├── DAL/
  │    ├── Contracts/
  │    │    ├── IBitacoraAdminRepository.cs ✅
  │    │    └── IBitacoraBibliotecarioRepository.cs ✅
  │    │
  │    └── Implementations/
  │         ├── BitacoraAdminRepository.cs ✅
  │         └── BitacoraBibliotecarioRepository.cs ✅
  │
  └── DomainModel/
       ├── BitacoraAdmin.cs ✅
       └── BitacoraBibliotecario.cs ✅

Database (SQL Scripts)
  ├── Database/
  │    ├── 07_CrearTablaBitacoraAdmin.sql ✅
  │    └── 08_AgregarPermisosBitacora.sql ✅
  │
  └── Database/Negocio/
       └── 07_CrearTablaBitacoraBibliotecario.sql ✅
```

---

## 3. BASE DE DATOS

### 3.1 Tabla: BitacoraAdmin (SeguridadBiblioteca)

```sql
CREATE TABLE [dbo].[BitacoraAdmin]
(
    [IdBitacora] INT IDENTITY(1,1) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE(),
    [IdUsuario] UNIQUEIDENTIFIER NULL,  -- FK a Usuario.IdUsuario
    [NombreUsuario] NVARCHAR(100) NULL,
    [TipoEvento] NVARCHAR(50) NOT NULL, -- 'Error', 'Seguridad', 'CambioCritico'
    [Modulo] NVARCHAR(100) NOT NULL,
    [Accion] NVARCHAR(255) NOT NULL,
    [Detalle] NVARCHAR(MAX) NULL,
    [Criticidad] NVARCHAR(20) NOT NULL DEFAULT 'Media', -- 'Baja', 'Media', 'Alta', 'Critica'
    [DireccionIP] NVARCHAR(45) NULL,

    CONSTRAINT [PK_BitacoraAdmin] PRIMARY KEY ([IdBitacora]),
    CONSTRAINT [FK_BitacoraAdmin_Usuario] FOREIGN KEY([IdUsuario])
        REFERENCES [dbo].[Usuario] ([IdUsuario]) ON DELETE SET NULL
);
```

**Índices creados:**
- `IX_BitacoraAdmin_Fecha` (DESC)
- `IX_BitacoraAdmin_TipoEvento`
- `IX_BitacoraAdmin_IdUsuario`

### 3.2 Tabla: BitacoraBibliotecario (NegocioBiblioteca)

```sql
CREATE TABLE [dbo].[BitacoraBibliotecario]
(
    [IdBitacora] INT IDENTITY(1,1) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE(),
    [IdUsuario] UNIQUEIDENTIFIER NULL,  -- Referencia externa a SeguridadBiblioteca.Usuario
    [NombreUsuario] NVARCHAR(100) NULL,
    [TipoOperacion] NVARCHAR(50) NOT NULL, -- 'Prestamo', 'Devolucion', 'Renovacion', etc.
    [Modulo] NVARCHAR(100) NOT NULL,
    [Accion] NVARCHAR(255) NOT NULL,
    [EntidadAfectada] NVARCHAR(50) NULL, -- 'Material', 'Ejemplar', 'Alumno', 'Prestamo', etc.
    [IdEntidad] INT NULL,
    [Detalle] NVARCHAR(MAX) NULL,

    CONSTRAINT [PK_BitacoraBibliotecario] PRIMARY KEY ([IdBitacora])
);
```

**Índices creados:**
- `IX_BitacoraBibliotecario_Fecha` (DESC)
- `IX_BitacoraBibliotecario_TipoOperacion`
- `IX_BitacoraBibliotecario_IdUsuario`
- `IX_BitacoraBibliotecario_EntidadAfectada` (con IdEntidad)

### 3.3 Stored Procedures

#### sp_RegistrarBitacoraAdmin
```sql
CREATE PROCEDURE [dbo].[sp_RegistrarBitacoraAdmin]
    @IdUsuario UNIQUEIDENTIFIER = NULL,
    @NombreUsuario NVARCHAR(100) = NULL,
    @TipoEvento NVARCHAR(50),
    @Modulo NVARCHAR(100),
    @Accion NVARCHAR(255),
    @Detalle NVARCHAR(MAX) = NULL,
    @Criticidad NVARCHAR(20) = 'Media',
    @DireccionIP NVARCHAR(45) = NULL
AS
BEGIN
    INSERT INTO [dbo].[BitacoraAdmin] ...
    RETURN SCOPE_IDENTITY();
END
```

#### sp_RegistrarBitacoraBibliotecario
```sql
CREATE PROCEDURE [dbo].[sp_RegistrarBitacoraBibliotecario]
    @IdUsuario UNIQUEIDENTIFIER = NULL,
    @NombreUsuario NVARCHAR(100) = NULL,
    @TipoOperacion NVARCHAR(50),
    @Modulo NVARCHAR(100),
    @Accion NVARCHAR(255),
    @EntidadAfectada NVARCHAR(50) = NULL,
    @IdEntidad INT = NULL,
    @Detalle NVARCHAR(MAX) = NULL
AS
BEGIN
    INSERT INTO [dbo].[BitacoraBibliotecario] ...
    RETURN SCOPE_IDENTITY();
END
```

---

## 4. PERMISOS CONFIGURADOS

### 4.1 Patentes Creadas

| FormName | MenuItemName | Descripción | Asignado a |
|----------|--------------|-------------|------------|
| `consultarBitacoraAdmin` | Consultar Bitácora Admin | Permite consultar la bitácora de administrador | ROL_Administrador |
| `consultarBitacoraBibliotecario` | Consultar Bitácora Bibliotecario | Permite consultar la bitácora de bibliotecarios | ROL_Administrador, ROL_Bibliotecario |

### 4.2 Asignaciones de Permisos

```sql
-- ROL_Administrador tiene acceso a AMBAS bitácoras
FamiliaPatente (IdFamilia=ROL_Administrador, IdPatente=consultarBitacoraAdmin) ✅
FamiliaPatente (IdFamilia=ROL_Administrador, IdPatente=consultarBitacoraBibliotecario) ✅

-- ROL_Bibliotecario solo tiene acceso a bitácora de bibliotecarios
FamiliaPatente (IdFamilia=ROL_Bibliotecario, IdPatente=consultarBitacoraBibliotecario) ✅
```

---

## 5. CAPA DE LÓGICA DE NEGOCIO (BLL)

### 5.1 BitacoraAdminBLL - Métodos Principales

#### Métodos de Registro:

```csharp
// Registrar un error del sistema
void RegistrarError(string modulo, string accion, string detalle,
    Guid? idUsuario = null, string nombreUsuario = null, string criticidad = "Alta")

// Registrar un evento de seguridad
void RegistrarEventoSeguridad(string modulo, string accion, string detalle,
    Guid? idUsuario = null, string nombreUsuario = null, string criticidad = "Alta",
    string direccionIP = null)

// Registrar un cambio crítico
void RegistrarCambioCritico(string modulo, string accion, string detalle,
    Guid? idUsuario = null, string nombreUsuario = null, string criticidad = "Critica")

// Registrar evento genérico
void RegistrarEvento(BitacoraAdmin registro)
```

#### Métodos de Consulta:

```csharp
List<BitacoraAdmin> ObtenerTodos()
List<BitacoraAdmin> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
List<BitacoraAdmin> ObtenerPorTipoEvento(string tipoEvento)
List<BitacoraAdmin> ObtenerPorUsuario(Guid idUsuario)
List<BitacoraAdmin> ObtenerPorModulo(string modulo)
List<BitacoraAdmin> ObtenerPorCriticidad(string criticidad)
List<BitacoraAdmin> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
    string tipoEvento = null, Guid? idUsuario = null, string modulo = null, string criticidad = null)
```

### 5.2 BitacoraBibliotecarioBLL - Métodos Principales

#### Métodos de Registro:

```csharp
// Registrar un préstamo
void RegistrarPrestamo(int idPrestamo, Guid idUsuario, string nombreUsuario, string detalle = null)

// Registrar una devolución
void RegistrarDevolucion(int idDevolucion, Guid idUsuario, string nombreUsuario, string detalle = null)

// Registrar una renovación
void RegistrarRenovacion(int idPrestamo, Guid idUsuario, string nombreUsuario, string detalle = null)

// Registrar operación de material
void RegistrarOperacionMaterial(string accion, int? idMaterial, Guid? idUsuario,
    string nombreUsuario, string detalle = null)

// Registrar operación de alumno
void RegistrarOperacionAlumno(string accion, int? idAlumno, Guid? idUsuario,
    string nombreUsuario, string detalle = null)

// Registrar consulta de material
void RegistrarConsultaMaterial(Guid? idUsuario, string nombreUsuario, string detalle = null)

// Registrar operación genérica
void RegistrarOperacion(BitacoraBibliotecario registro)
```

#### Métodos de Consulta:

```csharp
List<BitacoraBibliotecario> ObtenerTodos()
List<BitacoraBibliotecario> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
List<BitacoraBibliotecario> ObtenerPorTipoOperacion(string tipoOperacion)
List<BitacoraBibliotecario> ObtenerPorUsuario(Guid idUsuario)
List<BitacoraBibliotecario> ObtenerPorModulo(string modulo)
List<BitacoraBibliotecario> ObtenerPorEntidad(string entidadAfectada, int? idEntidad = null)
List<BitacoraBibliotecario> ObtenerConFiltros(DateTime? fechaInicio = null, DateTime? fechaFin = null,
    string tipoOperacion = null, Guid? idUsuario = null, string modulo = null, string entidadAfectada = null)
```

---

## 6. EJEMPLOS DE USO

### 6.1 Registrar un Error de Sistema (BitacoraAdmin)

```csharp
using BLL;
using ServicesSecurity.Services;

// En un bloque catch de tu código
try
{
    // Operación que puede fallar
}
catch (Exception ex)
{
    var bitacoraAdminBLL = new BitacoraAdminBLL();
    var usuarioActual = SessionManager.GetInstance().UsuarioActual;

    bitacoraAdminBLL.RegistrarError(
        modulo: "Módulo Préstamos",
        accion: "Error al registrar préstamo",
        detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}",
        idUsuario: usuarioActual?.IdUsuario,
        nombreUsuario: usuarioActual?.Nombre,
        criticidad: "Alta"
    );
}
```

### 6.2 Registrar un Evento de Seguridad (BitacoraAdmin)

```csharp
// En el proceso de login
var bitacoraAdminBLL = new BitacoraAdminBLL();

bitacoraAdminBLL.RegistrarEventoSeguridad(
    modulo: "Sistema de Autenticación",
    accion: "Intento de login fallido",
    detalle: $"Usuario '{nombreUsuario}' intentó ingresar con contraseña incorrecta",
    idUsuario: null,  // No hay usuario autenticado todavía
    nombreUsuario: nombreUsuario,
    criticidad: "Media",
    direccionIP: "192.168.1.100"  // Obtener IP del cliente
);
```

### 6.3 Registrar un Cambio Crítico (BitacoraAdmin)

```csharp
// Al eliminar un usuario
var bitacoraAdminBLL = new BitacoraAdminBLL();
var usuarioActual = SessionManager.GetInstance().UsuarioActual;

bitacoraAdminBLL.RegistrarCambioCritico(
    modulo: "Gestión de Usuarios",
    accion: "Eliminar usuario",
    detalle: $"Usuario eliminado: {usuarioEliminado.Nombre} (ID: {usuarioEliminado.IdUsuario})",
    idUsuario: usuarioActual.IdUsuario,
    nombreUsuario: usuarioActual.Nombre,
    criticidad: "Critica"
);
```

### 6.4 Registrar un Préstamo (BitacoraBibliotecario)

```csharp
// En PrestamoBLL.RegistrarPrestamo(), después de guardar el préstamo
var bitacoraBibliotecarioBLL = new BitacoraBibliotecarioBLL();
var usuarioActual = SessionManager.GetInstance().UsuarioActual;

bitacoraBibliotecarioBLL.RegistrarPrestamo(
    idPrestamo: prestamo.IdPrestamo,
    idUsuario: usuarioActual.IdUsuario,
    nombreUsuario: usuarioActual.Nombre,
    detalle: $"Préstamo de ejemplar '{ejemplar.CodigoBarras}' a alumno '{alumno.Nombre} {alumno.Apellido}'"
);
```

### 6.5 Registrar una Devolución (BitacoraBibliotecario)

```csharp
// En DevolucionBLL.RegistrarDevolucion(), después de guardar la devolución
var bitacoraBibliotecarioBLL = new BitacoraBibliotecarioBLL();
var usuarioActual = SessionManager.GetInstance().UsuarioActual;

bitacoraBibliotecarioBLL.RegistrarDevolucion(
    idDevolucion: devolucion.IdDevolucion,
    idUsuario: usuarioActual.IdUsuario,
    nombreUsuario: usuarioActual.Nombre,
    detalle: $"Devolución de ejemplar '{ejemplar.CodigoBarras}' por alumno '{alumno.Nombre} {alumno.Apellido}'. Estado: {devolucion.EstadoDevolucion}"
);
```

### 6.6 Consultar Bitácora con Filtros

```csharp
// Consultar errores de los últimos 7 días con criticidad Alta
var bitacoraAdminBLL = new BitacoraAdminBLL();

var registros = bitacoraAdminBLL.ObtenerConFiltros(
    fechaInicio: DateTime.Now.AddDays(-7),
    fechaFin: DateTime.Now,
    tipoEvento: "Error",
    criticidad: "Alta"
);

// Consultar préstamos del último mes
var bitacoraBibliotecarioBLL = new BitacoraBibliotecarioBLL();

var operaciones = bitacoraBibliotecarioBLL.ObtenerConFiltros(
    fechaInicio: DateTime.Now.AddMonths(-1),
    fechaFin: DateTime.Now,
    tipoOperacion: "Prestamo"
);
```

---

## 7. PENDIENTES DE IMPLEMENTACIÓN

### 7.1 Interfaz de Usuario (WinForms) - PENDIENTE

Se deben crear dos formularios:

#### ConsultarBitacoraAdmin.cs
- DataGridView para mostrar registros
- Filtros por: Fecha, TipoEvento, Usuario, Módulo, Criticidad
- Botones: Filtrar, Limpiar, Exportar
- Validación de permisos: `consultarBitacoraAdmin`

#### ConsultarBitacoraBibliotecario.cs
- DataGridView para mostrar registros
- Filtros por: Fecha, TipoOperacion, Usuario, Módulo, EntidadAfectada
- Botones: Filtrar, Limpiar, Exportar
- Validación de permisos: `consultarBitacoraBibliotecario`

### 7.2 Integración Automática en Módulos Existentes - PENDIENTE

Se debe agregar logging automático en:

**BitacoraAdmin:**
- Login exitoso/fallido (`Security/ServicesSeguridad/Services/LoginService.cs`)
- Cambios en permisos de usuarios
- Eliminación de usuarios
- Try-catch globales en operaciones críticas

**BitacoraBibliotecario:**
- PrestamoBLL.RegistrarPrestamo()
- DevolucionBLL.RegistrarDevolucion()
- PrestamoBLL.RenovarPrestamo()
- MaterialBLL.GuardarMaterial()
- MaterialBLL.ActualizarMaterial()
- MaterialBLL.EliminarMaterial()
- AlumnoBLL.GuardarAlumno()
- AlumnoBLL.ActualizarAlumno()

### 7.3 Traducciones (i18n) - PENDIENTE

Agregar claves en `idioma.es-AR` y `idioma.en-GB`:

```
# Bitácoras - Admin
bitacora_admin_titulo=Consultar Bitácora de Administrador
bitacora_admin_tipo_evento=Tipo de Evento
bitacora_admin_criticidad=Criticidad
bitacora_admin_detalle=Detalle
bitacora_admin_direccion_ip=Dirección IP

# Bitácoras - Bibliotecario
bitacora_bibliotecario_titulo=Consultar Bitácora de Bibliotecario
bitacora_bibliotecario_tipo_operacion=Tipo de Operación
bitacora_bibliotecario_entidad_afectada=Entidad Afectada
bitacora_bibliotecario_id_entidad=ID Entidad

# Comunes
bitacora_fecha=Fecha
bitacora_usuario=Usuario
bitacora_modulo=Módulo
bitacora_accion=Acción
bitacora_filtrar=Filtrar
bitacora_limpiar=Limpiar Filtros
bitacora_exportar=Exportar a CSV
bitacora_fecha_inicio=Fecha Inicio
bitacora_fecha_fin=Fecha Fin
```

---

## 8. SCRIPTS SQL DE INSTALACIÓN

### Orden de Ejecución:

1. **Crear tabla BitacoraAdmin:**
   ```powershell
   sqlcmd -S localhost -E -i "Database\07_CrearTablaBitacoraAdmin.sql"
   ```

2. **Crear tabla BitacoraBibliotecario:**
   ```powershell
   sqlcmd -S localhost -E -i "Database\Negocio\07_CrearTablaBitacoraBibliotecario.sql"
   ```

3. **Agregar permisos:**
   ```powershell
   sqlcmd -S localhost -E -i "Database\08_AgregarPermisosBitacora.sql"
   ```

   **Nota:** Los permisos ya fueron asignados manualmente a las familias `ROL_Administrador` y `ROL_Bibliotecario`.

---

## 9. CONSIDERACIONES TÉCNICAS

### 9.1 Tipos de Datos

**IMPORTANTE:** El sistema usa `UNIQUEIDENTIFIER` (GUID) para `IdUsuario`, no `INT`.

- `BitacoraAdmin.IdUsuario`: `Guid?`
- `BitacoraBibliotecario.IdUsuario`: `Guid?`
- Todas las interfaces, repositorios y BLL usan `Guid` para IdUsuario

### 9.2 Performance

**Índices optimizados:**
- Índice en `Fecha DESC` para consultas recientes rápidas
- Índices en `TipoEvento`/`TipoOperacion` para filtros por tipo
- Índices en `IdUsuario` para filtros por usuario
- Índice compuesto en `EntidadAfectada` + `IdEntidad` para trazabilidad

**Consultas con filtros múltiples:**
- Método `ObtenerConFiltros()` construye queries dinámicamente
- Solo agrega condiciones WHERE cuando los parámetros no son nulos
- Evita cargar datos innecesarios

### 9.3 Seguridad

**Separación de responsabilidades:**
- BitacoraAdmin: Solo administradores (datos sensibles)
- BitacoraBibliotecario: Bibliotecarios y administradores (operaciones cotidianas)

**Validación de permisos:**
```csharp
if (SessionManager.GetInstance().UsuarioActual.TienePermiso("consultarBitacoraAdmin"))
{
    // Mostrar formulario de bitácora admin
}
```

**Integridad referencial:**
- `FK_BitacoraAdmin_Usuario` con `ON DELETE SET NULL`
- Si un usuario es eliminado, sus registros se mantienen pero el ID se anula

---

## 10. RESUMEN DE ARCHIVOS CREADOS

### Scripts SQL (3 archivos):
- `Database\07_CrearTablaBitacoraAdmin.sql` ✅
- `Database\Negocio\07_CrearTablaBitacoraBibliotecario.sql` ✅
- `Database\08_AgregarPermisosBitacora.sql` ✅

### Domain Model (2 archivos):
- `Model\DomainModel\BitacoraAdmin.cs` ✅
- `Model\DomainModel\BitacoraBibliotecario.cs` ✅

### DAL Contracts (2 archivos):
- `Model\DAL\Contracts\IBitacoraAdminRepository.cs` ✅
- `Model\DAL\Contracts\IBitacoraBibliotecarioRepository.cs` ✅

### DAL Implementations (2 archivos):
- `Model\DAL\Implementations\BitacoraAdminRepository.cs` ✅
- `Model\DAL\Implementations\BitacoraBibliotecarioRepository.cs` ✅

### BLL (2 archivos):
- `Model\BLL\BitacoraAdminBLL.cs` ✅
- `Model\BLL\BitacoraBibliotecarioBLL.cs` ✅

### Documentación (1 archivo):
- `RESUMEN_IMPLEMENTACION_BITACORAS.md` ✅ (este archivo)

**Total: 14 archivos creados**

---

## 11. PRÓXIMOS PASOS

1. ✅ **Estructura de base de datos** - COMPLETADO
2. ✅ **Entidades de dominio** - COMPLETADO
3. ✅ **Repositorios DAL** - COMPLETADO
4. ✅ **Servicios BLL** - COMPLETADO
5. ✅ **Permisos en base de datos** - COMPLETADO
6. ⏳ **Formularios WinForms** - PENDIENTE
7. ⏳ **Integración automática en módulos existentes** - PENDIENTE
8. ⏳ **Traducciones i18n** - PENDIENTE

---

## 12. SOPORTE Y CONTACTO

Para consultas o problemas con la implementación del sistema de bitácoras, referirse a:
- Este documento: `RESUMEN_IMPLEMENTACION_BITACORAS.md`
- CLAUDE.md (instrucciones del proyecto)
- IMPLEMENTACION_UNIT_OF_WORK.md (patrón Unit of Work existente)

---

**Generado por: Claude Code**
**Fecha: 2025-10-28**
