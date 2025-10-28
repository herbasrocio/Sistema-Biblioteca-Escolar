# Instrucciones para Agregar Bitácoras al Menú Principal

## Fecha: 2025-10-28

---

## 1. RESUMEN

Los formularios de consulta de bitácoras ya están creados y funcionando. Solo falta agregarlos al menú principal del sistema para que los usuarios puedan acceder a ellos.

### Archivos Creados:
- ✅ `View\UI\WinUi\Reportes\ConsultarBitacoraAdmin.cs`
- ✅ `View\UI\WinUi\Reportes\ConsultarBitacoraAdmin.Designer.cs`
- ✅ `View\UI\WinUi\Reportes\ConsultarBitacoraBibliotecario.cs`
- ✅ `View\UI\WinUi\Reportes\ConsultarBitacoraBibliotecario.Designer.cs`

---

## 2. AGREGAR AL MENÚ PRINCIPAL

### Paso 1: Abrir el archivo del menú

Archivo: `View\UI\WinUi\Administración\menu.Designer.cs`

### Paso 2: Agregar ítems de menú en el Designer

Busca la sección donde se definen los menús (probablemente hay un `MenuStrip` o `ToolStrip`). Necesitas agregar dos nuevos ítems de menú:

#### Opción A: Si existe un menú "Reportes" o "Consultas"

Agregar dentro de ese menú:

```csharp
// En el método InitializeComponent(), agregar después de los otros ítems de menú:

// MenuItem para Bitácora Admin
this.consultarBitacoraAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
this.consultarBitacoraAdminToolStripMenuItem.Name = "consultarBitacoraAdminToolStripMenuItem";
this.consultarBitacoraAdminToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
this.consultarBitacoraAdminToolStripMenuItem.Text = "Consultar Bitácora Admin";
this.consultarBitacoraAdminToolStripMenuItem.Click += new System.EventHandler(this.consultarBitacoraAdminToolStripMenuItem_Click);

// MenuItem para Bitácora Bibliotecario
this.consultarBitacoraBibliotecarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
this.consultarBitacoraBibliotecarioToolStripMenuItem.Name = "consultarBitacoraBibliotecarioToolStripMenuItem";
this.consultarBitacoraBibliotecarioToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
this.consultarBitacoraBibliotecarioToolStripMenuItem.Text = "Consultar Bitácora Bibliotecario";
this.consultarBitacoraBibliotecarioToolStripMenuItem.Click += new System.EventHandler(this.consultarBitacoraBibliotecarioToolStripMenuItem_Click);

// Agregar al menú padre (reemplaza "menuReportes" por el nombre real de tu menú):
this.menuReportes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
    // ... otros ítems existentes ...
    this.consultarBitacoraAdminToolStripMenuItem,
    this.consultarBitacoraBibliotecarioToolStripMenuItem
});
```

#### Opción B: Si NO existe un menú "Reportes"

Crear un nuevo menú principal:

```csharp
// Crear el menú Auditoría
this.menuAuditoria = new System.Windows.Forms.ToolStripMenuItem();
this.menuAuditoria.Name = "menuAuditoria";
this.menuAuditoria.Size = new System.Drawing.Size(70, 20);
this.menuAuditoria.Text = "Auditoría";

// Agregar los ítems
this.consultarBitacoraAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
this.consultarBitacoraAdminToolStripMenuItem.Name = "consultarBitacoraAdminToolStripMenuItem";
this.consultarBitacoraAdminToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
this.consultarBitacoraAdminToolStripMenuItem.Text = "Consultar Bitácora Admin";
this.consultarBitacoraAdminToolStripMenuItem.Click += new System.EventHandler(this.consultarBitacoraAdminToolStripMenuItem_Click);

this.consultarBitacoraBibliotecarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
this.consultarBitacoraBibliotecarioToolStripMenuItem.Name = "consultarBitacoraBibliotecarioToolStripMenuItem";
this.consultarBitacoraBibliotecarioToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
this.consultarBitacoraBibliotecarioToolStripMenuItem.Text = "Consultar Bitácora Bibliotecario";
this.consultarBitacoraBibliotecarioToolStripMenuItem.Click += new System.EventHandler(this.consultarBitacoraBibliotecarioToolStripMenuItem_Click);

// Agregar ítems al menú
this.menuAuditoria.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
    this.consultarBitacoraAdminToolStripMenuItem,
    this.consultarBitacoraBibliotecarioToolStripMenuItem
});

// Agregar el menú al MenuStrip principal
this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    // ... otros menús existentes ...
    this.menuAuditoria
});
```

### Paso 3: Declarar las variables en el Designer

Al final del archivo `menu.Designer.cs`, en la sección de declaración de controles:

```csharp
private System.Windows.Forms.ToolStripMenuItem menuAuditoria; // Si creaste un nuevo menú
private System.Windows.Forms.ToolStripMenuItem consultarBitacoraAdminToolStripMenuItem;
private System.Windows.Forms.ToolStripMenuItem consultarBitacoraBibliotecarioToolStripMenuItem;
```

### Paso 4: Implementar los manejadores de eventos

Archivo: `View\UI\WinUi\Administración\menu.cs`

Agregar al principio del archivo (después de los using existentes):

```csharp
using UI.WinUi.Reportes;
```

Agregar los métodos manejadores de eventos al final de la clase:

```csharp
private void consultarBitacoraAdminToolStripMenuItem_Click(object sender, EventArgs e)
{
    try
    {
        // Verificar permisos
        if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("consultarBitacoraAdmin"))
        {
            MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                LanguageManager.Translate("error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        // Abrir formulario
        ConsultarBitacoraAdmin frmBitacoraAdmin = new ConsultarBitacoraAdmin(_usuarioLogueado);
        frmBitacoraAdmin.ShowDialog();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al abrir la bitácora de administrador: {ex.Message}",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void consultarBitacoraBibliotecarioToolStripMenuItem_Click(object sender, EventArgs e)
{
    try
    {
        // Verificar permisos
        if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("consultarBitacoraBibliotecario"))
        {
            MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                LanguageManager.Translate("error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        // Abrir formulario
        ConsultarBitacoraBibliotecario frmBitacoraBibliotecario = new ConsultarBitacoraBibliotecario(_usuarioLogueado);
        frmBitacoraBibliotecario.ShowDialog();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error al abrir la bitácora de bibliotecario: {ex.Message}",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

---

## 3. ALTERNATIVA: Agregar usando el Diseñador Visual de Visual Studio

Si prefieres usar el diseñador visual de Visual Studio en lugar de editar manualmente:

### Pasos:

1. **Abrir `menu.Designer.cs` en el diseñador:**
   - En Visual Studio, hacer clic derecho en `menu.cs` → "View Designer"

2. **Buscar el MenuStrip en el formulario**

3. **Agregar nuevo menú o ítem:**
   - Si existe un menú "Reportes" o similar:
     - Hacer clic en ese menú
     - Hacer clic en "Add Item" o escribir directamente en el espacio vacío
     - Escribir "Consultar Bitácora Admin"
     - Repetir para "Consultar Bitácora Bibliotecario"

   - Si NO existe un menú adecuado:
     - Hacer clic al final del MenuStrip (en la última posición)
     - Escribir "Auditoría"
     - Hacer clic en "Auditoría" para expandir
     - Agregar "Consultar Bitácora Admin"
     - Agregar "Consultar Bitácora Bibliotecario"

4. **Configurar propiedades:**
   - Seleccionar "Consultar Bitácora Admin"
   - En la ventana de Propiedades:
     - Name: `consultarBitacoraAdminToolStripMenuItem`
     - Text: `Consultar Bitácora Admin`

   - Seleccionar "Consultar Bitácora Bibliotecario"
   - En la ventana de Propiedades:
     - Name: `consultarBitacoraBibliotecarioToolStripMenuItem`
     - Text: `Consultar Bitácora Bibliotecario`

5. **Agregar manejadores de eventos:**
   - Seleccionar "Consultar Bitácora Admin"
   - En la ventana de Propiedades, hacer clic en el icono de rayo (Events)
   - Hacer doble clic en el evento "Click"
   - Visual Studio creará automáticamente el método `consultarBitacoraAdminToolStripMenuItem_Click`

   - Repetir para "Consultar Bitácora Bibliotecario"

6. **Implementar el código en los manejadores** (usar el código del Paso 4 anterior)

---

## 4. VERIFICAR LA IMPLEMENTACIÓN

### Checklist:

- [ ] Los ítems de menú aparecen en el menú principal
- [ ] Al hacer clic en "Consultar Bitácora Admin":
  - [ ] Si el usuario NO tiene permiso → Muestra mensaje de error
  - [ ] Si el usuario SÍ tiene permiso → Abre el formulario
- [ ] Al hacer clic en "Consultar Bitácora Bibliotecario":
  - [ ] Si el usuario NO tiene permiso → Muestra mensaje de error
  - [ ] Si el usuario SÍ tiene permiso → Abre el formulario
- [ ] Los formularios cargan datos correctamente
- [ ] Los filtros funcionan
- [ ] El botón "Volver" cierra el formulario
- [ ] Las traducciones se aplican correctamente (español/inglés)

---

## 5. SOLUCIÓN DE PROBLEMAS COMUNES

### Error: "The name 'ConsultarBitacoraAdmin' does not exist in the current context"

**Solución:** Agregar el using al principio del archivo `menu.cs`:
```csharp
using UI.WinUi.Reportes;
```

### Error: "The type or namespace name 'BitacoraAdminBLL' could not be found"

**Solución:** Asegurarse de que el proyecto UI tenga referencia al proyecto BLL:
1. En Visual Studio, hacer clic derecho en el proyecto UI → "Add" → "Reference"
2. Buscar el proyecto "BLL" y marcarlo
3. Hacer clic en "OK"

### Los menús no aparecen visualmente

**Solución:** Verificar que:
1. Los ítems estén agregados al menú padre correcto
2. El menú padre esté visible (`Visible = true`)
3. Los permisos estén correctamente asignados en la base de datos

### Error de permisos: "Usuario no tiene permiso"

**Solución:** Ejecutar nuevamente el script de permisos:
```powershell
sqlcmd -S localhost -E -i "Database\08_AgregarPermisosBitacora.sql"
```

Y verificar que el usuario actual tenga asignada la familia `ROL_Administrador` o `ROL_Bibliotecario`.

---

## 6. EJEMPLO COMPLETO DE INTEGRACIÓN

Si quieres ver un ejemplo completo de cómo están integrados otros reportes en el menú, puedes revisar:

- `View\UI\WinUi\Administración\menu.Designer.cs` (buscar "Reporte")
- `View\UI\WinUi\Administración\menu.cs` (buscar "Reporte")

Y usar el mismo patrón para las bitácoras.

---

## 7. RESULTADO ESPERADO

Después de implementar estos cambios, el menú principal debería verse así:

```
[Archivo]  [Administración]  [Transacciones]  [Reportes]  [Auditoría]  [Ayuda]
                                                            |
                                                            ├─ Consultar Bitácora Admin
                                                            └─ Consultar Bitácora Bibliotecario
```

O si agregaste los ítems a un menú existente:

```
[Archivo]  [Administración]  [Transacciones]  [Reportes]  [Ayuda]
                                                    |
                                                    ├─ Reporte Préstamos Activos
                                                    ├─ Reporte Materiales Más Prestados
                                                    ├─ Reporte Uso por Grado
                                                    ├─ (separator)
                                                    ├─ Consultar Bitácora Admin
                                                    └─ Consultar Bitácora Bibliotecario
```

---

## 8. PRÓXIMOS PASOS

Una vez agregados los formularios al menú:

1. **Compilar el proyecto:** `msbuild "Sistema Biblioteca Escolar.sln" /t:Build /p:Configuration=Debug`
2. **Ejecutar la aplicación:** `View\UI\bin\Debug\UI.exe`
3. **Probar los formularios:**
   - Login con usuario admin
   - Navegar al menú de bitácoras
   - Verificar que se abren correctamente
   - Probar los filtros
   - Verificar permisos con diferentes usuarios

4. **Integrar logging automático:** Seguir las instrucciones en `EJEMPLO_INTEGRACION_BITACORA.md`

---

**Autor: Sistema Biblioteca Escolar**
**Fecha: 2025-10-28**
