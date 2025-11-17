# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Quick Overview

N-tier school library system: C# / .NET 4.7.2 / WinForms / SQL Server

**6 Layers**: Domain Model → DAL (Repository + UoW) → BLL → Services → Security Services → UI

**2 Databases**: SeguridadBiblioteca (auth/security) + NegocioBiblioteca (library operations)

## Build and Run

**Build the solution**:
```bash
# Using build.bat (preferred)
./build.bat

# Or using MSBuild directly
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Sistema Biblioteca Escolar.sln" /p:Configuration=Debug
```

**Database setup**:
```sql
-- Execute master script to create both databases
:r "Database/00_EJECUTAR_TODO.sql"

-- Or execute individual scripts in order (see Database folder)
```

**Run the application**:
- Build the solution first
- Run from Visual Studio (F5) or execute `View\UI\bin\Debug\UI.exe`
- Default credentials: username=`admin`, password=`admin`

---

## 1. Architecture Layers

**Layer 1: DomainModel** (`Model/DomainModel/`)
- Pure POCOs: Alumno, Material, Ejemplar, Prestamo, Devolucion, BitacoraSeguridad
- No dependencies, Guid-based identifiers auto-initialized
- Enums: TipoMaterial, EstadoMaterial

**Layer 2: DAL** (`Model/DAL/`)
- Repository Pattern with Unit of Work
- Direct SQL queries (no ORM), SqlCommand + DataTable
- Adapters: DataRow → Domain objects
- Connection strings from App.config

**Layer 3: BLL** (`Model/BLL/`)
- Business logic, validation, rules
- AlumnoBLL, MaterialBLL, PrestamoBLL, DevolucionBLL, ReporteBLL
- Transactions via Unit of Work

**Layer 4: Services** (`Model/Services/`)
- ExportService: CSV export for reports

**Layer 5: Services Security** (`Model/ServicesSeguridad/`)
- LoginService: Authentication
- PermissionManager: Observer pattern for permission updates
- CryptographyService: Password hashing (SHA256)
- LanguageManager: Multilingual support
- BackupBLL/Repository: Backup and restore

**Layer 6: View** (`View/UI/`)
- WinForms application
- Entry point: Login.cs form
- Forms in WinUi/Administración/

---

## 2. Key Patterns

### 1. Repository Pattern
CRUD: Add, Update, Delete, GetAll, ObtenerPorId
Each entity has IRepository interface + Implementation

### 2. Unit of Work
- Manages transactions across multiple repositories
- Usage: BeginTransaction() → operations → Commit() or Rollback()
- Implementation: TransactionScope, ReadCommitted, 2-min timeout

### 3. Composite Pattern (Security)
```
Component (abstract)
  ├── Patente (leaf) - individual permission
  └── Familia (composite) - role or group (e.g., "ROL_Administrador")

Usuario.Permisos: List<Component>
Methods:
  usuario.TienePermiso("FormName")  - recursive search
  usuario.TieneRol("Administrador") - check ROL_ family
```

### 4. Observer Pattern
PermissionManager notifies UI when permissions change

### 5. Adapter Pattern
Convert DataTable rows to domain objects

### 6. Factory Pattern
ServiceFactory creates repository instances

---

## 3. Databases

### SeguridadBiblioteca (Security)
- Usuario, Familia (roles), Patente (permissions)
- FamiliaPatente, UsuarioFamilia, UsuarioPatente (relationships)
- BitacoraSeguridad, Backup, Idioma

### NegocioBiblioteca (Business)
- Alumno, Material, Ejemplar, Prestamo, Devolucion
- HistorialEstadoEjemplar, RenovacionPrestamo
- BitacoraOperaciones, AnioLectivo, Inscripcion

---

## 4. Security System

**Authentication**: LoginService validates credentials, loads permission tree

**Authorization**: Usuario.TienePermiso() recursively searches Composite tree

**Real-Time Updates**: PermissionManager notifies observers of permission changes

**Audit Logging**: BitacoraSeguridad captures all critical events

---

## 5. Multilingual Support

**LanguageManager** (Static Singleton):
- LanguageManager.Translate("key") - returns translation
- LanguageManager.ChangeLanguage("es-AR") - switches language, fires event
- Storage: Idioma table in SeguridadBiblioteca
- Supported: es-AR (Spanish), en-GB (English)

---

## 6. Domain Entities

**Alumno**: Student (Name, Grade, Division, DNI)
**Material**: Catalog entry (Title, Author, Type, Quantity)
**Ejemplar**: Physical copy (Barcode, State, Location)
**Prestamo**: Loan (Dates, Status, Renewals)
**Devolucion**: Return (Date, Observations)
**BitacoraSeguridad**: Security audit log

---

## 7. Naming Conventions

**Repositories**: I{Entity}Repository, {Entity}Repository
**BLL**: {Entity}BLL
**Domain Models**: {Entity}
**Methods**: PascalCase, Obtener*, Esta*, Puede*, Fue*
**Variables**: _camelCase (private), PascalCase (public)
**Database**: PascalCase tables/columns
**UI Controls**: btn*, lbl*, txt*, dgv*, cmb*

---

## 8. Backup & Restore

**Components**:
- Backup.cs domain model
- BackupRepository.cs (BACKUP/RESTORE commands)
- BackupBLL.cs (validation, naming)
- FrmGestionBackup.cs (UI form)

**Features**:
- Full and Differential backup types
- Automatic filename generation with timestamp
- Disk space validation
- Restore from catalog or external file
- Permissions-based access control
- Comprehensive audit logging

---

## 9. Configuration

**App.config**:
- ServicesConString: SeguridadBiblioteca
- NegocioConString: NegocioBiblioteca
- LanguagePath: Resources\I18n\idioma
- .NET Framework: 4.7.2
- Authentication: Windows/Integrated Security

---

## 10. Common Tasks

**Authenticate**:
```csharp
var usuario = LoginService.Authenticate(username, password);
```

**Check Permission**:
```csharp
if (usuario.TienePermiso("FormName")) { }
```

**Transaction**:
```csharp
using (var uow = new UnitOfWork())
{
    uow.BeginTransaction();
    try { ... uow.Commit(); }
    catch { uow.Rollback(); }
}
```

**Translate**:
```csharp
string text = LanguageManager.Translate("key");
LanguageManager.ChangeLanguage("es-AR");
```

**Export**:
```csharp
var service = new ExportService();
service.ExportarPrestamosCsv(prestamos, "path.csv");
```

---

## Summary

Professional N-tier library system with:
- Clean architecture and separation of concerns
- Advanced design patterns (Repository, UoW, Composite, Observer)
- Comprehensive security and RBAC
- Multilingual support with dynamic switching
- Transaction management for data integrity
- Professional code organization and naming

---

**Last Updated**: November 16, 2025
**Framework**: .NET Framework 4.7.2
**Language**: C#
