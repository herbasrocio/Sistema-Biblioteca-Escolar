-- =====================================================
-- Script: Datos Iniciales de Negocio
-- Descripción: Inserta datos de ejemplo para materiales y alumnos
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Insertando datos iniciales de negocio...'
PRINT ''

-- =====================================================
-- MATERIALES DE EJEMPLO
-- =====================================================
PRINT 'Insertando materiales de ejemplo...'

INSERT INTO Material (Titulo, Autor, Editorial, Tipo, Genero, CantidadTotal, CantidadDisponible) VALUES
-- Libros
('Don Quijote de la Mancha', 'Miguel de Cervantes', 'Editorial Planeta', 'Libro', 'Novela', 5, 3),
('Cien Años de Soledad', 'Gabriel García Márquez', 'Editorial Sudamericana', 'Libro', 'Novela', 3, 2),
('El Principito', 'Antoine de Saint-Exupéry', 'Editorial Salamandra', 'Libro', 'Fantasia', 10, 8),
('Drácula', 'Bram Stoker', 'Editorial Penguin', 'Libro', 'Terror', 2, 1),
('El Código Da Vinci', 'Dan Brown', 'Editorial Planeta', 'Libro', 'Policial', 4, 4),
('Romeo y Julieta', 'William Shakespeare', 'Editorial Cátedra', 'Libro', 'Drama', 6, 5),
('Los Miserables', 'Victor Hugo', 'Editorial Porrúa', 'Libro', 'Historico', 3, 2),
('Orgullo y Prejuicio', 'Jane Austen', 'Editorial Penguin', 'Libro', 'Romance', 4, 3),
('Crónica de una Muerte Anunciada', 'Gabriel García Márquez', 'Editorial Sudamericana', 'Libro', 'Cronica', 2, 2),
('Hamlet', 'William Shakespeare', 'Editorial Cátedra', 'Libro', 'Teatral', 5, 4),

-- Revistas
('National Geographic - Octubre 2024', 'Varios Autores', 'National Geographic Society', 'Revista', 'Historico', 2, 1),
('Muy Interesante - Septiembre 2024', 'Varios Autores', 'Editorial Televisa', 'Revista', 'Historico', 3, 3),
('Revista Ciencia Hoy', 'Varios Autores', 'CIENCIA HOY', 'Revista', 'Historico', 5, 4),

-- Manuales
('Manual de Gramática Española', 'Real Academia Española', 'Espasa', 'Manual', 'Historico', 8, 7),
('Manual de Matemáticas Secundaria', 'Varios Autores', 'Editorial Santillana', 'Manual', 'Historico', 12, 10),
('Manual de Ciencias Naturales', 'Varios Autores', 'Editorial Kapelusz', 'Manual', 'Historico', 10, 9);

PRINT '  ✓ 16 materiales insertados'

-- =====================================================
-- ALUMNOS DE EJEMPLO
-- =====================================================
PRINT 'Insertando alumnos de ejemplo...'

INSERT INTO Alumno (Nombre, Apellido, DNI, Grado, Division) VALUES
('Juan', 'Pérez', '12345678', '1° Año', 'A'),
('María', 'González', '23456789', '1° Año', 'A'),
('Carlos', 'Rodríguez', '34567890', '2° Año', 'B'),
('Ana', 'Martínez', '45678901', '2° Año', 'A'),
('Luis', 'García', '56789012', '3° Año', 'C'),
('Laura', 'López', '67890123', '3° Año', 'B'),
('Pedro', 'Fernández', '78901234', '4° Año', 'A'),
('Sofía', 'Díaz', '89012345', '4° Año', 'B'),
('Diego', 'Sánchez', '90123456', '5° Año', 'A'),
('Valentina', 'Romero', '01234567', '5° Año', 'C');

PRINT '  ✓ 10 alumnos insertados'

PRINT ''
PRINT '=== Datos Iniciales Cargados Exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • 16 materiales en catálogo (10 libros, 3 revistas, 3 manuales)'
PRINT '  • 10 alumnos registrados'
PRINT '  • Géneros variados: Novela, Fantasía, Terror, Drama, etc.'
PRINT ''
PRINT 'La base de datos está lista para usar.'
PRINT ''
GO
