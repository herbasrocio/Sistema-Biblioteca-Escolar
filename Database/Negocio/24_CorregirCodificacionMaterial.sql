-- Script para corregir problemas de codificación en la tabla Material
-- Reemplaza caracteres corruptos por los caracteres correctos

USE NegocioBiblioteca
GO

PRINT 'Corrigiendo problemas de codificación en tabla Material...'
PRINT ''

-- Ver datos antes de la corrección
PRINT 'DATOS ANTES DE LA CORRECCIÓN:'
SELECT Titulo, Autor, Editorial
FROM Material
ORDER BY Titulo
GO

-- Corregir tildes y caracteres especiales mal codificados
UPDATE Material
SET
    Titulo = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
             REPLACE(REPLACE(REPLACE(REPLACE(
             Titulo,
             'Ã¡', 'á'),  -- á mal codificada
             'Ã©', 'é'),  -- é mal codificada
             'Ã­', 'í'),  -- í mal codificada
             'Ã³', 'ó'),  -- ó mal codificada
             'Ãº', 'ú'),  -- ú mal codificada
             'Ã±', 'ñ'),  -- ñ mal codificada
             'Ã', 'Á'),   -- Á mal codificada
             'Ã‰', 'É'),  -- É mal codificada
             'Ã', 'Í'),   -- Í mal codificada
             'Ã"', 'Ó'),  -- Ó mal codificada

    Autor = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
            REPLACE(REPLACE(REPLACE(REPLACE(
            Autor,
            'Ã¡', 'á'),
            'Ã©', 'é'),
            'Ã­', 'í'),
            'Ã³', 'ó'),
            'Ãº', 'ú'),
            'Ã±', 'ñ'),
            'Ã', 'Á'),
            'Ã‰', 'É'),
            'Ã', 'Í'),
            'Ã"', 'Ó'),

    Editorial = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                REPLACE(REPLACE(REPLACE(REPLACE(
                Editorial,
                'Ã¡', 'á'),
                'Ã©', 'é'),
                'Ã­', 'í'),
                'Ã³', 'ó'),
                'Ãº', 'ú'),
                'Ã±', 'ñ'),
                'Ã', 'Á'),
                'Ã‰', 'É'),
                'Ã', 'Í'),
                'Ã"', 'Ó')
GO

-- Segundo pase para caracteres que aparecen en la salida de sqlcmd
UPDATE Material
SET
    Titulo = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
             Titulo,
             'A��os', 'Años'),
             'C��digo', 'Código'),
             'Cr��nica', 'Crónica'),
             'Drǭcula', 'Drácula'),
             'ExupǸry', 'Exupéry'),

    Autor = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
            Autor,
            'Garc��a', 'García'),
            'Mǭrquez', 'Márquez'),
            'Espa��ola', 'Española'),
            'Gramǭtica', 'Gramática'),
            'Matemǭticas', 'Matemáticas'),
            'ExupǸry', 'Exupéry'),

    Editorial = REPLACE(REPLACE(REPLACE(REPLACE(
                Editorial,
                'Cǭtedra', 'Cátedra'),
                'Porrǧa', 'Porrúa'),
                'Espa��ola', 'Española'),
                'Gramǭtica', 'Gramática')
GO

-- Tercer pase para limpiar cualquier otro carácter extraño restante
UPDATE Material
SET
    Titulo = REPLACE(REPLACE(REPLACE(REPLACE(
             Titulo,
             '��', 'ñ'),
             'Ǹ', 'é'),
             'ǭ', 'ó'),
             'ǧ', 'ú'),

    Autor = REPLACE(REPLACE(REPLACE(REPLACE(
            Autor,
            '��', 'ñ'),
            'Ǹ', 'é'),
            'ǭ', 'ó'),
            'ǧ', 'ú'),

    Editorial = REPLACE(REPLACE(REPLACE(REPLACE(
                Editorial,
                '��', 'ñ'),
                'Ǹ', 'é'),
                'ǭ', 'ó'),
                'ǧ', 'ú')
GO

-- Ver datos después de la corrección
PRINT ''
PRINT 'DATOS DESPUÉS DE LA CORRECCIÓN:'
SELECT Titulo, Autor, Editorial
FROM Material
ORDER BY Titulo
GO

PRINT ''
PRINT '=== Codificación corregida exitosamente ==='
GO
