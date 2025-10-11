-- =====================================================
-- Script: Corregir Acentos en Descripciones de Patentes
-- Descripción: Actualiza las descripciones con acentos correctos
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT ''
PRINT '=========================================='
PRINT '  CORRECCIÓN DE ACENTOS EN PATENTES'
PRINT '=========================================='
PRINT ''

-- Actualizar descripciones con acentos correctos
UPDATE Patente SET Descripcion = 'Acceso al módulo de Gestión de Usuarios'
WHERE MenuItemName = 'GestionUsuarios' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso al módulo de Gestión de Permisos y Roles'
WHERE MenuItemName = 'GestionPermisos' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso al módulo de Gestión de Catálogo de Materiales'
WHERE MenuItemName = 'GestionCatalogo' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso al módulo de Gestión de Alumnos'
WHERE MenuItemName = 'GestionAlumnos' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso al módulo de Gestión de Préstamos'
WHERE MenuItemName = 'GestionPrestamos' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso al módulo de Gestión de Devoluciones'
WHERE MenuItemName = 'GestionDevoluciones' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso al módulo de Consulta de Reportes'
WHERE MenuItemName = 'ConsultarReportes' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso a consulta de materiales del catálogo'
WHERE MenuItemName = 'ConsultarMaterial' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Acceso a registro y modificación de materiales'
WHERE MenuItemName = 'RegistrarMaterial' AND FormName = 'menu';

UPDATE Patente SET Descripcion = 'Configurar parámetros del sistema'
WHERE MenuItemName = 'Configuración Sistema' AND FormName = 'frmConfiguracion';

PRINT '✓ Descripciones actualizadas correctamente'
PRINT ''

-- Mostrar patentes actualizadas
PRINT 'PATENTES ACTUALIZADAS:'
PRINT ''

SELECT
    MenuItemName AS [Permiso],
    Descripcion AS [Descripción]
FROM Patente
WHERE FormName = 'menu'
ORDER BY Orden

PRINT ''
PRINT '=========================================='
PRINT '  ✓ CORRECCIÓN COMPLETADA'
PRINT '=========================================='
PRINT ''

GO
