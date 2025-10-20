/*
Script para corregir acentos y caracteres especiales en la tabla Familia
Problema: Los textos con acentos se guardan mal codificados
Solución: Actualizar directamente con los valores correctos en UTF-8
*/

USE SeguridadBiblioteca;
GO

PRINT '================================================'
PRINT 'CORRIGIENDO ACENTOS EN TABLA FAMILIA'
PRINT '================================================'
PRINT ''

-- Mostrar estado actual
PRINT '--- ESTADO ACTUAL ---'
SELECT
    IdFamilia,
    Nombre,
    Descripcion,
    FechaCreacion
FROM Familia
ORDER BY FechaCreacion;
PRINT ''

-- Corregir ROL_Docente
UPDATE Familia
SET
    Nombre = N'ROL_Docente',
    Descripcion = N'Rol de Docente - Gestión de préstamos y consultas'
WHERE IdFamilia = '447BCD5D-689F-45D8-8033-0A142C07C0E2';

-- Corregir ROL_Bibliotecario
UPDATE Familia
SET
    Nombre = N'ROL_Bibliotecario',
    Descripcion = N'Rol de Bibliotecario - Gestión de catálogo, préstamos y devoluciones'
WHERE IdFamilia = 'D0838EA8-9A37-4353-85A7-284C8768E0C4';

-- Corregir Gestión de Usuarios
UPDATE Familia
SET
    Nombre = N'Gestión de Usuarios',
    Descripcion = N'Administración de usuarios del sistema'
WHERE IdFamilia = '1BD8B5E6-955E-4D86-AEB7-08BD03CD4FC2';

-- Corregir ROL_Administrador
UPDATE Familia
SET
    Nombre = N'ROL_Administrador',
    Descripcion = N'Rol de Administrador del Sistema - Acceso completo al sistema'
WHERE IdFamilia = 'C536AC7A-095E-4DA8-98CC-CCCEC9B31FA2';

-- Corregir Configuración
UPDATE Familia
SET
    Nombre = N'Configuración',
    Descripcion = N'Configuración del sistema'
WHERE IdFamilia = '9FD5F833-E2BB-4374-B092-D7649630622C';

-- Corregir Gestión de Permisos
UPDATE Familia
SET
    Nombre = N'Gestión de Permisos',
    Descripcion = N'Administración de familias y patentes'
WHERE IdFamilia = 'B898B23E-96A6-4D22-AA7F-F32BA5B44D75';

PRINT '--- ACTUALIZACIONES COMPLETADAS ---'
PRINT 'Registros actualizados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
PRINT ''

-- Mostrar estado final
PRINT '--- ESTADO FINAL (CORREGIDO) ---'
SELECT
    IdFamilia,
    Nombre,
    Descripcion,
    FechaCreacion
FROM Familia
ORDER BY FechaCreacion;
PRINT ''

PRINT '================================================'
PRINT 'CORRECCIÓN DE ACENTOS COMPLETADA EXITOSAMENTE'
PRINT '================================================'
PRINT ''
PRINT 'NOTA: Si los acentos siguen apareciendo mal en alguna'
PRINT 'herramienta de consulta, verifica la configuración de'
PRINT 'codificación de la herramienta (debe ser UTF-8).'
PRINT ''

GO
