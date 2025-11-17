# Cómo Importar Casos de Uso a Enterprise Architect

## 📋 Archivos Disponibles para Importación

En la carpeta `Documentacion/` tienes:

1. **PlantUML (`.puml`)** - Para importación directa si EA tiene soporte PlantUML
   - `casos_uso_seguridad.puml`
   - `casos_uso_backup_permisos.puml`
   - `diagrama_dominio.puml`

2. **CSV** - Para importación manual garantizada
   - `casos_uso_EA_import.csv` (60 casos de uso)
   - `actores_EA_import.csv` (5 actores)

3. **Markdown** - Para referencia
   - `ESPECIFICACION_CASOS_USO.md` (especificaciones detalladas)

---

## ⭐ MÉTODO 1: Importar desde CSV (MÁS FÁCIL Y GARANTIZADO)

### Ventajas:
- ✅ Funciona en TODAS las versiones de EA
- ✅ No requiere plugins
- ✅ Proceso simple y directo

### Pasos:

#### A. Importar Actores

1. Abre Enterprise Architect
2. Crea o abre tu proyecto `.eap`
3. En el **Project Browser**, crea un nuevo paquete llamado **"Actores"**
4. Click derecho en el paquete → **Import/Export → Import CSV**
5. Selecciona el archivo `actores_EA_import.csv`
6. En el diálogo de importación:
   - Type: `Actor`
   - Marca la opción **"Strip Quotes"**
   - Click **OK**
7. Los 5 actores se importarán automáticamente

#### B. Importar Casos de Uso

1. En el **Project Browser**, crea un nuevo paquete llamado **"Casos de Uso"**
2. Dentro, crea los siguientes sub-paquetes:
   - Gestión de Login y Logout
   - Gestión de Idiomas
   - Criptografía
   - Bitácora
   - Gestión de Excepciones
   - Backup y Restore
   - Gestión de Perfiles y Permisos

3. Selecciona el paquete raíz **"Casos de Uso"**
4. Click derecho → **Import/Export → Import CSV**
5. Selecciona `casos_uso_EA_import.csv`
6. En el diálogo:
   - Type: `UseCase`
   - Marca **"Strip Quotes"**
   - Marca **"Create Package Structure"** (para que use la columna Package)
   - Click **OK**
7. ¡60 casos de uso importados!

#### C. Crear Diagramas de Casos de Uso

1. Click derecho en el paquete "Gestión de Login y Logout"
2. **Add Diagram → Use Case Diagram**
3. Arrastra los casos de uso del Project Browser al diagrama
4. Arrastra los actores al diagrama
5. Dibuja las relaciones:
   - Asociaciones (líneas) entre actores y casos de uso
   - `<<include>>` y `<<extend>>` según la especificación

**Repetir para cada paquete.**

---

## 🔧 MÉTODO 2: Importar desde PlantUML (Requiere Plugin)

### Requisitos:
- Enterprise Architect versión 15 o superior
- MDG Technology para PlantUML instalado

### Pasos:

#### A. Instalar Soporte PlantUML

1. En EA, ve a **Extensions → MDG Technology Import**
2. Descarga el MDG de PlantUML (si no está instalado)
3. Importa el archivo MDG
4. Reinicia EA

#### B. Importar Archivos .puml

**Opción 2A - Si EA puede abrir .puml directamente:**
1. **File → Open**
2. Selecciona `casos_uso_seguridad.puml`
3. EA creará el modelo automáticamente

**Opción 2B - Convertir a XMI primero:**
1. Ve a http://www.plantuml.com/plantuml/uml/
2. Pega el contenido de `casos_uso_seguridad.puml`
3. Descarga como XMI
4. En EA: **File → Import Model → Import XMI 1.1**
5. Selecciona el archivo XMI descargado

**Repetir para los 3 archivos .puml**

---

## 🔄 MÉTODO 3: Usar PlantUML CLI (Para usuarios avanzados)

### Requisitos:
- Java JRE instalado
- `plantuml.jar` descargado de https://plantuml.com/download

### Comandos en PowerShell:

```powershell
# Navegar a la carpeta Documentacion
cd Documentacion

# Convertir todos los .puml a XMI
java -jar plantuml.jar -txmi casos_uso_seguridad.puml
java -jar plantuml.jar -txmi casos_uso_backup_permisos.puml
java -jar plantuml.jar -txmi diagrama_dominio.puml
```

Esto generará archivos `.xmi` que puedes importar:
- **File → Import Model → Import XMI 1.1** en EA

---

## 📊 MÉTODO 4: Importación Manual (Control Total)

Si prefieres crear todo manualmente pero usando la documentación como guía:

1. Abre `ESPECIFICACION_CASOS_USO.md`
2. Crea cada actor manualmente en EA
3. Crea cada caso de uso siguiendo la especificación:
   - Usa el campo **Notes** para copiar el flujo principal
   - Usa **Alternate Flows** para los flujos alternativos
   - Agrega **Tagged Values** para información específica

**Ventaja:** Control total del modelo
**Desventaja:** Toma más tiempo

---

## 🎯 Estructura Recomendada en EA

```
📦 Sistema Biblioteca Escolar
├─ 📂 Actores
│  ├─ 👤 Usuario Anónimo
│  ├─ 👤 Bibliotecario
│  ├─ 👤 Administrador
│  ├─ 👤 Sistema
│  └─ 👤 Docente
│
├─ 📂 Casos de Uso
│  ├─ 📂 Gestión de Login y Logout
│  │  ├─ UC-001: Iniciar Sesión
│  │  ├─ UC-001.1: Validar Credenciales
│  │  ├─ UC-001.2: Cargar Permisos
│  │  └─ UC-002: Cerrar Sesión
│  │
│  ├─ 📂 Gestión de Idiomas
│  │  ├─ UC-003: Cambiar Idioma
│  │  ├─ UC-003.1: Obtener Traducciones
│  │  └─ UC-003.2: Notificar Cambio
│  │
│  ├─ 📂 Criptografía
│  │  ├─ UC-004: Hash de Contraseña
│  │  ├─ UC-005: Validar Contraseña
│  │  └─ UC-006: Calcular DVH
│  │
│  ├─ 📂 Bitácora
│  │  ├─ UC-007: Registrar Evento Seguridad
│  │  ├─ UC-008: Registrar Operación
│  │  └─ UC-009: Consultar Bitácora
│  │
│  ├─ 📂 Gestión de Excepciones
│  │  ├─ UC-010: Capturar Excepción
│  │  └─ UC-011: Verificar Integridad
│  │
│  ├─ 📂 Backup y Restore
│  │  ├─ UC-012: Crear Backup
│  │  ├─ UC-013: Restaurar Backup
│  │  └─ UC-014: Consultar Catálogo
│  │
│  └─ 📂 Gestión de Perfiles y Permisos
│     ├─ UC-015: Crear Familia
│     ├─ UC-016: Crear Patente
│     ├─ UC-017: Asignar Familia a Usuario
│     ├─ UC-018: Asignar Patente a Usuario
│     ├─ UC-019: Quitar Permisos
│     ├─ UC-020: Consultar Permisos
│     └─ UC-021: Verificar Permiso
│
└─ 📂 Modelo de Dominio
   ├─ 📂 DomainModel - Biblioteca
   ├─ 📂 Services.DomainModel.Security
   └─ 📂 Auditoría y Backup
```

---

## 💡 Tips para Enterprise Architect

### 1. **Personalizar Propiedades de Casos de Uso**

Después de importar, puedes agregar:
- **Precondiciones:** Element Properties → Requirements → Pre-conditions
- **Postcondiciones:** Element Properties → Requirements → Post-conditions
- **Notas:** Ya vienen en el campo Notes desde el CSV

### 2. **Crear Matriz de Trazabilidad**

EA puede generar automáticamente:
- **View → Traceability → Relationship Matrix**
- Muestra qué actores usan qué casos de uso

### 3. **Generar Documentación**

EA puede exportar todo a:
- **Publish → Publish as... → RTF/Word**
- **Publish → HTML Report**
- **Publish → PDF** (requiere plugin)

### 4. **Validación del Modelo**

- **Tools → Validate → Validate Current Package**
- Verifica relaciones y estructura

---

## ✅ Verificación Post-Importación

Después de importar, verifica:

- [ ] 5 actores importados
- [ ] 60 casos de uso importados
- [ ] Casos de uso organizados en 7 paquetes
- [ ] Propiedades (Complexity, Priority, Status) asignadas
- [ ] Notas con descripciones incluidas

---

## 🆘 Solución de Problemas

### Problema: "CSV format incorrect"
**Solución:** Abre el CSV en un editor de texto y verifica que use comas como separador.

### Problema: "Cannot import XMI"
**Solución:** Usa el Método 1 (CSV) que es más confiable.

### Problema: "PlantUML not supported"
**Solución:** Actualiza EA a versión 15+ o usa Método 1 (CSV).

### Problema: "Duplicate elements"
**Solución:** Limpia el paquete antes de reimportar.

---

## 📞 Recursos Adicionales

- **Especificaciones completas:** Ver `ESPECIFICACION_CASOS_USO.md`
- **Diagramas PlantUML:** Abrir `.puml` en editor PlantUML online
- **Modelo de dominio:** Ver `diagrama_dominio.puml`

---

## 🎓 Recomendación Final

**Para la mayoría de usuarios:** Usa **MÉTODO 1 (CSV)** porque:
- ✅ Es el más simple
- ✅ Funciona en todas las versiones de EA
- ✅ No requiere software adicional
- ✅ Importa todos los datos estructurados

Una vez importado en EA, puedes:
1. Crear los diagramas visuales
2. Agregar detalles adicionales
3. Generar documentación profesional
4. Exportar a diferentes formatos

**¡Listo para usar en tu proyecto de EA!** 🎉
