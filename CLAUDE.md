# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Sistema Biblioteca Escolar** - School Library Management System developed for UAI (Prácticas Profesionales 3ro).

This is a .NET Framework 4.7.2 Windows Forms application for managing a school library with student enrollment, material catalog, loans/returns, and role-based access control.

## Architecture

### Solution Structure

The solution follows a layered architecture with clear separation of concerns:

```
Sistema Biblioteca Escolar.sln
├── Model/                          # Business & Data Logic
│   ├── DomainModel/               # Domain entities (Alumno, Material, Inscripcion, etc.)
│   ├── DAL/                       # Data Access Layer
│   │   ├── Contracts/            # Repository interfaces
│   │   ├── Implementations/      # Repository implementations
│   │   └── Tools/                # Adapters (DataRow → Entity conversion)
│   ├── BLL/                       # Business Logic Layer
│   └── Services/                  # Application services
├── View/                          # Presentation Layer
│   └── UI/                       # Windows Forms application
│       └── WinUi/                # Form implementations
│           ├── Administración/   # Admin forms (Users, Permissions, Students, Materials)
│           └── Transacciones/    # Transaction forms (Loans, Returns)
└── Security/                      # Security Module (separate concern)
    └── ServicesSeguridad/        # Authentication, Authorization, Logging, i18n
```

### Key Architectural Patterns

**1. Repository Pattern**
- Interfaces in `DAL/Contracts` (IGenericRepository, IAlumnoRepository, IMaterialRepository, etc.)
- Implementations in `DAL/Implementations`
- Adapters in `DAL/Tools` convert DataRow to domain entities

**2. Composite Pattern (Security System)**
- `Component` (abstract base) → `Familia` (composite) and `Patente` (leaf)
- Located in `Security/ServicesSeguridad/DomainModel/Security/Composite/`
- Enables hierarchical role/permission structures
- Roles are `Familia` with names starting with "ROL_" (e.g., "ROL_Administrador")
- Permissions are `Patente` objects with `MenuItemName` property for UI access control

**3. Multilingual Support (i18n)**
- Translation files: `View/UI/Resources/I18n/idioma.es-AR` and `idioma.en-GB`
- Use `LanguageManager.Translate("key")` for all UI text
- LanguageManager service in `Security/ServicesSeguridad/Services/`

## Database Configuration

**Two SQL Server databases:**
1. **SeguridadBiblioteca** - Security (Users, Roles, Permissions, Logs)
2. **NegocioBiblioteca** - Business (Students, Materials, Loans, Enrollments)

Connection strings in `View/UI/App.config`:
```xml
<connectionStrings>
  <add name="ServicesConString" connectionString="Data Source=localhost;Initial Catalog=SeguridadBiblioteca;Integrated Security=True;TrustServerCertificate=True"/>
  <add name="NegocioConString" connectionString="Data Source=localhost;Initial Catalog=NegocioBiblioteca;Integrated Security=True;TrustServerCertificate=True"/>
</connectionStrings>
```

**Important:**
- Security DAL uses `SqlHelper` class (in `Security/ServicesSeguridad/DAL/Tools/`) which reads `ServicesConString`
- Business DAL repositories instantiate connection string directly from `NegocioConString`

## Common Development Commands

### Build the Solution
```bash
# From solution root directory
msbuild "Sistema Biblioteca Escolar.sln" -t:Rebuild -p:Configuration=Debug
```

### Run the Application
```bash
# After building
cd "View\UI\bin\Debug"
.\UI.exe
```

### Database Setup

**Initial Setup (First Time):**
```bash
# Execute master script for Security DB
sqlcmd -S localhost -E -i "Database\00_EJECUTAR_TODO.sql"

# Execute master script for Business DB
sqlcmd -S localhost -E -i "Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql"
```

**Enrollment System Setup (if needed):**
```bash
sqlcmd -S localhost -E -i "Database\EJECUTAR_SETUP_INSCRIPCIONES.sql"
```

**Default Admin Credentials:**
- Username: `admin`
- Password: `admin123`

## Key Domain Concepts

### Student Enrollment System
- `Inscripcion` entity tracks student enrollment per academic year (`AnioLectivo`)
- One student can have only ONE active enrollment per year
- States: `Activo`, `Finalizado`, `Abandonado`
- Promotion system moves students between grades automatically or manually
- Stored procedures: `sp_PromocionarAlumnosPorGrado`, `sp_PromocionarTodosLosAlumnos`

### Material Catalog
- `Material` entity supports different types (Libro, Revista, etc.) via `TipoMaterial` enum
- States managed via `EstadoMaterial` enum
- Both enums in `Model/DomainModel/Enums/`
- Logical deletion (Activo flag)

### Security & Permissions
- Dynamic menu generation based on user permissions in `View/UI/WinUi/Administración/menu.cs`
- `TienePermiso(string nombrePatente)` checks if user has specific permission
- Permission checking is recursive through Familia hierarchy
- Pattern: Check `Patente.MenuItemName` property for UI permission names

### Permission Constants (in menu.cs)
- `GestionUsuarios` - User management
- `GestionPermisos` - Permission management
- `ConsultarMaterial` - Material catalog viewing
- `RegistrarMaterial` - Material registration
- `GestionAlumnos` - Student management
- `GestionPrestamos` - Loan management
- `GestionDevoluciones` - Return management
- `PromocionAlumnos` - Student promotion (bulk operations)

## Important Development Notes

### When Adding New Forms
1. Always pass `Usuario _usuarioLogueado` to constructor
2. Use `LanguageManager.Translate()` for all text
3. Check permissions using `TienePermiso()` if needed
4. Add new permission `Patente` via SQL script in `Database/` folder
5. Update menu.cs if form needs menu item

### When Adding Database Features
1. Create SQL scripts in `Database/` (security) or `Database/Negocio/` (business)
2. Follow naming convention: `##_DescriptiveName.sql`
3. Create stored procedures for complex operations
4. Create repository interface in `DAL/Contracts`
5. Implement repository in `DAL/Implementations`
6. Create adapter in `DAL/Tools` for DataRow mapping
7. Create BLL class in `BLL/` for business logic

### Data Access Pattern
```csharp
// Example repository implementation
public class MaterialRepository : IMaterialRepository
{
    private readonly string _connectionString;

    public MaterialRepository()
    {
        var connStringSetting = ConfigurationManager.ConnectionStrings["NegocioConString"];
        _connectionString = connStringSetting.ConnectionString;
    }

    public List<Material> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            // Execute query
            // Use MaterialAdapter.AdaptMaterial(DataRow) to convert
        }
    }
}
```

### Adding Translations
1. Add key-value pairs to both `idioma.es-AR` and `idioma.en-GB`
2. Use format: `key=value` (one per line)
3. Keys use snake_case convention

## Current System Status

**Completed Features:**
- ✅ User authentication & authorization
- ✅ Role-based access control (Composite pattern)
- ✅ Student management (CRUD)
- ✅ Student enrollment & promotion system
- ✅ Material catalog (CRUD with search filters)
- ✅ Multilingual support (Spanish/English)
- ✅ Dynamic menu based on permissions
- ✅ Audit logging (Bitacora)

**Pending/Not Implemented:**
- ⏳ Loan/Return transactions (forms exist but not functional)
- ⏳ Reports generation
- ⏳ Testing suite

## Known Issues

**Compilation Warnings:**
- `LoginService.cs:87` - Unused variable 'iex'
- `Login.cs:210` - Unused variable 'ex'
- `gestionPromocionAlumnos.cs:153` - Unused variable 'ex'

These do not affect functionality but should be cleaned up.

## Documentation

Reference documentation in repository:
- `README_SISTEMA_INSCRIPCIONES.md` - Enrollment & promotion system overview
- `INSTRUCCIONES_INSTALACION.md` - Detailed installation guide (if exists)
- `RESUMEN_IMPLEMENTACION_COMPLETA.md` - Technical implementation details (if exists)

## Git Workflow

Current branch: `master`
No main branch configured - PRs should target `master`
