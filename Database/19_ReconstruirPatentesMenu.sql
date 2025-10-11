-- =====================================================
-- Script: Reconstruir Patentes del Menú
-- Descripción: Elimina y recrea las patentes del menú
--              con descripciones correctas en Unicode
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT ''
PRINT '=========================================='
PRINT '  RECONSTRUCCIÓN DE PATENTES DEL MENÚ'
PRINT '=========================================='
PRINT ''

-- Primero, eliminar las relaciones en FamiliaPatente para estas patentes
PRINT 'Eliminando relaciones FamiliaPatente...'
DELETE FROM FamiliaPatente
WHERE IdPatente IN (
    SELECT IdPatente FROM Patente WHERE FormName = 'menu'
)
PRINT '  OK Relaciones eliminadas'

-- Eliminar las patentes del menú
PRINT 'Eliminando patentes antiguas del menú...'
DELETE FROM Patente WHERE FormName = 'menu'
PRINT '  OK Patentes eliminadas'

PRINT ''
PRINT 'Recreando patentes con descripciones correctas...'
PRINT ''

-- Recrear las patentes con NVARCHAR Unicode
INSERT INTO Patente (IdPatente, MenuItemName, FormName, Orden, Descripcion)
VALUES
    (NEWID(), 'GestionUsuarios', 'menu', 1, N'Acceso al módulo de Gestión de Usuarios'),
    (NEWID(), 'ConsultarMaterial', 'menu', 2, N'Acceso a consulta de materiales del catálogo'),
    (NEWID(), 'RegistrarMaterial', 'menu', 3, N'Acceso a registro y modificación de materiales'),
    (NEWID(), 'GestionPermisos', 'menu', 4, N'Acceso al módulo de Gestión de Permisos y Roles'),
    (NEWID(), 'GestionCatalogo', 'menu', 5, N'Acceso al módulo de Gestión de Catálogo de Materiales'),
    (NEWID(), 'GestionAlumnos', 'menu', 6, N'Acceso al módulo de Gestión de Alumnos'),
    (NEWID(), 'GestionPrestamos', 'menu', 7, N'Acceso al módulo de Gestión de Préstamos'),
    (NEWID(), 'GestionDevoluciones', 'menu', 8, N'Acceso al módulo de Gestión de Devoluciones'),
    (NEWID(), 'ConsultarReportes', 'menu', 9, N'Acceso al módulo de Consulta de Reportes'),
    (NEWID(), 'PromocionAlumnos', 'menu', 10, N'Acceso al módulo de Promoción de Alumnos')

PRINT '  OK 10 patentes creadas'

PRINT ''
PRINT 'Asignando patentes al rol Administrador...'

-- Obtener el ID del rol Administrador
DECLARE @IdAdministrador UNIQUEIDENTIFIER
SELECT @IdAdministrador = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador'

-- Asignar todas las patentes del menú al Administrador
INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
SELECT @IdAdministrador, IdPatente
FROM Patente
WHERE FormName = 'menu'

PRINT '  OK Permisos asignados al Administrador'

PRINT ''
PRINT 'Asignando patentes al rol Bibliotecario...'

-- Obtener el ID del rol Bibliotecario
DECLARE @IdBibliotecario UNIQUEIDENTIFIER
SELECT @IdBibliotecario = IdFamilia FROM Familia WHERE Nombre = 'ROL_Bibliotecario'

-- Asignar patentes específicas al Bibliotecario
INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
SELECT @IdBibliotecario, IdPatente
FROM Patente
WHERE FormName = 'menu'
AND MenuItemName IN (
    'ConsultarMaterial',
    'RegistrarMaterial',
    'GestionCatalogo',
    'GestionAlumnos',
    'GestionPrestamos',
    'GestionDevoluciones',
    'ConsultarReportes'
)

PRINT '  OK Permisos asignados al Bibliotecario'

PRINT ''
PRINT '=========================================='
PRINT '  VERIFICACIÓN DE PATENTES CREADAS'
PRINT '=========================================='
PRINT ''

-- Mostrar las patentes creadas
SELECT
    MenuItemName AS [Permiso],
    Descripcion AS [Descripción],
    Orden,
    CASE
        WHEN DATALENGTH(Descripcion) > LEN(Descripcion) THEN 'Unicode ✓'
        ELSE 'ASCII ✗'
    END AS Codificacion
FROM Patente
WHERE FormName = 'menu'
ORDER BY Orden

PRINT ''
PRINT '=========================================='
PRINT '  OK RECONSTRUCCIÓN COMPLETADA'
PRINT '=========================================='
PRINT ''
PRINT 'IMPORTANTE: Reiniciar la aplicación para ver los cambios'
PRINT ''

GO
