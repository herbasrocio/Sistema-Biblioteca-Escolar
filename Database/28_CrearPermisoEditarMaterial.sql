-- =====================================================
-- Script: Crear Permiso para Editar Material
-- Descripción: Agrega la patente EditarMaterial al submenú Catálogo
--              y la asigna a los roles con permisos de gestión
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Creando permiso EditarMaterial...'
PRINT ''

-- =====================================================
-- PASO 1: Crear la nueva patente para Editar Material
-- =====================================================
PRINT 'Creando patente EditarMaterial...'

DECLARE @IdPatEditarMaterial UNIQUEIDENTIFIER = NEWID()

-- Insertar la nueva patente
INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion) VALUES
(@IdPatEditarMaterial, 'menu', 'EditarMaterial', 3, 'Acceso a edición de materiales del catálogo');

PRINT '  ✓ Patente creada: EditarMaterial'

-- =====================================================
-- PASO 2: Obtener IDs de los roles
-- =====================================================
PRINT 'Obteniendo roles...'

DECLARE @IdRolAdmin UNIQUEIDENTIFIER
DECLARE @IdRolBibliotecario UNIQUEIDENTIFIER

SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador'
SELECT @IdRolBibliotecario = IdFamilia FROM Familia WHERE Nombre = 'ROL_Bibliotecario'

IF @IdRolAdmin IS NULL OR @IdRolBibliotecario IS NULL
BEGIN
    PRINT '  ✗ ERROR: No se encontraron todos los roles necesarios.'
    RAISERROR('Roles no encontrados', 16, 1)
    RETURN
END

PRINT '  ✓ Roles encontrados: Administrador, Bibliotecario'

-- =====================================================
-- PASO 3: Asignar permisos a los roles
-- =====================================================
PRINT 'Asignando permisos de edición...'

-- Administrador: acceso a edición de material
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolAdmin, @IdPatEditarMaterial);

PRINT '  ✓ ROL_Administrador: Editar Material'

-- Bibliotecario: acceso a edición de material
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolBibliotecario, @IdPatEditarMaterial);

PRINT '  ✓ ROL_Bibliotecario: Editar Material'

PRINT ''
PRINT '=== Permiso EditarMaterial configurado exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • Patente creada: EditarMaterial'
PRINT '  • ROL_Administrador: Acceso a edición'
PRINT '  • ROL_Bibliotecario: Acceso a edición'
PRINT '  • ROL_Docente: Sin acceso (solo consulta)'
PRINT ''
PRINT 'SIGUIENTE PASO:'
PRINT '  - Actualizar menu.cs para agregar constante PATENTE_EDITAR_MATERIAL'
PRINT '  - Agregar la opción al menú de Catálogo si es necesario'
PRINT '  - Actualizar archivos de traducción (idioma.es-AR e idioma.en-GB)'
PRINT ''
GO
