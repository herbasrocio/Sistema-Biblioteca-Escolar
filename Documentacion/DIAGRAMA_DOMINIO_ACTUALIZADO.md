# Diagrama de Dominio Actualizado - Sistema Biblioteca Escolar

## Módulo Principal: Gestión Biblioteca (DomainModel)

### Entidades Principales:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          MATERIAL                                        │
├─────────────────────────────────────────────────────────────────────────┤
│ + id_material: Guid                                                      │
│ + titulo: String                                                         │
│ + autor: String                                                          │
│ + editorial: String                                                      │
│ + tipo: TipoMaterial (enum)                                             │
│ + genero: String                                                         │
│ + isbn: String                                                           │
│ + anio_publicacion: Integer?                                            │
│ + nivel: String                                                          │
│ + cantidad_total: Integer                                               │
│ + cantidad_disponible: Integer                                          │
│ + fecha_registro: DateTime                                              │
│ + activo: Boolean                                                        │
└─────────────────────────────────────────────────────────────────────────┘
                    │
                    │ 1
                    │
                    │ tiene
                    │
                    │ 0..*
                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                          EJEMPLAR                                        │
├─────────────────────────────────────────────────────────────────────────┤
│ + id_ejemplar: Guid                                                      │
│ + id_material: Guid (FK)                                                │
│ + numero_ejemplar: Integer                                              │
│ + codigo_ejemplar: String                                               │
│ + estado: EstadoMaterial (enum)                                         │
│ + ubicacion: String                                                      │
│ + observaciones: String                                                  │
│ + fecha_registro: DateTime                                              │
│ + activo: Boolean                                                        │
└─────────────────────────────────────────────────────────────────────────┘
                    │
                    │ es prestado en
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                          PRESTAMO                                        │
├─────────────────────────────────────────────────────────────────────────┤
│ + id_prestamo: Guid                                                      │
│ + id_material: Guid (FK)                                                │
│ + id_ejemplar: Guid (FK)                                                │
│ + id_alumno: Guid (FK)                                                  │
│ + id_usuario: Guid (FK)                                                 │
│ + fecha_prestamo: DateTime                                              │
│ + fecha_devolucion_prevista: DateTime                                   │
│ + estado: String (Activo/Devuelto/Atrasado)                            │
│ + cantidad_renovaciones: Integer                                        │
│ + fecha_ultima_renovacion: DateTime?                                    │
├─────────────────────────────────────────────────────────────────────────┤
│ + EstaAtrasado(): Boolean                                               │
│ + DiasRestantes(): Integer                                              │
│ + PuedeRenovarse(maxRenovaciones): Boolean                             │
└─────────────────────────────────────────────────────────────────────────┘
            │                               │
            │ 1                             │ 1
            │                               │
            │ solicitado por                │ registrado por
            │                               │
            │ 1                             │ 1
            ▼                               ▼
┌──────────────────────────┐    ┌──────────────────────────────────────┐
│       ALUMNO             │    │    USUARIO (Security Module)         │
├──────────────────────────┤    │    Services.DomainModel.Security     │
│ + id_alumno: Guid        │    ├──────────────────────────────────────┤
│ + nombre: String         │    │ + id_usuario: Guid                   │
│ + apellido: String       │    │ + nombre: String                     │
│ + dni: String            │    │ + email: String                      │
│ + grado: String          │    │ + clave: String (hashed)            │
│ + division: String       │    │ + activo: Boolean                    │
│ + fecha_registro: Date   │    │ + idioma_preferido: String           │
├──────────────────────────┤    │ + fecha_ultimo_acceso: DateTime?     │
│ + NombreCompleto: String │    │ + dvh: String                        │
│ + GradoCompleto: String  │    │ - permisos: List<Component>          │
└──────────────────────────┘    ├──────────────────────────────────────┤
            │                   │ + ObtenerFamiliaRol(): Familia       │
            │ 1                 │ + TienePermiso(patente): Boolean     │
            │                   └──────────────────────────────────────┘
            │ pertenece a                   │
            │                               │ tiene
            │ 0..*                          │ 0..*
            ▼                               ▼
┌──────────────────────────┐    ┌──────────────────────────────────────┐
│      INSCRIPCION         │    │         COMPONENT (Abstract)         │
├──────────────────────────┤    │    Services.DomainModel.Security     │
│ + id_inscripcion: Guid   │    ├──────────────────────────────────────┤
│ + id_alumno: Guid (FK)   │    │ + id: Guid                           │
│ + anio_lectivo: Integer  │    │ + nombre: String                     │
│ + grado: String          │    │ + permiso: String                    │
│ + division: String       │    │ + activo: Boolean                    │
│ + fecha_inscripcion: Date│    ├──────────────────────────────────────┤
│ + estado: String         │    │ + TieneHijos: Boolean (abstract)     │
├──────────────────────────┤    └──────────────────────────────────────┘
│ + EsActiva: Boolean      │                    △
└──────────────────────────┘                    │
                                               ╱│╲
                                              ╱ │ ╲
                                             ╱  │  ╲
                    ┌───────────────────────┘   │   └──────────────────┐
                    │                           │                       │
        ┌───────────────────────┐   ┌───────────────────┐  ┌──────────────────┐
        │      FAMILIA          │   │     PATENTE       │  │  DOCENTE         │
        │   (Composite)         │   │     (Leaf)        │  ├──────────────────┤
        ├───────────────────────┤   ├───────────────────┤  │ + apellido: Str  │
        │ - hijos: List<Comp>   │   │ (sin hijos)       │  │ + contraseña: Str│
        ├───────────────────────┤   └───────────────────┘  │ + id_docente: Int│
        │ + Agregar(Component)  │                           │ + nombre: String │
        │ + Quitar(Component)   │                           │ + usuario: String│
        │ + TieneHijos: true    │                           └──────────────────┘
        └───────────────────────┘

┌─────────────────────────────────────────────────────────────────────────┐
│                          DEVOLUCION                                      │
├─────────────────────────────────────────────────────────────────────────┤
│ + id_devolucion: Guid                                                    │
│ + id_prestamo: Guid (FK)                                                │
│ + fecha_devolucion: DateTime                                            │
│ + id_usuario: Guid (FK)                                                 │
│ + observaciones: String                                                  │
├─────────────────────────────────────────────────────────────────────────┤
│ + FueDevueltoATiempo(): Boolean                                         │
│ + DiasDeAtraso(): Integer                                               │
└─────────────────────────────────────────────────────────────────────────┘
                    │
                    │ 1
                    │ corresponde a
                    │ 1
                    ▼
              (PRESTAMO)

┌─────────────────────────────────────────────────────────────────────────┐
│                   RENOVACION_PRESTAMO                                    │
├─────────────────────────────────────────────────────────────────────────┤
│ + id_renovacion: Guid                                                    │
│ + id_prestamo: Guid (FK)                                                │
│ + fecha_renovacion: DateTime                                            │
│ + nueva_fecha_devolucion: DateTime                                      │
│ + id_usuario: Guid (FK)                                                 │
└─────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────┐
│                 HISTORIAL_ESTADO_EJEMPLAR                                │
├─────────────────────────────────────────────────────────────────────────┤
│ + id_historial: Guid                                                     │
│ + id_ejemplar: Guid (FK)                                                │
│ + estado_anterior: EstadoMaterial                                       │
│ + estado_nuevo: EstadoMaterial                                          │
│ + fecha_cambio: DateTime                                                │
│ + id_usuario: Guid (FK)                                                 │
│ + motivo: String                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

## Módulo de Seguridad (Services.DomainModel.Security)

### Relaciones de Permisos:

```
┌──────────────────────┐      ┌──────────────────────┐
│   USUARIO_FAMILIA    │      │   USUARIO_PATENTE    │
├──────────────────────┤      ├──────────────────────┤
│ + id_usuario: Guid   │      │ + id_usuario: Guid   │
│ + id_familia: Guid   │      │ + id_patente: Guid   │
└──────────────────────┘      └──────────────────────┘

┌──────────────────────┐      ┌──────────────────────┐
│   FAMILIA_FAMILIA    │      │   FAMILIA_PATENTE    │
├──────────────────────┤      ├──────────────────────┤
│ + id_padre: Guid     │      │ + id_familia: Guid   │
│ + id_hijo: Guid      │      │ + id_patente: Guid   │
└──────────────────────┘      └──────────────────────┘
```

## Enumeraciones:

```
┌───────────────────────┐       ┌──────────────────────────┐
│   TipoMaterial        │       │   EstadoMaterial         │
├───────────────────────┤       ├──────────────────────────┤
│ - Libro               │       │ - Disponible             │
│ - Revista             │       │ - Prestado               │
│ - Manual              │       │ - EnReparacion           │
└───────────────────────┘       │ - Perdido                │
                                └──────────────────────────┘
```

## Módulo de Auditoría:

```
┌─────────────────────────────────────────────────────────────┐
│                  BITACORA_SEGURIDAD                         │
├─────────────────────────────────────────────────────────────┤
│ + id_bitacora: Guid                                         │
│ + id_usuario: Guid?                                         │
│ + nombre_usuario: String                                    │
│ + tipo_evento: String (Error/Seguridad/CambioCritico)     │
│ + modulo: String                                            │
│ + accion: String                                            │
│ + detalle: String                                           │
│ + gravedad: String                                          │
│ + direccion_ip: String?                                     │
│ + fecha_hora: DateTime                                      │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                 BITACORA_OPERACIONES                        │
├─────────────────────────────────────────────────────────────┤
│ + id_bitacora: Guid                                         │
│ + id_usuario: Guid?                                         │
│ + nombre_usuario: String                                    │
│ + tipo_operacion: String (Prestamo/Devolucion/etc)        │
│ + modulo: String                                            │
│ + accion: String                                            │
│ + entidad_afectada: String                                  │
│ + id_entidad: Integer?                                      │
│ + detalle: String                                           │
│ + fecha_hora: DateTime                                      │
└─────────────────────────────────────────────────────────────┘
```

## Módulo de Backup:

```
┌─────────────────────────────────────────────────────────────┐
│                        BACKUP                               │
├─────────────────────────────────────────────────────────────┤
│ + id_backup: Guid                                           │
│ + nombre_archivo: String                                    │
│ + ruta_completa: String                                     │
│ + base_datos: String                                        │
│ + tipo: String (Completo/Diferencial/Transaccional)        │
│ + tamaño_mb: Decimal                                        │
│ + fecha_creacion: DateTime                                  │
│ + id_usuario: Guid                                          │
│ + nombre_usuario: String                                    │
│ + descripcion: String                                       │
│ + estado: String (Exitoso/Fallido/EnProceso)               │
└─────────────────────────────────────────────────────────────┘
```

---

## Cambios Principales respecto al diagrama anterior:

### ✅ Nuevas Entidades:
1. **Inscripcion** - Gestión de inscripciones por año lectivo
2. **RenovacionPrestamo** - Tracking de renovaciones de préstamos
3. **HistorialEstadoEjemplar** - Auditoría de cambios de estado
4. **BitacoraSeguridad** - Auditoría de eventos de seguridad
5. **BitacoraOperaciones** - Auditoría de operaciones del negocio
6. **Backup** - Gestión de backups de base de datos
7. **Docente** - Gestión de docentes del sistema

### ✅ Módulo Security Separado:
- Ahora está en `Services.DomainModel.Security` (autocontenido)
- Usuario, Familia, Patente, Component están en el módulo Security
- Relaciones de permisos (UsuarioFamilia, FamiliaPatente, etc.)

### ✅ Mejoras en Entidades Existentes:
- **Material**: Agregados campos nivel, isbn, año_publicación
- **Ejemplar**: Ahora con ubicación, observaciones, tracking individual
- **Prestamo**: Tracking de renovaciones y estados
- **Alumno**: Métodos calculados para nombre completo y grado completo
- **Usuario**: DVH para integridad, idioma preferido, fecha último acceso

### ✅ Enumeraciones:
- **TipoMaterial** (Libro/Revista/Manual)
- **EstadoMaterial** (Disponible/Prestado/EnReparacion/Perdido)
- **EstadoPrestamo** strings (Activo/Devuelto/Atrasado)
