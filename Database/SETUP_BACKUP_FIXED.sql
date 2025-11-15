USE SeguridadBiblioteca;
GO

-- Crear tabla Backup si no existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Backup]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Backup] (
        IdBackup INT IDENTITY(1,1) PRIMARY KEY,
        FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
        NombreBaseDatos NVARCHAR(100) NOT NULL,
        RutaArchivo NVARCHAR(500) NOT NULL,
        TipoBackup NVARCHAR(20) NOT NULL,
        TamanoMB DECIMAL(10,2) NULL,
        Estado NVARCHAR(20) NOT NULL,
        MensajeError NVARCHAR(MAX) NULL,
        IdUsuarioCreacion INT NULL,
        Descripcion NVARCHAR(500) NULL
    );
    PRINT 'Tabla Backup creada.';
END
ELSE
BEGIN
    PRINT 'La tabla Backup ya existe.';
END
GO

-- Crear patente Gestion Backup
DECLARE @IdPatenteBackup UNIQUEIDENTIFIER = NEWID();

IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'FrmGestionBackup')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteBackup, 'FrmGestionBackup', 'Gestión Backup', 100, 'Permite crear y restaurar backups de bases de datos');
    PRINT 'Patente Gestion Backup creada.';
END
ELSE
BEGIN
    SELECT @IdPatenteBackup = IdPatente FROM Patente WHERE FormName = 'FrmGestionBackup';
    PRINT 'La patente Gestion Backup ya existe.';
END
GO

-- Asignar patente a ROL_Administrador
DECLARE @IdRolAdmin UNIQUEIDENTIFIER;
DECLARE @IdPatenteBackup UNIQUEIDENTIFIER;

SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdPatenteBackup = IdPatente FROM Patente WHERE FormName = 'FrmGestionBackup';

IF @IdRolAdmin IS NOT NULL AND @IdPatenteBackup IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE IdFamilia = @IdRolAdmin AND IdPatente = @IdPatenteBackup)
    BEGIN
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdRolAdmin, @IdPatenteBackup);
        PRINT 'Patente asignada a ROL_Administrador.';
    END
    ELSE
    BEGIN
        PRINT 'La patente ya esta asignada a ROL_Administrador.';
    END
END
ELSE
BEGIN
    PRINT 'ERROR: No se encontro ROL_Administrador o la patente.';
END
GO

-- Verificar resultado
SELECT 'Verificacion: Patentes del Administrador que incluyen Backup' AS Info;
SELECT p.FormName, p.MenuItemName, p.Descripcion
FROM Familia f
JOIN FamiliaPatente fp ON f.IdFamilia = fp.IdFamilia
JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.Nombre = 'ROL_Administrador'
  AND p.FormName LIKE '%Backup%';
GO
