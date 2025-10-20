/*
Script de verificación de acentos en tabla Familia
EJECUTAR EN SQL SERVER MANAGEMENT STUDIO (SSMS) para ver correctamente los acentos
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'VERIFICACIÓN DE ACENTOS EN TABLA FAMILIA'
PRINT '================================================'
PRINT ''

-- Consulta completa con formato
SELECT
    LEFT(IdFamilia, 8) + '...' AS ID,
    Nombre AS [Nombre del Rol],
    Descripcion AS [Descripción],
    FechaCreacion AS [Fecha de Creación]
FROM Familia
ORDER BY Nombre;

PRINT ''
PRINT '================================================'
PRINT 'VERIFICACIÓN COMPLETADA'
PRINT '================================================'
PRINT ''
PRINT 'Si ves acentos correctamente en SSMS, la corrección'
PRINT 'fue exitosa. Si ves símbolos raros, es un problema'
PRINT 'de la herramienta de consulta, no de la base de datos.'
PRINT ''

GO
