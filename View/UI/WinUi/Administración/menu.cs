using System;
using System.Linq;
using System.Windows.Forms;
using Services.DomainModel.Security.Composite;
using Services.Services;
using Services.BLL;
using BLL;

namespace UI.WinUi.Administrador
{
    public partial class menu : BaseForm, IPermissionObserver
    {
        private Usuario _usuarioLogueado;
        private BitacoraSeguridadBLL _bitacoraSeguridadBLL;

        // Nombres de las patentes que controlan cada opción del menú
        private const string PATENTE_USUARIOS = "Gestion Usuarios";
        private const string PATENTE_PERMISOS = "Gestion Permisos";
        private const string PATENTE_CONSULTAR_MATERIAL = "Consultar Material";
        private const string PATENTE_REGISTRAR_MATERIAL = "Registrar Material";
        private const string PATENTE_EDITAR_MATERIAL = "Editar Material";
        private const string PATENTE_GESTIONAR_EJEMPLARES = "Gestionar Ejemplares";
        private const string PATENTE_ALUMNOS = "Gestion Alumnos";
        private const string PATENTE_PRESTAMOS = "Gestion Prestamos";
        private const string PATENTE_DEVOLUCIONES = "Gestion Devoluciones";
        private const string PATENTE_REPORTES = "consultarReportes";
        private const string PATENTE_BITACORA_SEGURIDAD = "consultarBitacoraSeguridad";
        private const string PATENTE_BITACORA_OPERACIONES = "consultarBitacoraOperaciones";
        private const string PATENTE_BACKUP = "FrmGestionBackup";

        public menu()
        {
            InitializeComponent();
            ConfigurarEstiloVisual();
        }

        public menu(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _bitacoraSeguridadBLL = new BitacoraSeguridadBLL();
            // AplicarTraducciones() se llamará automáticamente desde BaseForm.Load
            ConfigurarVisibilidadPorPermisos();

            // Registrar este formulario como observador de cambios de permisos
            PermissionManager.Instance.Attach(this);

            // Desregistrar cuando se cierre el formulario
            this.FormClosing += Menu_FormClosing;
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Desregistrar este formulario como observador
            PermissionManager.Instance.Detach(this);
        }

        private void ConfigurarEstiloVisual()
        {
            // Aplicar colores del Login al menú
            this.BackColor = System.Drawing.Color.FromArgb(236, 240, 241); // Fondo gris claro

            // Configurar color de texto blanco para todos los items del menú
            foreach (System.Windows.Forms.ToolStripMenuItem item in menuStrip1.Items)
            {
                item.ForeColor = System.Drawing.Color.White;
            }
        }

        protected override void AplicarTraducciones()
        {
            // Traducir textos del formulario
            this.Text = LanguageManager.Translate("menu_principal");

            // Traducir menú
            usuariosToolStripMenuItem.Text = LanguageManager.Translate("usuarios");
            permisosToolStripMenuItem.Text = LanguageManager.Translate("permisos");
            catalogoToolStripMenuItem.Text = LanguageManager.Translate("catalogo");
            consultarMaterialToolStripMenuItem.Text = LanguageManager.Translate("consultar_material");
            registrarMaterialToolStripMenuItem.Text = LanguageManager.Translate("registrar_material");
            alumnosToolStripMenuItem.Text = LanguageManager.Translate("alumnos");
            prestamosToolStripMenuItem.Text = LanguageManager.Translate("prestamos");
            devolucionesToolStripMenuItem.Text = LanguageManager.Translate("devoluciones");
            reportesToolStripMenuItem.Text = LanguageManager.Translate("reportes");
            bitacorasToolStripMenuItem.Text = LanguageManager.Translate("bitacoras");
            consultarBitacoraAdminToolStripMenuItem.Text = LanguageManager.Translate("bitacora_admin_titulo");
            consultarBitacoraBibliotecarioToolStripMenuItem.Text = LanguageManager.Translate("bitacora_bibliotecario_titulo");
            backupToolStripMenuItem.Text = LanguageManager.Translate("backup_menu");
            miPerfilToolStripMenuItem.Text = LanguageManager.Translate("mi_perfil");
            cerrarSesionToolStripMenuItem.Text = LanguageManager.Translate("cerrar_sesion");

            // Actualizar información del usuario en el panel de bienvenida
            lblTituloPrincipal.Text = LanguageManager.Translate("sistema_biblioteca");

            // Centrar el label del título después de cambiar el texto
            CentrarLabel(lblTituloPrincipal);

            lblBienvenida.Text = "¡" + LanguageManager.Translate("bienvenido") + "!";
            lblNombreUsuario.Text = LanguageManager.Translate("usuario") + ": " + (_usuarioLogueado?.Nombre ?? "");
            var rol = _usuarioLogueado?.ObtenerNombreRol();
            lblRolUsuario.Text = LanguageManager.Translate("rol") + ": " + (rol ?? LanguageManager.Translate("sin_rol"));
        }

        /// <summary>
        /// Centra un label horizontalmente en el formulario
        /// </summary>
        private void CentrarLabel(System.Windows.Forms.Label label)
        {
            label.Left = (this.ClientSize.Width - label.Width) / 2;
        }

        private void ConfigurarVisibilidadPorPermisos()
        {
            // Configurar visibilidad de cada opción del menú según permisos
            usuariosToolStripMenuItem.Visible = TienePermiso(PATENTE_USUARIOS);
            permisosToolStripMenuItem.Visible = TienePermiso(PATENTE_PERMISOS);

            // Catálogo: visible si tiene al menos uno de los submenús
            bool tieneConsultar = TienePermiso(PATENTE_CONSULTAR_MATERIAL);
            bool tieneRegistrar = TienePermiso(PATENTE_REGISTRAR_MATERIAL);
            catalogoToolStripMenuItem.Visible = tieneConsultar || tieneRegistrar;
            consultarMaterialToolStripMenuItem.Visible = tieneConsultar;
            registrarMaterialToolStripMenuItem.Visible = tieneRegistrar;

            alumnosToolStripMenuItem.Visible = TienePermiso(PATENTE_ALUMNOS);

            // Préstamos: mostrar si tiene permiso de préstamos (la ventana gestiona internamente la visibilidad de renovar)
            prestamosToolStripMenuItem.Visible = TienePermiso(PATENTE_PRESTAMOS);

            devolucionesToolStripMenuItem.Visible = TienePermiso(PATENTE_DEVOLUCIONES);

            // Reportes: visible si tiene el permiso unificado "consultarReportes"
            bool tieneAccesoReportes = TienePermiso(PATENTE_REPORTES);
            reportesToolStripMenuItem.Visible = tieneAccesoReportes;

            // Todos los reportes individuales usan el mismo permiso unificado
            reportePrestamosActivosToolStripMenuItem.Visible = tieneAccesoReportes;
            reporteMaterialesMasPrestadosToolStripMenuItem.Visible = tieneAccesoReportes;
            reporteUsoPorGradoToolStripMenuItem.Visible = tieneAccesoReportes;

            // Bitácoras: visible si tiene al menos una de las bitácoras
            bool tieneBitacoraSeguridad = TienePermiso(PATENTE_BITACORA_SEGURIDAD);
            bool tieneBitacoraOperaciones = TienePermiso(PATENTE_BITACORA_OPERACIONES);
            bitacorasToolStripMenuItem.Visible = tieneBitacoraSeguridad || tieneBitacoraOperaciones;
            consultarBitacoraAdminToolStripMenuItem.Visible = tieneBitacoraSeguridad;
            consultarBitacoraBibliotecarioToolStripMenuItem.Visible = tieneBitacoraOperaciones;

            // Backup: solo visible para usuarios con permiso
            backupToolStripMenuItem.Visible = TienePermiso(PATENTE_BACKUP);
        }

        private bool TienePermiso(string nombrePatente)
        {
            // Usar el método centralizado del Usuario que maneja el bypass de Administrador
            return _usuarioLogueado?.TienePermiso(nombrePatente) ?? false;
        }

        /// <summary>
        /// Verifica si el usuario tiene permiso y registra en bitácora si el acceso es denegado
        /// </summary>
        private bool VerificarYRegistrarAcceso(string nombrePatente, string nombreFormulario)
        {
            bool tienePermiso = TienePermiso(nombrePatente);

            if (!tienePermiso)
            {
                // Registrar intento de acceso denegado en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Seguridad",
                    accion: "Acceso denegado",
                    detalle: $"Usuario '{_usuarioLogueado.Nombre}' intentó acceder a '{nombreFormulario}' sin permiso '{nombrePatente}'",
                    idUsuario: _usuarioLogueado.IdUsuario,
                    nombreUsuario: _usuarioLogueado.Nombre,
                    gravedad: "Medio"
                );
            }

            return tienePermiso;
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_USUARIOS, "Gestión de Usuarios"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                gestionUsuarios formGestion = new gestionUsuarios(_usuarioLogueado);
                formGestion.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_PERMISOS, "Gestión de Permisos"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                gestionPermisos formPermisos = new gestionPermisos(_usuarioLogueado);
                formPermisos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de permisos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void consultarMaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_CONSULTAR_MATERIAL, "Consultar Material"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                consultarMaterial formConsultar = new consultarMaterial(_usuarioLogueado);
                formConsultar.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir consulta de material: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void registrarMaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_REGISTRAR_MATERIAL, "Registrar Material"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RegistrarMaterial formRegistrar = new RegistrarMaterial(_usuarioLogueado);
                formRegistrar.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir registro de material: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void alumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_ALUMNOS, "Gestión de Alumnos"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                gestionAlumnos formAlumnos = new gestionAlumnos(_usuarioLogueado);
                formAlumnos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de alumnos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void prestamosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_PRESTAMOS, "Gestión de Préstamos"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Abrir ventana unificada de préstamos con pestañas (Registrar y Renovar)
                UI.WinUi.Transacciones.Form1gestionPrestamos formGestionPrestamos = new UI.WinUi.Transacciones.Form1gestionPrestamos(_usuarioLogueado);
                formGestionPrestamos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de préstamos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void devolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_DEVOLUCIONES, "Gestión de Devoluciones"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UI.WinUi.Transacciones.registrarDevolucion formDevolucion = new UI.WinUi.Transacciones.registrarDevolucion(_usuarioLogueado);
                formDevolucion.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir registro de devoluciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reportePrestamosActivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_REPORTES, "Reporte Préstamos Activos"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UI.WinUi.Reportes.ReportePrestamosActivos formReporte = new UI.WinUi.Reportes.ReportePrestamosActivos(_usuarioLogueado);
                formReporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir reporte de préstamos activos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reporteMaterialesMasPrestadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_REPORTES, "Reporte Materiales Más Prestados"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UI.WinUi.Reportes.ReporteMaterialesMasPrestados formReporte = new UI.WinUi.Reportes.ReporteMaterialesMasPrestados(_usuarioLogueado);
                formReporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir reporte de materiales más prestados: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reporteUsoPorGradoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_REPORTES, "Reporte Uso por Grado"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UI.WinUi.Reportes.ReporteUsoPorGrado formReporte = new UI.WinUi.Reportes.ReporteUsoPorGrado(_usuarioLogueado);
                formReporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir reporte de uso por grado: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void consultarBitacoraAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_BITACORA_SEGURIDAD, "Consultar Bitácora de Seguridad"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UI.WinUi.Bitacoras.ConsultarBitacoraSeguridad formBitacoraSeguridad = new UI.WinUi.Bitacoras.ConsultarBitacoraSeguridad(_usuarioLogueado);
                formBitacoraSeguridad.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir bitácora de seguridad: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void consultarBitacoraBibliotecarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_BITACORA_OPERACIONES, "Consultar Bitácora de Operaciones"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UI.WinUi.Bitacoras.ConsultarBitacoraOperaciones formBitacoraOperaciones = new UI.WinUi.Bitacoras.ConsultarBitacoraOperaciones(_usuarioLogueado);
                formBitacoraOperaciones.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir bitácora de operaciones: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarYRegistrarAcceso(PATENTE_BACKUP, "Gestión de Backup"))
                {
                    MessageBox.Show(LanguageManager.Translate("acceso_denegado"),
                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FrmGestionBackup formBackup = new FrmGestionBackup(_usuarioLogueado);
                formBackup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de backup: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void miPerfilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Mi Perfil no requiere permiso especial - todos los usuarios pueden acceder
                FrmMiPerfil formMiPerfil = new FrmMiPerfil(_usuarioLogueado);
                formMiPerfil.ShowDialog();

                // Refrescar traducciones en caso de que el usuario haya cambiado el idioma
                AplicarTraducciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir Mi Perfil: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var resultado = MessageBox.Show(
                    LanguageManager.Translate("confirmar_cerrar_sesion"),
                    LanguageManager.Translate("cerrar_sesion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Registrar cierre de sesión en bitácora
                    _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                        modulo: "Login",
                        accion: "Cierre de sesión",
                        detalle: $"Usuario '{_usuarioLogueado.Nombre}' cerró sesión",
                        idUsuario: _usuarioLogueado.IdUsuario,
                        nombreUsuario: _usuarioLogueado.Nombre,
                        gravedad: "Bajo"
                    );

                    // Cerrar este formulario
                    this.Close();

                    // Mostrar el formulario de login nuevamente
                    Login loginForm = new Login();
                    loginForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region IPermissionObserver Implementation

        /// <summary>
        /// Implementación del patrón Observer: se ejecuta cuando los permisos del usuario cambian.
        /// Actualiza dinámicamente la visibilidad de las opciones del menú.
        /// </summary>
        public void OnPermissionsChanged(Usuario usuario)
        {
            // Si no se proporciona usuario, recargar el usuario actual
            if (usuario == null)
            {
                // Recargar usuario actual desde la base de datos
                usuario = UsuarioBLL.ObtenerUsuarioPorId(_usuarioLogueado.IdUsuario);
                if (usuario == null)
                    return;
            }

            // Verificar que sea el usuario actualmente logueado
            if (usuario.IdUsuario != _usuarioLogueado.IdUsuario)
                return;

            // Actualizar referencia al usuario con permisos actualizados
            _usuarioLogueado = usuario;

            // Invocar en el thread de UI si es necesario
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ActualizarInterfazPorPermisos()));
            }
            else
            {
                ActualizarInterfazPorPermisos();
            }
        }

        /// <summary>
        /// Actualiza la interfaz del menú según los permisos actuales del usuario
        /// </summary>
        private void ActualizarInterfazPorPermisos()
        {
            try
            {
                // Reconfigurar visibilidad de opciones del menú
                ConfigurarVisibilidadPorPermisos();

                // Mostrar mensaje informativo al usuario
                MessageBox.Show(
                    LanguageManager.Translate("permisos_actualizados") ??
                    "Tus permisos han sido actualizados. El menú se ha actualizado automáticamente.",
                    LanguageManager.Translate("informacion") ?? "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al actualizar interfaz por permisos: {ex.Message}");
            }
        }

        #endregion
    }
}
