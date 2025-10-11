# ✅ Checklist de Verificación - Sistema de Inscripciones

## 📋 Antes de la Instalación

- [ ] **Backup realizado** de ambas bases de datos
  - [ ] `NegocioBiblioteca` respaldada
  - [ ] `SeguridadBiblioteca` respaldada
  - [ ] Backups guardados en ubicación segura

- [ ] **Conexión a SQL Server verificada**
  ```sql
  SELECT @@VERSION
  ```

- [ ] **Bases de datos existen**
  ```sql
  SELECT name FROM sys.databases
  WHERE name IN ('NegocioBiblioteca', 'SeguridadBiblioteca')
  ```

---

## 📦 Archivos del Proyecto

### Modelo de Dominio (DomainModel)
- [ ] `Inscripcion.cs` existe
- [ ] `AnioLectivo.cs` existe
- [ ] `Exceptions/ValidacionException.cs` existe
- [ ] `DomainModel.csproj` actualizado

### Capa de Acceso a Datos (DAL)
- [ ] `Contracts/IInscripcionRepository.cs` existe
- [ ] `Tools/InscripcionAdapter.cs` existe
- [ ] `Implementations/InscripcionRepository.cs` existe
- [ ] `DAL.csproj` actualizado

### Lógica de Negocio (BLL)
- [ ] `InscripcionBLL.cs` existe
- [ ] `ValidationBLL.cs` mejorado
- [ ] `AlumnoBLL.cs` mejorado
- [ ] `BLL.csproj` actualizado

### Interfaz de Usuario (UI)
- [ ] `WinUi/Administración/gestionPromocionAlumnos.cs` existe
- [ ] `WinUi/Administración/gestionPromocionAlumnos.Designer.cs` existe
- [ ] `WinUi/Administración/gestionPromocionAlumnos.resx` existe
- [ ] `UI.csproj` actualizado

### Traducciones
- [ ] `Resources/I18n/idioma.es-AR` actualizado
- [ ] `Resources/I18n/idioma.en-GB` actualizado

### Scripts SQL
- [ ] `Database/Negocio/04_CrearTablasInscripcion.sql` existe
- [ ] `Database/Negocio/05_MigrarDatosInscripcion.sql` existe
- [ ] `Database/Negocio/06_StoredProceduresInscripcion.sql` existe
- [ ] `Database/15_CrearPermisoPromocionAlumnos.sql` existe
- [ ] `Database/EJECUTAR_SETUP_INSCRIPCIONES.sql` existe

### Documentación
- [ ] `README_SISTEMA_INSCRIPCIONES.md` existe
- [ ] `INSTRUCCIONES_INSTALACION.md` existe
- [ ] `RESUMEN_IMPLEMENTACION_COMPLETA.md` existe
- [ ] `IMPLEMENTACION_INSCRIPCIONES.md` existe

---

## 🔨 Compilación

- [ ] **Proyecto compila sin errores**
  ```bash
  msbuild "Sistema Biblioteca Escolar.sln" -t:Rebuild
  ```

- [ ] **Warnings esperados (3):**
  - [ ] LoginService.cs:87 (variable iex)
  - [ ] Login.cs:210 (variable ex)
  - [ ] gestionPromocionAlumnos.cs:153 (variable ex)

- [ ] **Todos los DLLs generados:**
  - [ ] DomainModel.dll
  - [ ] DAL.dll
  - [ ] BLL.dll
  - [ ] ServicesSecurity.dll
  - [ ] Services.dll
  - [ ] UI.exe

---

## 🗄️ Instalación de Base de Datos

### Ejecutar Scripts SQL

- [ ] **Script 1: Crear Tablas**
  ```bash
  sqlcmd -S localhost -d NegocioBiblioteca -E -i "Database\Negocio\04_CrearTablasInscripcion.sql"
  ```
  - [ ] Tabla `Inscripcion` creada
  - [ ] Tabla `AnioLectivo` creada
  - [ ] Año lectivo 2025 insertado
  - [ ] Índices creados

- [ ] **Script 2: Migrar Datos**
  ```bash
  sqlcmd -S localhost -d NegocioBiblioteca -E -i "Database\Negocio\05_MigrarDatosInscripcion.sql"
  ```
  - [ ] Alumnos migrados correctamente
  - [ ] Mensaje de éxito mostrado
  - [ ] Estadísticas verificadas

- [ ] **Script 3: Stored Procedures**
  ```bash
  sqlcmd -S localhost -d NegocioBiblioteca -E -i "Database\Negocio\06_StoredProceduresInscripcion.sql"
  ```
  - [ ] sp_ObtenerInscripcionActiva creado
  - [ ] sp_PromocionarAlumnosPorGrado creado
  - [ ] sp_PromocionarTodosLosAlumnos creado
  - [ ] sp_ObtenerAlumnosPorGradoDivision creado

- [ ] **Script 4: Permisos**
  ```bash
  sqlcmd -S localhost -d SeguridadBiblioteca -E -i "Database\15_CrearPermisoPromocionAlumnos.sql"
  ```
  - [ ] Patente `PromocionAlumnos` creada
  - [ ] Asignada a rol `ROL_Administrador`

### Verificación en Base de Datos

```sql
-- Verificar tablas
USE NegocioBiblioteca;
SELECT COUNT(*) FROM Inscripcion;           -- Debe haber registros
SELECT COUNT(*) FROM AnioLectivo;           -- Debe haber al menos 1

-- Verificar SPs
SELECT name FROM sys.procedures
WHERE name LIKE 'sp_%Inscripcion%'
OR name LIKE 'sp_Promocionar%';             -- Debe mostrar 4 SPs

-- Verificar permiso
USE SeguridadBiblioteca;
SELECT p.Nombre FROM Patente p
WHERE p.Nombre = 'PromocionAlumnos';        -- Debe existir
```

- [ ] Todas las consultas retornan resultados esperados

---

## 🖥️ Pruebas de la Aplicación

### Inicio de Sesión

- [ ] **Aplicación inicia correctamente**
- [ ] **Login funciona**
  - Usuario: `admin`
  - Contraseña: `admin123`
- [ ] **Menú principal se carga**

### Ventana de Promoción de Alumnos

- [ ] **Ventana accesible desde el menú**
  - Ruta: Menú Administrador → Promoción de Alumnos

- [ ] **Interfaz se carga correctamente**
  - [ ] NumericUpDown de años visibles
  - [ ] DataGridView visible
  - [ ] Labels de resumen visibles
  - [ ] GroupBox de promoción por grado visible
  - [ ] Botón de promoción masiva visible

- [ ] **Cargar Estadísticas funciona**
  - [ ] Click en "Cargar Estadísticas"
  - [ ] Grid se llena con datos
  - [ ] Resumen muestra total correcto

### Promoción Manual

- [ ] **Promoción por grado funciona**
  1. [ ] Seleccionar grado actual
  2. [ ] Ingresar división (opcional)
  3. [ ] Seleccionar grado nuevo
  4. [ ] Click "Promocionar Grado"
  5. [ ] Confirmación aparece
  6. [ ] Confirmar operación
  7. [ ] Mensaje de éxito mostrado
  8. [ ] Estadísticas actualizadas

- [ ] **Verificar en BD**
  ```sql
  SELECT TOP 10
      a.Nombre,
      a.Apellido,
      i.Grado,
      i.Division,
      i.AnioLectivo,
      i.Estado
  FROM Inscripcion i
  INNER JOIN Alumno a ON i.IdAlumno = a.IdAlumno
  WHERE i.AnioLectivo = 2026
  ORDER BY i.FechaInscripcion DESC
  ```
  - [ ] Inscripciones nuevas existen
  - [ ] Grados correctos
  - [ ] Estado "Activo"

### Promoción Masiva

- [ ] **Promoción masiva funciona**
  1. [ ] Click "PROMOCIÓN MASIVA"
  2. [ ] Primera confirmación aparece
  3. [ ] Aceptar
  4. [ ] Segunda confirmación aparece
  5. [ ] Aceptar
  6. [ ] Proceso completa
  7. [ ] Reporte con detalles mostrado
     - [ ] Alumnos promovidos
     - [ ] Egresados
     - [ ] Total procesados

### Ventana Gestionar Alumnos

- [ ] **Ventana sigue funcionando igual**
  - [ ] Abrir "Gestionar Alumnos"
  - [ ] Lista de alumnos se carga
  - [ ] Crear nuevo alumno funciona
  - [ ] Editar alumno funciona
  - [ ] Eliminar alumno funciona

- [ ] **Inscripción automática**
  - [ ] Crear alumno nuevo con grado
  - [ ] Verificar en BD que se creó inscripción
  ```sql
  SELECT TOP 1 * FROM Inscripcion
  ORDER BY FechaInscripcion DESC
  ```

---

## 🌐 Internacionalización

### Español (es-AR)

- [ ] **Cambiar idioma a Español**
- [ ] **Verificar traducciones en ventana:**
  - [ ] Título: "Promoción de Alumnos"
  - [ ] "Año Actual"
  - [ ] "Año Siguiente"
  - [ ] "Cargar Estadísticas"
  - [ ] "Promoción por Grado"
  - [ ] "Promoción Masiva de Todos los Grados"

### Inglés (en-GB)

- [ ] **Cambiar idioma a Inglés**
- [ ] **Verificar traducciones en ventana:**
  - [ ] Título: "Student Promotion"
  - [ ] "Current Year"
  - [ ] "Next Year"
  - [ ] "Load Statistics"
  - [ ] "Promotion by Grade"
  - [ ] "Massive Promotion of All Grades"

---

## 🔒 Seguridad y Permisos

- [ ] **Usuario sin permiso NO ve la opción**
  1. Crear usuario de prueba sin rol Admin
  2. Verificar que no aparece "Promoción de Alumnos" en menú

- [ ] **Usuario Administrador SÍ ve la opción**
  1. Login como admin
  2. Verificar que aparece "Promoción de Alumnos"

- [ ] **Confirmaciones de seguridad funcionan**
  - [ ] Promoción por grado requiere confirmación
  - [ ] Promoción masiva requiere doble confirmación
  - [ ] Cancelar confirmación cancela operación

---

## 📊 Consultas de Verificación

### Integridad de Datos

```sql
-- Verificar que no hay inscripciones duplicadas
USE NegocioBiblioteca;

SELECT IdAlumno, AnioLectivo, COUNT(*) AS Cantidad
FROM Inscripcion
WHERE Estado = 'Activo'
GROUP BY IdAlumno, AnioLectivo
HAVING COUNT(*) > 1

-- Resultado esperado: 0 filas
```
- [ ] No hay inscripciones duplicadas

```sql
-- Verificar consistencia de grados
SELECT DISTINCT Grado
FROM Inscripcion
ORDER BY Grado

-- Resultado esperado: 1, 2, 3, 4, 5, 6, 7 (o subconjunto)
```
- [ ] Grados válidos (1-7)

```sql
-- Verificar años lectivos
SELECT Anio, Estado
FROM AnioLectivo
ORDER BY Anio DESC

-- Resultado esperado: Años configurados correctamente
```
- [ ] Año lectivo actual existe y está activo

---

## 🐛 Resolución de Problemas

### Si algo falla:

- [ ] **Revisar mensajes de error SQL**
- [ ] **Verificar logs de compilación**
- [ ] **Consultar INSTRUCCIONES_INSTALACION.md sección Troubleshooting**
- [ ] **Verificar que todos los scripts SQL se ejecutaron en orden**
- [ ] **Confirmar que la aplicación se recompiló después de los cambios**

---

## ✅ Checklist Final de Aprobación

### Funcionalidad
- [ ] Todas las ventanas funcionan correctamente
- [ ] Promoción manual funciona
- [ ] Promoción masiva funciona
- [ ] Estadísticas se muestran correctamente
- [ ] Validaciones funcionan
- [ ] Mensajes de error claros

### Datos
- [ ] Datos existentes migrados correctamente
- [ ] Historial se mantiene
- [ ] No hay pérdida de información
- [ ] Integridad referencial mantenida

### Seguridad
- [ ] Permisos funcionan
- [ ] Confirmaciones aparecen
- [ ] Operaciones seguras

### UI/UX
- [ ] Interfaz clara y usable
- [ ] Traducciones correctas
- [ ] Mensajes informativos
- [ ] Colores y diseño consistentes

### Documentación
- [ ] Documentación completa
- [ ] Instrucciones claras
- [ ] Ejemplos de código incluidos
- [ ] Troubleshooting documentado

---

## 🎉 Sistema Aprobado

- [ ] **TODAS las verificaciones pasaron**
- [ ] **Sistema listo para producción**
- [ ] **Documentación entregada**
- [ ] **Usuarios capacitados**

---

**Fecha de verificación:** _______________

**Verificado por:** _______________

**Observaciones:**
```
_________________________________________
_________________________________________
_________________________________________
```

**Estado final:**
- [ ] ✅ APROBADO - Listo para producción
- [ ] ⚠️ APROBADO CON OBSERVACIONES - Ver notas
- [ ] ❌ NO APROBADO - Requiere correcciones

---

**Firma:** _______________
