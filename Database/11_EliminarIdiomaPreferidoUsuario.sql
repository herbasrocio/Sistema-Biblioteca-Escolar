-- =====================================================
-- Script: Eliminar columna IdiomaPreferido de Usuario
-- Descripción: Elimina la columna IdiomaPreferido ya que no es relevante
-- Fecha: 2025-11-03
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Eliminando columna IdiomaPreferido de tabla Usuario...'
PRINT ''

-- Verificar si la columna existe antes de eliminarla
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Usuario'
    AND COLUMN_NAME = 'IdiomaPreferido'
)
BEGIN
    -- Primero, eliminar el constraint DEFAULT si existe
    DECLARE @ConstraintName NVARCHAR(200);
    SELECT @ConstraintName = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON dc.parent_column_id = c.column_id
    INNER JOIN sys.tables t ON dc.parent_object_id = t.object_id
    WHERE t.name = 'Usuario' AND c.name = 'IdiomaPreferido';

    IF @ConstraintName IS NOT NULL
    BEGIN
        DECLARE @SQL NVARCHAR(500);
        SET @SQL = 'ALTER TABLE Usuario DROP CONSTRAINT ' + @ConstraintName;
        EXEC sp_executesql @SQL;
        PRINT '✓ Constraint DEFAULT eliminado';
    END

    -- Ahora eliminar la columna
    ALTER TABLE Usuario
    DROP COLUMN IdiomaPreferido;

    PRINT '✓ Columna IdiomaPreferido eliminada exitosamente';
END
ELSE
BEGIN
    PRINT '⚠ La columna IdiomaPreferido no existe';
END
GO

-- Verificar estructura actualizada de la tabla
PRINT ''
PRINT 'Estructura actualizada de tabla Usuario:'
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuario'
ORDER BY ORDINAL_POSITION;
GO

PRINT ''
PRINT '=== Script completado exitosamente ==='
PRINT ''
GO
