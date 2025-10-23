-- =====================================================
-- Script: Agregar Campos de Renovación a Préstamos
-- Descripción: Agrega campos para controlar renovaciones
-- Fecha: 2025-10-22
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT '========================================='
PRINT 'Agregando campos de renovación a Prestamo'
PRINT '========================================='
PRINT ''

-- Verificar si la columna CantidadRenovaciones ya existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Prestamo') AND name = 'CantidadRenovaciones')
BEGIN
    ALTER TABLE Prestamo
    ADD CantidadRenovaciones INT NOT NULL DEFAULT 0;

    PRINT '✓ Campo CantidadRenovaciones agregado'
END
ELSE
BEGIN
    PRINT '  → Campo CantidadRenovaciones ya existe'
END

-- Verificar si la columna FechaUltimaRenovacion ya existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Prestamo') AND name = 'FechaUltimaRenovacion')
BEGIN
    ALTER TABLE Prestamo
    ADD FechaUltimaRenovacion DATETIME NULL;

    PRINT '✓ Campo FechaUltimaRenovacion agregado'
END
ELSE
BEGIN
    PRINT '  → Campo FechaUltimaRenovacion ya existe'
END

PRINT ''
PRINT '========================================='
PRINT 'Campos de renovación agregados exitosamente'
PRINT '========================================='
GO
