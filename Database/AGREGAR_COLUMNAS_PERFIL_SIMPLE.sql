-- =====================================================
-- Script: Agregar Columnas IdiomaPreferido y FechaUltimoAcceso
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Agregando columna IdiomaPreferido...'

-- Agregar IdiomaPreferido si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Usuario') AND name = 'IdiomaPreferido')
BEGIN
    ALTER TABLE Usuario ADD IdiomaPreferido NVARCHAR(10) NULL DEFAULT 'es-AR';
    PRINT '✓ Columna IdiomaPreferido agregada'
END
ELSE
BEGIN
    PRINT 'ℹ Columna IdiomaPreferido ya existe'
END

GO

PRINT 'Agregando columna FechaUltimoAcceso...'

-- Agregar FechaUltimoAcceso si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Usuario') AND name = 'FechaUltimoAcceso')
BEGIN
    ALTER TABLE Usuario ADD FechaUltimoAcceso DATETIME NULL;
    PRINT '✓ Columna FechaUltimoAcceso agregada'
END
ELSE
BEGIN
    PRINT 'ℹ Columna FechaUltimoAcceso ya existe'
END

GO

PRINT ''
PRINT '✓ COLUMNAS AGREGADAS EXITOSAMENTE'
PRINT 'Ahora puedes iniciar sesión en el sistema'
PRINT ''

-- Mostrar usuarios
SELECT Nombre, Email, CASE WHEN Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
FROM Usuario
ORDER BY Nombre

GO
