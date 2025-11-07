-- =============================================
-- Script: VERIFICAR_PERMISO_GESTION_PERMISOS.sql
-- Descripcion: Verifica si existe el permiso "Gestion Permisos"
--              y si esta asignado al ROL_Administrador
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'VERIFICACION: Permiso Gestion Permisos';
PRINT '========================================';
PRINT '';

-- Buscar la patente
PRINT 'Buscando patente "Gestion Permisos"...';
SELECT
    IdPatente,
    FormName,
    MenuItemName,
    Descripcion
FROM Patente
WHERE MenuItemName LIKE '%Gestion%Permiso%'
   OR MenuItemName LIKE '%Gestion Permisos%'
   OR FormName LIKE '%permiso%';

PRINT '';
PRINT 'Buscando ROL_Administrador...';
SELECT IdFamilia, Nombre, Descripcion
FROM Familia
WHERE Nombre = 'ROL_Administrador';

PRINT '';
PRINT 'Patentes asignadas a ROL_Administrador:';
SELECT
    p.FormName,
    p.MenuItemName,
    p.Descripcion
FROM FamiliaPatente fp
INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'ROL_Administrador'
ORDER BY p.MenuItemName;

GO
