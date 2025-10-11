-- =====================================================
-- Script: Reinsertar Materiales con Codificación Correcta
-- Descripción: Elimina los datos corruptos y vuelve a insertar
--              los materiales con la codificación correcta
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Limpiando y reinsertando materiales con codificación correcta...'
PRINT ''

-- Eliminar los datos existentes
DELETE FROM Material;
GO

PRINT '  ✓ Materiales anteriores eliminados'
PRINT ''

-- Reinsertar los materiales con codificación correcta
PRINT 'Insertando materiales con codificación correcta...'

INSERT INTO Material (Titulo, Autor, Editorial, Tipo, Genero, CantidadTotal, CantidadDisponible) VALUES
-- Libros
(N'Don Quijote de la Mancha', N'Miguel de Cervantes', N'Editorial Planeta', N'Libro', N'Novela', 5, 3),
(N'Cien Años de Soledad', N'Gabriel García Márquez', N'Editorial Sudamericana', N'Libro', N'Novela', 3, 2),
(N'El Principito', N'Antoine de Saint-Exupéry', N'Editorial Salamandra', N'Libro', N'Fantasia', 10, 8),
(N'Drácula', N'Bram Stoker', N'Editorial Penguin', N'Libro', N'Terror', 2, 1),
(N'El Código Da Vinci', N'Dan Brown', N'Editorial Planeta', N'Libro', N'Policial', 4, 4),
(N'Romeo y Julieta', N'William Shakespeare', N'Editorial Cátedra', N'Libro', N'Drama', 6, 5),
(N'Los Miserables', N'Victor Hugo', N'Editorial Porrúa', N'Libro', N'Historico', 3, 2),
(N'Orgullo y Prejuicio', N'Jane Austen', N'Editorial Penguin', N'Libro', N'Romance', 4, 3),
(N'Crónica de una Muerte Anunciada', N'Gabriel García Márquez', N'Editorial Sudamericana', N'Libro', N'Cronica', 2, 2),
(N'Hamlet', N'William Shakespeare', N'Editorial Cátedra', N'Libro', N'Teatral', 5, 4),

-- Revistas
(N'National Geographic - Octubre 2024', N'Varios Autores', N'National Geographic Society', N'Revista', N'Historico', 2, 1),
(N'Muy Interesante - Septiembre 2024', N'Varios Autores', N'Editorial Televisa', N'Revista', N'Historico', 3, 3),
(N'Revista Ciencia Hoy', N'Varios Autores', N'CIENCIA HOY', N'Revista', N'Historico', 5, 4),

-- Manuales
(N'Manual de Gramática Española', N'Real Academia Española', N'Espasa', N'Manual', N'Historico', 8, 7),
(N'Manual de Matemáticas Secundaria', N'Varios Autores', N'Editorial Santillana', N'Manual', N'Historico', 12, 10),
(N'Manual de Ciencias Naturales', N'Varios Autores', N'Editorial Kapelusz', N'Manual', N'Historico', 10, 9);

PRINT '  ✓ 16 materiales insertados correctamente'
PRINT ''

-- Verificar los datos insertados
PRINT 'MATERIALES INSERTADOS:'
SELECT Titulo, Autor, Editorial
FROM Material
ORDER BY Titulo;
GO

PRINT ''
PRINT '=== Materiales reinsertados exitosamente con codificación correcta ==='
PRINT ''
GO
