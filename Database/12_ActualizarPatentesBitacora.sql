-- =============================================
-- Script: Actualizar patentes de bitácora con nuevos nombres
-- Descripción: Actualiza los FormName y MenuItemName para reflejar
--              los nuevos nombres de BitacoraSeguridad y BitacoraOperaciones
-- Fecha: 2025-11-03
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT 'Actualizando patentes de bitácora con nuevos nombres...'
PRINT ''

-- Actualizar patente de BitacoraAdmin → BitacoraSeguridad
IF EXISTS (SELECT 1 FROM Patente WHERE FormName = 'consultarBitacoraAdmin')
BEGIN
    UPDATE Patente
    SET FormName = 'consultarBitacoraSeguridad',
        MenuItemName = 'Consultar Bitácora de Seguridad',
        Descripcion = 'Permite consultar la bitácora de eventos de seguridad (autenticación, permisos, usuarios)'
    WHERE FormName = 'consultarBitacoraAdmin';

    PRINT '✓ Patente actualizada: consultarBitacoraAdmin → consultarBitacoraSeguridad';
END
ELSE IF EXISTS (SELECT 1 FROM Patente WHERE FormName = 'consultarBitacoraSeguridad')
BEGIN
    PRINT '✓ La patente consultarBitacoraSeguridad ya existe';
END
ELSE
BEGIN
    -- Crear la patente si no existe
    INSERT INTO Patente (FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        'consultarBitacoraSeguridad',
        'Consultar Bitácora de Seguridad',
        1,
        'Permite consultar la bitácora de eventos de seguridad (autenticación, permisos, usuarios)'
    );
    PRINT '✓ Patente creada: consultarBitacoraSeguridad';
END
GO

-- Actualizar patente de BitacoraBibliotecario → BitacoraOperaciones
IF EXISTS (SELECT 1 FROM Patente WHERE FormName = 'consultarBitacoraBibliotecario')
BEGIN
    UPDATE Patente
    SET FormName = 'consultarBitacoraOperaciones',
        MenuItemName = 'Consultar Bitácora de Operaciones',
        Descripcion = 'Permite consultar la bitácora de operaciones del negocio (préstamos, devoluciones, materiales)'
    WHERE FormName = 'consultarBitacoraBibliotecario';

    PRINT '✓ Patente actualizada: consultarBitacoraBibliotecario → consultarBitacoraOperaciones';
END
ELSE IF EXISTS (SELECT 1 FROM Patente WHERE FormName = 'consultarBitacoraOperaciones')
BEGIN
    PRINT '✓ La patente consultarBitacoraOperaciones ya existe';
END
ELSE
BEGIN
    -- Crear la patente si no existe
    INSERT INTO Patente (FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        'consultarBitacoraOperaciones',
        'Consultar Bitácora de Operaciones',
        2,
        'Permite consultar la bitácora de operaciones del negocio (préstamos, devoluciones, materiales)'
    );
    PRINT '✓ Patente creada: consultarBitacoraOperaciones';
END
GO

-- Verificar patentes actualizadas
PRINT ''
PRINT 'Patentes de bitácora actuales:'
SELECT FormName, MenuItemName, Descripcion
FROM Patente
WHERE FormName LIKE '%bitacora%' OR FormName LIKE '%Bitacora%'
ORDER BY Orden, FormName;
GO

-- Verificar asignaciones a roles
PRINT ''
PRINT 'Asignaciones de patentes de bitácora a roles:'
SELECT
    f.Nombre AS Rol,
    p.FormName,
    p.MenuItemName
FROM FamiliaPatente fp
INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE p.FormName LIKE '%bitacora%' OR p.FormName LIKE '%Bitacora%'
ORDER BY f.Nombre, p.FormName;
GO

PRINT ''
PRINT '=== Script completado exitosamente ==='
PRINT ''
GO
