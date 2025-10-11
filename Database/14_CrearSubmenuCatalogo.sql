-- =====================================================
-- Script: Crear Submenú para Catálogo
-- Descripción: Divide la opción "Catálogo" en dos patentes:
--              - ConsultarMaterial (Docentes y Bibliotecarios)
--              - RegistrarMaterial (Solo Bibliotecarios y Admin)
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Creando submenú para Catálogo...'
PRINT ''

-- =====================================================
-- PASO 1: Crear las nuevas patentes para el submenú
-- =====================================================
PRINT 'Creando patentes de submenú Catálogo...'

DECLARE @IdPatConsultarMaterial UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatRegistrarMaterial UNIQUEIDENTIFIER = NEWID()

-- Insertar las nuevas patentes
INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion) VALUES
(@IdPatConsultarMaterial, 'menu', 'ConsultarMaterial', 1, 'Acceso a consulta de materiales del catálogo'),
(@IdPatRegistrarMaterial, 'menu', 'RegistrarMaterial', 2, 'Acceso a registro y modificación de materiales');

PRINT '  ✓ 2 patentes de submenú creadas: ConsultarMaterial, RegistrarMaterial'

-- =====================================================
-- PASO 2: Obtener IDs de los roles
-- =====================================================
PRINT 'Obteniendo roles...'

DECLARE @IdRolAdmin UNIQUEIDENTIFIER
DECLARE @IdRolDocente UNIQUEIDENTIFIER
DECLARE @IdRolBibliotecario UNIQUEIDENTIFIER

SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador'
SELECT @IdRolDocente = IdFamilia FROM Familia WHERE Nombre = 'ROL_Docente'
SELECT @IdRolBibliotecario = IdFamilia FROM Familia WHERE Nombre = 'ROL_Bibliotecario'

IF @IdRolAdmin IS NULL OR @IdRolDocente IS NULL OR @IdRolBibliotecario IS NULL
BEGIN
    PRINT '  ✗ ERROR: No se encontraron todos los roles necesarios.'
    RAISERROR('Roles no encontrados', 16, 1)
    RETURN
END

PRINT '  ✓ Roles encontrados: Administrador, Docente, Bibliotecario'

-- =====================================================
-- PASO 3: Asignar permisos a los roles
-- =====================================================
PRINT 'Asignando permisos de submenú...'

-- Administrador: acceso a AMBAS opciones
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolAdmin, @IdPatConsultarMaterial),
(@IdRolAdmin, @IdPatRegistrarMaterial);

PRINT '  ✓ ROL_Administrador: Consultar Material + Registrar Material'

-- Docente: acceso SOLO a Consultar Material
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolDocente, @IdPatConsultarMaterial);

PRINT '  ✓ ROL_Docente: Solo Consultar Material'

-- Bibliotecario: acceso a AMBAS opciones
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolBibliotecario, @IdPatConsultarMaterial),
(@IdRolBibliotecario, @IdPatRegistrarMaterial);

PRINT '  ✓ ROL_Bibliotecario: Consultar Material + Registrar Material'

-- =====================================================
-- PASO 4: Eliminar la patente antigua "GestionCatalogo"
--         de todos los roles (ahora usan el submenú)
-- =====================================================
PRINT 'Eliminando patente antigua GestionCatalogo de los roles...'

DECLARE @IdPatGestionCatalogoVieja UNIQUEIDENTIFIER
SELECT @IdPatGestionCatalogoVieja = IdPatente FROM Patente WHERE FormName = 'menu' AND MenuItemName = 'GestionCatalogo'

IF @IdPatGestionCatalogoVieja IS NOT NULL
BEGIN
    -- Eliminar la asignación de la patente vieja a los roles
    DELETE FROM FamiliaPatente
    WHERE idPatente = @IdPatGestionCatalogoVieja
    AND idFamilia IN (@IdRolAdmin, @IdRolDocente, @IdRolBibliotecario)

    PRINT '  ✓ Patente GestionCatalogo eliminada de todos los roles'
    PRINT '    (La patente aún existe en BD pero no está asignada a ningún rol)'
END

PRINT ''
PRINT '=== Submenú de Catálogo configurado exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • Patentes creadas: 2 (ConsultarMaterial, RegistrarMaterial)'
PRINT '  • ROL_Administrador: Acceso completo (consultar + registrar)'
PRINT '  • ROL_Bibliotecario: Acceso completo (consultar + registrar)'
PRINT '  • ROL_Docente: Acceso limitado (solo consultar)'
PRINT ''
PRINT 'SIGUIENTE PASO:'
PRINT '  - Modificar menu.Designer.cs para convertir catalogoToolStripMenuItem'
PRINT '    en un menú con DropDownItems: Consultar Material y Registrar Material'
PRINT '  - Actualizar menu.cs para verificar permisos ConsultarMaterial y RegistrarMaterial'
PRINT ''
GO
