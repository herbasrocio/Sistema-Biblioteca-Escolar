-- =====================================================
-- Script: Corrección Definitiva de Acentos
-- Descripción: Reescribe todas las descripciones
--              caracter por caracter con Unicode
-- =====================================================

USE SeguridadBiblioteca;
GO

SET NOCOUNT ON;

PRINT ''
PRINT '=========================================='
PRINT '  CORRECCIÓN DEFINITIVA DE ACENTOS'
PRINT '=========================================='
PRINT ''

-- GestionUsuarios
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Usuarios'
WHERE MenuItemName = 'GestionUsuarios' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK GestionUsuarios'

-- GestionPermisos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Permisos y Roles'
WHERE MenuItemName = 'GestionPermisos' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK GestionPermisos'

-- GestionCatalogo
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Catálogo de Materiales'
WHERE MenuItemName = 'GestionCatalogo' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK GestionCatalogo'

-- GestionAlumnos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Alumnos'
WHERE MenuItemName = 'GestionAlumnos' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK GestionAlumnos'

-- GestionPrestamos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Préstamos'
WHERE MenuItemName = 'GestionPrestamos' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK GestionPrestamos'

-- GestionDevoluciones
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Devoluciones'
WHERE MenuItemName = 'GestionDevoluciones' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK GestionDevoluciones'

-- ConsultarReportes
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Consulta de Reportes'
WHERE MenuItemName = 'ConsultarReportes' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK ConsultarReportes'

-- ConsultarMaterial
UPDATE Patente
SET Descripcion = N'Acceso a consulta de materiales del catálogo'
WHERE MenuItemName = 'ConsultarMaterial' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK ConsultarMaterial'

-- RegistrarMaterial
UPDATE Patente
SET Descripcion = N'Acceso a registro y modificación de materiales'
WHERE MenuItemName = 'RegistrarMaterial' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK RegistrarMaterial'

-- PromocionAlumnos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Promoción de Alumnos'
WHERE MenuItemName = 'PromocionAlumnos' AND FormName = 'menu';
IF @@ROWCOUNT > 0 PRINT '  OK PromocionAlumnos'

PRINT ''
PRINT '=========================================='
PRINT '  DESCRIPCIONES ACTUALIZADAS'
PRINT '=========================================='
PRINT ''

-- Mostrar todas las descripciones
SELECT
    Orden,
    MenuItemName AS Permiso,
    Descripcion
FROM Patente
WHERE FormName = 'menu'
ORDER BY Orden

PRINT ''
PRINT '=========================================='
PRINT '  OK CORRECCIÓN COMPLETADA'
PRINT '=========================================='
PRINT ''
PRINT 'Cerrar y volver a abrir la aplicación'
PRINT ''

GO
