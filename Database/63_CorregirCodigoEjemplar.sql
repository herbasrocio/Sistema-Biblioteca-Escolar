-- =============================================
-- Script: Corregir columna CodigoEjemplar
-- Descripción: Verifica y corrige el nombre de la columna
-- =============================================

USE NegocioBiblioteca
GO

PRINT '===== VERIFICANDO ESTADO ACTUAL ====='

-- Verificar si existe CodigoBarras
IF EXISTS (SELECT * FROM sys.columns
           WHERE object_id = OBJECT_ID(N'dbo.Ejemplar')
           AND name = 'CodigoBarras')
BEGIN
    PRINT 'Encontrada columna: CodigoBarras (ANTIGUO)'

    -- Verificar si ya existe CodigoEjemplar
    IF EXISTS (SELECT * FROM sys.columns
               WHERE object_id = OBJECT_ID(N'dbo.Ejemplar')
               AND name = 'CodigoEjemplar')
    BEGIN
        PRINT 'ERROR: Ambas columnas existen. Eliminando CodigoBarras...'
        ALTER TABLE dbo.Ejemplar DROP COLUMN CodigoBarras
        PRINT 'Columna CodigoBarras eliminada.'
    END
    ELSE
    BEGIN
        PRINT 'Renombrando CodigoBarras a CodigoEjemplar...'
        EXEC sp_rename 'dbo.Ejemplar.CodigoBarras', 'CodigoEjemplar', 'COLUMN'
        PRINT 'Columna renombrada exitosamente.'
    END
END
ELSE IF EXISTS (SELECT * FROM sys.columns
                WHERE object_id = OBJECT_ID(N'dbo.Ejemplar')
                AND name = 'CodigoEjemplar')
BEGIN
    PRINT 'Columna CodigoEjemplar ya existe (CORRECTO)'
END
ELSE
BEGIN
    PRINT 'ERROR: No existe ni CodigoBarras ni CodigoEjemplar'
    PRINT 'Agregando columna CodigoEjemplar...'
    ALTER TABLE dbo.Ejemplar
    ADD CodigoEjemplar NVARCHAR(100) NULL
    PRINT 'Columna CodigoEjemplar agregada.'
END

-- Verificar y renombrar constraint si existe
IF EXISTS (SELECT * FROM sys.indexes
           WHERE name = 'UQ_Ejemplar_CodigoBarras'
           AND object_id = OBJECT_ID(N'dbo.Ejemplar'))
BEGIN
    PRINT 'Renombrando constraint UQ_Ejemplar_CodigoBarras...'
    EXEC sp_rename 'dbo.UQ_Ejemplar_CodigoBarras', 'UQ_Ejemplar_CodigoEjemplar', 'INDEX'
    PRINT 'Constraint renombrado exitosamente.'
END

PRINT '===== VERIFICACIÓN FINAL ====='
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Ejemplar'
  AND COLUMN_NAME IN ('CodigoBarras', 'CodigoEjemplar')

PRINT '===== PROCESO COMPLETADO ====='
GO
