-- =============================================
-- Script: 07_CrearTablaBitacoraAdmin.sql
-- Descripción: Crea la tabla BitacoraAdmin en la base de datos SeguridadBiblioteca
-- Autor: Sistema Biblioteca Escolar
-- Fecha: 2025-10-28
-- =============================================

USE SeguridadBiblioteca;
GO

-- Verificar si la tabla ya existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BitacoraAdmin]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[BitacoraAdmin]
    (
        [IdBitacora] INT IDENTITY(1,1) NOT NULL,
        [Fecha] DATETIME NOT NULL DEFAULT GETDATE(),
        [IdUsuario] UNIQUEIDENTIFIER NULL,
        [NombreUsuario] NVARCHAR(100) NULL,
        [TipoEvento] NVARCHAR(50) NOT NULL, -- 'Error', 'Seguridad', 'CambioCritico'
        [Modulo] NVARCHAR(100) NOT NULL, -- Nombre del módulo/formulario
        [Accion] NVARCHAR(255) NOT NULL, -- Descripción breve de la acción
        [Detalle] NVARCHAR(MAX) NULL, -- Información detallada (stack trace, datos anteriores, etc.)
        [Gravedad] NVARCHAR(20) NOT NULL DEFAULT 'Medio', -- 'Bajo', 'Medio', 'Alto'
        [DireccionIP] NVARCHAR(45) NULL, -- IPv4 o IPv6

        CONSTRAINT [PK_BitacoraAdmin] PRIMARY KEY CLUSTERED ([IdBitacora] ASC),
        CONSTRAINT [FK_BitacoraAdmin_Usuario] FOREIGN KEY([IdUsuario])
            REFERENCES [dbo].[Usuario] ([IdUsuario]) ON DELETE SET NULL
    );

    -- Índices para mejorar consultas
    CREATE NONCLUSTERED INDEX [IX_BitacoraAdmin_Fecha]
        ON [dbo].[BitacoraAdmin]([Fecha] DESC);

    CREATE NONCLUSTERED INDEX [IX_BitacoraAdmin_TipoEvento]
        ON [dbo].[BitacoraAdmin]([TipoEvento]);

    CREATE NONCLUSTERED INDEX [IX_BitacoraAdmin_IdUsuario]
        ON [dbo].[BitacoraAdmin]([IdUsuario]);

    PRINT 'Tabla BitacoraAdmin creada exitosamente en SeguridadBiblioteca.';
END
ELSE
BEGIN
    PRINT 'La tabla BitacoraAdmin ya existe en SeguridadBiblioteca.';
END
GO

-- Stored Procedure para registrar eventos de administrador
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RegistrarBitacoraAdmin]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_RegistrarBitacoraAdmin];
GO

CREATE PROCEDURE [dbo].[sp_RegistrarBitacoraAdmin]
    @IdUsuario UNIQUEIDENTIFIER = NULL,
    @NombreUsuario NVARCHAR(100) = NULL,
    @TipoEvento NVARCHAR(50),
    @Modulo NVARCHAR(100),
    @Accion NVARCHAR(255),
    @Detalle NVARCHAR(MAX) = NULL,
    @Gravedad NVARCHAR(20) = 'Medio',
    @DireccionIP NVARCHAR(45) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[BitacoraAdmin]
        ([IdUsuario], [NombreUsuario], [TipoEvento], [Modulo], [Accion], [Detalle], [Gravedad], [DireccionIP])
    VALUES
        (@IdUsuario, @NombreUsuario, @TipoEvento, @Modulo, @Accion, @Detalle, @Gravedad, @DireccionIP);

    RETURN SCOPE_IDENTITY();
END
GO

PRINT 'Stored procedure sp_RegistrarBitacoraAdmin creado exitosamente.';
GO
