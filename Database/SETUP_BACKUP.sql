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
        TamañoMB DECIMAL(10,2) NULL,
        Estado NVARCHAR(20) NOT NULL,
        MensajeError NVARCHAR(MAX) NULL,
        IdUsuarioCreacion INT NULL,
        Descripcion NVARCHAR(500) NULL,
        CONSTRAINT FK_Backup_Usuario FOREIGN KEY (IdUsuarioCreacion)
            REFERENCES Usuario(IdUsuario)
    );
    PRINT 'Tabla Backup creada.';
END
ELSE
BEGIN
    PRINT 'La tabla Backup ya existe.';
END
GO

-- Crear patente Gestion Backup
IF NOT EXISTS (SELECT 1 FROM Patente WHERE NombrePatente = 'Gestión Backup')
BEGIN
    INSERT INTO Patente (NombrePatente, FormName)
    VALUES ('Gestión Backup', 'FrmGestionBackup');
    PRINT 'Patente Gestion Backup creada.';
END
ELSE
BEGIN
    PRINT 'La patente Gestion Backup ya existe.';
END
GO

-- Asignar patente a ROL_Administrador
DECLARE @IdRolAdmin INT;
DECLARE @IdPatenteBackup INT;

SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE NombreFamilia = 'ROL_Administrador';
SELECT @IdPatenteBackup = IdPatente FROM Patente WHERE NombrePatente = 'Gestión Backup';

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
GO

-- Verificar resultado
SELECT 'Patentes del Administrador:' AS Resultado;
SELECT p.NombrePatente, p.FormName
FROM Familia f
JOIN FamiliaPatente fp ON f.IdFamilia = fp.IdFamilia
JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.NombreFamilia = 'ROL_Administrador'
  AND p.NombrePatente LIKE '%Backup%';
GO
