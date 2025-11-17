# Especificación de Casos de Uso - Sistema Biblioteca Escolar

## Índice
1. [Gestión de Login y Logout](#1-gestión-de-login-y-logout)
2. [Gestión de Idiomas](#2-gestión-de-idiomas)
3. [Criptografía](#3-criptografía)
4. [Bitácora](#4-bitácora)
5. [Gestión de Excepciones](#5-gestión-de-excepciones)
6. [Backup y Restore](#6-backup-y-restore)
7. [Gestión de Perfiles y Permisos](#7-gestión-de-perfiles-y-permisos)

---

# 1. Gestión de Login y Logout

## CU-001: Iniciar Sesión

**Actor Principal:** Usuario Anónimo
**Precondiciones:** El sistema está en funcionamiento
**Postcondiciones:** El usuario ha iniciado sesión exitosamente

### Flujo Principal:
1. El usuario ingresa su nombre de usuario y contraseña
2. El sistema valida las credenciales (UC-001.1)
3. El sistema carga los permisos del usuario (UC-001.2)
4. El sistema registra el acceso en la bitácora (UC-001.3)
5. El sistema aplica el idioma preferido del usuario (UC-001.4)
6. El sistema actualiza la fecha de último acceso
7. El sistema muestra la pantalla principal según los permisos

### Flujos Alternativos:
- **2a. Credenciales inválidas:**
  - El sistema muestra mensaje "Usuario o contraseña incorrecta"
  - El sistema registra el intento fallido en bitácora de seguridad
  - Retorna al paso 1

- **2b. Usuario inactivo:**
  - El sistema muestra mensaje "Usuario inactivo"
  - El sistema registra el intento en bitácora
  - Fin del caso de uso

- **2c. Error de integridad de datos:**
  - El sistema detecta DVH inválido
  - El sistema muestra mensaje de error crítico
  - El sistema registra el evento en bitácora de seguridad
  - Fin del caso de uso

### Información Específica:
- **Clase:** `Services.Services.LoginService`
- **Método:** `Login(string nombre, string password)`
- **Excepciones:** `UsuarioNoEncontradoException`, `ContraseñaInvalidaException`, `IntegridadException`

---

## CU-001.1: Validar Credenciales

**Actor Principal:** Sistema
**Precondiciones:** Se han ingresado usuario y contraseña

### Flujo Principal:
1. El sistema busca el usuario por nombre
2. El sistema hashea la contraseña ingresada con SHA-256
3. El sistema compara el hash con el almacenado en la base de datos
4. El sistema valida que el usuario esté activo
5. El sistema verifica la integridad de los datos (DVH)

### Información Específica:
- **Clase:** `Services.Services.LoginService`
- **Servicio de hash:** `Services.Services.CryptographyService.HashPassword()`

---

## CU-001.2: Cargar Permisos de Usuario

**Actor Principal:** Sistema
**Precondiciones:** El usuario ha sido autenticado

### Flujo Principal:
1. El sistema limpia la lista de permisos del usuario
2. El sistema obtiene las familias (roles) asignadas al usuario
3. El sistema carga cada familia con su estructura jerárquica de permisos
4. El sistema obtiene las patentes asignadas directamente al usuario
5. El sistema agrega todas las patentes a la lista de permisos

### Información Específica:
- **Clase:** `Services.Services.LoginService`
- **Método:** `CargarPermisosUsuario(Usuario usuario)`
- **Patrón:** Composite Pattern para jerarquía de permisos

---

## CU-002: Cerrar Sesión

**Actor Principal:** Bibliotecario, Administrador
**Precondiciones:** El usuario ha iniciado sesión
**Postcondiciones:** La sesión ha sido cerrada

### Flujo Principal:
1. El usuario solicita cerrar sesión
2. El sistema limpia la sesión actual (UC-002.1)
3. El sistema registra el cierre de sesión en bitácora
4. El sistema limpia los observers de permisos
5. El sistema muestra la pantalla de login

### Información Específica:
- **Clase:** `Services.Services.PermissionManager`
- **Método:** `ClearObservers()`

---

# 2. Gestión de Idiomas

## CU-003: Cambiar Idioma

**Actor Principal:** Bibliotecario, Administrador
**Precondiciones:** El usuario ha iniciado sesión
**Postcondiciones:** El idioma de la interfaz ha cambiado

### Flujo Principal:
1. El usuario selecciona un idioma del menú
2. El sistema obtiene las traducciones del idioma seleccionado (UC-003.1)
3. El sistema cambia la cultura del thread actual
4. El sistema notifica a todos los observers del cambio (UC-003.2)
5. El sistema actualiza la interfaz de todos los formularios abiertos (UC-003.3)
6. El sistema guarda la preferencia de idioma del usuario (UC-003.4)

### Flujos Alternativos:
- **2a. Error al cargar traducciones:**
  - El sistema muestra mensaje de error
  - El sistema mantiene el idioma actual
  - Fin del caso de uso

### Información Específica:
- **Clase:** `Services.Services.LanguageManager`
- **Método:** `ChangeLanguage(string newCulture)`
- **Evento:** `LanguageChanged`
- **Patrón:** Observer Pattern

---

## CU-003.1: Obtener Traducciones

**Actor Principal:** Sistema
**Precondiciones:** Se ha seleccionado un idioma

### Flujo Principal:
1. El sistema lee el archivo de idioma correspondiente
2. El sistema carga todas las traducciones en un diccionario
3. El sistema retorna el diccionario de traducciones

### Información Específica:
- **Clase:** `Services.BLL.LanguageBLL`
- **Método:** `Translate(string word)`
- **Repositorio:** `Services.DAL.Implementations.LanguageRepository`

---

## CU-003.2: Notificar Cambio a Observers

**Actor Principal:** Sistema
**Precondiciones:** El idioma ha cambiado

### Flujo Principal:
1. El sistema dispara el evento `LanguageChanged`
2. Todos los formularios suscritos reciben la notificación
3. Cada formulario actualiza sus controles con las nuevas traducciones

### Información Específica:
- **Evento:** `LanguageManager.LanguageChanged`
- **EventArgs:** `LanguageChangedEventArgs(newCulture, previousCulture)`

---

## CU-003.4: Guardar Preferencia de Idioma

**Actor Principal:** Sistema
**Precondiciones:** El idioma ha cambiado exitosamente

### Flujo Principal:
1. El sistema actualiza el campo `IdiomaPreferido` del usuario
2. El sistema guarda el cambio en la base de datos
3. El sistema recalcula el DVH del usuario

### Información Específica:
- **Clase:** `Services.BLL.UsuarioBLL`
- **Método:** `ModificarIdioma(Usuario usuario, string idioma)`

---

# 3. Criptografía

## CU-004: Hash de Contraseña

**Actor Principal:** Sistema
**Precondiciones:** Se necesita hashear una contraseña
**Postcondiciones:** Se obtiene el hash SHA-256

### Flujo Principal:
1. El sistema recibe el texto plano de la contraseña
2. El sistema aplica el algoritmo SHA-256 (UC-004.1)
3. El sistema convierte el resultado a string hexadecimal
4. El sistema retorna el hash

### Información Específica:
- **Clase:** `Services.Services.CryptographyService`
- **Método:** `HashPassword(string textPlainPass)`
- **Algoritmo:** SHA-256
- **Encoding:** Unicode (UTF-16) para compatibilidad con SQL Server NVARCHAR

---

## CU-005: Validar Contraseña

**Actor Principal:** Sistema
**Precondiciones:** Se necesita validar una contraseña ingresada
**Postcondiciones:** Se determina si la contraseña es válida

### Flujo Principal:
1. El sistema hashea la contraseña ingresada
2. El sistema compara el hash con el almacenado (UC-005.1)
3. El sistema retorna verdadero si coinciden, falso en caso contrario

### Información Específica:
- **Uso:** Login, cambio de contraseña
- **Clase:** `Services.Services.LoginService`

---

## CU-006: Calcular DVH (Dígito Verificador Horizontal)

**Actor Principal:** Sistema
**Precondiciones:** Se han modificado datos críticos del usuario
**Postcondiciones:** El DVH ha sido calculado y almacenado

### Flujo Principal:
1. El sistema concatena los campos críticos del usuario (Nombre + Password)
2. El sistema hashea la concatenación
3. El sistema almacena el hash como DVH
4. El sistema guarda el usuario en la base de datos

### Información Específica:
- **Clase:** `Services.DomainModel.Security.Composite.Usuario`
- **Propiedad:** `HashDH`
- **Uso:** Verificación de integridad de datos

---

# 4. Bitácora

## CU-007: Registrar Evento de Seguridad

**Actor Principal:** Sistema
**Precondiciones:** Ocurre un evento de seguridad
**Postcondiciones:** El evento queda registrado en la bitácora

### Flujo Principal:
1. El sistema captura el evento (login, error, cambio crítico)
2. El sistema crea un registro de `BitacoraSeguridad`
3. El sistema completa los campos:
   - TipoEvento (Error/Seguridad/CambioCritico)
   - Módulo
   - Acción
   - Detalle
   - Gravedad (Bajo/Medio/Alto)
   - Usuario (si está autenticado)
   - Dirección IP (si disponible)
   - Fecha y hora
4. El sistema guarda el registro en la base de datos

### Información Específica:
- **Clase:** `BLL.BitacoraSeguridadBLL`
- **Métodos:**
  - `RegistrarError()`
  - `RegistrarEventoSeguridad()`
  - `RegistrarCambioCritico()`
- **Entidad:** `DomainModel.BitacoraSeguridad`

---

## CU-008: Registrar Operación de Negocio

**Actor Principal:** Sistema
**Precondiciones:** Se ejecuta una operación del bibliotecario
**Postcondiciones:** La operación queda registrada

### Flujo Principal:
1. El sistema captura la operación (préstamo, devolución, etc.)
2. El sistema crea un registro de `BitacoraOperaciones`
3. El sistema completa los campos:
   - TipoOperacion (Prestamo/Devolucion/GestionMaterial/etc)
   - Módulo
   - Acción
   - EntidadAfectada
   - IdEntidad
   - Detalle
   - Usuario
   - Fecha y hora
4. El sistema guarda el registro en la base de datos

### Información Específica:
- **Clase:** `BLL.BitacoraOperacionesBLL`
- **Métodos:**
  - `RegistrarPrestamo()`
  - `RegistrarDevolucion()`
  - `RegistrarOperacionMaterial()`
  - `RegistrarOperacionAlumno()`
- **Entidad:** `DomainModel.BitacoraOperaciones`

---

## CU-009: Consultar Bitácora

**Actor Principal:** Administrador, Bibliotecario
**Precondiciones:** El usuario tiene permisos de consulta
**Postcondiciones:** Se muestra la bitácora filtrada

### Flujo Principal:
1. El usuario selecciona el tipo de bitácora (Seguridad u Operaciones)
2. El usuario selecciona filtros opcionales:
   - Rango de fechas
   - Tipo de evento/operación
   - Usuario
   - Módulo
   - Gravedad
3. El sistema aplica los filtros (UC-009.1)
4. El sistema muestra los registros en una grilla
5. El usuario puede exportar los resultados (UC-009.2)

### Flujos Alternativos:
- **3a. Sin resultados:**
  - El sistema muestra mensaje "No se encontraron registros"

### Información Específica:
- **Formularios:**
  - `frmConsultarBitacoraSeguridad`
  - `frmConsultarBitacoraOperaciones`
- **Clase BLL:** `BitacoraSeguridadBLL`, `BitacoraOperacionesBLL`
- **Métodos:** `ObtenerConFiltros()`, `ObtenerPorFechas()`, `ObtenerPorUsuario()`

---

## CU-009.2: Exportar Bitácora

**Actor Principal:** Administrador
**Precondiciones:** Hay registros de bitácora mostrados
**Postcondiciones:** Los registros se exportan a un archivo

### Flujo Principal:
1. El usuario selecciona "Exportar"
2. El sistema muestra diálogo para seleccionar ubicación y nombre de archivo
3. El usuario confirma
4. El sistema exporta los registros a formato CSV/Excel
5. El sistema muestra mensaje de éxito

### Información Específica:
- **Clase:** `Services.Services.ExportService`
- **Formato:** CSV

---

# 5. Gestión de Excepciones

## CU-010: Capturar Excepción

**Actor Principal:** Sistema
**Precondiciones:** Ocurre una excepción en el sistema
**Postcondiciones:** La excepción ha sido manejada apropiadamente

### Flujo Principal:
1. El sistema captura la excepción
2. El sistema registra en log de aplicación (UC-010.1)
3. El sistema registra en bitácora de seguridad (UC-010.2)
4. El sistema traduce el mensaje de error al idioma actual
5. El sistema muestra mensaje amigable al usuario (UC-010.3)
6. El sistema continúa la ejecución o cierra gracefully

### Flujos Alternativos:
- **1a. Excepción de integridad:**
  - El sistema marca como "Crítico"
  - El sistema bloquea el acceso al módulo afectado
  - El sistema notifica al administrador

- **1b. Excepción de autenticación:**
  - El sistema cierra la sesión
  - El sistema muestra mensaje de error
  - El sistema redirige al login

### Información Específica:
- **Clase:** `Services.Services.ExceptionManager`
- **Método:** `Handle(Exception ex)`
- **Patrón:** Singleton

---

## CU-010.1: Registrar en Log

**Actor Principal:** Sistema
**Precondiciones:** Se ha capturado una excepción

### Flujo Principal:
1. El sistema obtiene los detalles de la excepción
2. El sistema escribe en el archivo de log:
   - Fecha y hora
   - Tipo de excepción
   - Mensaje
   - Stack trace
   - Usuario (si está autenticado)
3. El sistema cierra el archivo de log

### Información Específica:
- **Ubicación log:** Configurable en App.config
- **Clase:** `Services.Services.ExceptionManager`

---

## CU-010.3: Mostrar Mensaje al Usuario

**Actor Principal:** Sistema
**Precondiciones:** Se ha capturado y procesado una excepción

### Flujo Principal:
1. El sistema determina el nivel de severidad
2. El sistema traduce el mensaje al idioma actual
3. El sistema muestra un MessageBox con:
   - Mensaje amigable
   - Tipo de mensaje (Error/Warning/Info)
   - Botones apropiados
4. El usuario cierra el mensaje

### Información Específica:
- **Mensajes traducidos:** Según `LanguageManager`
- **Tipos de mensaje:**
  - Validación → Warning
  - Error de negocio → Error
  - Error de integridad → Error crítico

---

## CU-011: Verificar Integridad de Datos

**Actor Principal:** Sistema
**Precondiciones:** Se accede a datos críticos
**Postcondiciones:** Se verifica la integridad

### Flujo Principal:
1. El sistema carga el usuario de la base de datos
2. El sistema recalcula el DVH con los datos actuales
3. El sistema compara el DVH calculado con el almacenado
4. Si coinciden, continúa normalmente
5. Si NO coinciden, lanza `IntegridadException`

### Flujos Alternativos:
- **5a. DVH no coincide:**
  - El sistema registra evento crítico en bitácora
  - El sistema bloquea el acceso
  - El sistema notifica al administrador
  - Fin del caso de uso

### Información Específica:
- **Clase:** `Services.BLL.UsuarioBLL`
- **Método:** `VerificarIntegridad(Usuario usuario)`
- **Excepción:** `IntegridadException`

---

# 6. Backup y Restore

## CU-012: Crear Backup

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos de administrador
**Postcondiciones:** Se ha creado un backup de la base de datos

### Flujo Principal:
1. El administrador selecciona "Crear Backup"
2. El sistema muestra formulario de configuración
3. El administrador selecciona:
   - Base de datos (Seguridad/Negocio/Ambas)
   - Tipo de backup (Completo/Diferencial/Transaccional)
   - Ruta de destino
   - Descripción opcional
4. El sistema valida la ruta de destino (UC-012.1)
5. El administrador confirma
6. El sistema ejecuta el backup en SQL Server (UC-012.2)
7. El sistema registra el backup en el catálogo (UC-012.3)
8. El sistema verifica la integridad del backup (UC-012.4)
9. El sistema muestra mensaje de éxito

### Flujos Alternativos:
- **4a. Ruta inválida:**
  - El sistema muestra mensaje de error
  - Retorna al paso 3

- **6a. Error al ejecutar backup:**
  - El sistema captura el error
  - El sistema registra en bitácora
  - El sistema muestra mensaje de error
  - Fin del caso de uso

- **8a. Integridad fallida:**
  - El sistema marca el backup como "Fallido"
  - El sistema muestra advertencia
  - El administrador decide si continuar o reintentar

### Información Específica:
- **Formulario:** `frmBackupRestore`
- **Clase:** `Services.BLL.BackupBLL`
- **Método:** `CrearBackup(tipoBackup, rutaDestino, baseDatos, descripcion)`
- **Entidad:** `DomainModel.Backup`

---

## CU-012.1: Validar Ruta de Destino

**Actor Principal:** Sistema
**Precondiciones:** Se ha ingresado una ruta de destino

### Flujo Principal:
1. El sistema verifica que la ruta exista
2. El sistema verifica permisos de escritura
3. El sistema verifica espacio disponible
4. El sistema valida que no exista un archivo con el mismo nombre

### Información Específica:
- **Clase:** `Services.BLL.BackupBLL`
- **Validaciones:** Existencia, permisos, espacio, duplicados

---

## CU-012.2: Ejecutar Backup SQL Server

**Actor Principal:** Sistema
**Precondiciones:** La configuración ha sido validada

### Flujo Principal:
1. El sistema construye la sentencia BACKUP DATABASE
2. El sistema ejecuta el comando en SQL Server
3. El sistema espera la finalización
4. El sistema obtiene el tamaño del archivo generado

### Información Específica:
- **Clase:** `Services.DAL.Implementations.BackupRepository`
- **Método:** `EjecutarBackup()`
- **SQL:** `BACKUP DATABASE [nombre] TO DISK = 'ruta'`

---

## CU-012.3: Registrar Backup en Catálogo

**Actor Principal:** Sistema
**Precondiciones:** El backup se ha ejecutado exitosamente

### Flujo Principal:
1. El sistema crea un registro de `Backup`
2. El sistema completa los campos:
   - Nombre de archivo
   - Ruta completa
   - Base de datos
   - Tipo
   - Tamaño en MB
   - Fecha de creación
   - Usuario que lo creó
   - Descripción
   - Estado (Exitoso/Fallido)
3. El sistema guarda en la base de datos de seguridad

### Información Específica:
- **Tabla:** `Backup` (en DB de Seguridad)
- **Clase:** `Services.DAL.Implementations.BackupRepository`

---

## CU-013: Restaurar Backup

**Actor Principal:** Administrador
**Precondiciones:** Existe un backup válido
**Postcondiciones:** La base de datos ha sido restaurada

### Flujo Principal:
1. El administrador selecciona "Restaurar Backup"
2. El sistema muestra catálogo de backups disponibles
3. El administrador selecciona un backup (UC-013.1)
4. El sistema valida el archivo de backup (UC-013.2)
5. El sistema muestra advertencia de que se perderán datos actuales
6. El administrador confirma
7. El sistema cierra todas las conexiones activas
8. El sistema ejecuta RESTORE DATABASE (UC-013.3)
9. El sistema verifica la restauración (UC-013.4)
10. El sistema registra la operación en bitácora
11. El sistema muestra mensaje de éxito

### Flujos Alternativos:
- **4a. Archivo de backup no existe:**
  - El sistema muestra mensaje de error
  - Retorna al paso 2

- **4b. Archivo corrupto:**
  - El sistema muestra mensaje de error
  - Fin del caso de uso

- **6a. Usuario cancela:**
  - Fin del caso de uso

- **8a. Error al restaurar:**
  - El sistema captura el error
  - El sistema intenta rollback si es posible
  - El sistema registra en bitácora
  - El sistema muestra mensaje de error
  - Fin del caso de uso

### Información Específica:
- **Clase:** `Services.BLL.BackupBLL`
- **Método:** `RestaurarBackup(idBackup)`
- **Formulario:** `frmBackupRestore`

---

## CU-013.2: Validar Archivo de Backup

**Actor Principal:** Sistema
**Precondiciones:** Se ha seleccionado un backup

### Flujo Principal:
1. El sistema verifica que el archivo exista físicamente
2. El sistema ejecuta RESTORE VERIFYONLY
3. El sistema valida el encabezado del backup
4. El sistema confirma que el backup es compatible

### Información Específica:
- **SQL:** `RESTORE VERIFYONLY FROM DISK = 'ruta'`
- **Clase:** `Services.DAL.Implementations.BackupRepository`

---

## CU-014: Consultar Catálogo de Backups

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos
**Postcondiciones:** Se muestra el catálogo

### Flujo Principal:
1. El administrador accede al módulo de backups
2. El sistema carga todos los backups registrados
3. El sistema muestra la información en una grilla:
   - Fecha de creación
   - Base de datos
   - Tipo
   - Tamaño
   - Estado
   - Usuario que lo creó
   - Descripción
4. El administrador puede filtrar y ordenar

### Información Específica:
- **Clase:** `Services.BLL.BackupBLL`
- **Método:** `ObtenerTodosLosBackups()`

---

# 7. Gestión de Perfiles y Permisos

## CU-015: Crear Familia (Rol)

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos de administrador
**Postcondiciones:** Se ha creado una nueva familia

### Flujo Principal:
1. El administrador selecciona "Nueva Familia"
2. El sistema muestra formulario
3. El administrador ingresa:
   - Nombre de la familia
   - Descripción/Permiso general
4. El administrador puede agregar patentes (UC-015.1)
5. El administrador puede agregar familias hijas (UC-015.2)
6. El administrador confirma
7. El sistema valida que no exista duplicado
8. El sistema guarda la familia
9. El sistema registra la operación en bitácora
10. El sistema muestra mensaje de éxito

### Flujos Alternativos:
- **7a. Nombre duplicado:**
  - El sistema muestra mensaje de error
  - Retorna al paso 3

### Información Específica:
- **Formulario:** `frmGestionFamilias`
- **Clase:** `Services.BLL.FamiliaBLL`
- **Método:** `AgregarFamilia(Familia familia)`
- **Entidad:** `Services.DomainModel.Security.Composite.Familia`

---

## CU-015.1: Agregar Patente a Familia

**Actor Principal:** Administrador
**Precondiciones:** Se está creando/modificando una familia

### Flujo Principal:
1. El administrador selecciona "Agregar Patente"
2. El sistema muestra lista de patentes disponibles
3. El administrador selecciona una o más patentes
4. El sistema agrega las patentes a la familia
5. El sistema actualiza la visualización

### Información Específica:
- **Clase:** `Services.BLL.FamiliaBLL`
- **Método:** `AgregarPatenteAFamilia(idFamilia, idPatente)`
- **Tabla relación:** `FamiliaPatente`

---

## CU-015.2: Agregar Familia Hija a Familia Padre

**Actor Principal:** Administrador
**Precondiciones:** Existen familias disponibles

### Flujo Principal:
1. El administrador selecciona "Agregar Familia Hija"
2. El sistema muestra lista de familias disponibles
3. El administrador selecciona una familia
4. El sistema valida que no cree ciclos (UC-015.3)
5. El sistema agrega la relación padre-hijo
6. El sistema actualiza la visualización jerárquica

### Flujos Alternativos:
- **4a. Se detecta ciclo:**
  - El sistema muestra mensaje "No se puede agregar porque crearía un ciclo"
  - Retorna al paso 2

### Información Específica:
- **Clase:** `Services.BLL.FamiliaBLL`
- **Método:** `AgregarFamiliaAFamilia(idPadre, idHijo)`
- **Tabla relación:** `FamiliaFamilia`

---

## CU-015.3: Validar Estructura Jerárquica

**Actor Principal:** Sistema
**Precondiciones:** Se intenta agregar una familia hija

### Flujo Principal:
1. El sistema verifica que la familia hija no sea la misma que la padre
2. El sistema verifica que la familia hija no sea ancestro de la padre
3. El sistema recorre recursivamente la jerarquía
4. Si no hay ciclos, retorna verdadero

### Información Específica:
- **Algoritmo:** Búsqueda en profundidad (DFS) para detectar ciclos
- **Clase:** `Services.BLL.FamiliaBLL`

---

## CU-016: Crear Patente

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos
**Postcondiciones:** Se ha creado una nueva patente

### Flujo Principal:
1. El administrador selecciona "Nueva Patente"
2. El sistema muestra formulario
3. El administrador ingresa:
   - Nombre de la patente
   - Descripción/Permiso específico
4. El administrador confirma
5. El sistema valida que no exista duplicado
6. El sistema guarda la patente
7. El sistema registra en bitácora
8. El sistema muestra mensaje de éxito

### Información Específica:
- **Formulario:** `frmGestionPatentes`
- **Clase:** `Services.BLL.FamiliaBLL`
- **Entidad:** `Services.DomainModel.Security.Composite.Patente`

---

## CU-017: Asignar Familia a Usuario

**Actor Principal:** Administrador
**Precondiciones:** Existen usuarios y familias
**Postcondiciones:** El usuario tiene la familia asignada

### Flujo Principal:
1. El administrador selecciona un usuario
2. El administrador selecciona "Asignar Familia"
3. El sistema muestra lista de familias disponibles
4. El administrador selecciona una familia
5. El administrador confirma
6. El sistema crea la relación usuario-familia
7. El sistema recalcula el DVH del usuario (UC-017.1)
8. El sistema notifica el cambio de permisos (UC-017.2)
9. El sistema registra en bitácora
10. El sistema muestra mensaje de éxito

### Información Específica:
- **Formulario:** `frmGestionUsuarios`
- **Clase:** `Services.BLL.UsuarioBLL`
- **Método:** `AsignarFamiliaAUsuario(idUsuario, idFamilia)`
- **Tabla relación:** `UsuarioFamilia`

---

## CU-017.1: Recalcular DVH de Usuario

**Actor Principal:** Sistema
**Precondiciones:** Los permisos del usuario han cambiado

### Flujo Principal:
1. El sistema calcula el nuevo DVH del usuario
2. El sistema actualiza el campo DVH en la base de datos

### Información Específica:
- **Clase:** `Services.BLL.UsuarioBLL`
- **Propiedad:** `Usuario.HashDH`

---

## CU-017.2: Notificar Cambio de Permisos

**Actor Principal:** Sistema
**Precondiciones:** Los permisos han sido modificados

### Flujo Principal:
1. El sistema obtiene la instancia del `PermissionManager`
2. El sistema llama a `NotifyPermissionsChanged(idUsuario)`
3. El `PermissionManager` recarga los permisos del usuario
4. El `PermissionManager` notifica a todos los observers
5. Cada formulario abierto actualiza su interfaz según los nuevos permisos

### Información Específica:
- **Clase:** `Services.Services.PermissionManager`
- **Método:** `NotifyPermissionsChanged(Guid idUsuario)`
- **Patrón:** Observer Pattern

---

## CU-018: Asignar Patente a Usuario

**Actor Principal:** Administrador
**Precondiciones:** Existen usuarios y patentes
**Postcondiciones:** El usuario tiene la patente asignada

### Flujo Principal:
1. El administrador selecciona un usuario
2. El administrador selecciona "Asignar Patente"
3. El sistema muestra lista de patentes disponibles
4. El administrador selecciona una o más patentes
5. El administrador confirma
6. El sistema crea las relaciones usuario-patente
7. El sistema recalcula el DVH
8. El sistema notifica el cambio de permisos
9. El sistema registra en bitácora
10. El sistema muestra mensaje de éxito

### Información Específica:
- **Clase:** `Services.BLL.UsuarioBLL`
- **Método:** `AsignarPatenteAUsuario(idUsuario, idPatente)`
- **Tabla relación:** `UsuarioPatente`

---

## CU-019: Quitar Permisos de Usuario

**Actor Principal:** Administrador
**Precondiciones:** El usuario tiene permisos asignados
**Postcondiciones:** Los permisos han sido removidos

### Flujo Principal:
1. El administrador selecciona un usuario
2. El sistema muestra los permisos actuales del usuario
3. El administrador selecciona permisos a quitar (familias o patentes)
4. El administrador confirma
5. El sistema elimina las relaciones
6. El sistema recalcula el DVH
7. El sistema notifica el cambio de permisos
8. El sistema registra en bitácora
9. El sistema muestra mensaje de éxito

### Información Específica:
- **Clase:** `Services.BLL.UsuarioBLL`
- **Métodos:**
  - `QuitarFamiliaDeUsuario()`
  - `QuitarPatenteDeUsuario()`

---

## CU-020: Consultar Permisos de Usuario

**Actor Principal:** Administrador, Bibliotecario
**Precondiciones:** El usuario tiene permisos de consulta
**Postcondiciones:** Se muestran los permisos

### Flujo Principal:
1. El usuario selecciona "Consultar Permisos"
2. El sistema muestra lista de usuarios
3. El usuario selecciona un usuario
4. El sistema carga los permisos del usuario seleccionado
5. El sistema muestra en árbol jerárquico:
   - Familias asignadas (con sus patentes)
   - Patentes directas
6. El usuario puede expandir/colapsar la jerarquía

### Información Específica:
- **Formulario:** `frmConsultarPermisos`
- **Control:** TreeView para visualización jerárquica
- **Clase:** `Services.BLL.UsuarioBLL`

---

## CU-021: Verificar Permiso de Usuario

**Actor Principal:** Sistema
**Precondiciones:** Se necesita verificar un permiso
**Postcondiciones:** Se determina si el usuario tiene el permiso

### Flujo Principal:
1. El sistema recibe la solicitud de verificación con:
   - Usuario actual
   - Patente a verificar
2. El sistema busca en los permisos directos del usuario
3. Si no lo encuentra, busca en las familias asignadas
4. El sistema recorre recursivamente la jerarquía de familias
5. Si encuentra la patente, retorna verdadero
6. Si no la encuentra, retorna falso

### Información Específica:
- **Clase:** `Services.DomainModel.Security.Composite.Usuario`
- **Método:** `TienePermiso(string patente)`
- **Patrón:** Composite Pattern

---

## Notas Finales

### Convenciones Usadas:
- **UC-XXX:** Identificador único de caso de uso
- **UC-XXX.Y:** Sub-caso de uso incluido
- **<<include>>:** Caso de uso siempre ejecutado
- **<<extend>>:** Caso de uso condicionalmente ejecutado

### Actores del Sistema:
1. **Usuario Anónimo:** Usuario no autenticado
2. **Bibliotecario:** Usuario autenticado con permisos básicos
3. **Administrador:** Usuario con permisos administrativos completos
4. **Sistema:** Actor interno que ejecuta procesos automáticos

### Patrones de Diseño Utilizados:
- **Singleton:** `PermissionManager`, `ExceptionManager`, `LanguageManager`
- **Observer:** `LanguageManager.LanguageChanged`, `PermissionManager` notifications
- **Composite:** Jerarquía de permisos (Component, Familia, Patente, Usuario)
- **Repository:** Acceso a datos
- **Factory:** Creación de repositorios

### Referencias Cruzadas:
- Todos los casos de uso están implementados en el código actual
- Las clases mencionadas corresponden a la estructura real del proyecto
- Los métodos referenciados existen en las clases indicadas
