-- =============================================
-- Script: Agregar Permisos para Módulo de Reportes
-- Fecha: 2025-01-24
-- Descripción: Crea patentes y permisos para el módulo de reportes
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT 'Iniciando creación de permisos para módulo de reportes...';
GO

-- =============================================
-- 1. CREAR PATENTES PARA REPORTES
-- =============================================

-- Verificar si ya existen las patentes
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'reportePrestamosActivos')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (NEWID(), 'reportePrestamosActivos', 'Préstamos Activos', 'Reporte de préstamos vigentes, vencidos y por vencer');
    PRINT '✓ Patente "reportePrestamosActivos" creada';
END
ELSE
    PRINT '⚠ Patente "reportePrestamosActivos" ya existe';

IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'reporteMaterialesMasPrestados')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (NEWID(), 'reporteMaterialesMasPrestados', 'Materiales Más Prestados', 'Estadísticas de demanda de materiales');
    PRINT '✓ Patente "reporteMaterialesMasPrestados" creada';
END
ELSE
    PRINT '⚠ Patente "reporteMaterialesMasPrestados" ya existe';

IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'reporteInventarioEjemplares')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (NEWID(), 'reporteInventarioEjemplares', 'Inventario de Ejemplares', 'Estado del catálogo físico por ejemplar');
    PRINT '✓ Patente "reporteInventarioEjemplares" creada';
END
ELSE
    PRINT '⚠ Patente "reporteInventarioEjemplares" ya existe';

IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'reporteAlumnosMorosos')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (NEWID(), 'reporteAlumnosMorosos', 'Alumnos Morosos', 'Alumnos con préstamos vencidos');
    PRINT '✓ Patente "reporteAlumnosMorosos" creada';
END
ELSE
    PRINT '⚠ Patente "reporteAlumnosMorosos" ya existe';

GO

-- =============================================
-- 2. CREAR FAMILIA "MÓDULO REPORTES"
-- =============================================

-- Verificar si ya existe la familia
DECLARE @IdFamiliaReportes UNIQUEIDENTIFIER;

IF NOT EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'Módulo Reportes')
BEGIN
    SET @IdFamiliaReportes = NEWID();
    INSERT INTO Familia (IdFamilia, Nombre, Descripcion, EsRol)
    VALUES (@IdFamiliaReportes, 'Módulo Reportes', 'Grupo de permisos para visualizar reportes', 0);
    PRINT '✓ Familia "Módulo Reportes" creada';
END
ELSE
BEGIN
    SELECT @IdFamiliaReportes = IdFamilia FROM Familia WHERE Nombre = 'Módulo Reportes';
    PRINT '⚠ Familia "Módulo Reportes" ya existe';
END

-- Asignar patentes a la familia
IF NOT EXISTS (
    SELECT 1 FROM FamiliaPatente fp
    INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
    WHERE fp.IdFamilia = @IdFamiliaReportes AND p.FormName = 'reportePrestamosActivos'
)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    SELECT @IdFamiliaReportes, IdPatente FROM Patente WHERE FormName = 'reportePrestamosActivos';
    PRINT '✓ Patente "reportePrestamosActivos" asignada a "Módulo Reportes"';
END

IF NOT EXISTS (
    SELECT 1 FROM FamiliaPatente fp
    INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
    WHERE fp.IdFamilia = @IdFamiliaReportes AND p.FormName = 'reporteMaterialesMasPrestados'
)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    SELECT @IdFamiliaReportes, IdPatente FROM Patente WHERE FormName = 'reporteMaterialesMasPrestados';
    PRINT '✓ Patente "reporteMaterialesMasPrestados" asignada a "Módulo Reportes"';
END

IF NOT EXISTS (
    SELECT 1 FROM FamiliaPatente fp
    INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
    WHERE fp.IdFamilia = @IdFamiliaReportes AND p.FormName = 'reporteInventarioEjemplares'
)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    SELECT @IdFamiliaReportes, IdPatente FROM Patente WHERE FormName = 'reporteInventarioEjemplares';
    PRINT '✓ Patente "reporteInventarioEjemplares" asignada a "Módulo Reportes"';
END

IF NOT EXISTS (
    SELECT 1 FROM FamiliaPatente fp
    INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
    WHERE fp.IdFamilia = @IdFamiliaReportes AND p.FormName = 'reporteAlumnosMorosos'
)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    SELECT @IdFamiliaReportes, IdPatente FROM Patente WHERE FormName = 'reporteAlumnosMorosos';
    PRINT '✓ Patente "reporteAlumnosMorosos" asignada a "Módulo Reportes"';
END

GO

-- =============================================
-- 3. ASIGNAR FAMILIA "MÓDULO REPORTES" A ROL ADMINISTRADOR
-- =============================================

DECLARE @IdRolAdministrador UNIQUEIDENTIFIER;
DECLARE @IdFamiliaReportes UNIQUEIDENTIFIER;

SELECT @IdRolAdministrador = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdFamiliaReportes = IdFamilia FROM Familia WHERE Nombre = 'Módulo Reportes';

IF @IdRolAdministrador IS NULL
BEGIN
    PRINT '❌ ERROR: No se encontró el rol "ROL_Administrador"';
END
ELSE IF @IdFamiliaReportes IS NULL
BEGIN
    PRINT '❌ ERROR: No se encontró la familia "Módulo Reportes"';
END
ELSE
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM FamiliaFamilia
        WHERE IdFamiliaPadre = @IdRolAdministrador
        AND IdFamiliaHija = @IdFamiliaReportes
    )
    BEGIN
        INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHija)
        VALUES (@IdRolAdministrador, @IdFamiliaReportes);
        PRINT '✓ "Módulo Reportes" asignado a "ROL_Administrador"';
    END
    ELSE
        PRINT '⚠ "Módulo Reportes" ya está asignado a "ROL_Administrador"';
END

GO

-- =============================================
-- 4. VERIFICACIÓN
-- =============================================

PRINT '';
PRINT '=== VERIFICACIÓN DE PERMISOS CREADOS ===';
PRINT '';

-- Mostrar patentes creadas
SELECT
    'PATENTE' AS Tipo,
    FormName AS Nombre,
    MenuItemName AS Display,
    Descripcion
FROM Patente
WHERE FormName LIKE 'reporte%'
ORDER BY FormName;

PRINT '';
PRINT '=== PERMISOS DEL ADMINISTRADOR ===';

-- Verificar permisos del rol Administrador
SELECT
    p.FormName AS Permiso,
    p.MenuItemName AS Display,
    'ROL_Administrador' AS Rol
FROM Familia f
INNER JOIN FamiliaFamilia ff ON f.IdFamilia = ff.IdFamiliaPadre
INNER JOIN Familia f2 ON ff.IdFamiliaHija = f2.IdFamilia
INNER JOIN FamiliaPatente fp ON f2.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'ROL_Administrador'
  AND p.FormName LIKE 'reporte%'
ORDER BY p.FormName;

PRINT '';
PRINT '✅ Script completado exitosamente';
PRINT '';

GO
