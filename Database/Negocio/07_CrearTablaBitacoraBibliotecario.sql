-- =============================================
-- Script: 07_CrearTablaBitacoraBibliotecario.sql
-- Descripción: Crea la tabla BitacoraBibliotecario en la base de datos NegocioBiblioteca
-- Autor: Sistema Biblioteca Escolar
-- Fecha: 2025-10-28
-- =============================================

USE NegocioBiblioteca;
GO

-- Verificar si la tabla ya existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BitacoraBibliotecario]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[BitacoraBibliotecario]
    (
        [IdBitacora] INT IDENTITY(1,1) NOT NULL,
        [Fecha] DATETIME NOT NULL DEFAULT GETDATE(),
        [IdUsuario] UNIQUEIDENTIFIER NULL, -- Referencia externa a SeguridadBiblioteca.Usuario
        [NombreUsuario] NVARCHAR(100) NULL,
        [TipoOperacion] NVARCHAR(50) NOT NULL, -- 'Prestamo', 'Devolucion', 'Renovacion', 'ConsultaMaterial', 'GestionAlumno', etc.
        [Modulo] NVARCHAR(100) NOT NULL, -- Nombre del módulo/formulario
        [Accion] NVARCHAR(255) NOT NULL, -- Descripción de la acción
        [EntidadAfectada] NVARCHAR(50) NULL, -- 'Material', 'Ejemplar', 'Alumno', 'Prestamo', etc.
        [IdEntidad] INT NULL, -- ID del registro afectado
        [Detalle] NVARCHAR(MAX) NULL, -- Información adicional (JSON con datos relevantes)

        CONSTRAINT [PK_BitacoraBibliotecario] PRIMARY KEY CLUSTERED ([IdBitacora] ASC)
    );

    -- Índices para mejorar consultas
    CREATE NONCLUSTERED INDEX [IX_BitacoraBibliotecario_Fecha]
        ON [dbo].[BitacoraBibliotecario]([Fecha] DESC);

    CREATE NONCLUSTERED INDEX [IX_BitacoraBibliotecario_TipoOperacion]
        ON [dbo].[BitacoraBibliotecario]([TipoOperacion]);

    CREATE NONCLUSTERED INDEX [IX_BitacoraBibliotecario_IdUsuario]
        ON [dbo].[BitacoraBibliotecario]([IdUsuario]);

    CREATE NONCLUSTERED INDEX [IX_BitacoraBibliotecario_EntidadAfectada]
        ON [dbo].[BitacoraBibliotecario]([EntidadAfectada], [IdEntidad]);

    PRINT 'Tabla BitacoraBibliotecario creada exitosamente en NegocioBiblioteca.';
END
ELSE
BEGIN
    PRINT 'La tabla BitacoraBibliotecario ya existe en NegocioBiblioteca.';
END
GO

-- Stored Procedure para registrar operaciones de bibliotecarios
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RegistrarBitacoraBibliotecario]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_RegistrarBitacoraBibliotecario];
GO

CREATE PROCEDURE [dbo].[sp_RegistrarBitacoraBibliotecario]
    @IdUsuario UNIQUEIDENTIFIER = NULL,
    @NombreUsuario NVARCHAR(100) = NULL,
    @TipoOperacion NVARCHAR(50),
    @Modulo NVARCHAR(100),
    @Accion NVARCHAR(255),
    @EntidadAfectada NVARCHAR(50) = NULL,
    @IdEntidad INT = NULL,
    @Detalle NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[BitacoraBibliotecario]
        ([IdUsuario], [NombreUsuario], [TipoOperacion], [Modulo], [Accion], [EntidadAfectada], [IdEntidad], [Detalle])
    VALUES
        (@IdUsuario, @NombreUsuario, @TipoOperacion, @Modulo, @Accion, @EntidadAfectada, @IdEntidad, @Detalle);

    RETURN SCOPE_IDENTITY();
END
GO

PRINT 'Stored procedure sp_RegistrarBitacoraBibliotecario creado exitosamente.';
GO
