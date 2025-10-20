/*
Script para corregir acentos reemplazando los caracteres mal codificados
Este script reemplaza directamente los bytes incorrectos
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'CORRIGIENDO ACENTOS - MÉTODO DE REEMPLAZO'
PRINT '================================================'
PRINT ''

-- Mostrar estado actual
PRINT '--- ESTADO ACTUAL ---'
SELECT IdFamilia, Nombre, Descripcion FROM Familia ORDER BY Nombre;
PRINT ''

-- Reemplazar caracteres mal codificados comunes:
-- �� = ó
-- �� = í
-- Ǹ = é
-- ǭ = á

-- Actualizar todos los registros en una sola operación
UPDATE Familia
SET
    Nombre = REPLACE(REPLACE(REPLACE(REPLACE(
        Nombre,
        '��', 'ó'),
        '��', 'í'),
        'Ǹ', 'é'),
        'ǭ', 'á'),
    Descripcion = REPLACE(REPLACE(REPLACE(REPLACE(
        Descripcion,
        '��', 'ó'),
        '��', 'í'),
        'Ǹ', 'é'),
        'ǭ', 'á');

PRINT 'Registros actualizados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
PRINT ''

-- Mostrar estado final
PRINT '--- ESTADO FINAL ---'
SELECT IdFamilia, Nombre, Descripcion FROM Familia ORDER BY Nombre;
PRINT ''

PRINT '================================================'
PRINT 'CORRECCIÓN COMPLETADA'
PRINT '================================================'

GO
