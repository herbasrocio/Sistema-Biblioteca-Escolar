-- =====================================================
-- SCRIPT MAESTRO: Instalación Completa BD Sistema Biblioteca Escolar
-- =====================================================
-- Este script ejecuta todos los scripts de creación en orden
--
-- ADVERTENCIA: Esto eliminará la base de datos SeguridadBiblioteca
-- y la recreará desde cero
-- =====================================================

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║  INSTALACIÓN BD - SISTEMA BIBLIOTECA ESCOLAR      ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Este script ejecutará:'
PRINT '  1. Crear base de datos SeguridadBiblioteca'
PRINT '  2. Crear tablas del sistema'
PRINT '  3. Insertar datos iniciales'
PRINT ''
PRINT 'ADVERTENCIA: La base SeguridadBiblioteca será eliminada si existe'
PRINT ''
PRINT 'Presione Ctrl+C para cancelar o continúe...'
PRINT ''
WAITFOR DELAY '00:00:03';
PRINT 'Iniciando instalación...'
PRINT ''
PRINT '════════════════════════════════════════════════════'
GO

-- =====================================================
-- PASO 1: Crear Base de Datos
-- =====================================================
PRINT ''
PRINT '▶ PASO 1: Creando Base de Datos...'
PRINT '════════════════════════════════════════════════════'
:r "01_CrearBaseDatos.sql"

-- =====================================================
-- PASO 2: Crear Tablas
-- =====================================================
PRINT ''
PRINT '▶ PASO 2: Creando Tablas...'
PRINT '════════════════════════════════════════════════════'
:r "02_CrearTablas.sql"

-- =====================================================
-- PASO 3: Datos Iniciales
-- =====================================================
PRINT ''
PRINT '▶ PASO 3: Insertando Datos Iniciales...'
PRINT '════════════════════════════════════════════════════'
:r "03_DatosIniciales.sql"

-- =====================================================
-- PASO 4: Recalcular DVH
-- =====================================================
PRINT ''
PRINT '▶ PASO 4: Recalculando DVH...'
PRINT '════════════════════════════════════════════════════'
:r "04_RecalcularDVH.sql"

-- =====================================================
-- PASO 5: Agregar Permisos de Reportes
-- =====================================================
PRINT ''
PRINT '▶ PASO 5: Agregando Permisos de Reportes...'
PRINT '════════════════════════════════════════════════════'
:r "05_AgregarPermisosReportes.sql"

-- =====================================================
-- PASO 6: Crear Stored Procedures
-- =====================================================
PRINT ''
PRINT '▶ PASO 6: Creando Stored Procedures...'
PRINT '════════════════════════════════════════════════════'
:r "05_CrearStoredProcedures.sql"

-- =====================================================
-- PASO 7: Agregar Nuevos Reportes
-- =====================================================
PRINT ''
PRINT '▶ PASO 7: Agregando Nuevos Reportes...'
PRINT '════════════════════════════════════════════════════'
:r "06_AgregarNuevosReportes.sql"

-- =====================================================
-- PASO 8: Crear Tabla Bitácora Administrador
-- =====================================================
PRINT ''
PRINT '▶ PASO 8: Creando Tabla Bitácora Administrador...'
PRINT '════════════════════════════════════════════════════'
:r "07_CrearTablaBitacoraAdmin.sql"

-- =====================================================
-- PASO 9: Agregar Permisos de Bitácora
-- =====================================================
PRINT ''
PRINT '▶ PASO 9: Agregando Permisos de Bitácora...'
PRINT '════════════════════════════════════════════════════'
:r "08_AgregarPermisosBitacora.sql"

-- =====================================================
-- PASO 10: Actualizar Criticidad de Bitácora
-- =====================================================
PRINT ''
PRINT '▶ PASO 10: Actualizando Criticidad de Bitácora...'
PRINT '════════════════════════════════════════════════════'
:r "09_ActualizarCriticidadBitacora.sql"

-- =====================================================
-- PASO 11: Unificar Permisos de Reportes
-- =====================================================
PRINT ''
PRINT '▶ PASO 11: Unificando Permisos de Reportes...'
PRINT '════════════════════════════════════════════════════'
:r "15_UnificarPermisosReportes.sql"

-- =====================================================
-- FINALIZACIÓN
-- =====================================================
PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║          INSTALACIÓN COMPLETADA CON ÉXITO         ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Base de datos: SeguridadBiblioteca'
PRINT 'Usuario admin: admin'
PRINT 'Password admin: admin123'
PRINT ''
PRINT '⚠ IMPORTANTE: Cambiar la contraseña del admin en producción'
PRINT ''
PRINT 'Verificar instalación:'
PRINT '  SELECT * FROM Usuario;'
PRINT '  SELECT * FROM Familia WHERE Nombre LIKE ''ROL_%'';'
PRINT ''
GO
