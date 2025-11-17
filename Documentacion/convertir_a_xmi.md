# Guía: Convertir PlantUML a formato Enterprise Architect

## Método 1: Usando PlantUML Online + Exportar a XMI

### Pasos:
1. Ve a http://www.plantuml.com/plantuml/uml/
2. Pega el contenido de `casos_uso_seguridad.puml`
3. En el menú superior, selecciona **"XMI"**
4. Descarga el archivo `.xmi`
5. En Enterprise Architect:
   - **File → Import Model → Import XMI**
   - Selecciona el archivo descargado

**Repetir para:**
- `casos_uso_backup_permisos.puml`
- `diagrama_dominio.puml`

---

## Método 2: Usando PlantUML CLI (si tienes Java instalado)

### Requisitos:
- Java instalado
- Descargar `plantuml.jar` de https://plantuml.com/download

### Comandos:

```bash
# Convertir a XMI
java -jar plantuml.jar -txmi casos_uso_seguridad.puml
java -jar plantuml.jar -txmi casos_uso_backup_permisos.puml
java -jar plantuml.jar -txmi diagrama_dominio.puml
```

Esto generará archivos `.xmi` que puedes importar a EA.

---

## Método 3: Importar manualmente usando CSV

Si XMI no funciona, EA puede importar desde CSV.

Voy a generar archivos CSV para ti.

---

## Método 4: MDG Technology para PlantUML

### Pasos:
1. En Enterprise Architect, ve a **Extensions → MDG Technology Import**
2. Descarga e importa el MDG de PlantUML
3. Luego podrás abrir archivos .puml directamente

---

## Recomendación:

**MÉTODO 1** es el más simple si no tienes Java instalado.
**MÉTODO 2** es el más rápido si tienes Java.

---

## Nota Importante:

Si ningún método funciona automáticamente, puedo generar para ti:
1. ✅ Archivos XMI nativos de EA
2. ✅ Script CSV para importación
3. ✅ Archivos de EA Project Browser

¿Qué método prefieres que prepare?
