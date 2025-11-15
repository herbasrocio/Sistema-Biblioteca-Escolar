-- =============================================
-- Script: 17_AgregarPatentesBackup.sql
-- Descripción: Agrega las patentes necesarias para gestión de Backup y Restore
-- Fecha: 2025-11-15
-- =============================================

USE SeguridadBiblioteca;
GO

-- Insertar patente para Gestión de Backup
IF NOT EXISTS (SELECT 1 FROM Patente WHERE NombrePatente = 'Gestión Backup')
BEGIN
    INSERT INTO Patente (NombrePatente, FormName)
    VALUES ('Gestión Backup', 'FrmGestionBackup');
    PRINT 'Patente "Gestión Backup" creada.';
END
ELSE
BEGIN
    PRINT 'La patente "Gestión Backup" ya existe.';
END
GO

-- Insertar patente para Consultar Historial de Backups
IF NOT EXISTS (SELECT 1 FROM Patente WHERE NombrePatente = 'Consultar Backups')
BEGIN
    INSERT INTO Patente (NombrePatente, FormName)
    VALUES ('Consultar Backups', 'FrmConsultarBackups');
    PRINT 'Patente "Consultar Backups" creada.';
END
ELSE
BEGIN
    PRINT 'La patente "Consultar Backups" ya existe.';
END
GO

-- Asignar patentes al rol Administrador
DECLARE @IdRolAdmin INT;
DECLARE @IdPatenteGestionBackup INT;
DECLARE @IdPatenteConsultarBackups INT;

-- Obtener IDs
SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE NombreFamilia = 'ROL_Administrador';
SELECT @IdPatenteGestionBackup = IdPatente FROM Patente WHERE NombrePatente = 'Gestión Backup';
SELECT @IdPatenteConsultarBackups = IdPatente FROM Patente WHERE NombrePatente = 'Consultar Backups';

-- Asignar Gestión Backup a Administrador
IF NOT EXISTS (SELECT 1 FROM FamiliaPatente
               WHERE IdFamilia = @IdRolAdmin AND IdPatente = @IdPatenteGestionBackup)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    VALUES (@IdRolAdmin, @IdPatenteGestionBackup);
    PRINT 'Patente "Gestión Backup" asignada a ROL_Administrador.';
END
ELSE
BEGIN
    PRINT 'La patente "Gestión Backup" ya está asignada a ROL_Administrador.';
END
GO

-- Asignar Consultar Backups a Administrador
IF NOT EXISTS (SELECT 1 FROM FamiliaPatente
               WHERE IdFamilia = @IdRolAdmin AND IdPatente = @IdPatenteConsultarBackups)
BEGIN
    INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
    VALUES (@IdRolAdmin, @IdPatenteConsultarBackups);
    PRINT 'Patente "Consultar Backups" asignada a ROL_Administrador.';
END
ELSE
BEGIN
    PRINT 'La patente "Consultar Backups" ya está asignada a ROL_Administrador.';
END
GO

PRINT 'Script 17_AgregarPatentesBackup.sql ejecutado correctamente.';
GO
