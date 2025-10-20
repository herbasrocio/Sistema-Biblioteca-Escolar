-- =============================================
-- Script: Renombrar CodigoBarras a CodigoEjemplar
-- Descripción: Cambia el nombre de la columna para reflejar mejor su propósito
--              (código escrito manualmente, no código de barras escaneado)
-- Fecha: 2025-10-19
-- =============================================

USE NegocioBiblioteca
GO

-- Verificar que la columna CodigoBarras existe
IF EXISTS (SELECT * FROM sys.columns
           WHERE object_id = OBJECT_ID(N'dbo.Ejemplar')
           AND name = 'CodigoBarras')
BEGIN
    PRINT 'Renombrando columna CodigoBarras a CodigoEjemplar...'

    -- Renombrar columna
    EXEC sp_rename 'dbo.Ejemplar.CodigoBarras', 'CodigoEjemplar', 'COLUMN'

    PRINT 'Columna renombrada exitosamente.'

    -- Verificar si existe constraint de unicidad con el nombre anterior
    IF EXISTS (SELECT * FROM sys.indexes
               WHERE name = 'UQ_Ejemplar_CodigoBarras'
               AND object_id = OBJECT_ID(N'dbo.Ejemplar'))
    BEGIN
        -- Renombrar constraint de unicidad
        EXEC sp_rename 'dbo.UQ_Ejemplar_CodigoBarras', 'UQ_Ejemplar_CodigoEjemplar', 'INDEX'
        PRINT 'Constraint de unicidad renombrado exitosamente.'
    END

    PRINT '=============================================
    PRINT 'Migración completada exitosamente.'
    PRINT 'CodigoBarras → CodigoEjemplar'
    PRINT '=============================================
END
ELSE
BEGIN
    PRINT 'La columna CodigoBarras no existe o ya fue renombrada.'

    -- Verificar si CodigoEjemplar ya existe
    IF EXISTS (SELECT * FROM sys.columns
               WHERE object_id = OBJECT_ID(N'dbo.Ejemplar')
               AND name = 'CodigoEjemplar')
    BEGIN
        PRINT 'La columna CodigoEjemplar ya existe. No se requiere acción.'
    END
END
GO
