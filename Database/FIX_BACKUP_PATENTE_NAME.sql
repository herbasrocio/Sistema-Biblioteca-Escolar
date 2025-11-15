USE SeguridadBiblioteca;
GO

-- Actualizar el MenuItemName de la patente de Backup para quitar el carácter especial
UPDATE Patente
SET MenuItemName = 'Gestion Backup'
WHERE FormName = 'FrmGestionBackup';

PRINT 'Patente actualizada: MenuItemName cambiado a "Gestion Backup"';
GO

-- Verificar el cambio
SELECT FormName, MenuItemName, Descripcion
FROM Patente
WHERE FormName = 'FrmGestionBackup';
GO
