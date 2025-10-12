-- =====================================================
-- Script: Agregar Estado a Material
-- Descripción: Agrega la columna Estado a la tabla Material
--              Estados: 1=Disponible, 2=Prestado, 3=En reparación, 4=No disponible
-- Fecha: 2025-10-12
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Agregando columna Estado a tabla Material...'
PRINT ''

-- =====================================================
-- Agregar columna Estado
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Material') AND name = 'Estado')
BEGIN
    PRINT 'Agregando columna Estado...'

    -- Agregar columna Estado con valor por defecto 1 (Disponible)
    ALTER TABLE Material
    ADD Estado INT NOT NULL DEFAULT 1
    CHECK (Estado IN (1, 2, 3, 4));

    PRINT '  ✓ Columna Estado agregada con valor por defecto: 1 (Disponible)'
    PRINT '  ✓ Constraint CHECK agregado (valores válidos: 1, 2, 3, 4)'
END
ELSE
BEGIN
    PRINT '  ⚠ Columna Estado ya existe'
END

PRINT ''
PRINT '=== Columna Estado Agregada Exitosamente ==='
PRINT ''
PRINT 'VALORES DEL ENUM EstadoMaterial:'
PRINT '  1 = Disponible      (Estado por defecto para nuevos materiales)'
PRINT '  2 = Prestado        (Material prestado a un alumno)'
PRINT '  3 = En reparación   (Material en mantenimiento o reparación)'
PRINT '  4 = No disponible   (Material no disponible temporalmente)'
PRINT ''
PRINT 'NOTA: Todos los materiales existentes han sido marcados como "Disponible" (Estado = 1)'
PRINT ''

-- Mostrar estructura actualizada de la tabla
PRINT 'ESTRUCTURA ACTUALIZADA DE LA TABLA MATERIAL:'
SELECT
    COLUMN_NAME AS 'Columna',
    DATA_TYPE AS 'Tipo',
    IS_NULLABLE AS 'Permite NULL',
    COLUMN_DEFAULT AS 'Valor por Defecto'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Material'
ORDER BY ORDINAL_POSITION;

PRINT ''
PRINT 'Actualización completada.'
PRINT ''
GO
