-- Script para actualizar géneros SIN tildes (evitar problemas de codificación)
-- Fecha: 2025-10-13

USE NegocioBiblioteca;
GO

-- 1. Eliminar restricción actual
DECLARE @ConstraintName NVARCHAR(200);
SELECT @ConstraintName = name
FROM sys.check_constraints
WHERE OBJECT_NAME(parent_object_id) = 'Material'
AND name LIKE '%Genero%';

IF @ConstraintName IS NOT NULL
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'ALTER TABLE Material DROP CONSTRAINT ' + @ConstraintName;
    EXEC sp_executesql @SQL;
    PRINT 'Restricción eliminada: ' + @ConstraintName;
END
GO

-- 2. Actualizar todos los registros existentes a formato sin tildes
UPDATE Material SET Genero = 'Fantasia' WHERE Genero LIKE 'Fantas%';
UPDATE Material SET Genero = 'Ciencia Ficcion' WHERE Genero LIKE 'Ciencia Ficc%';
UPDATE Material SET Genero = 'Historico' WHERE Genero LIKE 'Hist%';
UPDATE Material SET Genero = 'Educativo' WHERE Genero = 'Educativo';
UPDATE Material SET Genero = 'Biografia' WHERE Genero LIKE 'Biograf%';
UPDATE Material SET Genero = 'Poesia' WHERE Genero LIKE 'Poes%';
UPDATE Material SET Genero = 'Tecnico' WHERE Genero LIKE 'T_cnico' OR Genero LIKE 'Tecnico';
UPDATE Material SET Genero = 'Cientifico' WHERE Genero LIKE 'Cient%';
GO

PRINT 'Registros actualizados a formato sin tildes';
GO

-- 3. Crear nueva restricción CHECK SIN tildes
ALTER TABLE Material
ADD CONSTRAINT CK_Material_Genero CHECK (
    Genero IN (
        'Fantasia',
        'Ciencia Ficcion',
        'Aventura',
        'Misterio',
        'Romance',
        'Terror',
        'Historico',
        'Educativo',
        'Biografia',
        'Poesia',
        'Drama',
        'Comedia',
        'Infantil',
        'Juvenil',
        'Tecnico',
        'Cientifico',
        'Otro'
    )
);
GO

PRINT 'Nueva restricción CHECK creada (sin tildes)';
GO

-- 4. Verificar
SELECT 'Géneros actuales en la tabla:' AS Info;
SELECT DISTINCT Genero, COUNT(*) as Cantidad
FROM Material
GROUP BY Genero
ORDER BY Genero;
GO

PRINT 'Migración completada exitosamente';
