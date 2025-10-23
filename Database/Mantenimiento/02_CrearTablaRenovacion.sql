-- =====================================================
-- Script: Crear Tabla RenovacionPrestamo
-- Descripción: Tabla de auditoría para renovaciones
-- Fecha: 2025-10-22
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT '========================================='
PRINT 'Creando tabla RenovacionPrestamo'
PRINT '========================================='
PRINT ''

-- Verificar si la tabla ya existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('RenovacionPrestamo') AND type = 'U')
BEGIN
    CREATE TABLE RenovacionPrestamo (
        IdRenovacion UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        IdPrestamo UNIQUEIDENTIFIER NOT NULL,
        FechaRenovacion DATETIME NOT NULL DEFAULT GETDATE(),
        FechaDevolucionAnterior DATETIME NOT NULL,
        FechaDevolucionNueva DATETIME NOT NULL,
        IdUsuario UNIQUEIDENTIFIER NOT NULL, -- Usuario que procesó la renovación
        Observaciones NVARCHAR(500) NULL,

        CONSTRAINT FK_RenovacionPrestamo_Prestamo FOREIGN KEY (IdPrestamo) REFERENCES Prestamo(IdPrestamo)
    );

    PRINT '✓ Tabla RenovacionPrestamo creada exitosamente'
    PRINT ''

    -- Crear índice para búsquedas por préstamo
    CREATE INDEX IX_RenovacionPrestamo_IdPrestamo ON RenovacionPrestamo(IdPrestamo);
    PRINT '✓ Índice IX_RenovacionPrestamo_IdPrestamo creado'

    -- Crear índice para búsquedas por fecha
    CREATE INDEX IX_RenovacionPrestamo_FechaRenovacion ON RenovacionPrestamo(FechaRenovacion);
    PRINT '✓ Índice IX_RenovacionPrestamo_FechaRenovacion creado'
END
ELSE
BEGIN
    PRINT '  → Tabla RenovacionPrestamo ya existe'
END

PRINT ''
PRINT '========================================='
PRINT 'Tabla de auditoría creada exitosamente'
PRINT '========================================='
GO
