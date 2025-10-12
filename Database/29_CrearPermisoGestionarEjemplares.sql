/*
    Script: 29_CrearPermisoGestionarEjemplares.sql
    Descripción: Crea el permiso para gestionar ejemplares (copias físicas) de materiales
    Fecha: 2025-10-12
*/

USE SeguridadBiblioteca;
GO

PRINT 'Iniciando creación de permiso GestionarEjemplares...';

-- Declarar variables para IDs
DECLARE @IdPatGestionarEjemplares UNIQUEIDENTIFIER = NEWID();
DECLARE @IdRolAdmin UNIQUEIDENTIFIER;
DECLARE @IdRolBibliotecario UNIQUEIDENTIFIER;

-- Obtener ID del rol Administrador
SELECT @IdRolAdmin = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Administrador';

-- Obtener ID del rol Bibliotecario
SELECT @IdRolBibliotecario = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Bibliotecario';

-- Verificar si la patente ya existe
IF NOT EXISTS (SELECT 1 FROM Patente WHERE MenuItemName = 'GestionarEjemplares')
BEGIN
    PRINT 'Creando patente GestionarEjemplares...';

    -- Insertar la nueva patente
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        @IdPatGestionarEjemplares,
        'menu',
        'GestionarEjemplares',
        4,
        'Acceso a gestión de ejemplares (copias físicas) de materiales'
    );

    PRINT 'Patente GestionarEjemplares creada exitosamente.';

    -- Asignar la patente al rol Administrador
    IF @IdRolAdmin IS NOT NULL
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdRolAdmin, @IdPatGestionarEjemplares);

        PRINT 'Permiso asignado al rol Administrador.';
    END
    ELSE
    BEGIN
        PRINT 'ADVERTENCIA: No se encontró el rol Administrador.';
    END

    -- Asignar la patente al rol Bibliotecario
    IF @IdRolBibliotecario IS NOT NULL
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdRolBibliotecario, @IdPatGestionarEjemplares);

        PRINT 'Permiso asignado al rol Bibliotecario.';
    END
    ELSE
    BEGIN
        PRINT 'ADVERTENCIA: No se encontró el rol Bibliotecario.';
    END
END
ELSE
BEGIN
    PRINT 'La patente GestionarEjemplares ya existe. No se realizaron cambios.';
END

GO

PRINT 'Script 29_CrearPermisoGestionarEjemplares.sql completado.';
