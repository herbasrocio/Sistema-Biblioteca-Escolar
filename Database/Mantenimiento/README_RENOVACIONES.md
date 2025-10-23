# Módulo de Renovación de Préstamos

## Descripción

Este módulo permite a los usuarios del sistema renovar préstamos activos, extendiendo la fecha de devolución sin necesidad de devolver físicamente el material y registrar un nuevo préstamo.

## Fecha de Implementación

**2025-10-22**

---

## Componentes Implementados

### 1. Base de Datos

#### Tabla Prestamo - Nuevos Campos
- **CantidadRenovaciones** (INT, DEFAULT 0): Contador de cuántas veces se ha renovado el préstamo
- **FechaUltimaRenovacion** (DATETIME, NULL): Fecha de la última renovación realizada

#### Nueva Tabla: RenovacionPrestamo
Tabla de auditoría que registra cada renovación realizada.

**Campos:**
- `IdRenovacion` (UNIQUEIDENTIFIER, PK): Identificador único
- `IdPrestamo` (UNIQUEIDENTIFIER, FK): Referencia al préstamo renovado
- `FechaRenovacion` (DATETIME): Fecha y hora de la renovación
- `FechaDevolucionAnterior` (DATETIME): Fecha de devolución antes de renovar
- `FechaDevolucionNueva` (DATETIME): Nueva fecha de devolución
- `IdUsuario` (UNIQUEIDENTIFIER): Usuario que procesó la renovación
- `Observaciones` (NVARCHAR(500), NULL): Comentarios opcionales

**Índices:**
- `IX_RenovacionPrestamo_IdPrestamo`: Para consultas por préstamo
- `IX_RenovacionPrestamo_FechaRenovacion`: Para reportes por fecha

### 2. Capa de Dominio (Model)

#### Entidades
- **Model/DomainModel/Prestamo.cs**: Agregadas propiedades `CantidadRenovaciones` y `FechaUltimaRenovacion`
- **Model/DomainModel/RenovacionPrestamo.cs**: Nueva entidad para auditoría

#### Adapters
- **Model/DAL/Tools/PrestamoAdapter.cs**: Actualizado para mapear campos de renovación
- **Model/DAL/Tools/RenovacionPrestamoAdapter.cs**: Nuevo adapter para RenovacionPrestamo

#### Repositories
- **Model/DAL/Contracts/IPrestamoRepository.cs**:
  - `RenovarPrestamo(...)`: Método para renovar un préstamo
  - `ObtenerRenovacionesPorPrestamo(...)`: Consultar historial de renovaciones

- **Model/DAL/Implementations/PrestamoRepository.cs**:
  - Implementación de métodos de renovación con transacciones SQL

#### Business Logic Layer
- **Model/BLL/PrestamoBLL.cs**:
  - `RenovarPrestamo(...)`: Lógica de negocio con validaciones completas
  - `ObtenerHistorialRenovaciones(...)`: Obtener historial de auditoría

### 3. Interfaz de Usuario (View)

#### Formularios
- **View/UI/WinUi/Transacciones/renovarPrestamo.cs**: Formulario principal
- **View/UI/WinUi/Transacciones/renovarPrestamo.Designer.cs**: Diseñador de formulario

**Características del formulario:**
- Búsqueda en tiempo real por alumno, título o código de ejemplar
- Visualización de préstamos activos con indicadores de estado
- Selector de días de extensión (1-60 días)
- Cálculo automático de nueva fecha de devolución
- Contador de renovaciones realizadas
- Campo de observaciones opcional
- Validación de límites y restricciones

### 4. Traducciones (i18n)

Agregadas las siguientes claves en español (es-AR) e inglés (en-GB):
- `renovar_prestamo`
- `renovaciones`, `renovaciones_realizadas`
- `dias_extension`, `nueva_fecha_devolucion`
- `fecha_devolucion_actual`, `datos_renovacion`
- `confirmar_renovacion`, `renovacion_exitosa`
- `error_renovar_prestamo`, `prestamo_vencido`
- `dias_atraso`, `seleccionar_prestamo`
- `permiso_renovar_prestamo`

### 5. Seguridad y Permisos

#### Nueva Patente
- **FormName**: `renovarPrestamo`
- **MenuItemName**: Renovar Préstamo
- **Descripción**: Permite renovar préstamos activos extendiendo la fecha de devolución

**Roles con acceso:**
- ROL_Administrador
- ROL_Bibliotecario

---

## Reglas de Negocio

### Validaciones Implementadas

1. **Estado del Préstamo**
   - Solo se pueden renovar préstamos con estado "Activo" o "Atrasado"
   - No se pueden renovar préstamos "Devueltos" o "Cancelados"

2. **Límite de Renovaciones**
   - Máximo **2 renovaciones** por préstamo (configurable)
   - El sistema muestra claramente cuántas renovaciones quedan disponibles

3. **Préstamos Atrasados**
   - Se permite renovar préstamos atrasados hasta **7 días** de atraso (configurable)
   - Si el atraso supera el límite, se debe devolver el material primero

4. **Validación del Alumno**
   - El alumno no debe tener otros préstamos con más de 7 días de atraso
   - Si los tiene, debe devolverlos antes de renovar

5. **Estado del Ejemplar**
   - El ejemplar debe seguir existiendo en el sistema
   - No puede estar en estado "Perdido" o "Mantenimiento"

6. **Cálculo de Nueva Fecha**
   - La nueva fecha se calcula desde **HOY**, no desde la fecha original
   - Ejemplo: Si hoy es 22/10/2025 y extiendo 14 días → nueva fecha: 05/11/2025
   - Rango permitido: 1 a 60 días de extensión

7. **Actualización de Estado**
   - Si un préstamo estaba "Atrasado" y se renueva, pasa automáticamente a "Activo"

### Flujo de Renovación

```
1. Usuario busca préstamo (por alumno, material o código)
2. Selecciona un préstamo activo
3. Sistema muestra:
   - Datos del préstamo
   - Renovaciones realizadas / límite
   - Advertencias si está vencido
4. Usuario elige días de extensión
5. Sistema calcula nueva fecha de devolución
6. Usuario confirma renovación
7. Sistema ejecuta:
   - Actualiza Prestamo.FechaDevolucionPrevista
   - Incrementa Prestamo.CantidadRenovaciones
   - Registra Prestamo.FechaUltimaRenovacion
   - Inserta registro en RenovacionPrestamo
   - Cambia Estado a "Activo" si estaba "Atrasado"
8. Operación completada (transacción atómica)
```

---

## Instalación

### Opción A: Script Maestro (Recomendado)

```bash
cd "Database/Mantenimiento"
sqlcmd -S localhost -E -i "00_EJECUTAR_RENOVACIONES.sql"
```

Este script ejecuta automáticamente:
1. Agregar campos a tabla Prestamo (NegocioBiblioteca)
2. Crear tabla RenovacionPrestamo (NegocioBiblioteca)
3. Agregar patente al sistema de permisos (SeguridadBiblioteca)

### Opción B: Scripts Individuales

```bash
# 1. Agregar campos de renovación
sqlcmd -S localhost -E -i "01_AgregarCamposRenovacion.sql"

# 2. Crear tabla de auditoría
sqlcmd -S localhost -E -i "02_CrearTablaRenovacion.sql"

# 3. Agregar patente
sqlcmd -S localhost -E -i "03_AgregarPatenteRenovacion.sql"
```

### Verificación de Instalación

```sql
-- Verificar campos en Prestamo
USE NegocioBiblioteca;
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Prestamo'
  AND COLUMN_NAME IN ('CantidadRenovaciones', 'FechaUltimaRenovacion');

-- Verificar tabla RenovacionPrestamo
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RenovacionPrestamo';

-- Verificar patente
USE SeguridadBiblioteca;
SELECT * FROM Patente WHERE FormName = 'renovarPrestamo';
```

---

## Uso del Sistema

### Para Usuarios Finales

1. **Acceder al módulo:**
   - Menú → Transacciones → Renovar Préstamo
   - Requiere permiso `renovarPrestamo`

2. **Buscar préstamo:**
   - Escribir nombre del alumno, título del material o código de ejemplar
   - La búsqueda es en tiempo real (500ms de delay)

3. **Seleccionar y renovar:**
   - Hacer clic en la fila del préstamo deseado
   - Ajustar días de extensión (por defecto: 14 días)
   - Agregar observaciones si es necesario
   - Presionar "Renovar Préstamo"

4. **Indicadores visuales:**
   - 🔴 Rojo: Préstamos vencidos
   - 🟡 Amarillo: Préstamos próximos a vencer (≤2 días)
   - ⚠️ Negrita roja: Préstamos con máximo de renovaciones alcanzado

### Para Administradores

#### Configurar Límites

Los límites se configuran en `PrestamoBLL.RenovarPrestamo()`:

```csharp
public void RenovarPrestamo(
    Guid idPrestamo,
    int diasExtension,
    Guid idUsuario,
    int maxRenovaciones = 2,      // Cambiar aquí
    int maxDiasAtraso = 7,         // Cambiar aquí
    string observaciones = null)
```

También en el formulario `renovarPrestamo.cs`:

```csharp
private const int MAX_RENOVACIONES = 2;
private const int MAX_DIAS_ATRASO = 7;
private const int DIAS_EXTENSION_DEFAULT = 14;
```

#### Consultar Auditoría

```sql
-- Historial de renovaciones de un préstamo específico
SELECT * FROM RenovacionPrestamo
WHERE IdPrestamo = '...'
ORDER BY FechaRenovacion DESC;

-- Renovaciones realizadas por un usuario
SELECT
    r.FechaRenovacion,
    u.NombreUsuario,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    m.Titulo AS Material,
    r.FechaDevolucionAnterior,
    r.FechaDevolucionNueva,
    r.Observaciones
FROM RenovacionPrestamo r
INNER JOIN SeguridadBiblioteca.dbo.Usuario u ON r.IdUsuario = u.IdUsuario
INNER JOIN Prestamo p ON r.IdPrestamo = p.IdPrestamo
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
WHERE r.IdUsuario = '...'
ORDER BY r.FechaRenovacion DESC;

-- Préstamos con más renovaciones
SELECT
    p.IdPrestamo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    m.Titulo AS Material,
    p.CantidadRenovaciones,
    p.FechaUltimaRenovacion,
    p.FechaDevolucionPrevista
FROM Prestamo p
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
WHERE p.CantidadRenovaciones > 0
ORDER BY p.CantidadRenovaciones DESC, p.FechaUltimaRenovacion DESC;
```

---

## Mantenimiento

### Problemas Comunes

**1. Error: "Este préstamo ya alcanzó el límite máximo de X renovaciones"**
- **Causa**: El préstamo ha sido renovado el máximo de veces permitido
- **Solución**: El alumno debe devolver el material y registrar un nuevo préstamo si lo necesita

**2. Error: "Este préstamo tiene X días de atraso. No se puede renovar préstamos con más de Y días de atraso"**
- **Causa**: El préstamo está muy atrasado
- **Solución**: Registrar la devolución del material primero

**3. Error: "El alumno [nombre] tiene otros préstamos con más de X días de atraso"**
- **Causa**: El alumno tiene préstamos pendientes muy atrasados
- **Solución**: Devolver los préstamos atrasados antes de renovar

**4. Error: "No se puede renovar un préstamo de un ejemplar reportado como perdido"**
- **Causa**: El ejemplar está marcado como perdido en el sistema
- **Solución**: Verificar el estado real del ejemplar y actualizar en Gestionar Ejemplares

### Consultas Útiles

```sql
-- Ver préstamos renovables (estado activo, menos de 2 renovaciones)
SELECT
    p.IdPrestamo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    m.Titulo AS Material,
    p.FechaDevolucionPrevista,
    p.CantidadRenovaciones,
    CASE
        WHEN p.FechaDevolucionPrevista < GETDATE() THEN 'Vencido'
        ELSE 'Vigente'
    END AS Estado
FROM Prestamo p
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
WHERE p.Estado = 'Activo'
  AND p.CantidadRenovaciones < 2
ORDER BY p.FechaDevolucionPrevista;

-- Resetear contador de renovaciones (solo en desarrollo/testing)
-- ¡NO USAR EN PRODUCCIÓN sin autorización!
UPDATE Prestamo
SET CantidadRenovaciones = 0,
    FechaUltimaRenovacion = NULL
WHERE IdPrestamo = '...';
```

---

## Arquitectura Técnica

### Transacciones SQL

La operación de renovación usa transacciones para garantizar consistencia:

```csharp
using (SqlTransaction transaction = conn.BeginTransaction())
{
    try
    {
        // 1. Leer fecha actual
        // 2. Actualizar Prestamo
        // 3. Insertar en RenovacionPrestamo
        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}
```

### Patrón Repository

- **IPrestamoRepository**: Contrato con métodos de renovación
- **PrestamoRepository**: Implementación con ADO.NET (sin ORM)
- **PrestamoBLL**: Orquestación y validaciones de negocio

### Patrón Adapter

- **PrestamoAdapter**: Mapea `DataRow` → `Prestamo` (incluye campos de renovación)
- **RenovacionPrestamoAdapter**: Mapea `DataRow` → `RenovacionPrestamo`

---

## Futuros Mejoramientos

### Ideas Adicionales

1. **Notificaciones automáticas**: Enviar email/SMS cuando quedan pocos días de renovación
2. **Restricción por demanda**: No permitir renovaciones si hay otros alumnos esperando el mismo material
3. **Renovación online**: Portal web para que alumnos renueven sus propios préstamos
4. **Renovación automática**: Sistema que renueva automáticamente si no hay demanda
5. **Reportes avanzados**: Dashboard con métricas de renovaciones por mes, material, etc.
6. **Políticas por tipo de material**: Límites diferentes según si es libro, revista, etc.
7. **Sanciones por uso excesivo**: Restricción temporal si un alumno renueva demasiado

---

## Contacto y Soporte

Para reportar problemas o sugerencias relacionadas con el módulo de renovaciones, contactar al equipo de desarrollo del Sistema Biblioteca Escolar.

**Versión del módulo:** 1.0
**Fecha de creación:** 2025-10-22
**Última actualización:** 2025-10-22
