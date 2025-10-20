-- Script para modificar tabla Material: reemplazar EdadRecomendada por Nivel y eliminar Descripcion
-- Fecha: 2025-10-13

USE NegocioBiblioteca;
GO

-- 1. Verificar estructura actual de la tabla
SELECT 'Estructura actual:' AS Info;
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Material'
ORDER BY ORDINAL_POSITION;
GO

-- 2. Agregar columna Nivel (si no existe)
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Material'
    AND COLUMN_NAME = 'Nivel'
)
BEGIN
    ALTER TABLE Material
    ADD Nivel NVARCHAR(50) NULL;

    PRINT 'Columna Nivel agregada exitosamente';
END
ELSE
BEGIN
    PRINT 'La columna Nivel ya existe';
END
GO

-- 3. Migrar datos de EdadRecomendada a Nivel (mapeo aproximado)
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Material'
    AND COLUMN_NAME = 'EdadRecomendada'
)
BEGIN
    -- Mapear edades a niveles educativos
    UPDATE Material
    SET Nivel = CASE
        WHEN EdadRecomendada LIKE '%0-3%' OR EdadRecomendada LIKE '%4-6%' THEN 'Inicial'
        WHEN EdadRecomendada LIKE '%7-9%' OR EdadRecomendada LIKE '%10-12%' THEN 'Primario'
        WHEN EdadRecomendada LIKE '%13-15%' OR EdadRecomendada LIKE '%16-18%' THEN 'Secundario'
        WHEN EdadRecomendada LIKE '%18+%' THEN 'Universitario'
        WHEN EdadRecomendada LIKE '%Todas%' THEN NULL
        ELSE EdadRecomendada -- Mantener valor original si no coincide
    END
    WHERE Nivel IS NULL;

    PRINT 'Datos migrados de EdadRecomendada a Nivel';
END
GO

-- 4. Eliminar columna EdadRecomendada (si existe)
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Material'
    AND COLUMN_NAME = 'EdadRecomendada'
)
BEGIN
    ALTER TABLE Material
    DROP COLUMN EdadRecomendada;

    PRINT 'Columna EdadRecomendada eliminada exitosamente';
END
ELSE
BEGIN
    PRINT 'La columna EdadRecomendada no existe';
END
GO

-- 5. Eliminar columna Descripcion (si existe)
IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Material'
    AND COLUMN_NAME = 'Descripcion'
)
BEGIN
    ALTER TABLE Material
    DROP COLUMN Descripcion;

    PRINT 'Columna Descripcion eliminada exitosamente';
END
ELSE
BEGIN
    PRINT 'La columna Descripcion no existe';
END
GO

-- 6. Verificar nueva estructura de la tabla
SELECT 'Estructura actualizada:' AS Info;
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Material'
ORDER BY ORDINAL_POSITION;
GO

-- 7. Mostrar algunos datos de ejemplo
SELECT TOP 5
    'Datos de ejemplo:' AS Info,
    Titulo,
    Autor,
    Tipo,
    Genero,
    Nivel,
    AnioPublicacion
FROM Material
ORDER BY Titulo;
GO

PRINT 'Migración completada exitosamente';
