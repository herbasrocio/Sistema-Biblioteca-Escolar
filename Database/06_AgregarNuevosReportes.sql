-- ============================================================================
-- Script: Agregar Nuevos Reportes al Sistema
-- Descripción: Agrega permisos y configuración para dos nuevos reportes:
--              1. Reporte de Materiales Más Prestados
--              2. Reporte de Uso por Grado/División
-- Fecha: 2025-10-26
-- ============================================================================

USE SeguridadBiblioteca;
GO

PRINT 'Iniciando configuración de nuevos reportes...';
GO

-- ============================================================================
-- PASO 1: Crear patentes (permisos individuales) para los nuevos reportes
-- ============================================================================

PRINT 'Paso 1: Creando patentes...';
GO

-- Verificar si ya existe la patente de Materiales Más Prestados
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'reporteMaterialesMasPrestados')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (
        NEWID(),
        'reporteMaterialesMasPrestados',
        'Materiales Más Prestados',
        'Permite ver el reporte de materiales más prestados con estadísticas'
    );
    PRINT '  ✓ Patente "reporteMaterialesMasPrestados" creada';
END
ELSE
BEGIN
    PRINT '  ℹ Patente "reporteMaterialesMasPrestados" ya existe';
END
GO

-- Verificar si ya existe la patente de Uso por Grado
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'reporteUsoPorGrado')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (
        NEWID(),
        'reporteUsoPorGrado',
        'Uso por Grado/División',
        'Permite ver el reporte de uso de biblioteca por grado y división'
    );
    PRINT '  ✓ Patente "reporteUsoPorGrado" creada';
END
ELSE
BEGIN
    PRINT '  ℹ Patente "reporteUsoPorGrado" ya existe';
END
GO

-- ============================================================================
-- PASO 2: Asignar patentes al rol de Administrador
-- ============================================================================

PRINT 'Paso 2: Asignando permisos al rol Administrador...';
GO

-- Asignar Materiales Más Prestados a Administrador
IF NOT EXISTS (
    SELECT 1
    FROM FamiliaPatente fp
    INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
    INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
    WHERE f.Nombre = 'ROL_Administrador'
      AND p.FormName = 'reporteMaterialesMasPrestados'
)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    SELECT f.IdFamilia, p.IdPatente
    FROM Familia f
    CROSS JOIN Patente p
    WHERE f.Nombre = 'ROL_Administrador'
      AND p.FormName = 'reporteMaterialesMasPrestados';

    PRINT '  ✓ Permiso "reporteMaterialesMasPrestados" asignado a Administrador';
END
ELSE
BEGIN
    PRINT '  ℹ Permiso "reporteMaterialesMasPrestados" ya asignado a Administrador';
END
GO

-- Asignar Uso por Grado a Administrador
IF NOT EXISTS (
    SELECT 1
    FROM FamiliaPatente fp
    INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
    INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
    WHERE f.Nombre = 'ROL_Administrador'
      AND p.FormName = 'reporteUsoPorGrado'
)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    SELECT f.IdFamilia, p.IdPatente
    FROM Familia f
    CROSS JOIN Patente p
    WHERE f.Nombre = 'ROL_Administrador'
      AND p.FormName = 'reporteUsoPorGrado';

    PRINT '  ✓ Permiso "reporteUsoPorGrado" asignado a Administrador';
END
ELSE
BEGIN
    PRINT '  ℹ Permiso "reporteUsoPorGrado" ya asignado a Administrador';
END
GO

-- ============================================================================
-- PASO 3: Agregar traducciones en español (es-AR)
-- ============================================================================

PRINT 'Paso 3: Agregando traducciones en español...';
GO

-- Traducciones para Materiales Más Prestados
IF NOT EXISTS (SELECT 1 FROM Idioma WHERE Nombre = 'es-AR' AND Clave = 'reporte_materiales_mas_prestados')
BEGIN
    INSERT INTO Idioma (IdIdioma, Nombre, Clave, Traduccion)
    VALUES (NEWID(), 'es-AR', 'reporte_materiales_mas_prestados', 'Reporte de Materiales Más Prestados');
    PRINT '  ✓ Traducción agregada: reporte_materiales_mas_prestados (es-AR)';
END
GO

-- Traducciones para Uso por Grado
IF NOT EXISTS (SELECT 1 FROM Idioma WHERE Nombre = 'es-AR' AND Clave = 'reporte_uso_por_grado')
BEGIN
    INSERT INTO Idioma (IdIdioma, Nombre, Clave, Traduccion)
    VALUES (NEWID(), 'es-AR', 'reporte_uso_por_grado', 'Reporte de Uso por Grado/División');
    PRINT '  ✓ Traducción agregada: reporte_uso_por_grado (es-AR)';
END
GO

-- Otras traducciones útiles
DECLARE @traducciones TABLE (Clave VARCHAR(100), Traduccion NVARCHAR(200));

INSERT INTO @traducciones (Clave, Traduccion) VALUES
('top', 'Top'),
('anio_lectivo', 'Año Lectivo'),
('materiales', 'Materiales'),
('grado_division', 'Grado/División'),
('total_ejemplares', 'Total Ejemplares'),
('disponibles', 'Disponibles'),
('ultimo_mes', 'Último Mes'),
('ultimo_anio', 'Último Año'),
('cantidad_alumnos', 'Cantidad Alumnos'),
('prestamos_activos', 'Préstamos Activos'),
('prestamos_devueltos', 'Préstamos Devueltos'),
('prestamos_vencidos', 'Préstamos Vencidos'),
('promedio_por_alumno', 'Promedio por Alumno'),
('genero_mas_prestado', 'Género Más Prestado'),
('material_mas_prestado', 'Material Más Prestado'),
('dias_promedio_retencion', 'Días Promedio Retención'),
('promedio_general', 'Promedio General');

INSERT INTO Idioma (IdIdioma, Nombre, Clave, Traduccion)
SELECT NEWID(), 'es-AR', t.Clave, t.Traduccion
FROM @traducciones t
WHERE NOT EXISTS (
    SELECT 1 FROM Idioma i
    WHERE i.Nombre = 'es-AR' AND i.Clave = t.Clave
);

PRINT '  ✓ Traducciones adicionales agregadas (es-AR)';
GO

-- ============================================================================
-- PASO 4: Agregar traducciones en inglés (en-GB)
-- ============================================================================

PRINT 'Paso 4: Agregando traducciones en inglés...';
GO

-- Traducciones principales en inglés
IF NOT EXISTS (SELECT 1 FROM Idioma WHERE Nombre = 'en-GB' AND Clave = 'reporte_materiales_mas_prestados')
BEGIN
    INSERT INTO Idioma (IdIdioma, Nombre, Clave, Traduccion)
    VALUES (NEWID(), 'en-GB', 'reporte_materiales_mas_prestados', 'Most Borrowed Materials Report');
    PRINT '  ✓ Translation added: reporte_materiales_mas_prestados (en-GB)';
END
GO

IF NOT EXISTS (SELECT 1 FROM Idioma WHERE Nombre = 'en-GB' AND Clave = 'reporte_uso_por_grado')
BEGIN
    INSERT INTO Idioma (IdIdioma, Nombre, Clave, Traduccion)
    VALUES (NEWID(), 'en-GB', 'reporte_uso_por_grado', 'Library Usage by Grade Report');
    PRINT '  ✓ Translation added: reporte_uso_por_grado (en-GB)';
END
GO

-- Otras traducciones en inglés
DECLARE @translationsEN TABLE (Clave VARCHAR(100), Traduccion NVARCHAR(200));

INSERT INTO @translationsEN (Clave, Traduccion) VALUES
('top', 'Top'),
('anio_lectivo', 'School Year'),
('materiales', 'Materials'),
('grado_division', 'Grade/Division'),
('total_ejemplares', 'Total Copies'),
('disponibles', 'Available'),
('ultimo_mes', 'Last Month'),
('ultimo_anio', 'Last Year'),
('cantidad_alumnos', 'Number of Students'),
('prestamos_activos', 'Active Loans'),
('prestamos_devueltos', 'Returned Loans'),
('prestamos_vencidos', 'Overdue Loans'),
('promedio_por_alumno', 'Average per Student'),
('genero_mas_prestado', 'Most Borrowed Genre'),
('material_mas_prestado', 'Most Borrowed Material'),
('dias_promedio_retencion', 'Average Retention Days'),
('promedio_general', 'Overall Average');

INSERT INTO Idioma (IdIdioma, Nombre, Clave, Traduccion)
SELECT NEWID(), 'en-GB', t.Clave, t.Traduccion
FROM @translationsEN t
WHERE NOT EXISTS (
    SELECT 1 FROM Idioma i
    WHERE i.Nombre = 'en-GB' AND i.Clave = t.Clave
);

PRINT '  ✓ Additional translations added (en-GB)';
GO

-- ============================================================================
-- PASO 5: Verificación final
-- ============================================================================

PRINT '';
PRINT '========================================';
PRINT 'VERIFICACIÓN FINAL';
PRINT '========================================';
GO

-- Verificar patentes creadas
PRINT 'Patentes de reportes:';
SELECT
    FormName,
    MenuItemName,
    Descripcion
FROM Patente
WHERE FormName IN ('reporteMaterialesMasPrestados', 'reporteUsoPorGrado');
GO

-- Verificar asignación a roles
PRINT '';
PRINT 'Permisos asignados a Administrador:';
SELECT
    f.Nombre AS Rol,
    p.FormName AS Permiso,
    p.MenuItemName AS NombreMenu
FROM FamiliaPatente fp
INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'ROL_Administrador'
  AND p.FormName IN ('reporteMaterialesMasPrestados', 'reporteUsoPorGrado');
GO

-- Contar traducciones agregadas
PRINT '';
PRINT 'Traducciones agregadas:';
SELECT
    Nombre AS Idioma,
    COUNT(*) AS CantidadTraducciones
FROM Idioma
WHERE Clave IN (
    'reporte_materiales_mas_prestados',
    'reporte_uso_por_grado',
    'top', 'anio_lectivo', 'materiales', 'grado_division',
    'total_ejemplares', 'disponibles', 'ultimo_mes', 'ultimo_anio',
    'cantidad_alumnos', 'prestamos_activos', 'prestamos_devueltos',
    'prestamos_vencidos', 'promedio_por_alumno', 'genero_mas_prestado',
    'material_mas_prestado', 'dias_promedio_retencion', 'promedio_general'
)
GROUP BY Nombre;
GO

PRINT '';
PRINT '========================================';
PRINT '✓ CONFIGURACIÓN COMPLETADA EXITOSAMENTE';
PRINT '========================================';
PRINT '';
PRINT 'Los nuevos reportes están listos para usar:';
PRINT '  • Reporte de Materiales Más Prestados';
PRINT '  • Reporte de Uso por Grado/División';
PRINT '';
PRINT 'NOTA: Para acceder a estos reportes, los usuarios deben:';
PRINT '  1. Tener el rol de Administrador (u otro rol con permisos asignados)';
PRINT '  2. Los formularios deben estar integrados en el menú principal';
PRINT '';
GO
