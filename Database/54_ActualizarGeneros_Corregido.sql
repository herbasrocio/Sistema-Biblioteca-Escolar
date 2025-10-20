-- Script para actualizar la restricción CHECK de géneros en tabla Material (CORREGIDO)
-- Fecha: 2025-10-13
-- Propósito: Ampliar los géneros permitidos para biblioteca escolar

USE NegocioBiblioteca;
GO

-- 1. Eliminar la restricción CHECK existente primero
DECLARE @ConstraintName NVARCHAR(200);
SELECT @ConstraintName = name
FROM sys.check_constraints
WHERE OBJECT_NAME(parent_object_id) = 'Material'
AND OBJECT_ID(name) IS NOT NULL
AND definition LIKE '%Genero%';

IF @ConstraintName IS NOT NULL
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'ALTER TABLE Material DROP CONSTRAINT ' + @ConstraintName;
    EXEC sp_executesql @SQL;
    PRINT 'Restricción anterior eliminada: ' + @ConstraintName;
END
ELSE
BEGIN
    PRINT 'No se encontró restricción de Genero para eliminar';
END
GO

-- 2. Actualizar registros existentes ANTES de crear la nueva restricción
PRINT 'Actualizando registros existentes...';

-- Actualizar con tilde
UPDATE Material SET Genero = 'Fantasía' WHERE Genero = 'Fantasia';
UPDATE Material SET Genero = 'Histórico' WHERE Genero = 'Historico';

-- Mapear géneros antiguos a nuevos
UPDATE Material SET Genero = 'Drama' WHERE Genero IN ('Teatral');
UPDATE Material SET Genero = 'Misterio' WHERE Genero IN ('Policial');
UPDATE Material SET Genero = 'Otro' WHERE Genero IN ('Novela', 'Cronica');

GO

PRINT 'Registros existentes actualizados';
GO

-- 3. Verificar que no queden registros con géneros no válidos
SELECT 'Géneros actuales en la tabla (antes de crear restricción):' AS Info;
SELECT DISTINCT Genero, COUNT(*) as Cantidad
FROM Material
GROUP BY Genero
ORDER BY Genero;
GO

-- 4. Crear nueva restricción CHECK con géneros ampliados
ALTER TABLE Material
ADD CONSTRAINT CK_Material_Genero CHECK (
    Genero IN (
        'Fantasía',
        'Ciencia Ficción',
        'Aventura',
        'Misterio',
        'Romance',
        'Terror',
        'Histórico',
        'Educativo',
        'Biografía',
        'Poesía',
        'Drama',
        'Comedia',
        'Infantil',
        'Juvenil',
        'Técnico',
        'Científico',
        'Otro'
    )
);
GO

PRINT 'Nueva restricción CHECK agregada con 17 géneros';
GO

-- 5. Verificar nueva restricción
SELECT 'Nueva restricción creada:' AS Info;
SELECT
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName,
    definition
FROM sys.check_constraints
WHERE OBJECT_NAME(parent_object_id) = 'Material'
AND name LIKE '%Genero%';
GO

PRINT 'Migración de géneros completada exitosamente';
