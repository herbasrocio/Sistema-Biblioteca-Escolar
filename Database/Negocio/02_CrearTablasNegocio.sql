-- =====================================================
-- Script: Crear Tablas de Negocio
-- Descripción: Crea las tablas para gestión de materiales,
--              alumnos, préstamos y devoluciones
-- =====================================================

USE NegocioBiblioteca;
GO

PRINT 'Creando tablas de negocio...'
PRINT ''

-- =====================================================
-- TABLA: Material
-- Descripción: Catálogo de materiales de la biblioteca
-- =====================================================
PRINT 'Creando tabla Material...'

CREATE TABLE Material (
    IdMaterial UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Titulo NVARCHAR(200) NOT NULL,
    Autor NVARCHAR(200) NOT NULL,
    Editorial NVARCHAR(200) NULL,
    Tipo NVARCHAR(50) NOT NULL CHECK (Tipo IN ('Libro', 'Revista', 'Manual')),
    Genero NVARCHAR(50) NOT NULL CHECK (Genero IN ('Fantasia', 'Terror', 'Comedia', 'Historico', 'Teatral', 'Novela', 'Cronica', 'Romance', 'Policial', 'Drama')),
    CantidadTotal INT NOT NULL DEFAULT 0 CHECK (CantidadTotal >= 0),
    CantidadDisponible INT NOT NULL DEFAULT 0 CHECK (CantidadDisponible >= 0),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,

    -- Constraint para asegurar que disponibles <= total
    CONSTRAINT CK_Material_Disponibilidad CHECK (CantidadDisponible <= CantidadTotal)
);

PRINT '  ✓ Tabla Material creada'

-- =====================================================
-- TABLA: Alumno
-- Descripción: Registro de alumnos de la escuela
-- =====================================================
PRINT 'Creando tabla Alumno...'

CREATE TABLE Alumno (
    IdAlumno UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DNI NVARCHAR(20) NOT NULL UNIQUE,
    Grado NVARCHAR(50) NULL,
    Division NVARCHAR(10) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);

PRINT '  ✓ Tabla Alumno creada'

-- =====================================================
-- TABLA: Prestamo
-- Descripción: Registro de préstamos de materiales
-- =====================================================
PRINT 'Creando tabla Prestamo...'

CREATE TABLE Prestamo (
    IdPrestamo UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdMaterial UNIQUEIDENTIFIER NOT NULL,
    IdAlumno UNIQUEIDENTIFIER NOT NULL,
    IdUsuario UNIQUEIDENTIFIER NOT NULL, -- Usuario que registra el préstamo (de SeguridadBiblioteca)
    FechaPrestamo DATETIME NOT NULL DEFAULT GETDATE(),
    FechaDevolucionPrevista DATETIME NOT NULL,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Activo' CHECK (Estado IN ('Activo', 'Devuelto', 'Atrasado')),

    CONSTRAINT FK_Prestamo_Material FOREIGN KEY (IdMaterial) REFERENCES Material(IdMaterial),
    CONSTRAINT FK_Prestamo_Alumno FOREIGN KEY (IdAlumno) REFERENCES Alumno(IdAlumno)
);

PRINT '  ✓ Tabla Prestamo creada'

-- =====================================================
-- TABLA: Devolucion
-- Descripción: Registro de devoluciones de materiales
-- =====================================================
PRINT 'Creando tabla Devolucion...'

CREATE TABLE Devolucion (
    IdDevolucion UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdPrestamo UNIQUEIDENTIFIER NOT NULL,
    FechaDevolucion DATETIME NOT NULL DEFAULT GETDATE(),
    IdUsuario UNIQUEIDENTIFIER NOT NULL, -- Usuario que registra la devolución
    Observaciones NVARCHAR(500) NULL,

    CONSTRAINT FK_Devolucion_Prestamo FOREIGN KEY (IdPrestamo) REFERENCES Prestamo(IdPrestamo)
);

PRINT '  ✓ Tabla Devolucion creada'

PRINT ''
PRINT '=== Tablas de Negocio Creadas Exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • Material: Catálogo con título, autor, editorial, tipo, género'
PRINT '  • Alumno: Datos de alumnos con DNI único'
PRINT '  • Prestamo: Registro de préstamos vinculado a Material y Alumno'
PRINT '  • Devolucion: Registro de devoluciones vinculado a Prestamo'
PRINT ''
PRINT 'TIPOS DE MATERIAL:'
PRINT '  - Libro, Revista, Manual'
PRINT ''
PRINT 'GÉNEROS:'
PRINT '  - Fantasia, Terror, Comedia, Historico, Teatral'
PRINT '  - Novela, Cronica, Romance, Policial, Drama'
PRINT ''
PRINT 'SIGUIENTE PASO:'
PRINT '  - Ejecutar 03_DatosInicialesNegocio.sql para cargar datos de ejemplo'
PRINT ''
GO
