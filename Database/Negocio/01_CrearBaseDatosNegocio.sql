-- =====================================================
-- Script: Crear Base de Datos de Negocio
-- Descripción: Crea la base de datos NegocioBiblioteca
--              para gestión de materiales, préstamos y alumnos
-- =====================================================

USE master;
GO

-- Eliminar la base de datos si existe
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'NegocioBiblioteca')
BEGIN
    PRINT 'Eliminando base de datos NegocioBiblioteca existente...'
    ALTER DATABASE NegocioBiblioteca SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE NegocioBiblioteca;
    PRINT '  ✓ Base de datos eliminada'
END
GO

-- Crear la base de datos
PRINT 'Creando base de datos NegocioBiblioteca...'
CREATE DATABASE NegocioBiblioteca;
GO

PRINT '  ✓ Base de datos NegocioBiblioteca creada exitosamente'
PRINT ''
PRINT '=== Base de Datos de Negocio Creada ==='
PRINT ''
PRINT 'SIGUIENTE PASO:'
PRINT '  - Ejecutar 02_CrearTablasNegocio.sql para crear las tablas'
PRINT ''
GO
