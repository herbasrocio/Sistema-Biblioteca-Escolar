-- =============================================
-- Script: CORREGIR_ENCODING_PATENTES.sql
-- Descripcion: Corrige el encoding de todas las patentes con
--              caracteres especiales (tildes, simbolos)
-- Fecha: 2025-11-06
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'CORRECCION DE ENCODING DE PATENTES';
PRINT '========================================';
PRINT '';

-- =============================================
-- Corregir patentes del menu principal
-- =============================================

PRINT 'Corrigiendo patentes del menu...';

-- Gestion Usuarios
UPDATE Patente
SET MenuItemName = 'Gestion Usuarios'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Usuarios%' AND MenuItemName LIKE '%Gesti%';
PRINT '  - Gestion Usuarios: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

-- Gestion Permisos
UPDATE Patente
SET MenuItemName = 'Gestion Permisos'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Permisos%' AND MenuItemName LIKE '%Gesti%';
PRINT '  - Gestion Permisos: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

-- Gestion Catalogo
UPDATE Patente
SET MenuItemName = 'Gestion Catalogo'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Catalogo%' OR MenuItemName LIKE '%Cat_logo%';
PRINT '  - Gestion Catalogo: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

-- Gestion Alumnos
UPDATE Patente
SET MenuItemName = 'Gestion Alumnos'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Alumnos%' AND MenuItemName LIKE '%Gesti%';
PRINT '  - Gestion Alumnos: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

-- Gestion Prestamos
UPDATE Patente
SET MenuItemName = 'Gestion Prestamos'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Prestamos%' AND MenuItemName LIKE '%Gesti%';
PRINT '  - Gestion Prestamos: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

-- Gestion Devoluciones
UPDATE Patente
SET MenuItemName = 'Gestion Devoluciones'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Devoluciones%' AND MenuItemName LIKE '%Gesti%';
PRINT '  - Gestion Devoluciones: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

-- Edicion
UPDATE Patente
SET MenuItemName = 'Editar Material'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Editar%' AND MenuItemName LIKE '%edici%';
PRINT '  - Editar Material: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

-- Consultar Material
UPDATE Patente
SET MenuItemName = 'Consultar Material'
WHERE FormName = 'menu' AND MenuItemName LIKE '%Consultar%' AND MenuItemName LIKE '%catalogo%';
PRINT '  - Consultar Material: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas actualizadas';

PRINT '';

GO

-- =============================================
-- Verificacion
-- =============================================

PRINT '========================================';
PRINT 'VERIFICACION';
PRINT '========================================';
PRINT '';

PRINT 'Todas las patentes del menu:';
SELECT FormName, MenuItemName, Descripcion
FROM Patente
WHERE FormName = 'menu'
ORDER BY MenuItemName;

PRINT '';
PRINT '========================================';
PRINT 'CORRECCION COMPLETADA';
PRINT '========================================';
PRINT '';

GO
