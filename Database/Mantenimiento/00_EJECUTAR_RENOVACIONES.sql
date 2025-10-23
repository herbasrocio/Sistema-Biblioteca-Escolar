-- =====================================================
-- Script Maestro: Configuración de Renovaciones
-- Descripción: Ejecuta todos los scripts de renovación
-- Fecha: 2025-10-22
-- =====================================================

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║  INSTALACIÓN DE MÓDULO DE RENOVACIÓN DE PRÉSTAMOS  ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- 1. Agregar campos de renovación a tabla Prestamo (NegocioBiblioteca)
PRINT 'Paso 1: Agregando campos de renovación a tabla Prestamo...'
:r 01_AgregarCamposRenovacion.sql
PRINT ''

-- 2. Crear tabla de auditoría (NegocioBiblioteca)
PRINT 'Paso 2: Creando tabla de auditoría RenovacionPrestamo...'
:r 02_CrearTablaRenovacion.sql
PRINT ''

-- 3. Agregar patente al sistema de permisos (SeguridadBiblioteca)
PRINT 'Paso 3: Agregando patente renovarPrestamo...'
:r 03_AgregarPatenteRenovacion.sql
PRINT ''

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║         INSTALACIÓN COMPLETADA EXITOSAMENTE        ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'El módulo de renovación de préstamos está listo para usar.'
PRINT ''
PRINT 'Funcionalidades agregadas:'
PRINT '  - Campos CantidadRenovaciones y FechaUltimaRenovacion en Prestamo'
PRINT '  - Tabla RenovacionPrestamo para auditoría'
PRINT '  - Patente renovarPrestamo asignada a roles'
PRINT ''
