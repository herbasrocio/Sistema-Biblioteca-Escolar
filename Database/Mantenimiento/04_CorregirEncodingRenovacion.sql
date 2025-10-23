-- =====================================================
-- Script: Corregir encoding de la patente Renovar Préstamo
-- Descripción: Actualiza el MenuItemName con la codificación correcta
-- Fecha: 2025-10-22
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT '========================================='
PRINT 'Corrigiendo encoding de patente'
PRINT '========================================='
PRINT ''

-- Actualizar el MenuItemName y Descripcion con la codificación correcta
UPDATE Patente
SET MenuItemName = N'Renovar Préstamo',
    Descripcion = N'Permite renovar préstamos activos extendiendo la fecha de devolución'
WHERE MenuItemName LIKE '%Renovar%Pr%stamo%';

IF @@ROWCOUNT > 0
    PRINT '✓ Patente corregida exitosamente'
ELSE
    PRINT '  → No se encontró la patente para corregir'

PRINT ''

-- Verificar el resultado
SELECT IdPatente, FormName, MenuItemName, Descripcion
FROM Patente
WHERE MenuItemName LIKE '%Renovar%Pr%stamo%';

PRINT ''
PRINT '========================================='
PRINT 'Corrección completada'
PRINT '========================================='
GO
