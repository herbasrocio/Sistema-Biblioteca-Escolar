# 📋 Instrucciones de Instalación - Sistema de Inscripciones y Promoción

## ⚠️ IMPORTANTE: Leer antes de comenzar

Este proceso instalará el nuevo sistema de inscripciones y promoción de alumnos. **Los datos existentes NO se perderán** - serán migrados automáticamente a la nueva estructura.

---

## ✅ Pre-requisitos

- [ ] SQL Server corriendo
- [ ] Base de datos `NegocioBiblioteca` existente (si no existe, ver sección "Instalación Desde Cero")
- [ ] Base de datos `SeguridadBiblioteca` existente
- [ ] Visual Studio 2022 (o MSBuild disponible)
- [ ] Backup de ambas bases de datos (**MUY RECOMENDADO**)

---

## 🆕 Instalación Desde Cero - Base de Datos de Negocio

**¿La base de datos NegocioBiblioteca NO existe?** Usa estos scripts primero:

### Opción A: Verificar Estado Actual

```bash
sqlcmd -S localhost -E -i "Database\Negocio\00_VerificarBaseDatos.sql"
```

Este script mostrará:
- ✅ Si la base de datos existe
- ✅ Qué tablas están creadas
- ✅ Cantidad de datos
- ✅ Diagnóstico completo

### Opción B: Crear Base de Datos Completa

Si la base de datos NO existe o está vacía:

```bash
# Desde línea de comandos
cd "Database\Negocio"
sqlcmd -S localhost -E -i "00_EJECUTAR_TODO_NEGOCIO.sql"
```

**Este script crea:**
1. Base de datos `NegocioBiblioteca`
2. Tablas: Material, Alumno, Prestamo, Devolucion
3. Datos iniciales: 16 materiales, 10 alumnos
4. Tablas de inscripción
5. Stored procedures

**⚠️ ADVERTENCIA:** Este script ELIMINA y RECREA la base de datos. Haz backup primero si ya existe.

### Opción C: Solo Agregar Datos de Prueba

Si la base de datos existe pero está vacía:

```bash
sqlcmd -S localhost -E -i "Database\Negocio\00_VerificarYCrearDatosPrueba.sql"
```

Crea 16 alumnos de prueba en diferentes grados si la tabla Alumno está vacía.

---

## 📦 Instalación - Opción 1: Script Automático (Recomendado)

### Paso 1: Hacer Backup de las Bases de Datos

```sql
-- En SQL Server Management Studio
BACKUP DATABASE NegocioBiblioteca
TO DISK = 'C:\Backups\NegocioBiblioteca_Backup.bak'

BACKUP DATABASE SeguridadBiblioteca
TO DISK = 'C:\Backups\SeguridadBiblioteca_Backup.bak'
```

### Paso 2: Ejecutar Script Maestro

Abrir **SQL Server Management Studio** y ejecutar:

```bash
# Opción A: Desde SSMS
# 1. Abrir el archivo: Database/EJECUTAR_SETUP_INSCRIPCIONES.sql
# 2. Presionar F5 o click en "Execute"

# Opción B: Desde línea de comandos
cd "C:\Users\Ro\Desktop\UAI\PRACTICAS PROF 3RO\MI PROYECTO - HERBAS\Sistema Biblioteca\Sistema Biblioteca Escolar - Rocio Herbas"

sqlcmd -S localhost -E -i "Database\EJECUTAR_SETUP_INSCRIPCIONES.sql"
```

**El script hace:**
1. ✅ Crea tablas `Inscripcion` y `AnioLectivo`
2. ✅ Migra datos existentes de alumnos
3. ✅ Crea 4 stored procedures
4. ✅ Crea permiso `PromocionAlumnos` en seguridad
5. ✅ Asigna permiso al rol Administrador
6. ✅ Muestra reporte de verificación

### Paso 3: Verificar Resultados

Al final del script deberías ver:

```
✓✓✓ SETUP COMPLETADO EXITOSAMENTE ✓✓✓

Tablas creadas: 2/2
Stored Procedures creados: 4/4
```

Si ves errores, revisa la sección de **Troubleshooting** al final.

---

## 📦 Instalación - Opción 2: Manual (Paso a Paso)

### Paso 1: Crear Tablas de Inscripciones

```bash
sqlcmd -S localhost -d NegocioBiblioteca -E -i "Database\Negocio\04_CrearTablasInscripcion.sql"
```

**Verifica que se crearon:**
- Tabla `Inscripcion` ✅
- Tabla `AnioLectivo` ✅
- Año lectivo actual (2025) ✅

### Paso 2: Migrar Datos Existentes

```bash
sqlcmd -S localhost -d NegocioBiblioteca -E -i "Database\Negocio\05_MigrarDatosInscripcion.sql"
```

**Verifica:**
- Cantidad de alumnos migrados
- Mensaje: "Migración completada exitosamente"

### Paso 3: Crear Stored Procedures

```bash
sqlcmd -S localhost -d NegocioBiblioteca -E -i "Database\Negocio\06_StoredProceduresInscripcion.sql"
```

**Verifica que se crearon:**
- `sp_ObtenerInscripcionActiva` ✅
- `sp_PromocionarAlumnosPorGrado` ✅
- `sp_PromocionarTodosLosAlumnos` ✅
- `sp_ObtenerAlumnosPorGradoDivision` ✅

### Paso 4: Configurar Permisos

```bash
sqlcmd -S localhost -d SeguridadBiblioteca -E -i "Database\15_CrearPermisoPromocionAlumnos.sql"
```

**Verifica:**
- Patente `PromocionAlumnos` creada ✅
- Asignada a rol `ROL_Administrador` ✅

---

## 🖥️ Compilación de la Aplicación

### Desde Visual Studio

1. Abrir `Sistema Biblioteca Escolar.sln`
2. **Build > Rebuild Solution** (Ctrl+Shift+B)
3. Verificar que compile sin errores

### Desde Línea de Comandos

```bash
cd "C:\Users\Ro\Desktop\UAI\PRACTICAS PROF 3RO\MI PROYECTO - HERBAS\Sistema Biblioteca\Sistema Biblioteca Escolar - Rocio Herbas"

"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Sistema Biblioteca Escolar.sln" -p:Configuration=Debug -v:minimal -t:Rebuild
```

**Resultado esperado:**
```
✓ DomainModel.dll
✓ DAL.dll
✓ BLL.dll
✓ ServicesSecurity.dll
✓ Services.dll
✓ UI.exe
```

**Warnings esperados (no son errores):**
- 3 warnings sobre variables no usadas (pueden ignorarse)

---

## 🚀 Primer Uso

### 1. Ejecutar la Aplicación

```bash
cd "View\UI\bin\Debug"
UI.exe
```

O desde Visual Studio: **F5** (Debug) o **Ctrl+F5** (Sin debug)

### 2. Iniciar Sesión

- Usuario: `admin`
- Contraseña: `admin123`

### 3. Acceder a Promoción de Alumnos

**Si el menú es dinámico:**
- La opción "Promoción de Alumnos" aparecerá automáticamente en el menú de Administrador

**Si el menú es estático:**
- Necesitarás agregar manualmente el ítem de menú (ver sección siguiente)

---

## 🔧 Agregar Ventana al Menú (Solo si el menú es estático)

Editar `View\UI\WinUi\Administración\menu.cs`:

```csharp
// En el método donde se crean los menús del administrador
private void ConfigurarMenuAdministrador()
{
    // ... código existente ...

    // Agregar ítem de Promoción de Alumnos
    ToolStripMenuItem menuPromocion = new ToolStripMenuItem();
    menuPromocion.Text = LanguageManager.Translate("promocion_alumnos");
    menuPromocion.Click += (sender, e) => {
        gestionPromocionAlumnos ventana = new gestionPromocionAlumnos(_usuarioLogueado);
        ventana.Show();
    };

    // Agregarlo al menú principal (ajustar según tu estructura)
    menuAdministracion.DropDownItems.Add(menuPromocion);
}
```

---

## 🧪 Pruebas Recomendadas

### Test 1: Ver Estadísticas

1. Abrir "Promoción de Alumnos"
2. Verificar que se muestran alumnos por grado/división
3. Verificar el total de alumnos

### Test 2: Promoción Manual

1. Seleccionar:
   - Año actual: 2025
   - Año siguiente: 2026
   - Grado actual: 1
   - División actual: A
   - Grado nuevo: 2
   - División nueva: A
2. Click "Promocionar Grado"
3. Confirmar
4. Verificar mensaje de éxito

### Test 3: Verificar en Base de Datos

```sql
USE NegocioBiblioteca;

-- Ver inscripciones del año 2026
SELECT
    a.Nombre,
    a.Apellido,
    i.Grado,
    i.Division,
    i.AnioLectivo,
    i.Estado
FROM Inscripcion i
INNER JOIN Alumno a ON i.IdAlumno = a.IdAlumno
WHERE i.AnioLectivo = 2026
ORDER BY i.Grado, i.Division, a.Apellido
```

### Test 4: Ventana de Gestionar Alumnos

1. Abrir "Gestionar Alumnos"
2. Verificar que sigue funcionando normalmente
3. Crear un nuevo alumno
4. Verificar que se crea su inscripción automáticamente

---

## 🐛 Troubleshooting

### Error: "La tabla Inscripcion ya existe"

**Solución:** La instalación ya se ejecutó. Verificar con:

```sql
SELECT COUNT(*) FROM Inscripcion
SELECT COUNT(*) FROM AnioLectivo
```

### Error: "No se encontraron alumnos para migrar"

**Causa:** No hay alumnos con Grado asignado en la tabla Alumno.

**Solución:** Normal si la BD está vacía. Agregar alumnos desde "Gestionar Alumnos".

### Error: "No se encontró el rol ROL_Administrador"

**Causa:** La base de datos de seguridad no tiene el rol configurado.

**Solución:** Ejecutar los scripts de seguridad base primero.

### La ventana no aparece en el menú

**Causas posibles:**
1. El permiso no está asignado al usuario
2. El menú es estático y falta agregarlo manualmente
3. Necesitas cerrar sesión y volver a entrar

**Verificar permiso:**
```sql
USE SeguridadBiblioteca;

SELECT
    u.Nombre AS Usuario,
    p.Nombre AS Permiso
FROM Usuario u
INNER JOIN UsuarioFamilia uf ON u.IdUsuario = uf.IdUsuario
INNER JOIN FamiliaPatente fp ON uf.IdFamilia = fp.IdFamilia
INNER JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE u.Nombre = 'admin'
AND p.Nombre = 'PromocionAlumnos'
```

### Error de compilación: "No se puede encontrar DomainModel.Exceptions"

**Solución:** Limpiar y recompilar:

```bash
msbuild "Sistema Biblioteca Escolar.sln" -t:Clean
msbuild "Sistema Biblioteca Escolar.sln" -t:Rebuild
```

---

## 📊 Consultas Útiles

### Ver todas las inscripciones activas del año actual

```sql
SELECT
    a.Apellido + ', ' + a.Nombre AS Alumno,
    i.Grado + '° ' + ISNULL(i.Division, '') AS Curso,
    i.AnioLectivo,
    i.Estado
FROM Inscripcion i
INNER JOIN Alumno a ON i.IdAlumno = a.IdAlumno
WHERE i.AnioLectivo = YEAR(GETDATE())
AND i.Estado = 'Activo'
ORDER BY i.Grado, i.Division, a.Apellido
```

### Ver historial de un alumno

```sql
SELECT
    i.AnioLectivo,
    i.Grado + '° ' + ISNULL(i.Division, '') AS Curso,
    i.Estado,
    i.FechaInscripcion
FROM Inscripcion i
INNER JOIN Alumno a ON i.IdAlumno = a.IdAlumno
WHERE a.DNI = '12345678'  -- Reemplazar con DNI del alumno
ORDER BY i.AnioLectivo DESC
```

### Estadísticas por grado

```sql
SELECT
    Grado + '°' AS Grado,
    Division,
    COUNT(*) AS CantidadAlumnos
FROM Inscripcion
WHERE AnioLectivo = YEAR(GETDATE())
AND Estado = 'Activo'
GROUP BY Grado, Division
ORDER BY Grado, Division
```

---

## 📞 Soporte

Si encuentras problemas:

1. Revisar los mensajes de error en el script SQL
2. Verificar los logs de compilación
3. Consultar `RESUMEN_IMPLEMENTACION_COMPLETA.md` para más detalles técnicos

---

## ✅ Checklist Final

- [ ] Backup de bases de datos realizado
- [ ] Scripts SQL ejecutados exitosamente
- [ ] Aplicación compilada sin errores
- [ ] Ventana de Promoción accesible desde el menú
- [ ] Pruebas básicas realizadas
- [ ] Ventana de Gestionar Alumnos funciona correctamente

---

**¡Instalación completa!** 🎉

El sistema está listo para usar. Consulta `RESUMEN_IMPLEMENTACION_COMPLETA.md` para documentación técnica detallada.
