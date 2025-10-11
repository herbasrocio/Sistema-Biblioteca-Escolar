-- =====================================================
-- Script: Crear Permiso para Promoción de Alumnos
-- Descripción: Agrega la patente PromocionAlumnos y
--              la asigna al rol de Administrador
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Creando permiso de Promoción de Alumnos...'
PRINT ''

-- =====================================================
-- Declarar variables
-- =====================================================
DECLARE @IdPatentePromocion UNIQUEIDENTIFIER = NEWID()
DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER
DECLARE @NombrePatente NVARCHAR(100) = 'PromocionAlumnos'
DECLARE @DescripcionPatente NVARCHAR(200) = 'Permite gestionar la promoción de alumnos entre años lectivos'
DECLARE @TipoPermiso INT = 5  -- Tipo de permiso (ajustar según tu sistema)

-- =====================================================
-- Verificar si la patente ya existe
-- =====================================================
IF EXISTS (SELECT 1 FROM Patente WHERE Nombre = @NombrePatente)
BEGIN
    PRINT '  ⚠ La patente PromocionAlumnos ya existe'
    PRINT '  Obteniendo ID existente...'
    SELECT @IdPatentePromocion = IdPatente FROM Patente WHERE Nombre = @NombrePatente
END
ELSE
BEGIN
    -- Crear la patente
    INSERT INTO Patente (IdPatente, Nombre, TipoPermiso)
    VALUES (@IdPatentePromocion, @NombrePatente, @TipoPermiso)

    PRINT '  ✓ Patente PromocionAlumnos creada'
END

-- =====================================================
-- Obtener ID de la familia ROL_Administrador
-- =====================================================
SELECT @IdFamiliaAdmin = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Administrador'

IF @IdFamiliaAdmin IS NULL
BEGIN
    PRINT '  ❌ ERROR: No se encontró el rol ROL_Administrador'
    PRINT '  La patente fue creada pero no asignada a ningún rol'
    RETURN
END

PRINT '  ✓ Rol Administrador encontrado'

-- =====================================================
-- Asignar patente al rol de Administrador
-- =====================================================
IF EXISTS (
    SELECT 1 FROM FamiliaPatente
    WHERE IdFamilia = @IdFamiliaAdmin
    AND IdPatente = @IdPatentePromocion
)
BEGIN
    PRINT '  ⚠ La patente ya está asignada al rol Administrador'
END
ELSE
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    VALUES (@IdFamiliaAdmin, @IdPatentePromocion)

    PRINT '  ✓ Patente asignada al rol Administrador'
END

-- =====================================================
-- Verificar la creación
-- =====================================================
PRINT ''
PRINT '=== Verificación ==='

SELECT
    p.Nombre AS Patente,
    p.TipoPermiso,
    f.Nombre AS Rol
FROM Patente p
INNER JOIN FamiliaPatente fp ON p.IdPatente = fp.IdPatente
INNER JOIN Familia f ON fp.IdFamilia = f.IdFamilia
WHERE p.Nombre = @NombrePatente

PRINT ''
PRINT '✓✓✓ Permiso de Promoción de Alumnos configurado exitosamente ✓✓✓'
PRINT ''
PRINT 'NOTAS:'
PRINT '  • Los usuarios con rol Administrador podrán acceder a la ventana de promoción'
PRINT '  • Si el menú es dinámico, la opción aparecerá automáticamente'
PRINT '  • Si es estático, debe agregarse manualmente al código del menú'
PRINT ''
GO
