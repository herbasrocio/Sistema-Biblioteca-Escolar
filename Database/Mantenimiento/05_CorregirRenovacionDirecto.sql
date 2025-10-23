-- =====================================================
-- Script: Corregir patente Renovar Prestamo (sin acentos)
-- Descripcion: Actualiza el MenuItemName sin caracteres especiales
-- Fecha: 2025-10-22
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT '========================================='
PRINT 'Corrigiendo patente Renovar Prestamo'
PRINT '========================================='
PRINT ''

-- Opcion 1: Cambiar a "Renovar Prestamo" (sin acento)
UPDATE Patente
SET MenuItemName = 'Renovar Prestamo',
    Descripcion = 'Permite renovar prestamos activos extendiendo la fecha de devolucion'
WHERE MenuItemName LIKE '%Renovar%Pr%stamo%';

PRINT 'Filas actualizadas: '
SELECT @@ROWCOUNT AS FilasActualizadas;

PRINT ''

-- Verificar el resultado
SELECT IdPatente, FormName, MenuItemName, Descripcion
FROM Patente
WHERE MenuItemName LIKE '%Renovar%Pr%stamo%';

PRINT ''
PRINT '========================================='
PRINT 'Correccion completada'
PRINT '========================================='
GO
