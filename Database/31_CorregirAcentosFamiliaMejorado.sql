/*
Script para corregir acentos y caracteres especiales en la tabla Familia
Versión mejorada: busca por nombre sin importar ID
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'CORRIGIENDO ACENTOS EN TABLA FAMILIA (V2)'
PRINT '================================================'
PRINT ''

-- Mostrar estado actual
PRINT '--- ESTADO ACTUAL ---'
SELECT
    IdFamilia,
    Nombre,
    Descripcion
FROM Familia
ORDER BY Nombre;
PRINT ''

-- Corregir ROL_Docente
UPDATE Familia
SET
    Nombre = N'ROL_Docente',
    Descripcion = N'Rol de Docente - Gestión de préstamos y consultas'
WHERE Nombre LIKE 'ROL_Docente%' OR Descripcion LIKE '%Docente%';

PRINT 'ROL_Docente actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- Corregir ROL_Bibliotecario
UPDATE Familia
SET
    Nombre = N'ROL_Bibliotecario',
    Descripcion = N'Rol de Bibliotecario - Gestión de catálogo, préstamos y devoluciones'
WHERE Nombre LIKE 'ROL_Bibliotecario%' OR Descripcion LIKE '%Bibliotecario%';

PRINT 'ROL_Bibliotecario actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- Corregir Gestión de Usuarios
UPDATE Familia
SET
    Nombre = N'Gestión de Usuarios',
    Descripcion = N'Administración de usuarios del sistema'
WHERE Nombre LIKE '%Usuarios%' OR Nombre LIKE 'Gesti%n de Usuarios%';

PRINT 'Gestión de Usuarios actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- Corregir ROL_Administrador
UPDATE Familia
SET
    Nombre = N'ROL_Administrador',
    Descripcion = N'Rol de Administrador del Sistema - Acceso completo al sistema'
WHERE Nombre LIKE 'ROL_Administrador%' OR Descripcion LIKE '%Administrador del Sistema%';

PRINT 'ROL_Administrador actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- Corregir Configuración
UPDATE Familia
SET
    Nombre = N'Configuración',
    Descripcion = N'Configuración del sistema'
WHERE Nombre LIKE 'Configuraci%n%';

PRINT 'Configuración actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- Corregir Gestión de Permisos
UPDATE Familia
SET
    Nombre = N'Gestión de Permisos',
    Descripcion = N'Administración de familias y patentes'
WHERE Nombre LIKE '%Permisos%' OR Nombre LIKE 'Gesti%n de Permisos%';

PRINT 'Gestión de Permisos actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'
PRINT ''

-- Mostrar estado final
PRINT '--- ESTADO FINAL (CORREGIDO) ---'
SELECT
    IdFamilia,
    Nombre,
    Descripcion
FROM Familia
ORDER BY Nombre;
PRINT ''

PRINT '================================================'
PRINT 'CORRECCIÓN COMPLETADA'
PRINT '================================================'
PRINT ''
PRINT 'Verifica los resultados en tu aplicación o'
PRINT 'herramienta de base de datos.'
PRINT ''

GO
