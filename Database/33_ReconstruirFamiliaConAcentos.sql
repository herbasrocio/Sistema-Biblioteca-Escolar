/*
Script para RECONSTRUIR completamente la tabla Familia con acentos correctos
Este script ELIMINA y REINSERTA los datos con codificación UTF-8 correcta
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'RECONSTRUYENDO TABLA FAMILIA CON ACENTOS CORRECTOS'
PRINT '================================================'
PRINT ''

-- Paso 1: Guardar IDs y fechas originales
PRINT '--- Paso 1: Guardando IDs originales ---'
IF OBJECT_ID('tempdb..#FamiliaBackup') IS NOT NULL
    DROP TABLE #FamiliaBackup;

SELECT
    IdFamilia,
    Nombre,
    FechaCreacion
INTO #FamiliaBackup
FROM Familia;

PRINT 'Respaldo creado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros'
PRINT ''

-- Paso 2: Eliminar datos existentes (pero NO la tabla)
PRINT '--- Paso 2: Eliminando datos existentes ---'
DELETE FROM Familia;
PRINT 'Registros eliminados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
PRINT ''

-- Paso 3: Insertar datos corregidos con sus IDs originales
PRINT '--- Paso 3: Insertando datos con acentos correctos ---'

-- ROL_Docente
INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
SELECT
    IdFamilia,
    N'ROL_Docente',
    N'Rol de Docente - Gestión de préstamos y consultas',
    FechaCreacion
FROM #FamiliaBackup
WHERE Nombre LIKE 'ROL_Docente%';

PRINT 'ROL_Docente insertado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- ROL_Bibliotecario
INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
SELECT
    IdFamilia,
    N'ROL_Bibliotecario',
    N'Rol de Bibliotecario - Gestión de catálogo, préstamos y devoluciones',
    FechaCreacion
FROM #FamiliaBackup
WHERE Nombre LIKE 'ROL_Bibliotecario%';

PRINT 'ROL_Bibliotecario insertado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- Gestión de Usuarios
INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
SELECT
    IdFamilia,
    N'Gestión de Usuarios',
    N'Administración de usuarios del sistema',
    FechaCreacion
FROM #FamiliaBackup
WHERE Nombre LIKE '%Usuarios%' OR Nombre LIKE 'Gesti%n de Usuarios%';

PRINT 'Gestión de Usuarios insertado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- ROL_Administrador
INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
SELECT
    IdFamilia,
    N'ROL_Administrador',
    N'Rol de Administrador del Sistema - Acceso completo al sistema',
    FechaCreacion
FROM #FamiliaBackup
WHERE Nombre LIKE 'ROL_Administrador%';

PRINT 'ROL_Administrador insertado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- Configuración
INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
SELECT
    IdFamilia,
    N'Configuración',
    N'Configuración del sistema',
    FechaCreacion
FROM #FamiliaBackup
WHERE Nombre LIKE 'Configuraci%n%';

PRINT 'Configuración insertado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))

-- Gestión de Permisos
INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
SELECT
    IdFamilia,
    N'Gestión de Permisos',
    N'Administración de familias y patentes',
    FechaCreacion
FROM #FamiliaBackup
WHERE Nombre LIKE '%Permisos%' OR Nombre LIKE 'Gesti%n de Permisos%';

PRINT 'Gestión de Permisos insertado: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
PRINT ''

-- Paso 4: Limpiar tabla temporal
DROP TABLE #FamiliaBackup;
PRINT '--- Paso 4: Limpieza completada ---'
PRINT ''

-- Paso 5: Verificar resultados
PRINT '--- RESULTADOS FINALES ---'
SELECT
    CAST(IdFamilia AS VARCHAR(36)) AS IdFamilia,
    Nombre,
    Descripcion,
    FechaCreacion
FROM Familia
ORDER BY Nombre;

PRINT ''
PRINT '================================================'
PRINT 'RECONSTRUCCIÓN COMPLETADA EXITOSAMENTE'
PRINT '================================================'
PRINT ''
PRINT 'Los datos ahora deberían tener acentos correctos.'
PRINT 'Verifica en SQL Server Management Studio (SSMS).'
PRINT ''

GO
