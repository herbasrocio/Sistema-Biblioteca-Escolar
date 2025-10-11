-- =====================================================
-- Script: Actualizar Tabla Alumno (Eliminar Columnas)
-- Descripción: Elimina las columnas Email, Telefono y Activo
--              de la tabla Alumno existente
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT ''
PRINT '=========================================='
PRINT '  ACTUALIZAR TABLA ALUMNO'
PRINT '=========================================='
PRINT ''

-- Verificar si existe la base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'NegocioBiblioteca')
BEGIN
    PRINT 'ERROR: La base de datos NegocioBiblioteca NO existe'
    PRINT 'Ejecutar primero: Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql'
    RETURN
END

-- Verificar si existe la tabla
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Alumno')
BEGIN
    PRINT 'ERROR: La tabla Alumno NO existe'
    PRINT 'Ejecutar primero: Database\Negocio\02_CrearTablasNegocio.sql'
    RETURN
END

PRINT 'Verificando columnas existentes...'
PRINT ''

-- Verificar y eliminar columna Email
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Alumno') AND name = 'Email')
BEGIN
    PRINT 'Eliminando columna Email...'
    ALTER TABLE Alumno DROP COLUMN Email;
    PRINT '  OK Columna Email eliminada'
END
ELSE
BEGIN
    PRINT '  - Columna Email ya no existe'
END

-- Verificar y eliminar columna Telefono
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Alumno') AND name = 'Telefono')
BEGIN
    PRINT 'Eliminando columna Telefono...'
    ALTER TABLE Alumno DROP COLUMN Telefono;
    PRINT '  OK Columna Telefono eliminada'
END
ELSE
BEGIN
    PRINT '  - Columna Telefono ya no existe'
END

-- Verificar y eliminar columna Activo
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Alumno') AND name = 'Activo')
BEGIN
    PRINT 'Eliminando columna Activo...'

    -- Primero eliminar el constraint de default si existe
    DECLARE @ConstraintName NVARCHAR(200)
    SELECT @ConstraintName = name
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID('Alumno')
    AND parent_column_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID('Alumno') AND name = 'Activo')

    IF @ConstraintName IS NOT NULL
    BEGIN
        PRINT '  - Eliminando constraint: ' + @ConstraintName
        EXEC('ALTER TABLE Alumno DROP CONSTRAINT ' + @ConstraintName)
    END

    -- Ahora eliminar la columna
    ALTER TABLE Alumno DROP COLUMN Activo;
    PRINT '  OK Columna Activo eliminada'
END
ELSE
BEGIN
    PRINT '  - Columna Activo ya no existe'
END

PRINT ''
PRINT '=========================================='
PRINT '  ESTRUCTURA ACTUAL DE TABLA ALUMNO'
PRINT '=========================================='
PRINT ''

-- Mostrar columnas actuales
SELECT
    COLUMN_NAME AS Columna,
    DATA_TYPE AS Tipo,
    CHARACTER_MAXIMUM_LENGTH AS Longitud,
    IS_NULLABLE AS Nullable
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Alumno'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT 'OK Tabla Alumno actualizada correctamente'
PRINT ''
PRINT 'COLUMNAS ACTUALES:'
PRINT '  - IdAlumno (UNIQUEIDENTIFIER)'
PRINT '  - Nombre (NVARCHAR)'
PRINT '  - Apellido (NVARCHAR)'
PRINT '  - DNI (NVARCHAR)'
PRINT '  - Grado (NVARCHAR)'
PRINT '  - Division (NVARCHAR)'
PRINT '  - FechaRegistro (DATETIME)'
PRINT ''

GO
