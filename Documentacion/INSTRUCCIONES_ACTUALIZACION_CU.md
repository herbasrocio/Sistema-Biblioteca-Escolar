# Instrucciones para Actualizar la Especificación de Casos de Uso

## Objetivo
Reemplazar la sección "7. Gestión de Perfiles y Permisos" en el archivo `ESPECIFICACION_CASOS_USO.md` con la versión simplificada que refleja la **asignación automática de roles** implementada en el sistema.

---

## Archivos Creados

1. **`CU_GESTION_PERFILES_SIMPLIFICADO.md`**
   - Especificación completa y detallada de los casos de uso simplificados
   - Incluye todos los flujos, información técnica y diagramas de flujo
   - Formato profesional con referencias a código fuente

2. **`casos_uso_perfiles_simplificado.puml`**
   - Diagrama PlantUML de casos de uso simplificado
   - Muestra visualmente la asignación automática de roles
   - Incluye notas explicativas sobre los procesos clave

---

## Cambios Principales

### Antes (Versión Original)
La documentación incluía múltiples casos de uso para gestión manual de permisos:
- CU-015: Crear Familia (Rol)
- CU-015.1: Agregar Patente a Familia
- CU-015.2: Agregar Familia Hija a Familia Padre
- CU-015.3: Validar Estructura Jerárquica
- CU-016: Crear Patente
- CU-017: Asignar Familia a Usuario (manual)
- CU-017.1: Recalcular DVH de Usuario
- CU-017.2: Notificar Cambio de Permisos
- CU-018: Asignar Patente a Usuario
- CU-019: Quitar Permisos de Usuario
- CU-020: Consultar Permisos de Usuario
- CU-021: Verificar Permiso de Usuario

**Problema:** La documentación sugería que era necesario asignar roles manualmente después de crear un usuario, lo cual no refleja el comportamiento real del sistema.

### Después (Versión Simplificada)
La nueva documentación se enfoca en los casos de uso reales implementados:
- CU-015: Crear Usuario con Rol (con asignación automática)
- CU-015.1: Validar Datos de Usuario
- CU-016: Modificar Usuario y Cambiar Rol (con reasignación automática)
- CU-016.1: Recalcular DVH de Usuario
- CU-017: Consultar Usuarios y Roles
- CU-018: Verificar Permiso de Usuario
- CU-019: Eliminar Usuario
- CU-019.1: Validar Eliminación de Usuario
- CU-020: Buscar Usuario

**Mejoras:**
- ✅ Refleja el comportamiento real del sistema
- ✅ Enfocado en la gestión de usuarios (no en la arquitectura interna de permisos)
- ✅ Documenta la asignación automática de roles
- ✅ Incluye referencias exactas al código fuente (archivos, líneas, métodos)
- ✅ Más fácil de entender para usuarios finales y desarrolladores

---

## Opción 1: Reemplazo Completo de la Sección 7

### Pasos:
1. Abrir `ESPECIFICACION_CASOS_USO.md`
2. Localizar la línea 660: `# 7. Gestión de Perfiles y Permisos`
3. Eliminar todo el contenido desde la línea 660 hasta la línea 936 (antes de "## Notas Finales")
4. Copiar el contenido completo de `CU_GESTION_PERFILES_SIMPLIFICADO.md`
5. Pegar en lugar de la sección eliminada
6. Ajustar el título de "# Gestión de Perfiles y Permisos - Versión Simplificada" a "# 7. Gestión de Perfiles y Permisos"
7. Guardar el archivo

### Resultado:
- El índice en la línea 10 ya tiene la entrada correcta
- Los números de casos de uso cambian de CU-015 a CU-021 en la versión original a CU-015 a CU-020 en la simplificada
- Todas las referencias internas funcionarán correctamente

---

## Opción 2: Mantener Ambas Versiones

Si prefieres mantener la documentación de la arquitectura de permisos junto con los casos de uso de usuario:

### Pasos:
1. Renombrar la sección actual (línea 660) a:
   ```markdown
   # 7. Gestión de Perfiles y Permisos (Vista de Arquitectura)
   ```

2. Al final de la sección actual, agregar:
   ```markdown
   # 8. Gestión de Usuarios y Roles (Vista de Usuario)
   ```

3. Copiar el contenido de `CU_GESTION_PERFILES_SIMPLIFICADO.md` después del nuevo título

4. Actualizar el índice (línea 10) agregando:
   ```markdown
   7. [Gestión de Perfiles y Permisos (Vista de Arquitectura)](#7-gestión-de-perfiles-y-permisos-vista-de-arquitectura)
   8. [Gestión de Usuarios y Roles (Vista de Usuario)](#8-gestión-de-usuarios-y-roles-vista-de-usuario)
   ```

### Resultado:
- Dos secciones complementarias:
  - **Sección 7:** Cómo funciona internamente el sistema de permisos (para desarrolladores)
  - **Sección 8:** Cómo gestionar usuarios en el sistema (para administradores y usuarios finales)

---

## Opción 3: Solo Usar la Nueva Versión

Si prefieres usar solo la versión simplificada como documento independiente:

### Uso:
- Mantener `ESPECIFICACION_CASOS_USO.md` sin cambios
- Usar `CU_GESTION_PERFILES_SIMPLIFICADO.md` como documento separado
- Referenciar ambos documentos según el contexto:
  - Para entender la arquitectura → `ESPECIFICACION_CASOS_USO.md` sección 7
  - Para gestionar usuarios → `CU_GESTION_PERFILES_SIMPLIFICADO.md`

---

## Actualización del Diagrama PlantUML

Si decides reemplazar la sección, también deberías actualizar las referencias al diagrama:

### En `ESPECIFICACION_CASOS_USO.md`:
Cambiar la referencia del diagrama original:
```markdown
Ver diagrama: casos_uso_backup_permisos.puml
```

Por:
```markdown
Ver diagrama: casos_uso_perfiles_simplificado.puml
```

### Generar el diagrama:
Puedes visualizar el diagrama `.puml` usando:
- **PlantUML Online:** http://www.plantuml.com/plantuml/uml/
- **VS Code:** Extensión "PlantUML"
- **IntelliJ IDEA:** Plugin PlantUML
- **Comando:** `java -jar plantuml.jar casos_uso_perfiles_simplificado.puml`

---

## Verificación de Consistencia

Después de actualizar la documentación, verificar:

### 1. Referencias al código
Todos los casos de uso tienen referencias exactas:
- ✅ Archivo: `gestionUsuarios.cs`
- ✅ Clase: `UsuarioBLL`
- ✅ Métodos con número de línea
- ✅ Tablas de base de datos

### 2. Flujos de casos de uso
- ✅ CU-015 (Crear Usuario) menciona asignación automática en paso 8
- ✅ CU-016 (Modificar Usuario) menciona reasignación automática en paso 12
- ✅ CU-018 (Verificar Permiso) explica el patrón Composite correctamente

### 3. Diagramas
- ✅ El diagrama muestra "Asignar Rol Automáticamente" como <<include>>
- ✅ Las notas explican el proceso de asignación automática

### 4. Conceptos clave
- ✅ La sección inicial explica que el sistema asigna roles automáticamente
- ✅ Se documentan los 4 roles disponibles (Administrador, Docente, Bibliotecario, Ayudante)
- ✅ Se explica la arquitectura Composite para verificación de permisos

---

## Beneficios de la Actualización

### Para Usuarios Finales
- Documentación más clara de cómo gestionar usuarios
- Enfoque en las tareas reales que pueden realizar
- Menos confusión sobre conceptos técnicos internos

### Para Administradores
- Instrucciones paso a paso para crear y gestionar usuarios
- Flujos alternativos bien documentados
- Validaciones y restricciones claramente indicadas

### Para Desarrolladores
- Referencias exactas al código fuente
- Explicación de los patrones de diseño utilizados
- Estructura de base de datos documentada
- Flujos de asignación de roles explicados con diagramas

### Para Auditoría y Seguridad
- Documentación completa de la bitácora de seguridad
- Explicación del DVH (Dígito Verificador Horizontal)
- Niveles de gravedad de eventos documentados

---

## Recomendación

**Opción recomendada: Opción 1 (Reemplazo Completo)**

Razones:
1. La versión simplificada refleja el comportamiento real del sistema
2. Es más fácil de mantener (una sola fuente de verdad)
3. Evita confusión entre lo que "se puede hacer teóricamente" vs. "lo que está implementado"
4. Incluye toda la información técnica necesaria para desarrolladores
5. Mantiene el enfoque en casos de uso del usuario final

Si necesitas documentar la arquitectura interna de permisos (Familias, Patentes, relaciones), considera crear un documento separado:
- `ARQUITECTURA_PERMISOS.md` - Documentación técnica del sistema de permisos
- `ESPECIFICACION_CASOS_USO.md` - Casos de uso desde la perspectiva del usuario

---

## Próximos Pasos Sugeridos

Después de actualizar la documentación:

1. **Revisar y validar** que toda la información sea correcta
2. **Generar el diagrama** a partir del archivo `.puml`
3. **Actualizar el índice** si fue necesario
4. **Comunicar los cambios** al equipo de desarrollo y usuarios
5. **Archivar la versión anterior** con fecha para referencia histórica

---

**Fecha de creación:** 16 de Noviembre de 2025
**Responsable de la actualización:** [Tu nombre]
**Versión del sistema:** 1.0 Final
