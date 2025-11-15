USE SeguridadBiblioteca;
GO

-- Eliminar tabla si existe
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Backup]') AND type in (N'U'))
BEGIN
    DROP TABLE [Backup];
    PRINT 'Tabla Backup eliminada.';
END
GO

-- Crear tabla con tipos correctos
CREATE TABLE [dbo].[Backup] (
    IdBackup INT IDENTITY(1,1) PRIMARY KEY,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    NombreBaseDatos NVARCHAR(100) NOT NULL,
    RutaArchivo NVARCHAR(500) NOT NULL,
    TipoBackup NVARCHAR(20) NOT NULL,
    TamanoMB DECIMAL(10,2) NULL,
    Estado NVARCHAR(20) NOT NULL,
    MensajeError NVARCHAR(MAX) NULL,
    IdUsuarioCreacion UNIQUEIDENTIFIER NULL,
    Descripcion NVARCHAR(500) NULL,
    CONSTRAINT FK_Backup_Usuario FOREIGN KEY (IdUsuarioCreacion)
        REFERENCES Usuario(IdUsuario)
);
PRINT 'Tabla Backup creada con tipos correctos.';
GO

-- Crear indices
CREATE INDEX IX_Backup_FechaCreacion ON [Backup](FechaCreacion DESC);
CREATE INDEX IX_Backup_NombreBaseDatos ON [Backup](NombreBaseDatos);
PRINT 'Indices creados.';
GO

PRINT 'Tabla Backup lista para usar.';
GO
