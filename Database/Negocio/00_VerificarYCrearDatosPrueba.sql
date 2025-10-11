-- =====================================================
-- Script: Verificar Base de Datos y Crear Datos de Prueba
-- Descripción: Verifica la estructura y agrega alumnos de prueba
-- =====================================================

-- Verificar si existe la base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'NegocioBiblioteca')
BEGIN
    PRINT '❌ ERROR: La base de datos NegocioBiblioteca NO existe'
    PRINT 'Por favor, ejecutar primero los scripts de creación de base de datos'
    RETURN
END

USE NegocioBiblioteca;
GO

PRINT '=== Verificando Estructura de Base de Datos ==='
PRINT ''

-- Verificar tabla Alumno
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Alumno')
BEGIN
    PRINT '❌ ERROR: La tabla Alumno NO existe'
    PRINT 'Ejecutar script: Database\Negocio\02_CrearTablasNegocio.sql'
    RETURN
END
ELSE
BEGIN
    PRINT '✓ Tabla Alumno existe'
END

-- Verificar tabla Inscripcion
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Inscripcion')
BEGIN
    PRINT '✓ Tabla Inscripcion existe'
    SET @TieneInscripciones = 1
END
ELSE
BEGIN
    PRINT '⚠ Tabla Inscripcion NO existe (se creará con datos de Alumno)'
    SET @TieneInscripciones = 0
END

PRINT ''
PRINT '=== Verificando Datos Existentes ==='
PRINT ''

DECLARE @TieneInscripciones BIT = 0
DECLARE @CantidadAlumnos INT
DECLARE @CantidadInscripciones INT

SELECT @CantidadAlumnos = COUNT(*) FROM Alumno

PRINT 'Alumnos activos en tabla Alumno: ' + CAST(@CantidadAlumnos AS NVARCHAR(10))

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Inscripcion')
BEGIN
    SELECT @CantidadInscripciones = COUNT(*) FROM Inscripcion WHERE Estado = 'Activo'
    PRINT 'Inscripciones activas: ' + CAST(@CantidadInscripciones AS NVARCHAR(10))
END

PRINT ''

-- Si no hay alumnos, crear datos de prueba
IF @CantidadAlumnos = 0
BEGIN
    PRINT '=== Creando Alumnos de Prueba ==='
    PRINT ''

    DECLARE @AnioActual INT = YEAR(GETDATE())

    -- Alumnos de 1° A
    INSERT INTO Alumno (IdAlumno, Nombre, Apellido, DNI, Grado, Division, FechaRegistro)
    VALUES
        (NEWID(), 'Juan', 'Pérez', '12345678', '1', 'A', GETDATE()),
        (NEWID(), 'María', 'González', '23456789', '1', 'A', GETDATE()),
        (NEWID(), 'Pedro', 'Rodríguez', '34567890', '1', 'A', GETDATE()),
        (NEWID(), 'Ana', 'Martínez', '45678901', '1', 'A', GETDATE()),
        (NEWID(), 'Luis', 'García', '56789012', '1', 'A', GETDATE())

    PRINT '  ✓ Creados 5 alumnos de 1° A'

    -- Alumnos de 2° B
    INSERT INTO Alumno (IdAlumno, Nombre, Apellido, DNI, Grado, Division, FechaRegistro)
    VALUES
        (NEWID(), 'Laura', 'Fernández', '67890123', '2', 'B', GETDATE()),
        (NEWID(), 'Carlos', 'López', '78901234', '2', 'B', GETDATE()),
        (NEWID(), 'Sofía', 'Díaz', '89012345', '2', 'B', GETDATE()),
        (NEWID(), 'Diego', 'Sánchez', '90123456', '2', 'B', GETDATE())

    PRINT '  ✓ Creados 4 alumnos de 2° B'

    -- Alumnos de 3° A
    INSERT INTO Alumno (IdAlumno, Nombre, Apellido, DNI, Grado, Division, FechaRegistro)
    VALUES
        (NEWID(), 'Valentina', 'Romero', '11223344', '3', 'A', GETDATE()),
        (NEWID(), 'Mateo', 'Torres', '22334455', '3', 'A', GETDATE()),
        (NEWID(), 'Isabella', 'Flores', '33445566', '3', 'A', GETDATE())

    PRINT '  ✓ Creados 3 alumnos de 3° A'

    -- Alumnos de 4° C
    INSERT INTO Alumno (IdAlumno, Nombre, Apellido, DNI, Grado, Division, FechaRegistro)
    VALUES
        (NEWID(), 'Joaquín', 'Vargas', '44556677', '4', 'C', GETDATE()),
        (NEWID(), 'Camila', 'Ruiz', '55667788', '4', 'C', GETDATE()),
        (NEWID(), 'Benjamín', 'Medina', '66778899', '4', 'C', GETDATE()),
        (NEWID(), 'Martina', 'Castro', '77889900', '4', 'C', GETDATE())

    PRINT '  ✓ Creados 4 alumnos de 4° C'

    PRINT ''
    PRINT '✓✓✓ Total: 16 alumnos de prueba creados ✓✓✓'

    -- Si existe tabla Inscripcion, crear inscripciones
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Inscripcion')
    BEGIN
        PRINT ''
        PRINT '=== Creando Inscripciones para los Alumnos ==='

        -- Verificar que existe tabla AnioLectivo
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AnioLectivo')
        BEGIN
            PRINT '⚠ Tabla AnioLectivo no existe, creando año actual...'

            CREATE TABLE AnioLectivo (
                Anio INT PRIMARY KEY,
                FechaInicio DATE NOT NULL,
                FechaFin DATE NOT NULL,
                Estado NVARCHAR(20) NOT NULL DEFAULT 'Planificado'
                    CHECK (Estado IN ('Activo', 'Cerrado', 'Planificado'))
            )

            INSERT INTO AnioLectivo (Anio, FechaInicio, FechaFin, Estado)
            VALUES (@AnioActual, DATEFROMPARTS(@AnioActual, 3, 1), DATEFROMPARTS(@AnioActual, 12, 15), 'Activo')

            PRINT '  ✓ Año lectivo ' + CAST(@AnioActual AS NVARCHAR(4)) + ' creado'
        END

        -- Crear inscripciones para todos los alumnos
        INSERT INTO Inscripcion (IdInscripcion, IdAlumno, AnioLectivo, Grado, Division, FechaInscripcion, Estado)
        SELECT
            NEWID(),
            IdAlumno,
            @AnioActual,
            Grado,
            Division,
            FechaRegistro,
            'Activo'
        FROM Alumno
        WHERE NOT EXISTS (
            SELECT 1 FROM Inscripcion i
            WHERE i.IdAlumno = Alumno.IdAlumno
            AND i.AnioLectivo = @AnioActual
        )

        PRINT '  ✓ Inscripciones creadas para año ' + CAST(@AnioActual AS NVARCHAR(4))
    END
END
ELSE
BEGIN
    PRINT '✓ La base de datos ya tiene ' + CAST(@CantidadAlumnos AS NVARCHAR(10)) + ' alumnos'
    PRINT 'No se agregaron datos de prueba'
END

PRINT ''
PRINT '=== Resumen Final ==='
PRINT ''

-- Mostrar resumen de alumnos por grado
SELECT
    ISNULL(Grado, 'SIN GRADO') AS Grado,
    ISNULL(Division, 'SIN DIV') AS Division,
    COUNT(*) AS CantidadAlumnos
FROM Alumno
GROUP BY Grado, Division
ORDER BY Grado, Division

PRINT ''
PRINT 'Total de alumnos: ' + CAST((SELECT COUNT(*) FROM Alumno) AS NVARCHAR(10))

-- Si hay inscripciones, mostrar resumen
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Inscripcion')
BEGIN
    PRINT 'Total de inscripciones activas: ' + CAST((SELECT COUNT(*) FROM Inscripcion WHERE Estado = 'Activo') AS NVARCHAR(10))
END

PRINT ''
PRINT '=== Verificación Completa ==='
PRINT ''
PRINT 'SIGUIENTE PASO: Abrir la aplicación y navegar a "Gestionar Alumnos"'
PRINT ''

GO
