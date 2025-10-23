-- =====================================================
-- Script: Agregar Patente para Renovación de Préstamos
-- Descripción: Crea la patente y la asigna a roles
-- Fecha: 2025-10-22
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT '========================================='
PRINT 'Agregando patente renovarPrestamo'
PRINT '========================================='
PRINT ''

-- 1. Verificar si la patente ya existe
IF NOT EXISTS (SELECT * FROM Patente WHERE MenuItemName = 'Renovar Préstamo')
BEGIN
    -- Insertar la patente
    DECLARE @IdPatente UNIQUEIDENTIFIER = NEWID();

    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Descripcion)
    VALUES (
        @IdPatente,
        'menu',
        'Renovar Préstamo',
        'Permite renovar préstamos activos extendiendo la fecha de devolución'
    );

    PRINT '✓ Patente renovarPrestamo creada exitosamente'

    -- 2. Asignar patente a la familia "Gestión Préstamos" (si existe)
    IF EXISTS (SELECT * FROM Familia WHERE Nombre = 'Gestión Préstamos')
    BEGIN
        DECLARE @IdFamiliaGestionPrestamos UNIQUEIDENTIFIER;
        SELECT @IdFamiliaGestionPrestamos = IdFamilia FROM Familia WHERE Nombre = 'Gestión Préstamos';

        IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE IdFamilia = @IdFamiliaGestionPrestamos AND IdPatente = @IdPatente)
        BEGIN
            INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
            VALUES (@IdFamiliaGestionPrestamos, @IdPatente);

            PRINT '✓ Patente asignada a familia "Gestión Préstamos"'
        END
    END
    ELSE
    BEGIN
        PRINT '  → Familia "Gestión Préstamos" no encontrada (se omitirá asignación)'
    END

    -- 3. Asignar patente al ROL_Administrador (para que tenga acceso inmediato)
    IF EXISTS (SELECT * FROM Familia WHERE Nombre = 'ROL_Administrador')
    BEGIN
        DECLARE @IdRolAdmin UNIQUEIDENTIFIER;
        SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';

        IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE IdFamilia = @IdRolAdmin AND IdPatente = @IdPatente)
        BEGIN
            INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
            VALUES (@IdRolAdmin, @IdPatente);

            PRINT '✓ Patente asignada al rol Administrador'
        END
    END

    -- 4. Asignar patente al ROL_Bibliotecario (si existe)
    IF EXISTS (SELECT * FROM Familia WHERE Nombre = 'ROL_Bibliotecario')
    BEGIN
        DECLARE @IdRolBibliotecario UNIQUEIDENTIFIER;
        SELECT @IdRolBibliotecario = IdFamilia FROM Familia WHERE Nombre = 'ROL_Bibliotecario';

        IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE IdFamilia = @IdRolBibliotecario AND IdPatente = @IdPatente)
        BEGIN
            INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
            VALUES (@IdRolBibliotecario, @IdPatente);

            PRINT '✓ Patente asignada al rol Bibliotecario'
        END
    END
END
ELSE
BEGIN
    PRINT '  → Patente "Renovar Préstamo" ya existe'

    -- Asegurarse de que el FormName sea correcto
    UPDATE Patente SET FormName = 'menu' WHERE MenuItemName = 'Renovar Préstamo' AND FormName != 'menu';

    IF @@ROWCOUNT > 0
        PRINT '  → FormName corregido a "menu"'
END

PRINT ''
PRINT '========================================='
PRINT 'Patente configurada exitosamente'
PRINT '========================================='
GO
