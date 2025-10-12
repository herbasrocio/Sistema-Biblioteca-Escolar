/*
    Script: 31_CorregirCheckEstadoEjemplar.sql
    Descripción: Corrige la restricción CHECK del campo Estado en tabla Ejemplar
                 para que coincida con los valores del enum EstadoMaterial (0-3)
    Fecha: 2025-10-12

    Valores del enum EstadoMaterial:
    - 0 = Disponible
    - 1 = Prestado
    - 2 = EnReparacion
    - 3 = NoDisponible
*/

USE NegocioBiblioteca;
GO

PRINT 'Iniciando corrección de restricción CHECK en tabla Ejemplar...';

-- Buscar el nombre de la restricción CHECK existente
DECLARE @ConstraintName NVARCHAR(200);
DECLARE @SQL NVARCHAR(MAX);

SELECT @ConstraintName = name
FROM sys.check_constraints
WHERE parent_object_id = OBJECT_ID('Ejemplar')
  AND COL_NAME(parent_object_id, parent_column_id) = 'Estado';

-- Si existe la restricción, eliminarla
IF @ConstraintName IS NOT NULL
BEGIN
    PRINT 'Eliminando restricción CHECK existente: ' + @ConstraintName;
    SET @SQL = 'ALTER TABLE Ejemplar DROP CONSTRAINT ' + QUOTENAME(@ConstraintName);
    EXEC sp_executesql @SQL;
    PRINT 'Restricción CHECK eliminada exitosamente.';
END
ELSE
BEGIN
    PRINT 'No se encontró restricción CHECK en el campo Estado.';
END

-- Crear nueva restricción CHECK con los valores correctos (0-3)
PRINT 'Creando nueva restricción CHECK con valores 0-3...';
ALTER TABLE Ejemplar
ADD CONSTRAINT CK_Ejemplar_Estado CHECK (Estado IN (0, 1, 2, 3));

PRINT 'Nueva restricción CHECK creada exitosamente.';
PRINT 'Valores permitidos: 0=Disponible, 1=Prestado, 2=EnReparacion, 3=NoDisponible';

GO

PRINT 'Script 31_CorregirCheckEstadoEjemplar.sql completado.';
