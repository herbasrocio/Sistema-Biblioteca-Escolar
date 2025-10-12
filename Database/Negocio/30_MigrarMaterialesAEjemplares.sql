/*
    Script: 30_MigrarMaterialesAEjemplares.sql
    Descripción: Migra materiales existentes creando ejemplares individuales
    Fecha: 2025-10-12

    Este script:
    1. Lee todos los materiales existentes
    2. Para cada material, crea N ejemplares donde N = CantidadTotal
    3. Asigna el estado del material original a todos sus ejemplares
    4. Elimina la columna Estado de la tabla Material (ahora está en Ejemplar)
*/

USE NegocioBiblioteca;
GO

PRINT 'Iniciando migración de materiales a ejemplares...';

-- Verificar si la tabla Ejemplar existe
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ejemplar')
BEGIN
    PRINT 'ERROR: La tabla Ejemplar no existe. Ejecute primero 29_CrearTablaEjemplar.sql';
    RETURN;
END

-- Verificar si ya hay ejemplares creados
IF EXISTS (SELECT TOP 1 * FROM Ejemplar)
BEGIN
    PRINT 'ADVERTENCIA: Ya existen ejemplares en la base de datos.';
    PRINT 'Se omitirá la migración automática para evitar duplicados.';
    PRINT 'Si desea recrear los ejemplares, elimine manualmente los datos de la tabla Ejemplar primero.';
END
ELSE
BEGIN
    PRINT 'Creando ejemplares para materiales existentes...';

    -- Crear ejemplares para cada material
    DECLARE @IdMaterial UNIQUEIDENTIFIER;
    DECLARE @CantidadTotal INT;
    DECLARE @Estado INT;
    DECLARE @Contador INT;

    DECLARE cur_materiales CURSOR FOR
        SELECT IdMaterial, CantidadTotal, Estado
        FROM Material
        WHERE Activo = 1 AND CantidadTotal > 0;

    OPEN cur_materiales;
    FETCH NEXT FROM cur_materiales INTO @IdMaterial, @CantidadTotal, @Estado;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Contador = 1;

        WHILE @Contador <= @CantidadTotal
        BEGIN
            INSERT INTO Ejemplar (
                IdEjemplar,
                IdMaterial,
                NumeroEjemplar,
                Estado,
                FechaRegistro,
                Activo
            )
            VALUES (
                NEWID(),
                @IdMaterial,
                @Contador,
                @Estado, -- El estado original del material
                GETDATE(),
                1
            );

            SET @Contador = @Contador + 1;
        END

        FETCH NEXT FROM cur_materiales INTO @IdMaterial, @CantidadTotal, @Estado;
    END

    CLOSE cur_materiales;
    DEALLOCATE cur_materiales;

    PRINT 'Ejemplares creados exitosamente.';

    -- Mostrar resumen de ejemplares creados
    SELECT
        m.Titulo,
        m.CantidadTotal,
        COUNT(e.IdEjemplar) AS EjemplaresCreados,
        SUM(CASE WHEN e.Estado = 1 THEN 1 ELSE 0 END) AS Disponibles,
        SUM(CASE WHEN e.Estado = 2 THEN 1 ELSE 0 END) AS Prestados,
        SUM(CASE WHEN e.Estado = 3 THEN 1 ELSE 0 END) AS EnReparacion,
        SUM(CASE WHEN e.Estado = 4 THEN 1 ELSE 0 END) AS NoDisponibles
    FROM Material m
    LEFT JOIN Ejemplar e ON m.IdMaterial = e.IdMaterial
    WHERE m.Activo = 1
    GROUP BY m.Titulo, m.CantidadTotal
    ORDER BY m.Titulo;
END

GO

-- Eliminar columna Estado de la tabla Material (ahora está en Ejemplar)
PRINT 'Eliminando columna Estado de la tabla Material...';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Material') AND name = 'Estado')
BEGIN
    -- Eliminar todos los constraints relacionados con la columna Estado
    DECLARE @ConstraintName NVARCHAR(200);
    DECLARE @SQL NVARCHAR(500);

    -- Eliminar constraint CHECK
    SELECT @ConstraintName = name
    FROM sys.check_constraints
    WHERE parent_object_id = OBJECT_ID('Material')
    AND COL_NAME(parent_object_id, parent_column_id) = 'Estado';

    IF @ConstraintName IS NOT NULL
    BEGIN
        SET @SQL = 'ALTER TABLE Material DROP CONSTRAINT ' + @ConstraintName;
        EXEC sp_executesql @SQL;
        PRINT 'Constraint CHECK de Estado eliminado: ' + @ConstraintName;
    END

    -- Eliminar constraint DEFAULT
    SELECT @ConstraintName = name
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID('Material')
    AND COL_NAME(parent_object_id, parent_column_id) = 'Estado';

    IF @ConstraintName IS NOT NULL
    BEGIN
        SET @SQL = 'ALTER TABLE Material DROP CONSTRAINT ' + @ConstraintName;
        EXEC sp_executesql @SQL;
        PRINT 'Constraint DEFAULT de Estado eliminado: ' + @ConstraintName;
    END

    -- Ahora eliminar la columna
    ALTER TABLE Material DROP COLUMN Estado;
    PRINT 'Columna Estado eliminada de Material exitosamente.';
END
ELSE
BEGIN
    PRINT 'La columna Estado ya no existe en Material.';
END

GO

PRINT 'Script 30_MigrarMaterialesAEjemplares.sql completado.';
