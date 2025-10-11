-- =====================================================
-- Script: Crear Rol Bibliotecario
-- Descripción: Inserta el rol Bibliotecario con los mismos
--              permisos que el rol Docente
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Creando rol Bibliotecario...'
PRINT ''

-- =====================================================
-- PASO 1: Crear el rol Bibliotecario
-- =====================================================
PRINT 'Creando familia ROL_Bibliotecario...'

DECLARE @IdRolBibliotecario UNIQUEIDENTIFIER = NEWID()

-- Verificar si ya existe el rol
IF EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'ROL_Bibliotecario')
BEGIN
    PRINT '  ⚠ El rol ROL_Bibliotecario ya existe. Eliminando rol anterior...'
    DELETE FROM UsuarioFamilia WHERE idFamilia IN (SELECT IdFamilia FROM Familia WHERE Nombre = 'ROL_Bibliotecario')
    DELETE FROM FamiliaPatente WHERE idFamilia IN (SELECT IdFamilia FROM Familia WHERE Nombre = 'ROL_Bibliotecario')
    DELETE FROM Familia WHERE Nombre = 'ROL_Bibliotecario'
    SET @IdRolBibliotecario = NEWID()
END

INSERT INTO Familia (IdFamilia, Nombre, Descripcion) VALUES
(@IdRolBibliotecario, 'ROL_Bibliotecario', 'Rol de Bibliotecario - Gestión de catálogo, préstamos y consultas');

PRINT '  ✓ Rol ROL_Bibliotecario creado'

-- =====================================================
-- PASO 2: Obtener IDs de las patentes del menú
-- =====================================================
PRINT 'Obteniendo patentes de menú...'

DECLARE @IdPatGestionCatalogo UNIQUEIDENTIFIER
DECLARE @IdPatGestionAlumnos UNIQUEIDENTIFIER
DECLARE @IdPatGestionPrestamos UNIQUEIDENTIFIER
DECLARE @IdPatGestionDevoluciones UNIQUEIDENTIFIER
DECLARE @IdPatConsultarReportes UNIQUEIDENTIFIER

-- Obtener los IDs de las patentes (deben existir desde 10_CrearPatentesMenuGeneral.sql)
SELECT @IdPatGestionCatalogo = IdPatente FROM Patente WHERE FormName = 'menu' AND MenuItemName = 'GestionCatalogo'
SELECT @IdPatGestionAlumnos = IdPatente FROM Patente WHERE FormName = 'menu' AND MenuItemName = 'GestionAlumnos'
SELECT @IdPatGestionPrestamos = IdPatente FROM Patente WHERE FormName = 'menu' AND MenuItemName = 'GestionPrestamos'
SELECT @IdPatGestionDevoluciones = IdPatente FROM Patente WHERE FormName = 'menu' AND MenuItemName = 'GestionDevoluciones'
SELECT @IdPatConsultarReportes = IdPatente FROM Patente WHERE FormName = 'menu' AND MenuItemName = 'ConsultarReportes'

-- Verificar que todas las patentes existen
IF @IdPatGestionCatalogo IS NULL OR @IdPatGestionAlumnos IS NULL OR
   @IdPatGestionPrestamos IS NULL OR @IdPatGestionDevoluciones IS NULL OR
   @IdPatConsultarReportes IS NULL
BEGIN
    PRINT '  ✗ ERROR: No se encontraron todas las patentes necesarias.'
    PRINT '           Ejecute primero 10_CrearPatentesMenuGeneral.sql'
    RAISERROR('Patentes no encontradas', 16, 1)
    RETURN
END

PRINT '  ✓ 5 patentes de menú encontradas'

-- =====================================================
-- PASO 3: Asignar patentes al ROL_Bibliotecario
-- =====================================================
PRINT 'Asignando permisos al ROL_Bibliotecario...'

-- El bibliotecario tiene los mismos permisos que el docente:
-- - Gestión de Catálogo (consulta y administración de libros)
-- - Gestión de Alumnos (consulta de alumnos)
-- - Gestión de Préstamos (realizar préstamos)
-- - Gestión de Devoluciones (registrar devoluciones)
-- - Consultar Reportes (ver reportes de préstamos)
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolBibliotecario, @IdPatGestionCatalogo),
(@IdRolBibliotecario, @IdPatGestionAlumnos),
(@IdRolBibliotecario, @IdPatGestionPrestamos),
(@IdRolBibliotecario, @IdPatGestionDevoluciones),
(@IdRolBibliotecario, @IdPatConsultarReportes);

PRINT '  ✓ ROL_Bibliotecario: 5 permisos asignados (catálogo, alumnos, préstamos, devoluciones, reportes)'
PRINT '  → El bibliotecario NO tiene acceso a: Gestión de Usuarios ni Gestión de Permisos'

PRINT ''
PRINT '=== Rol Bibliotecario creado exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • Rol creado: ROL_Bibliotecario'
PRINT '  • Permisos asignados: 5/7 módulos (igual que ROL_Docente)'
PRINT '  • Acceso a: Catálogo, Alumnos, Préstamos, Devoluciones, Reportes'
PRINT '  • Sin acceso a: Usuarios, Permisos'
PRINT ''
PRINT 'NOTA:'
PRINT '  - El rol Bibliotecario está disponible para asignar a usuarios desde gestionUsuarios'
PRINT '  - Los permisos pueden modificarse desde gestionPermisos si es necesario'
PRINT ''
GO
