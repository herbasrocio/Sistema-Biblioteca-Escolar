-- ============================================
-- Script: 59_CrearEjemplaresFaltantes.sql
-- Descripción: Crea ejemplares faltantes para materiales que tienen CantidadTotal > ejemplares registrados
-- Fecha: 2025-10-13
-- ============================================

USE NegocioBiblioteca
GO

PRINT 'Creando ejemplares faltantes...'
GO

-- Declarar variables
DECLARE @IdMaterial UNIQUEIDENTIFIER
DECLARE @Titulo NVARCHAR(500)
DECLARE @Tipo NVARCHAR(50)
DECLARE @Genero NVARCHAR(100)
DECLARE @CantidadTotal INT
DECLARE @EjemplaresExistentes INT
DECLARE @NumeroEjemplar INT
DECLARE @CodigoBarras NVARCHAR(50)
DECLARE @Ubicacion NVARCHAR(200)
DECLARE @Observaciones NVARCHAR(500)

-- ========================================
-- CENICIENTA - 10 ejemplares
-- ========================================
SET @IdMaterial = '6420BE10-679E-4A6E-B1F8-04C57D8B068D'
SET @Titulo = 'Cenicienta'
SET @CantidadTotal = 10

PRINT 'Creando 10 ejemplares para: ' + @Titulo

-- Generar ejemplares del 1 al 10
DECLARE @i INT = 1
WHILE @i <= @CantidadTotal
BEGIN
    SET @NumeroEjemplar = @i
    SET @CodigoBarras = 'BIB-' + RIGHT(CONVERT(VARCHAR(36), @IdMaterial), 8) + '-' + RIGHT('000' + CAST(@NumeroEjemplar AS VARCHAR(3)), 3)
    SET @Ubicacion = 'Estanteria 01 - Fantasia'
    SET @Observaciones = 'Ejemplar nuevo - Ejemplar en buen estado, disponible para prestamo'

    INSERT INTO Ejemplar (IdEjemplar, IdMaterial, NumeroEjemplar, CodigoBarras, Estado, Ubicacion, Observaciones, FechaRegistro, Activo)
    VALUES (NEWID(), @IdMaterial, @NumeroEjemplar, @CodigoBarras, 0, @Ubicacion, @Observaciones, GETDATE(), 1)

    SET @i = @i + 1
END

PRINT 'Ejemplares de Cenicienta creados.'
GO

-- ========================================
-- CIEN AÑOS DE SOLEDAD - 1 ejemplar faltante (ya tiene 1, necesita 1 más)
-- ========================================
DECLARE @IdMaterial UNIQUEIDENTIFIER = 'D1EBC6EF-ACB8-4426-94FE-4053FACAE4C3'
DECLARE @NumeroEjemplar INT = 2
DECLARE @CodigoBarras NVARCHAR(50)
DECLARE @Ubicacion NVARCHAR(200) = 'Estanteria 04 - Drama'
DECLARE @Observaciones NVARCHAR(500) = 'Ejemplar en buen estado, disponible para prestamo'

SET @CodigoBarras = 'BIB-' + RIGHT(CONVERT(VARCHAR(36), @IdMaterial), 8) + '-' + RIGHT('000' + CAST(@NumeroEjemplar AS VARCHAR(3)), 3)

INSERT INTO Ejemplar (IdEjemplar, IdMaterial, NumeroEjemplar, CodigoBarras, Estado, Ubicacion, Observaciones, FechaRegistro, Activo)
VALUES (NEWID(), @IdMaterial, @NumeroEjemplar, @CodigoBarras, 0, @Ubicacion, @Observaciones, GETDATE(), 1)

PRINT 'Ejemplar faltante de Cien Años de Soledad creado.'
GO

-- ========================================
-- PINOCHO - 5 ejemplares
-- ========================================
DECLARE @IdMaterial UNIQUEIDENTIFIER
DECLARE @CantidadTotal INT
DECLARE @NumeroEjemplar INT
DECLARE @CodigoBarras NVARCHAR(50)
DECLARE @Ubicacion NVARCHAR(200)
DECLARE @Observaciones NVARCHAR(500)

SET @IdMaterial = 'EBFCB9D5-12A1-4719-819C-6B3704BEFBFA'
SET @CantidadTotal = 5

PRINT 'Creando 5 ejemplares para: Pinocho'

-- Generar ejemplares del 1 al 5
DECLARE @j INT = 1
WHILE @j <= @CantidadTotal
BEGIN
    SET @NumeroEjemplar = @j
    SET @CodigoBarras = 'BIB-' + RIGHT(CONVERT(VARCHAR(36), @IdMaterial), 8) + '-' + RIGHT('000' + CAST(@NumeroEjemplar AS VARCHAR(3)), 3)
    SET @Ubicacion = 'Estanteria 02 - Romance'
    SET @Observaciones = 'Ejemplar nuevo - Ejemplar en buen estado, disponible para prestamo'

    INSERT INTO Ejemplar (IdEjemplar, IdMaterial, NumeroEjemplar, CodigoBarras, Estado, Ubicacion, Observaciones, FechaRegistro, Activo)
    VALUES (NEWID(), @IdMaterial, @NumeroEjemplar, @CodigoBarras, 0, @Ubicacion, @Observaciones, GETDATE(), 1)

    SET @j = @j + 1
END

PRINT 'Ejemplares de Pinocho creados.'
GO

-- Verificar resultados
PRINT 'Verificación de ejemplares creados:'
GO

SELECT
    m.Titulo,
    m.CantidadTotal,
    COUNT(e.IdEjemplar) AS EjemplaresRegistrados,
    CASE
        WHEN m.CantidadTotal = COUNT(e.IdEjemplar) THEN 'OK'
        ELSE 'FALTA'
    END AS Estado
FROM Material m
LEFT JOIN Ejemplar e ON m.IdMaterial = e.IdMaterial
WHERE m.Titulo IN ('Cenicienta', 'Cien Años de Soledad', 'Pinocho')
GROUP BY m.Titulo, m.CantidadTotal
ORDER BY m.Titulo
GO

PRINT 'Proceso completado exitosamente.'
GO
