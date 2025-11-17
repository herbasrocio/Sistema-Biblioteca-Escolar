# Índice de Casos de Uso - Sistema Biblioteca Escolar

## Organización de Módulos

---

## CU 05 - Gestor de Catálogo (Materiales)

**Descripción:** Gestión de la información bibliográfica de los materiales (fichas de catálogo)

### Casos de Uso:
- **CU-05.1:** Registrar Material
- **CU-05.2:** Consultar Material
- **CU-05.3:** Modificar Material
- **CU-05.4:** Eliminar Material

**Entidad principal:** Material (título, autor, editorial, ISBN, tipo, género)

**Documentación detallada:** Ver archivo `CU_05_06_07_08_09_COMPLETO.md`

---

## CU 06 - Gestor de Inventario (Ejemplares)

**Descripción:** Gestión de las copias físicas individuales de cada material

### Casos de Uso:
- **CU-06.1:** Agregar Ejemplar
- **CU-06.2:** Editar Ejemplar (incluye cambio de estado)
- **CU-06.3:** Eliminar Ejemplar

**Entidad principal:** Ejemplar (número, código de barras, ubicación, estado)

**Relación:** Un Material (CU-05) tiene muchos Ejemplares (CU-06)

**Documentación detallada:** Ver archivo `CU_06_GESTOR_INVENTARIO.md` ✨ NUEVO

---

## CU 07 - Gestor de Usuarios

**Descripción:** Administración de usuarios del sistema y sus roles

### Casos de Uso:
- **CU-07.1:** Crear Usuario
- **CU-07.2:** Modificar Usuario
- **CU-07.3:** Eliminar Usuario
- **CU-07.4:** Consultar Usuarios
- **CU-07.5:** Buscar Usuario
- **CU-07.6:** Cambiar Rol de Usuario

**Entidad principal:** Usuario (nombre, email, contraseña, rol)

**Características especiales:** Asignación automática de roles, Hash SHA-256, DVH

**Documentación detallada:** Ver archivo `CU_05_06_07_08_09_COMPLETO.md`

---

## CU 08 - Gestor de Alumnos

**Descripción:** Administración de estudiantes que utilizan la biblioteca

### Casos de Uso:
- **CU-08.1:** Registrar Alumno
- **CU-08.2:** Modificar Alumno
- **CU-08.3:** Eliminar Alumno
- **CU-08.4:** Consultar Alumnos
- **CU-08.5:** Buscar Alumno por DNI
- **CU-08.6:** Promocionar Alumnos (cambio de año lectivo)

**Entidad principal:** Alumno (nombre, apellido, DNI, grado, división)

**Documentación detallada:** Ver archivo `CU_05_06_07_08_09_COMPLETO.md`

---

## CU 09 - Préstamos y Devoluciones

**Descripción:** Gestión del circuito completo de préstamos de materiales

### Casos de Uso:
- **CU-09.1:** Registrar Préstamo
- **CU-09.2:** Registrar Devolución
- **CU-09.3:** Renovar Préstamo
- **CU-09.4:** Consultar Préstamos Activos
- **CU-09.5:** Consultar Préstamos Atrasados
- **CU-09.6:** Consultar Historial de Préstamos

**Entidades principales:** Prestamo, Devolucion, RenovacionPrestamo

**Características especiales:** Transacciones (Unit of Work), validación de estados

**Documentación detallada:** Ver archivo `CU_05_06_07_08_09_COMPLETO.md`

---

## CU 10 - Gestor de Reportes

**Descripción:** Generación de reportes estadísticos y exportación de datos

### Casos de Uso:
- **CU-10.1:** Generar Reporte de Préstamos Activos
- **CU-10.2:** Generar Reporte de Materiales Más Prestados
- **CU-10.3:** Generar Reporte de Uso por Grado
- **CU-10.4:** Generar Reporte de Inventario
- **CU-10.5:** Exportar Reporte a CSV

**Servicio principal:** ReporteBLL, ExportService

**Características especiales:** Filtros dinámicos, formato condicional, exportación CSV

**Documentación detallada:** Ver archivo `CU_05_06_07_08_09_COMPLETO.md`

---

## Módulo de Seguridad (Transversal)

**Descripción:** Configuración de arquitectura de seguridad del sistema

### Casos de Uso:
- Crear Rol
- Asignar Permisos a Rol
- Eliminar Rol
- Consultar Permisos de Rol

**Documentación detallada:** Ver archivo `CU_GESTION_ROLES_PERMISOS.md`

---

## Resumen Estadístico

| Módulo | Casos de Uso | Actores | Archivo Documentación |
|--------|--------------|---------|----------------------|
| CU 05 - Catálogo | 4 | Admin, Biblio, Docente | CU_05_06_07_08_09_COMPLETO.md |
| **CU 06 - Inventario** | **3** | **Admin, Biblio** | **CU_06_GESTOR_INVENTARIO.md** ✨ |
| CU 07 - Usuarios | 6 | Admin | CU_05_06_07_08_09_COMPLETO.md |
| CU 08 - Alumnos | 6 | Admin, Biblio, Docente | CU_05_06_07_08_09_COMPLETO.md |
| CU 09 - Préstamos | 6 | Admin, Biblio | CU_05_06_07_08_09_COMPLETO.md |
| CU 10 - Reportes | 5 | Admin, Biblio, Docente | CU_05_06_07_08_09_COMPLETO.md |
| Seguridad | 4 | Admin | CU_GESTION_ROLES_PERMISOS.md |
| **TOTAL** | **34** | **3 roles** | **3 archivos** |

---

## Archivos de Documentación

### 1. `INDICE_CASOS_USO_REORGANIZADO.md` (este archivo)
- Índice general del sistema
- Resumen de cada módulo
- Referencias a documentación detallada

### 2. `CU_05_06_07_08_09_COMPLETO.md`
- Especificaciones detalladas de CU 05, 07, 08, 09, 10
- Catálogo, Usuarios, Alumnos, Préstamos, Reportes

### 3. `CU_06_GESTOR_INVENTARIO.md` ✨ NUEVO
- Especificación completa de gestión de ejemplares
- Agregar, editar y eliminar ejemplares
- Cambio de estados y historial
- Relación con préstamos

### 4. `CU_GESTION_ROLES_PERMISOS.md`
- Configuración de seguridad
- Creación de roles y asignación de permisos

---

## Diferencia: Catálogo vs Inventario

### CU 05 - Gestor de Catálogo (Materiales)

**Concepto:** Información bibliográfica abstracta

**Ejemplo:**
```
Título: Don Quijote de la Mancha
Autor: Miguel de Cervantes
ISBN: 978-84-376-0494-7
Tipo: Libro
Género: Novela clásica
CantidadTotal: 3 ejemplares
CantidadDisponible: 2 ejemplares
```

**Operaciones:** Registrar, consultar, modificar, eliminar fichas de catálogo

---

### CU 06 - Gestor de Inventario (Ejemplares)

**Concepto:** Copias físicas individuales

**Ejemplo:**
```
Material: Don Quijote de la Mancha
   ├─ Ejemplar #1
   │    Código: BAR001234
   │    Ubicación: Estante A3
   │    Estado: Disponible
   │
   ├─ Ejemplar #2
   │    Código: BAR001235
   │    Ubicación: Estante A3
   │    Estado: Prestado
   │
   └─ Ejemplar #3
        Código: BAR001236
        Ubicación: Estante A3
        Estado: Disponible
```

**Operaciones:** Agregar copias físicas, editar ubicación/estado, dar de baja

---

## Flujo de Trabajo Típico

```
1. CATALOGAR MATERIAL (CU-05.1)
   Registrar: "Don Quijote de la Mancha"
        ↓

2. AGREGAR EJEMPLARES (CU-06.1)
   Crear 3 copias físicas con códigos de barras
        ↓

3. REGISTRAR PRÉSTAMO (CU-09.1)
   Prestar Ejemplar #2 a Juan Pérez
        ↓
   Ejemplar #2 → Estado: Prestado
   Material → CantidadDisponible: 2
        ↓

4. REGISTRAR DEVOLUCIÓN (CU-09.2)
   Juan devuelve Ejemplar #2
        ↓
   Ejemplar #2 → Estado: Disponible
   Material → CantidadDisponible: 3
        ↓

5. CAMBIAR ESTADO (CU-06.2)
   Ejemplar #3 se daña
        ↓
   Editar Ejemplar #3 → Estado: En Reparación
   Material → CantidadDisponible: 2
```

---

## Beneficios de la Separación

✅ **Claridad conceptual** - Diferencia clara entre catálogo e inventario

✅ **Separación de responsabilidades** - Cada módulo con propósito específico

✅ **Mejor trazabilidad** - Historial individual de cada copia física

✅ **Gestión de estados** - Control preciso del estado de cada ejemplar

✅ **Escalabilidad** - Fácil agregar más copias sin modificar catálogo

✅ **Auditoría** - Registro completo de cambios por ejemplar

---

**Última actualización:** 16 de Noviembre de 2025
**Versión:** 2.0 - Reorganización con separación Catálogo/Inventario
**Total de casos de uso:** 34 casos de uso documentados
