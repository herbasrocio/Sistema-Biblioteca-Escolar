namespace DomainModel.Enums
{
    /// <summary>
    /// Define los estados posibles de un ejemplar físico en la biblioteca.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Esta enumeración representa el estado actual de disponibilidad de un ejemplar específico.
    /// El valor ordinal se utiliza en consultas SQL para filtrar ejemplares disponibles.
    /// </para>
    /// <para>
    /// IMPORTANTE: El valor 0 (Disponible) se usa en las consultas SQL del repositorio
    /// para calcular cantidades de ejemplares disponibles.
    /// </para>
    /// </remarks>
    public enum EstadoMaterial
    {
        /// <summary>
        /// El ejemplar está disponible para préstamo. Valor ordinal: 0.
        /// </summary>
        Disponible,

        /// <summary>
        /// El ejemplar está actualmente prestado a un alumno. Valor ordinal: 1.
        /// </summary>
        Prestado,

        /// <summary>
        /// El ejemplar está en reparación o mantenimiento. Valor ordinal: 2.
        /// </summary>
        EnReparacion,

        /// <summary>
        /// El ejemplar no está disponible por otros motivos (extraviado, dañado irreparablemente, etc.). Valor ordinal: 3.
        /// </summary>
        NoDisponible
    }
}
