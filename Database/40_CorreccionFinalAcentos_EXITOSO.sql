/*
Script FINAL para corregir acentos en tabla Familia
ESTE SCRIPT FUNCIONÓ CORRECTAMENTE
Fecha: 2025-10-13
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'CORRIGIENDO ACENTOS EN TABLA FAMILIA'
PRINT '================================================'
PRINT ''

-- Configuración
UPDATE Familia
SET Nombre = N'Configuración',
    Descripcion = N'Configuración del sistema'
WHERE IdFamilia = '9FD5F833-E2BB-4374-B092-D7649630622C';
PRINT 'Configuración actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- Gestión de Permisos
UPDATE Familia
SET Nombre = N'Gestión de Permisos',
    Descripcion = N'Administración de familias y patentes'
WHERE IdFamilia = 'B896823E-96A6-4D22-AA7F-F32BA5B44D75';
PRINT 'Gestión de Permisos actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- Gestión de Usuarios
UPDATE Familia
SET Nombre = N'Gestión de Usuarios',
    Descripcion = N'Administración de usuarios del sistema'
WHERE IdFamilia = '1BD9B5E6-9555-4D96-AEB7-30B8D3CD46C2';
PRINT 'Gestión de Usuarios actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- ROL_Bibliotecario
UPDATE Familia
SET Nombre = N'ROL_Bibliotecario',
    Descripcion = N'Rol de Bibliotecario - Gestión de catálogo, préstamos y devoluciones'
WHERE IdFamilia = 'DD838EA8-9A37-4353-85A7-284C8766E0C4';
PRINT 'ROL_Bibliotecario actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- ROL_Docente
UPDATE Familia
SET Nombre = N'ROL_Docente',
    Descripcion = N'Rol de Docente - Gestión de préstamos y consultas'
WHERE IdFamilia = '447BCDED-689F-45D8-8033-0A142C07C0E2';
PRINT 'ROL_Docente actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

-- ROL_Administrador
UPDATE Familia
SET Nombre = N'ROL_Administrador',
    Descripcion = N'Rol de Administrador del Sistema - Acceso completo al sistema'
WHERE IdFamilia = 'C5365ACA-995E-4DA8-9BC6-CCCEC9B31FA2';
PRINT 'ROL_Administrador actualizado: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registro(s)'

PRINT ''
PRINT '================================================'
PRINT 'VERIFICACIÓN FINAL'
PRINT '================================================'
PRINT ''

-- Mostrar resultados
SELECT
    ROW_NUMBER() OVER (ORDER BY Nombre) AS [#],
    Nombre AS [Nombre del Rol],
    Descripcion AS [Descripción],
    FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm') AS [Fecha]
FROM Familia
ORDER BY Nombre;

PRINT ''
PRINT '================================================'
PRINT 'CORRECCIÓN COMPLETADA EXITOSAMENTE'
PRINT '================================================'
PRINT ''
PRINT 'Los acentos ahora deberían verse correctamente'
PRINT 'tanto en SSMS como en la aplicación.'
PRINT ''

GO
