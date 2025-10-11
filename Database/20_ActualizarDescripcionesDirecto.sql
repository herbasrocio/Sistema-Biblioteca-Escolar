-- =====================================================
-- Script: Actualizar Descripciones Directamente
-- Descripción: Actualiza las descripciones usando
--              NCHAR para forzar Unicode correcto
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT ''
PRINT '=========================================='
PRINT '  ACTUALIZACIÓN DIRECTA DE DESCRIPCIONES'
PRINT '=========================================='
PRINT ''

-- Usar NCHAR para caracteres específicos con acento
DECLARE @modulo NVARCHAR(10) = NCHAR(109) + NCHAR(243) + NCHAR(100) + NCHAR(117) + NCHAR(108) + NCHAR(111)  -- módulo
DECLARE @gestion NVARCHAR(10) = NCHAR(71) + NCHAR(101) + NCHAR(115) + NCHAR(116) + NCHAR(105) + NCHAR(243) + NCHAR(110)  -- Gestión
DECLARE @catalogo NVARCHAR(10) = NCHAR(67) + NCHAR(97) + NCHAR(116) + NCHAR(225) + NCHAR(108) + NCHAR(111) + NCHAR(103) + NCHAR(111)  -- Catálogo
DECLARE @prestamos NVARCHAR(10) = NCHAR(80) + NCHAR(114) + NCHAR(233) + NCHAR(115) + NCHAR(116) + NCHAR(97) + NCHAR(109) + NCHAR(111) + NCHAR(115)  -- Préstamos
DECLARE @modificacion NVARCHAR(15) = NCHAR(109) + NCHAR(111) + NCHAR(100) + NCHAR(105) + NCHAR(102) + NCHAR(105) + NCHAR(99) + NCHAR(97) + NCHAR(99) + NCHAR(105) + NCHAR(243) + NCHAR(110)  -- modificación
DECLARE @promocion NVARCHAR(15) = NCHAR(80) + NCHAR(114) + NCHAR(111) + NCHAR(109) + NCHAR(111) + NCHAR(99) + NCHAR(105) + NCHAR(243) + NCHAR(110)  -- Promoción

-- Actualizar descripciones
UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de ' + @gestion + N' de Usuarios'
WHERE MenuItemName = 'GestionUsuarios' AND FormName = 'menu';
PRINT '  OK GestionUsuarios actualizado'

UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de ' + @gestion + N' de Permisos y Roles'
WHERE MenuItemName = 'GestionPermisos' AND FormName = 'menu';
PRINT '  OK GestionPermisos actualizado'

UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de ' + @gestion + N' de ' + @catalogo + N' de Materiales'
WHERE MenuItemName = 'GestionCatalogo' AND FormName = 'menu';
PRINT '  OK GestionCatalogo actualizado'

UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de ' + @gestion + N' de Alumnos'
WHERE MenuItemName = 'GestionAlumnos' AND FormName = 'menu';
PRINT '  OK GestionAlumnos actualizado'

UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de ' + @gestion + N' de ' + @prestamos
WHERE MenuItemName = 'GestionPrestamos' AND FormName = 'menu';
PRINT '  OK GestionPrestamos actualizado'

UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de ' + @gestion + N' de Devoluciones'
WHERE MenuItemName = 'GestionDevoluciones' AND FormName = 'menu';
PRINT '  OK GestionDevoluciones actualizado'

UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de Consulta de Reportes'
WHERE MenuItemName = 'ConsultarReportes' AND FormName = 'menu';
PRINT '  OK ConsultarReportes actualizado'

UPDATE Patente SET Descripcion = N'Acceso a consulta de materiales del ' + @catalogo
WHERE MenuItemName = 'ConsultarMaterial' AND FormName = 'menu';
PRINT '  OK ConsultarMaterial actualizado'

UPDATE Patente SET Descripcion = N'Acceso a registro y ' + @modificacion + N' de materiales'
WHERE MenuItemName = 'RegistrarMaterial' AND FormName = 'menu';
PRINT '  OK RegistrarMaterial actualizado'

UPDATE Patente SET Descripcion = N'Acceso al ' + @modulo + N' de ' + @promocion + N' de Alumnos'
WHERE MenuItemName = 'PromocionAlumnos' AND FormName = 'menu';
PRINT '  OK PromocionAlumnos actualizado'

PRINT ''
PRINT '=========================================='
PRINT '  VERIFICACIÓN'
PRINT '=========================================='
PRINT ''

-- Mostrar resultados
SELECT
    MenuItemName,
    Descripcion,
    LEN(Descripcion) AS Longitud,
    UNICODE(SUBSTRING(Descripcion, 13, 1)) AS CodigoChar13
FROM Patente
WHERE FormName = 'menu'
AND MenuItemName IN ('GestionUsuarios', 'GestionCatalogo', 'GestionPrestamos')
ORDER BY MenuItemName

PRINT ''
PRINT 'Códigos Unicode esperados:'
PRINT '  ó = 243'
PRINT '  á = 225'
PRINT '  é = 233'
PRINT ''
PRINT 'Si CodigoChar13 muestra 243, los acentos están correctos'
PRINT ''

GO
