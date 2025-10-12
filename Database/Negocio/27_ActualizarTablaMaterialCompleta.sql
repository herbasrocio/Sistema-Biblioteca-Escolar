-- =====================================================
-- Script: Actualizar Tabla Material
-- Descripción: Agrega las columnas ISBN, AnioPublicacion,
--              EdadRecomendada y Descripcion a la tabla Material
-- Fecha: 2025-10-11
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Actualizando tabla Material con nuevas columnas...'
PRINT ''

-- =====================================================
-- Agregar columna ISBN
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Material') AND name = 'ISBN')
BEGIN
    PRINT 'Agregando columna ISBN...'
    ALTER TABLE Material
    ADD ISBN NVARCHAR(50) NULL;
    PRINT '  ✓ Columna ISBN agregada'
END
ELSE
BEGIN
    PRINT '  ✓ Columna ISBN ya existe'
END

-- =====================================================
-- Agregar columna AnioPublicacion
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Material') AND name = 'AnioPublicacion')
BEGIN
    PRINT 'Agregando columna AnioPublicacion...'
    ALTER TABLE Material
    ADD AnioPublicacion INT NULL
    CHECK (AnioPublicacion >= 1900 AND AnioPublicacion <= 2100);
    PRINT '  ✓ Columna AnioPublicacion agregada (con validación 1900-2100)'
END
ELSE
BEGIN
    PRINT '  ✓ Columna AnioPublicacion ya existe'
END

-- =====================================================
-- Agregar columna EdadRecomendada
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Material') AND name = 'EdadRecomendada')
BEGIN
    PRINT 'Agregando columna EdadRecomendada...'
    ALTER TABLE Material
    ADD EdadRecomendada NVARCHAR(50) NULL;
    PRINT '  ✓ Columna EdadRecomendada agregada'
END
ELSE
BEGIN
    PRINT '  ✓ Columna EdadRecomendada ya existe'
END

-- =====================================================
-- Agregar columna Descripcion
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Material') AND name = 'Descripcion')
BEGIN
    PRINT 'Agregando columna Descripcion...'
    ALTER TABLE Material
    ADD Descripcion NVARCHAR(500) NULL;
    PRINT '  ✓ Columna Descripcion agregada'
END
ELSE
BEGIN
    PRINT '  ✓ Columna Descripcion ya existe'
END

PRINT ''
PRINT '=== Tabla Material Actualizada Exitosamente ==='
PRINT ''
PRINT 'RESUMEN DE COLUMNAS:'
PRINT '  • ISBN: Código ISBN del material (opcional)'
PRINT '  • AnioPublicacion: Año de publicación (1900-2100)'
PRINT '  • EdadRecomendada: Edad recomendada para el material'
PRINT '  • Descripcion: Descripción detallada del material'
PRINT ''
PRINT 'ESTRUCTURA COMPLETA DE LA TABLA MATERIAL:'
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    IS_NULLABLE AS 'Permite NULL',
    CHARACTER_MAXIMUM_LENGTH AS 'Longitud'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Material'
ORDER BY ORDINAL_POSITION;

PRINT ''
PRINT 'Actualización completada.'
PRINT ''
GO
