using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Services.DomainModel.Security.Composite;
using Services.Services;
using Services.BLL;
using BLL;

namespace UI.WinUi.Administrador
{
    public partial class gestionPermisos : BaseForm
    {
        private Usuario _usuarioLogueado;
        private Familia _familiaSeleccionada;
        private Usuario _usuarioSeleccionado;
        private readonly BitacoraSeguridadBLL _bitacoraSeguridadBLL;

        public gestionPermisos()
        {
            InitializeComponent();
        }

        public gestionPermisos(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _bitacoraSeguridadBLL = new BitacoraSeguridadBLL();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Establecer textos con codificación correcta
            EstablecerTextos();

            this.Load += GestionPermisos_Load;

            // Eventos de tabs
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Eventos de gestión de roles
            cboRoles.SelectedIndexChanged += CboRoles_SelectedIndexChanged;
            btnGuardarRol.Click += BtnGuardarRol_Click;
            btnCrearRol.Click += BtnCrearRol_Click;
            btnEliminarRol.Click += BtnEliminarRol_Click;

            // Eventos de gestión de usuarios
            cboUsuarios.SelectedIndexChanged += CboUsuarios_SelectedIndexChanged;
            btnAsignarRolUsuario.Click += BtnAsignarRolUsuario_Click;
            btnGuardarPermisosUsuario.Click += BtnGuardarPermisosUsuario_Click;
        }

        private void EstablecerTextos()
        {
            // Establecer textos con acentos correctos
            this.Text = "Gestión de Permisos";
            tabGestionRoles.Text = "Gestión de Roles";
            groupBoxRol.Text = "Seleccionar Rol";
            groupBoxPatentesRol.Text = "Permisos del Rol";
            tabGestionUsuarios.Text = "Gestión de Usuarios";
            groupBoxUsuario.Text = "Seleccionar Usuario";
            groupBoxPatentesUsuario.Text = "Permisos Adicionales (independientes del rol)";
        }

        private void GestionPermisos_Load(object sender, EventArgs e)
        {
            // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
            CargarPatentesDisponibles();
            CargarRoles();
            CargarUsuarios();

            // Forzar la carga de permisos del rol Administrador si está seleccionado
            if (cboRoles.SelectedItem != null)
            {
                var rolDisplay = cboRoles.SelectedItem as RolDisplay;
                if (rolDisplay != null && rolDisplay.Familia != null)
                {
                    _familiaSeleccionada = rolDisplay.Familia;
                    CargarPatentesDelRol(_familiaSeleccionada);
                }
            }
        }

        protected override void AplicarTraducciones()
        {
            // Sobrescribir con traducciones si están disponibles
            this.Text = LanguageManager.Translate("gestion_permisos");

            // Tabs
            tabGestionRoles.Text = LanguageManager.Translate("gestion_roles");
            tabGestionUsuarios.Text = LanguageManager.Translate("gestion_usuarios_permisos");

            // Tab Roles
            groupBoxRol.Text = LanguageManager.Translate("seleccionar_rol");
            lblRol.Text = LanguageManager.Translate("rol") + ":";
            groupBoxPatentesRol.Text = LanguageManager.Translate("permisos_rol");
            btnGuardarRol.Text = LanguageManager.Translate("guardar_cambios");

            // Tab Usuarios
            groupBoxUsuario.Text = LanguageManager.Translate("seleccionar_usuario");
            lblUsuario.Text = LanguageManager.Translate("usuario") + ":";
            groupBoxRolUsuario.Text = LanguageManager.Translate("rol_asignado");
            lblRolActual.Text = LanguageManager.Translate("rol_actual") + ":";
            lblNuevoRol.Text = LanguageManager.Translate("nuevo_rol") + ":";
            btnAsignarRolUsuario.Text = LanguageManager.Translate("asignar_rol");
            groupBoxPatentesUsuario.Text = LanguageManager.Translate("permisos_adicionales");
            btnGuardarPermisosUsuario.Text = LanguageManager.Translate("guardar_cambios");
        }

        #region Carga de Datos

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

                // Seleccionar el rol de Administrador por defecto
                var rolAdministrador = rolesConTraduccion.FirstOrDefault(r =>
                    r.Familia.NombreRol != null &&
                    r.Familia.NombreRol.Contains("Administrador"));

                if (rolAdministrador != null)
                {
                    cboRoles.SelectedItem = rolAdministrador;
                }
                else
                {
                    cboRoles.SelectedIndex = -1;
                }

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

        // Clase auxiliar para mostrar roles con traducción
        private class RolDisplay
        {
            public Familia Familia { get; set; }
            public string NombreTraducido { get; set; }
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = UsuarioBLL.ObtenerTodosLosUsuarios();

                cboUsuarios.DataSource = null;
                cboUsuarios.DataSource = usuarios.ToList();
                cboUsuarios.DisplayMember = "Nombre";
                cboUsuarios.ValueMember = "IdUsuario";
                cboUsuarios.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarPatentesDisponibles()
        {
            try
            {
                var patentes = UsuarioBLL.ObtenerTodasLasPatentes();

                // Filtrar solo las patentes que deben mostrarse:
                // 1. Patentes del menú principal (FormName = "menu")
                // 2. Patentes de reportes (FormName contiene "reporte")
                // 3. Patentes de bitácoras (FormName contiene "bitacora")
                // 4. Patentes específicas de formularios (renovarPrestamo, FrmGestionBackup, etc.)
                // Excluir: Patentes internas de gestión (FormName empieza con "frm" excepto FrmGestionBackup)
                var patentesVisibles = patentes.Where(p =>
                    p.FormName == "menu" ||
                    p.FormName.ToLower().Contains("reporte") ||
                    p.FormName.ToLower().Contains("bitacora") ||
                    p.FormName == "renovarPrestamo" ||
                    p.FormName == "FrmGestionBackup"
                ).ToList();

                // Ordenar patentes: primero las del menú, luego reportes, luego bitácoras
                var patentesOrdenadas = patentesVisibles
                    .OrderBy(p => GetCategoriaPatente(p.FormName))
                    .ThenBy(p => p.Orden)
                    .ThenBy(p => p.MenuItemName)
                    .ToList();

                // Cargar en CheckedListBox de roles
                checkedListPatentesRol.Items.Clear();
                foreach (var patente in patentesOrdenadas)
                {
                    // Intentar obtener traducción, si no existe usar el texto de la BD
                    string claveTraduccion = ObtenerClaveTraduccionPermiso(patente.MenuItemName);
                    string textoTraducido = LanguageManager.Translate(claveTraduccion);

                    // Si la traducción no existe, usar el formato original
                    string textoMostrar = !string.IsNullOrEmpty(textoTraducido) && textoTraducido != claveTraduccion
                        ? textoTraducido
                        : patente.MenuItemName;

                    checkedListPatentesRol.Items.Add(new PatenteDisplay { Patente = patente, TextoMostrar = textoMostrar }, false);
                }

                // Cargar en CheckedListBox de usuarios
                checkedListPatentesUsuario.Items.Clear();
                foreach (var patente in patentesOrdenadas)
                {
                    // Intentar obtener traducción, si no existe usar el texto de la BD
                    string claveTraduccion = ObtenerClaveTraduccionPermiso(patente.MenuItemName);
                    string textoTraducido = LanguageManager.Translate(claveTraduccion);

                    // Si la traducción no existe, usar el formato original
                    string textoMostrar = !string.IsNullOrEmpty(textoTraducido) && textoTraducido != claveTraduccion
                        ? textoTraducido
                        : patente.MenuItemName;

                    checkedListPatentesUsuario.Items.Add(new PatenteDisplay { Patente = patente, TextoMostrar = textoMostrar }, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar patentes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Obtiene la categoría de una patente para ordenamiento
        /// </summary>
        private int GetCategoriaPatente(string formName)
        {
            if (string.IsNullOrEmpty(formName))
                return 999;

            // Orden de prioridad:
            // 1. Patentes del menú principal
            if (formName == "menu")
                return 1;

            // 2. Patentes de reportes
            if (formName.ToLower().Contains("reporte"))
                return 2;

            // 3. Patentes de bitácoras
            if (formName.ToLower().Contains("bitacora"))
                return 3;

            // 4. Funciones administrativas
            if (formName == "renovarPrestamo" || formName == "FrmGestionBackup")
                return 4;

            // 5. Otras patentes (no deberían aparecer debido al filtro)
            return 999;
        }

        /// <summary>
        /// Convierte el nombre del permiso a su clave de traducción
        /// Ejemplo: "Consultar Material" -> "permiso_consultar_material"
        /// </summary>
        private string ObtenerClaveTraduccionPermiso(string nombrePermiso)
        {
            if (string.IsNullOrEmpty(nombrePermiso))
                return string.Empty;

            // Convertir a minúsculas, reemplazar espacios por guiones bajos, quitar acentos
            string clave = nombrePermiso.ToLower()
                .Replace(" ", "_")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u")
                .Replace("ñ", "n");

            return $"permiso_{clave}";
        }

        // Clase auxiliar para mostrar las patentes con formato personalizado
        private class PatenteDisplay
        {
            public Patente Patente { get; set; }
            public string TextoMostrar { get; set; }

            public override string ToString()
            {
                return TextoMostrar;
            }
        }

        #endregion

        #region Gestión de Roles

        private void CboRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRoles.SelectedItem == null) return;

            var rolDisplay = cboRoles.SelectedItem as RolDisplay;
            if (rolDisplay == null) return;

            _familiaSeleccionada = rolDisplay.Familia;
            if (_familiaSeleccionada == null) return;

            CargarPatentesDelRol(_familiaSeleccionada);
        }

        private void CargarPatentesDelRol(Familia rol)
        {
            try
            {
                // Obtener patentes directas del rol desde la BLL (no recursivas)
                var patentesDelRol = FamiliaBLL.ObtenerPatentesDirectasDeFamilia(rol.IdComponent);

                // Desmarcar todas
                for (int i = 0; i < checkedListPatentesRol.Items.Count; i++)
                {
                    checkedListPatentesRol.SetItemChecked(i, false);
                }

                // Marcar las que tiene el rol
                for (int i = 0; i < checkedListPatentesRol.Items.Count; i++)
                {
                    var displayItem = checkedListPatentesRol.Items[i] as PatenteDisplay;
                    if (displayItem != null && patentesDelRol.Any(p => p.IdComponent == displayItem.Patente.IdComponent))
                    {
                        checkedListPatentesRol.SetItemChecked(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar patentes del rol: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCrearRol_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar nombre del nuevo rol
                string nombreRol = Microsoft.VisualBasic.Interaction.InputBox(
                    LanguageManager.Translate("ingrese_nombre_rol"),
                    LanguageManager.Translate("crear_rol"),
                    "",
                    -1, -1);

                // Validar que no esté vacío
                if (string.IsNullOrWhiteSpace(nombreRol))
                {
                    return; // Usuario canceló o dejó vacío
                }

                // Crear el rol
                var nuevoRol = FamiliaBLL.CrearRol(nombreRol);

                // Registrar en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Permisos",
                    accion: "Creación de rol",
                    detalle: $"Nuevo rol creado: '{nuevoRol.Nombre}'",
                    idUsuario: _usuarioLogueado.IdUsuario,
                    nombreUsuario: _usuarioLogueado.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show(
                    LanguageManager.Translate("rol_creado_exitosamente"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Recargar roles
                CargarRoles();

                // Seleccionar el nuevo rol
                var rolDisplay = (cboRoles.DataSource as List<RolDisplay>)?.FirstOrDefault(r => r.Familia.IdComponent == nuevoRol.IdComponent);
                if (rolDisplay != null)
                {
                    cboRoles.SelectedItem = rolDisplay;
                }
            }
            catch (DomainModel.Exceptions.ValidacionException vex)
            {
                MessageBox.Show(vex.Message,
                    LanguageManager.Translate("error_validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Registrar error en bitácora
                _bitacoraSeguridadBLL.RegistrarError(
                    modulo: "Permisos",
                    accion: "Error al crear rol",
                    detalle: $"Error al crear nuevo rol. Error: {ex.Message}. StackTrace: {ex.StackTrace}",
                    idUsuario: _usuarioLogueado?.IdUsuario,
                    nombreUsuario: _usuarioLogueado?.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show($"Error al crear rol: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminarRol_Click(object sender, EventArgs e)
        {
            try
            {
                if (_familiaSeleccionada == null)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_rol"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirmar eliminación
                var resultado = MessageBox.Show(
                    $"¿Está seguro de que desea eliminar el rol '{_familiaSeleccionada.NombreRol}'?\n\n" +
                    "Esta acción no se puede deshacer.",
                    LanguageManager.Translate("confirmar_eliminacion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (resultado != DialogResult.Yes)
                {
                    return;
                }

                string nombreRolEliminado = _familiaSeleccionada.NombreRol;
                Guid idRolEliminado = _familiaSeleccionada.IdComponent;

                // Eliminar el rol
                FamiliaBLL.EliminarRol(_familiaSeleccionada.IdComponent);

                // Registrar en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Permisos",
                    accion: "Eliminación de rol",
                    detalle: $"Rol eliminado: '{nombreRolEliminado}'",
                    idUsuario: _usuarioLogueado.IdUsuario,
                    nombreUsuario: _usuarioLogueado.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show(
                    LanguageManager.Translate("rol_eliminado_exitosamente"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Limpiar selección
                _familiaSeleccionada = null;

                // Recargar roles
                CargarRoles();
            }
            catch (DomainModel.Exceptions.ValidacionException vex)
            {
                MessageBox.Show(vex.Message,
                    LanguageManager.Translate("error_validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Registrar error en bitácora
                _bitacoraSeguridadBLL.RegistrarError(
                    modulo: "Permisos",
                    accion: "Error al eliminar rol",
                    detalle: $"Error al eliminar rol '{_familiaSeleccionada?.NombreRol}'. Error: {ex.Message}. StackTrace: {ex.StackTrace}",
                    idUsuario: _usuarioLogueado?.IdUsuario,
                    nombreUsuario: _usuarioLogueado?.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show($"Error al eliminar rol: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardarRol_Click(object sender, EventArgs e)
        {
            try
            {
                if (_familiaSeleccionada == null)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_rol"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener patentes actuales antes del cambio (para la bitácora)
                var patentesAnteriores = FamiliaBLL.ObtenerPatentesDirectasDeFamilia(_familiaSeleccionada.IdComponent);

                // Obtener patentes seleccionadas
                var patentesSeleccionadas = new List<Patente>();
                foreach (var item in checkedListPatentesRol.CheckedItems)
                {
                    if (item is PatenteDisplay displayItem)
                        patentesSeleccionadas.Add(displayItem.Patente);
                }

                // Actualizar permisos del rol
                FamiliaBLL.ActualizarPatentesDeRol(_familiaSeleccionada.IdComponent, patentesSeleccionadas);

                // Registrar en bitácora
                var patentesAgregadas = patentesSeleccionadas.Where(p => !patentesAnteriores.Any(pa => pa.IdComponent == p.IdComponent)).ToList();
                var patentesQuitadas = patentesAnteriores.Where(pa => !patentesSeleccionadas.Any(p => p.IdComponent == pa.IdComponent)).ToList();

                string detalle = $"Rol '{_familiaSeleccionada.NombreRol}' modificado. ";
                if (patentesAgregadas.Any())
                    detalle += $"Permisos agregados: {string.Join(", ", patentesAgregadas.Select(p => p.MenuItemName))}. ";
                if (patentesQuitadas.Any())
                    detalle += $"Permisos quitados: {string.Join(", ", patentesQuitadas.Select(p => p.MenuItemName))}. ";

                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Permisos",
                    accion: "Modificación de permisos de rol",
                    detalle: detalle,
                    idUsuario: _usuarioLogueado.IdUsuario,
                    nombreUsuario: _usuarioLogueado.Nombre,
                    gravedad: "Alto"
                );

                // Notificar a todos los usuarios que los permisos han cambiado (patrón Observer)
                PermissionManager.Instance.NotifyAllUsersPermissionsChanged();

                string mensaje = LanguageManager.Translate("permisos_actualizados") + "\n\n" +
                                 "Los usuarios afectados verán los cambios actualizados automáticamente.";
                MessageBox.Show(mensaje,
                    LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Registrar error crítico en bitácora
                _bitacoraSeguridadBLL.RegistrarError(
                    modulo: "Permisos",
                    accion: "Error al guardar permisos de rol",
                    detalle: $"Error al modificar permisos del rol '{_familiaSeleccionada?.NombreRol}'. Error: {ex.Message}. StackTrace: {ex.StackTrace}",
                    idUsuario: _usuarioLogueado?.IdUsuario,
                    nombreUsuario: _usuarioLogueado?.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show($"Error al guardar permisos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Gestión de Usuarios

        private void CboUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUsuarios.SelectedItem == null) return;

            _usuarioSeleccionado = cboUsuarios.SelectedItem as Usuario;
            if (_usuarioSeleccionado == null) return;

            CargarDatosDelUsuario(_usuarioSeleccionado);
        }

        private void CargarDatosDelUsuario(Usuario usuario)
        {
            try
            {
                // Mostrar rol actual con traducción
                var rolActual = usuario.ObtenerFamiliaRol();
                lblRolActualValor.Text = rolActual != null
                    ? TraducirNombreRol(rolActual.NombreRol)
                    : LanguageManager.Translate("sin_rol");

                // Obtener patentes directas del usuario (no heredadas del rol)
                var patentesDirectas = UsuarioBLL.ObtenerPatentesDelUsuario(usuario.IdUsuario);

                // Desmarcar todas
                for (int i = 0; i < checkedListPatentesUsuario.Items.Count; i++)
                {
                    checkedListPatentesUsuario.SetItemChecked(i, false);
                }

                // Marcar las patentes directas
                for (int i = 0; i < checkedListPatentesUsuario.Items.Count; i++)
                {
                    var displayItem = checkedListPatentesUsuario.Items[i] as PatenteDisplay;
                    if (displayItem != null && patentesDirectas.Any(p => p.IdComponent == displayItem.Patente.IdComponent))
                    {
                        checkedListPatentesUsuario.SetItemChecked(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del usuario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                // Obtener rol anterior para la bitácora
                var rolAnterior = _usuarioSeleccionado.ObtenerFamiliaRol();
                string nombreRolAnterior = rolAnterior != null ? rolAnterior.NombreRol : "Sin rol";

                // Cambiar el rol del usuario
                UsuarioBLL.CambiarRol(_usuarioSeleccionado.IdUsuario, nuevoRol.IdComponent);

                // Registrar en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Usuarios",
                    accion: "Cambio de rol de usuario",
                    detalle: $"Usuario '{_usuarioSeleccionado.Nombre}' cambió de rol '{nombreRolAnterior}' a '{nuevoRol.NombreRol}'",
                    idUsuario: _usuarioLogueado.IdUsuario,
                    nombreUsuario: _usuarioLogueado.Nombre,
                    gravedad: "Alto"
                );

                // Notificar al usuario que su rol cambió (patrón Observer)
                PermissionManager.Instance.NotifyPermissionsChanged(_usuarioSeleccionado.IdUsuario);

                string mensaje = LanguageManager.Translate("rol_actualizado") + "\n\n" +
                                 "Los cambios se aplicarán automáticamente si el usuario tiene una sesión activa.";
                MessageBox.Show(mensaje,
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

        private void BtnGuardarPermisosUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (_usuarioSeleccionado == null)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_usuario"),
                        LanguageManager.Translate("validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener patentes seleccionadas
                var patentesSeleccionadas = new List<Patente>();
                foreach (var item in checkedListPatentesUsuario.CheckedItems)
                {
                    if (item is PatenteDisplay displayItem)
                        patentesSeleccionadas.Add(displayItem.Patente);
                }

                // Obtener patentes actuales del usuario
                var patentesActuales = UsuarioBLL.ObtenerPatentesDelUsuario(_usuarioSeleccionado.IdUsuario);

                // Listas para la bitácora
                var patentesAgregadas = new List<Patente>();
                var patentesQuitadas = new List<Patente>();

                // Quitar patentes que ya no están seleccionadas
                foreach (var patenteActual in patentesActuales)
                {
                    if (!patentesSeleccionadas.Any(p => p.IdComponent == patenteActual.IdComponent))
                    {
                        UsuarioBLL.QuitarPatente(_usuarioSeleccionado.IdUsuario, patenteActual.IdComponent);
                        patentesQuitadas.Add(patenteActual);
                    }
                }

                // Agregar patentes nuevas
                foreach (var patenteSeleccionada in patentesSeleccionadas)
                {
                    if (!patentesActuales.Any(p => p.IdComponent == patenteSeleccionada.IdComponent))
                    {
                        UsuarioBLL.AsignarPatente(_usuarioSeleccionado.IdUsuario, patenteSeleccionada.IdComponent);
                        patentesAgregadas.Add(patenteSeleccionada);
                    }
                }

                // Registrar en bitácora solo si hubo cambios
                if (patentesAgregadas.Any() || patentesQuitadas.Any())
                {
                    string detalle = $"Permisos adicionales del usuario '{_usuarioSeleccionado.Nombre}' modificados. ";
                    if (patentesAgregadas.Any())
                        detalle += $"Permisos agregados: {string.Join(", ", patentesAgregadas.Select(p => p.MenuItemName))}. ";
                    if (patentesQuitadas.Any())
                        detalle += $"Permisos quitados: {string.Join(", ", patentesQuitadas.Select(p => p.MenuItemName))}. ";

                    _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                        modulo: "Usuarios",
                        accion: "Modificación de permisos adicionales de usuario",
                        detalle: detalle,
                        idUsuario: _usuarioLogueado.IdUsuario,
                        nombreUsuario: _usuarioLogueado.Nombre,
                        gravedad: "Alto"
                    );

                    // Notificar al usuario específico que sus permisos cambiaron (patrón Observer)
                    PermissionManager.Instance.NotifyPermissionsChanged(_usuarioSeleccionado.IdUsuario);
                }

                string mensaje = LanguageManager.Translate("permisos_actualizados") + "\n\n" +
                                 "Los cambios se aplicarán automáticamente si el usuario tiene una sesión activa.";
                MessageBox.Show(mensaje,
                    LanguageManager.Translate("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Registrar error crítico en bitácora
                _bitacoraSeguridadBLL.RegistrarError(
                    modulo: "Permisos",
                    accion: "Error al guardar permisos adicionales de usuario",
                    detalle: $"Error al modificar permisos del usuario '{_usuarioSeleccionado?.Nombre}'. Error: {ex.Message}. StackTrace: {ex.StackTrace}",
                    idUsuario: _usuarioLogueado?.IdUsuario,
                    nombreUsuario: _usuarioLogueado?.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show($"Error al guardar permisos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Resetear selecciones al cambiar de tab
            if (tabControl.SelectedTab == tabGestionRoles)
            {
                // Si no hay ningún rol seleccionado, seleccionar Administrador por defecto
                if (cboRoles.SelectedIndex == -1)
                {
                    var rolesConTraduccion = cboRoles.DataSource as List<RolDisplay>;
                    if (rolesConTraduccion != null)
                    {
                        var rolAdministrador = rolesConTraduccion.FirstOrDefault(r =>
                            r.Familia.NombreRol != null &&
                            r.Familia.NombreRol.Contains("Administrador"));

                        if (rolAdministrador != null)
                        {
                            cboRoles.SelectedItem = rolAdministrador;
                        }
                    }
                }
            }
            else if (tabControl.SelectedTab == tabGestionUsuarios)
            {
                cboUsuarios.SelectedIndex = -1;
            }
        }

        private void checkedListPatentesRol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
