# 🗄️ Scripts de Base de Datos - Negocio

Esta carpeta contiene todos los scripts SQL necesarios para crear y configurar la base de datos **NegocioBiblioteca**.

---

## 🚀 Inicio Rápido

### ¿Primera vez? Ejecuta esto:

```bash
# Desde la carpeta raíz del proyecto
sqlcmd -S localhost -E -i "Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql"
```

Esto crea la base de datos completa con datos de ejemplo.

---

## 📋 Scripts Disponibles

### Scripts de Setup Automático

| Script | Propósito | Cuándo usar |
|--------|-----------|-------------|
| `00_VerificarBaseDatos.sql` | Verifica estado sin modificar | Cuando quieres revisar qué hay |
| `00_VerificarYCrearDatosPrueba.sql` | Agrega datos de prueba si faltan | Cuando la BD existe pero está vacía |
| `00_EJECUTAR_TODO_NEGOCIO.sql` | Setup completo desde cero | Primera instalación o reinstalación |

### Scripts Base (Ejecutar en orden)

| # | Script | Descripción |
|---|--------|-------------|
| 1 | `01_CrearBaseDatosNegocio.sql` | Crea la base de datos |
| 2 | `02_CrearTablasNegocio.sql` | Crea tablas: Material, Alumno, Prestamo, Devolucion |
| 3 | `03_DatosInicialesNegocio.sql` | Inserta 16 materiales y 10 alumnos de ejemplo |

### Scripts de Inscripciones (Ejecutar después de los base)

| # | Script | Descripción |
|---|--------|-------------|
| 4 | `04_CrearTablasInscripcion.sql` | Crea tablas: Inscripcion, AnioLectivo |
| 5 | `05_MigrarDatosInscripcion.sql` | Migra datos existentes a inscripciones |
| 6 | `06_StoredProceduresInscripcion.sql` | Crea SPs de promoción y consultas |

---

## 📖 Casos de Uso

### Caso 1: Instalación Inicial (No existe nada)

```bash
sqlcmd -S localhost -E -i "Database\Negocio\00_EJECUTAR_TODO_NEGOCIO.sql"
```

✅ Crea base de datos completa con datos de ejemplo

---

### Caso 2: Verificar Estado Actual

```bash
sqlcmd -S localhost -E -i "Database\Negocio\00_VerificarBaseDatos.sql"
```

✅ Muestra diagnóstico completo sin modificar nada

---

### Caso 3: Base de Datos Existe pero sin Alumnos

```bash
sqlcmd -S localhost -E -i "Database\Negocio\00_VerificarYCrearDatosPrueba.sql"
```

✅ Agrega 16 alumnos de prueba si la tabla está vacía

---

### Caso 4: Instalación Manual (Paso a Paso)

```bash
# Desde la carpeta Database\Negocio
sqlcmd -S localhost -E -i "01_CrearBaseDatosNegocio.sql"
sqlcmd -S localhost -E -i "02_CrearTablasNegocio.sql"
sqlcmd -S localhost -E -i "03_DatosInicialesNegocio.sql"
sqlcmd -S localhost -E -i "04_CrearTablasInscripcion.sql"
sqlcmd -S localhost -E -i "05_MigrarDatosInscripcion.sql"
sqlcmd -S localhost -E -i "06_StoredProceduresInscripcion.sql"
```

✅ Control total del proceso

---

## 🗂️ Estructura de Base de Datos

### Tablas Creadas

**Negocio Principal:**
- `Material` - Catálogo de libros, revistas, manuales
- `Alumno` - Datos de alumnos
- `Prestamo` - Préstamos de materiales
- `Devolucion` - Registro de devoluciones

**Sistema de Inscripciones:**
- `Inscripcion` - Historial académico por año
- `AnioLectivo` - Años lectivos (2025, 2026, etc.)

### Stored Procedures

- `sp_ObtenerInscripcionActiva` - Obtiene inscripción actual de un alumno
- `sp_PromocionarAlumnosPorGrado` - Promociona un grado/división específico
- `sp_PromocionarTodosLosAlumnos` - Promoción masiva de todos los grados
- `sp_ObtenerAlumnosPorGradoDivision` - Lista alumnos por grado/div

---

## ⚠️ Importante

- **Hacer backup antes de ejecutar** `00_EJECUTAR_TODO_NEGOCIO.sql` (elimina y recrea la BD)
- Los scripts `00_Verificar*.sql` NO modifican datos, son seguros
- Los scripts están diseñados para ejecutarse desde la carpeta raíz del proyecto

---

## 🆘 Troubleshooting

**Error: "Database already exists"**
- Solución: Usar `00_EJECUTAR_TODO_NEGOCIO.sql` (elimina y recrea) o ejecutar scripts individuales

**Error: "Table already exists"**
- Causa: Ya ejecutaste los scripts antes
- Solución: Usar `00_VerificarBaseDatos.sql` para revisar estado actual

**Error: "Cannot open database"**
- Causa: SQL Server no está corriendo o no hay permisos
- Solución: Verificar que SQL Server esté activo y usar `-E` para Windows Authentication

**La aplicación no carga alumnos**
- Solución: Ejecutar `00_VerificarYCrearDatosPrueba.sql` para agregar datos de prueba

---

## 📞 Más Información

Ver documentación completa en:
- `INSTRUCCIONES_INSTALACION.md` (carpeta raíz)
- `README_SISTEMA_INSCRIPCIONES.md` (carpeta raíz)

---

**Última actualización:** 08/10/2025
