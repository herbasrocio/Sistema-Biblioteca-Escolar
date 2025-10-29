-- =============================================
-- Script: 08_AgregarPermisosBitacora.sql
-- Descripción: Agrega permisos para consultar las bitácoras
-- Autor: Sistema Biblioteca Escolar
-- Fecha: 2025-10-28
-- =============================================

USE SeguridadBiblioteca;
GO

-- Patente para Consultar Bitácora de Administrador (solo admin)
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'consultarBitacoraAdmin')
BEGIN
    INSERT INTO Patente (FormName, MenuItemName, Orden, Descripcion)
    VALUES ('consultarBitacoraAdmin', 'Consultar Bitácora Admin', 1, 'Permite consultar la bitácora de administrador');

    PRINT 'Patente "consultarBitacoraAdmin" creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La patente "consultarBitacoraAdmin" ya existe.';
END
GO

-- Patente para Consultar Bitácora de Bibliotecarios (bibliotecarios y admin)
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'consultarBitacoraBibliotecario')
BEGIN
    INSERT INTO Patente (FormName, MenuItemName, Orden, Descripcion)
    VALUES ('consultarBitacoraBibliotecario', 'Consultar Bitácora Bibliotecario', 2, 'Permite consultar la bitácora de bibliotecarios');

    PRINT 'Patente "consultarBitacoraBibliotecario" creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La patente "consultarBitacoraBibliotecario" ya existe.';
END
GO

-- Asignar permiso de BitacoraAdmin a la familia ROL_Administrador
DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER;
DECLARE @IdPatenteAdmin UNIQUEIDENTIFIER;

SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdPatenteAdmin = IdPatente FROM Patente WHERE FormName = 'consultarBitacoraAdmin';

IF @IdFamiliaAdmin IS NOT NULL AND @IdPatenteAdmin IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE IdFamilia = @IdFamiliaAdmin AND IdPatente = @IdPatenteAdmin)
    BEGIN
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdFamiliaAdmin, @IdPatenteAdmin);

        PRINT 'Permiso "consultarBitacoraAdmin" asignado a familia ROL_Administrador.';
    END
    ELSE
    BEGIN
        PRINT 'El permiso "consultarBitacoraAdmin" ya estaba asignado a familia ROL_Administrador.';
    END
END
ELSE
BEGIN
    PRINT 'No se pudo asignar el permiso - Familia ROL_Administrador o Patente no encontrada.';
END
GO

-- Asignar permiso de BitacoraBibliotecario a la familia ROL_Administrador
DECLARE @IdFamiliaAdmin2 UNIQUEIDENTIFIER;
DECLARE @IdPatenteBibliotecario UNIQUEIDENTIFIER;

SELECT @IdFamiliaAdmin2 = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdPatenteBibliotecario = IdPatente FROM Patente WHERE FormName = 'consultarBitacoraBibliotecario';

IF @IdFamiliaAdmin2 IS NOT NULL AND @IdPatenteBibliotecario IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE IdFamilia = @IdFamiliaAdmin2 AND IdPatente = @IdPatenteBibliotecario)
    BEGIN
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdFamiliaAdmin2, @IdPatenteBibliotecario);

        PRINT 'Permiso "consultarBitacoraBibliotecario" asignado a familia ROL_Administrador.';
    END
    ELSE
    BEGIN
        PRINT 'El permiso "consultarBitacoraBibliotecario" ya estaba asignado a familia ROL_Administrador.';
    END
END
GO

-- Asignar permiso de BitacoraBibliotecario a la familia ROL_Bibliotecario (si existe)
DECLARE @IdFamiliaBibliotecario UNIQUEIDENTIFIER;
DECLARE @IdPatenteBibliotecario2 UNIQUEIDENTIFIER;

SELECT @IdFamiliaBibliotecario = IdFamilia FROM Familia WHERE Nombre = 'ROL_Bibliotecario';
SELECT @IdPatenteBibliotecario2 = IdPatente FROM Patente WHERE FormName = 'consultarBitacoraBibliotecario';

IF @IdFamiliaBibliotecario IS NOT NULL AND @IdPatenteBibliotecario2 IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE IdFamilia = @IdFamiliaBibliotecario AND IdPatente = @IdPatenteBibliotecario2)
    BEGIN
        INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
        VALUES (@IdFamiliaBibliotecario, @IdPatenteBibliotecario2);

        PRINT 'Permiso "consultarBitacoraBibliotecario" asignado a familia ROL_Bibliotecario.';
    END
    ELSE
    BEGIN
        PRINT 'El permiso "consultarBitacoraBibliotecario" ya estaba asignado a familia ROL_Bibliotecario.';
    END
END
ELSE
BEGIN
    PRINT 'Familia ROL_Bibliotecario no encontrada - permiso solo asignado a ROL_Administrador.';
END
GO

PRINT 'Script de permisos de bitácora ejecutado exitosamente.';
GO
