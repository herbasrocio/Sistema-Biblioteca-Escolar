using System;

namespace DomainModel
{
    /// <summary>
    /// Representa un préstamo de un ejemplar bibliográfico a un alumno.
    /// Gestiona el ciclo de vida del préstamo desde su creación hasta la devolución.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Un préstamo vincula un <see cref="Ejemplar"/> específico con un <see cref="Alumno"/>,
    /// registrando las fechas y el estado del préstamo. Soporta renovaciones y control de vencimientos.
    /// </para>
    /// <para>
    /// Estados posibles del préstamo:
    /// <list type="bullet">
    /// <item><description>Activo: Préstamo vigente, dentro del plazo de devolución.</description></item>
    /// <item><description>Atrasado: Préstamo vigente que superó la fecha de devolución prevista.</description></item>
    /// <item><description>Devuelto: Préstamo finalizado, el ejemplar fue devuelto.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class Prestamo
    {
        /// <summary>
        /// Obtiene o establece el identificador único del préstamo.
        /// </summary>
        /// <value>GUID que identifica de manera única el préstamo.</value>
        public Guid IdPrestamo { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del material prestado.
        /// </summary>
        /// <value>GUID del material del catálogo.</value>
        public Guid IdMaterial { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del ejemplar específico prestado.
        /// </summary>
        /// <value>GUID del ejemplar físico que se prestó.</value>
        public Guid IdEjemplar { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del alumno que realizó el préstamo.
        /// </summary>
        /// <value>GUID del alumno beneficiario del préstamo.</value>
        public Guid IdAlumno { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del usuario que registró el préstamo.
        /// </summary>
        /// <value>GUID del usuario bibliotecario que procesó el préstamo.</value>
        public Guid IdUsuario { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha en que se realizó el préstamo.
        /// </summary>
        /// <value>Fecha y hora del registro del préstamo.</value>
        public DateTime FechaPrestamo { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha prevista de devolución del ejemplar.
        /// </summary>
        /// <value>Fecha límite para devolver el ejemplar sin incurrir en atraso.</value>
        public DateTime FechaDevolucionPrevista { get; set; }

        /// <summary>
        /// Obtiene o establece el estado actual del préstamo.
        /// </summary>
        /// <value>
        /// Estado del préstamo: "Activo", "Devuelto" o "Atrasado".
        /// </value>
        public string Estado { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de veces que se renovó este préstamo.
        /// </summary>
        /// <value>Número de renovaciones realizadas. Valor inicial es 0.</value>
        public int CantidadRenovaciones { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de la última renovación del préstamo.
        /// </summary>
        /// <value>Fecha y hora de la última renovación, o <c>null</c> si nunca se renovó.</value>
        public DateTime? FechaUltimaRenovacion { get; set; }

        /// <summary>
        /// Obtiene o establece el material asociado al préstamo.
        /// </summary>
        /// <value>Objeto <see cref="Material"/> para navegación en la interfaz de usuario.</value>
        /// <remarks>Propiedad de navegación utilizada para mostrar información del material.</remarks>
        public Material Material { get; set; }

        /// <summary>
        /// Obtiene o establece el alumno asociado al préstamo.
        /// </summary>
        /// <value>Objeto <see cref="Alumno"/> para navegación en la interfaz de usuario.</value>
        /// <remarks>Propiedad de navegación utilizada para mostrar información del alumno.</remarks>
        public Alumno Alumno { get; set; }

        /// <summary>
        /// Obtiene o establece el ejemplar asociado al préstamo.
        /// </summary>
        /// <value>Objeto <see cref="Ejemplar"/> para navegación en la interfaz de usuario.</value>
        /// <remarks>Propiedad de navegación utilizada para mostrar información del ejemplar específico.</remarks>
        public Ejemplar Ejemplar { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Prestamo"/>.
        /// </summary>
        /// <remarks>
        /// El constructor genera un identificador único, establece la fecha de préstamo actual,
        /// marca el estado como "Activo" e inicializa el contador de renovaciones en cero.
        /// </remarks>
        public Prestamo()
        {
            IdPrestamo = Guid.NewGuid();
            FechaPrestamo = DateTime.Now;
            Estado = "Activo";
            CantidadRenovaciones = 0;
            FechaUltimaRenovacion = null;
        }

        /// <summary>
        /// Determina si el préstamo está atrasado comparando la fecha actual con la fecha de devolución prevista.
        /// </summary>
        /// <returns>
        /// <c>true</c> si el préstamo está activo y la fecha actual supera la fecha de devolución prevista;
        /// de lo contrario, <c>false</c>.
        /// </returns>
        public bool EstaAtrasado()
        {
            return Estado == "Activo" && DateTime.Now > FechaDevolucionPrevista;
        }

        /// <summary>
        /// Calcula los días restantes hasta la fecha de devolución prevista.
        /// </summary>
        /// <returns>
        /// Número de días restantes si el préstamo está activo. Si no está activo o está vencido, devuelve 0.
        /// </returns>
        /// <remarks>
        /// Los valores negativos indican días de atraso, pero este método devuelve 0 para préstamos no activos.
        /// </remarks>
        public int DiasRestantes()
        {
            if (Estado != "Activo") return 0;
            return (FechaDevolucionPrevista - DateTime.Now).Days;
        }

        /// <summary>
        /// Determina si el préstamo puede ser renovado según su estado y cantidad de renovaciones previas.
        /// </summary>
        /// <param name="maxRenovaciones">Número máximo de renovaciones permitidas. Por defecto es 2.</param>
        /// <returns>
        /// <c>true</c> si el préstamo está activo y no ha alcanzado el límite de renovaciones;
        /// de lo contrario, <c>false</c>.
        /// </returns>
        public bool PuedeRenovarse(int maxRenovaciones = 2)
        {
            return Estado == "Activo" && CantidadRenovaciones < maxRenovaciones;
        }
    }
}
