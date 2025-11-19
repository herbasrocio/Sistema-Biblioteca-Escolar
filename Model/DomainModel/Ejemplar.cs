using System;
using DomainModel.Enums;

namespace DomainModel
{
    /// <summary>
    /// Representa una copia física individual de un material bibliográfico.
    /// Cada ejemplar tiene su propio estado, ubicación y seguimiento independiente.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Esta clase implementa el concepto de ejemplar como instancia física de un <see cref="Material"/>.
    /// Mientras que un Material representa el concepto abstracto (ej: "El Principito"), un Ejemplar
    /// representa cada copia física individual que puede ser prestada.
    /// </para>
    /// <para>
    /// Cada ejemplar tiene un código único que lo identifica físicamente en la biblioteca,
    /// permitiendo el seguimiento individual de cada copia.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// Ejemplar ejemplar = new Ejemplar
    /// {
    ///     IdMaterial = materialId,
    ///     NumeroEjemplar = 1,
    ///     CodigoEjemplar = "LIB-0001-001",
    ///     Ubicacion = "Estante A3",
    ///     Estado = EstadoMaterial.Disponible
    /// };
    /// </code>
    /// </example>
    public class Ejemplar
    {
        /// <summary>
        /// Obtiene o establece el identificador único del ejemplar.
        /// </summary>
        /// <value>GUID que identifica de manera única este ejemplar en el sistema.</value>
        public Guid IdEjemplar { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del material al que pertenece este ejemplar.
        /// </summary>
        /// <value>GUID del <see cref="Material"/> del cual esta es una copia física.</value>
        public Guid IdMaterial { get; set; }

        /// <summary>
        /// Obtiene o establece el número secuencial del ejemplar dentro del material.
        /// </summary>
        /// <value>Número entero secuencial (1, 2, 3, etc.) que identifica este ejemplar entre las copias del material.</value>
        public int NumeroEjemplar { get; set; }

        /// <summary>
        /// Obtiene o establece el código único del ejemplar.
        /// </summary>
        /// <value>
        /// Código alfanumérico único que se imprime en la etiqueta física del ejemplar
        /// para su identificación en la biblioteca (ej: "LIB-0001-001").
        /// </value>
        public string CodigoEjemplar { get; set; }

        /// <summary>
        /// Obtiene o establece el estado actual del ejemplar.
        /// </summary>
        /// <value>Estado según la enumeración <see cref="EstadoMaterial"/>.</value>
        public EstadoMaterial Estado { get; set; }

        /// <summary>
        /// Obtiene o establece la ubicación física del ejemplar en la biblioteca.
        /// </summary>
        /// <value>Descripción de la ubicación física (ej: "Estante A3", "Sala Infantil B2").</value>
        public string Ubicacion { get; set; }

        /// <summary>
        /// Obtiene o establece observaciones adicionales sobre el ejemplar.
        /// </summary>
        /// <value>Texto libre con observaciones, notas sobre el estado físico, marcas, etc.</value>
        public string Observaciones { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de registro del ejemplar en el sistema.
        /// </summary>
        /// <value>Fecha y hora en que se registró este ejemplar.</value>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Obtiene o establece si el ejemplar está activo en el sistema.
        /// </summary>
        /// <value><c>true</c> si el ejemplar está activo; <c>false</c> si ha sido dado de baja.</value>
        public bool Activo { get; set; }

        /// <summary>
        /// Obtiene o establece el material asociado a este ejemplar.
        /// </summary>
        /// <value>Objeto <see cref="Material"/> para navegación en la interfaz de usuario.</value>
        /// <remarks>
        /// Propiedad de navegación que no se mapea directamente desde la base de datos.
        /// Se utiliza para mostrar información del material al que pertenece el ejemplar.
        /// </remarks>
        public Material Material { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Ejemplar"/>.
        /// </summary>
        /// <remarks>
        /// El constructor genera un identificador único, establece la fecha de registro actual,
        /// marca el ejemplar como activo y lo establece en estado Disponible por defecto.
        /// </remarks>
        public Ejemplar()
        {
            IdEjemplar = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            Estado = EstadoMaterial.Disponible;
        }
    }
}
