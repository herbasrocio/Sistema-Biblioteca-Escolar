# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Sistema Biblioteca Escolar (School Library System) - A Windows Forms application for managing a school library with role-based access control, built using .NET Framework 4.7.2 and SQL Server.

## Architecture

This is a **multi-layered .NET solution** organized into three main groups:

### 1. Model Layer (Business Domain)
- **DomainModel**: Core business entities for library operations
- **DAL** (Data Access Layer): Repositories for CRUD operations on library entities
- **BLL** (Business Logic Layer): Business rules and validation for library operations
- **Services**: Utility services for library-specific functionality

### 2. Security Layer (ServicesSecurity project)
Self-contained security module implementing **Composite pattern** for permissions:

- **DomainModel/Security/Composite**:
  - `Component` (abstract): Base class for permissions hierarchy
  - `Patente`: Leaf node representing atomic permissions
  - `Familia`: Composite node that can contain Patentes or other Familias
  - `Usuario`: User entity with permissions collection
  - Roles are Familias with names starting with "ROL_" (e.g., "ROL_Administrador")

- **DAL Layer**:
  - Repository pattern with `IGenericRepository<T>` and `IJoinRepository<T>` contracts
  - Adapter pattern for entity-to-database mapping
  - `SqlHelper`: Centralized database access using ADO.NET (no ORM)
  - Factory pattern: `ServiceFactory` and `FactoryDAL` for repository instantiation

- **BLL Layer**:
  - `UsuarioBLL`: User management
  - `LanguageBLL`: i18n translation logic
  - `ValidationBLL`: Business validation rules
  - `LoggerBLL`: Logging operations
  - `ExceptionBLL`: Exception handling logic

- **Services**:
  - `LoginService`: Authentication and permission loading
  - `CryptographyService`: SHA256 password hashing (uses Unicode encoding for SQL Server NVARCHAR compatibility)
  - `LanguageManager`: i18n translation facade
  - `ExceptionManager`: Centralized exception handling
  - `LoggerService`: Logging facade
  - `Bitacora`: Audit trail service
  - `ExportarAExcel`: Excel export functionality

- **Custom Exceptions** (all in DomainModel/Exceptions):
  - `AutenticacionException`, `UsuarioNoEncontradoException`, `ContraseñaInvalidaException`
  - `ValidacionException`, `IntegridadException`, `WordNotFoundException`

### 3. View Layer
- **UI**: Windows Forms application with role-based menus
  - `WinUi/Administrador`: Admin forms (user management, catalog management)
  - `WinUi/Docente`: Teacher forms
  - `WinUi/_RecepcionistaOld`: Legacy receptionist forms

## Database Setup

**Database**: `SeguridadBiblioteca` (SQL Server)

Run scripts in order from the `Database/` folder:
```bash
# Option 1: Run master script (drops and recreates DB)
sqlcmd -S localhost -i Database/00_EJECUTAR_TODO.sql

# Option 2: Run individual scripts
sqlcmd -S localhost -i Database/01_CrearBaseDatos.sql
sqlcmd -S localhost -i Database/02_CrearTablas.sql
sqlcmd -S localhost -i Database/03_DatosIniciales.sql
sqlcmd -S localhost -i Database/05_CrearStoredProcedures.sql
```

**Default credentials**: `admin` / `admin123` (change in production)

**Connection string** (in `UI/App.config`):
```xml
Data Source=localhost;Initial Catalog=SeguridadBiblioteca;Integrated Security=True;TrustServerCertificate=True
```

## Building and Running

**Build the solution**:
```bash
# Using MSBuild (recommended)
msbuild "Sistema Biblioteca Escolar.sln" /p:Configuration=Debug

# Using Visual Studio
# Open "Sistema Biblioteca Escolar.sln" in Visual Studio 2022
# Build > Build Solution (Ctrl+Shift+B)
```

**Run the application**:
```bash
# From command line
cd UI\bin\Debug
UI.exe

# From Visual Studio
# Set UI as startup project and press F5
```

**Clean build**:
```bash
msbuild "Sistema Biblioteca Escolar.sln" /t:Clean /p:Configuration=Debug
```

## Key Configuration Files

- `UI/App.config`:
  - Connection string: `ServicesConString`
  - Language path: `Resources\I18n\idioma`
  - Security repository namespace: `ServicesSecurity.DAL.Implementations`

## Project Dependencies

- **ServicesSecurity**: Self-contained, no dependencies on other projects
- **UI**: References ServicesSecurity, DomainModel, BLL, DAL, Services
- **BLL/DAL/Services**: Reference DomainModel
- Uses ADO.NET directly (no Entity Framework or ORM)

## Permission System

The system uses **Composite pattern** for hierarchical permissions:

1. **Patente** (Permission): Atomic action (e.g., "CRUDUsuario", "CRUDCatalogo")
2. **Familia** (Role/Group): Container of Patentes and/or other Familias
3. **Roles**: Familias with names starting with "ROL_" (e.g., "ROL_Administrador")
4. Users are assigned Familias (roles) and/or individual Patentes

Permission assignment relationships:
- `UsuarioFamilia`: Many-to-many (User ↔ Familia/Role)
- `UsuarioPatente`: Many-to-many (User ↔ Patente)
- `FamiliaPatente`: Many-to-many (Familia ↔ Patente)
- `FamiliaFamilia`: Many-to-many (Familia ↔ Familia, for role hierarchies)

## i18n (Internationalization)

- Translations stored in: `Resources\I18n\idioma\{culture}.txt`
- Current culture: `Thread.CurrentThread.CurrentCulture`
- Usage: `LanguageManager.Translate("wordKey")`
- Auto-creates missing translations with original word as value

## Important Security Notes

- **Password hashing**: SHA256 with `Encoding.Unicode` (UTF-16) to match SQL Server NVARCHAR
- **Integrity checks**: DVH (Dígito Verificador Horizontal) for row integrity
- Database includes stored procedures for hash calculation and verification
- All authentication exceptions inherit from `AutenticacionException`
