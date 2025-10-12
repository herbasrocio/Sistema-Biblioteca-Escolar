/*
    Script: 32_ActualizarValoresEstadoEjemplar.sql
    Descripción: Actualiza los valores existentes del campo Estado en tabla Ejemplar
                 de la escala 1-4 a la escala 0-3 para coincidir con el enum
    Fecha: 2025-10-12

    Conversión:
    - 1 (Disponible) -> 0
    - 2 (Prestado) -> 1
    - 3 (EnReparacion) -> 2
    - 4 (NoDisponible) -> 3
*/

USE NegocioBiblioteca;
GO

PRINT 'Iniciando actualización de valores de Estado en tabla Ejemplar...';

-- Mostrar cantidad de registros a actualizar
DECLARE @TotalRegistros INT;
SELECT @TotalRegistros = COUNT(*) FROM Ejemplar WHERE Estado BETWEEN 1 AND 4;
PRINT 'Registros a actualizar: ' + CAST(@TotalRegistros AS NVARCHAR(10));

-- Actualizar valores de Estado (restar 1 a cada valor)
-- Usar transacción para asegurar consistencia
BEGIN TRANSACTION;

BEGIN TRY
    -- Actualizar Estado: restar 1 a todos los valores
    UPDATE Ejemplar
    SET Estado = Estado - 1
    WHERE Estado BETWEEN 1 AND 4;

    DECLARE @RegistrosActualizados INT = @@ROWCOUNT;
    PRINT 'Registros actualizados: ' + CAST(@RegistrosActualizados AS NVARCHAR(10));

    -- Verificar que no haya valores fuera de rango
    DECLARE @RegistrosFueraRango INT;
    SELECT @RegistrosFueraRango = COUNT(*) FROM Ejemplar WHERE Estado NOT IN (0, 1, 2, 3);

    IF @RegistrosFueraRango > 0
    BEGIN
        PRINT 'ADVERTENCIA: Se encontraron ' + CAST(@RegistrosFueraRango AS NVARCHAR(10)) + ' registros con valores fuera de rango.';
        ROLLBACK TRANSACTION;
        PRINT 'Transacción revertida.';
    END
    ELSE
    BEGIN
        COMMIT TRANSACTION;
        PRINT 'Transacción confirmada exitosamente.';

        -- Mostrar distribución de estados
        PRINT '';
        PRINT 'Distribución de estados actualizada:';
        SELECT
            Estado,
            CASE Estado
                WHEN 0 THEN 'Disponible'
                WHEN 1 THEN 'Prestado'
                WHEN 2 THEN 'EnReparacion'
                WHEN 3 THEN 'NoDisponible'
                ELSE 'Desconocido'
            END AS NombreEstado,
            COUNT(*) AS Cantidad
        FROM Ejemplar
        GROUP BY Estado
        ORDER BY Estado;
    END
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'ERROR: ' + ERROR_MESSAGE();
    PRINT 'Transacción revertida.';
END CATCH

GO

PRINT 'Script 32_ActualizarValoresEstadoEjemplar.sql completado.';
