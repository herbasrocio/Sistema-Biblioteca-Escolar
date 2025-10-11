# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Sistema Biblioteca Escolar** - School Library Management System for UAI (Universidad Abierta Interamericana). This is a Windows Forms .NET Framework 4.7.2 application that manages library materials, student records, loans/returns, and includes a complete student enrollment and promotion system by academic year.

The system uses a **dual-database architecture**:
- **NegocioBiblioteca**: Business logic (materials, students, loans, enrollments)
- **SeguridadBiblioteca**: Security system (users, permissions, logging)

## Solution Structure

The solution follows a **layered architecture** organized into three main folders:

### Model/ - Business Layer
Contains all business logic separated into layers:
- **DomainModel**: Entity classes (Material, Alumno, Prestamo, Devolucion, Inscripcion, AnioLectivo)
- **DAL**: Data Access Layer with Repository pattern (Contracts, Implementations, Adapters)
- **BLL**: Business Logic Layer (MaterialBLL, AlumnoBLL, PrestamoBLL, InscripcionBLL, etc.)
- **Services**: Utility services (shared across the application)

### Security/ - Security System
Complete security subsystem with Composite pattern for permissions:
- **ServicesSecurity**: Self-contained security library
  - DomainModel: Security entities (Usuario, Familia, Patente) using Composite pattern
  - DAL: Repositories for security entities
  - BLL: Security business logic (UsuarioBLL, FamiliaBLL)
  - Services: CryptographyService, LanguageManager, LoginService, ExceptionManager

### View/ - Presentation Layer
- **UI**: Windows Forms application (WinForms)
  - WinUi/Administración: Admin forms (Login, menu, user/permission management, student promotion)
  - WinUi/Transacciones: Transaction forms (loans, returns)
  - Resources/I18n: Multi-language support (es-AR, en-GB)

## Building and Running

### Build Commands
```bash
# From solution root - Full rebuild
msbuild "Sistema Biblioteca Escolar.sln" -t:Rebuild -p:Configuration=Debug

# Clean before build
msbuild "Sistema Biblioteca Escolar.sln" -t:Clean
msbuild "Sistema Biblioteca Escolar.sln" -t:Build

# From Visual Studio
# Build > Rebuild Solution (Ctrl+Shift+B)
```

### Run Application
```bash
# After building
cd "View\UI\bin\Debug"
UI.exe

# From Visual Studio
# Press F5 (Debug) or Ctrl+F5 (Without Debug)
```

### Build Order (Dependencies)
1. ServicesSecurity (Security layer - no dependencies)
2. DomainModel (Entities - no dependencies)
3. DAL (depends on DomainModel)
4. Services (depends on DomainModel)
5. BLL (depends on DAL, DomainModel, ServicesSecurity)
6. UI (depends on all layers)

## Database Setup

### Initial Setup - Complete Installation
```bash
# 1. Setup Security Database (Run FIRST)
sqlcmd -S localhost -E -i "Database\00_EJECUTAR_TODO.sql"

# 2. Setup Business Database
sqlcmd -S localhost -E -i "Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql"

# 3. Setup Enrollment System (inscriptions)
sqlcmd -S localhost -E -i "Database\EJECUTAR_SETUP_INSCRIPCIONES.sql"
```

### Database Verification
```bash
# Check current state without modifying
sqlcmd -S localhost -E -i "Database\Negocio\00_VerificarBaseDatos.sql"
```

### Important Database Notes
- **Two databases**: SeguridadBiblioteca (security) and NegocioBiblioteca (business)
- Security database uses **Composite Pattern** for hierarchical permissions (Familia = roles/groups, Patente = atomic permissions)
- Business database includes enrollment history system (Inscripcion table) for tracking students across academic years
- Connection strings in `View\UI\App.config` - update server name if needed

## Key Architecture Patterns

### 1. Composite Pattern (Security Permissions)
The security system uses Composite pattern for flexible permission hierarchies:

```
Component (abstract base)
├── Patente (Leaf - atomic permission)
└── Familia (Composite - role or permission group)
    ├── Can contain Patentes (atomic permissions)
    └── Can contain other Familias (nested groups)

Example hierarchy:
ROL_Administrador (Familia)
├── Gestión de Usuarios (Familia)
│   ├── Alta de Usuario (Patente)
│   ├── Baja de Usuario (Patente)
│   └── Modificar Usuario (Patente)
└── PromocionAlumnos (Patente)
```

Key classes:
- `Security/ServicesSeguridad/DomainModel/Security/Composite/Component.cs` - Base class
- `Security/ServicesSeguridad/DomainModel/Security/Composite/Familia.cs` - Composite (roles/groups)
- `Security/ServicesSeguridad/DomainModel/Security/Composite/Patente.cs` - Leaf (permissions)

### 2. Repository Pattern (Data Access)
All data access uses Repository pattern with Adapters for SQL mapping:

```
IGenericRepository<T>        (contract)
└── SpecificRepository       (implementation - SQL queries)
    └── SpecificAdapter      (SQL DataReader to Entity mapping)
```

Example flow: BLL → Repository (via contract) → Adapter → Database

### 3. Adapter Pattern (SQL Mapping)
Adapters convert between DataReader and domain entities:
- Located in `Model/DAL/Tools/` and `Security/ServicesSeguridad/DAL/Implementations/Adapter/`
- Example: `MaterialAdapter.cs` maps SQL results to Material objects

## Multi-Language Support (i18n)

The application supports Spanish (es-AR) and English (en-GB):

### Language Files Location
- `View/UI/Resources/I18n/idioma.es-AR` - Spanish translations
- `View/UI/Resources/I18n/idioma.en-GB` - English translations

### Language File Format
```
key_name=Translation text
ejemplo_clave=Texto en español
example_key=Text in English
```

### Using Translations in Code
```csharp
// Import LanguageManager
using ServicesSecurity.Services;

// Use in forms
string text = LanguageManager.Translate("key_name");
button.Text = LanguageManager.Translate("guardar");
```

### Adding New Translations
1. Add same key to BOTH language files (`idioma.es-AR` and `idioma.en-GB`)
2. Ensure keys are identical, only values differ
3. Use lowercase with underscores (e.g., `promocion_alumnos`)

## Student Enrollment System

Recent major feature: Complete enrollment and promotion system for managing students across academic years.

### Core Concept
- Students have **historical enrollments** by academic year (tabla Inscripcion)
- Each enrollment tracks: Year, Grade, Division, Status (Activo/Finalizado/Abandonado/Egresado)
- Maintains complete academic history per student

### Key Components
- **Domain Model**: `Model/DomainModel/Inscripcion.cs`, `Model/DomainModel/AnioLectivo.cs`
- **Repository**: `Model/DAL/Implementations/InscripcionRepository.cs`
- **Business Logic**: `Model/BLL/InscripcionBLL.cs`
- **UI**: `View/UI/WinUi/Administración/gestionPromocionAlumnos.cs`

### Stored Procedures (Database/Negocio/)
- `sp_ObtenerInscripcionActiva` - Get current enrollment for a student
- `sp_PromocionarAlumnosPorGrado` - Promote specific grade/division
- `sp_PromocionarTodosLosAlumnos` - Mass promotion (all grades)
- `sp_ObtenerAlumnosPorGradoDivision` - List students by grade/division

### Usage Example
```csharp
// Get student's enrollment history
var inscripcionBLL = new InscripcionBLL();
var historial = inscripcionBLL.ObtenerHistorialAlumno(idAlumno);

// Promote students manually
inscripcionBLL.PromocionarAlumnosPorGrado(2025, 2026, gradoActual, divisionActual, gradoNuevo, divisionNueva);

// Mass promotion (end of school year)
inscripcionBLL.PromocionarTodosLosAlumnos(2025, 2026);
```

## Security System Details

### User Authentication
```csharp
// Login flow (in forms)
using ServicesSecurity.Services;

var loginService = new LoginService();
var usuario = loginService.Login(username, password);
// Returns Usuario object if successful, throws exception if failed
```

### Permission Checking
Permissions are checked via the Composite pattern:
- User has Familias (roles/groups) assigned
- Each Familia contains Patentes (permissions) and can contain other Familias
- Recursive traversal checks all permissions

### Important Security Services
- **CryptographyService**: Password hashing (SHA256)
- **LoginService**: Authentication logic
- **LanguageManager**: i18n support
- **ExceptionManager**: Centralized exception handling

### Default Admin Credentials
- User: `admin`
- Password: `admin123`
- **IMPORTANT**: Change password after first login in production

## Common Development Tasks

### Adding a New Entity
1. Create entity class in `Model/DomainModel/`
2. Create repository interface in `Model/DAL/Contracts/`
3. Create adapter in `Model/DAL/Tools/`
4. Implement repository in `Model/DAL/Implementations/`
5. Create BLL in `Model/BLL/`
6. Reference in UI layer

### Adding a New Form
1. Create form in `View/UI/WinUi/[Administración or Transacciones]/`
2. Add translations to both i18n files
3. Create corresponding permission (Patente) in database if needed
4. Wire up form opening from menu

### Adding Database Changes
1. Create SQL script in appropriate `Database/` subfolder
2. Test script independently
3. Update master setup scripts if part of initial setup
4. Document in README files

### Validation Pattern
Both layers have ValidationBLL classes:
- `Model/BLL/ValidationBLL.cs` - Business validations (DNI format, required fields)
- `Security/ServicesSeguridad/BLL/ValidationBLL.cs` - Security validations (user/password rules)

Always validate in BLL before calling DAL:
```csharp
// In BLL class
ValidationBLL.ValidarCamposRequeridos(alumno.Nombre, alumno.Apellido, alumno.DNI);
ValidationBLL.ValidarFormatoDNI(alumno.DNI);
// Then proceed with repository call
```

## Exception Handling

Custom exception hierarchy in both layers:

### Business Layer Exceptions
- `Model/DomainModel/Exceptions/ValidacionException.cs` - Validation errors

### Security Layer Exceptions
- `ServicesSecurity.DomainModel.Exceptions.AutenticacionException` - Authentication failures
- `ServicesSecurity.DomainModel.Exceptions.ValidacionException` - Validation errors
- `ServicesSecurity.DomainModel.Exceptions.IntegridadException` - Data integrity issues

### Usage Pattern
```csharp
try {
    // Business logic
} catch (ValidacionException ex) {
    MessageBox.Show(ex.Message, "Validation Error");
} catch (Exception ex) {
    var mensaje = ExceptionManager.ManejarExcepcion(ex);
    MessageBox.Show(mensaje, "Error");
}
```

## Documentation Files

Extensive documentation exists - always check before asking:

- `INDICE_DOCUMENTACION.md` - Central index of all documentation
- `RESUMEN_EJECUTIVO.md` - Executive summary of enrollment system
- `README_SISTEMA_INSCRIPCIONES.md` - Technical overview of enrollment system
- `INSTRUCCIONES_INSTALACION.md` - Complete installation guide
- `RESUMEN_IMPLEMENTACION_COMPLETA.md` - Full implementation details
- `CHECKLIST_VERIFICACION.md` - QA verification checklist
- `Database/README_INSTALACION.md` - Database setup guide
- `Database/Negocio/README.md` - Business database scripts guide

## Important Notes

### Database Connection
- Connection strings in `View/UI/App.config`
- Uses Windows Authentication by default (Integrated Security=True)
- Update server name for your SQL Server instance

### Build Warnings
- 3 expected warnings about unused variables (not critical)
- Focus on errors, not warnings

### Git Status Notes
- Old ServicesSeguridad files were moved to Security folder (marked as deleted in git)
- Old UI, BLL, DAL, DomainModel, Services files also reorganized under Model/View folders
- New structure is in Model/, Security/, View/ folders

### Testing Strategy
- No unit tests currently implemented
- Testing done via UI and manual database verification
- Use `Database/Negocio/00_VerificarBaseDatos.sql` for database state checks

### Performance Optimization
- Database has indexes on Inscripcion table (IdAlumno, AnioLectivo)
- Stored procedures used for complex queries (promotion operations)
- Use SPs for bulk operations rather than iterating in C#

## Troubleshooting

### Build Errors
```bash
# Clean and rebuild
msbuild "Sistema Biblioteca Escolar.sln" -t:Clean
msbuild "Sistema Biblioteca Escolar.sln" -t:Rebuild
```

### Database Connection Issues
- Verify SQL Server is running
- Check server name in App.config matches your instance
- Ensure Windows Authentication has permissions

### Missing Translations
- Check both `idioma.es-AR` and `idioma.en-GB` have the same keys
- Use lowercase with underscores for key names
- If key missing, LanguageManager returns the key itself (visible in UI)

### Enrollment System Issues
- Run `Database\Negocio\00_VerificarBaseDatos.sql` to check data
- Verify stored procedures exist with query: `SELECT * FROM sys.procedures WHERE name LIKE 'sp_%Inscripcion%'`
- Check migration completed: `SELECT COUNT(*) FROM Inscripcion`
