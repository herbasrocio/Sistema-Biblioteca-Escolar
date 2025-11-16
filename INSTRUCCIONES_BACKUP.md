# Instrucciones: Sistema de Backup y Restore

## 📋 Resumen de la Implementación

Se ha implementado un sistema completo de **Gestión de Backup y Restore** para el Sistema de Biblioteca Escolar, siguiendo la arquitectura N-Tier existente del proyecto.

---

## 🗂️ Componentes Implementados

### 1. **Base de Datos**
- ✅ **Tabla `Backup`** en `SeguridadBiblioteca` (catálogo de backups)
- ✅ **Patentes de seguridad**: "Gestión Backup" y "Consultar Backups"
- ✅ Scripts SQL numerados y integrados en el script maestro

### 2. **Capa de Dominio** (`Model/DomainModel`)
- ✅ **`Backup.cs`**: Entidad con propiedades calculadas (NombreArchivo, TamañoFormateado, etc.)

### 3. **Capa de Datos** (`Model/ServicesSeguridad/DAL`)
- ✅ **`BackupRepository.cs`**:
  - Gestión del catálogo (CRUD)
  - Ejecución de comandos BACKUP/RESTORE de SQL Server
  - Manejo de transacciones y seguridad

### 4. **Capa de Negocio** (`Model/ServicesSeguridad/BLL`)
- ✅ **`BackupBLL.cs`**:
  - Validaciones de permisos y reglas de negocio
  - Generación automática de nombres de archivo con timestamp
  - Verificación de espacio en disco
  - Registro en bitácora de seguridad
  - Soporte para backups Full y Differential

### 5. **Interfaz de Usuario** (`View/UI/WinUi/Administración`)
- ✅ **`FrmGestionBackup.cs`**:
  - Crear backups (Full/Differential)
  - Restaurar desde catálogo o archivo externo
  - Explorador de carpetas para destino personalizado
  - DataGridView con historial de backups (coloreado por estado)
  - Eliminación de backups
  - Indicador de espacio disponible

### 6. **Internacionalización**
- ✅ **Español (es-AR)**: 40+ claves de traducción
- ✅ **Inglés (en-GB)**: 40+ claves de traducción
- ✅ Integración con `LanguageManager`

### 7. **Integración con Menú Principal**
- ✅ Nuevo ítem "Backup" en el menú principal
- ✅ Control de permisos basado en patente "FrmGestionBackup"
- ✅ Solo visible para usuarios con permiso (Administrador por defecto)

---

## 🚀 Instalación

### Paso 1: Ejecutar Scripts SQL

**IMPORTANTE**: Ejecutar estos scripts en el siguiente orden:

```sql
-- Opción 1: Ejecutar el script maestro completo (RECOMENDADO)
-- Esto recreará toda la base de datos SeguridadBiblioteca
:r "C:\Users\roc_2\OneDrive\Desktop\PRACTICAS 3RO\PROYECTO BIBLIOTECA ESCOLAR\Database\00_EJECUTAR_TODO.sql"

-- Opción 2: Ejecutar solo los scripts de Backup (si ya tienes la BD configurada)
:r "C:\Users\roc_2\OneDrive\Desktop\PRACTICAS 3RO\PROYECTO BIBLIOTECA ESCOLAR\Database\16_CrearTablaBackup.sql"
:r "C:\Users\roc_2\OneDrive\Desktop\PRACTICAS 3RO\PROYECTO BIBLIOTECA ESCOLAR\Database\17_AgregarPatentesBackup.sql"
```

**Verificar instalación:**
```sql
USE SeguridadBiblioteca;

-- Verificar tabla
SELECT * FROM Backup;

-- Verificar patentes
SELECT * FROM Patente WHERE NombrePatente LIKE '%Backup%';

-- Verificar asignación a Administrador
SELECT f.NombreFamilia, p.NombrePatente
FROM Familia f
JOIN FamiliaPatente fp ON f.IdFamilia = fp.IdFamilia
JOIN Patente p ON fp.IdPatente = p.IdPatente
WHERE f.NombreFamilia = 'ROL_Administrador'
  AND p.NombrePatente LIKE '%Backup%';
```

### Paso 2: Compilar el Proyecto

```bash
# Desde la raíz del proyecto
msbuild "Sistema Biblioteca Escolar.sln" /p:Configuration=Debug

# O usar Visual Studio: Build > Build Solution (Ctrl+Shift+B)
```

### Paso 3: Verificar Referencias

El proyecto `UI.csproj` debe referenciar:
- ✅ `ServicesSecurity.csproj` (ya existe)
- ✅ `DomainModel.csproj` (ya existe)
- ✅ `BLL.csproj` (ya existe)

---

## 📖 Uso del Sistema

### 1. **Acceso al Sistema**

1. Iniciar sesión como **admin** (o usuario con rol Administrador)
2. En el menú principal, hacer clic en **"Backup"**
3. Se abrirá el formulario `FrmGestionBackup`

### 2. **Crear un Backup**

**Crear Backup Completo (Full):**
```
1. Seleccionar Base de Datos: SeguridadBiblioteca o NegocioBiblioteca
2. Tipo de Backup: Completo (Full)
3. Descripción (opcional): "Backup antes de actualización"
4. Ruta Destino: Por defecto en [AppDir]\Backups
   - Click "Examinar..." para cambiar ubicación
5. Click "Crear Backup"
6. Confirmar en el diálogo
7. Esperar... (puede tardar según tamaño de BD)
8. Verificar en la grilla: Estado = "Exitoso", color verde
```

**Crear Backup Diferencial:**
```
- Requiere al menos un backup Full previo
- Solo guarda cambios desde el último Full
- Más rápido y ocupa menos espacio
```

**Nombre de archivo generado automáticamente:**
```
SeguridadBiblioteca_Full_20251115_143025.bak
NegocioBiblioteca_Diff_20251115_150530.bak
```

### 3. **Restaurar un Backup**

**⚠️ ADVERTENCIA**: La restauración sobrescribe TODOS los datos actuales.

**Restaurar desde Catálogo:**
```
1. Seleccionar un backup de la grilla (Estado: "Exitoso")
2. Click "Restaurar Seleccionado"
3. Leer advertencia CRÍTICA
4. Confirmar PRIMERA vez
5. Confirmar SEGUNDA vez (confirmación adicional)
6. Esperar... (se desconectarán usuarios activos)
7. La BD se restaura
8. IMPORTANTE: Reiniciar la aplicación
```

**Restaurar desde Archivo Externo:**
```
1. Click "Restaurar desde Archivo"
2. Seleccionar archivo .bak en el explorador
3. Seleccionar BD destino (SeguridadBiblioteca o NegocioBiblioteca)
4. Confirmar DOS veces
5. Esperar...
6. Reiniciar aplicación
```

### 4. **Gestión del Catálogo**

**Ver Historial:**
- La grilla muestra todos los backups registrados
- Colores:
  - 🟢 Verde: Exitoso
  - 🔴 Rojo: Fallido
  - 🟡 Amarillo: En Proceso

**Eliminar Backup:**
```
1. Seleccionar backup en la grilla
2. Click "Eliminar Backup"
3. Confirmar
4. Se elimina el registro Y el archivo físico
```

**Refrescar:**
- Click "Refrescar" para actualizar la grilla

---

## ⚙️ Configuración Técnica

### Ubicación de Backups

**Por defecto:**
```
[Directorio de la aplicación]\Backups\
Ejemplo: C:\...\PROYECTO BIBLIOTECA ESCOLAR\View\UI\bin\Debug\Backups\
```

**Personalizada:**
- Usar botón "Examinar..." para cambiar carpeta destino
- El sistema crea la carpeta si no existe

### Permisos SQL Server

El usuario de SQL Server (con Integrated Security) debe tener:
```sql
GRANT BACKUP DATABASE TO [DOMINIO\Usuario]
GRANT RESTORE DATABASE TO [DOMINIO\Usuario]
```

Si usas autenticación SQL, el usuario debe tener rol `db_backupoperator` o `sysadmin`.

### Espacio en Disco

- El sistema muestra espacio disponible antes de crear backup
- Recomendado: Al menos 2x el tamaño de la BD mayor

### Timeouts

- **Backup**: 5 minutos (300 segundos)
- **Restore**: 10 minutos (600 segundos)

Estos valores están en `BackupRepository.cs` y pueden ajustarse si necesario.

---

## 🔒 Seguridad y Auditoría

### Registro en Bitácora

**Todas las operaciones se registran en `BitacoraSeguridad`:**

| Operación | TipoEvento | Gravedad |
|-----------|------------|----------|
| Backup creado | Backup | INFO |
| Backup fallido | Backup | ERROR |
| Restore iniciado | Restore | WARNING |
| Restore exitoso | Restore | INFO |
| Restore fallido | Restore | CRITICAL |
| Backup eliminado | Backup | INFO |

**Consultar bitácora:**
```sql
USE SeguridadBiblioteca;

SELECT * FROM BitacoraSeguridad
WHERE Accion LIKE '%Backup%' OR Accion LIKE '%Restore%'
ORDER BY Fecha DESC;
```

### Control de Permisos

- Solo usuarios con permiso "FrmGestionBackup" pueden acceder
- Por defecto: Solo **ROL_Administrador**
- Para dar permiso a otros roles:
  ```sql
  -- Ejemplo: Dar permiso a Bibliotecario
  DECLARE @IdRol INT, @IdPatente INT;
  SELECT @IdRol = IdFamilia FROM Familia WHERE NombreFamilia = 'ROL_Bibliotecario';
  SELECT @IdPatente = IdPatente FROM Patente WHERE NombrePatente = 'Gestión Backup';

  INSERT INTO FamiliaPatente (IdFamilia, IdPatente)
  VALUES (@IdRol, @IdPatente);
  ```

---

## 🛠️ Mantenimiento

### Limpieza de Backups Antiguos

**Manual:**
1. Abrir `FrmGestionBackup`
2. Seleccionar backups antiguos
3. Click "Eliminar Backup"

**Consulta SQL para identificar backups antiguos:**
```sql
SELECT IdBackup, FechaCreacion, NombreBaseDatos, NombreArchivo =
    REVERSE(SUBSTRING(REVERSE(RutaArchivo), 1, CHARINDEX('\', REVERSE(RutaArchivo))-1)),
    TamañoMB
FROM Backup
WHERE FechaCreacion < DATEADD(MONTH, -3, GETDATE()) -- Mayores a 3 meses
  AND Estado = 'Exitoso'
ORDER BY FechaCreacion;
```

### Estrategia de Backup Recomendada

**Producción:**
```
- Backup Full: Semanal (Domingos a las 2:00 AM)
- Backup Differential: Diario (Lunes-Sábado a las 2:00 AM)
- Retención: 3 meses
```

**Desarrollo:**
```
- Backup Full antes de:
  - Actualización de software
  - Cambios en esquema de BD
  - Promoción de alumnos
  - Fin de ciclo lectivo
```

---

## ❓ Troubleshooting

### Error: "No se puede abrir el archivo de backup"
**Causa**: Archivo en uso o permisos insuficientes
**Solución**:
- Cerrar todas las instancias de la aplicación
- Verificar permisos NTFS en la carpeta de backups

### Error: "No se puede poner la BD en modo SINGLE_USER"
**Causa**: Conexiones activas a la BD
**Solución**:
- El sistema automáticamente cierra conexiones
- Si persiste, ejecutar manualmente:
  ```sql
  USE master;
  ALTER DATABASE [NombreDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
  ALTER DATABASE [NombreDB] SET MULTI_USER;
  ```

### Error: "Timeout expired"
**Causa**: BD muy grande o servidor lento
**Solución**: Aumentar timeout en `BackupRepository.cs`:
```csharp
cmd.CommandTimeout = 600; // 10 minutos para backup
cmd.CommandTimeout = 1200; // 20 minutos para restore
```

### Backup se muestra como "En Proceso" indefinidamente
**Causa**: Error no capturado durante la operación
**Solución**:
```sql
-- Actualizar estado manualmente
UPDATE Backup
SET Estado = 'Fallido',
    MensajeError = 'Proceso interrumpido'
WHERE IdBackup = [ID_DEL_BACKUP]
  AND Estado = 'En Proceso';
```

---

## 📊 Estructura de Archivos Creados

```
PROYECTO BIBLIOTECA ESCOLAR/
├── Database/
│   ├── 16_CrearTablaBackup.sql          ← Script tabla Backup
│   ├── 17_AgregarPatentesBackup.sql     ← Script permisos
│   └── 00_EJECUTAR_TODO.sql             ← Actualizado (incluye pasos 12-13)
│
├── Model/DomainModel/
│   └── Backup.cs                         ← Entidad de dominio
│
├── Model/ServicesSeguridad/
│   ├── DAL/Implementations/
│   │   └── BackupRepository.cs           ← Repositorio (catálogo + física)
│   └── BLL/
│       └── BackupBLL.cs                  ← Lógica de negocio
│
├── View/UI/
│   ├── WinUi/Administración/
│   │   ├── FrmGestionBackup.cs           ← Formulario principal
│   │   ├── FrmGestionBackup.Designer.cs  ← Diseñador
│   │   ├── FrmGestionBackup.resx         ← Recursos
│   │   └── menu.cs                       ← Menú (modificado)
│   │   └── menu.Designer.cs              ← Menú diseñador (modificado)
│   │
│   └── Resources/I18n/
│       ├── idioma.es-AR                  ← Traducciones español
│       └── idioma.en-GB                  ← Traducciones inglés
│
└── INSTRUCCIONES_BACKUP.md               ← Este archivo
```

---

## 🎯 Próximos Pasos (Opcional)

### Mejoras Futuras

1. **Backup Automático Programado**
   - Implementar con `System.Timers.Timer`
   - Configuración en `App.config`

2. **Compresión de Backups**
   - SQL Server soporta: `WITH COMPRESSION`
   - Reduce tamaño hasta 60%

3. **Notificaciones por Email**
   - Al crear/fallar backups
   - Integrar con `System.Net.Mail`

4. **Validación de Backups**
   - Comando: `RESTORE VERIFYONLY`
   - Verificar integridad sin restaurar

5. **Exportación de Catálogo**
   - Exportar lista de backups a Excel/CSV
   - Usar `Microsoft.Office.Interop.Excel`

---

## 📞 Soporte

Para problemas o consultas:
1. Revisar bitácora de seguridad en BD
2. Verificar logs de SQL Server (`C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Log\`)
3. Consultar documentación de SQL Server BACKUP/RESTORE

---

## ✅ Checklist de Implementación

- [x] Scripts SQL creados y ejecutados
- [x] Tabla Backup creada en SeguridadBiblioteca
- [x] Patentes creadas y asignadas a Administrador
- [x] Entidad Backup en DomainModel
- [x] BackupRepository implementado
- [x] BackupBLL con validaciones
- [x] Interfaz FrmGestionBackup diseñada
- [x] Traducciones es-AR y en-GB
- [x] Integración con menú principal
- [x] Control de permisos
- [x] Registro en bitácora
- [x] Documentación completa

---

**Versión**: 1.0
**Fecha**: 2025-11-15
**Autor**: Sistema implementado para Biblioteca Escolar

---

¡El sistema de Backup y Restore está listo para usarse! 🎉
