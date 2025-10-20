-- Script para limpiar préstamos de prueba y resetear ejemplares
-- EJECUTAR ESTE SCRIPT ANTES DE PROBAR LA CORRECCIÓN

-- 1. Primero, mostrar los préstamos activos de Romeo y Julieta
PRINT '========================================';
PRINT 'PRÉSTAMOS ACTIVOS DE ROMEO Y JULIETA (ANTES DE LIMPIAR)';
PRINT '========================================';

SELECT
    p.IdPrestamo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE m.Titulo = 'Romeo y Julieta'
    AND p.Estado IN ('Activo', 'Atrasado');

-- 2. Actualizar el estado del ejemplar #5 de Romeo y Julieta a Disponible
PRINT '';
PRINT '========================================';
PRINT 'MARCANDO EJEMPLAR #5 DE ROMEO Y JULIETA COMO DISPONIBLE';
PRINT '========================================';

UPDATE Ejemplar
SET Estado = 1  -- 1 = Disponible
WHERE IdEjemplar = 'A00226D8-6BAF-413E-A4BF-6F1B5AEC1F5F';

SELECT 'Ejemplar #5 actualizado a Disponible' AS Resultado;

-- 3. Verificar el estado actualizado
PRINT '';
PRINT '========================================';
PRINT 'VERIFICAR ESTADO DEL EJEMPLAR #5';
PRINT '========================================';

SELECT
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Estado,
    CASE e.Estado
        WHEN 0 THEN 'Prestado'
        WHEN 1 THEN 'Disponible'
        WHEN 2 THEN 'En Reparación'
        ELSE 'Desconocido'
    END AS EstadoDescripcion
FROM Ejemplar e
WHERE e.IdEjemplar = 'A00226D8-6BAF-413E-A4BF-6F1B5AEC1F5F';

PRINT '';
PRINT '========================================';
PRINT 'LISTO PARA PROBAR';
PRINT '========================================';
PRINT 'El ejemplar #5 de Romeo y Julieta está ahora disponible';
PRINT 'Puedes probarlo en la aplicación con el código corregido';
