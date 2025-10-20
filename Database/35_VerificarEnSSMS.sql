-- ============================================
-- SCRIPT DE VERIFICACIÓN - EJECUTAR EN SSMS
-- ============================================
-- IMPORTANTE: Este script DEBE ejecutarse en
-- SQL Server Management Studio (SSMS) para ver
-- correctamente los acentos.
-- ============================================

USE SeguridadBiblioteca;
GO

-- Verificación completa de la tabla Familia
SELECT
    ROW_NUMBER() OVER (ORDER BY Nombre) AS [#],
    LEFT(CAST(IdFamilia AS VARCHAR(36)), 8) + '...' AS [ID],
    Nombre AS [Nombre del Rol/Familia],
    Descripcion AS [Descripción],
    FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm') AS [Fecha de Creación]
FROM Familia
ORDER BY Nombre;

PRINT ''
PRINT '================================================'
PRINT 'VERIFICACIÓN DE ACENTOS'
PRINT '================================================'
PRINT ''
PRINT 'Si ves correctamente los siguientes textos:'
PRINT '- Configuración'
PRINT '- Gestión de Permisos'
PRINT '- Gestión de Usuarios'
PRINT '- Administración'
PRINT '- catálogo, préstamos'
PRINT ''
PRINT 'Entonces la corrección fue EXITOSA.'
PRINT ''
PRINT 'Si ves símbolos raros (��), verifica que:'
PRINT '1. Estés usando SQL Server Management Studio (SSMS)'
PRINT '2. La fuente en SSMS esté configurada correctamente'
PRINT '   (Tools > Options > Fonts and Colors)'
PRINT ''
