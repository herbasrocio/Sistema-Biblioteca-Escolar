-- Script para actualizar la restricción CHECK de géneros en tabla Material
-- Fecha: 2025-10-13
-- Propósito: Ampliar los géneros permitidos para biblioteca escolar

USE NegocioBiblioteca;
GO

-- 1. Verificar restricción actual
SELECT 'Restricción actual:' AS Info;
SELECT
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName,
    definition
FROM sys.check_constraints
WHERE OBJECT_NAME(parent_object_id) = 'Material'
AND name LIKE '%Genero%';
GO

-- 2. Eliminar la restricción CHECK existente
ALTER TABLE Material
DROP CONSTRAINT CK__Material__Genero__398D8EEE;
GO

PRINT 'Restricción anterior eliminada';
GO

-- 3. Crear nueva restricción CHECK con géneros ampliados
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

-- 4. Verificar nueva restricción
SELECT 'Nueva restricción:' AS Info;
SELECT
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName,
    definition
FROM sys.check_constraints
WHERE OBJECT_NAME(parent_object_id) = 'Material'
AND name LIKE '%Genero%';
GO

-- 5. Actualizar registros existentes que tengan géneros antiguos
UPDATE Material SET Genero = 'Fantasía' WHERE Genero = 'Fantasia';
UPDATE Material SET Genero = 'Histórico' WHERE Genero = 'Historico';
UPDATE Material SET Genero = 'Otro' WHERE Genero IN ('Novela', 'Cronica', 'Teatral', 'Policial');
GO

PRINT 'Registros existentes actualizados';
GO

-- 6. Mostrar datos de ejemplo
SELECT TOP 10
    'Datos de ejemplo:' AS Info,
    Titulo,
    Genero,
    Tipo
FROM Material
ORDER BY FechaRegistro DESC;
GO

PRINT 'Migración de géneros completada exitosamente';
