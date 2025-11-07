-- ============================================================================
-- Script: Unificar Permisos de Reportes
-- Descripción: Elimina las patentes individuales de reportes y crea una
--              patente unificada "Consultar Reportes" que da acceso a todos
-- Fecha: 2025-11-06
-- ============================================================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'UNIFICACIÓN DE PERMISOS DE REPORTES';
PRINT '========================================';
PRINT '';

-- ============================================================================
-- PASO 1: Identificar patentes individuales de reportes a eliminar
-- ============================================================================

PRINT 'Paso 1: Identificando patentes individuales de reportes...';
GO

DECLARE @PatentesAEliminar TABLE (
    IdPatente UNIQUEIDENTIFIER,
    FormName NVARCHAR(100),
    MenuItemName NVARCHAR(100)
);

-- Obtener IDs de las patentes individuales de reportes
INSERT INTO @PatentesAEliminar (IdPatente, FormName, MenuItemName)
SELECT IdPatente, FormName, MenuItemName
FROM Patente
WHERE FormName IN (
    'reportePrestamosActivos',
    'reporteMaterialesMasPrestados',
    'reporteUsoPorGrado',
    'reporteInventarioEjemplares',
    'reporteAlumnosMorosos'
)
OR (FormName = 'menu' AND MenuItemName = 'Consultar Reportes');

-- Mostrar patentes encontradas
IF EXISTS (SELECT 1 FROM @PatentesAEliminar)
BEGIN
    PRINT '  Patentes individuales encontradas:';
    SELECT '    • ' + FormName + ' (' + MenuItemName + ')' AS Patente
    FROM @PatentesAEliminar;
END
ELSE
BEGIN
    PRINT '  ℹ No se encontraron patentes individuales de reportes';
END

GO

-- ============================================================================
-- PASO 2: Eliminar relaciones de las patentes individuales
-- ============================================================================

PRINT '';
PRINT 'Paso 2: Eliminando relaciones de patentes individuales...';
GO

DECLARE @PatentesAEliminar TABLE (
    IdPatente UNIQUEIDENTIFIER,
    FormName NVARCHAR(100)
);

INSERT INTO @PatentesAEliminar (IdPatente, FormName)
SELECT IdPatente, FormName
FROM Patente
WHERE FormName IN (
    'reportePrestamosActivos',
    'reporteMaterialesMasPrestados',
    'reporteUsoPorGrado',
    'reporteInventarioEjemplares',
    'reporteAlumnosMorosos'
)
OR (FormName = 'menu' AND MenuItemName = 'Consultar Reportes');

-- Eliminar de FamiliaPatente
DECLARE @FilasEliminadasFP INT = 0;
DELETE fp
FROM FamiliaPatente fp
INNER JOIN @PatentesAEliminar p ON fp.IdPatente = p.IdPatente;

SET @FilasEliminadasFP = @@ROWCOUNT;
PRINT '  ✓ Eliminadas ' + CAST(@FilasEliminadasFP AS VARCHAR(10)) + ' relaciones FamiliaPatente';

-- Eliminar de UsuarioPatente
DECLARE @FilasEliminadasUP INT = 0;
DELETE up
FROM UsuarioPatente up
INNER JOIN @PatentesAEliminar p ON up.IdPatente = p.IdPatente;

SET @FilasEliminadasUP = @@ROWCOUNT;
PRINT '  ✓ Eliminadas ' + CAST(@FilasEliminadasUP AS VARCHAR(10)) + ' relaciones UsuarioPatente';

GO

-- ============================================================================
-- PASO 3: Eliminar las patentes individuales
-- ============================================================================

PRINT '';
PRINT 'Paso 3: Eliminando patentes individuales...';
GO

DECLARE @CantidadEliminada INT = 0;

DELETE FROM Patente
WHERE FormName IN (
    'reportePrestamosActivos',
    'reporteMaterialesMasPrestados',
    'reporteUsoPorGrado',
    'reporteInventarioEjemplares',
    'reporteAlumnosMorosos'
)
OR (FormName = 'menu' AND MenuItemName = 'Consultar Reportes');

SET @CantidadEliminada = @@ROWCOUNT;
PRINT '  ✓ Eliminadas ' + CAST(@CantidadEliminada AS VARCHAR(10)) + ' patentes individuales';

GO

-- ============================================================================
-- PASO 4: Crear patente unificada "Consultar Reportes"
-- ============================================================================

PRINT '';
PRINT 'Paso 4: Creando patente unificada "Consultar Reportes"...';
GO

DECLARE @IdPatenteConsultarReportes UNIQUEIDENTIFIER;

IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'consultarReportes')
BEGIN
    SET @IdPatenteConsultarReportes = NEWID();
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (
        @IdPatenteConsultarReportes,
        'consultarReportes',
        'Consultar Reportes',
        'Permite acceder a todos los reportes del sistema (Préstamos Activos, Materiales Más Prestados, Uso por Grado)'
    );
    PRINT '  ✓ Patente "Consultar Reportes" creada';
END
ELSE
BEGIN
    SELECT @IdPatenteConsultarReportes = IdPatente
    FROM Patente
    WHERE FormName = 'consultarReportes';
    PRINT '  ℹ Patente "Consultar Reportes" ya existe';
END

GO

-- ============================================================================
-- PASO 5: Asignar patente a la familia "Módulo Reportes"
-- ============================================================================

PRINT '';
PRINT 'Paso 5: Asignando patente a "Módulo Reportes"...';
GO

DECLARE @IdFamiliaReportes UNIQUEIDENTIFIER;
DECLARE @IdPatenteConsultarReportes UNIQUEIDENTIFIER;

-- Obtener ID de la familia "Módulo Reportes" (crear si no existe)
IF NOT EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'Módulo Reportes')
BEGIN
    SET @IdFamiliaReportes = NEWID();
    INSERT INTO Familia (IdFamilia, Nombre, Descripcion)
    VALUES (
        @IdFamiliaReportes,
        'Módulo Reportes',
        'Grupo de permisos para visualizar reportes'
    );
    PRINT '  ✓ Familia "Módulo Reportes" creada';
END
ELSE
BEGIN
    SELECT @IdFamiliaReportes = IdFamilia
    FROM Familia
    WHERE Nombre = 'Módulo Reportes';
    PRINT '  ℹ Familia "Módulo Reportes" ya existe';
END

-- Obtener ID de la patente
SELECT @IdPatenteConsultarReportes = IdPatente
FROM Patente
WHERE FormName = 'consultarReportes';

-- Asignar patente a familia
IF NOT EXISTS (
    SELECT 1 FROM FamiliaPatente
    WHERE IdFamilia = @IdFamiliaReportes
    AND IdPatente = @IdPatenteConsultarReportes
)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    VALUES (@IdFamiliaReportes, @IdPatenteConsultarReportes);
    PRINT '  ✓ Patente "Consultar Reportes" asignada a "Módulo Reportes"';
END
ELSE
BEGIN
    PRINT '  ℹ Patente "Consultar Reportes" ya está asignada a "Módulo Reportes"';
END

GO

-- ============================================================================
-- PASO 6: Asignar familia "Módulo Reportes" a ROL_Administrador
-- ============================================================================

PRINT '';
PRINT 'Paso 6: Asignando "Módulo Reportes" a ROL_Administrador...';
GO

DECLARE @IdRolAdministrador UNIQUEIDENTIFIER;
DECLARE @IdFamiliaReportes UNIQUEIDENTIFIER;

SELECT @IdRolAdministrador = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdFamiliaReportes = IdFamilia FROM Familia WHERE Nombre = 'Módulo Reportes';

IF @IdRolAdministrador IS NULL
BEGIN
    PRINT '  ❌ ERROR: No se encontró el rol "ROL_Administrador"';
END
ELSE IF @IdFamiliaReportes IS NULL
BEGIN
    PRINT '  ❌ ERROR: No se encontró la familia "Módulo Reportes"';
END
ELSE
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM FamiliaFamilia
        WHERE IdFamiliaPadre = @IdRolAdministrador
        AND IdFamiliaHijo = @IdFamiliaReportes
    )
    BEGIN
        INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo)
        VALUES (@IdRolAdministrador, @IdFamiliaReportes);
        PRINT '  ✓ "Módulo Reportes" asignado a "ROL_Administrador"';
    END
    ELSE
    BEGIN
        PRINT '  ℹ "Módulo Reportes" ya está asignado a "ROL_Administrador"';
    END
END

GO

-- ============================================================================
-- PASO 7: Verificación final
-- ============================================================================

PRINT '';
PRINT '========================================';
PRINT 'VERIFICACIÓN FINAL';
PRINT '========================================';
GO

-- Verificar que no existen patentes individuales
PRINT '';
PRINT 'Patentes individuales restantes (debe ser 0):';
SELECT COUNT(*) AS CantidadPatentesIndividuales
FROM Patente
WHERE FormName IN (
    'reportePrestamosActivos',
    'reporteMaterialesMasPrestados',
    'reporteUsoPorGrado',
    'reporteInventarioEjemplares',
    'reporteAlumnosMorosos'
)
OR (FormName = 'menu' AND MenuItemName = 'Consultar Reportes');

-- Verificar patente unificada
PRINT '';
PRINT 'Patente unificada creada:';
SELECT
    FormName,
    MenuItemName,
    Descripcion
FROM Patente
WHERE FormName = 'consultarReportes';

-- Verificar asignación a familia
PRINT '';
PRINT 'Patentes en "Módulo Reportes":';
SELECT
    p.FormName,
    p.MenuItemName
FROM FamiliaPatente fp
INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'Módulo Reportes';

-- Verificar asignación a rol Administrador
PRINT '';
PRINT 'Familias asignadas a ROL_Administrador (debe incluir Módulo Reportes):';
SELECT
    f2.Nombre AS FamiliaAsignada,
    f2.Descripcion
FROM Familia f
INNER JOIN FamiliaFamilia ff ON f.IdFamilia = ff.IdFamiliaPadre
INNER JOIN Familia f2 ON ff.IdFamiliaHijo = f2.IdFamilia
WHERE f.Nombre = 'ROL_Administrador'
ORDER BY f2.Nombre;

PRINT '';
PRINT '========================================';
PRINT '✓ UNIFICACIÓN COMPLETADA EXITOSAMENTE';
PRINT '========================================';
PRINT '';
PRINT 'RESUMEN:';
PRINT '  • Patentes individuales de reportes eliminadas';
PRINT '  • Patente unificada "Consultar Reportes" creada';
PRINT '  • Asignada a familia "Módulo Reportes"';
PRINT '  • "Módulo Reportes" asignada a ROL_Administrador';
PRINT '';
PRINT 'NOTA IMPORTANTE:';
PRINT '  La clase Usuario.cs ya contiene la lógica en TienePermiso()';
PRINT '  que hace que "Consultar Reportes" dé acceso a todos los reportes.';
PRINT '  No se requieren cambios adicionales en el código.';
PRINT '';

GO
