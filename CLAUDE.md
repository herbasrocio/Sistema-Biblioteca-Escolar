# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Sistema Biblioteca Escolar is a comprehensive school library management system built with C# .NET Framework 4.7.2, WinForms, and SQL Server. It follows a layered architecture with separate concerns for security, business logic, data access, and presentation.

## Build & Run Commands

### Building the Solution
```powershell
# Build entire solution (from repository root)
msbuild "Sistema Biblioteca Escolar.sln" /t:Build /p:Configuration=Debug

# Clean and rebuild
msbuild "Sistema Biblioteca Escolar.sln" /t:Clean,Build /p:Configuration=Release
```

### Running the Application
```powershell
# From repository root
cd "View\UI\bin\Debug"
.\UI.exe

# Default credentials
# Username: admin
# Password: admin123
```

### Database Setup

**Initial Setup (First Time):**
```powershell
# Security database (creates SeguridadBiblioteca)
sqlcmd -S localhost -E -i "Database\00_EJECUTAR_TODO.sql"

# Business database (creates NegocioBiblioteca)
sqlcmd -S localhost -E -i "Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql"
```

**Verify Database State:**
```powershell
sqlcmd -S localhost -E -i "Database\Negocio\00_VerificarBaseDatos.sql"
```

**Add Test Data (if database exists but empty):**
```powershell
sqlcmd -S localhost -E -i "Database\Negocio\00_VerificarYCrearDatosPrueba.sql"
```

## Architecture

### Project Structure

The solution uses a **3-tier layered architecture** with an additional Security layer:

```
Sistema Biblioteca Escolar.sln
├── Model/ (Business Logic Layer)
│   ├── DomainModel/ - Entity classes and DTOs
│   ├── DAL/ - Data Access Layer (Repository Pattern)
│   ├── BLL/ - Business Logic Layer
│   └── Services/ - Cross-cutting services (Export, etc.)
├── View/
│   └── UI/ - WinForms presentation layer
└── Security/
    └── ServicesSeguridad/ - Authentication, authorization, permissions
```

### Key Architectural Patterns

**Repository Pattern (DAL Layer):**
- All data access goes through repository interfaces in `Model/DAL/Contracts/`
- Implementations in `Model/DAL/Implementations/`
- Generic repository interface: `IGenericRepository<T>` with Add, Update, Delete, GetAll
- Adapters in `Model/DAL/Tools/` handle DataReader-to-Entity mapping

**Business Logic Layer (BLL):**
- Each entity has a corresponding BLL class (e.g., `MaterialBLL`, `AlumnoBLL`)
- BLL classes inject repository dependencies (can use constructor injection or default constructor)
- Contains validation logic before calling repository methods
- Example pattern:
  ```csharp
  public MaterialBLL(IMaterialRepository materialRepository) { }
  public MaterialBLL() : this(new MaterialRepository()) { }
  ```

**Composite Pattern (Security Layer):**
- Permissions system uses Composite pattern
- `Component` base class with `Familia` (composite) and `Patente` (leaf) implementations
- `Usuario` contains a tree of `Component` permissions
- Permission checking traverses the tree to find matching form access

### Database Architecture

**Two Separate Databases:**

1. **SeguridadBiblioteca** - Security/authentication database
   - Tables: Usuario, Familia, Patente, UsuarioFamilia, UsuarioPatente, FamiliaPatente, FamiliaFamilia
   - Handles user authentication (SHA-256 hashing), roles, and permissions
   - Digital validation with DVH (Digito Verificador Horizontal) for integrity

2. **NegocioBiblioteca** - Business/domain database
   - Tables: Material, Ejemplar, Alumno, Prestamo, Devolucion, Inscripcion, AnioLectivo, HistorialEstadoEjemplar
   - Business logic for library operations

**Connection Strings:**
Located in `View\UI\App.config`:
- `ServicesConString`: SeguridadBiblioteca
- `NegocioConString`: NegocioBiblioteca

### Domain Model Key Concepts

**Material vs Ejemplar (Important Distinction):**
- `Material`: Catalog concept (e.g., "The Little Prince" as a book title)
- `Ejemplar`: Physical copy instance (e.g., specific copy #1 with barcode "EJ001")
- One Material has many Ejemplares
- Each Ejemplar has its own state: Disponible(0), Prestado(1), Mantenimiento(2), Perdido(3)

**Inscripcion System:**
- Students are enrolled by academic year (`AnioLectivo`)
- `Inscripcion` table tracks grade/division per year
- Stored procedures handle grade promotion: `sp_PromocionarAlumnosPorGrado`, `sp_PromocionarTodosLosAlumnos`

**Prestamo Lifecycle:**
- Prestamo created with `Estado = 'Activo'`
- Devolucion records when returned (`FechaDevolucionReal`)
- Prestamo has `FechaDevolucionPrevista` for due date tracking
- RenovacionPrestamo extends loans when needed

### Internationalization (i18n)

**Translation Files:**
- Located in `View\UI\Resources\I18n\`
- Formats: `idioma.es-AR` (Spanish Argentina), `idioma.en-GB` (English UK)
- Plain text format: `key=value`
- Loaded via `LanguagePath` in App.config

**Usage Pattern:**
```csharp
// In BLL or Services layer
using ServicesSecurity.BLL;
var translator = new LanguageBLL();
string translatedText = translator.GetWord("key_name");
```

### Security & Permissions

**Permission System:**
- Each form has a FormName identifier (e.g., `consultarMaterial`, `gestionPrestamos`)
- Permissions are assigned as Patentes (individual) or Familias (groups of permissions)
- Menu items check permissions before enabling: `SessionManager.GetInstance().UsuarioActual.TienePermiso(formName)`

**Digital Validation:**
- System uses DVH (hash verification) for data integrity
- Validation handled in Security layer

**User Session:**
- `SessionManager.GetInstance().UsuarioActual` holds logged-in user
- Session persists throughout application lifetime

## Module Organization

### Administración (Administration)
- Forms in `View\UI\WinUi\Administración\`
- User management (`gestionUsuarios.cs`)
- Permission management (`gestionPermisos.cs`)
- Student management (`gestionAlumnos.cs`, `editarAlumno.cs`)
- Material catalog (`consultarMaterial.cs`, `registrarMaterial.cs`, `EditarMaterial.cs`)
- Ejemplar management (`GestionarEjemplares.cs`)
- Student promotion (`gestionPromocionAlumnos.cs`)

### Transacciones (Transactions)
- Forms in `View\UI\WinUi\Transacciones\`
- Loan management (`gestionPrestamos.cs`, `registrarPrestamo.cs`)
- Returns (`registrarDevolucion.cs`)
- Renewals (`renovarPrestamo.cs`)
- Ejemplar selection (`SeleccionarEjemplar.cs`)

### Reportes (Reports)
- Forms in `View\UI\WinUi\Reportes\`
- Active loans report (`ReportePrestamosActivos.cs`)
- Most borrowed materials (`ReporteMaterialesMasPrestados.cs`)
- Usage by grade/division (`ReporteUsoPorGrado.cs`)
- Export to CSV functionality via `ExportService.cs`
- Uses DTOs in `Model\DomainModel\DTOs\`

## Common Development Tasks

### Adding a New Form with Permissions

1. Create the form in appropriate `View\UI\WinUi\` folder
2. Add SQL script to create Patente in `Database\` with unique FormName
3. Execute SQL to add permission to database
4. Add menu item in `menu.Designer.cs`
5. Add click handler in `menu.cs` with permission check:
   ```csharp
   if (SessionManager.GetInstance().UsuarioActual.TienePermiso("yourFormName"))
   {
       // Open form
   }
   ```
6. Add translations to `idioma.es-AR` and `idioma.en-GB`

### Adding a New Entity

1. Create entity class in `Model\DomainModel\` with properties
2. Create repository interface in `Model\DAL\Contracts\`
3. Implement repository in `Model\DAL\Implementations\`
4. Create adapter in `Model\DAL\Tools\` for DataReader mapping
5. Create BLL class in `Model\BLL\` with validation logic
6. Create database table via SQL script
7. Update relevant `.csproj` files if needed

### Adding a New Report

1. Create DTO in `Model\DomainModel\DTOs\`
2. Add query method to `ReporteRepository.cs` in DAL
3. Add business method to `ReporteBLL.cs`
4. Add export method to `ExportService.cs` if needed
5. Create WinForm in `View\UI\WinUi\Reportes\`
6. Create Patente permission via SQL script
7. Add menu item and permission check
8. Add translations

### Working with SQL Scripts

**Script Naming Convention:**
- `00_*.sql` - Master execution scripts
- `01_*.sql`, `02_*.sql`, etc. - Sequential installation steps
- Numbered scripts for incremental features (e.g., `05_AgregarPermisosReportes.sql`)

**Executing Scripts:**
```powershell
# From repository root
sqlcmd -S localhost -E -i "Database\ScriptName.sql"

# For Negocio database scripts
sqlcmd -S localhost -E -i "Database\Negocio\ScriptName.sql"
```

### Validation Patterns

**BLL Validation:**
- Always validate in BLL before calling repository
- Throw descriptive exceptions for validation failures
- Example:
  ```csharp
  if (string.IsNullOrWhiteSpace(material.Titulo))
      throw new Exception("El título es obligatorio");
  ```

**Custom Exceptions:**
- Security exceptions in `Security\ServicesSeguridad\DomainModel\Exceptions\`
- Business exceptions in `Model\DomainModel\Exceptions\` (if exists)

## Project References

**UI Layer depends on:**
- Model\BLL
- Model\DAL
- Model\DomainModel
- Model\Services
- Security\ServicesSeguridad

**BLL Layer depends on:**
- Model\DAL
- Model\DomainModel

**DAL Layer depends on:**
- Model\DomainModel

**Services Layer:**
- Independent utility layer (export, etc.)

## Transaction Management

**UPDATE (Oct 27, 2025): Unit of Work pattern has been implemented** ✅

The project now uses Unit of Work pattern for critical transaction operations. See `IMPLEMENTACION_UNIT_OF_WORK.md` for complete implementation details.

### Transaction Patterns Used

The project uses **three different transaction approaches** depending on the layer:

#### 1. TransactionScope (UI Layer) - `System.Transactions`

**Location:** `View\UI\WinUi\Administración\registrarMaterial.cs:147`

Used for **multi-repository operations** that require atomicity at the UI level:

```csharp
using (TransactionScope transaction = new TransactionScope())
{
    try
    {
        _materialBLL.GuardarMaterial(nuevoMaterial);  // Repository 1

        for (int i = 1; i <= nuevoMaterial.CantidadTotal; i++)
        {
            _ejemplarBLL.GuardarEjemplar(nuevoEjemplar);  // Repository 2 (multiple times)
        }

        transaction.Complete();  // Commit only if all succeed
    }
    catch
    {
        // Automatic rollback if Complete() not called
        throw;
    }
}
```

**Characteristics:**
- ✅ Distributed transaction (requires MSDTC)
- ✅ Automatic rollback if `Complete()` not called
- ✅ Coordinates multiple repositories
- ⚠️ Only used in `registrarMaterial.cs` currently

#### 2. SqlTransaction (DAL Layer) - ADO.NET

**Location:** `Model\DAL\Implementations\PrestamoRepository.cs:344` (RenovarPrestamo method)

Used for **complex operations within a single repository**:

```csharp
using (SqlConnection conn = new SqlConnection(_connectionString))
{
    conn.Open();
    SqlTransaction transaction = conn.BeginTransaction();

    try
    {
        // 1. Read current data
        using (SqlCommand cmd = new SqlCommand(queryGetFecha, conn, transaction)) { }

        // 2. Update Prestamo
        using (SqlCommand cmd = new SqlCommand(queryUpdate, conn, transaction)) { }

        // 3. Insert into RenovacionPrestamo (audit table)
        using (SqlCommand cmd = new SqlCommand(queryInsertRenovacion, conn, transaction)) { }

        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}
```

**Characteristics:**
- ✅ Local transaction (single connection)
- ✅ Explicit `Commit()` and `Rollback()` control
- ✅ More efficient than TransactionScope for single-repository operations
- ✅ Does not require MSDTC

#### 3. Unit of Work Pattern (BLL Layer) - ✅ **IMPLEMENTED**

**Location:** `PrestamoBLL.cs`, `DevolucionBLL.cs`

**Implementation:** Critical multi-repository operations now use Unit of Work with TransactionScope for atomicity.

**Example - PrestamoBLL.RegistrarPrestamo (lines 59-138):**
```csharp
public void RegistrarPrestamo(Prestamo prestamo)
{
    // Validations...

    // TRANSACTIONAL OPERATIONS: Using Unit of Work for atomicity
    using (var uow = new UnitOfWork())
    {
        uow.BeginTransaction();
        try
        {
            // Operation 1: Update Ejemplar state
            ejemplarSeleccionado.Estado = EstadoMaterial.Prestado;
            uow.Ejemplares.Update(ejemplarSeleccionado);

            // Operation 2: Insert Prestamo
            uow.Prestamos.Add(prestamo);

            // ✅ ATOMIC COMMIT - both operations committed together
            uow.Commit();
        }
        catch
        {
            uow.Rollback(); // Automatic rollback if any operation fails
            throw;
        }
    }
}
```

**Example - DevolucionBLL.RegistrarDevolucion (lines 51-109):**
```csharp
public void RegistrarDevolucion(Devolucion devolucion)
{
    // Validations...

    // TRANSACTIONAL OPERATIONS: Using Unit of Work for atomicity
    using (var uow = new UnitOfWork())
    {
        uow.BeginTransaction();
        try
        {
            // Operation 1: Insert Devolucion
            uow.Devoluciones.Add(devolucion);

            // Operation 2: Update Prestamo state
            uow.Prestamos.ActualizarEstado(prestamo.IdPrestamo, "Devuelto");

            // Operation 3: Update Ejemplar state
            uow.Ejemplares.Update(ejemplar);

            // ✅ ATOMIC COMMIT - all 3 operations committed together
            uow.Commit();
        }
        catch
        {
            uow.Rollback(); // Automatic rollback if any operation fails
            throw;
        }
    }
}
```

### Protected Operations (Using Unit of Work)

These BLL methods now use **Unit of Work for transactional consistency**:

1. ✅ **PrestamoBLL.RegistrarPrestamo** (line 59)
   - Updates `Ejemplar.Estado` + Inserts `Prestamo`
   - **Protected:** Both operations commit atomically

2. ✅ **DevolucionBLL.RegistrarDevolucion** (line 51)
   - Inserts `Devolucion` + Updates `Prestamo.Estado` + Updates `Ejemplar.Estado`
   - **Protected:** All 3 operations commit atomically

3. ✅ **PrestamoBLL.MarcarComoDevuelto** (line 149)
   - Updates `Ejemplar.Estado` + Updates `Prestamo.Estado`
   - **Protected:** Both operations commit atomically

4. ✅ **DevolucionBLL.EliminarDevolucion** (line 123)
   - Deletes `Devolucion` + Updates `Prestamo.Estado` + Updates `Ejemplar.Estado`
   - **Protected:** All 3 operations commit atomically

### Unit of Work Implementation Details

**Interface:** `Model/DAL/Contracts/IUnitOfWork.cs`

```csharp
public interface IUnitOfWork : IDisposable
{
    IPrestamoRepository Prestamos { get; }
    IEjemplarRepository Ejemplares { get; }
    IDevolucionRepository Devoluciones { get; }
    IMaterialRepository Materiales { get; }
    IAlumnoRepository Alumnos { get; }

    void BeginTransaction();
    void Commit();
    void Rollback();
    bool HasActiveTransaction { get; }
}
```

**Implementation:** `Model/DAL/Implementations/UnitOfWork.cs`

Uses `TransactionScope` (System.Transactions) to coordinate multiple repository operations:

```csharp
public class UnitOfWork : IUnitOfWork
{
    private TransactionScope _transactionScope;

    public void BeginTransaction()
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromMinutes(2)
        };

        _transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            options,
            TransactionScopeAsyncFlowOption.Enabled
        );
    }

    public void Commit()
    {
        _transactionScope.Complete();
        _transactionScope.Dispose();
    }

    public void Rollback()
    {
        _transactionScope?.Dispose(); // Without Complete() = automatic rollback
    }
}
```

**Why TransactionScope?**
- ✅ No need to refactor existing repositories
- ✅ Automatically coordinates multiple database connections
- ✅ Automatic promotion to distributed transaction if needed
- ⚠️ Requires MSDTC (Microsoft Distributed Transaction Coordinator)

### Transaction Management Summary by Layer

| Layer | Pattern | Scope | Example | Status |
|-------|---------|-------|---------|--------|
| UI | `TransactionScope` | Multi-repository coordination | `registrarMaterial.cs` | ✅ Used (limited) |
| DAL | `SqlTransaction` | Complex single-repository ops | `RenovarPrestamo` | ✅ Used |
| BLL | `Unit of Work` (TransactionScope) | Multi-repository coordination | `RegistrarPrestamo`, `RegistrarDevolucion` | ✅ **IMPLEMENTED** |

## Important Notes

- **Unit of Work Pattern:** ✅ **NOW IMPLEMENTED** (Oct 27, 2025) - Critical transaction operations now use Unit of Work pattern. See `IMPLEMENTACION_UNIT_OF_WORK.md` for details.
- **Transaction Management:** Critical BLL operations (RegistrarPrestamo, RegistrarDevolucion, EliminarDevolucion, MarcarComoDevuelto) now use atomic transactions via Unit of Work
- **Ejemplar Tracking:** Always update Material.CantidadDisponible when Ejemplar.Estado changes
- **Stored Procedures:** Located in `Database\Negocio\06_StoredProceduresInscripcion.sql` for inscription operations
- **Hash Calculation:** Security uses SHA-256 for passwords; recalculation scripts in `Database\04_RecalcularDVH.sql`
- **ReportViewer:** Project includes Microsoft.ReportViewer.WinForms package for advanced reporting (optional feature)

## Testing

**Manual Testing Workflow:**
1. Ensure both databases are created and populated
2. Run application
3. Login with admin/admin123
4. Test specific module functionality
5. Check database state after operations

**Test Data:**
- Security DB: Default admin user created by `Database\03_DatosIniciales.sql`
- Negocio DB: 16 materials and 10 students from `Database\Negocio\03_DatosInicialesNegocio.sql`

## Recent Changes

Per git status, recent work includes:
- Reports module implementation (RESUMEN_NUEVOS_REPORTES.md, INSTRUCCIONES_MODULO_REPORTES.md)
- New DTOs in `Model\DomainModel\DTOs\`
- Export service implementation
- Menu updates for reports functionality
- Translation updates for new modules
