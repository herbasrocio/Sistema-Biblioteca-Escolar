-- =============================================
-- Script: Corregir FormName de la patente Renovar Préstamo
-- Fecha: 2025-11-03
-- Descripción: Actualiza FormName de 'menu' a 'renovarPrestamo'
--              para corregir el control de permisos
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT '=========================================='
PRINT 'Corrigiendo FormName de Renovar Préstamo'
PRINT '=========================================='
PRINT ''

-- Verificar si la patente existe con FormName incorrecto
IF EXISTS (
    SELECT 1 FROM Patente
    WHERE MenuItemName LIKE '%Renovar%Pr%stamo%'
    AND FormName = 'menu'
)
BEGIN
    PRINT 'Estado actual (ANTES de la corrección):'
    SELECT IdPatente, FormName, MenuItemName, Descripcion
    FROM Patente
    WHERE MenuItemName LIKE '%Renovar%Pr%stamo%';

    PRINT ''
    PRINT 'Aplicando corrección...'

    -- Actualizar FormName a 'renovarPrestamo'
    UPDATE Patente
    SET FormName = 'renovarPrestamo'
    WHERE MenuItemName LIKE '%Renovar%Pr%stamo%'
    AND FormName = 'menu';

    PRINT '✓ FormName actualizado de ''menu'' a ''renovarPrestamo'''
    PRINT ''
    PRINT 'Estado actualizado (DESPUÉS de la corrección):'
    SELECT IdPatente, FormName, MenuItemName, Descripcion
    FROM Patente
    WHERE MenuItemName LIKE '%Renovar%Pr%stamo%';

    PRINT ''
    PRINT '✓ Corrección aplicada exitosamente'
END
ELSE IF EXISTS (
    SELECT 1 FROM Patente
    WHERE MenuItemName LIKE '%Renovar%Pr%stamo%'
    AND FormName = 'renovarPrestamo'
)
BEGIN
    PRINT '✓ La patente ya tiene el FormName correcto:'
    SELECT IdPatente, FormName, MenuItemName, Descripcion
    FROM Patente
    WHERE MenuItemName LIKE '%Renovar%Pr%stamo%';
    PRINT ''
    PRINT '⚠ No se requiere ninguna acción'
END
ELSE
BEGIN
    PRINT '⚠ ADVERTENCIA: No se encontró la patente "Renovar Préstamo"'
    PRINT 'Ejecute primero: Database\Mantenimiento\03_AgregarPatenteRenovacion.sql'
END

PRINT ''
PRINT '=========================================='
PRINT 'Script completado'
PRINT '=========================================='
GO
