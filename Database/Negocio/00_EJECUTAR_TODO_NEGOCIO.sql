-- =====================================================
-- Script Maestro: Setup Completo de Base de Datos de Negocio
-- Descripción: Ejecuta todos los scripts necesarios para crear
--              la base de datos NegocioBiblioteca desde cero
-- =====================================================
--
-- IMPORTANTE: Este script debe ejecutarse desde la carpeta Database/Negocio/
--
-- USO:
--   Opción 1 - Desde SQL Server Management Studio:
--     1. Abrir este archivo en SSMS
--     2. Asegurarse de estar en modo SQLCMD (Query > SQLCMD Mode)
--     3. Ejecutar (F5)
--
--   Opción 2 - Desde línea de comandos:
--     cd "Database\Negocio"
--     sqlcmd -S localhost -E -i 00_EJECUTAR_TODO_NEGOCIO.sql
--
-- =====================================================

PRINT ''
PRINT '=========================================='
PRINT '  SETUP COMPLETO - BASE DE DATOS NEGOCIO'
PRINT '=========================================='
PRINT ''
PRINT 'Este script ejecutará:'
PRINT '  1. Crear base de datos NegocioBiblioteca'
PRINT '  2. Crear tablas (Material, Alumno, Prestamo, Devolucion)'
PRINT '  3. Cargar datos iniciales (16 materiales, 10 alumnos)'
PRINT '  4. Crear tablas de Inscripción (opcional si ya existen)'
PRINT '  5. Migrar datos a inscripciones'
PRINT '  6. Crear stored procedures'
PRINT ''
PRINT 'ADVERTENCIA: La base de datos NegocioBiblioteca será eliminada y recreada.'
PRINT ''
PRINT 'Presiona Ctrl+C para cancelar o espera 5 segundos...'
WAITFOR DELAY '00:00:05'
PRINT ''

-- =====================================================
-- PASO 1: Crear Base de Datos
-- =====================================================
PRINT ''
PRINT '=== PASO 1/6: Crear Base de Datos ==='
PRINT ''

:r 01_CrearBaseDatosNegocio.sql

-- =====================================================
-- PASO 2: Crear Tablas de Negocio
-- =====================================================
PRINT ''
PRINT '=== PASO 2/6: Crear Tablas de Negocio ==='
PRINT ''

:r 02_CrearTablasNegocio.sql

-- =====================================================
-- PASO 3: Cargar Datos Iniciales
-- =====================================================
PRINT ''
PRINT '=== PASO 3/6: Cargar Datos Iniciales ==='
PRINT ''

:r 03_DatosInicialesNegocio.sql

-- =====================================================
-- PASO 4: Crear Tablas de Inscripción
-- =====================================================
PRINT ''
PRINT '=== PASO 4/6: Crear Tablas de Inscripción ==='
PRINT ''

:r 04_CrearTablasInscripcion.sql

-- =====================================================
-- PASO 5: Migrar Datos a Inscripciones
-- =====================================================
PRINT ''
PRINT '=== PASO 5/6: Migrar Datos a Inscripciones ==='
PRINT ''

:r 05_MigrarDatosInscripcion.sql

-- =====================================================
-- PASO 6: Crear Stored Procedures
-- =====================================================
PRINT ''
PRINT '=== PASO 6/6: Crear Stored Procedures ==='
PRINT ''

:r 06_StoredProceduresInscripcion.sql

-- =====================================================
-- VERIFICACIÓN FINAL
-- =====================================================
PRINT ''
PRINT '=========================================='
PRINT '  VERIFICACIÓN FINAL'
PRINT '=========================================='
PRINT ''

USE NegocioBiblioteca;
GO

DECLARE @CantidadMateriales INT
DECLARE @CantidadAlumnos INT
DECLARE @CantidadInscripciones INT
DECLARE @CantidadSPs INT

SELECT @CantidadMateriales = COUNT(*) FROM Material WHERE Activo = 1
SELECT @CantidadAlumnos = COUNT(*) FROM Alumno WHERE Activo = 1
SELECT @CantidadInscripciones = COUNT(*) FROM Inscripcion WHERE Estado = 'Activo'
SELECT @CantidadSPs = COUNT(*) FROM sys.procedures
    WHERE name LIKE 'sp_%Inscripcion%' OR name LIKE 'sp_Promocionar%' OR name LIKE 'sp_ObtenerAlumnos%'

PRINT 'TABLAS CREADAS:'
PRINT '  ✓ Material'
PRINT '  ✓ Alumno'
PRINT '  ✓ Prestamo'
PRINT '  ✓ Devolucion'
PRINT '  ✓ Inscripcion'
PRINT '  ✓ AnioLectivo'
PRINT ''
PRINT 'DATOS CARGADOS:'
PRINT '  • Materiales: ' + CAST(@CantidadMateriales AS NVARCHAR(10))
PRINT '  • Alumnos: ' + CAST(@CantidadAlumnos AS NVARCHAR(10))
PRINT '  • Inscripciones: ' + CAST(@CantidadInscripciones AS NVARCHAR(10))
PRINT ''
PRINT 'STORED PROCEDURES:'
PRINT '  • Total creados: ' + CAST(@CantidadSPs AS NVARCHAR(10))
PRINT ''

-- Mostrar detalles de alumnos por grado
PRINT 'ALUMNOS POR GRADO/DIVISIÓN:'
PRINT ''
SELECT
    Grado,
    Division,
    COUNT(*) AS Cantidad
FROM Alumno
WHERE Activo = 1
GROUP BY Grado, Division
ORDER BY Grado, Division

PRINT ''
PRINT '=========================================='
PRINT '  ✓✓✓ SETUP COMPLETADO EXITOSAMENTE ✓✓✓'
PRINT '=========================================='
PRINT ''
PRINT 'SIGUIENTE PASO:'
PRINT '  1. Compilar la solución: msbuild "Sistema Biblioteca Escolar.sln" /t:Rebuild'
PRINT '  2. Ejecutar la aplicación: View\UI\bin\Debug\UI.exe'
PRINT '  3. Login con: admin / admin123'
PRINT ''
PRINT 'DOCUMENTACIÓN:'
PRINT '  - Ver INSTRUCCIONES_INSTALACION.md'
PRINT '  - Ver README_SISTEMA_INSCRIPCIONES.md'
PRINT ''

GO
