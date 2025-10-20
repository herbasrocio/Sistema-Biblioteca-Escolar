/*
    Script: 40_CrearTablaHistorialEstadoEjemplar.sql
    Descripción: Crea la tabla HistorialEstadoEjemplar para rastrear todos los cambios de estado
    Fecha: 2025-10-20

    Esta tabla permite auditar:
    - Cuándo cambió el estado de un ejemplar
    - Quién realizó el cambio
    - Cuál era el estado anterior y el nuevo
    - Motivo del cambio (opcional)
    - Relación con préstamo/devolución si aplica
*/

USE NegocioBiblioteca;
GO

PRINT 'Iniciando creación de tabla HistorialEstadoEjemplar...';
GO

-- Verificar si la tabla ya existe
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HistorialEstadoEjemplar')
BEGIN
    PRINT 'Creando tabla HistorialEstadoEjemplar...';

    CREATE TABLE HistorialEstadoEjemplar (
        IdHistorial UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        IdEjemplar UNIQUEIDENTIFIER NOT NULL,
        EstadoAnterior INT NOT NULL,  -- 0=Disponible, 1=Prestado, 2=EnReparacion, 3=NoDisponible
        EstadoNuevo INT NOT NULL,     -- 0=Disponible, 1=Prestado, 2=EnReparacion, 3=NoDisponible
        FechaCambio DATETIME NOT NULL DEFAULT GETDATE(),
        IdUsuario UNIQUEIDENTIFIER NULL,  -- Usuario que realizó el cambio (puede ser NULL para cambios automáticos)
        Motivo NVARCHAR(500) NULL,        -- Razón del cambio
        IdPrestamo UNIQUEIDENTIFIER NULL, -- Si el cambio fue por un préstamo
        IdDevolucion UNIQUEIDENTIFIER NULL, -- Si el cambio fue por una devolución
        TipoCambio NVARCHAR(50) NOT NULL, -- 'Manual', 'Prestamo', 'Devolucion', 'Sistema'

        -- Foreign Key a Ejemplar
        CONSTRAINT FK_HistorialEstadoEjemplar_Ejemplar FOREIGN KEY (IdEjemplar)
            REFERENCES Ejemplar(IdEjemplar),

        -- Check: Estados válidos (0-3)
        CONSTRAINT CK_HistorialEstadoEjemplar_Estados
            CHECK (EstadoAnterior IN (0, 1, 2, 3) AND EstadoNuevo IN (0, 1, 2, 3)),

        -- Check: Tipo de cambio válido
        CONSTRAINT CK_HistorialEstadoEjemplar_TipoCambio
            CHECK (TipoCambio IN ('Manual', 'Prestamo', 'Devolucion', 'Sistema'))
    );

    PRINT 'Tabla HistorialEstadoEjemplar creada exitosamente.';

    -- Crear índices para mejorar performance
    CREATE INDEX IX_HistorialEstadoEjemplar_IdEjemplar
        ON HistorialEstadoEjemplar(IdEjemplar);

    CREATE INDEX IX_HistorialEstadoEjemplar_FechaCambio
        ON HistorialEstadoEjemplar(FechaCambio DESC);

    CREATE INDEX IX_HistorialEstadoEjemplar_TipoCambio
        ON HistorialEstadoEjemplar(TipoCambio);

    CREATE INDEX IX_HistorialEstadoEjemplar_IdPrestamo
        ON HistorialEstadoEjemplar(IdPrestamo);

    PRINT 'Índices creados exitosamente.';

    PRINT '';
    PRINT '=== TABLA CREADA EXITOSAMENTE ===';
    PRINT '';
    PRINT 'La tabla HistorialEstadoEjemplar permitirá:';
    PRINT '  • Rastrear todos los cambios de estado de cada ejemplar';
    PRINT '  • Identificar quién realizó cada cambio';
    PRINT '  • Vincular cambios con préstamos/devoluciones';
    PRINT '  • Auditar el uso y mantenimiento de los ejemplares';
    PRINT '';
END
ELSE
BEGIN
    PRINT 'La tabla HistorialEstadoEjemplar ya existe. No se realizaron cambios.';
END

GO

-- Stored Procedure: Registrar cambio de estado
PRINT '';
PRINT 'Creando stored procedure: sp_RegistrarCambioEstadoEjemplar...';
GO

IF OBJECT_ID('sp_RegistrarCambioEstadoEjemplar', 'P') IS NOT NULL
    DROP PROCEDURE sp_RegistrarCambioEstadoEjemplar;
GO

CREATE PROCEDURE sp_RegistrarCambioEstadoEjemplar
    @IdEjemplar UNIQUEIDENTIFIER,
    @EstadoAnterior INT,
    @EstadoNuevo INT,
    @IdUsuario UNIQUEIDENTIFIER = NULL,
    @Motivo NVARCHAR(500) = NULL,
    @IdPrestamo UNIQUEIDENTIFIER = NULL,
    @IdDevolucion UNIQUEIDENTIFIER = NULL,
    @TipoCambio NVARCHAR(50) = 'Manual'
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar que el ejemplar existe
    IF NOT EXISTS (SELECT 1 FROM Ejemplar WHERE IdEjemplar = @IdEjemplar)
    BEGIN
        RAISERROR('El ejemplar no existe', 16, 1);
        RETURN;
    END

    -- No registrar si los estados son iguales
    IF @EstadoAnterior = @EstadoNuevo
    BEGIN
        PRINT 'No se registra cambio porque el estado no cambió.';
        RETURN;
    END

    -- Insertar registro de historial
    INSERT INTO HistorialEstadoEjemplar (
        IdHistorial,
        IdEjemplar,
        EstadoAnterior,
        EstadoNuevo,
        FechaCambio,
        IdUsuario,
        Motivo,
        IdPrestamo,
        IdDevolucion,
        TipoCambio
    )
    VALUES (
        NEWID(),
        @IdEjemplar,
        @EstadoAnterior,
        @EstadoNuevo,
        GETDATE(),
        @IdUsuario,
        @Motivo,
        @IdPrestamo,
        @IdDevolucion,
        @TipoCambio
    );

    PRINT 'Cambio de estado registrado en historial.';
END
GO

PRINT 'Stored procedure creado exitosamente.';
GO

-- Stored Procedure: Obtener historial de un ejemplar
PRINT '';
PRINT 'Creando stored procedure: sp_ObtenerHistorialEjemplar...';
GO

IF OBJECT_ID('sp_ObtenerHistorialEjemplar', 'P') IS NOT NULL
    DROP PROCEDURE sp_ObtenerHistorialEjemplar;
GO

CREATE PROCEDURE sp_ObtenerHistorialEjemplar
    @IdEjemplar UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.IdHistorial,
        h.IdEjemplar,
        h.EstadoAnterior,
        h.EstadoNuevo,
        h.FechaCambio,
        h.IdUsuario,
        h.Motivo,
        h.IdPrestamo,
        h.IdDevolucion,
        h.TipoCambio
    FROM HistorialEstadoEjemplar h
    WHERE h.IdEjemplar = @IdEjemplar
    ORDER BY h.FechaCambio DESC;
END
GO

PRINT 'Stored procedure creado exitosamente.';
GO

PRINT '';
PRINT '===============================================';
PRINT 'Script 40_CrearTablaHistorialEstadoEjemplar.sql completado.';
PRINT '===============================================';
PRINT '';
PRINT 'PRÓXIMOS PASOS:';
PRINT '1. Modificar EjemplarRepository para registrar cambios';
PRINT '2. Actualizar EjemplarBLL para usar el historial';
PRINT '3. Crear interfaz para visualizar historial';
PRINT '';
