-- Buscar el préstamo del ejemplar #5 de Romeo y Julieta que parece estar perdido

PRINT '========================================';
PRINT 'BUSCAR PRÉSTAMO CON ID EXACTO DEL EJEMPLAR #5 DE ROMEO Y JULIETA';
PRINT 'ID: A00226D8-6BAF-413E-A4BF-6F1B5AEC1F5F';
PRINT '========================================';

SELECT
    p.IdPrestamo,
    p.IdMaterial,
    p.IdEjemplar,
    p.IdAlumno,
    m.Titulo AS Material,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
LEFT JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Alumno a ON p.IdAlumno = a.IdAlumno
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
WHERE p.IdEjemplar = 'A00226D8-6BAF-413E-A4BF-6F1B5AEC1F5F';

PRINT '';
PRINT '========================================';
PRINT 'BUSCAR TODOS LOS PRÉSTAMOS DE ROMEO Y JULIETA';
PRINT '========================================';

SELECT
    p.IdPrestamo,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.FechaPrestamo,
    p.Estado,
    CASE
        WHEN d.IdDevolucion IS NOT NULL THEN 'Devuelto'
        ELSE 'Activo'
    END AS Devuelto
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
LEFT JOIN Devolucion d ON p.IdPrestamo = d.IdPrestamo
WHERE m.Titulo = 'Romeo y Julieta'
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT 'VERIFICAR: ÚLTIMO PRÉSTAMO REGISTRADO (CUALQUIER MATERIAL)';
PRINT '========================================';

SELECT TOP 5
    p.IdPrestamo,
    m.Titulo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT 'BUSCAR PRÉSTAMOS CON IdMaterial DE ROMEO Y JULIETA';
PRINT '========================================';

-- Primero obtener el IdMaterial de Romeo y Julieta
DECLARE @IdMaterialRomeo UNIQUEIDENTIFIER;
SELECT @IdMaterialRomeo = IdMaterial FROM Material WHERE Titulo = 'Romeo y Julieta';

PRINT 'IdMaterial de Romeo y Julieta: ' + CONVERT(VARCHAR(50), @IdMaterialRomeo);

SELECT
    p.IdPrestamo,
    p.IdMaterial,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE p.IdMaterial = @IdMaterialRomeo
ORDER BY p.FechaPrestamo DESC;
