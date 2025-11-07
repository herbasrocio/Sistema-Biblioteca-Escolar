# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

**Sistema Biblioteca Escolar** is a C# .NET Framework 4.7.2 Windows Forms application for managing a school library. The system handles student management, library materials (books and other resources), loans, returns, renewals, and includes comprehensive security and audit logging capabilities.

## Technology Stack

- .NET Framework 4.7.2
- Windows Forms (WinForms)
- SQL Server (two databases: SeguridadBiblioteca, NegocioBiblioteca)
- Microsoft Report Viewer 15.0 (SSRS for reports)
- Integrated Security for SQL Server

## Building and Running

**Build the solution:**
```bash
msbuild "Sistema Biblioteca Escolar.sln" /p:Configuration=Debug
# Or use Visual Studio: Build > Build Solution (Ctrl+Shift+B)
```

**Run the application:**
The main entry point is `View/UI/UI.csproj`. Run from Visual Studio or execute the compiled `.exe` from `View/UI/bin/Debug/`.

**Database setup:**
1. Ensure SQL Server is running on localhost
2. Execute `Database/00_EJECUTAR_TODO.sql` to create the security database and initial data
3. Execute `Database/Negocio/00_EJECUTAR_TODO_NEGOCIO.sql` to create the business database and test data

**Connection strings** are configured in `View/UI/App.config`:
- `ServicesConString`: SeguridadBiblioteca (security, users, permissions, audit logs)
- `NegocioConString`: NegocioBiblioteca (students, materials, loans, returns)

## Architecture: Layered (N-Tier)

The application follows a strict layered architecture with clear separation of concerns:

```
View/UI (Presentation)
    ↓
Model/BLL (Business Logic)
    ↓
Model/DAL (Data Access with Repository Pattern)
    ↓
Model/DomainModel (Domain Entities)
    ↓
SQL Server Databases
```

**Security** is a cross-cutting concern implemented in `Security/ServicesSeguridad/`.

### Project Dependencies

- **UI** → BLL, Services, ServicesSecurity
- **BLL** → DAL, DomainModel, Services
- **DAL** → DomainModel, Services (connection strings)
- **Services** → DomainModel
- **ServicesSecurity** → standalone (security domain model + DAL)

## Key Projects

### Model/DomainModel
Domain entities representing the business model:
- **Alumno**: Student (IdAlumno, Nombre, Apellido, DNI, Grado, Division)
- **Material**: Library catalog item (Titulo, Autor, Editorial, Tipo, Genero, ISBN)
- **Ejemplar**: Physical copy of a material (CodigoEjemplar/barcode, Estado, Ubicacion)
- **Prestamo**: Loan (IdPrestamo, IdAlumno, IdEjemplar, FechaPrestamo, FechaDevolucionPrevista, Estado)
- **Devolucion**: Return (IdDevolucion, IdPrestamo, FechaDevolucion)
- **RenovacionPrestamo**: Loan renewal (IdRenovacion, IdPrestamo, FechaRenovacion, FechaDevolucionAnterior, FechaDevolucionNueva)
- **BitacoraSeguridad**: Security audit log
- **BitacoraOperaciones**: Operations audit log
- **Inscripcion**: Student enrollment
- **AnioLectivo**: School year
- **HistorialEstadoEjemplar**: Copy state history

### Model/DAL
Repository pattern with Unit of Work for transactional operations:
- **Contracts/**: Repository interfaces (IPrestamoRepository, IEjemplarRepository, IAlumnoRepository, etc.)
- **Implementations/**: Concrete repository classes
- **Tools/Adapter**: DataReader → object mapping
- **IUnitOfWork**: Coordinates multiple repository operations in a single transaction

### Model/BLL
Business logic layer with validation and calculations:
- **PrestamoBLL**: Loan business rules (eligibility, maximum loans, due date calculation)
- **DevolucionBLL**: Return processing
- **RenovacionBLL**: Renewal logic (max 2 renewals, extends by ~14 days)
- **MaterialBLL**, **AlumnoBLL**, **EjemplarBLL**: CRUD operations with business rules
- **ReporteBLL**: Report data preparation
- **ValidationBLL**: Cross-cutting validations

### View/UI
Windows Forms organized by functional area:
- **Administración/**: Login, main menu, student/material/user/copy management
- **Transacciones/**: Loan registration, returns, renewals, loan management
- **Reportes/**: SSRS-based reports (active loans, most borrowed materials, usage by grade)
- **Bitacoras/**: Audit log viewers (security and operations)

### Security/ServicesSeguridad
Complete authentication and authorization system using the **Composite Pattern**:
- **DomainModel/Security/Composite/**: Component, Patente (leaf), Familia (composite), Usuario
- **Services/**: Cryptography, Login, Logger, LanguageManager
- **DAL/**: Repositories for security entities

## Security System (Composite Pattern)

The permission system uses a hierarchical tree structure:

- **Component** (abstract base): Base class for the hierarchy
- **Patente** (leaf): Individual permission (FormName, MenuItemName)
- **Familia** (composite): Permission group/role that can contain Patentes or other Familias
- **Usuario**: Has a List<Component> called Permisos representing the full permission tree

**Important permission methods on Usuario.cs:68-142:**
- `TienePermiso(nombrePatente)`: Checks if user has a specific permission with special rules:
  - "Gestión Alumnos" grants "Promoción Alumnos"
  - "Gestión Préstamos" grants "Renovar Préstamo"
  - "Consultar Reportes" grants all individual report permissions
- `TieneRol(nombreRol)`: Checks if user has a specific role
- `ObtenerNombreRol()`: Gets role name without "ROL_" prefix

## Unit of Work Pattern

The `IUnitOfWork` interface (Model/DAL/Contracts/IUnitOfWork.cs) coordinates transactional operations across multiple repositories:

```csharp
using (var uow = new UnitOfWork())
{
    uow.BeginTransaction();
    try
    {
        // Multiple repository operations
        uow.Prestamos.Insert(prestamo);
        uow.Ejemplares.Update(ejemplar);
        uow.Commit();
    }
    catch
    {
        uow.Rollback();
        throw;
    }
}
```

**Use cases requiring Unit of Work:**
- Creating a loan: Insert Prestamo + Update Ejemplar.Estado to "Prestado"
- Processing a return: Update Prestamo.Estado + Update Ejemplar.Estado + Insert Devolucion
- Renewing a loan: Insert RenovacionPrestamo + Update Prestamo.FechaDevolucionPrevista

## Business Rules

### Loan System (Préstamos)
- Default loan period: ~14 days from registration date
- Maximum concurrent loans per student: configurable (typically 3)
- States: Activo, Devuelto, Atrasado
- Late loans (Atrasado) are automatically identified when current date > FechaDevolucionPrevista
- Students with overdue loans cannot borrow new materials

### Renewal System (Renovaciones)
- Maximum renewals per loan: 2
- Each renewal extends the due date by ~14 days
- Cannot renew if loan is overdue
- Tracks RenovacionPrestamo records with old and new due dates

### Copy States (Estado de Ejemplares)
- **Disponible**: Available for loan
- **Prestado**: Currently on loan
- **Reparación**: Under repair, not available
- **Perdido**: Lost
- **Dado de baja**: Decommissioned

State transitions are tracked in `HistorialEstadoEjemplar`.

## Internationalization (i18n)

The application supports multiple languages:
- **Spanish (es-AR)**: Default
- **English (en-GB)**: Available

Language files: `View/UI/Resources/I18n/idioma.es-AR` and `idioma.en-GB`

Managed by `LanguageManager` in ServicesSecurity. UI labels are dynamically loaded based on the selected language.

## Reports

Three SSRS reports using Microsoft Report Viewer:
1. **ReportePrestamosActivos**: Active loans with student info and due dates
2. **ReporteMaterialesMasPrestados**: Most borrowed materials (ranking)
3. **ReporteUsoPorGrado**: Library usage breakdown by student grade level

Reports are accessed through the "Reportes" menu with the "Consultar Reportes" permission.

## Audit Logging (Bitácoras)

Two comprehensive audit logs:

**BitacoraSeguridad** (Security database):
- Login attempts, permission changes, user management
- Fields: Fecha, IdUsuario, TipoEvento, Descripcion, Gravedad (INFO/WARNING/ERROR/CRITICAL)

**BitacoraOperaciones** (Business database):
- Business operations: loans created, returns processed, materials modified
- Same structure as BitacoraSeguridad

Both logs are queryable through dedicated UI forms in `View/UI/WinUi/Bitacoras/`.

## Design Patterns

1. **Layered Architecture**: Clear separation of UI, BLL, DAL, Domain
2. **Repository Pattern**: Abstraction over data access with interfaces
3. **Unit of Work Pattern**: Transactional integrity across operations
4. **Composite Pattern**: Hierarchical permissions (Patente/Familia/Component)
5. **Adapter Pattern**: DataReader → domain object conversion
6. **Dependency Injection**: BLL constructors accept repository interfaces for testing

## Common Development Workflows

### Adding a new entity
1. Create domain class in `Model/DomainModel/`
2. Add repository interface in `Model/DAL/Contracts/`
3. Implement repository in `Model/DAL/Implementations/`
4. Create adapter in `Model/DAL/Tools/`
5. Add BLL class in `Model/BLL/`
6. Create UI forms in `View/UI/WinUi/[Category]/`
7. Update `IUnitOfWork` if transactional operations are needed

### Adding a new permission
1. Add Patente record in database (table: Patente)
2. Assign Patente to appropriate Familia (role)
3. Check forms for permission with `Usuario.TienePermiso(FormName)`
4. Consider adding special rules in `Usuario.TienePermiso()` if needed

### Database migrations
- Add numbered SQL scripts in `Database/` (e.g., `15_NewFeature.sql`)
- Update `Database/00_EJECUTAR_TODO.sql` to include the new script
- For business database changes, use `Database/Negocio/` folder

## Important Files

- `View/UI/App.config`: Connection strings, language path, security settings
- `Security/ServicesSeguridad/DomainModel/Security/Composite/Usuario.cs`: Core security logic and permission rules
- `Model/DAL/Contracts/IUnitOfWork.cs`: Transaction coordination interface
- `Database/00_EJECUTAR_TODO.sql`: Master security database setup script
- `Database/Negocio/00_EJECUTAR_TODO_NEGOCIO.sql`: Master business database setup script

## Code Quality Notes

- DVH (Dígito Verificador Horizontal): Checksum for data integrity on Usuario table
- All passwords are hashed using `CryptographyService.HashPassword()`
- Use parameterized queries in all DAL methods to prevent SQL injection
- Forms implement proper disposal of database connections
- BLL methods throw descriptive exceptions that are caught and displayed in the UI layer
