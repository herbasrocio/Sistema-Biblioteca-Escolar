-- =====================================================
-- Script: Verificar y Crear Usuario Admin
-- Descripción: Verifica si existe la BD y el usuario admin, y lo crea si no existe
-- =====================================================

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║  VERIFICAR Y CREAR USUARIO ADMIN                  ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- =====================================================
-- PASO 1: Verificar si existe la base de datos
-- =====================================================
PRINT '▶ PASO 1: Verificando base de datos SeguridadBiblioteca...'

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SeguridadBiblioteca')
BEGIN
    PRINT '  ✗ ERROR: La base de datos SeguridadBiblioteca NO existe'
    PRINT ''
    PRINT 'SOLUCIÓN: Ejecutar el script completo:'
    PRINT '  Database\00_EJECUTAR_TODO.sql'
    PRINT ''
    RETURN
END

PRINT '  ✓ Base de datos SeguridadBiblioteca existe'

USE SeguridadBiblioteca;
GO

PRINT ''
PRINT '▶ PASO 2: Verificando tablas...'

-- Verificar tabla Usuario
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuario')
BEGIN
    PRINT '  ✗ ERROR: La tabla Usuario NO existe'
    PRINT ''
    PRINT 'SOLUCIÓN: Ejecutar el script completo:'
    PRINT '  Database\00_EJECUTAR_TODO.sql'
    PRINT ''
    RETURN
END

PRINT '  ✓ Tabla Usuario existe'

-- Verificar tabla Familia
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Familia')
BEGIN
    PRINT '  ✗ ERROR: La tabla Familia NO existe'
    PRINT ''
    PRINT 'SOLUCIÓN: Ejecutar el script completo:'
    PRINT '  Database\00_EJECUTAR_TODO.sql'
    PRINT ''
    RETURN
END

PRINT '  ✓ Tabla Familia existe'

PRINT ''
PRINT '▶ PASO 3: Verificando usuario admin...'

-- Verificar si existe el usuario admin
DECLARE @UsuarioExiste INT
SELECT @UsuarioExiste = COUNT(*) FROM Usuario WHERE Nombre = N'admin'

IF @UsuarioExiste > 0
BEGIN
    PRINT '  ✓ Usuario admin YA EXISTE'
    PRINT ''

    -- Mostrar información del usuario
    SELECT
        Nombre AS [Usuario],
        Email,
        CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
        IdiomaPreferido AS Idioma,
        FechaUltimoAcceso AS [Último Acceso]
    FROM Usuario
    WHERE Nombre = N'admin'

    PRINT ''
    PRINT 'Credenciales:'
    PRINT '  Usuario: admin'
    PRINT '  Contraseña: admin123'
    PRINT ''
    PRINT 'Si no puedes ingresar, verifica la contraseña o ejecuta:'
    PRINT '  Database\RESETEAR_PASSWORD_ADMIN.sql'
    PRINT ''
    RETURN
END

PRINT '  ℹ Usuario admin NO existe - creando...'

-- =====================================================
-- PASO 4: Buscar el rol de administrador
-- =====================================================
PRINT ''
PRINT '▶ PASO 4: Buscando rol de administrador...'

DECLARE @IdRolAdmin UNIQUEIDENTIFIER
SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE Nombre = N'ROL_Administrador'

IF @IdRolAdmin IS NULL
BEGIN
    PRINT '  ✗ ERROR: No existe el rol ROL_Administrador'
    PRINT ''
    PRINT 'SOLUCIÓN: Ejecutar el script completo:'
    PRINT '  Database\00_EJECUTAR_TODO.sql'
    PRINT ''
    RETURN
END

PRINT '  ✓ Rol ROL_Administrador encontrado'

-- =====================================================
-- PASO 5: Crear usuario admin
-- =====================================================
PRINT ''
PRINT '▶ PASO 5: Creando usuario admin...'

DECLARE @IdUsuarioAdmin UNIQUEIDENTIFIER = NEWID()
DECLARE @NombreAdmin NVARCHAR(100) = N'admin'
DECLARE @EmailAdmin NVARCHAR(150) = N'admin@biblioteca.edu'
DECLARE @PasswordTextoPlano NVARCHAR(100) = N'admin123'
DECLARE @ClaveHasheada NVARCHAR(256) = CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', @PasswordTextoPlano), 2)

-- Calcular DVH
DECLARE @DVH NVARCHAR(64)
DECLARE @IdiomaAdmin NVARCHAR(10) = N'es-AR'
DECLARE @DatosParaDVH NVARCHAR(MAX) = UPPER(CAST(@IdUsuarioAdmin AS NVARCHAR(36))) + N'|' + @NombreAdmin + N'|' + @ClaveHasheada + N'|' + N'1'
SET @DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @DatosParaDVH), 2)

-- Insertar usuario
BEGIN TRY
    INSERT INTO Usuario (IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH) VALUES
    (@IdUsuarioAdmin, @NombreAdmin, @EmailAdmin, @ClaveHasheada, 1, @IdiomaAdmin, @DVH);

    PRINT '  ✓ Usuario creado exitosamente'
END TRY
BEGIN CATCH
    PRINT '  ✗ ERROR al crear usuario:'
    PRINT '    ' + ERROR_MESSAGE()
    RETURN
END CATCH

-- =====================================================
-- PASO 6: Asignar rol de administrador
-- =====================================================
PRINT ''
PRINT '▶ PASO 6: Asignando rol de administrador...'

BEGIN TRY
    INSERT INTO UsuarioFamilia (idUsuario, idFamilia) VALUES
    (@IdUsuarioAdmin, @IdRolAdmin);

    PRINT '  ✓ Rol asignado exitosamente'
END TRY
BEGIN CATCH
    PRINT '  ✗ ERROR al asignar rol:'
    PRINT '    ' + ERROR_MESSAGE()
    RETURN
END CATCH

-- =====================================================
-- RESUMEN
-- =====================================================
PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║         USUARIO ADMIN CREADO CON ÉXITO            ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Credenciales de acceso:'
PRINT '  Usuario:    admin'
PRINT '  Contraseña: admin123'
PRINT '  Email:      admin@biblioteca.edu'
PRINT '  Idioma:     es-AR (Español - Argentina)'
PRINT ''
PRINT 'Ahora puedes iniciar sesión en el sistema'
PRINT ''

-- Mostrar información del usuario creado
SELECT
    Nombre AS [Usuario],
    Email,
    CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
    IdiomaPreferido AS Idioma,
    FechaCreacion AS [Fecha Creación]
FROM Usuario
WHERE Nombre = N'admin'

PRINT ''
GO
