-- Buscar específicamente el préstamo del ejemplar #5 de Romeo y Julieta
-- ID del ejemplar #5: A00226D8-6BAF-413E-A4BF-6F1B5AEC1F5F

PRINT '========================================';
PRINT 'BUSCAR PRÉSTAMO DEL EJEMPLAR #5';
PRINT '========================================';

-- Buscar por el ID exacto del ejemplar
SELECT
    p.IdPrestamo,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    m.Titulo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.FechaPrestamo,
    p.FechaDevolucionPrevista,
    p.Estado,
    CASE
        WHEN d.IdDevolucion IS NOT NULL THEN 'Devuelto'
        ELSE 'No devuelto'
    END AS EstadoDevolucion
FROM Prestamo p
INNER JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
LEFT JOIN Devolucion d ON p.IdPrestamo = d.IdPrestamo
WHERE e.IdEjemplar = 'A00226D8-6BAF-413E-A4BF-6F1B5AEC1F5F'
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT 'TODOS LOS PRÉSTAMOS DE ALAN CLAU (ACTIVOS Y DEVUELTOS)';
PRINT '========================================';

SELECT
    p.IdPrestamo,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    m.Titulo,
    p.FechaPrestamo,
    p.Estado AS EstadoPrestamo,
    CASE
        WHEN d.IdDevolucion IS NOT NULL THEN 'Devuelto el ' + CONVERT(varchar, d.FechaDevolucion, 103)
        ELSE 'Activo/Pendiente'
    END AS EstadoDevolucion
FROM Prestamo p
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
LEFT JOIN Devolucion d ON p.IdPrestamo = d.IdPrestamo
WHERE a.Nombre = 'Alan' AND a.Apellido = 'Clau'
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT 'VERIFICAR: ¿EL EJEMPLAR #5 ESTÁ PRESTADO?';
PRINT '========================================';

SELECT
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Estado AS EstadoEjemplar,
    CASE e.Estado
        WHEN 0 THEN 'No Disponible/Prestado'
        WHEN 1 THEN 'Disponible'
        WHEN 2 THEN 'En Reparación'
        ELSE 'Desconocido'
    END AS EstadoDescripcion
FROM Ejemplar e
WHERE e.IdEjemplar = 'A00226D8-6BAF-413E-A4BF-6F1B5AEC1F5F';
