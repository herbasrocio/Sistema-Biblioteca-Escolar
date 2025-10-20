-- Script para agregar columna IdiomaPreferido a tabla Usuario
-- Fecha: 2025-10-13

USE SeguridadBiblioteca;
GO

-- 1. Verificar estructura actual de la tabla Usuario
SELECT 'Estructura actual de Usuario:' AS Info;
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuario'
ORDER BY ORDINAL_POSITION;
GO

-- 2. Agregar columna IdiomaPreferido (si no existe)
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Usuario'
    AND COLUMN_NAME = 'IdiomaPreferido'
)
BEGIN
    ALTER TABLE Usuario
    ADD IdiomaPreferido NVARCHAR(10) NULL DEFAULT 'es-AR';

    PRINT 'Columna IdiomaPreferido agregada exitosamente con valor por defecto es-AR';
END
ELSE
BEGIN
    PRINT 'La columna IdiomaPreferido ya existe';
END
GO

-- 3. Actualizar usuarios existentes con idioma español por defecto
UPDATE Usuario
SET IdiomaPreferido = 'es-AR'
WHERE IdiomaPreferido IS NULL;
GO

PRINT 'Usuarios actualizados con idioma es-AR por defecto';
GO

-- 4. Verificar nueva estructura de la tabla
SELECT 'Estructura actualizada de Usuario:' AS Info;
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuario'
ORDER BY ORDINAL_POSITION;
GO

-- 5. Mostrar algunos datos de ejemplo
SELECT TOP 5
    'Datos de ejemplo:' AS Info,
    NombreUsuario,
    Email,
    IdiomaPreferido,
    Activo
FROM Usuario
ORDER BY NombreUsuario;
GO

PRINT 'Migración de IdiomaPreferido completada exitosamente';
