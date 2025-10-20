-- Investigar préstamos de Romeo y Julieta para debugging
-- Ejecutar en SQL Server Management Studio

-- 1. Ver todos los ejemplares de Romeo y Julieta
SELECT
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Ubicacion,
    e.Estado,
    m.Titulo
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE m.Titulo LIKE '%Romeo%Julieta%'
ORDER BY e.NumeroEjemplar;

-- 2. Ver los préstamos activos de Romeo y Julieta
SELECT
    p.IdPrestamo,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    m.Titulo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.FechaPrestamo,
    p.FechaDevolucionPrevista,
    p.Estado
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE m.Titulo LIKE '%Romeo%Julieta%'
    AND p.Estado IN ('Activo', 'Atrasado')
ORDER BY p.FechaPrestamo DESC;

-- 3. Ver TODOS los préstamos de Romeo y Julieta (activos e históricos)
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
        WHEN d.IdDevolucion IS NOT NULL THEN 'Devuelto el ' + CONVERT(varchar, d.FechaDevolucion, 103)
        ELSE 'No devuelto'
    END AS EstadoDevolucion
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
LEFT JOIN Devolucion d ON p.IdPrestamo = d.IdPrestamo
WHERE m.Titulo LIKE '%Romeo%Julieta%'
ORDER BY p.FechaPrestamo DESC;

-- 4. Verificar el ejemplar #5 específico de Romeo y Julieta
SELECT
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Ubicacion,
    e.Estado,
    m.Titulo
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE m.Titulo LIKE '%Romeo%Julieta%'
    AND e.NumeroEjemplar = 5;

-- 5. Verificar préstamos del ejemplar #5
SELECT
    p.IdPrestamo,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    m.Titulo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE e.NumeroEjemplar = 5
    AND m.Titulo LIKE '%Romeo%Julieta%'
ORDER BY p.FechaPrestamo DESC;
