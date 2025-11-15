-- =====================================================
-- Script: Actualizar Usuarios Existentes con Campos Nuevos
-- Descripción: Agrega IdiomaPreferido por defecto a usuarios que no lo tienen
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║  ACTUALIZAR USUARIOS CON IDIOMA PREFERIDO         ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- Verificar si la columna IdiomaPreferido existe
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID(N'Usuario')
    AND name = 'IdiomaPreferido'
)
BEGIN
    PRINT '✗ ERROR: La columna IdiomaPreferido no existe en la tabla Usuario'
    PRINT ''
    PRINT 'La columna ya debería existir según el script 02_CrearTablas.sql'
    PRINT 'Verifica que hayas ejecutado el script correcto de creación de tablas'
    PRINT ''
    RETURN
END

PRINT '✓ Columna IdiomaPreferido existe'
PRINT ''

-- Mostrar usuarios actuales
PRINT 'Usuarios actuales:'
SELECT
    Nombre,
    Email,
    CASE WHEN Activo = 1 THEN 'Sí' ELSE 'No' END AS Activo,
    ISNULL(IdiomaPreferido, '(NULL)') AS IdiomaPreferido,
    FechaUltimoAcceso
FROM Usuario
ORDER BY Nombre

PRINT ''
PRINT 'Actualizando usuarios con idioma NULL...'

-- Actualizar usuarios que tienen IdiomaPreferido NULL
UPDATE Usuario
SET IdiomaPreferido = 'es-AR'
WHERE IdiomaPreferido IS NULL

DECLARE @UsuariosActualizados INT = @@ROWCOUNT

PRINT ''
IF @UsuariosActualizados > 0
BEGIN
    PRINT '✓ ' + CAST(@UsuariosActualizados AS VARCHAR(10)) + ' usuario(s) actualizado(s) con idioma es-AR'
END
ELSE
BEGIN
    PRINT '✓ Todos los usuarios ya tienen idioma configurado'
END

PRINT ''
PRINT 'Usuarios después de la actualización:'
SELECT
    Nombre,
    Email,
    CASE WHEN Activo = 1 THEN 'Sí' ELSE 'No' END AS Activo,
    IdiomaPreferido,
    FechaUltimoAcceso
FROM Usuario
ORDER BY Nombre

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║         ACTUALIZACIÓN COMPLETADA                   ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Ahora intenta iniciar sesión nuevamente con:'
PRINT '  • Usuario: admin (o cualquier usuario que tengas)'
PRINT '  • Contraseña: tu contraseña'
PRINT ''
GO
