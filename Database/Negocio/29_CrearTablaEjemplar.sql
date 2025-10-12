/*
    Script: 29_CrearTablaEjemplar.sql
    Descripción: Crea la tabla Ejemplar para gestionar copias físicas individuales de materiales
    Fecha: 2025-10-12

    Cada ejemplar representa una copia física de un material con su propio estado
    Por ejemplo: Si tenemos 5 copias de "El Principito", habrá 5 ejemplares
*/

USE NegocioBiblioteca;
GO

PRINT 'Iniciando creación de tabla Ejemplar...';

-- Verificar si la tabla ya existe
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ejemplar')
BEGIN
    PRINT 'Creando tabla Ejemplar...';

    CREATE TABLE Ejemplar (
        IdEjemplar UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        IdMaterial UNIQUEIDENTIFIER NOT NULL,
        NumeroEjemplar INT NOT NULL,
        CodigoBarras NVARCHAR(50) NULL,
        Estado INT NOT NULL CHECK (Estado IN (1, 2, 3, 4)), -- 1=Disponible, 2=Prestado, 3=En reparación, 4=No disponible
        Ubicacion NVARCHAR(100) NULL,
        Observaciones NVARCHAR(500) NULL,
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
        Activo BIT NOT NULL DEFAULT 1,

        -- Foreign Key a Material
        CONSTRAINT FK_Ejemplar_Material FOREIGN KEY (IdMaterial) REFERENCES Material(IdMaterial),

        -- Constraint único: Un material no puede tener dos ejemplares con el mismo número
        CONSTRAINT UK_Ejemplar_MaterialNumero UNIQUE (IdMaterial, NumeroEjemplar)
    );

    PRINT 'Tabla Ejemplar creada exitosamente.';

    -- Crear índices para mejorar performance
    CREATE INDEX IX_Ejemplar_IdMaterial ON Ejemplar(IdMaterial);
    CREATE INDEX IX_Ejemplar_Estado ON Ejemplar(Estado);
    CREATE INDEX IX_Ejemplar_CodigoBarras ON Ejemplar(CodigoBarras);

    PRINT 'Índices creados exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Ejemplar ya existe. No se realizaron cambios.';
END

GO

PRINT 'Script 29_CrearTablaEjemplar.sql completado.';
