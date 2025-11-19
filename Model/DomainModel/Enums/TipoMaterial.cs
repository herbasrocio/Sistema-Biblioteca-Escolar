namespace DomainModel.Enums
{
    /// <summary>
    /// Define los tipos de materiales bibliográficos disponibles en el catálogo de la biblioteca.
    /// </summary>
    /// <remarks>
    /// Esta enumeración se utiliza para clasificar los materiales y permite
    /// filtrar y organizar el catálogo por tipo de publicación.
    /// </remarks>
    public enum TipoMaterial
    {
        /// <summary>
        /// Material de tipo libro, publicación no periódica con contenido extenso.
        /// </summary>
        Libro,

        /// <summary>
        /// Material de tipo manual, guía práctica o instructivo sobre un tema específico.
        /// </summary>
        Manual,

        /// <summary>
        /// Material de tipo revista, publicación periódica con contenido variado.
        /// </summary>
        Revista
    }
}
