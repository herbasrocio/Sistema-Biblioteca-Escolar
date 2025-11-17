# CU 11 - PROMOCIÓN DE ALUMNOS

## Descripción General

La Promoción de Alumnos es una operación administrativa de fin de año lectivo que permite avanzar a los estudiantes al siguiente grado. Esta operación puede realizarse de manera masiva (todos los grados automáticamente) o selectiva (por grado y división específicos).

### Características Principales

- ✅ Promoción masiva de todos los grados en un solo paso
- ✅ Promoción selectiva por grado y división
- ✅ Finaliza inscripciones del año actual
- ✅ Crea nuevas inscripciones para el año siguiente
- ✅ Mantiene historial completo de inscripciones
- ✅ Doble confirmación para operaciones masivas
- ✅ Registro detallado en bitácora

### Momento de Ejecución

**Una vez al año** - Al finalizar el ciclo lectivo (diciembre/febrero)

### Actor Principal

**Administrador** - Requiere máximos permisos del sistema

---

## Casos de Uso del Módulo

- **CU-11.1:** Promocionar Alumnos por Grado
- **CU-11.2:** Promoción Masiva de Todos los Grados
- **CU-11.3:** Consultar Estadísticas de Promoción

---

## CU-11.1: Promocionar Alumnos por Grado

**Actor Principal:** Administrador

**Precondiciones:**
- El usuario tiene permisos de administrador
- Existen alumnos inscriptos en el año lectivo actual
- Se ha finalizado el ciclo lectivo

**Postcondiciones:**
- Las inscripciones del grado origen han sido finalizadas
- Se han creado nuevas inscripciones en el grado destino
- Se ha registrado la operación en bitácora

### Flujo Principal:

1. El administrador accede a "Promoción de Alumnos"
2. El sistema muestra el formulario de promoción con dos secciones:
   - Promoción por grado (individual)
   - Promoción masiva (todos)
3. El sistema carga y muestra estadísticas actuales:
   - Cantidad de alumnos por grado y división
   - Total de alumnos inscriptos en el año actual
4. El administrador selecciona la sección "Promoción por Grado"
5. El administrador configura los parámetros:
   - **Año actual:** [por defecto: año en curso]
   - **Año siguiente:** [por defecto: año en curso + 1]
   - **Grado actual:** 1° a 7° (ComboBox)
   - **División actual:** A, B, C, etc. (opcional - si vacío: todas las divisiones)
   - **Grado nuevo:** 1° a 7° (ComboBox)
   - **División nueva:** (opcional - si vacío: mantiene división actual)
6. El administrador hace clic en "Promocionar Grado"
7. El sistema valida los parámetros (CU-11.1.1):
   - Grado actual y nuevo están seleccionados
   - Año siguiente > año actual
   - Existen alumnos en el grado/división origen
8. El sistema muestra diálogo de confirmación:
   - "¿Está seguro de promocionar a los alumnos de [grado] [división] del año [actual] al grado [nuevo] [división] del año [siguiente]?"
9. El administrador confirma
10. El sistema inicia el proceso de promoción (CU-11.1.2):
    - Obtiene todas las inscripciones del grado/división origen
    - Finaliza cada inscripción del año actual
    - Crea nueva inscripción en el grado/división destino
    - Actualiza estado del alumno
11. El sistema registra en bitácora con:
    - Módulo: "Administración Escolar"
    - Acción: "Promoción de alumnos por grado"
    - Detalle: Grado origen, grado destino, cantidad de alumnos
    - Gravedad: "Alto"
12. El sistema muestra resultado:
    - Cantidad de alumnos promovidos
    - Cantidad de alumnos egresados (si grado origen = 7°)
    - Total procesados
13. El sistema actualiza las estadísticas en pantalla
14. El sistema muestra mensaje de éxito

### Flujos Alternativos:

**7a. Parámetros inválidos:**
- El sistema muestra mensaje de error específico
- Retorna al paso 5

**7b. No hay alumnos en el grado origen:**
- El sistema muestra "No se encontraron alumnos en [grado] [división]"
- Retorna al paso 5

**9a. Usuario cancela:**
- No se realizan cambios
- Fin del caso de uso

**10a. Error durante el proceso:**
- El sistema revierte cambios (si es posible)
- El sistema registra error en bitácora
- El sistema muestra mensaje de error detallado
- Fin del caso de uso

### Información Específica:

- **Formulario:** `View/UI/WinUi/Administración/gestionPromocionAlumnos.cs`
- **Evento botón:** `BtnPromocionarGrado_Click` (línea 199)
- **Clase BLL:** `Model/BLL/InscripcionBLL.cs`
- **Método principal:** `PromocionarAlumnosPorGrado(int anioActual, int anioSiguiente, string gradoActual, string divisionActual, string gradoNuevo, string divisionNueva)` (línea 155)
- **Entidad resultado:** `ResultadoPromocion` con propiedades:
  - `Exitoso`: bool
  - `Mensaje`: string
  - `AlumnosPromovidos`: int
  - `Egresados`: int
  - `AlumnosFinalizados`: int
- **Tablas afectadas:**
  - `Inscripcion` (tabla principal)
  - `Alumno` (se actualiza estado si egresa)
  - `BitacoraOperaciones`

### Validaciones Clave:

- Grado actual y nuevo obligatorios (línea 165-166)
- Año siguiente > año actual (línea 168-169)
- Existen alumnos en grado origen (línea 177-180)

---

## CU-11.1.1: Validar Parámetros de Promoción

**Actor Principal:** Sistema

**Precondiciones:** El administrador ha configurado los parámetros de promoción

### Flujo Principal:

1. El sistema valida que grado actual esté seleccionado
2. El sistema valida que grado nuevo esté seleccionado
3. El sistema valida que año siguiente sea mayor a año actual
4. El sistema obtiene inscripciones del grado/división origen
5. Si no hay inscripciones, lanza excepción `ValidacionException`
6. Si todas las validaciones pasan, retorna verdadero

### Información Específica:

- **Ubicación:** Líneas 165-180 de `InscripcionBLL.cs`
- **Validaciones:**
  - `ValidationBLL.ValidarCampoRequerido(gradoActual, "Grado Actual")`
  - `ValidationBLL.ValidarCampoRequerido(gradoNuevo, "Grado Nuevo")`
  - `anioSiguiente > anioActual`
  - `inscripciones.Count > 0`

---

## CU-11.1.2: Ejecutar Proceso de Promoción Individual

**Actor Principal:** Sistema

**Precondiciones:** Se han validado los parámetros de promoción

### Flujo Principal:

1. El sistema obtiene todas las inscripciones del grado/división origen
2. Para cada inscripción:
   - Actualiza campo `Estado` = "Finalizado"
   - Actualiza campo `FechaFinalizacion` = fecha actual
3. El sistema crea nuevas inscripciones para cada alumno:
   - IdAlumno: mismo alumno
   - AnioLectivo: año siguiente
   - Grado: grado nuevo
   - Division: división nueva (o mantiene actual si no se especificó)
   - Estado: "Activo"
   - FechaInscripcion: fecha actual
4. Si grado nuevo = 7° y se está promoviendo de 7° a "EGRESADO":
   - Actualiza Alumno.Estado = "Egresado"
   - NO crea nueva inscripción
   - Incrementa contador de egresados
5. El sistema cuenta los resultados:
   - Alumnos promovidos (con nueva inscripción)
   - Alumnos egresados (sin nueva inscripción)
   - Total finalizados
6. El sistema retorna objeto `ResultadoPromocion` con los contadores

### Información Específica:

- **Ubicación:** Líneas 182-220 de `InscripcionBLL.cs`
- **Repositorio:** `InscripcionRepository`
- **Métodos utilizados:**
  - `FinalizarInscripcion(Guid idInscripcion)`
  - `CrearInscripcion(Inscripcion nuevaInscripcion)`
- **Lógica especial egresados:** Si grado origen = "7", no crea inscripción sino que marca como egresado

---

## CU-11.2: Promoción Masiva de Todos los Grados

**Actor Principal:** Administrador

**Precondiciones:**
- El usuario tiene permisos de administrador
- Existen alumnos inscriptos en el año actual
- Se ha finalizado el ciclo lectivo
- **IMPORTANTE:** Se recomienda realizar backup previo

**Postcondiciones:**
- TODOS los alumnos han sido promovidos al siguiente grado
- Las inscripciones del año actual han sido finalizadas
- Se han creado inscripciones para el año siguiente
- Los alumnos de 7° grado han egresado

### Flujo Principal:

1. El administrador accede a "Promoción de Alumnos"
2. El sistema muestra el formulario con estadísticas actuales
3. El administrador configura:
   - **Año actual:** [por defecto: año en curso]
   - **Año siguiente:** [por defecto: año en curso + 1]
4. El administrador hace clic en "Promoción Masiva de Todos los Grados"
5. El sistema muestra **primera confirmación** con advertencia:
   ```
   ATENCIÓN: Esta operación promocionará TODOS los alumnos del año [actual]
   al año [siguiente] según el siguiente esquema:

   1° → 2°
   2° → 3°
   3° → 4°
   4° → 5°
   5° → 6°
   6° → 7°
   7° → EGRESADOS

   Esta operación NO se puede deshacer. ¿Desea continuar?
   ```
6. El administrador confirma primera vez
7. El sistema muestra **segunda confirmación**:
   - "¿Está completamente seguro? Esta es su última oportunidad para cancelar."
8. El administrador confirma segunda vez
9. El sistema cambia cursor a "Espera" (reloj de arena)
10. El sistema valida:
    - Año siguiente > año actual
    - Existen inscripciones activas en el año actual
11. El sistema ejecuta promoción masiva (CU-11.2.1):
    - Define mapeo automático de grados (1→2, 2→3, ..., 7→EGRESADO)
    - Obtiene todas las inscripciones activas del año actual
    - Agrupa por grado
    - Promociona cada grupo al siguiente grado
12. El sistema registra en bitácora con:
    - Módulo: "Administración Escolar"
    - Acción: "Promoción masiva de alumnos"
    - Detalle: Total procesados, promovidos, egresados
    - Gravedad: "Alto"
13. El sistema restaura cursor normal
14. El sistema muestra resultado detallado:
    ```
    Promoción Masiva Completada Exitosamente

    Alumnos promovidos: [N]
    Egresados: [M]
    Total procesados: [T]
    ```
15. El sistema actualiza las estadísticas en pantalla
16. El sistema muestra mensaje de éxito

### Flujos Alternativos:

**6a. Usuario cancela en primera confirmación:**
- No se realizan cambios
- Fin del caso de uso

**8a. Usuario cancela en segunda confirmación:**
- No se realizan cambios
- Fin del caso de uso

**10a. No hay inscripciones activas:**
- El sistema muestra "No se encontraron inscripciones activas para el año [actual]"
- Fin del caso de uso

**11a. Error durante el proceso masivo:**
- El sistema intenta revertir cambios
- El sistema registra error crítico en bitácora
- El sistema muestra mensaje de error detallado con stack trace
- El sistema recomienda verificar estado de las inscripciones
- Fin del caso de uso

### Información Específica:

- **Evento botón:** `BtnPromocionMasiva_Click` (línea 290)
- **Método principal:** `PromocionarTodosLosAlumnos(int anioActual, int anioSiguiente)` (línea 230 de `InscripcionBLL.cs`)
- **Mapeo de grados:**
  ```csharp
  Dictionary<string, string> mapeoGrados = new Dictionary<string, string>
  {
      { "1", "2" },
      { "2", "3" },
      { "3", "4" },
      { "4", "5" },
      { "5", "6" },
      { "6", "7" },
      { "7", "EGRESADO" }
  };
  ```
- **Doble confirmación:** Líneas 306-325
- **Cursor de espera:** Líneas 328-330 (Cursor = Cursors.WaitCursor)

### Recomendaciones de Seguridad:

⚠️ **CRÍTICO:**
1. Realizar backup de la base de datos ANTES de ejecutar
2. Verificar que no haya préstamos pendientes de devolución
3. Generar reporte de alumnos pre-promoción
4. Ejecutar en horario sin usuarios activos
5. Tener plan de rollback preparado

---

## CU-11.2.1: Ejecutar Promoción Masiva Automática

**Actor Principal:** Sistema

**Precondiciones:** El administrador ha confirmado la promoción masiva

### Flujo Principal:

1. El sistema define el mapeo automático de grados (1→2, 2→3, etc.)
2. El sistema obtiene todas las inscripciones activas del año actual
3. El sistema agrupa inscripciones por grado
4. Para cada grado (del 1° al 7°):
   - Obtiene el grado destino del mapeo
   - Si grado origen = "7":
     - Marca alumnos como "Egresado"
     - NO crea nuevas inscripciones
     - Incrementa contador de egresados
   - Si grado origen ≠ "7":
     - Finaliza inscripciones del año actual
     - Crea nuevas inscripciones en grado destino
     - Mantiene la división original
     - Incrementa contador de promovidos
5. El sistema suma todos los contadores
6. El sistema retorna `ResultadoPromocion` con totales

### Información Específica:

- **Ubicación:** Líneas 240-290 de `InscripcionBLL.cs`
- **Procesamiento:** Por grado, manteniendo divisiones
- **Optimización:** Agrupa operaciones por grado para mejorar performance
- **Transaction:** Se recomienda ejecutar dentro de una transacción

---

## CU-11.3: Consultar Estadísticas de Promoción

**Actor Principal:** Administrador

**Precondiciones:** Existen inscripciones en el sistema

**Postcondiciones:** Se muestran las estadísticas actualizadas

### Flujo Principal:

1. El administrador accede a "Promoción de Alumnos"
2. El sistema carga automáticamente las estadísticas al abrir
3. El sistema consulta la base de datos:
   - Agrupa inscripciones por año lectivo
   - Filtra por año seleccionado
   - Agrupa por grado y división
   - Cuenta cantidad de alumnos por grupo
4. El sistema muestra en DataGridView:
   - Grado
   - División
   - Cantidad de Alumnos
5. El sistema muestra resumen:
   - "Total de alumnos inscriptos en [año]: [total]"
6. El administrador puede hacer clic en "Cargar Estadísticas" para refrescar

### Información Específica:

- **Método carga:** `CargarEstadisticas()` (línea 175)
- **Método BLL:** `ObtenerEstadisticasPorAnio(int anio)`
- **Evento botón:** `BtnCargarEstadisticas_Click` (línea 170)
- **DataGridView:** Configurado con selección de fila completa, sin edición

---

## Estructura de Datos

### Tabla Inscripcion

```sql
CREATE TABLE Inscripcion (
    IdInscripcion UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdAlumno UNIQUEIDENTIFIER NOT NULL,
    AnioLectivo INT NOT NULL,
    Grado NVARCHAR(10) NOT NULL,
    Division NVARCHAR(10) NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activo', -- 'Activo', 'Finalizado'
    FechaInscripcion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaFinalizacion DATETIME NULL,
    Observaciones NVARCHAR(MAX) NULL,

    CONSTRAINT FK_Inscripcion_Alumno FOREIGN KEY (IdAlumno)
        REFERENCES Alumno(IdAlumno)
)

-- Índices para optimización
CREATE INDEX IX_Inscripcion_AnioLectivo ON Inscripcion(AnioLectivo)
CREATE INDEX IX_Inscripcion_Estado ON Inscripcion(Estado)
CREATE INDEX IX_Inscripcion_Grado ON Inscripcion(Grado, Division)
```

### Clase ResultadoPromocion

```csharp
public class ResultadoPromocion
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; }
    public int AlumnosPromovidos { get; set; }
    public int Egresados { get; set; }
    public int AlumnosFinalizados { get; set; }
}
```

---

## Flujo de Datos - Promoción Individual

```
ANTES DE LA PROMOCIÓN:

Año 2024:
  3° A → Juan, María, Pedro (Estado: Activo)
  3° B → Ana, Luis (Estado: Activo)

DESPUÉS DE PROMOCIONAR 3° A → 4° A:

Año 2024:
  3° A → Juan, María, Pedro (Estado: Finalizado, FechaFin: 15/12/2024)
  3° B → Ana, Luis (Estado: Activo) [SIN CAMBIOS]

Año 2025:
  4° A → Juan, María, Pedro (Estado: Activo, FechaInscripción: 15/12/2024) [NUEVAS]
```

---

## Flujo de Datos - Promoción Masiva

```
ANTES DE LA PROMOCIÓN MASIVA:

Año 2024:
  1° A → 5 alumnos (Activo)
  2° A → 8 alumnos (Activo)
  ...
  7° A → 12 alumnos (Activo)
  Total: 120 alumnos

DESPUÉS DE LA PROMOCIÓN MASIVA:

Año 2024:
  1° A → 5 alumnos (Finalizado)
  2° A → 8 alumnos (Finalizado)
  ...
  7° A → 12 alumnos (Finalizado)

Año 2025:
  2° A → 5 alumnos (Activo) [de 1° A]
  3° A → 8 alumnos (Activo) [de 2° A]
  ...
  [NO hay 8° grado]

Alumnos:
  12 alumnos de 7° A → Estado: "Egresado"
```

---

## Consideraciones Especiales

### 1. Operación Irreversible

La promoción NO se puede deshacer automáticamente porque:
- Modifica múltiples registros de inscripción
- Cambia estados de alumnos a "Egresado"
- No hay función de rollback implementada

**Solución:** Backup de base de datos antes de ejecutar

### 2. Validación de Préstamos

**Recomendación:** Antes de promocionar, verificar que no haya:
- Préstamos activos sin devolver
- Sanciones pendientes
- Deudas de biblioteca

### 3. Rendimiento

Para escuelas grandes (500+ alumnos):
- La promoción masiva puede tardar varios segundos
- Se muestra cursor de espera durante el proceso
- Se recomienda ejecutar en horario sin usuarios

### 4. Alumnos Repitentes

**Caso especial:** Si un alumno repite año:
- NO promocionarlo automáticamente con promoción masiva
- Usar promoción individual para mantenerlo en el mismo grado
- O crear nueva inscripción manualmente en el mismo grado

### 5. Divisiones

- La promoción individual permite cambiar división
- La promoción masiva mantiene las divisiones originales
- Si se necesita reorganizar divisiones, hacerlo manualmente después

---

## Resumen de Permisos

| Caso de Uso | Administrador | Bibliotecario | Docente |
|-------------|---------------|---------------|---------|
| CU-11.1 Promoción Individual | ✓ | ✗ | ✗ |
| CU-11.2 Promoción Masiva | ✓ | ✗ | ✗ |
| CU-11.3 Consultar Estadísticas | ✓ | ✓ (solo lectura) | ✗ |

---

## Checklist Pre-Promoción

Antes de ejecutar la promoción masiva, verificar:

- [ ] Backup de base de datos realizado
- [ ] Todos los préstamos devueltos
- [ ] Reportes de fin de año generados
- [ ] Bitácora de operaciones revisada
- [ ] No hay usuarios activos en el sistema
- [ ] Plan de rollback preparado
- [ ] Año lectivo siguiente configurado
- [ ] Comunicación a docentes enviada

---

**Última actualización:** 16 de Noviembre de 2025
**Versión:** 1.0
**Criticidad:** ALTA - Operación irreversible
**Frecuencia:** Una vez al año (fin de ciclo lectivo)
