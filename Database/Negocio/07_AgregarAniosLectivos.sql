-- =====================================================
-- Script: Agregar Años Lectivos Faltantes
-- Descripción: Agrega años lectivos desde 2020 hasta 2030
--              para permitir inscripciones y promociones
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Agregando años lectivos...'
PRINT ''

-- Verificar que existe la tabla AnioLectivo
IF OBJECT_ID('AnioLectivo', 'U') IS NULL
BEGIN
    PRINT 'ERROR: La tabla AnioLectivo no existe.'
    RETURN
END

-- Insertar años lectivos desde 2020 hasta 2030
DECLARE @Anio INT = 2020
DECLARE @AnioFin INT = 2030
DECLARE @Insertados INT = 0

WHILE @Anio <= @AnioFin
BEGIN
    -- Verificar si el año ya existe
    IF NOT EXISTS (SELECT 1 FROM AnioLectivo WHERE Anio = @Anio)
    BEGIN
        -- Calcular fechas de inicio y fin del año lectivo (marzo a diciembre)
        DECLARE @FechaInicio DATE = CAST(CAST(@Anio AS VARCHAR) + '-03-01' AS DATE)
        DECLARE @FechaFin DATE = CAST(CAST(@Anio AS VARCHAR) + '-12-31' AS DATE)

        INSERT INTO AnioLectivo (Anio, FechaInicio, FechaFin, Estado)
        VALUES (@Anio, @FechaInicio, @FechaFin, 'Activo')

        PRINT '  ✓ Año ' + CAST(@Anio AS VARCHAR) + ' agregado'
        SET @Insertados = @Insertados + 1
    END
    ELSE
    BEGIN
        PRINT '  - Año ' + CAST(@Anio AS VARCHAR) + ' ya existe'
    END

    SET @Anio = @Anio + 1
END

PRINT ''
PRINT '======================================'
PRINT 'Años lectivos agregados: ' + CAST(@Insertados AS VARCHAR)
PRINT '======================================'
PRINT ''

-- Mostrar todos los años lectivos disponibles
PRINT 'Años lectivos disponibles:'
SELECT Anio, FechaInicio, FechaFin, Estado
FROM AnioLectivo
ORDER BY Anio

GO
