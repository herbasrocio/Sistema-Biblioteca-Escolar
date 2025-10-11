-- =====================================================
-- Script: Corregir Acentos en Patentes (Unicode)
-- Descripción: Actualiza descripciones usando NVARCHAR Unicode
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT ''
PRINT '=========================================='
PRINT '  CORRECCION DE ACENTOS EN PATENTES'
PRINT '=========================================='
PRINT ''

-- Usar N'' para strings Unicode
UPDATE Patente SET Descripcion = N'Acceso al módulo de Gestión de Usuarios'
WHERE MenuItemName = 'GestionUsuarios' AND FormName = 'menu';
PRINT '  OK GestionUsuarios'

UPDATE Patente SET Descripcion = N'Acceso al módulo de Gestión de Permisos y Roles'
WHERE MenuItemName = 'GestionPermisos' AND FormName = 'menu';
PRINT '  OK GestionPermisos'

UPDATE Patente SET Descripcion = N'Acceso al módulo de Gestión de Catálogo de Materiales'
WHERE MenuItemName = 'GestionCatalogo' AND FormName = 'menu';
PRINT '  OK GestionCatalogo'

UPDATE Patente SET Descripcion = N'Acceso al módulo de Gestión de Alumnos'
WHERE MenuItemName = 'GestionAlumnos' AND FormName = 'menu';
PRINT '  OK GestionAlumnos'

UPDATE Patente SET Descripcion = N'Acceso al módulo de Gestión de Préstamos'
WHERE MenuItemName = 'GestionPrestamos' AND FormName = 'menu';
PRINT '  OK GestionPrestamos'

UPDATE Patente SET Descripcion = N'Acceso al módulo de Gestión de Devoluciones'
WHERE MenuItemName = 'GestionDevoluciones' AND FormName = 'menu';
PRINT '  OK GestionDevoluciones'

UPDATE Patente SET Descripcion = N'Acceso al módulo de Consulta de Reportes'
WHERE MenuItemName = 'ConsultarReportes' AND FormName = 'menu';
PRINT '  OK ConsultarReportes'

UPDATE Patente SET Descripcion = N'Acceso a consulta de materiales del catálogo'
WHERE MenuItemName = 'ConsultarMaterial' AND FormName = 'menu';
PRINT '  OK ConsultarMaterial'

UPDATE Patente SET Descripcion = N'Acceso a registro y modificación de materiales'
WHERE MenuItemName = 'RegistrarMaterial' AND FormName = 'menu';
PRINT '  OK RegistrarMaterial'

UPDATE Patente SET Descripcion = N'Acceso al módulo de Promoción de Alumnos'
WHERE MenuItemName = 'PromocionAlumnos' AND FormName = 'menu';
PRINT '  OK PromocionAlumnos'

UPDATE Patente SET Descripcion = N'Configurar parámetros del sistema'
WHERE MenuItemName = N'Configuración Sistema' AND FormName = 'frmConfiguracion';
PRINT '  OK Configuracion'

PRINT ''
PRINT '=========================================='
PRINT '  OK CORRECCION COMPLETADA'
PRINT '=========================================='
PRINT ''
PRINT 'Las descripciones ahora muestran correctamente:'
PRINT '  - modulo (con acento en o)'
PRINT '  - Gestion (con acento en o)'
PRINT '  - Catalogo (con acento en a)'
PRINT '  - Prestamos (con acento en e)'
PRINT '  - modificacion (con acento en o)'
PRINT ''

GO
