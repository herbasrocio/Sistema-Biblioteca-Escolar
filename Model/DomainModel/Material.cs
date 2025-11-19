using System;
using DomainModel.Enums;

namespace DomainModel
{
    /// <summary>
    /// Representa un material bibliográfico del catálogo de la biblioteca.
    /// Define el concepto general del material, mientras que los ejemplares representan copias físicas individuales.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Esta clase modela el concepto abstracto de un material (ej: "El Principito" como libro genérico),
    /// diferente de sus copias físicas (ejemplares). Un material puede tener múltiples ejemplares asociados.
    /// </para>
    /// <para>
    /// Los estados individuales de disponibilidad se gestionan a nivel de cada <see cref="Ejemplar"/>.
    /// Las propiedades <see cref="CantidadTotal"/> y <see cref="CantidadDisponible"/> se calculan
    /// dinámicamente basándose en los ejemplares activos y su estado.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// Material libro = new Material
    /// {
    ///     Titulo = "El Principito",
    ///     Autor = "Antoine de Saint-Exupéry",
    ///     Tipo = TipoMaterial.Libro,
    ///     Genero = "Fantasía",
    ///     ISBN = "978-84-376-0494-7"
    /// };
    /// </code>
    /// </example>
    public class Material
    {
        /// <summary>
        /// Obtiene o establece el identificador único del material.
        /// </summary>
        /// <value>GUID que identifica de manera única el material en el catálogo.</value>
        public Guid IdMaterial { get; set; }

        /// <summary>
        /// Obtiene o establece el título del material.
        /// </summary>
        /// <value>Título completo del material bibliográfico.</value>
        public string Titulo { get; set; }

        /// <summary>
        /// Obtiene o establece el autor del material.
        /// </summary>
        /// <value>Nombre del autor o autores del material.</value>
        public string Autor { get; set; }

        /// <summary>
        /// Obtiene o establece la editorial del material.
        /// </summary>
        /// <value>Casa editorial que publicó el material.</value>
        public string Editorial { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de material.
        /// </summary>
        /// <value>Tipo de material según la enumeración <see cref="TipoMaterial"/> (Libro, Revista, Manual).</value>
        public TipoMaterial Tipo { get; set; }

        /// <summary>
        /// Obtiene o establece el género literario o temático del material.
        /// </summary>
        /// <value>Género como Fantasía, Terror, Comedia, Ciencia, etc.</value>
        public string Genero { get; set; }

        /// <summary>
        /// Obtiene o establece el código ISBN del material.
        /// </summary>
        /// <value>International Standard Book Number que identifica el material.</value>
        public string ISBN { get; set; }

        /// <summary>
        /// Obtiene o establece el año de publicación del material.
        /// </summary>
        /// <value>Año en que fue publicado el material. Puede ser nulo si se desconoce.</value>
        public int? AnioPublicacion { get; set; }

        /// <summary>
        /// Obtiene o establece el nivel educativo al que está destinado el material.
        /// </summary>
        /// <value>Nivel educativo como Inicial, Primario, Secundario, Universitario.</value>
        public string Nivel { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad total de ejemplares de este material.
        /// </summary>
        /// <value>Número total de ejemplares físicos, calculado dinámicamente desde la tabla Ejemplar.</value>
        /// <remarks>Este valor se calcula automáticamente contando los ejemplares activos asociados.</remarks>
        public int CantidadTotal { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de ejemplares disponibles para préstamo.
        /// </summary>
        /// <value>Número de ejemplares en estado disponible, calculado dinámicamente.</value>
        /// <remarks>
        /// Este valor se calcula automáticamente contando los ejemplares activos
        /// cuyo estado es <see cref="EstadoMaterial.Disponible"/>.
        /// </remarks>
        public int CantidadDisponible { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de registro del material en el sistema.
        /// </summary>
        /// <value>Fecha y hora en que se registró el material en el catálogo.</value>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Obtiene o establece si el material está activo en el sistema.
        /// </summary>
        /// <value><c>true</c> si el material está activo; <c>false</c> si ha sido dado de baja lógicamente.</value>
        public bool Activo { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Material"/>.
        /// </summary>
        /// <remarks>
        /// El constructor genera un identificador único, establece la fecha de registro actual,
        /// marca el material como activo e inicializa las cantidades en cero.
        /// </remarks>
        public Material()
        {
            IdMaterial = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
            Activo = true;
            CantidadDisponible = 0;
            CantidadTotal = 0;
        }
    }
}
