-- =============================================
-- Script: Agregar patente de Reporte Préstamos Activos
-- Descripción: Crea la patente faltante para el reporte de préstamos activos
-- Fecha: 2025-11-03
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT 'Agregando patente de Reporte Préstamos Activos...'
PRINT ''

-- Crear patente si no existe
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'reportePrestamosActivos')
BEGIN
    INSERT INTO Patente (FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        'reportePrestamosActivos',
        'Préstamos Activos',
        0,
        'Permite ver el reporte de préstamos activos con información detallada de alumnos y materiales'
    );

    PRINT '✓ Patente "reportePrestamosActivos" creada exitosamente';
END
ELSE
BEGIN
    PRINT '✓ La patente "reportePrestamosActivos" ya existe';
END
GO

-- Asignar permiso a la familia ROL_Administrador
DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER;
DECLARE @IdPatente UNIQUEIDENTIFIER;

SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdPatente = IdPatente FROM Patente WHERE FormName = 'reportePrestamosActivos';

IF @IdFamiliaAdmin IS NOT NULL AND @IdPatente IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE IdFamilia = @IdFamiliaAdmin AND IdPatente = @IdPatente)
    BEGIN
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdFamiliaAdmin, @IdPatente);

        PRINT '✓ Permiso "reportePrestamosActivos" asignado a familia ROL_Administrador';
    END
    ELSE
    BEGIN
        PRINT '✓ El permiso "reportePrestamosActivos" ya estaba asignado a familia ROL_Administrador';
    END
END
ELSE
BEGIN
    PRINT '✗ No se pudo asignar el permiso - Familia ROL_Administrador o Patente no encontrada';
END
GO

-- Verificar patentes de reportes
PRINT ''
PRINT 'Patentes de reportes actuales:'
SELECT FormName, MenuItemName, Orden, Descripcion
FROM Patente
WHERE FormName LIKE '%reporte%' OR FormName LIKE '%Reporte%'
ORDER BY Orden, MenuItemName;
GO

PRINT ''
PRINT '=== Script completado exitosamente ==='
PRINT ''
GO
