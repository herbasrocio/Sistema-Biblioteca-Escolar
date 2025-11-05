/*
    Script para renombrar las tablas de bitácora con los nuevos nombres
    BitacoraAdmin -> BitacoraSeguridad
    Ejecutar en la base de datos SeguridadBiblioteca
*/

USE SeguridadBiblioteca;
GO

-- Verificar si la tabla antigua existe
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BitacoraAdmin')
BEGIN
    -- Renombrar la tabla BitacoraAdmin a BitacoraSeguridad
    EXEC sp_rename 'BitacoraAdmin', 'BitacoraSeguridad';
    PRINT 'Tabla BitacoraAdmin renombrada exitosamente a BitacoraSeguridad';
END
ELSE
BEGIN
    PRINT 'La tabla BitacoraAdmin no existe o ya fue renombrada';
END
GO

-- Verificar que la nueva tabla existe
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BitacoraSeguridad')
BEGIN
    PRINT 'Verificación exitosa: La tabla BitacoraSeguridad existe';

    -- Mostrar conteo de registros
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) FROM BitacoraSeguridad;
    PRINT 'Total de registros en BitacoraSeguridad: ' + CAST(@Count AS VARCHAR(10));
END
GO
