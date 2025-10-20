-- =============================================
-- Script: Verificar si existe la columna CodigoEjemplar
-- =============================================

USE NegocioBiblioteca
GO

-- Verificar columnas de la tabla Ejemplar
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Ejemplar'
ORDER BY ORDINAL_POSITION
GO
