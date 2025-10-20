-- ============================================
-- Script: 57_CompletarDatosEjemplares.sql
-- Descripción: Completa datos faltantes en la tabla Ejemplar
-- Genera códigos de barras, ubicaciones y observaciones
-- Fecha: 2025-10-13
-- ============================================

USE NegocioBiblioteca
GO

PRINT 'Completando datos faltantes de Ejemplares...'
GO

-- Actualizar CodigoBarras para ejemplares que no lo tienen
-- Formato: BIB-[últimos 8 dígitos del IdMaterial]-[número de ejemplar con 3 dígitos]
UPDATE Ejemplar
SET CodigoBarras = 'BIB-' +
    RIGHT(CONVERT(VARCHAR(36), IdMaterial), 8) + '-' +
    RIGHT('000' + CAST(NumeroEjemplar AS VARCHAR(3)), 3)
WHERE CodigoBarras IS NULL;

PRINT 'Códigos de barras generados.'
GO

-- Actualizar Ubicación según el tipo de material
-- Libros: Estantería A-F según género
-- Manuales: Estantería M (Manuales)
-- Revistas: Estantería R (Revistas)

-- Primero, actualizamos según el tipo de material y género
UPDATE e
SET e.Ubicacion = CASE
    -- Libros por género
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Fantasia' THEN 'Estantería A - Fantasía'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Romance' THEN 'Estantería B - Romance'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Terror' THEN 'Estantería C - Terror'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Drama' THEN 'Estantería D - Drama'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Historico' THEN 'Estantería E - Histórico'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Otro' THEN 'Estantería F - Otros'
    -- Manuales
    WHEN m.Tipo = 'Manual' THEN 'Estantería M - Manuales Escolares'
    -- Revistas
    WHEN m.Tipo = 'Revista' THEN 'Estantería R - Revistas'
    ELSE 'Depósito General'
END
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE e.Ubicacion IS NULL OR e.Ubicacion = '';

PRINT 'Ubicaciones asignadas.'
GO

-- Actualizar Observaciones según el estado del ejemplar
UPDATE Ejemplar
SET Observaciones = CASE Estado
    WHEN 0 THEN 'Ejemplar en buen estado, disponible para préstamo'
    WHEN 1 THEN 'Ejemplar prestado - Ver tabla de Préstamos para detalles'
    WHEN 2 THEN 'Ejemplar dado de baja - No disponible para préstamo'
    WHEN 3 THEN 'Ejemplar extraviado - En proceso de búsqueda o reposición'
    ELSE 'Estado no definido'
END
WHERE Observaciones IS NULL OR Observaciones = '';

PRINT 'Observaciones completadas.'
GO

-- Actualizar algunos ejemplares con observaciones específicas para ejemplares antiguos o en mal estado
-- Esto es para agregar variedad y realismo

-- Ejemplares de libros clásicos publicados antes de 2000
UPDATE e
SET e.Observaciones = 'Edición clásica - ' + e.Observaciones
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE m.AnioPublicacion < 2000
  AND m.Tipo = 'Libro'
  AND e.Estado = 0
  AND e.NumeroEjemplar % 3 = 0; -- Solo algunos ejemplares

-- Algunos ejemplares con desgaste leve
UPDATE e
SET e.Observaciones = 'Desgaste leve en la portada - ' + e.Observaciones
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE e.Estado = 0
  AND e.NumeroEjemplar % 5 = 0 -- Solo algunos ejemplares
  AND e.Observaciones NOT LIKE 'Edición clásica%';

-- Algunos ejemplares recién adquiridos
UPDATE e
SET e.Observaciones = 'Ejemplar nuevo - ' + e.Observaciones
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE m.AnioPublicacion >= 2024
  AND e.Estado = 0;

PRINT 'Observaciones específicas agregadas.'
GO

-- Verificar resultados mostrando una muestra de ejemplares actualizados
PRINT 'Muestra de ejemplares actualizados:'
GO

SELECT TOP 20
    e.CodigoBarras,
    m.Titulo,
    m.Tipo,
    e.NumeroEjemplar,
    e.Estado,
    e.Ubicacion,
    LEFT(e.Observaciones, 50) + '...' AS Observaciones
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
ORDER BY m.Titulo, e.NumeroEjemplar;
GO

-- Contar ejemplares actualizados
PRINT 'Resumen de actualización:'
GO

SELECT
    COUNT(*) AS TotalEjemplares,
    SUM(CASE WHEN CodigoBarras IS NOT NULL THEN 1 ELSE 0 END) AS ConCodigoBarras,
    SUM(CASE WHEN Ubicacion IS NOT NULL AND Ubicacion <> '' THEN 1 ELSE 0 END) AS ConUbicacion,
    SUM(CASE WHEN Observaciones IS NOT NULL AND Observaciones <> '' THEN 1 ELSE 0 END) AS ConObservaciones
FROM Ejemplar;
GO

PRINT 'Datos de ejemplares completados exitosamente.'
GO
