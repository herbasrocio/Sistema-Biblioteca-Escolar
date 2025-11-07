-- =============================================
-- Script: CORREGIR_ENCODING_ADICIONAL.sql
-- Descripcion: Corrige patentes restantes con encoding corrupto
-- Fecha: 2025-11-06
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'CORRECCION ADICIONAL DE ENCODING';
PRINT '========================================';
PRINT '';

-- Gestion Catalogo (con diferentes variaciones)
UPDATE Patente
SET MenuItemName = 'Gestion Catalogo'
WHERE FormName = 'menu' AND (
    MenuItemName LIKE '%Cat_logo%' OR
    MenuItemName LIKE '%Cat%logo%' OR
    (MenuItemName LIKE '%Catalogo%' AND MenuItemName LIKE '%Gesti%')
);
PRINT 'Gestion Catalogo actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Gestion Prestamos
UPDATE Patente
SET MenuItemName = 'Gestion Prestamos'
WHERE FormName = 'menu' AND (
    MenuItemName LIKE '%Pr_stamos%' OR
    MenuItemName LIKE '%Pr%stamos%' OR
    (MenuItemName LIKE '%Prestamos%' AND MenuItemName LIKE '%Gesti%')
);
PRINT 'Gestion Prestamos actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Consultar Material (con catalog en descripcion)
UPDATE Patente
SET MenuItemName = 'Consultar Material',
    Descripcion = 'Acceso a consulta de materiales del catalogo'
WHERE FormName = 'menu' AND MenuItemName = 'Consultar Material';
PRINT 'Consultar Material actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Editar Material
UPDATE Patente
SET MenuItemName = 'Editar Material',
    Descripcion = 'Acceso a edicion de materiales del catalogo'
WHERE FormName = 'menu' AND MenuItemName = 'Editar Material';
PRINT 'Editar Material actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Gestionar Ejemplares
UPDATE Patente
SET MenuItemName = 'Gestionar Ejemplares',
    Descripcion = 'Acceso a gestion de ejemplares (copias fisicas) de materiales'
WHERE FormName = 'menu' AND MenuItemName = 'Gestionar Ejemplares';
PRINT 'Gestionar Ejemplares actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Registrar Material
UPDATE Patente
SET MenuItemName = 'Registrar Material',
    Descripcion = 'Acceso a registro y modificacion de materiales'
WHERE FormName = 'menu' AND MenuItemName = 'Registrar Material';
PRINT 'Registrar Material actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Gestion Alumnos
UPDATE Patente
SET Descripcion = 'Acceso al modulo de Gestion de Alumnos'
WHERE FormName = 'menu' AND MenuItemName = 'Gestion Alumnos';
PRINT 'Gestion Alumnos descripcion actualizada: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Gestion Usuarios
UPDATE Patente
SET Descripcion = 'Acceso al modulo de Gestion de Usuarios'
WHERE FormName = 'menu' AND MenuItemName = 'Gestion Usuarios';
PRINT 'Gestion Usuarios descripcion actualizada: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Gestion Permisos
UPDATE Patente
SET Descripcion = 'Acceso al modulo de Gestion de Permisos y Roles'
WHERE FormName = 'menu' AND MenuItemName = 'Gestion Permisos';
PRINT 'Gestion Permisos descripcion actualizada: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

-- Gestion Devoluciones
UPDATE Patente
SET Descripcion = 'Acceso al modulo de Gestion de Devoluciones'
WHERE FormName = 'menu' AND MenuItemName = 'Gestion Devoluciones';
PRINT 'Gestion Devoluciones descripcion actualizada: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' filas';

PRINT '';
PRINT '========================================';
PRINT 'VERIFICACION FINAL';
PRINT '========================================';
PRINT '';

SELECT FormName, MenuItemName, Descripcion
FROM Patente
WHERE FormName = 'menu'
ORDER BY MenuItemName;

GO
