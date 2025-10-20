-- DIAGNÓSTICO COMPLETO DEL PROBLEMA DE EJEMPLARES
-- Ejecutar en SQL Server Management Studio

PRINT '========================================';
PRINT '1. TODOS LOS EJEMPLARES DE ROMEO Y JULIETA';
PRINT '========================================';
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

PRINT '';
PRINT '========================================';
PRINT '2. VERIFICAR EJEMPLAR #5 ESPECÍFICO';
PRINT '========================================';
SELECT
    'EJEMPLAR #5' AS Info,
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Ubicacion,
    e.Estado
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE m.Titulo LIKE '%Romeo%Julieta%'
    AND e.NumeroEjemplar = 5;

PRINT '';
PRINT '========================================';
PRINT '3. TODOS LOS PRÉSTAMOS ACTIVOS (TODAS LAS MATERIAS)';
PRINT '========================================';
SELECT
    p.IdPrestamo,
    m.Titulo AS Material,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.IdEjemplar AS 'ID_Ejemplar_en_Prestamo',
    e.NumeroEjemplar AS 'Num_Ej',
    e.CodigoEjemplar AS 'Codigo',
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE p.Estado IN ('Activo', 'Atrasado')
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT '4. PRÉSTAMOS DE ROMEO Y JULIETA (ACTIVOS)';
PRINT '========================================';
SELECT
    p.IdPrestamo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.IdEjemplar AS ID_Ejemplar_en_Prestamo,
    e.NumeroEjemplar AS Numero_Ejemplar,
    e.CodigoEjemplar AS Codigo_Ejemplar,
    e.Ubicacion,
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

PRINT '';
PRINT '========================================';
PRINT '5. ÚLTIMO PRÉSTAMO REGISTRADO (cualquier material)';
PRINT '========================================';
SELECT TOP 1
    p.IdPrestamo,
    m.Titulo AS Material,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.IdEjemplar AS ID_Ejemplar_en_BD,
    e.NumeroEjemplar AS Numero_Ejemplar_Real,
    e.CodigoEjemplar AS Codigo_Ejemplar_Real,
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT '6. BUSCAR EL CÓDIGO ESPECÍFICO: BIB-22E31657-005';
PRINT '========================================';
SELECT
    'BÚSQUEDA POR CÓDIGO' AS Info,
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    m.Titulo,
    e.Estado
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE e.CodigoEjemplar = 'BIB-22E31657-005';

PRINT '';
PRINT '========================================';
PRINT '7. PRÉSTAMOS DEL CÓDIGO BIB-22E31657-005';
PRINT '========================================';
SELECT
    p.IdPrestamo,
    p.IdEjemplar,
    e.CodigoEjemplar,
    e.NumeroEjemplar,
    m.Titulo,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.FechaPrestamo,
    p.Estado
FROM Prestamo p
INNER JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE e.CodigoEjemplar = 'BIB-22E31657-005'
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT '8. COMPARAR: ¿HAY EJEMPLARES CON IDS SIMILARES?';
PRINT '========================================';
-- Buscar si hay confusión entre ejemplares #2 y #5
SELECT
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    m.Titulo,
    e.Estado
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE m.Titulo LIKE '%Romeo%Julieta%'
    AND e.NumeroEjemplar IN (2, 5)
ORDER BY e.NumeroEjemplar;
