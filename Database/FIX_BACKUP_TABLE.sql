USE SeguridadBiblioteca;
GO

-- Eliminar FK si existe
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Backup_Usuario')
BEGIN
    ALTER TABLE [Backup] DROP CONSTRAINT FK_Backup_Usuario;
    PRINT 'Foreign key eliminada.';
END
GO

-- Eliminar todos los registros de la tabla Backup (ya que vamos a cambiar el tipo)
DELETE FROM [Backup];
PRINT 'Registros eliminados.';
GO

-- Cambiar tipo de columna
ALTER TABLE [Backup] ALTER COLUMN IdUsuarioCreacion UNIQUEIDENTIFIER NULL;
PRINT 'Columna IdUsuarioCreacion cambiada a UNIQUEIDENTIFIER.';
GO

-- Recrear FK
ALTER TABLE [Backup]
ADD CONSTRAINT FK_Backup_Usuario
FOREIGN KEY (IdUsuarioCreacion) REFERENCES Usuario(IdUsuario);
PRINT 'Foreign key recreada.';
GO

PRINT 'Tabla Backup corregida exitosamente.';
GO
