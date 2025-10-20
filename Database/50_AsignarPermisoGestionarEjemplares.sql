-- Script para asignar el permiso "Gestionar Ejemplares" al rol Administrador
-- Fecha: 2025-10-13

USE SeguridadBiblioteca;
GO

-- 1. Verificar si existe el permiso "Gestionar Ejemplares"
SELECT 'Permiso encontrado' AS Estado, *
FROM Patente
WHERE MenuItemName = 'Gestionar Ejemplares';
GO

-- 2. Verificar si existe el rol Administrador
SELECT 'Rol encontrado' AS Estado, *
FROM Familia
WHERE Nombre = 'ROL_Administrador';
GO

-- 3. Verificar si ya está asignado el permiso al rol
SELECT 'Asignación actual' AS Estado,
       F.Nombre,
       P.MenuItemName
FROM FamiliaPatente FP
INNER JOIN Familia F ON FP.IdFamilia = F.IdFamilia
INNER JOIN Patente P ON FP.IdPatente = P.IdPatente
WHERE F.Nombre = 'ROL_Administrador'
  AND P.MenuItemName = 'Gestionar Ejemplares';
GO

-- 4. Si no está asignado, asignarlo
DECLARE @IdRolAdmin UNIQUEIDENTIFIER;
DECLARE @IdPermisoEjemplares UNIQUEIDENTIFIER;

-- Obtener ID del rol Administrador
SELECT @IdRolAdmin = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Administrador';

-- Obtener ID del permiso "Gestionar Ejemplares"
SELECT @IdPermisoEjemplares = IdPatente
FROM Patente
WHERE MenuItemName = 'Gestionar Ejemplares';

-- Verificar que ambos existen
IF @IdRolAdmin IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el rol Administrador';
END
ELSE IF @IdPermisoEjemplares IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el permiso "Gestionar Ejemplares"';
END
ELSE
BEGIN
    -- Verificar si ya está asignado
    IF NOT EXISTS (
        SELECT 1
        FROM FamiliaPatente
        WHERE IdFamilia = @IdRolAdmin
          AND IdPatente = @IdPermisoEjemplares
    )
    BEGIN
        -- Asignar el permiso al rol
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdRolAdmin, @IdPermisoEjemplares);

        PRINT 'ÉXITO: Permiso "Gestionar Ejemplares" asignado al rol Administrador';
    END
    ELSE
    BEGIN
        PRINT 'INFO: El permiso ya está asignado al rol Administrador';
    END
END
GO

-- 5. Verificar todos los permisos del rol Administrador
SELECT 'Permisos del Administrador' AS Info,
       F.Nombre,
       P.MenuItemName,
       P.Descripcion
FROM FamiliaPatente FP
INNER JOIN Familia F ON FP.IdFamilia = F.IdFamilia
INNER JOIN Patente P ON FP.IdPatente = P.IdPatente
WHERE F.Nombre = 'ROL_Administrador'
ORDER BY P.MenuItemName;
GO
