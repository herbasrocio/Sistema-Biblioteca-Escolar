-- =============================================
-- Script: REPARAR_PERMISOS_BITACORA.sql
-- Descripcion: Repara los permisos de bitacora eliminando datos corruptos
--              y creando las patentes correctas SIN simbolos especiales
-- Fecha: 2025-11-06
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'REPARACION DE PERMISOS DE BITACORA';
PRINT '========================================';
PRINT '';

-- =============================================
-- PASO 1: Eliminar patentes corruptas o antiguas
-- =============================================

PRINT 'Paso 1: Eliminando patentes antiguas o corruptas...';
GO

DECLARE @PatentesAEliminar TABLE (
    IdPatente UNIQUEIDENTIFIER,
    FormName NVARCHAR(100)
);

-- Obtener IDs de todas las patentes de bitacora (antiguas y nuevas)
INSERT INTO @PatentesAEliminar (IdPatente, FormName)
SELECT IdPatente, FormName
FROM Patente
WHERE FormName IN (
    'consultarBitacoraAdmin',
    'consultarBitacoraBibliotecario',
    'consultarBitacoraSeguridad',
    'consultarBitacoraOperaciones'
);

-- Eliminar de FamiliaPatente
DELETE fp
FROM FamiliaPatente fp
INNER JOIN @PatentesAEliminar p ON fp.IdPatente = p.IdPatente;

PRINT '  - Relaciones FamiliaPatente eliminadas: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- Eliminar de UsuarioPatente
DELETE up
FROM UsuarioPatente up
INNER JOIN @PatentesAEliminar p ON up.IdPatente = p.IdPatente;

PRINT '  - Relaciones UsuarioPatente eliminadas: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- Eliminar las patentes
DELETE FROM Patente
WHERE FormName IN (
    'consultarBitacoraAdmin',
    'consultarBitacoraBibliotecario',
    'consultarBitacoraSeguridad',
    'consultarBitacoraOperaciones'
);

PRINT '  - Patentes eliminadas: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
PRINT '';

GO

-- =============================================
-- PASO 2: Crear patentes limpias
-- =============================================

PRINT 'Paso 2: Creando patentes limpias...';
GO

DECLARE @IdPatenteSeguridad UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteOperaciones UNIQUEIDENTIFIER = NEWID();

-- Crear patente de Bitacora de Seguridad
INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
VALUES (
    @IdPatenteSeguridad,
    'consultarBitacoraSeguridad',
    'Consultar Bitacora de Seguridad',
    'Permite consultar la bitacora de eventos de seguridad (autenticacion, permisos, usuarios)'
);

PRINT '  - Patente consultarBitacoraSeguridad creada';

-- Crear patente de Bitacora de Operaciones
INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
VALUES (
    @IdPatenteOperaciones,
    'consultarBitacoraOperaciones',
    'Consultar Bitacora de Operaciones',
    'Permite consultar la bitacora de operaciones del negocio (prestamos, devoluciones, materiales)'
);

PRINT '  - Patente consultarBitacoraOperaciones creada';
PRINT '';

GO

-- =============================================
-- PASO 3: Asignar patentes a ROL_Administrador
-- =============================================

PRINT 'Paso 3: Asignando patentes a ROL_Administrador...';
GO

DECLARE @IdRolAdmin UNIQUEIDENTIFIER;
DECLARE @IdPatenteSeguridad UNIQUEIDENTIFIER;
DECLARE @IdPatenteOperaciones UNIQUEIDENTIFIER;

-- Obtener IDs
SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdPatenteSeguridad = IdPatente FROM Patente WHERE FormName = 'consultarBitacoraSeguridad';
SELECT @IdPatenteOperaciones = IdPatente FROM Patente WHERE FormName = 'consultarBitacoraOperaciones';

IF @IdRolAdmin IS NULL
BEGIN
    PRINT '  ERROR: No se encontro el rol ROL_Administrador';
END
ELSE
BEGIN
    -- Asignar patente de seguridad
    IF @IdPatenteSeguridad IS NOT NULL
    BEGIN
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdRolAdmin, @IdPatenteSeguridad);

        PRINT '  - consultarBitacoraSeguridad asignada a ROL_Administrador';
    END

    -- Asignar patente de operaciones
    IF @IdPatenteOperaciones IS NOT NULL
    BEGIN
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdRolAdmin, @IdPatenteOperaciones);

        PRINT '  - consultarBitacoraOperaciones asignada a ROL_Administrador';
    END
END

PRINT '';

GO

-- =============================================
-- PASO 4: Verificacion final
-- =============================================

PRINT '========================================';
PRINT 'VERIFICACION FINAL';
PRINT '========================================';
PRINT '';

PRINT 'Patentes de bitacora en el sistema:';
SELECT FormName, MenuItemName, Descripcion
FROM Patente
WHERE FormName LIKE '%bitacora%' OR FormName LIKE '%Bitacora%'
ORDER BY FormName;

PRINT '';
PRINT 'Asignaciones a roles:';
SELECT
    f.Nombre AS Rol,
    p.FormName,
    p.MenuItemName
FROM FamiliaPatente fp
INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE p.FormName LIKE '%bitacora%' OR p.FormName LIKE '%Bitacora%'
ORDER BY f.Nombre, p.FormName;

PRINT '';
PRINT '========================================';
PRINT 'REPARACION COMPLETADA';
PRINT '========================================';
PRINT '';
PRINT 'IMPORTANTE:';
PRINT '  1. Cierre la aplicacion si esta abierta';
PRINT '  2. Recompile el proyecto';
PRINT '  3. Inicie sesion nuevamente';
PRINT '';

GO
