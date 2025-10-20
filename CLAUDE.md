# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Sistema Biblioteca Escolar** - A comprehensive school library management system built with C# .NET Framework 4.7.2 and Windows Forms. The system manages library catalog, student enrollment, material loans, returns, and includes a sophisticated role-based permission system.

## Build and Run Commands

### Build Solution
```bash
# Build entire solution
msbuild "Sistema Biblioteca Escolar.sln" /t:Rebuild /p:Configuration=Debug

# Build specific project
msbuild "View\UI\UI.csproj" /t:Rebuild /p:Configuration=Debug
```

### Run Application
```bash
# From Visual Studio: Press F5 or Start Debugging
# From command line:
cd "View\UI\bin\Debug"
UI.exe
```

### Database Setup
```bash
# Full database setup (creates both databases with all data)
sqlcmd -S localhost -E -i "Database\00_EJECUTAR_TODO.sql"

# Or individual steps:
sqlcmd -S localhost -E -i "Database\01_CrearBaseDatos.sql"
sqlcmd -S localhost -E -i "Database\02_CrearTablas.sql"
sqlcmd -S localhost -E -i "Database\03_DatosIniciales.sql"

# Business database setup (inscriptions, materials, etc.)
sqlcmd -S localhost -E -i "Database\Negocio\00_CrearBDNegocio.sql"
```

**Default Admin Credentials:**
- Username: `admin`
- Password: `admin123`

## Architecture Overview

### Three-Tier Layered Architecture

```
View Layer (UI)
    ↓
Model Layer (BLL + DAL + DomainModel)
    ↓
Data Layer (SQL Server)
    ↓
Security Layer (Independent ServicesSecurity)
```

### Project Structure

- **View/UI/** - Windows Forms user interface
  - `WinUi/Administración/` - Admin forms (login, menus, user management, catalog)
  - `WinUi/Transacciones/` - Transaction forms (loans, returns)
  - `Resources/I18n/` - Translation files (Spanish, English)

- **Model/** - Business logic and data access
  - `DomainModel/` - Domain entities (Alumno, Material, Ejemplar, Prestamo, etc.)
  - `BLL/` - Business Logic Layer (validation, orchestration)
  - `DAL/` - Data Access Layer (repositories, adapters)
  - `Services/` - Utility services

- **Security/ServicesSeguridad/** - Authentication and authorization
  - `DomainModel/Security/Composite/` - Composite pattern for permissions
  - `DAL/` - Security repositories
  - `BLL/` - Login service, cryptography

- **Database/** - SQL scripts for setup and migrations

### Two-Database Architecture

The system uses **two separate databases** for security isolation:

1. **SeguridadBiblioteca** - Users, roles, permissions, i18n, audit logs
2. **NegocioBiblioteca** - Library materials, students, loans, returns, inscriptions

Connection strings are configured in `View/UI/App.config`:
- `ServicesConString` → SeguridadBiblioteca
- `NegocioConString` → NegocioBiblioteca

## Key Architectural Patterns

### 1. Repository Pattern (NO ORM - ADO.NET Direct)

All data access uses ADO.NET with parameterized queries (NO Entity Framework or Dapper).

**Generic Repository Interface:**
```csharp
public interface IGenericRepository<T>
{
    void Insert(T entity);
    void Update(T entity);
    void Delete(Guid id);
    T SelectOne(Guid id);
    List<T> SelectAll();
}
```

**Specialized Repositories:**
- `IMaterialRepository` - Catalog operations
- `IPrestamoRepository` - Loan transactions
- `IEjemplarRepository` - Physical copy management
- `IInscripcionRepository` - Student enrollment

Location: `Model/DAL/Contracts/` (interfaces) and `Model/DAL/Implementations/` (concrete)

### 2. Adapter Pattern (DataRow to Domain Object Mapping)

Centralizes SQL-to-Object conversion logic.

**Location:** `Model/DAL/Tools/`

**Example:**
```csharp
public static Material AdaptMaterial(DataRow row)
{
    return new Material
    {
        IdMaterial = (Guid)row["IdMaterial"],
        Titulo = row["Titulo"].ToString(),
        CantidadTotal = Convert.ToInt32(row["CantidadTotal"]),
        // ... more fields
    };
}
```

### 3. Composite Pattern (Hierarchical Permissions)

Enables flexible role-based access control with inheritance.

**Location:** `Security/ServicesSeguridad/DomainModel/Security/Composite/`

**Structure:**
```
Component (abstract base)
├── Patente (leaf) - Individual permission
└── Familia (composite) - Permission group or Role
```

**Roles vs Groups:**
- Families named `ROL_*` → Roles (e.g., ROL_Administrador)
- Other families → Permission groups (e.g., "Gestión Catálogo")

**Permission Hierarchy Example:**
```
ROL_Administrador
├── Gestión Usuarios (Familia)
│   ├── Alta de Usuario (Patente)
│   ├── Modificar Usuario (Patente)
│   └── Ver Usuarios (Patente)
├── Gestión Catálogo (Familia)
│   ├── Consultar Material (Patente)
│   ├── Registrar Material (Patente)
│   └── Editar Material (Patente)
└── More permissions...
```

### 4. Dependency Injection in BLL

Each BLL class has dual constructors for testability:

```csharp
public class MaterialBLL
{
    private readonly IMaterialRepository _repository;

    // For production (UI ease of use)
    public MaterialBLL()
    {
        _repository = new MaterialRepository();
    }

    // For testing (dependency injection)
    public MaterialBLL(IMaterialRepository repository)
    {
        _repository = repository;
    }
}
```

## Domain Model - Key Entities

### Material (Conceptual Catalog Entry)
- **IdMaterial** - GUID primary key
- **Titulo** - Title
- **Autor** - Author
- **Genero** - Genre
- **Nivel** - Academic level
- **CantidadTotal** - Dynamically calculated from Ejemplar count
- **CantidadDisponible** - Dynamically calculated available copies

**Important:** Material quantities are NOT stored, they are calculated in real-time from Ejemplar records.

### Ejemplar (Physical Copy)
- **IdEjemplar** - GUID primary key
- **IdMaterial** - FK to Material
- **CodigoEjemplar** - Barcode/unique identifier
- **Estado** - 0=Disponible, 1=Prestado, 2=Mantenimiento, 3=Perdido
- **Ubicacion** - Physical location in library
- **Activo** - Logical delete flag

**Important:** Ejemplar is the single source of truth for availability. Each physical book/copy is one Ejemplar.

### Prestamo (Loan)
- **IdPrestamo** - GUID primary key
- **IdAlumno** - FK to student
- **IdMaterial** - FK to material (conceptual)
- **IdEjemplar** - FK to specific copy (CRITICAL)
- **FechaPrestamo** - Loan date
- **FechaDevolucionEsperada** - Due date
- **Estado** - 0=Activo, 1=Devuelto, 2=Vencido

**Important:** Prestamo links to BOTH Material (for reporting) and Ejemplar (for state tracking).

### Devolucion (Return)
- **IdDevolucion** - GUID primary key
- **IdPrestamo** - FK to loan
- **FechaDevolucion** - Actual return date
- **Observaciones** - Notes (damage, late fees, etc.)

### Alumno (Student)
- **IdAlumno** - GUID primary key
- **Nombre**, **Apellido** - Name
- **DNI** - National ID
- **Email**, **Telefono** - Contact info

### Inscripcion (Enrollment)
- **IdInscripcion** - GUID primary key
- **IdAlumno** - FK to student
- **AnioLectivo** - Academic year (INT)
- **Grado** - Grade (1-7 or "EGRESADO")
- **Division** - Division (A, B, C, etc.)
- **Estado** - Activo/Finalizado/Abandonado

**Important:** Maintains complete academic history. One student can have multiple inscriptions across different years.

## Security and Permissions

### Authentication Flow

1. User enters credentials in Login form
2. `LoginService.Login(username, password)` is called
3. Password is hashed with SHA256 and compared
4. If valid, user's permission tree is loaded recursively
5. User object with all permissions is returned
6. Session stored in `SessionService.CurrentUser`

### Authorization

**Check Permission:**
```csharp
if (SessionService.CurrentUser.TienePermiso("FormName"))
{
    // Allow access
}
```

**Get User's Role:**
```csharp
string roleName = SessionService.CurrentUser.ObtenerNombreRol();
// Returns: "Administrador", "Veterinario", etc. (without ROL_ prefix)
```

### Permission Checking in UI

Forms check permissions on load:
```csharp
private void FormLoad(object sender, EventArgs e)
{
    if (!SessionService.CurrentUser.TienePermiso("registrarMaterial"))
    {
        MessageBox.Show("No tiene permisos");
        this.Close();
        return;
    }
    // Continue loading...
}
```

Menu items are dynamically shown/hidden based on permissions.

## Internationalization (i18n)

### Translation Files

Located at `View/UI/Resources/I18n/`:
- `idioma.es-AR` - Spanish (Argentina)
- `idioma.en-GB` - English

**Format:** Simple key=value pairs
```
login=Iniciar Sesión
username=Usuario
password=Contraseña
```

### Usage in Code

```csharp
using ServicesSecurity.Services.Contracts;

// Get service
ILanguageService langService = LanguageService.GetInstance();

// Translate
string translated = langService.Translate("login"); // → "Iniciar Sesión"
```

### Language Switching

User can change language in UI, stored in `Usuario.IdiomaPreferido` field.

### Adding New Translations

1. Add key=value to both `idioma.es-AR` and `idioma.en-GB`
2. Use `LanguageService.Translate("your_key")` in code
3. Translations are loaded at runtime, no recompilation needed

## Business Logic Patterns

### Complex Validation in BLL

**ValidationBLL** contains reusable field validators:
- `ValidarDNI(string dni)` - 7-8 digits
- `ValidarEmail(string email)` - Email format
- `ValidarNombre(string nombre)` - Non-empty, reasonable length
- `ValidarAnioLectivo(int anio)` - Valid year range
- `ValidarGrado(string grado)` - 1-7 or EGRESADO

### Transaction Pattern (Loan Registration)

PrestamoBLL orchestrates complex multi-step operations:

1. Validate material exists (`MaterialBLL.ObtenerPorId()`)
2. Validate student exists (`AlumnoBLL.ObtenerPorId()`)
3. Check student has no overdue loans (`VerificarPrestamosVencidos()`)
4. Validate specific exemplar is available (`EjemplarBLL.ObtenerPorId()`)
5. Update exemplar state to "Prestado" (`EjemplarBLL.ActualizarEstado()`)
6. Create loan record (`PrestamoRepository.Insert()`)
7. Recalculate material quantities (automatic)

**Important:** All operations within BLL should use ADO.NET transactions for atomicity.

### Dynamic Calculation Pattern

Material quantities are NEVER stored in database:

```sql
-- CantidadTotal calculation
SELECT COUNT(*) FROM Ejemplar
WHERE IdMaterial = @IdMaterial AND Activo = 1

-- CantidadDisponible calculation
SELECT COUNT(*) FROM Ejemplar
WHERE IdMaterial = @IdMaterial AND Activo = 1 AND Estado = 0
```

This ensures single source of truth and prevents data inconsistencies.

## Common Development Tasks

### Adding a New Entity

1. Create domain class in `Model/DomainModel/YourEntity.cs`
2. Create repository interface in `Model/DAL/Contracts/IYourEntityRepository.cs`
3. Create adapter in `Model/DAL/Tools/YourEntityAdapter.cs`
4. Implement repository in `Model/DAL/Implementations/YourEntityRepository.cs`
5. Create BLL in `Model/BLL/YourEntityBLL.cs`
6. Create UI form in `View/UI/WinUi/Administración/` or `Transacciones/`
7. Create database table and stored procedures in `Database/`

### Adding a New Permission

1. Insert into Patente table:
```sql
INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
VALUES (NEWUUID(), 'YourFormName', 'Menu Display Text', 'Description');
```

2. Assign to role/familia:
```sql
INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
SELECT f.IdFamilia, p.IdPatente
FROM Familia f, Patente p
WHERE f.Nombre = 'ROL_Administrador'
  AND p.FormName = 'YourFormName';
```

3. Check permission in form:
```csharp
if (!SessionService.CurrentUser.TienePermiso("YourFormName")) { /* deny */ }
```

### Adding a New Translation

1. Add to both language files:
```
# idioma.es-AR
your_key=Tu texto en español

# idioma.en-GB
your_key=Your text in English
```

2. Use in code:
```csharp
label.Text = langService.Translate("your_key");
```

### Creating a Database Migration Script

Follow naming convention: `Database/XX_DescripcionDelCambio.sql`

Example structure:
```sql
-- Descripción del cambio
-- Fecha: YYYY-MM-DD

USE NegocioBiblioteca;
GO

-- 1. Crear tabla/modificar estructura
ALTER TABLE ...

-- 2. Migrar datos existentes
UPDATE ...

-- 3. Agregar constraints
ALTER TABLE ...

-- 4. Verificación
SELECT ...
```

## Important Conventions

### Naming Conventions

- **Classes:** PascalCase (`MaterialBLL`, `PrestamoRepository`)
- **Methods:** PascalCase (`ObtenerTodosMateriales()`, `RegistrarPrestamo()`)
- **Private fields:** `_camelCase` (`_materialRepository`)
- **Properties:** PascalCase (`IdMaterial`, `Titulo`)
- **Database objects:** PascalCase (`Alumno`, `Prestamo`, `FamiliaPatente`)
- **SQL parameters:** `@PascalCase` (`@IdMaterial`, `@Titulo`)

### Error Handling

Custom exceptions in `Model/DomainModel/Exceptions/`:
- `ValidacionException` - Validation errors (display to user)
- `AutenticacionException` - Authentication failures
- `UsuarioNoEncontradoException` - User not found
- `IntegridadException` - Data integrity violations

**Pattern:**
```csharp
// BLL throws domain exceptions
throw new ValidacionException("Material no encontrado");

// UI catches and displays
try {
    materialBLL.Registrar(material);
} catch (ValidacionException ex) {
    MessageBox.Show(ex.Message);
}
```

### Logical Deletes

**Always use logical deletes (Activo field) instead of physical DELETE:**

```sql
-- WRONG
DELETE FROM Material WHERE IdMaterial = @Id;

-- CORRECT
UPDATE Material SET Activo = 0 WHERE IdMaterial = @Id;
```

This maintains referential integrity and audit trail.

### SQL Injection Prevention

**Always use parameterized queries:**

```csharp
// WRONG
string sql = $"SELECT * FROM Usuario WHERE Nombre = '{nombre}'";

// CORRECT
string sql = "SELECT * FROM Usuario WHERE Nombre = @Nombre";
SqlCommand cmd = new SqlCommand(sql, connection);
cmd.Parameters.AddWithValue("@Nombre", nombre);
```

## Configuration

### App.config Structure

```xml
<connectionStrings>
  <add name="ServicesConString" connectionString="Data Source=localhost;Initial Catalog=SeguridadBiblioteca;Integrated Security=True;TrustServerCertificate=True"/>
  <add name="NegocioConString" connectionString="Data Source=localhost;Initial Catalog=NegocioBiblioteca;Integrated Security=True;TrustServerCertificate=True"/>
</connectionStrings>

<appSettings>
  <add key="LanguagePath" value="Resources\I18n\idioma"/>
  <add key="SecurityRepositoryServices" value="ServicesSecurity.DAL.Implementations"/>
</appSettings>
```

**To change database server:** Update `Data Source=` in connection strings.

## Known Issues and Warnings

1. **Compilation Warnings:**
   - `LoginService.cs:87` - Unused variable 'iex'
   - `Login.cs:210` - Unused variable 'ex'
   - `gestionPromocionAlumnos.cs:153` - Unused variable 'ex'

   These are safe to ignore but should be cleaned up.

2. **Loan-Exemplar Relationship:**
   - Always ensure `IdEjemplar` is set when creating Prestamo
   - Update Ejemplar.Estado when creating/returning loans
   - Verify Ejemplar state before allowing new loan

3. **Role Migration:**
   - System migrated from `Usuario.Rol` string to Composite pattern
   - Old code using `usuario.Rol` must use `usuario.ObtenerNombreRol()`

## Testing Guidance

### Manual Testing Checklist

**Login & Permissions:**
- [ ] Login with admin credentials
- [ ] Verify menu shows only permitted items
- [ ] Try accessing form without permission (should deny)

**Material & Exemplar Management:**
- [ ] Register new material (conceptual catalog entry)
- [ ] Add multiple exemplars for same material
- [ ] Verify CantidadTotal/Disponible updates correctly
- [ ] Edit material details
- [ ] Logical delete material (verify exemplars become inactive)

**Loan & Return Flow:**
- [ ] Register loan for available exemplar
- [ ] Verify exemplar estado changes to "Prestado"
- [ ] Verify material CantidadDisponible decreases
- [ ] Try loaning same exemplar again (should fail)
- [ ] Register return
- [ ] Verify exemplar estado returns to "Disponible"
- [ ] Verify material CantidadDisponible increases

**Student Enrollment:**
- [ ] Register new student
- [ ] Create inscription for current year
- [ ] Try creating duplicate inscription (should fail)
- [ ] Promote students to next grade
- [ ] Verify inscription history maintained

### Unit Testing (Future)

Key classes to prioritize for unit tests:
- `ValidationBLL` - Static validators
- `MaterialBLL.CalcularCantidades()` - Dynamic calculations
- `PrestamoBLL.RegistrarPrestamo()` - Complex transaction
- `EjemplarBLL.VerificarDisponibilidad()` - State validation
- Composite pattern permission checks

## Database Schema Notes

### Key Relationships

```
Material (1) ────< (N) Ejemplar
                         │
                         │ (FK)
                         ↓
Prestamo >───────< Ejemplar
   │
   └──────< Devolucion

Alumno (1) ────< (N) Prestamo
       │
       └──────< (N) Inscripcion

Usuario (N) ────< (N) Familia (via UsuarioFamilia)
Usuario (N) ────< (N) Patente (via UsuarioPatente)
Familia (N) ────< (N) Familia (via FamiliaFamilia) [Composite hierarchy]
Familia (N) ────< (N) Patente (via FamiliaPatente)
```

### Indexed Columns

For performance, ensure indexes exist on:
- `Ejemplar.IdMaterial` - Used in COUNT() queries
- `Ejemplar.Estado` - Used in availability checks
- `Prestamo.IdAlumno` - Used in student loan history
- `Prestamo.Estado` - Used in active/overdue queries
- `Inscripcion.IdAlumno` - Used in enrollment history
- `Inscripcion.AnioLectivo` - Used in year filtering

### Stored Procedures

Located in `Database/Negocio/`:
- `sp_ObtenerInscripcionActiva` - Gets student's current enrollment
- `sp_PromocionarAlumnosPorGrado` - Manual grade promotion
- `sp_PromocionarTodosLosAlumnos` - Mass promotion
- `sp_ObtenerAlumnosPorGradoDivision` - Filtered student lists

Use stored procedures for complex operations involving multiple tables or calculations.

## Troubleshooting

### Cannot Connect to Database

1. Verify SQL Server is running
2. Check connection strings in `App.config`
3. Verify Windows Authentication has permissions
4. Run database setup scripts if databases don't exist

### Permission Denied on Form Access

1. Check user has been assigned a role (ROL_* familia)
2. Verify patente exists with correct FormName
3. Check familia-patente relationships in database
4. Debug: Print `SessionService.CurrentUser.ObtenerPermisos()` to console

### Material Quantities Not Updating

1. Verify Ejemplar records exist and are Activo=1
2. Check if adapter is using correct column names from JOIN
3. Ensure MaterialRepository uses updated SELECT with COUNT() calculations
4. Check for orphaned Ejemplar records (IdMaterial FK exists?)

### Exemplar Already Loaned Error

1. Verify Ejemplar.Estado is updated when creating Prestamo
2. Check if previous loan was properly returned (Devolucion record exists?)
3. Manually fix state: `UPDATE Ejemplar SET Estado=0 WHERE IdEjemplar=...`

### Translation Not Showing

1. Verify key exists in BOTH language files
2. Check spelling matches exactly (case-sensitive)
3. Ensure files are copied to output directory (Build Action = Content, Copy Always)
4. Restart application to reload translation files

## Additional Documentation

- `Database/README_INSTALACION.md` - Complete database setup guide
- `Database/README_MIGRACION.md` - Role composite pattern migration
- `README_SISTEMA_INSCRIPCIONES.md` - Student enrollment system details
