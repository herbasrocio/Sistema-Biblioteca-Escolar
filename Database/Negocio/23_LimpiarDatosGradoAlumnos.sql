-- Script para limpiar los datos corruptos en la columna Grado de la tabla Alumno
-- Elimina caracteres especiales mal codificados (°, Â, Ã, etc.)

USE NegocioBiblioteca
GO

-- Ver los datos actuales antes de limpiar
SELECT IdAlumno, Nombre, Apellido, Grado, Division
FROM Alumno
ORDER BY Grado, Division
GO

-- Limpiar la columna Grado eliminando caracteres especiales
-- Solo dejamos números (1, 2, 3, 4, 5, 6, 7)
UPDATE Alumno
SET Grado =
    CASE
        WHEN Grado LIKE '%1%' THEN '1'
        WHEN Grado LIKE '%2%' THEN '2'
        WHEN Grado LIKE '%3%' THEN '3'
        WHEN Grado LIKE '%4%' THEN '4'
        WHEN Grado LIKE '%5%' THEN '5'
        WHEN Grado LIKE '%6%' THEN '6'
        WHEN Grado LIKE '%7%' THEN '7'
        ELSE Grado
    END
WHERE Grado LIKE '%°%'
   OR Grado LIKE '%Â%'
   OR Grado LIKE '%Ã%'
   OR Grado LIKE '%º%'
   OR LEN(Grado) > 1
GO

-- Ver los datos después de limpiar
SELECT IdAlumno, Nombre, Apellido, Grado, Division
FROM Alumno
ORDER BY Grado, Division
GO

PRINT 'Datos de Grado limpiados exitosamente'
GO
