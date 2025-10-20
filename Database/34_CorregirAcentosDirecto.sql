/*
Script para corregir acentos usando UPDATE directo con CAST a VARBINARY
Este método reemplaza los bytes incorrectos directamente
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'CORRIGIENDO ACENTOS - MÉTODO DIRECTO'
PRINT '================================================'
PRINT ''

-- Mostrar estado actual
PRINT '--- ESTADO ACTUAL ---'
SELECT Nombre, Descripcion FROM Familia ORDER BY Nombre;
PRINT ''

-- Actualizar cada registro individualmente con los IDs exactos de la base de datos

-- 1. Configuración (9FD5F833-E2BB-4374-B092-D7649630622C)
UPDATE Familia
SET
    Nombre = CAST(N'Configuración' AS NVARCHAR(100)),
    Descripcion = CAST(N'Configuración del sistema' AS NVARCHAR(255))
WHERE IdFamilia = '9FD5F833-E2BB-4374-B092-D7649630622C';
PRINT 'Configuración actualizado'

-- 2. Gestión de Permisos (B896823E-96A6-4D22-AA7F-F32BA5B44D75)
UPDATE Familia
SET
    Nombre = CAST(N'Gestión de Permisos' AS NVARCHAR(100)),
    Descripcion = CAST(N'Administración de familias y patentes' AS NVARCHAR(255))
WHERE IdFamilia = 'B896823E-96A6-4D22-AA7F-F32BA5B44D75';
PRINT 'Gestión de Permisos actualizado'

-- 3. Gestión de Usuarios (1BD9B5E6-9555-4D96-AEB7-30B8D3CD46C2)
UPDATE Familia
SET
    Nombre = CAST(N'Gestión de Usuarios' AS NVARCHAR(100)),
    Descripcion = CAST(N'Administración de usuarios del sistema' AS NVARCHAR(255))
WHERE IdFamilia = '1BD9B5E6-9555-4D96-AEB7-30B8D3CD46C2';
PRINT 'Gestión de Usuarios actualizado'

-- 4. ROL_Administrador (C5365ACA-995E-4DA8-9BC6-CCCEC9B31FA2)
UPDATE Familia
SET
    Nombre = CAST(N'ROL_Administrador' AS NVARCHAR(100)),
    Descripcion = CAST(N'Rol de Administrador del Sistema - Acceso completo al sistema' AS NVARCHAR(255))
WHERE IdFamilia = 'C5365ACA-995E-4DA8-9BC6-CCCEC9B31FA2';
PRINT 'ROL_Administrador actualizado'

-- 5. ROL_Bibliotecario (DD838EA8-9A37-4353-85A7-284C8766E0C4)
UPDATE Familia
SET
    Nombre = CAST(N'ROL_Bibliotecario' AS NVARCHAR(100)),
    Descripcion = CAST(N'Rol de Bibliotecario - Gestión de catálogo, préstamos y devoluciones' AS NVARCHAR(255))
WHERE IdFamilia = 'DD838EA8-9A37-4353-85A7-284C8766E0C4';
PRINT 'ROL_Bibliotecario actualizado'

-- 6. ROL_Docente (447BCDED-689F-45D8-8033-0A142C07C0E2)
UPDATE Familia
SET
    Nombre = CAST(N'ROL_Docente' AS NVARCHAR(100)),
    Descripcion = CAST(N'Rol de Docente - Gestión de préstamos y consultas' AS NVARCHAR(255))
WHERE IdFamilia = '447BCDED-689F-45D8-8033-0A142C07C0E2';
PRINT 'ROL_Docente actualizado'

PRINT ''
PRINT '--- ESTADO FINAL ---'
SELECT Nombre, Descripcion FROM Familia ORDER BY Nombre;
PRINT ''

PRINT '================================================'
PRINT 'CORRECCIÓN COMPLETADA'
PRINT '================================================'
PRINT ''
PRINT 'INSTRUCCIONES PARA VERIFICAR:'
PRINT '1. Abre SQL Server Management Studio (SSMS)'
PRINT '2. Ejecuta: SELECT * FROM SeguridadBiblioteca.dbo.Familia'
PRINT '3. Los acentos deberían verse correctamente'
PRINT ''
PRINT 'Si aún ves símbolos raros, el problema es de la'
PRINT 'herramienta de visualización, NO de la base de datos.'
PRINT ''

GO
