-- =====================================================
-- Script: Stored Procedures para Inscripciones
-- Descripción: Procedimientos para gestión de inscripciones
--              y promoción de alumnos
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Creando Stored Procedures para Inscripciones...'
PRINT ''

-- =====================================================
-- SP: Obtener Inscripción Activa de un Alumno
-- =====================================================
IF OBJECT_ID('sp_ObtenerInscripcionActiva', 'P') IS NOT NULL
    DROP PROCEDURE sp_ObtenerInscripcionActiva
GO

CREATE PROCEDURE sp_ObtenerInscripcionActiva
    @IdAlumno UNIQUEIDENTIFIER,
    @AnioLectivo INT = NULL -- Si es NULL, usa el año actual
AS
BEGIN
    SET NOCOUNT ON;

    IF @AnioLectivo IS NULL
        SET @AnioLectivo = YEAR(GETDATE())

    SELECT
        i.IdInscripcion,
        i.IdAlumno,
        i.AnioLectivo,
        i.Grado,
        i.Division,
        i.FechaInscripcion,
        i.Estado
    FROM Inscripcion i
    WHERE i.IdAlumno = @IdAlumno
        AND i.AnioLectivo = @AnioLectivo
        AND i.Estado = 'Activo'
END
GO

PRINT '  ✓ sp_ObtenerInscripcionActiva creado'

-- =====================================================
-- SP: Promocionar Alumnos de un Grado
-- =====================================================
IF OBJECT_ID('sp_PromocionarAlumnosPorGrado', 'P') IS NOT NULL
    DROP PROCEDURE sp_PromocionarAlumnosPorGrado
GO

CREATE PROCEDURE sp_PromocionarAlumnosPorGrado
    @AnioActual INT,
    @AnioSiguiente INT,
    @GradoActual NVARCHAR(10),
    @DivisionActual NVARCHAR(10) = NULL,
    @GradoNuevo NVARCHAR(10),
    @DivisionNueva NVARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION

    BEGIN TRY
        -- 1. Finalizar inscripciones del año actual
        UPDATE Inscripcion
        SET Estado = 'Finalizado'
        WHERE AnioLectivo = @AnioActual
            AND Grado = @GradoActual
            AND (@DivisionActual IS NULL OR Division = @DivisionActual)
            AND Estado = 'Activo'

        DECLARE @AlumnosFinalizados INT = @@ROWCOUNT

        -- 2. Crear nuevas inscripciones para el año siguiente
        INSERT INTO Inscripcion (IdInscripcion, IdAlumno, AnioLectivo, Grado, Division, FechaInscripcion, Estado)
        SELECT
            NEWID(),
            i.IdAlumno,
            @AnioSiguiente,
            @GradoNuevo,
            ISNULL(@DivisionNueva, i.Division), -- Mantiene división si no se especifica nueva
            GETDATE(),
            'Activo'
        FROM Inscripcion i
        WHERE i.AnioLectivo = @AnioActual
            AND i.Grado = @GradoActual
            AND (@DivisionActual IS NULL OR i.Division = @DivisionActual)
            AND i.Estado = 'Finalizado' -- Solo los que acabamos de finalizar
            AND NOT EXISTS (
                -- Evitar duplicados si ya existe inscripción
                SELECT 1
                FROM Inscripcion i2
                WHERE i2.IdAlumno = i.IdAlumno
                    AND i2.AnioLectivo = @AnioSiguiente
            )

        DECLARE @AlumnosPromovidos INT = @@ROWCOUNT

        COMMIT TRANSACTION

        -- Retornar resumen
        SELECT
            @AlumnosFinalizados AS AlumnosFinalizados,
            @AlumnosPromovidos AS AlumnosPromovidos,
            'OK' AS Resultado

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION

        SELECT
            0 AS AlumnosFinalizados,
            0 AS AlumnosPromovidos,
            ERROR_MESSAGE() AS Resultado
    END CATCH
END
GO

PRINT '  ✓ sp_PromocionarAlumnosPorGrado creado'

-- =====================================================
-- SP: Promocionar Todos los Alumnos
-- =====================================================
IF OBJECT_ID('sp_PromocionarTodosLosAlumnos', 'P') IS NOT NULL
    DROP PROCEDURE sp_PromocionarTodosLosAlumnos
GO

CREATE PROCEDURE sp_PromocionarTodosLosAlumnos
    @AnioActual INT,
    @AnioSiguiente INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION

    BEGIN TRY
        -- Crear tabla temporal para mapeo de grados
        CREATE TABLE #MapeoGrados (
            GradoActual NVARCHAR(10),
            GradoSiguiente NVARCHAR(10)
        )

        -- Definir reglas de promoción
        INSERT INTO #MapeoGrados VALUES ('1', '2')
        INSERT INTO #MapeoGrados VALUES ('2', '3')
        INSERT INTO #MapeoGrados VALUES ('3', '4')
        INSERT INTO #MapeoGrados VALUES ('4', '5')
        INSERT INTO #MapeoGrados VALUES ('5', '6')
        INSERT INTO #MapeoGrados VALUES ('6', '7')
        INSERT INTO #MapeoGrados VALUES ('7', 'EGRESADO')

        -- 1. Finalizar todas las inscripciones activas del año actual
        UPDATE Inscripcion
        SET Estado = 'Finalizado'
        WHERE AnioLectivo = @AnioActual
            AND Estado = 'Activo'

        DECLARE @AlumnosFinalizados INT = @@ROWCOUNT

        -- 2. Crear nuevas inscripciones según mapeo de grados
        INSERT INTO Inscripcion (IdInscripcion, IdAlumno, AnioLectivo, Grado, Division, FechaInscripcion, Estado)
        SELECT
            NEWID(),
            i.IdAlumno,
            @AnioSiguiente,
            m.GradoSiguiente,
            i.Division,
            GETDATE(),
            'Activo'
        FROM Inscripcion i
        INNER JOIN #MapeoGrados m ON i.Grado = m.GradoActual
        WHERE i.AnioLectivo = @AnioActual
            AND i.Estado = 'Finalizado'
            AND m.GradoSiguiente <> 'EGRESADO' -- No inscribir egresados
            AND NOT EXISTS (
                SELECT 1
                FROM Inscripcion i2
                WHERE i2.IdAlumno = i.IdAlumno
                    AND i2.AnioLectivo = @AnioSiguiente
            )

        DECLARE @AlumnosPromovidos INT = @@ROWCOUNT

        -- Contar egresados
        DECLARE @Egresados INT = (
            SELECT COUNT(*)
            FROM Inscripcion i
            INNER JOIN #MapeoGrados m ON i.Grado = m.GradoActual
            WHERE i.AnioLectivo = @AnioActual
                AND i.Estado = 'Finalizado'
                AND m.GradoSiguiente = 'EGRESADO'
        )

        DROP TABLE #MapeoGrados

        COMMIT TRANSACTION

        -- Retornar resumen
        SELECT
            @AlumnosFinalizados AS AlumnosFinalizados,
            @AlumnosPromovidos AS AlumnosPromovidos,
            @Egresados AS Egresados,
            'OK' AS Resultado

    END TRY
    BEGIN CATCH
        IF OBJECT_ID('tempdb..#MapeoGrados') IS NOT NULL
            DROP TABLE #MapeoGrados

        ROLLBACK TRANSACTION

        SELECT
            0 AS AlumnosFinalizados,
            0 AS AlumnosPromovidos,
            0 AS Egresados,
            ERROR_MESSAGE() AS Resultado
    END CATCH
END
GO

PRINT '  ✓ sp_PromocionarTodosLosAlumnos creado'

-- =====================================================
-- SP: Obtener Alumnos por Grado y División
-- =====================================================
IF OBJECT_ID('sp_ObtenerAlumnosPorGradoDivision', 'P') IS NOT NULL
    DROP PROCEDURE sp_ObtenerAlumnosPorGradoDivision
GO

CREATE PROCEDURE sp_ObtenerAlumnosPorGradoDivision
    @AnioLectivo INT = NULL,
    @Grado NVARCHAR(10) = NULL,
    @Division NVARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @AnioLectivo IS NULL
        SET @AnioLectivo = YEAR(GETDATE())

    SELECT
        a.IdAlumno,
        a.Nombre,
        a.Apellido,
        a.DNI,
        a.Email,
        a.Telefono,
        i.Grado,
        i.Division,
        i.AnioLectivo,
        i.Estado AS EstadoInscripcion,
        a.FechaRegistro,
        a.Activo
    FROM Alumno a
    INNER JOIN Inscripcion i ON a.IdAlumno = i.IdAlumno
    WHERE i.AnioLectivo = @AnioLectivo
        AND (@Grado IS NULL OR i.Grado = @Grado)
        AND (@Division IS NULL OR i.Division = @Division)
        AND a.Activo = 1
        AND i.Estado = 'Activo'
    ORDER BY a.Apellido, a.Nombre
END
GO

PRINT '  ✓ sp_ObtenerAlumnosPorGradoDivision creado'

PRINT ''
PRINT '=== Stored Procedures Creados Exitosamente ==='
PRINT ''
PRINT 'Procedimientos disponibles:'
PRINT '  • sp_ObtenerInscripcionActiva - Obtener inscripción activa de un alumno'
PRINT '  • sp_PromocionarAlumnosPorGrado - Promocionar un grado/división específico'
PRINT '  • sp_PromocionarTodosLosAlumnos - Promoción masiva automática'
PRINT '  • sp_ObtenerAlumnosPorGradoDivision - Listar alumnos por grado/división'
PRINT ''
GO
