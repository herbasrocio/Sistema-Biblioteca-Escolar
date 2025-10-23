-- =====================================================
-- Script de Diagnóstico y Reparación de Estados de Ejemplares
-- Fecha: 2025-10-22
-- Propósito: Detectar y corregir inconsistencias entre el estado
--            de los ejemplares y los préstamos activos
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT '========================================';
PRINT 'DIAGNÓSTICO DE ESTADOS DE EJEMPLARES';
PRINT '========================================';
PRINT '';

-- =====================================================
-- PASO 1: Detectar ejemplares marcados como Prestados
--         sin préstamo activo correspondiente
-- =====================================================

PRINT 'PASO 1: Ejemplares marcados como PRESTADOS sin préstamo activo';
PRINT '----------------------------------------------------------------';

SELECT
    m.Titulo AS Material,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Estado AS EstadoActual,
    CASE
        WHEN EXISTS (
            SELECT 1 FROM Prestamo p
            WHERE p.IdEjemplar = e.IdEjemplar
            AND (p.Estado = 'Activo' OR p.Estado = 'Atrasado')
        ) THEN 'SÍ'
        ELSE 'NO'
    END AS TienePrestamoActivo,
    (
        SELECT TOP 1 a.Nombre + ' ' + a.Apellido
        FROM Prestamo p2
        INNER JOIN Alumno a ON p2.IdAlumno = a.IdAlumno
        WHERE p2.IdEjemplar = e.IdEjemplar
        ORDER BY p2.FechaPrestamo DESC
    ) AS UltimoAlumno,
    (
        SELECT TOP 1 p3.Estado
        FROM Prestamo p3
        WHERE p3.IdEjemplar = e.IdEjemplar
        ORDER BY p3.FechaPrestamo DESC
    ) AS EstadoUltimoPrestamo
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE e.Estado = 1 -- Prestado
AND e.Activo = 1
AND NOT EXISTS (
    SELECT 1 FROM Prestamo p
    WHERE p.IdEjemplar = e.IdEjemplar
    AND (p.Estado = 'Activo' OR p.Estado = 'Atrasado')
);

DECLARE @EjemplaresMalPrestados INT;
SELECT @EjemplaresMalPrestados = COUNT(*)
FROM Ejemplar e
WHERE e.Estado = 1
AND e.Activo = 1
AND NOT EXISTS (
    SELECT 1 FROM Prestamo p
    WHERE p.IdEjemplar = e.IdEjemplar
    AND (p.Estado = 'Activo' OR p.Estado = 'Atrasado')
);

PRINT '';
PRINT 'Total de ejemplares inconsistentes: ' + CAST(@EjemplaresMalPrestados AS VARCHAR(10));
PRINT '';

-- =====================================================
-- PASO 2: Detectar ejemplares marcados como Disponibles
--         CON préstamo activo
-- =====================================================

PRINT 'PASO 2: Ejemplares marcados como DISPONIBLES con préstamo activo';
PRINT '------------------------------------------------------------------';

SELECT
    m.Titulo AS Material,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Estado AS EstadoActual,
    a.Nombre + ' ' + a.Apellido AS AlumnoConPrestamo,
    p.FechaPrestamo,
    p.FechaDevolucionPrevista,
    p.Estado AS EstadoPrestamo,
    CASE
        WHEN p.FechaDevolucionPrevista < GETDATE() THEN 'SÍ'
        ELSE 'NO'
    END AS EstaVencido
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
INNER JOIN Prestamo p ON e.IdEjemplar = p.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE e.Estado = 0 -- Disponible
AND e.Activo = 1
AND (p.Estado = 'Activo' OR p.Estado = 'Atrasado');

DECLARE @EjemplaresDisponiblesConPrestamo INT;
SELECT @EjemplaresDisponiblesConPrestamo = COUNT(*)
FROM Ejemplar e
INNER JOIN Prestamo p ON e.IdEjemplar = p.IdEjemplar
WHERE e.Estado = 0
AND e.Activo = 1
AND (p.Estado = 'Activo' OR p.Estado = 'Atrasado');

PRINT '';
PRINT 'Total de ejemplares inconsistentes: ' + CAST(@EjemplaresDisponiblesConPrestamo AS VARCHAR(10));
PRINT '';

-- =====================================================
-- PASO 3: Resumen general
-- =====================================================

PRINT '========================================';
PRINT 'RESUMEN GENERAL';
PRINT '========================================';
PRINT 'Ejemplares PRESTADOS sin préstamo activo: ' + CAST(@EjemplaresMalPrestados AS VARCHAR(10));
PRINT 'Ejemplares DISPONIBLES con préstamo activo: ' + CAST(@EjemplaresDisponiblesConPrestamo AS VARCHAR(10));
PRINT '';

IF @EjemplaresMalPrestados > 0 OR @EjemplaresDisponiblesConPrestamo > 0
BEGIN
    PRINT '⚠️  SE DETECTARON INCONSISTENCIAS';
    PRINT '';
    PRINT 'Para reparar automáticamente, ejecute la sección de REPARACIÓN a continuación';
END
ELSE
BEGIN
    PRINT '✅ NO SE DETECTARON INCONSISTENCIAS';
    PRINT 'Los estados de ejemplares están sincronizados con los préstamos';
END

PRINT '';
PRINT '========================================';
PRINT 'SECCIÓN DE REPARACIÓN';
PRINT '========================================';
PRINT '';
PRINT '-- Descomente las siguientes líneas para REPARAR automáticamente:';
PRINT '';

/*
-- =====================================================
-- REPARACIÓN AUTOMÁTICA
-- =====================================================

BEGIN TRANSACTION;

-- Reparar ejemplares marcados como PRESTADOS sin préstamo activo
-- (Cambiarlos a DISPONIBLE)
UPDATE e
SET Estado = 0 -- Disponible
FROM Ejemplar e
WHERE e.Estado = 1
AND e.Activo = 1
AND NOT EXISTS (
    SELECT 1 FROM Prestamo p
    WHERE p.IdEjemplar = e.IdEjemplar
    AND (p.Estado = 'Activo' OR p.Estado = 'Atrasado')
);

DECLARE @EjemplaresReparados1 INT = @@ROWCOUNT;

-- Reparar ejemplares marcados como DISPONIBLES con préstamo activo
-- (Cambiarlos a PRESTADO)
UPDATE e
SET Estado = 1 -- Prestado
FROM Ejemplar e
INNER JOIN Prestamo p ON e.IdEjemplar = p.IdEjemplar
WHERE e.Estado = 0
AND e.Activo = 1
AND (p.Estado = 'Activo' OR p.Estado = 'Atrasado');

DECLARE @EjemplaresReparados2 INT = @@ROWCOUNT;

PRINT '';
PRINT 'REPARACIÓN COMPLETADA:';
PRINT 'Ejemplares cambiados de PRESTADO a DISPONIBLE: ' + CAST(@EjemplaresReparados1 AS VARCHAR(10));
PRINT 'Ejemplares cambiados de DISPONIBLE a PRESTADO: ' + CAST(@EjemplaresReparados2 AS VARCHAR(10));
PRINT '';

-- Descomentar la siguiente línea para confirmar los cambios
-- COMMIT TRANSACTION;

-- O descomentar esta línea para revertir los cambios
ROLLBACK TRANSACTION;

PRINT '';
PRINT '⚠️  CAMBIOS REVERTIDOS (ROLLBACK)';
PRINT 'Para aplicar los cambios permanentemente, cambie ROLLBACK por COMMIT';
*/

GO
