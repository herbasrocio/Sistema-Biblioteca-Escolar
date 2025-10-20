USE SeguridadBiblioteca;
GO

-- Actualizar directamente con c贸digos Unicode usando CHAR()
-- 243 = 贸, 237 = 铆, 225 = 谩, 233 = 茅

-- ConfiguraciÃ³n -> Configuraci贸n
UPDATE Familia
SET
    Nombre = N'Configuraci' + NCHAR(243) + N'n',
    Descripcion = N'Configuraci' + NCHAR(243) + N'n del sistema'
WHERE Nombre LIKE 'Configuraci%';

-- Gesti贸n de Permisos
UPDATE Familia
SET
    Nombre = N'Gesti' + NCHAR(243) + N'n de Permisos',
    Descripcion = N'Administraci' + NCHAR(243) + N'n de familias y patentes'
WHERE Nombre LIKE '%Permisos%';

-- Gesti贸n de Usuarios
UPDATE Familia
SET
    Nombre = N'Gesti' + NCHAR(243) + N'n de Usuarios',
    Descripcion = N'Administraci' + NCHAR(243) + N'n de usuarios del sistema'
WHERE Nombre LIKE '%Usuarios%';

-- ROL_Bibliotecario - Gesti贸n de cat谩logo, pr茅stamos
UPDATE Familia
SET
    Nombre = N'ROL_Bibliotecario',
    Descripcion = N'Rol de Bibliotecario - Gesti' + NCHAR(243) + N'n de cat' + NCHAR(225) + N'logo, pr' + NCHAR(233) + N'stamos y devoluciones'
WHERE Nombre LIKE 'ROL_Bibliotecario%';

-- ROL_Docente - Gesti贸n de pr茅stamos
UPDATE Familia
SET
    Nombre = N'ROL_Docente',
    Descripcion = N'Rol de Docente - Gesti' + NCHAR(243) + N'n de pr' + NCHAR(233) + N'stamos y consultas'
WHERE Nombre LIKE 'ROL_Docente%';

-- Verificar resultados
SELECT
    Nombre AS [Nombre del Rol],
    Descripcion AS [Descripci贸n]
FROM Familia
ORDER BY Nombre;

GO
