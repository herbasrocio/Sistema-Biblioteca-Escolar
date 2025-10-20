/*
Script EXITOSO para corregir acentos en tabla Patente
Fecha: 2025-10-13
IMPORTANTE: Ejecutar copiando y pegando en SSMS (no abrir como archivo)
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'CORRIGIENDO ACENTOS EN TABLA PATENTE'
PRINT '================================================'
PRINT ''

-- ConsultarMaterial
UPDATE Patente
SET Descripcion = N'Acceso a consulta de materiales del catálogo'
WHERE MenuItemName = 'ConsultarMaterial';
PRINT 'ConsultarMaterial actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- ConsultarReportes
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Consulta de Reportes'
WHERE MenuItemName = 'ConsultarReportes';
PRINT 'ConsultarReportes actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- EditarMaterial
UPDATE Patente
SET Descripcion = N'Acceso a edición de materiales del catálogo'
WHERE MenuItemName = 'EditarMaterial';
PRINT 'EditarMaterial actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- GestionAlumnos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Alumnos'
WHERE MenuItemName = 'GestionAlumnos';
PRINT 'GestionAlumnos actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- GestionarEjemplares
UPDATE Patente
SET Descripcion = N'Acceso a gestión de ejemplares (copias físicas) de materiales'
WHERE MenuItemName = 'GestionarEjemplares';
PRINT 'GestionarEjemplares actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- GestionCatalogo
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Catálogo de Materiales'
WHERE MenuItemName = 'GestionCatalogo';
PRINT 'GestionCatalogo actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- GestionDevoluciones
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Devoluciones'
WHERE MenuItemName = 'GestionDevoluciones';
PRINT 'GestionDevoluciones actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- GestionPermisos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Permisos y Roles'
WHERE MenuItemName = 'GestionPermisos';
PRINT 'GestionPermisos actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- GestionPrestamos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Préstamos'
WHERE MenuItemName = 'GestionPrestamos';
PRINT 'GestionPrestamos actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- GestionUsuarios
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Gestión de Usuarios'
WHERE MenuItemName = 'GestionUsuarios';
PRINT 'GestionUsuarios actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- PromocionAlumnos
UPDATE Patente
SET Descripcion = N'Acceso al módulo de Promoción de Alumnos'
WHERE MenuItemName = 'PromocionAlumnos';
PRINT 'PromocionAlumnos actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- RegistrarMaterial
UPDATE Patente
SET Descripcion = N'Acceso a registro y modificación de materiales'
WHERE MenuItemName = 'RegistrarMaterial';
PRINT 'RegistrarMaterial actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- Configuración Sistema
UPDATE Patente
SET Descripcion = N'Configurar parámetros del sistema'
WHERE MenuItemName = 'Configuración Sistema';
PRINT 'Configuración Sistema actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

PRINT ''
PRINT '================================================'
PRINT 'VERIFICACIÓN FINAL'
PRINT '================================================'
PRINT ''

-- Verificar resultados
SELECT
    ROW_NUMBER() OVER (ORDER BY MenuItemName) AS [#],
    MenuItemName AS [Nombre del Permiso],
    Descripcion AS [Descripción],
    FormName AS [Formulario]
FROM Patente
ORDER BY MenuItemName;

PRINT ''
PRINT '================================================'
PRINT 'CORRECCIÓN COMPLETADA EXITOSAMENTE'
PRINT '================================================'
PRINT ''
PRINT 'Registros actualizados: 13'
PRINT 'Los acentos ahora se ven correctamente.'
PRINT ''

GO
