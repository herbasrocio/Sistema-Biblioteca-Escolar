-- =====================================================
-- Script: Migrar Datos Existentes a Inscripciones
-- Descripción: Migra los datos de Grado/División actuales
--              de la tabla Alumno a Inscripcion
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Iniciando migración de datos...'
PRINT ''

-- =====================================================
-- Verificar que existan las tablas
-- =====================================================
IF OBJECT_ID('Inscripcion', 'U') IS NULL
BEGIN
    PRINT '❌ ERROR: La tabla Inscripcion no existe.'
    PRINT '   Ejecutar primero: 04_CrearTablasInscripcion.sql'
    RETURN
END

IF OBJECT_ID('AnioLectivo', 'U') IS NULL
BEGIN
    PRINT '❌ ERROR: La tabla AnioLectivo no existe.'
    PRINT '   Ejecutar primero: 04_CrearTablasInscripcion.sql'
    RETURN
END

-- =====================================================
-- Migrar alumnos existentes con Grado/División
-- =====================================================
PRINT 'Migrando alumnos existentes...'

DECLARE @AnioActual INT = YEAR(GETDATE())
DECLARE @AlumnosMigrados INT = 0

-- Migrar solo alumnos que tengan Grado especificado y no tengan inscripción
INSERT INTO Inscripcion (IdInscripcion, IdAlumno, AnioLectivo, Grado, Division, FechaInscripcion, Estado)
SELECT
    NEWID(),
    a.IdAlumno,
    @AnioActual,
    a.Grado,
    a.Division,
    a.FechaRegistro,
    CASE WHEN a.Activo = 1 THEN 'Activo' ELSE 'Abandonado' END
FROM Alumno a
WHERE a.Grado IS NOT NULL
    AND a.Grado <> ''
    AND NOT EXISTS (
        SELECT 1
        FROM Inscripcion i
        WHERE i.IdAlumno = a.IdAlumno
            AND i.AnioLectivo = @AnioActual
    )

SET @AlumnosMigrados = @@ROWCOUNT

PRINT '  ✓ ' + CAST(@AlumnosMigrados AS NVARCHAR(10)) + ' alumnos migrados'

-- =====================================================
-- Verificación de resultados
-- =====================================================
PRINT ''
PRINT 'Verificando migración...'

DECLARE @TotalAlumnosConGrado INT
DECLARE @TotalInscripciones INT
DECLARE @AlumnosSinInscripcion INT

SELECT @TotalAlumnosConGrado = COUNT(*)
FROM Alumno
WHERE Grado IS NOT NULL AND Grado <> ''

SELECT @TotalInscripciones = COUNT(*)
FROM Inscripcion
WHERE AnioLectivo = @AnioActual

SELECT @AlumnosSinInscripcion = COUNT(*)
FROM Alumno a
WHERE a.Grado IS NOT NULL
    AND a.Grado <> ''
    AND NOT EXISTS (
        SELECT 1
        FROM Inscripcion i
        WHERE i.IdAlumno = a.IdAlumno
            AND i.AnioLectivo = @AnioActual
    )

PRINT ''
PRINT '=== Resumen de Migración ==='
PRINT '  • Total alumnos con grado: ' + CAST(@TotalAlumnosConGrado AS NVARCHAR(10))
PRINT '  • Total inscripciones creadas: ' + CAST(@TotalInscripciones AS NVARCHAR(10))
PRINT '  • Alumnos sin inscripción: ' + CAST(@AlumnosSinInscripcion AS NVARCHAR(10))

IF @AlumnosSinInscripcion = 0
BEGIN
    PRINT ''
    PRINT '✓✓✓ Migración completada exitosamente ✓✓✓'
END
ELSE
BEGIN
    PRINT ''
    PRINT '⚠ ADVERTENCIA: Hay alumnos sin inscripción'
END

PRINT ''
PRINT 'NOTA IMPORTANTE:'
PRINT '  - Los campos Grado y Division en la tabla Alumno ahora son históricos'
PRINT '  - El grado actual se obtiene de la tabla Inscripcion'
PRINT '  - Considerar ejecutar 06_AlterTablaAlumno.sql para remover estos campos'
PRINT ''
GO

-- =====================================================
-- Consulta para verificar inscripciones
-- =====================================================
PRINT 'Inscripciones creadas por grado:'
PRINT ''

SELECT
    Grado,
    Division,
    COUNT(*) as CantidadAlumnos,
    SUM(CASE WHEN Estado = 'Activo' THEN 1 ELSE 0 END) as Activos
FROM Inscripcion
WHERE AnioLectivo = YEAR(GETDATE())
GROUP BY Grado, Division
ORDER BY Grado, Division

GO
