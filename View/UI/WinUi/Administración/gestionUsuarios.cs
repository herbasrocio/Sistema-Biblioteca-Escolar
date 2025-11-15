using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using ServicesSecurity.BLL;
using ServicesSecurity.Services;
using BLL;

namespace UI.WinUi.Administrador
{
    public partial class gestionUsuarios : BaseForm
    {
        private Usuario _usuarioLogueado;
        private Usuario _usuarioSeleccionado;
        private bool _modoEdicion = false;
        private const string PLACEHOLDER_PASSWORD = "••••••••";
        private BitacoraSeguridadBLL _bitacoraSeguridadBLL;

        // Clase wrapper para traducir nombres de roles en el ComboBox
        private class RolComboBoxItem
        {
            public Familia Familia { get; set; }
            public string NombreTraducido { get; set; }

            public override string ToString()
            {
                return NombreTraducido;
            }
        }

        public gestionUsuarios()
        {
            InitializeComponent();
        }

        public gestionUsuarios(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _bitacoraSeguridadBLL = new BitacoraSeguridadBLL();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configurar eventos
            this.Load += GestionUsuarios_Load;
            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;
            btnBuscar.Click += BtnBuscar_Click;
            dgvUsuarios.SelectionChanged += DgvUsuarios_SelectionChanged;

            // Configurar estilo visual del DataGridView
            ConfigurarEstiloDataGridView();

            // Cargar roles (Familias) en ComboBox
            CargarRolesEnComboBox();

            // Configurar estado inicial
            BloquearCampos();
            btnGuardar.Enabled = false;
        }

        private void ConfigurarEstiloDataGridView()
        {
            // Colores de selección
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            // Estilo alternado de filas
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 246, 247);
            dgvUsuarios.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;

            // Estilo del header
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;

            // Borde y líneas
            dgvUsuarios.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.GridColor = System.Drawing.Color.FromArgb(189, 195, 199);
        }

        private void CargarRolesEnComboBox()
        {
            try
            {
                var rolesDisponibles = UsuarioBLL.ObtenerRolesDisponibles();

                comboBoxPerfil.Items.Clear();

                foreach (var rol in rolesDisponibles)
                {
                    string nombreTraducido = TraducirNombreRol(rol.NombreRol);
                    comboBoxPerfil.Items.Add(new RolComboBoxItem
                    {
                        Familia = rol,
                        NombreTraducido = nombreTraducido
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TraducirNombreRol(string nombreRol)
        {
            // Mapeo de nombres de roles a sus keys de traducción
            switch (nombreRol?.ToLower())
            {
                case "administrador":
                    return LanguageManager.Translate("rol_administrador");
                case "bibliotecario":
                    return LanguageManager.Translate("rol_bibliotecario");
                case "docente":
                    return LanguageManager.Translate("rol_docente");
                case "ayudante":
                    return LanguageManager.Translate("rol_ayudante");
                default:
                    return nombreRol ?? "Sin rol";
            }
        }


        private void GestionUsuarios_Load(object sender, EventArgs e)
        {
            // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
            CargarTodosLosUsuarios();
        }

        protected override void AplicarTraducciones()
        {
            try
            {
                // Traducir título del formulario
                this.Text = LanguageManager.Translate("gestion_usuarios");

                // Traducir GroupBox
                groupBoxDatosUsuario.Text = LanguageManager.Translate("datos_usuario");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                // Traducir Labels
                label1.Text = LanguageManager.Translate("buscar_usuario") + ":";
                lblNombreUsuario.Text = LanguageManager.Translate("nombre_usuario");
                lblEmail.Text = LanguageManager.Translate("email");
                lblContraseña.Text = LanguageManager.Translate("contraseña");
                lblPerfil.Text = LanguageManager.Translate("rol");

                // Traducir Botones
                btnNuevo.Text = LanguageManager.Translate("nuevo");
                btnGuardar.Text = LanguageManager.Translate("guardar");
                btnModificar.Text = LanguageManager.Translate("editar");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnVolver.Text = LanguageManager.Translate("volver");

                // Traducir encabezados del DataGridView
                TraducirColumnasDataGridView();

                // Recargar ComboBox de roles con traducciones
                RecargarRolesConTraducciones();

                // Recargar DataGridView para actualizar los nombres de roles traducidos
                if (dgvUsuarios.DataSource != null)
                {
                    CargarTodosLosUsuarios();
                }
            }
            catch (Exception ex)
            {
                // Log error pero no interrumpir la carga del formulario
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void RecargarRolesConTraducciones()
        {
            // Guardar el rol seleccionado actualmente (si hay uno)
            RolComboBoxItem rolSeleccionadoActual = comboBoxPerfil.SelectedItem as RolComboBoxItem;
            Guid? idRolSeleccionado = rolSeleccionadoActual?.Familia.IdComponent;

            // Recargar el ComboBox con traducciones actualizadas
            CargarRolesEnComboBox();

            // Restaurar la selección si había un rol seleccionado
            if (idRolSeleccionado.HasValue)
            {
                foreach (RolComboBoxItem item in comboBoxPerfil.Items)
                {
                    if (item.Familia.IdComponent == idRolSeleccionado.Value)
                    {
                        comboBoxPerfil.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Traduce los encabezados de las columnas del DataGridView
        /// </summary>
        private void TraducirColumnasDataGridView()
        {
            if (dgvUsuarios.Columns.Count > 0)
            {
                if (dgvUsuarios.Columns["Nombre"] != null)
                    dgvUsuarios.Columns["Nombre"].HeaderText = LanguageManager.Translate("nombre");

                if (dgvUsuarios.Columns["Email"] != null)
                    dgvUsuarios.Columns["Email"].HeaderText = LanguageManager.Translate("email");

                if (dgvUsuarios.Columns["Rol"] != null)
                    dgvUsuarios.Columns["Rol"].HeaderText = LanguageManager.Translate("rol");
            }
        }

        #region Carga de Datos

        private void CargarTodosLosUsuarios()
        {
            try
            {
                var usuarios = UsuarioBLL.ObtenerTodosLosUsuarios();

                // Configurar DataGridView
                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = usuarios.Select(u => new
                {
                    u.IdUsuario,
                    Nombre = u.Nombre,
                    Email = u.Email ?? "",
                    Rol = TraducirNombreRol(u.ObtenerNombreRol()) ?? LanguageManager.Translate("sin_rol")
                }).ToList();

                // Ocultar columna ID
                if (dgvUsuarios.Columns["IdUsuario"] != null)
                    dgvUsuarios.Columns["IdUsuario"].Visible = false;

                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Búsqueda

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreBusqueda = txtBuscarPorUsuario.Text.Trim();

                if (string.IsNullOrWhiteSpace(nombreBusqueda))
                {
                    MessageBox.Show("Ingrese un nombre de usuario para buscar",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var usuario = UsuarioBLL.ObtenerUsuarioPorNombre(nombreBusqueda);

                // Cargar datos en los campos
                _usuarioSeleccionado = usuario;
                MostrarDatosUsuario(usuario);

                MessageBox.Show($"Usuario '{usuario.Nombre}' encontrado",
                    "Búsqueda exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UsuarioNoEncontradoException ex)
            {
                MessageBox.Show(ex.Message, "Usuario no encontrado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (ValidacionException ex)
            {
                MessageBox.Show(ex.Message, "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar usuario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region CRUD Operations

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            _modoEdicion = false;
            _usuarioSeleccionado = null;
            LimpiarCampos();
            DesbloquearCampos();

            // Asegurar que el campo de contraseña esté completamente habilitado para nuevo usuario
            txtContraseña.ReadOnly = false;
            txtContraseña.UseSystemPasswordChar = true;
            txtContraseña.BackColor = System.Drawing.Color.White;

            btnGuardar.Enabled = true;
            txtNombreUsuario.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombreUsuario.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtContraseña.Text;

                // Validar formato de email
                if (!ValidarFormatoEmail(email))
                {
                    MessageBox.Show("El formato del email no es válido. Debe contener '@' y un dominio válido (ej: usuario@dominio.com)",
                        "Validación de Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Obtener la Familia de rol seleccionada
                var rolComboBoxItem = comboBoxPerfil.SelectedItem as RolComboBoxItem;
                var rolSeleccionado = rolComboBoxItem?.Familia;
                if (rolSeleccionado == null)
                {
                    MessageBox.Show("Debe seleccionar un rol", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar contraseña obligatoria para nuevo usuario
                if (!_modoEdicion && string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Debe ingresar una contraseña para el nuevo usuario", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContraseña.Focus();
                    return;
                }

                if (_modoEdicion && _usuarioSeleccionado != null)
                {
                    // Si la contraseña es el placeholder, enviar string vacío para no actualizarla
                    string passwordToUpdate = (password == PLACEHOLDER_PASSWORD) ? "" : password;

                    // Obtener rol anterior para la bitácora
                    var rolAnterior = _usuarioSeleccionado.ObtenerFamiliaRol();
                    string nombreRolAnterior = rolAnterior != null ? rolAnterior.NombreRol : "Sin rol";

                    // Actualizar usuario existente
                    UsuarioBLL.ActualizarUsuario(
                        _usuarioSeleccionado.IdUsuario,
                        nombre,
                        email,
                        passwordToUpdate,  // Solo actualiza si no es el placeholder
                        rolSeleccionado.IdComponent  // Pasar ID de la Familia de rol
                    );

                    // Registrar en bitácora
                    string detalle = $"Usuario '{nombre}' actualizado. Email: {email}. Rol: {rolSeleccionado.NombreRol}";
                    if (!string.IsNullOrWhiteSpace(passwordToUpdate))
                        detalle += ". Contraseña modificada";
                    if (rolAnterior?.IdComponent != rolSeleccionado.IdComponent)
                        detalle += $". Rol cambiado de '{nombreRolAnterior}' a '{rolSeleccionado.NombreRol}'";

                    _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                        modulo: "Usuarios",
                        accion: "Modificación de usuario",
                        detalle: detalle,
                        idUsuario: _usuarioLogueado.IdUsuario,
                        nombreUsuario: _usuarioLogueado.Nombre,
                        gravedad: "Alto"
                    );

                    MessageBox.Show("Usuario actualizado correctamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Crear nuevo usuario
                    UsuarioBLL.CrearUsuario(nombre, email, password, rolSeleccionado.IdComponent);

                    // Registrar en bitácora
                    _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                        modulo: "Usuarios",
                        accion: "Creación de usuario",
                        detalle: $"Usuario '{nombre}' creado con email '{email}' y rol '{rolSeleccionado.NombreRol}'",
                        idUsuario: _usuarioLogueado.IdUsuario,
                        nombreUsuario: _usuarioLogueado.Nombre,
                        gravedad: "Alto"
                    );

                    MessageBox.Show("Usuario creado correctamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarTodosLosUsuarios();
                BloquearCampos();
                btnGuardar.Enabled = false;
                _modoEdicion = false;
            }
            catch (ValidacionException ex)
            {
                MessageBox.Show(ex.Message, "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Registrar error crítico en bitácora
                _bitacoraSeguridadBLL.RegistrarError(
                    modulo: "Usuarios",
                    accion: _modoEdicion ? "Error al actualizar usuario" : "Error al crear usuario",
                    detalle: $"Error: {ex.Message}. StackTrace: {ex.StackTrace}",
                    idUsuario: _usuarioLogueado?.IdUsuario,
                    nombreUsuario: _usuarioLogueado?.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show($"Error al guardar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Seleccione un usuario de la lista o busque uno para modificar",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _modoEdicion = true;
            DesbloquearCampos();

            // Configurar el campo de contraseña para edición
            // El usuario debe hacer click en el campo para cambiarlo
            txtContraseña.Text = PLACEHOLDER_PASSWORD;
            txtContraseña.ReadOnly = true;
            txtContraseña.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);

            // Agregar evento para permitir cambio de contraseña al hacer click
            txtContraseña.Enter += TxtContraseña_Enter;

            btnGuardar.Enabled = true;
            txtNombreUsuario.Focus();
        }

        /// <summary>
        /// Evento que se dispara cuando el usuario hace click en el campo contraseña en modo edición
        /// Permite cambiar la contraseña limpiando el placeholder
        /// </summary>
        private void TxtContraseña_Enter(object sender, EventArgs e)
        {
            if (_modoEdicion && txtContraseña.Text == PLACEHOLDER_PASSWORD)
            {
                DialogResult result = MessageBox.Show(
                    "¿Desea cambiar la contraseña de este usuario?\n\nSi selecciona 'Sí', deberá ingresar una nueva contraseña.",
                    "Cambiar contraseña",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    txtContraseña.ReadOnly = false;
                    txtContraseña.Text = "";
                    txtContraseña.BackColor = System.Drawing.Color.White;
                    txtContraseña.UseSystemPasswordChar = true;
                }
                else
                {
                    // Devolver el foco a otro campo
                    txtEmail.Focus();
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_usuarioSeleccionado == null)
                {
                    MessageBox.Show("Seleccione un usuario de la lista para eliminar",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que no se elimine a sí mismo
                if (_usuarioSeleccionado.IdUsuario == _usuarioLogueado.IdUsuario)
                {
                    MessageBox.Show("No puede eliminar su propio usuario",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"¿Está seguro que desea eliminar el usuario '{_usuarioSeleccionado.Nombre}'?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Guardar datos antes de eliminar para la bitácora
                    string nombreUsuarioEliminado = _usuarioSeleccionado.Nombre;
                    string emailUsuarioEliminado = _usuarioSeleccionado.Email ?? "";
                    var rolUsuarioEliminado = _usuarioSeleccionado.ObtenerFamiliaRol();
                    string nombreRolEliminado = rolUsuarioEliminado != null ? rolUsuarioEliminado.NombreRol : "Sin rol";

                    UsuarioBLL.EliminarUsuario(_usuarioSeleccionado.IdUsuario);

                    // Registrar en bitácora
                    _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                        modulo: "Usuarios",
                        accion: "Eliminación de usuario",
                        detalle: $"Usuario '{nombreUsuarioEliminado}' eliminado. Email: {emailUsuarioEliminado}. Rol: {nombreRolEliminado}",
                        idUsuario: _usuarioLogueado.IdUsuario,
                        nombreUsuario: _usuarioLogueado.Nombre,
                        gravedad: "Alto"
                    );

                    MessageBox.Show("Usuario eliminado correctamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarTodosLosUsuarios();
                }
            }
            catch (Exception ex)
            {
                // Registrar error crítico en bitácora
                _bitacoraSeguridadBLL.RegistrarError(
                    modulo: "Usuarios",
                    accion: "Error al eliminar usuario",
                    detalle: $"Error al intentar eliminar usuario '{_usuarioSeleccionado?.Nombre}'. Error: {ex.Message}. StackTrace: {ex.StackTrace}",
                    idUsuario: _usuarioLogueado?.IdUsuario,
                    nombreUsuario: _usuarioLogueado?.Nombre,
                    gravedad: "Alto"
                );

                MessageBox.Show($"Error al eliminar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region DataGridView Events

        private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.CurrentRow != null && dgvUsuarios.CurrentRow.DataBoundItem != null)
                {
                    var item = dgvUsuarios.CurrentRow.DataBoundItem;
                    var idUsuario = (Guid)item.GetType().GetProperty("IdUsuario").GetValue(item);

                    _usuarioSeleccionado = UsuarioBLL.ObtenerUsuarioPorId(idUsuario);
                    MostrarDatosUsuario(_usuarioSeleccionado);
                }
            }
            catch (Exception ex)
            {
                // Log error silenciosamente
                Console.WriteLine($"Error al seleccionar usuario: {ex.Message}");
            }
        }

        #endregion


        #region Helpers

        private bool ValidarFormatoEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expresión regular para validar email
            // Requiere: texto@dominio.extension
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, patron);
        }

        private void MostrarDatosUsuario(Usuario usuario)
        {
            if (usuario != null)
            {
                txtNombreUsuario.Text = usuario.Nombre;
                txtEmail.Text = usuario.Email ?? "";

                // Mostrar placeholder en lugar de vacío para evitar cambio accidental
                txtContraseña.Text = PLACEHOLDER_PASSWORD;
                txtContraseña.ReadOnly = true; // Bloquear el campo por defecto
                txtContraseña.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);

                // Seleccionar el rol en el ComboBox
                var familiaRol = usuario.ObtenerFamiliaRol();
                if (familiaRol != null)
                {
                    // Buscar la Familia en el ComboBox por su ID
                    foreach (RolComboBoxItem item in comboBoxPerfil.Items)
                    {
                        if (item.Familia.IdComponent == familiaRol.IdComponent)
                        {
                            comboBoxPerfil.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    comboBoxPerfil.SelectedIndex = -1;
                }

                // Traducir el label de estado dinámicamente
                string estadoTraducido = LanguageManager.Translate("estado");
                string estadoValor = usuario.Activo ? LanguageManager.Translate("activo") : LanguageManager.Translate("inactivo");
                lblEstado.Text = $"{estadoTraducido}: {estadoValor}";
            }
        }

        private void LimpiarCampos()
        {
            txtNombreUsuario.Clear();
            txtEmail.Clear();
            txtContraseña.Clear();
            txtContraseña.ReadOnly = false;
            txtContraseña.UseSystemPasswordChar = true;
            comboBoxPerfil.SelectedIndex = -1;
            lblEstado.Text = LanguageManager.Translate("estado") + ":";
            _usuarioSeleccionado = null;
        }

        private void BloquearCampos()
        {
            txtNombreUsuario.Enabled = false;
            txtEmail.Enabled = false;
            txtContraseña.Enabled = false;
            comboBoxPerfil.Enabled = false;

            // Feedback visual: campos bloqueados con fondo gris claro
            txtNombreUsuario.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            txtEmail.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            txtContraseña.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            comboBoxPerfil.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
        }

        private void DesbloquearCampos()
        {
            txtNombreUsuario.Enabled = true;
            txtEmail.Enabled = true;
            txtContraseña.Enabled = true;
            comboBoxPerfil.Enabled = true;

            // Feedback visual: campos desbloqueados con fondo blanco
            txtNombreUsuario.BackColor = System.Drawing.Color.White;
            txtEmail.BackColor = System.Drawing.Color.White;
            txtContraseña.BackColor = System.Drawing.Color.White;
            comboBoxPerfil.BackColor = System.Drawing.Color.White;
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Evento legacy del diseñador - redirigir a BtnModificar_Click
            BtnModificar_Click(sender, e);
        }

        #endregion

        private void lblEstado_Click(object sender, EventArgs e)
        {

        }
    }
}
