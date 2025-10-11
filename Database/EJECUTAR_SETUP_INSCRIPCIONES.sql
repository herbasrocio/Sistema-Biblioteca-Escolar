-- =====================================================
-- SCRIPT MAESTRO: Setup Completo de Inscripciones
-- Descripción: Ejecuta todos los scripts necesarios
--              para implementar el sistema de inscripciones
-- =====================================================

PRINT '=========================================='
PRINT 'SETUP COMPLETO DEL SISTEMA DE INSCRIPCIONES'
PRINT 'Sistema Biblioteca Escolar - Rocio Herbas'
PRINT '=========================================='
PRINT ''

-- =====================================================
-- PASO 1: Crear tablas de inscripciones
-- =====================================================
PRINT '>>> PASO 1/5: Creando tablas de inscripciones...'
PRINT ''
:r Negocio\04_CrearTablasInscripcion.sql
PRINT ''

-- =====================================================
-- PASO 2: Migrar datos existentes
-- =====================================================
PRINT '>>> PASO 2/5: Migrando datos existentes...'
PRINT ''
:r Negocio\05_MigrarDatosInscripcion.sql
PRINT ''

-- =====================================================
-- PASO 3: Crear stored procedures
-- =====================================================
PRINT '>>> PASO 3/5: Creando stored procedures...'
PRINT ''
:r Negocio\06_StoredProceduresInscripcion.sql
PRINT ''

-- =====================================================
-- PASO 4: Crear permiso en sistema de seguridad
-- =====================================================
PRINT '>>> PASO 4/5: Configurando permisos de seguridad...'
PRINT ''
:r 15_CrearPermisoPromocionAlumnos.sql
PRINT ''

-- =====================================================
-- PASO 5: Verificación final
-- =====================================================
PRINT '>>> PASO 5/5: Verificación final...'
PRINT ''

USE NegocioBiblioteca;
GO

PRINT 'Verificando estructura de base de datos...'

DECLARE @TablasCreadas INT = 0
DECLARE @SPsCreados INT = 0

-- Verificar tablas
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Inscripcion')
    SET @TablasCreadas = @TablasCreadas + 1

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AnioLectivo')
    SET @TablasCreadas = @TablasCreadas + 1

-- Verificar SPs
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_ObtenerInscripcionActiva')
    SET @SPsCreados = @SPsCreados + 1

IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_PromocionarAlumnosPorGrado')
    SET @SPsCreados = @SPsCreados + 1

IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_PromocionarTodosLosAlumnos')
    SET @SPsCreados = @SPsCreados + 1

IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_ObtenerAlumnosPorGradoDivision')
    SET @SPsCreados = @SPsCreados + 1

PRINT ''
PRINT '=== RESUMEN DE VERIFICACIÓN ==='
PRINT 'Tablas creadas: ' + CAST(@TablasCreadas AS NVARCHAR(10)) + '/2'
PRINT 'Stored Procedures creados: ' + CAST(@SPsCreados AS NVARCHAR(10)) + '/4'

IF @TablasCreadas = 2 AND @SPsCreados = 4
BEGIN
    PRINT ''
    PRINT '✓✓✓ SETUP COMPLETADO EXITOSAMENTE ✓✓✓'
    PRINT ''
    PRINT 'PRÓXIMOS PASOS:'
    PRINT '  1. Compilar la solución en Visual Studio'
    PRINT '  2. Ejecutar la aplicación'
    PRINT '  3. Acceder al menú de Administrador'
    PRINT '  4. Abrir "Promoción de Alumnos"'
    PRINT ''
    PRINT 'NOTAS IMPORTANTES:'
    PRINT '  • Los datos existentes de alumnos fueron migrados'
    PRINT '  • El sistema mantiene historial completo de inscripciones'
    PRINT '  • La ventana de Gestionar Alumnos sigue funcionando igual'
    PRINT ''
END
ELSE
BEGIN
    PRINT ''
    PRINT '⚠⚠⚠ ADVERTENCIA: Algunas tablas o SPs no fueron creados ⚠⚠⚠'
    PRINT 'Revisar los mensajes de error anteriores'
    PRINT ''
END

-- Mostrar estadísticas de inscripciones
PRINT '=== ESTADÍSTICAS DE INSCRIPCIONES ==='
SELECT
    AnioLectivo,
    COUNT(*) AS TotalInscripciones,
    SUM(CASE WHEN Estado = 'Activo' THEN 1 ELSE 0 END) AS Activas,
    SUM(CASE WHEN Estado = 'Finalizado' THEN 1 ELSE 0 END) AS Finalizadas
FROM Inscripcion
GROUP BY AnioLectivo
ORDER BY AnioLectivo DESC

PRINT ''
PRINT '=========================================='
PRINT 'FIN DEL SETUP'
PRINT '=========================================='

GO
