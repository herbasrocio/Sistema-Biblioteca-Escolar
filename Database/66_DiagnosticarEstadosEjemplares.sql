-- Diagnosticar estados de ejemplares y calcular cantidades

PRINT '========================================';
PRINT '1. ESTADOS DE EJEMPLARES DE DON QUIJOTE';
PRINT '========================================';

SELECT
    e.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    e.Ubicacion,
    e.Estado,
    CASE e.Estado
        WHEN 0 THEN 'Prestado/No Disponible'
        WHEN 1 THEN 'Disponible'
        WHEN 2 THEN 'En Reparación'
        WHEN 3 THEN 'No Disponible'
        ELSE 'Desconocido (' + CAST(e.Estado AS VARCHAR) + ')'
    END AS EstadoDescripcion,
    e.Activo,
    m.Titulo
FROM Ejemplar e
INNER JOIN Material m ON e.IdMaterial = m.IdMaterial
WHERE m.Titulo = 'Don Quijote de la Mancha'
ORDER BY e.NumeroEjemplar;

PRINT '';
PRINT '========================================';
PRINT '2. RESUMEN DE CANTIDADES - DON QUIJOTE';
PRINT '========================================';

-- NOTA: Valores del enum EstadoMaterial:
-- 0 = Disponible
-- 1 = Prestado
-- 2 = EnReparacion
-- 3 = NoDisponible

SELECT
    m.Titulo,
    COUNT(*) AS TotalEjemplares,
    SUM(CASE WHEN e.Activo = 1 THEN 1 ELSE 0 END) AS EjemplaresActivos,
    SUM(CASE WHEN e.Activo = 1 AND e.Estado = 0 THEN 1 ELSE 0 END) AS Disponibles,
    SUM(CASE WHEN e.Activo = 1 AND e.Estado = 1 THEN 1 ELSE 0 END) AS Prestados,
    SUM(CASE WHEN e.Activo = 1 AND e.Estado = 2 THEN 1 ELSE 0 END) AS EnReparacion
FROM Material m
LEFT JOIN Ejemplar e ON e.IdMaterial = m.IdMaterial
WHERE m.Titulo = 'Don Quijote de la Mancha'
GROUP BY m.Titulo;

PRINT '';
PRINT '========================================';
PRINT '3. PRÉSTAMOS ACTIVOS DE DON QUIJOTE';
PRINT '========================================';

SELECT
    p.IdPrestamo,
    p.IdEjemplar,
    e.NumeroEjemplar,
    e.CodigoEjemplar,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.FechaPrestamo,
    p.Estado AS EstadoPrestamo,
    e.Estado AS EstadoEjemplar
FROM Prestamo p
INNER JOIN Material m ON p.IdMaterial = m.IdMaterial
LEFT JOIN Ejemplar e ON p.IdEjemplar = e.IdEjemplar
INNER JOIN Alumno a ON p.IdAlumno = a.IdAlumno
WHERE m.Titulo = 'Don Quijote de la Mancha'
    AND p.Estado IN ('Activo', 'Atrasado')
ORDER BY p.FechaPrestamo DESC;

PRINT '';
PRINT '========================================';
PRINT '4. VERIFICAR VALORES EN TABLA MATERIAL';
PRINT '========================================';

SELECT
    IdMaterial,
    Titulo,
    CantidadTotal AS CantTotal_EnTabla,
    CantidadDisponible AS CantDisp_EnTabla,
    (SELECT COUNT(*) FROM Ejemplar e WHERE e.IdMaterial = Material.IdMaterial AND e.Activo = 1) AS CantTotal_Real,
    (SELECT COUNT(*) FROM Ejemplar e WHERE e.IdMaterial = Material.IdMaterial AND e.Activo = 1 AND e.Estado = 0) AS CantDisp_Real
FROM Material
WHERE Titulo = 'Don Quijote de la Mancha';

PRINT '';
PRINT '========================================';
PRINT '5. TODOS LOS MATERIALES - COMPARACIÓN';
PRINT '========================================';

SELECT
    m.Titulo,
    m.CantidadTotal AS EnTabla_Total,
    m.CantidadDisponible AS EnTabla_Disp,
    ISNULL((SELECT COUNT(*) FROM Ejemplar e WHERE e.IdMaterial = m.IdMaterial AND e.Activo = 1), 0) AS Real_Total,
    ISNULL((SELECT COUNT(*) FROM Ejemplar e WHERE e.IdMaterial = m.IdMaterial AND e.Activo = 1 AND e.Estado = 0), 0) AS Real_Disp
FROM Material m
WHERE m.Activo = 1
ORDER BY m.Titulo;
