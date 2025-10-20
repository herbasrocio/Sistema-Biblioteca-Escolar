/*
Script de diagnóstico para identificar exactamente qué caracteres están mal codificados
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'DIAGNÓSTICO DE CARACTERES MAL CODIFICADOS'
PRINT '================================================'
PRINT ''

-- Ver los bytes exactos de cada carácter en hexadecimal
SELECT
    Nombre,
    Descripcion,
    -- Mostrar los bytes en hexadecimal para análisis
    CAST(Nombre AS VARBINARY(200)) AS Nombre_HEX,
    CAST(Descripcion AS VARBINARY(500)) AS Descripcion_HEX
FROM Familia
WHERE Nombre LIKE '%Gesti%' OR Nombre LIKE '%Configuraci%';

PRINT ''
PRINT '================================================'
PRINT 'INFORMACIÓN ADICIONAL'
PRINT '================================================'

-- Ver la collation actual de las columnas
SELECT
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    COLLATION_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Familia'
    AND COLUMN_NAME IN ('Nombre', 'Descripcion');

GO
