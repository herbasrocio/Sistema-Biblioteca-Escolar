-- =====================================================
-- Script: Verificar Acentos en Patentes
-- Descripción: Muestra las descripciones en formato binario
--              para verificar la codificación Unicode
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT ''
PRINT '=========================================='
PRINT '  VERIFICACIÓN DE ACENTOS EN PATENTES'
PRINT '=========================================='
PRINT ''

-- Mostrar descripciones con información de codificación
SELECT
    MenuItemName,
    Descripcion,
    LEN(Descripcion) AS Longitud,
    DATALENGTH(Descripcion) AS Bytes,
    -- Mostrar si contiene caracteres Unicode (bytes > len significa Unicode)
    CASE
        WHEN DATALENGTH(Descripcion) > LEN(Descripcion) THEN 'SI (Unicode)'
        ELSE 'NO (ASCII)'
    END AS ContieneUnicode
FROM Patente
WHERE FormName = 'menu'
ORDER BY Orden, MenuItemName

PRINT ''
PRINT '=========================================='
PRINT '  PRUEBA DE CARACTERES ESPECÍFICOS'
PRINT '=========================================='
PRINT ''

-- Verificar caracteres específicos con acento
SELECT
    MenuItemName,
    Descripcion,
    CASE
        WHEN Descripcion LIKE N'%módulo%' THEN 'OK - contiene "módulo"'
        WHEN Descripcion LIKE '%modulo%' THEN 'ERROR - solo "modulo" sin acento'
        ELSE 'No contiene "módulo"'
    END AS TestModulo,
    CASE
        WHEN Descripcion LIKE N'%Gestión%' THEN 'OK - contiene "Gestión"'
        WHEN Descripcion LIKE '%Gestion%' THEN 'ERROR - solo "Gestion" sin acento'
        ELSE 'No contiene "Gestión"'
    END AS TestGestion,
    CASE
        WHEN Descripcion LIKE N'%Catálogo%' THEN 'OK - contiene "Catálogo"'
        WHEN Descripcion LIKE '%Catalogo%' THEN 'ERROR - solo "Catalogo" sin acento'
        ELSE 'No contiene "Catálogo"'
    END AS TestCatalogo,
    CASE
        WHEN Descripcion LIKE N'%Préstamos%' THEN 'OK - contiene "Préstamos"'
        WHEN Descripcion LIKE '%Prestamos%' THEN 'ERROR - solo "Prestamos" sin acento'
        ELSE 'No contiene "Préstamos"'
    END AS TestPrestamos
FROM Patente
WHERE FormName = 'menu'
AND (Descripcion LIKE '%modulo%' OR Descripcion LIKE N'%módulo%'
     OR Descripcion LIKE '%Gestion%' OR Descripcion LIKE N'%Gestión%'
     OR Descripcion LIKE '%Catalogo%' OR Descripcion LIKE N'%Catálogo%'
     OR Descripcion LIKE '%Prestamo%' OR Descripcion LIKE N'%Préstamo%')
ORDER BY MenuItemName

PRINT ''
PRINT 'Si la columna "ContieneUnicode" muestra "SI (Unicode)" y'
PRINT 'las pruebas muestran "OK", los datos están correctos.'
PRINT 'El problema sería entonces en la aplicación o su caché.'
PRINT ''

GO
