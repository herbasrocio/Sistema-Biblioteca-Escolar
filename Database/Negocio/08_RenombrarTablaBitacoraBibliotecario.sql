/*
    Script para renombrar la tabla de bitácora de bibliotecario
    BitacoraBibliotecario -> BitacoraOperaciones
    Ejecutar en la base de datos NegocioBiblioteca
*/

USE NegocioBiblioteca;
GO

-- Verificar si la tabla antigua existe
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BitacoraBibliotecario')
BEGIN
    -- Renombrar la tabla BitacoraBibliotecario a BitacoraOperaciones
    EXEC sp_rename 'BitacoraBibliotecario', 'BitacoraOperaciones';
    PRINT 'Tabla BitacoraBibliotecario renombrada exitosamente a BitacoraOperaciones';
END
ELSE
BEGIN
    PRINT 'La tabla BitacoraBibliotecario no existe o ya fue renombrada';
END
GO

-- Verificar que la nueva tabla existe
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'BitacoraOperaciones')
BEGIN
    PRINT 'Verificación exitosa: La tabla BitacoraOperaciones existe';

    -- Mostrar conteo de registros
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) FROM BitacoraOperaciones;
    PRINT 'Total de registros en BitacoraOperaciones: ' + CAST(@Count AS VARCHAR(10));
END
GO
