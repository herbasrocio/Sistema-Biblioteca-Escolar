-- =====================================================
-- Script: Verificar Estado de Base de Datos de Negocio
-- Descripción: Verifica si la base de datos existe y muestra
--              estadísticas sin modificar nada
-- =====================================================

PRINT ''
PRINT '=========================================='
PRINT '  VERIFICACIÓN DE BASE DE DATOS'
PRINT '=========================================='
PRINT ''

-- Verificar si existe la base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'NegocioBiblioteca')
BEGIN
    PRINT '❌ ERROR: La base de datos NegocioBiblioteca NO existe'
    PRINT ''
    PRINT 'SOLUCIÓN:'
    PRINT '  Ejecutar: Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql'
    PRINT ''
    RETURN
END

PRINT '✓ Base de datos NegocioBiblioteca existe'
PRINT ''

USE NegocioBiblioteca;
GO

PRINT '=========================================='
PRINT '  VERIFICACIÓN DE TABLAS'
PRINT '=========================================='
PRINT ''

-- Verificar tabla Material
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Material')
BEGIN
    DECLARE @CantidadMateriales INT
    SELECT @CantidadMateriales = COUNT(*) FROM Material WHERE Activo = 1
    PRINT '✓ Tabla Material existe - ' + CAST(@CantidadMateriales AS NVARCHAR(10)) + ' materiales activos'
END
ELSE
BEGIN
    PRINT '❌ Tabla Material NO existe'
END

-- Verificar tabla Alumno
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Alumno')
BEGIN
    DECLARE @CantidadAlumnos INT
    SELECT @CantidadAlumnos = COUNT(*) FROM Alumno
    PRINT '✓ Tabla Alumno existe - ' + CAST(@CantidadAlumnos AS NVARCHAR(10)) + ' alumnos'
END
ELSE
BEGIN
    PRINT '❌ Tabla Alumno NO existe'
END

-- Verificar tabla Prestamo
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Prestamo')
BEGIN
    DECLARE @CantidadPrestamos INT
    SELECT @CantidadPrestamos = COUNT(*) FROM Prestamo
    PRINT '✓ Tabla Prestamo existe - ' + CAST(@CantidadPrestamos AS NVARCHAR(10)) + ' préstamos registrados'
END
ELSE
BEGIN
    PRINT '❌ Tabla Prestamo NO existe'
END

-- Verificar tabla Inscripcion
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Inscripcion')
BEGIN
    DECLARE @CantidadInscripciones INT
    SELECT @CantidadInscripciones = COUNT(*) FROM Inscripcion WHERE Estado = 'Activo'
    PRINT '✓ Tabla Inscripcion existe - ' + CAST(@CantidadInscripciones AS NVARCHAR(10)) + ' inscripciones activas'
END
ELSE
BEGIN
    PRINT '⚠ Tabla Inscripcion NO existe (ejecutar scripts de inscripción)'
END

-- Verificar tabla AnioLectivo
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AnioLectivo')
BEGIN
    DECLARE @AnioActual INT
    SELECT TOP 1 @AnioActual = Anio FROM AnioLectivo WHERE Estado = 'Activo' ORDER BY Anio DESC
    IF @AnioActual IS NOT NULL
        PRINT '✓ Tabla AnioLectivo existe - Año activo: ' + CAST(@AnioActual AS NVARCHAR(10))
    ELSE
        PRINT '⚠ Tabla AnioLectivo existe pero no hay año activo'
END
ELSE
BEGIN
    PRINT '⚠ Tabla AnioLectivo NO existe'
END

PRINT ''
PRINT '=========================================='
PRINT '  STORED PROCEDURES'
PRINT '=========================================='
PRINT ''

DECLARE @CantidadSPs INT
SELECT @CantidadSPs = COUNT(*) FROM sys.procedures
WHERE name LIKE 'sp_%Inscripcion%'
   OR name LIKE 'sp_Promocionar%'
   OR name LIKE 'sp_ObtenerAlumnos%'

IF @CantidadSPs > 0
BEGIN
    PRINT '✓ Stored Procedures encontrados: ' + CAST(@CantidadSPs AS NVARCHAR(10))
    PRINT ''
    SELECT name AS [Stored Procedure]
    FROM sys.procedures
    WHERE name LIKE 'sp_%Inscripcion%'
       OR name LIKE 'sp_Promocionar%'
       OR name LIKE 'sp_ObtenerAlumnos%'
    ORDER BY name
END
ELSE
BEGIN
    PRINT '⚠ No se encontraron Stored Procedures de inscripción'
END

PRINT ''
PRINT '=========================================='
PRINT '  ALUMNOS POR GRADO/DIVISIÓN'
PRINT '=========================================='
PRINT ''

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Alumno')
BEGIN
    SELECT
        ISNULL(Grado, 'SIN GRADO') AS Grado,
        ISNULL(Division, 'SIN DIV') AS Division,
        COUNT(*) AS Cantidad
    FROM Alumno
    GROUP BY Grado, Division
    ORDER BY Grado, Division
END

PRINT ''
PRINT '=========================================='
PRINT '  DIAGNÓSTICO'
PRINT '=========================================='
PRINT ''

-- Evaluar estado general
DECLARE @TodoOK BIT = 1

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Material')
    SET @TodoOK = 0

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Alumno')
    SET @TodoOK = 0

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Prestamo')
    SET @TodoOK = 0

IF @TodoOK = 1
BEGIN
    PRINT '✓✓✓ La base de datos está correctamente configurada'
    PRINT ''

    IF EXISTS (SELECT * FROM Alumno)
    BEGIN
        PRINT 'ESTADO: ✅ LISTO PARA USAR'
        PRINT ''
        PRINT 'Puedes abrir la aplicación y gestionar alumnos.'
    END
    ELSE
    BEGIN
        PRINT 'ESTADO: ⚠ SIN DATOS'
        PRINT ''
        PRINT 'La base de datos está vacía. Ejecuta uno de estos scripts:'
        PRINT '  • Database\Negocio\03_DatosInicialesNegocio.sql (datos de ejemplo)'
        PRINT '  • Database\Negocio\00_VerificarYCrearDatosPrueba.sql (datos de prueba)'
    END
END
ELSE
BEGIN
    PRINT '❌ La base de datos tiene problemas'
    PRINT ''
    PRINT 'SOLUCIÓN:'
    PRINT '  Ejecutar: Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql'
END

PRINT ''
GO
