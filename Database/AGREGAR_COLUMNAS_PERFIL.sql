-- =====================================================
-- Script: Agregar Columnas para Mi Perfil
-- Descripción: Agrega IdiomaPreferido y FechaUltimoAcceso a tabla Usuario
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║  AGREGAR COLUMNAS PARA MI PERFIL                  ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- =====================================================
-- Agregar columna IdiomaPreferido
-- =====================================================
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID(N'Usuario')
    AND name = 'IdiomaPreferido'
)
BEGIN
    PRINT '▶ Agregando columna IdiomaPreferido...'

    ALTER TABLE Usuario
    ADD IdiomaPreferido NVARCHAR(10) NULL DEFAULT 'es-AR';

    PRINT '  ✓ Columna IdiomaPreferido agregada'
    PRINT '  ✓ Usuarios existentes tendrán idioma es-AR por defecto'
END
ELSE
BEGIN
    PRINT '  ℹ Columna IdiomaPreferido ya existe'
END

PRINT ''

-- =====================================================
-- Agregar columna FechaUltimoAcceso (si no existe)
-- =====================================================
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID(N'Usuario')
    AND name = 'FechaUltimoAcceso'
)
BEGIN
    PRINT '▶ Agregando columna FechaUltimoAcceso...'

    ALTER TABLE Usuario
    ADD FechaUltimoAcceso DATETIME NULL;

    PRINT '  ✓ Columna FechaUltimoAcceso agregada'
END
ELSE
BEGIN
    PRINT '  ℹ Columna FechaUltimoAcceso ya existe'
END

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║         COLUMNAS AGREGADAS CON ÉXITO               ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- Mostrar estructura actualizada de la tabla Usuario
PRINT 'Estructura actualizada de la tabla Usuario:'
PRINT ''

SELECT
    COLUMN_NAME AS [Columna],
    DATA_TYPE AS [Tipo],
    CHARACTER_MAXIMUM_LENGTH AS [Longitud],
    IS_NULLABLE AS [Nullable],
    COLUMN_DEFAULT AS [Default]
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuario'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT 'Usuarios actuales con idioma configurado:'
PRINT ''

-- Verificar que ambas columnas existan antes de hacer el SELECT
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Usuario') AND name = 'IdiomaPreferido')
    AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Usuario') AND name = 'FechaUltimoAcceso')
BEGIN
    SELECT
        Nombre,
        Email,
        IdiomaPreferido,
        FechaUltimoAcceso,
        CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
    FROM Usuario
    ORDER BY Nombre
END
ELSE
BEGIN
    SELECT
        Nombre,
        Email,
        CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
    FROM Usuario
    ORDER BY Nombre
END

PRINT ''
PRINT 'Ahora puedes iniciar sesión en el sistema'
PRINT ''
GO
