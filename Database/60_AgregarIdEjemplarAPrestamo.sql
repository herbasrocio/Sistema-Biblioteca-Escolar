-- =============================================
-- Script: Agregar columna IdEjemplar a tabla Prestamo
-- Descripción: Modifica la tabla Prestamo para rastrear qué ejemplar específico se prestó
-- Fecha: 2025-10-17
-- =============================================

USE NegocioBiblioteca
GO

-- Verificar si la columna ya existe
IF NOT EXISTS (SELECT * FROM sys.columns
               WHERE object_id = OBJECT_ID(N'dbo.Prestamo')
               AND name = 'IdEjemplar')
BEGIN
    PRINT 'Agregando columna IdEjemplar a tabla Prestamo...'

    -- Agregar columna IdEjemplar (permitir NULL temporalmente)
    ALTER TABLE dbo.Prestamo
    ADD IdEjemplar UNIQUEIDENTIFIER NULL

    PRINT 'Columna IdEjemplar agregada exitosamente.'

    -- Crear relación con tabla Ejemplar
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys
                   WHERE name = 'FK_Prestamo_Ejemplar')
    BEGIN
        ALTER TABLE dbo.Prestamo
        ADD CONSTRAINT FK_Prestamo_Ejemplar
        FOREIGN KEY (IdEjemplar) REFERENCES dbo.Ejemplar(IdEjemplar)

        PRINT 'Relación FK_Prestamo_Ejemplar creada exitosamente.'
    END

    -- Crear índice para mejorar el rendimiento de consultas
    IF NOT EXISTS (SELECT * FROM sys.indexes
                   WHERE name = 'IX_Prestamo_IdEjemplar'
                   AND object_id = OBJECT_ID(N'dbo.Prestamo'))
    BEGIN
        CREATE NONCLUSTERED INDEX IX_Prestamo_IdEjemplar
        ON dbo.Prestamo (IdEjemplar)

        PRINT 'Índice IX_Prestamo_IdEjemplar creado exitosamente.'
    END

    PRINT '============================================='
    PRINT 'Migración completada exitosamente.'
    PRINT 'NOTA: Los préstamos existentes tienen IdEjemplar = NULL'
    PRINT 'Los nuevos préstamos requerirán un IdEjemplar válido.'
    PRINT '============================================='
END
ELSE
BEGIN
    PRINT 'La columna IdEjemplar ya existe en la tabla Prestamo.'
END
GO
