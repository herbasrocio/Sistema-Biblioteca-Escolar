-- ============================================
-- Script: 58_CorregirUbicacionesEjemplares.sql
-- Descripción: Corrige las ubicaciones de ejemplares eliminando acentos y caracteres extraños
-- Usa formato: Estanteria 01, Estanteria 02, etc.
-- Fecha: 2025-10-13
-- ============================================

USE NegocioBiblioteca
GO

PRINT 'Corrigiendo ubicaciones de Ejemplares sin acentos...'
GO

-- Actualizar ubicaciones según el tipo de material y género
-- Formato: Estanteria 01, Estanteria 02, etc. (sin acentos ni guiones)

UPDATE e
SET e.Ubicacion = CASE
    -- Libros por género
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Fantasia' THEN 'Estanteria 01 - Fantasia'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Romance' THEN 'Estanteria 02 - Romance'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Terror' THEN 'Estanteria 03 - Terror'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Drama' THEN 'Estanteria 04 - Drama'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Historico' THEN 'Estanteria 05 - Historico'
    WHEN m.Tipo = 'Libro' AND m.Genero = 'Otro' THEN 'Estanteria 06 - Otros'
    -- Manuales
    WHEN m.Tipo = 'Manual' THEN 'Estanteria 07 - Manuales Escolares'
    -- Revistas
    WHEN m.Tipo = 'Revista' THEN 'Estanteria 08 - Revistas'
    ELSE 'Deposito General'
END
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial;

PRINT 'Ubicaciones corregidas exitosamente.'
GO

-- Verificar ubicaciones únicas después de la corrección
PRINT 'Ubicaciones únicas actualizadas:'
GO

SELECT DISTINCT Ubicacion, COUNT(*) AS CantidadEjemplares
FROM Ejemplar
GROUP BY Ubicacion
ORDER BY Ubicacion;
GO

-- Mostrar muestra de ejemplares con ubicaciones corregidas
PRINT 'Muestra de ejemplares con ubicaciones corregidas:'
GO

SELECT TOP 15
    e.CodigoBarras,
    m.Titulo,
    m.Tipo,
    m.Genero,
    e.Ubicacion
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
ORDER BY m.Tipo, m.Genero, m.Titulo;
GO

PRINT 'Corrección de ubicaciones completada.'
GO
