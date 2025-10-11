-- =====================================================
-- Script: Crear Tablas de Inscripción y Año Lectivo
-- Descripción: Agrega tablas para gestión de inscripciones
--              y promoción de alumnos por año lectivo
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Agregando tablas de Inscripción...'
PRINT ''

-- =====================================================
-- TABLA: AnioLectivo
-- Descripción: Gestión de años lectivos
-- =====================================================
PRINT 'Creando tabla AnioLectivo...'

IF OBJECT_ID('AnioLectivo', 'U') IS NULL
BEGIN
    CREATE TABLE AnioLectivo (
        Anio INT PRIMARY KEY,
        FechaInicio DATE NOT NULL,
        FechaFin DATE NOT NULL,
        Estado NVARCHAR(20) NOT NULL DEFAULT 'Planificado'
            CHECK (Estado IN ('Activo', 'Cerrado', 'Planificado')),

        CONSTRAINT CK_AnioLectivo_Fechas CHECK (FechaInicio < FechaFin)
    );

    PRINT '  ✓ Tabla AnioLectivo creada'
END
ELSE
BEGIN
    PRINT '  ⚠ Tabla AnioLectivo ya existe, omitiendo...'
END

-- =====================================================
-- TABLA: Inscripcion
-- Descripción: Inscripciones de alumnos por año lectivo
-- =====================================================
PRINT 'Creando tabla Inscripcion...'

IF OBJECT_ID('Inscripcion', 'U') IS NULL
BEGIN
    CREATE TABLE Inscripcion (
        IdInscripcion UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        IdAlumno UNIQUEIDENTIFIER NOT NULL,
        AnioLectivo INT NOT NULL,
        Grado NVARCHAR(10) NOT NULL,
        Division NVARCHAR(10) NULL,
        FechaInscripcion DATETIME NOT NULL DEFAULT GETDATE(),
        Estado NVARCHAR(20) NOT NULL DEFAULT 'Activo'
            CHECK (Estado IN ('Activo', 'Finalizado', 'Abandonado')),

        CONSTRAINT FK_Inscripcion_Alumno FOREIGN KEY (IdAlumno)
            REFERENCES Alumno(IdAlumno),
        CONSTRAINT FK_Inscripcion_AnioLectivo FOREIGN KEY (AnioLectivo)
            REFERENCES AnioLectivo(Anio),

        -- Un alumno no puede tener dos inscripciones activas en el mismo año
        CONSTRAINT UQ_Inscripcion_Alumno_Anio UNIQUE (IdAlumno, AnioLectivo)
    );

    -- Índice para búsquedas por año lectivo y grado
    CREATE INDEX IX_Inscripcion_AnioGrado
        ON Inscripcion(AnioLectivo, Grado, Division);

    -- Índice para búsquedas por alumno
    CREATE INDEX IX_Inscripcion_Alumno
        ON Inscripcion(IdAlumno);

    PRINT '  ✓ Tabla Inscripcion creada'
    PRINT '  ✓ Índices creados'
END
ELSE
BEGIN
    PRINT '  ⚠ Tabla Inscripcion ya existe, omitiendo...'
END

-- =====================================================
-- Insertar Año Lectivo actual
-- =====================================================
PRINT ''
PRINT 'Insertando año lectivo actual...'

DECLARE @AnioActual INT = YEAR(GETDATE())
DECLARE @FechaInicioAnioActual DATE = DATEFROMPARTS(@AnioActual, 3, 1) -- 1 de marzo
DECLARE @FechaFinAnioActual DATE = DATEFROMPARTS(@AnioActual, 12, 15)  -- 15 de diciembre

IF NOT EXISTS (SELECT 1 FROM AnioLectivo WHERE Anio = @AnioActual)
BEGIN
    INSERT INTO AnioLectivo (Anio, FechaInicio, FechaFin, Estado)
    VALUES (@AnioActual, @FechaInicioAnioActual, @FechaFinAnioActual, 'Activo');

    PRINT '  ✓ Año lectivo ' + CAST(@AnioActual AS NVARCHAR(4)) + ' creado'
END
ELSE
BEGIN
    PRINT '  ⚠ Año lectivo ' + CAST(@AnioActual AS NVARCHAR(4)) + ' ya existe'
END

PRINT ''
PRINT '=== Tablas de Inscripción Creadas Exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • AnioLectivo: Gestión de ciclos lectivos'
PRINT '  • Inscripcion: Historial de grado/división por año'
PRINT ''
PRINT 'ESTADOS DE INSCRIPCIÓN:'
PRINT '  - Activo: Inscripción vigente'
PRINT '  - Finalizado: Año lectivo completado'
PRINT '  - Abandonado: Alumno abandonó durante el año'
PRINT ''
PRINT 'SIGUIENTE PASO:'
PRINT '  - Ejecutar 05_MigrarDatosInscripcion.sql para migrar datos existentes'
PRINT ''
GO
