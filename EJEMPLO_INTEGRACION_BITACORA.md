# Ejemplo de Integración del Sistema de Bitácoras

## Cómo integrar logging en módulos existentes

---

## 1. EJEMPLO: Integración en LoginService (BitacoraAdmin)

### Archivo: `Security\ServicesSeguridad\Services\LoginService.cs`

```csharp
using ServicesSecurity.DomainModel;
using ServicesSecurity.BLL;
using ServicesSecurity.DAL.Tools;
using BLL;  // ← AGREGAR: Para acceder a BitacoraAdminBLL
using System;

namespace ServicesSecurity.Services
{
    public class LoginService
    {
        private UsuarioBLL usuarioBLL;
        private BitacoraAdminBLL bitacoraAdminBLL;  // ← AGREGAR

        public LoginService()
        {
            usuarioBLL = new UsuarioBLL();
            bitacoraAdminBLL = new BitacoraAdminBLL();  // ← AGREGAR
        }

        public Usuario Login(string nombreUsuario, string password)
        {
            try
            {
                Usuario usuario = usuarioBLL.GetUserByUserName(nombreUsuario);

                if (usuario == null)
                {
                    // ← AGREGAR LOGGING: Usuario no encontrado
                    bitacoraAdminBLL.RegistrarEventoSeguridad(
                        modulo: "Sistema de Autenticación",
                        accion: "Login fallido - Usuario no existe",
                        detalle: $"Intento de login con usuario inexistente: '{nombreUsuario}'",
                        idUsuario: null,
                        nombreUsuario: nombreUsuario,
                        criticidad: "Media"
                    );

                    throw new Exception("Usuario o contraseña incorrectos.");
                }

                // Verificar contraseña
                string hashedPassword = DigitoBLL.GetSha256Hash(password);

                if (usuario.Password != hashedPassword)
                {
                    // ← AGREGAR LOGGING: Contraseña incorrecta
                    bitacoraAdminBLL.RegistrarEventoSeguridad(
                        modulo: "Sistema de Autenticación",
                        accion: "Login fallido - Contraseña incorrecta",
                        detalle: $"Usuario '{nombreUsuario}' intentó ingresar con contraseña incorrecta",
                        idUsuario: usuario.IdUsuario,
                        nombreUsuario: nombreUsuario,
                        criticidad: "Media"
                    );

                    throw new Exception("Usuario o contraseña incorrectos.");
                }

                // ← AGREGAR LOGGING: Login exitoso
                bitacoraAdminBLL.RegistrarEventoSeguridad(
                    modulo: "Sistema de Autenticación",
                    accion: "Login exitoso",
                    detalle: $"Usuario '{nombreUsuario}' inició sesión correctamente",
                    idUsuario: usuario.IdUsuario,
                    nombreUsuario: nombreUsuario,
                    criticidad: "Baja"
                );

                return usuario;
            }
            catch (Exception ex)
            {
                // ← AGREGAR LOGGING: Error general en el proceso de login
                bitacoraAdminBLL.RegistrarError(
                    modulo: "Sistema de Autenticación",
                    accion: "Error en proceso de login",
                    detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}",
                    idUsuario: null,
                    nombreUsuario: nombreUsuario,
                    criticidad: "Alta"
                );

                throw;
            }
        }
    }
}
```

---

## 2. EJEMPLO: Integración en PrestamoBLL (BitacoraBibliotecario)

### Archivo: `Model\BLL\PrestamoBLL.cs`

```csharp
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using ServicesSecurity.Services;  // Para SessionManager
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PrestamoBLL
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IEjemplarRepository _ejemplarRepository;
        private readonly BitacoraBibliotecarioBLL _bitacoraBibliotecarioBLL;  // ← AGREGAR

        public PrestamoBLL()
        {
            _prestamoRepository = new PrestamoRepository();
            _ejemplarRepository = new EjemplarRepository();
            _bitacoraBibliotecarioBLL = new BitacoraBibliotecarioBLL();  // ← AGREGAR
        }

        public void RegistrarPrestamo(Prestamo prestamo)
        {
            // Validaciones existentes...
            if (prestamo.IdAlumno <= 0)
                throw new ArgumentException("Debe seleccionar un alumno");

            if (prestamo.IdEjemplar <= 0)
                throw new ArgumentException("Debe seleccionar un ejemplar");

            try
            {
                // Obtener el ejemplar para verificar disponibilidad
                var ejemplar = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);

                if (ejemplar == null)
                    throw new Exception("El ejemplar no existe");

                if (ejemplar.Estado != EstadoMaterial.Disponible)
                    throw new Exception("El ejemplar no está disponible");

                // TRANSACTIONAL OPERATIONS: Using Unit of Work for atomicity
                using (var uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    try
                    {
                        // Operation 1: Update Ejemplar state
                        ejemplar.Estado = EstadoMaterial.Prestado;
                        uow.Ejemplares.Update(ejemplar);

                        // Operation 2: Insert Prestamo
                        uow.Prestamos.Add(prestamo);

                        // ATOMIC COMMIT - both operations committed together
                        uow.Commit();

                        // ← AGREGAR LOGGING: Registrar préstamo en bitácora
                        var usuarioActual = SessionManager.GetInstance().UsuarioActual;

                        _bitacoraBibliotecarioBLL.RegistrarPrestamo(
                            idPrestamo: prestamo.IdPrestamo,
                            idUsuario: usuarioActual.IdUsuario,
                            nombreUsuario: usuarioActual.Nombre,
                            detalle: $"Préstamo registrado - Ejemplar: '{ejemplar.CodigoBarras}', Alumno ID: {prestamo.IdAlumno}, Fecha Devolución Prevista: {prestamo.FechaDevolucionPrevista:dd/MM/yyyy}"
                        );
                    }
                    catch (Exception ex)
                    {
                        uow.Rollback();

                        // ← AGREGAR LOGGING: Error al registrar préstamo
                        var usuarioActual = SessionManager.GetInstance().UsuarioActual;

                        var bitacoraAdminBLL = new BitacoraAdminBLL();
                        bitacoraAdminBLL.RegistrarError(
                            modulo: "Transacciones - Préstamos",
                            accion: "Error al registrar préstamo",
                            detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}\nEjemplar ID: {prestamo.IdEjemplar}, Alumno ID: {prestamo.IdAlumno}",
                            idUsuario: usuarioActual?.IdUsuario,
                            nombreUsuario: usuarioActual?.Nombre,
                            criticidad: "Alta"
                        );

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar el préstamo: {ex.Message}", ex);
            }
        }
    }
}
```

---

## 3. EJEMPLO: Integración en DevolucionBLL (BitacoraBibliotecario)

### Archivo: `Model\BLL\DevolucionBLL.cs`

```csharp
public void RegistrarDevolucion(Devolucion devolucion)
{
    // Validaciones...
    if (devolucion.IdPrestamo <= 0)
        throw new ArgumentException("Debe especificar un préstamo válido");

    try
    {
        // Obtener préstamo y ejemplar
        var prestamo = _prestamoRepository.ObtenerPorId(devolucion.IdPrestamo);
        var ejemplar = _ejemplarRepository.ObtenerPorId(prestamo.IdEjemplar);

        // Determinar el estado del ejemplar según la devolución
        if (devolucion.EstadoDevolucion == "Bueno")
            ejemplar.Estado = EstadoMaterial.Disponible;
        else if (devolucion.EstadoDevolucion == "Dañado")
            ejemplar.Estado = EstadoMaterial.Mantenimiento;
        else if (devolucion.EstadoDevolucion == "Perdido")
            ejemplar.Estado = EstadoMaterial.Perdido;

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

                // ATOMIC COMMIT - all 3 operations committed together
                uow.Commit();

                // ← AGREGAR LOGGING: Registrar devolución en bitácora
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;

                _bitacoraBibliotecarioBLL.RegistrarDevolucion(
                    idDevolucion: devolucion.IdDevolucion,
                    idUsuario: usuarioActual.IdUsuario,
                    nombreUsuario: usuarioActual.Nombre,
                    detalle: $"Devolución registrada - Ejemplar: '{ejemplar.CodigoBarras}', Estado: {devolucion.EstadoDevolucion}, Fecha Real: {devolucion.FechaDevolucionReal:dd/MM/yyyy}, Observaciones: {devolucion.Observaciones ?? "Ninguna"}"
                );
            }
            catch (Exception ex)
            {
                uow.Rollback();

                // ← AGREGAR LOGGING: Error al registrar devolución
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;

                var bitacoraAdminBLL = new BitacoraAdminBLL();
                bitacoraAdminBLL.RegistrarError(
                    modulo: "Transacciones - Devoluciones",
                    accion: "Error al registrar devolución",
                    detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}\nPrestamo ID: {devolucion.IdPrestamo}",
                    idUsuario: usuarioActual?.IdUsuario,
                    nombreUsuario: usuarioActual?.Nombre,
                    criticidad: "Alta"
                );

                throw;
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"Error al registrar la devolución: {ex.Message}", ex);
    }
}
```

---

## 4. EJEMPLO: Integración en MaterialBLL (BitacoraBibliotecario)

### Archivo: `Model\BLL\MaterialBLL.cs`

```csharp
using BLL;
using DAL.Contracts;
using DAL.Implementations;
using DomainModel;
using ServicesSecurity.Services;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class MaterialBLL
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly BitacoraBibliotecarioBLL _bitacoraBibliotecarioBLL;  // ← AGREGAR

        public MaterialBLL()
        {
            _materialRepository = new MaterialRepository();
            _bitacoraBibliotecarioBLL = new BitacoraBibliotecarioBLL();  // ← AGREGAR
        }

        public void GuardarMaterial(Material material)
        {
            // Validaciones...
            if (string.IsNullOrWhiteSpace(material.Titulo))
                throw new ArgumentException("El título es obligatorio");

            try
            {
                _materialRepository.Add(material);

                // ← AGREGAR LOGGING: Material registrado
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;

                _bitacoraBibliotecarioBLL.RegistrarOperacionMaterial(
                    accion: "Registrar nuevo material",
                    idMaterial: material.IdMaterial,
                    idUsuario: usuarioActual?.IdUsuario,
                    nombreUsuario: usuarioActual?.Nombre,
                    detalle: $"Material creado - Título: '{material.Titulo}', Tipo: {material.TipoMaterial}, Cantidad: {material.CantidadTotal}"
                );
            }
            catch (Exception ex)
            {
                // ← AGREGAR LOGGING: Error al guardar material
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;
                var bitacoraAdminBLL = new BitacoraAdminBLL();

                bitacoraAdminBLL.RegistrarError(
                    modulo: "Administración - Materiales",
                    accion: "Error al registrar material",
                    detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}\nTítulo: {material.Titulo}",
                    idUsuario: usuarioActual?.IdUsuario,
                    nombreUsuario: usuarioActual?.Nombre,
                    criticidad: "Alta"
                );

                throw new Exception($"Error al guardar el material: {ex.Message}", ex);
            }
        }

        public void ActualizarMaterial(Material material)
        {
            try
            {
                _materialRepository.Update(material);

                // ← AGREGAR LOGGING: Material actualizado
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;

                _bitacoraBibliotecarioBLL.RegistrarOperacionMaterial(
                    accion: "Actualizar material",
                    idMaterial: material.IdMaterial,
                    idUsuario: usuarioActual?.IdUsuario,
                    nombreUsuario: usuarioActual?.Nombre,
                    detalle: $"Material modificado - Título: '{material.Titulo}', Tipo: {material.TipoMaterial}"
                );
            }
            catch (Exception ex)
            {
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;
                var bitacoraAdminBLL = new BitacoraAdminBLL();

                bitacoraAdminBLL.RegistrarError(
                    modulo: "Administración - Materiales",
                    accion: "Error al actualizar material",
                    detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}\nID Material: {material.IdMaterial}",
                    idUsuario: usuarioActual?.IdUsuario,
                    nombreUsuario: usuarioActual?.Nombre,
                    criticidad: "Alta"
                );

                throw;
            }
        }

        public void EliminarMaterial(int idMaterial)
        {
            try
            {
                var material = _materialRepository.ObtenerPorId(idMaterial);

                if (material == null)
                    throw new Exception("El material no existe");

                _materialRepository.Delete(idMaterial);

                // ← AGREGAR LOGGING: Material eliminado (cambio crítico)
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;
                var bitacoraAdminBLL = new BitacoraAdminBLL();

                bitacoraAdminBLL.RegistrarCambioCritico(
                    modulo: "Administración - Materiales",
                    accion: "Eliminar material",
                    detalle: $"Material eliminado - ID: {idMaterial}, Título: '{material.Titulo}', Tipo: {material.TipoMaterial}, Cantidad Total: {material.CantidadTotal}",
                    idUsuario: usuarioActual?.IdUsuario,
                    nombreUsuario: usuarioActual?.Nombre,
                    criticidad: "Critica"
                );
            }
            catch (Exception ex)
            {
                var usuarioActual = SessionManager.GetInstance().UsuarioActual;
                var bitacoraAdminBLL = new BitacoraAdminBLL();

                bitacoraAdminBLL.RegistrarError(
                    modulo: "Administración - Materiales",
                    accion: "Error al eliminar material",
                    detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}\nID Material: {idMaterial}",
                    idUsuario: usuarioActual?.IdUsuario,
                    nombreUsuario: usuarioActual?.Nombre,
                    criticidad: "Alta"
                );

                throw;
            }
        }
    }
}
```

---

## 5. EJEMPLO: Integración en AlumnoBLL (BitacoraBibliotecario)

### Archivo: `Model\BLL\AlumnoBLL.cs`

```csharp
public void GuardarAlumno(Alumno alumno)
{
    // Validaciones...
    if (string.IsNullOrWhiteSpace(alumno.Nombre))
        throw new ArgumentException("El nombre es obligatorio");

    if (string.IsNullOrWhiteSpace(alumno.Apellido))
        throw new ArgumentException("El apellido es obligatorio");

    try
    {
        _alumnoRepository.Add(alumno);

        // ← AGREGAR LOGGING: Alumno registrado
        var usuarioActual = SessionManager.GetInstance().UsuarioActual;

        _bitacoraBibliotecarioBLL.RegistrarOperacionAlumno(
            accion: "Registrar nuevo alumno",
            idAlumno: alumno.IdAlumno,
            idUsuario: usuarioActual?.IdUsuario,
            nombreUsuario: usuarioActual?.Nombre,
            detalle: $"Alumno creado - Nombre: '{alumno.Nombre} {alumno.Apellido}', DNI: {alumno.DNI}"
        );
    }
    catch (Exception ex)
    {
        var usuarioActual = SessionManager.GetInstance().UsuarioActual;
        var bitacoraAdminBLL = new BitacoraAdminBLL();

        bitacoraAdminBLL.RegistrarError(
            modulo: "Administración - Alumnos",
            accion: "Error al registrar alumno",
            detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}\nNombre: {alumno.Nombre} {alumno.Apellido}",
            idUsuario: usuarioActual?.IdUsuario,
            nombreUsuario: usuarioActual?.Nombre,
            criticidad: "Alta"
        );

        throw;
    }
}

public void ActualizarAlumno(Alumno alumno)
{
    try
    {
        _alumnoRepository.Update(alumno);

        // ← AGREGAR LOGGING: Alumno actualizado
        var usuarioActual = SessionManager.GetInstance().UsuarioActual;

        _bitacoraBibliotecarioBLL.RegistrarOperacionAlumno(
            accion: "Actualizar alumno",
            idAlumno: alumno.IdAlumno,
            idUsuario: usuarioActual?.IdUsuario,
            nombreUsuario: usuarioActual?.Nombre,
            detalle: $"Alumno modificado - Nombre: '{alumno.Nombre} {alumno.Apellido}', DNI: {alumno.DNI}"
        );
    }
    catch (Exception ex)
    {
        var usuarioActual = SessionManager.GetInstance().UsuarioActual;
        var bitacoraAdminBLL = new BitacoraAdminBLL();

        bitacoraAdminBLL.RegistrarError(
            modulo: "Administración - Alumnos",
            accion: "Error al actualizar alumno",
            detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}\nID Alumno: {alumno.IdAlumno}",
            idUsuario: usuarioActual?.IdUsuario,
            nombreUsuario: usuarioActual?.Nombre,
            criticidad: "Alta"
        );

        throw;
    }
}
```

---

## 6. PATRÓN GENERAL DE INTEGRACIÓN

### Template para agregar logging en cualquier método:

```csharp
public void MiMetodo(params)
{
    try
    {
        // 1. Validaciones

        // 2. Operación principal
        _repository.HacerAlgo();

        // 3. ← AGREGAR LOGGING: Operación exitosa
        var usuarioActual = SessionManager.GetInstance().UsuarioActual;

        // Si es operación de negocio → BitacoraBibliotecarioBLL
        _bitacoraBibliotecarioBLL.RegistrarOperacion(...);

        // Si es cambio crítico → BitacoraAdminBLL
        // var bitacoraAdminBLL = new BitacoraAdminBLL();
        // bitacoraAdminBLL.RegistrarCambioCritico(...);
    }
    catch (Exception ex)
    {
        // 4. ← AGREGAR LOGGING: Error
        var usuarioActual = SessionManager.GetInstance().UsuarioActual;
        var bitacoraAdminBLL = new BitacoraAdminBLL();

        bitacoraAdminBLL.RegistrarError(
            modulo: "Nombre del Módulo",
            accion: "Descripción del error",
            detalle: $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}",
            idUsuario: usuarioActual?.IdUsuario,
            nombreUsuario: usuarioActual?.Nombre,
            criticidad: "Alta"
        );

        throw;
    }
}
```

---

## 7. CRITERIOS PARA ELEGIR QUÉ BITÁCORA USAR

### Usar BitacoraAdmin cuando:
- ❌ Errores del sistema (exceptions)
- 🔒 Eventos de seguridad (login, permisos)
- ⚠️ Cambios críticos (eliminaciones, modificaciones importantes)
- 🛡️ Intentos de acceso no autorizado

### Usar BitacoraBibliotecario cuando:
- ✅ Operaciones de negocio exitosas
- 📚 Préstamos, devoluciones, renovaciones
- 📝 CRUD de materiales, alumnos, ejemplares
- 🔍 Consultas importantes

### Criticidad (BitacoraAdmin):
- **Baja**: Login exitoso, consultas
- **Media**: Login fallido, validaciones
- **Alta**: Errores del sistema, excepciones
- **Critica**: Eliminaciones, cambios irreversibles

---

## 8. NOTAS IMPORTANTES

### SessionManager:
```csharp
// Obtener usuario actual
var usuarioActual = SessionManager.GetInstance().UsuarioActual;

// Usar sus propiedades
Guid idUsuario = usuarioActual.IdUsuario;
string nombreUsuario = usuarioActual.Nombre;
```

### Validación de nulos:
```csharp
// Siempre usar operador condicional (?) para evitar NullReferenceException
idUsuario: usuarioActual?.IdUsuario,
nombreUsuario: usuarioActual?.Nombre
```

### Try-catch en BLL:
```csharp
// Siempre capturar exceptions en BLL
// Registrar en bitácora ANTES de lanzar la excepción
catch (Exception ex)
{
    // Logging aquí
    throw;  // ← Re-lanzar para que la UI la maneje
}
```

---

**Autor: Sistema Biblioteca Escolar**
**Fecha: 2025-10-28**
