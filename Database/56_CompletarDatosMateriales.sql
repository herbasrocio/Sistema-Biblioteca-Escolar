-- ============================================
-- Script: 56_CompletarDatosMateriales.sql
-- Descripción: Completa datos faltantes en la tabla Material
-- Fecha: 2025-10-13
-- ============================================

USE NegocioBiblioteca
GO

PRINT 'Completando datos faltantes de Materiales...'
GO

-- Actualizar ISBN, Año de Publicación y Nivel para materiales existentes

-- Hamlet
UPDATE Material
SET ISBN = '978-0140714548',
    AnioPublicacion = 1998,
    Nivel = 'Secundario'
WHERE Titulo = 'Hamlet' AND Autor = 'William Shakespeare';

-- El Principito
UPDATE Material
SET ISBN = '978-8498381498',
    AnioPublicacion = 2010,
    Nivel = 'Primario'
WHERE Titulo = 'El Principito' AND Autor LIKE 'Antoine de Saint%';

-- Manual de Ciencias Naturales
UPDATE Material
SET ISBN = '978-9504630890',
    AnioPublicacion = 2020,
    Nivel = 'Secundario'
WHERE Titulo = 'Manual de Ciencias Naturales';

-- Los Miserables
UPDATE Material
SET ISBN = '978-9700754932',
    AnioPublicacion = 2009,
    Nivel = 'Secundario'
WHERE Titulo = 'Los Miserables' AND Autor = 'Victor Hugo';

-- Revista Ciencia Hoy
UPDATE Material
SET ISBN = 'ISSN-0327-1218',
    AnioPublicacion = 2024,
    Nivel = 'Secundario'
WHERE Titulo = 'Revista Ciencia Hoy';

-- Orgullo y Prejuicio
UPDATE Material
SET ISBN = '978-0141439518',
    AnioPublicacion = 2012,
    Nivel = 'Secundario'
WHERE Titulo = 'Orgullo y Prejuicio' AND Autor = 'Jane Austen';

-- Romeo y Julieta
UPDATE Material
SET ISBN = '978-0140707267',
    AnioPublicacion = 2000,
    Nivel = 'Secundario'
WHERE Titulo = 'Romeo y Julieta' AND Autor = 'William Shakespeare';

-- Manual de Matemáticas Secundaria
UPDATE Material
SET ISBN = '978-9504652984',
    AnioPublicacion = 2021,
    Nivel = 'Secundario'
WHERE Titulo LIKE 'Manual de Matem%';

-- National Geographic - Octubre 2024
UPDATE Material
SET ISBN = 'ISSN-1945-3027',
    AnioPublicacion = 2024,
    Nivel = 'Secundario'
WHERE Titulo LIKE 'National Geographic%';

-- Muy Interesante - Septiembre 2024
UPDATE Material
SET ISBN = 'ISSN-1665-3629',
    AnioPublicacion = 2024,
    Nivel = 'Secundario'
WHERE Titulo LIKE 'Muy Interesante%';

-- Drácula
UPDATE Material
SET ISBN = '978-0141439846',
    Nivel = 'Secundario'
WHERE Titulo LIKE 'Dr%cula' AND Autor = 'Bram Stoker';

-- Manual de Gramática Española
UPDATE Material
SET ISBN = '978-8467005004',
    AnioPublicacion = 2010,
    Nivel = 'Secundario'
WHERE Titulo LIKE 'Manual de Gram%';

-- Don Quijote de la Mancha
UPDATE Material
SET ISBN = '978-8408072713',
    AnioPublicacion = 2007,
    Nivel = 'Secundario'
WHERE Titulo = 'Don Quijote de la Mancha' AND Autor = 'Miguel de Cervantes';

PRINT 'Datos de materiales completados exitosamente.'
GO

-- Verificar resultados
SELECT
    Titulo,
    Autor,
    ISBN,
    AnioPublicacion,
    Nivel
FROM Material
ORDER BY Titulo;
GO
