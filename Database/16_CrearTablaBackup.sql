-- =============================================
-- Script: 16_CrearTablaBackup.sql
-- Descripción: Crea la tabla Backup para gestionar el catálogo de copias de seguridad
-- Fecha: 2025-11-15
-- =============================================

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
        TipoBackup NVARCHAR(20) NOT NULL, -- 'Full', 'Differential', 'Transaction Log'
        TamañoMB DECIMAL(10,2) NULL,
        Estado NVARCHAR(20) NOT NULL, -- 'Exitoso', 'Fallido', 'En Proceso'
        MensajeError NVARCHAR(MAX) NULL,
        IdUsuarioCreacion INT NULL,
        Descripcion NVARCHAR(500) NULL,
        CONSTRAINT FK_Backup_Usuario FOREIGN KEY (IdUsuarioCreacion)
            REFERENCES Usuario(IdUsuario)
    );

    PRINT 'Tabla Backup creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Backup ya existe.';
END
GO

-- Crear índices para mejorar consultas
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Backup_FechaCreacion' AND object_id = OBJECT_ID('Backup'))
BEGIN
    CREATE INDEX IX_Backup_FechaCreacion ON Backup(FechaCreacion DESC);
    PRINT 'Índice IX_Backup_FechaCreacion creado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Backup_NombreBaseDatos' AND object_id = OBJECT_ID('Backup'))
BEGIN
    CREATE INDEX IX_Backup_NombreBaseDatos ON Backup(NombreBaseDatos);
    PRINT 'Índice IX_Backup_NombreBaseDatos creado.';
END
GO

PRINT 'Script 16_CrearTablaBackup.sql ejecutado correctamente.';
GO
