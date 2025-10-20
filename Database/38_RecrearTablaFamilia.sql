/*
Script para RECREAR la tabla Familia con acentos correctos
ADVERTENCIA: Este script elimina y recrea la tabla
*/

USE SeguridadBiblioteca;
GO

PRINT '========================================================'
PRINT 'RECREANDO TABLA FAMILIA CON ACENTOS CORRECTOS'
PRINT '========================================================'
PRINT ''

-- ========================================
-- PASO 1: Guardar datos de relaciones
-- ========================================
PRINT '--- Paso 1: Guardando datos de relaciones ---'

-- Guardar UsuarioFamilia
SELECT * INTO #BackupUsuarioFamilia FROM UsuarioFamilia;
PRINT 'UsuarioFamilia: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros guardados'

-- Guardar FamiliaFamilia
SELECT * INTO #BackupFamiliaFamilia FROM FamiliaFamilia;
PRINT 'FamiliaFamilia: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros guardados'

-- Guardar FamiliaPatente
SELECT * INTO #BackupFamiliaPatente FROM FamiliaPatente;
PRINT 'FamiliaPatente: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros guardados'

-- Guardar IDs y fechas de Familia
SELECT IdFamilia, Nombre, FechaCreacion INTO #BackupFamilia FROM Familia;
PRINT 'Familia: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros guardados'
PRINT ''

-- ========================================
-- PASO 2: Eliminar Foreign Keys
-- ========================================
PRINT '--- Paso 2: Eliminando Foreign Keys ---'

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_UsuarioFamilia_Familia')
BEGIN
    ALTER TABLE UsuarioFamilia DROP CONSTRAINT FK_UsuarioFamilia_Familia;
    PRINT 'FK_UsuarioFamilia_Familia eliminada'
END

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_FamiliaFamilia_Padre')
BEGIN
    ALTER TABLE FamiliaFamilia DROP CONSTRAINT FK_FamiliaFamilia_Padre;
    PRINT 'FK_FamiliaFamilia_Padre eliminada'
END

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_FamiliaFamilia_Hijo')
BEGIN
    ALTER TABLE FamiliaFamilia DROP CONSTRAINT FK_FamiliaFamilia_Hijo;
    PRINT 'FK_FamiliaFamilia_Hijo eliminada'
END

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_FamiliaPatente_Familia')
BEGIN
    ALTER TABLE FamiliaPatente DROP CONSTRAINT FK_FamiliaPatente_Familia;
    PRINT 'FK_FamiliaPatente_Familia eliminada'
END
PRINT ''

-- ========================================
-- PASO 3: Eliminar y recrear tabla Familia
-- ========================================
PRINT '--- Paso 3: Recreando tabla Familia ---'

DROP TABLE IF EXISTS Familia;
PRINT 'Tabla Familia eliminada'

CREATE TABLE Familia (
    IdFamilia UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
PRINT 'Tabla Familia recreada'
PRINT ''

-- ========================================
-- PASO 4: Insertar datos con acentos correctos
-- ========================================
PRINT '--- Paso 4: Insertando datos con acentos correctos ---'

-- Usar los IDs y fechas originales para mantener integridad referencial
INSERT INTO Familia (IdFamilia, Nombre, Descripcion, FechaCreacion)
SELECT
    b.IdFamilia,
    CASE
        WHEN b.Nombre LIKE 'ROL_Docente%' THEN N'ROL_Docente'
        WHEN b.Nombre LIKE 'ROL_Bibliotecario%' THEN N'ROL_Bibliotecario'
        WHEN b.Nombre LIKE '%Usuarios%' THEN N'Gestión de Usuarios'
        WHEN b.Nombre LIKE 'ROL_Administrador%' THEN N'ROL_Administrador'
        WHEN b.Nombre LIKE 'Configuraci%' THEN N'Configuración'
        WHEN b.Nombre LIKE '%Permisos%' THEN N'Gestión de Permisos'
        ELSE b.Nombre
    END,
    CASE
        WHEN b.Nombre LIKE 'ROL_Docente%' THEN N'Rol de Docente - Gestión de préstamos y consultas'
        WHEN b.Nombre LIKE 'ROL_Bibliotecario%' THEN N'Rol de Bibliotecario - Gestión de catálogo, préstamos y devoluciones'
        WHEN b.Nombre LIKE '%Usuarios%' THEN N'Administración de usuarios del sistema'
        WHEN b.Nombre LIKE 'ROL_Administrador%' THEN N'Rol de Administrador del Sistema - Acceso completo al sistema'
        WHEN b.Nombre LIKE 'Configuraci%' THEN N'Configuración del sistema'
        WHEN b.Nombre LIKE '%Permisos%' THEN N'Administración de familias y patentes'
        ELSE N''
    END,
    b.FechaCreacion
FROM #BackupFamilia b;

PRINT 'Registros insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
PRINT ''

-- ========================================
-- PASO 5: Recrear Foreign Keys
-- ========================================
PRINT '--- Paso 5: Recreando Foreign Keys ---'

ALTER TABLE UsuarioFamilia
ADD CONSTRAINT FK_UsuarioFamilia_Familia
FOREIGN KEY (IdFamilia) REFERENCES Familia(IdFamilia);
PRINT 'FK_UsuarioFamilia_Familia recreada'

ALTER TABLE FamiliaFamilia
ADD CONSTRAINT FK_FamiliaFamilia_Padre
FOREIGN KEY (IdFamiliaPadre) REFERENCES Familia(IdFamilia);
PRINT 'FK_FamiliaFamilia_Padre recreada'

ALTER TABLE FamiliaFamilia
ADD CONSTRAINT FK_FamiliaFamilia_Hijo
FOREIGN KEY (IdFamiliaHijo) REFERENCES Familia(IdFamilia);
PRINT 'FK_FamiliaFamilia_Hijo recreada'

ALTER TABLE FamiliaPatente
ADD CONSTRAINT FK_FamiliaPatente_Familia
FOREIGN KEY (IdFamilia) REFERENCES Familia(IdFamilia);
PRINT 'FK_FamiliaPatente_Familia recreada'
PRINT ''

-- ========================================
-- PASO 6: Restaurar datos de relaciones
-- ========================================
PRINT '--- Paso 6: Restaurando datos de relaciones ---'

-- Limpiar y reinsertar UsuarioFamilia
DELETE FROM UsuarioFamilia;
INSERT INTO UsuarioFamilia SELECT * FROM #BackupUsuarioFamilia;
PRINT 'UsuarioFamilia: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros restaurados'

-- Limpiar y reinsertar FamiliaFamilia
DELETE FROM FamiliaFamilia;
INSERT INTO FamiliaFamilia SELECT * FROM #BackupFamiliaFamilia;
PRINT 'FamiliaFamilia: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros restaurados'

-- Limpiar y reinsertar FamiliaPatente
DELETE FROM FamiliaPatente;
INSERT INTO FamiliaPatente SELECT * FROM #BackupFamiliaPatente;
PRINT 'FamiliaPatente: ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' registros restaurados'
PRINT ''

-- ========================================
-- PASO 7: Limpiar tablas temporales
-- ========================================
DROP TABLE #BackupUsuarioFamilia;
DROP TABLE #BackupFamiliaFamilia;
DROP TABLE #BackupFamiliaPatente;
DROP TABLE #BackupFamilia;
PRINT '--- Paso 7: Tablas temporales eliminadas ---'
PRINT ''

-- ========================================
-- VERIFICACIÓN FINAL
-- ========================================
PRINT '========================================================'
PRINT 'VERIFICACIÓN FINAL'
PRINT '========================================================'
PRINT ''

SELECT
    ROW_NUMBER() OVER (ORDER BY Nombre) AS [#],
    Nombre AS [Nombre del Rol],
    Descripcion AS [Descripción],
    FORMAT(FechaCreacion, 'dd/MM/yyyy HH:mm') AS [Fecha]
FROM Familia
ORDER BY Nombre;

PRINT ''
PRINT '========================================================'
PRINT 'RECREACIÓN COMPLETADA EXITOSAMENTE'
PRINT '========================================================'
PRINT ''
PRINT 'Ahora deberías ver los acentos correctamente en SSMS.'
PRINT 'Si los acentos se ven bien, la corrección fue exitosa.'
PRINT ''

GO
