using System;

namespace DomainModel
{
    /// <summary>
    /// Entidad que representa un registro de bitácora de bibliotecario.
    /// Almacena operaciones de negocio: préstamos, devoluciones, consultas, etc.
    /// </summary>
    public class BitacoraBibliotecario
    {
        public int IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public Guid? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoOperacion { get; set; } // 'Prestamo', 'Devolucion', 'Renovacion', 'ConsultaMaterial', etc.
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string EntidadAfectada { get; set; } // 'Material', 'Ejemplar', 'Alumno', 'Prestamo', etc.
        public int? IdEntidad { get; set; }
        public string Detalle { get; set; }

        public BitacoraBibliotecario()
        {
            Fecha = DateTime.Now;
        }
    }

    /// <summary>
    /// Enumeración para tipos de operaciones de bibliotecario
    /// </summary>
    public enum TipoOperacionBibliotecario
    {
        Prestamo,
        Devolucion,
        Renovacion,
        ConsultaMaterial,
        RegistrarMaterial,
        EditarMaterial,
        EliminarMaterial,
        GestionAlumno,
        GestionEjemplar,
        ConsultaGeneral
    }
}
