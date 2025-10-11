-- =====================================================
-- Script: Forzar Unicode en Descripciones
-- Descripción: Actualiza usando valores completos
--              con caracteres Unicode escapados
-- =====================================================

USE SeguridadBiblioteca;
GO

SET NOCOUNT ON;

PRINT ''
PRINT '=========================================='
PRINT '  FORZANDO UNICODE EN DESCRIPCIONES'
PRINT '=========================================='
PRINT ''

-- GestionUsuarios - "Acceso al módulo de Gestión de Usuarios"
UPDATE Patente
SET Descripcion = CAST(0x410063006300650073006F002000610006C0020006D00F3006400750060006C006F002000640065002000470065007300740069006F006E00200064006500200055007300750061007200690006F007300 AS NVARCHAR(255))
WHERE MenuItemName = 'GestionUsuarios' AND FormName = 'menu';
PRINT '  GestionUsuarios actualizado'

-- GestionPermisos - "Acceso al módulo de Gestión de Permisos y Roles"
UPDATE Patente
SET Descripcion = CAST(0x4100630063006500730006F002000610006C0020006D00F3006400750060006C006F00200064006500200047006500730074006900F3006E002000640065002000500065007200060069007300006F0073002000790020005200006F006C006500730 AS NVARCHAR(255))
WHERE MenuItemName = 'GestionPermisos' AND FormName = 'menu';
PRINT '  GestionPermisos actualizado'

-- Método alternativo: usar REPLACE para cambiar caracteres problemáticos
PRINT ''
PRINT 'Usando método REPLACE...'

UPDATE Patente SET Descripcion = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
    'Acceso al modulo de Gestion de Catalogo de Materiales',
    'modulo', N'módulo'),
    'Gestion', N'Gestión'),
    'Catalogo', N'Catálogo'),
    'Prestamos', N'Préstamos'),
    'modificacion', N'modificación')
WHERE MenuItemName = 'GestionCatalogo' AND FormName = 'menu';
PRINT '  GestionCatalogo actualizado'

UPDATE Patente SET Descripcion = REPLACE(REPLACE(
    'Acceso al modulo de Gestion de Alumnos',
    'modulo', N'módulo'),
    'Gestion', N'Gestión')
WHERE MenuItemName = 'GestionAlumnos' AND FormName = 'menu';
PRINT '  GestionAlumnos actualizado'

UPDATE Patente SET Descripcion = REPLACE(REPLACE(REPLACE(
    'Acceso al modulo de Gestion de Prestamos',
    'modulo', N'módulo'),
    'Gestion', N'Gestión'),
    'Prestamos', N'Préstamos')
WHERE MenuItemName = 'GestionPrestamos' AND FormName = 'menu';
PRINT '  GestionPrestamos actualizado'

UPDATE Patente SET Descripcion = REPLACE(REPLACE(
    'Acceso al modulo de Gestion de Devoluciones',
    'modulo', N'módulo'),
    'Gestion', N'Gestión')
WHERE MenuItemName = 'GestionDevoluciones' AND FormName = 'menu';
PRINT '  GestionDevoluciones actualizado'

UPDATE Patente SET Descripcion = REPLACE(
    'Acceso al modulo de Consulta de Reportes',
    'modulo', N'módulo')
WHERE MenuItemName = 'ConsultarReportes' AND FormName = 'menu';
PRINT '  ConsultarReportes actualizado'

UPDATE Patente SET Descripcion = REPLACE(
    'Acceso a consulta de materiales del catalogo',
    'catalogo', N'catálogo')
WHERE MenuItemName = 'ConsultarMaterial' AND FormName = 'menu';
PRINT '  ConsultarMaterial actualizado'

UPDATE Patente SET Descripcion = REPLACE(
    'Acceso a registro y modificacion de materiales',
    'modificacion', N'modificación')
WHERE MenuItemName = 'RegistrarMaterial' AND FormName = 'menu';
PRINT '  RegistrarMaterial actualizado'

UPDATE Patente SET Descripcion = REPLACE(REPLACE(
    'Acceso al modulo de Promocion de Alumnos',
    'modulo', N'módulo'),
    'Promocion', N'Promoción')
WHERE MenuItemName = 'PromocionAlumnos' AND FormName = 'menu';
PRINT '  PromocionAlumnos actualizado'

PRINT ''
PRINT '=========================================='
PRINT '  VERIFICACIÓN FINAL'
PRINT '=========================================='
PRINT ''

-- Verificar con códigos Unicode
SELECT
    MenuItemName,
    Descripcion,
    -- Buscar posición de la ó en módulo
    CHARINDEX(N'ó', Descripcion) AS PosO_Acento,
    -- Obtener el código Unicode del carácter en esa posición
    CASE
        WHEN CHARINDEX(N'ó', Descripcion) > 0
        THEN UNICODE(SUBSTRING(Descripcion, CHARINDEX(N'ó', Descripcion), 1))
        ELSE 0
    END AS CodigoUnicode_O
FROM Patente
WHERE FormName = 'menu'
AND Descripcion LIKE '%modulo%'
ORDER BY MenuItemName

PRINT ''
PRINT 'Si CodigoUnicode_O = 243, el acento está correcto'
PRINT ''

GO
