-- =====================================================
-- Script: Actualizar Materiales Directamente
-- Descripción: Actualiza cada material individualmente con los valores correctos
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Actualizando materiales con valores correctos...'
PRINT ''

-- Actualizar cada libro individualmente
UPDATE Material SET Titulo = N'Cien Años de Soledad', Autor = N'Gabriel García Márquez' WHERE Titulo LIKE '%Cien A%';
UPDATE Material SET Titulo = N'Crónica de una Muerte Anunciada', Autor = N'Gabriel García Márquez' WHERE Titulo LIKE '%Cr%nica%';
UPDATE Material SET Titulo = N'Drácula' WHERE Titulo LIKE '%Dr%cula%';
UPDATE Material SET Titulo = N'El Código Da Vinci' WHERE Titulo LIKE '%C%digo%';
UPDATE Material SET Autor = N'Antoine de Saint-Exupéry' WHERE Autor LIKE '%Exup%';
UPDATE Material SET Editorial = N'Editorial Cátedra' WHERE Editorial LIKE '%C%tedra%';
UPDATE Material SET Editorial = N'Editorial Porrúa' WHERE Editorial LIKE '%Porr%';
UPDATE Material SET Titulo = N'Manual de Gramática Española', Autor = N'Real Academia Española' WHERE Titulo LIKE '%Gram%tica%';
UPDATE Material SET Titulo = N'Manual de Matemáticas Secundaria' WHERE Titulo LIKE '%Matem%ticas%';

GO

-- Verificar los resultados
PRINT 'MATERIALES ACTUALIZADOS:'
SELECT Titulo, Autor, Editorial
FROM Material
ORDER BY Titulo;
GO

PRINT ''
PRINT '=== Materiales actualizados exitosamente ==='
PRINT ''
GO
