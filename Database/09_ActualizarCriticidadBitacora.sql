-- =============================================
-- Script: Renombrar columna Criticidad a Gravedad en BitacoraAdmin
-- Descripción:
--   1. Actualiza valores: Baja→Bajo, Media→Medio, Alta→Alto, Crítica→Alto
--   2. Renombra columna Criticidad → Gravedad
-- Fecha: 2025-01-30
-- =============================================

USE SeguridadBiblioteca;
GO

PRINT 'Actualizando niveles de Criticidad a Gravedad en BitacoraAdmin...';

-- Paso 1: Actualizar valores de criticidad existentes
UPDATE BitacoraAdmin
SET Criticidad = CASE Criticidad
    WHEN 'Baja' THEN 'Bajo'
    WHEN 'Media' THEN 'Medio'
    WHEN 'Alta' THEN 'Alto'
    WHEN 'Critica' THEN 'Alto'  -- Crítica se convierte en Alto
    ELSE Criticidad
END
WHERE Criticidad IN ('Baja', 'Media', 'Alta', 'Critica');

PRINT 'Valores actualizados correctamente.';

-- Paso 2: Renombrar la columna de Criticidad a Gravedad
EXEC sp_rename 'BitacoraAdmin.Criticidad', 'Gravedad', 'COLUMN';

PRINT 'Columna renombrada de Criticidad a Gravedad.';

-- Verificar resultados
SELECT
    Gravedad,
    COUNT(*) AS Cantidad
FROM BitacoraAdmin
GROUP BY Gravedad
ORDER BY
    CASE Gravedad
        WHEN 'Bajo' THEN 1
        WHEN 'Medio' THEN 2
        WHEN 'Alto' THEN 3
        ELSE 4
    END;

PRINT 'Actualización completada.';
PRINT 'Nuevos niveles de gravedad: Bajo, Medio, Alto';
GO
