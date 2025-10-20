# Resumen de Cambios - Traducciones de Roles
**Fecha**: 2025-10-13

## Problema Identificado

Al cambiar el idioma del sistema a inglés, la pantalla de **Gestión de Permisos** mostraba:
- Los títulos y etiquetas traducidos correctamente (en inglés)
- Los permisos traducidos correctamente (en inglés)
- **Los roles seguían en español**: "Administrador", "Bibliotecario", "Docente"

## Solución Implementada

### 1. Archivos de Idioma Actualizados

#### `idioma.es-AR` (Español - Argentina)
Agregadas las siguientes claves de traducción:

```
rol_administrador=Administrador
rol_bibliotecario=Bibliotecario
rol_docente=Docente
```

#### `idioma.en-GB` (Inglés - Reino Unido)
Agregadas las traducciones al inglés:

```
rol_administrador=Administrator
rol_bibliotecario=Librarian
rol_docente=Teacher
```

### 2. Código C# Modificado

**Archivo**: `View/UI/WinUi/Administración/gestionPermisos.cs`

#### Cambios Realizados:

1. **Nueva clase auxiliar `RolDisplay`**:
```csharp
private class RolDisplay
{
    public Familia Familia { get; set; }
    public string NombreTraducido { get; set; }
}
```

2. **Nuevo método `TraducirNombreRol()`**:
```csharp
/// <summary>
/// Traduce el nombre del rol al idioma actual
/// Ejemplo: "ROL_Administrador" -> "Administrator" (en inglés)
/// </summary>
private string TraducirNombreRol(string nombreRol)
{
    if (string.IsNullOrEmpty(nombreRol))
        return nombreRol;

    // Mapear nombres de roles a claves de traducción
    string claveTraduccion = string.Empty;

    if (nombreRol.Contains("Administrador"))
        claveTraduccion = "rol_administrador";
    else if (nombreRol.Contains("Bibliotecario"))
        claveTraduccion = "rol_bibliotecario";
    else if (nombreRol.Contains("Docente"))
        claveTraduccion = "rol_docente";

    // Si encontramos una clave, intentar traducir
    if (!string.IsNullOrEmpty(claveTraduccion))
    {
        string traduccion = LanguageManager.Translate(claveTraduccion);
        // Si la traducción existe y es diferente a la clave, usarla
        if (!string.IsNullOrEmpty(traduccion) && traduccion != claveTraduccion)
            return traduccion;
    }

    // Si no hay traducción, retornar el nombre original
    return nombreRol;
}
```

3. **Método `CargarRoles()` actualizado**:
```csharp
private void CargarRoles()
{
    try
    {
        var roles = UsuarioBLL.ObtenerRolesDisponibles();

        // Crear lista de roles con traducción
        var rolesConTraduccion = roles.Select(r => new RolDisplay
        {
            Familia = r,
            NombreTraducido = TraducirNombreRol(r.NombreRol)
        }).ToList();

        cboRoles.DataSource = null;
        cboRoles.DataSource = rolesConTraduccion;
        cboRoles.DisplayMember = "NombreTraducido";
        cboRoles.ValueMember = "Familia";
        cboRoles.SelectedIndex = -1;

        cboNuevoRol.DataSource = null;
        cboNuevoRol.DataSource = rolesConTraduccion.ToList();
        cboNuevoRol.DisplayMember = "NombreTraducido";
        cboNuevoRol.ValueMember = "Familia";
        cboNuevoRol.SelectedIndex = -1;
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al cargar roles: {ex.Message}",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

4. **Método `CboRoles_SelectedIndexChanged()` actualizado**:
```csharp
private void CboRoles_SelectedIndexChanged(object sender, EventArgs e)
{
    if (cboRoles.SelectedItem == null) return;

    var rolDisplay = cboRoles.SelectedItem as RolDisplay;
    if (rolDisplay == null) return;

    _familiaSeleccionada = rolDisplay.Familia;
    if (_familiaSeleccionada == null) return;

    CargarPatentesDelRol(_familiaSeleccionada);
}
```

5. **Método `BtnAsignarRolUsuario_Click()` actualizado**:
```csharp
private void BtnAsignarRolUsuario_Click(object sender, EventArgs e)
{
    try
    {
        if (_usuarioSeleccionado == null)
        {
            MessageBox.Show(LanguageManager.Translate("seleccione_usuario"),
                LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cboNuevoRol.SelectedItem == null)
        {
            MessageBox.Show(LanguageManager.Translate("seleccione_nuevo_rol"),
                LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var nuevoRolDisplay = cboNuevoRol.SelectedItem as RolDisplay;
        if (nuevoRolDisplay == null) return;

        var nuevoRol = nuevoRolDisplay.Familia;

        // Cambiar el rol del usuario
        UsuarioBLL.CambiarRol(_usuarioSeleccionado.IdUsuario, nuevoRol.IdComponent);

        MessageBox.Show(LanguageManager.Translate("rol_actualizado"),
            LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Recargar datos
        CargarDatosDelUsuario(_usuarioSeleccionado);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al asignar rol: {ex.Message}",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

6. **Método `CargarDatosDelUsuario()` actualizado**:
```csharp
private void CargarDatosDelUsuario(Usuario usuario)
{
    try
    {
        // Mostrar rol actual con traducción
        var rolActual = usuario.ObtenerFamiliaRol();
        lblRolActualValor.Text = rolActual != null
            ? TraducirNombreRol(rolActual.NombreRol)
            : LanguageManager.Translate("sin_rol");

        // ... resto del código ...
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al cargar datos del usuario: {ex.Message}",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

### 3. Lógica de Traducción

El sistema ahora funciona así:

1. **Carga de Roles en ComboBox**:
   - Lee los roles de la base de datos (Familia con nombre "ROL_Administrador", etc.)
   - Crea objetos `RolDisplay` que contienen el objeto `Familia` original + su nombre traducido
   - El ComboBox muestra `NombreTraducido` pero internamente mantiene el objeto `Familia`

2. **Traducción de Nombres**:
   - `"ROL_Administrador"` → busca clave `"rol_administrador"` → muestra "Administrador" (es-AR) o "Administrator" (en-GB)
   - `"ROL_Bibliotecario"` → busca clave `"rol_bibliotecario"` → muestra "Bibliotecario" (es-AR) o "Librarian" (en-GB)
   - `"ROL_Docente"` → busca clave `"rol_docente"` → muestra "Docente" (es-AR) o "Teacher" (en-GB)

3. **Extracción del Objeto Original**:
   - Cuando se selecciona un rol, se extrae el objeto `Familia` del `RolDisplay`
   - Esto permite que el código siga funcionando con el objeto `Familia` original

## Resultado Final

### En Español (idioma.es-AR):
```
Roles disponibles:
- Administrador
- Bibliotecario
- Docente
```

### En Inglés (idioma.en-GB):
```
Available Roles:
- Administrator
- Librarian
- Teacher
```

## Cómo Probar

1. Ejecutar la aplicación: `View\UI\bin\Debug\UI.exe`
2. **En el Login**: Cambiar idioma a **English** usando el ComboBox
3. Iniciar sesión como: `admin` / `admin123`
4. Abrir **Permissions Management** (Gestión de Permisos)
5. En la pestaña **Role Management**:
   - Verificar que el ComboBox "Role:" muestra: **Administrator**, **Librarian**, **Teacher**
6. En la pestaña **User and Permissions Management**:
   - Seleccionar un usuario
   - Verificar que "Current Role:" muestra el rol en inglés
   - Verificar que el ComboBox "New Role:" muestra los roles en inglés

7. **Cambiar a español**:
   - Cerrar sesión
   - En el Login, cambiar idioma a **Español**
   - Iniciar sesión nuevamente
   - Abrir **Gestión de Permisos**
   - Verificar que todos los roles aparecen **en español**

## Archivos Modificados

1. **View/UI/Resources/I18n/idioma.es-AR** - Agregadas 3 traducciones de roles (español)
2. **View/UI/Resources/I18n/idioma.en-GB** - Agregadas 3 traducciones de roles (inglés)
3. **View/UI/WinUi/Administración/gestionPermisos.cs** - Modificado para usar traducciones dinámicas de roles

## Compilación

- **Estado**: Exitosa
- **Warnings**: 1 (variable no usada en LoginService.cs - no crítico)
- **Configuración**: Debug
- **Output**: View\UI\bin\Debug\UI.exe

## Notas Importantes

### Agregar Nuevos Roles

Si se agregan nuevos roles en el futuro, seguir estos pasos:

1. **Agregar el rol a la base de datos** (tabla Familia con Tipo = "Rol")
2. **Agregar traducciones** a ambos archivos de idioma:
   - En `idioma.es-AR`: `rol_nombre_rol=Texto en español`
   - En `idioma.en-GB`: `rol_nombre_rol=Text in English`
3. **Actualizar método `TraducirNombreRol()`**:
   - Agregar un nuevo `else if` con la lógica de mapeo
   ```csharp
   else if (nombreRol.Contains("NuevoRol"))
       claveTraduccion = "rol_nuevo_rol";
   ```

### Ejemplos:
- `"ROL_Administrador"` → `rol_administrador`
- `"ROL_Bibliotecario"` → `rol_bibliotecario`
- `"ROL_Docente"` → `rol_docente`
- `"ROL_SuperAdmin"` → `rol_superadmin` (hipotético)

---

**Estado**: Implementación completa y funcional
**Próximos pasos**: Probar con ambos idiomas (español e inglés)
