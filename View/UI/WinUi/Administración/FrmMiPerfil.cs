using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.BLL;
using Services.DomainModel.Security.Composite;
using Services.DomainModel.Exceptions;
using Services.Services;

namespace UI.WinUi.Administrador
{
    public partial class FrmMiPerfil : BaseForm
    {
        private Usuario _usuarioActual;

        public FrmMiPerfil(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
            this.Load += FrmMiPerfil_Load;
        }

        private void FrmMiPerfil_Load(object sender, EventArgs e)
        {
            CargarDatosUsuario();
            ConfigurarComboIdiomas();
            // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
        }

        private void CargarDatosUsuario()
        {
            // Recargar usuario desde BD para obtener datos actualizados
            _usuarioActual = UsuarioBLL.ObtenerUsuarioPorId(_usuarioActual.IdUsuario);

            txtNombreUsuario.Text = _usuarioActual.Nombre;
            txtEmail.Text = _usuarioActual.Email ?? "";
            txtRol.Text = _usuarioActual.ObtenerNombreRol() ?? "Sin rol";

            if (_usuarioActual.FechaUltimoAcceso.HasValue)
            {
                txtUltimoAcceso.Text = _usuarioActual.FechaUltimoAcceso.Value.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                txtUltimoAcceso.Text = "No disponible";
            }

            // Seleccionar idioma actual
            string idiomaActual = _usuarioActual.IdiomaPreferido ?? "es-AR";
            cboIdioma.SelectedValue = idiomaActual;
        }

        private void ConfigurarComboIdiomas()
        {
            var idiomas = new List<IdiomaItem>
            {
                new IdiomaItem { Codigo = "es-AR", Nombre = "Español (Argentina)" },
                new IdiomaItem { Codigo = "en-GB", Nombre = "English (United Kingdom)" }
            };

            cboIdioma.DataSource = idiomas;
            cboIdioma.DisplayMember = "Nombre";
            cboIdioma.ValueMember = "Codigo";
        }

        protected override void AplicarTraducciones()
        {
            this.Text = LanguageManager.Translate("mi_perfil");
            grpDatosUsuario.Text = LanguageManager.Translate("datos_usuario");
            grpPreferencias.Text = LanguageManager.Translate("preferencias");
            grpCambiarPassword.Text = LanguageManager.Translate("cambiar_contraseña");

            lblNombreUsuario.Text = LanguageManager.Translate("nombre_usuario") + ":";
            lblEmail.Text = LanguageManager.Translate("email") + ":";
            lblRol.Text = LanguageManager.Translate("rol") + ":";
            lblUltimoAcceso.Text = LanguageManager.Translate("ultimo_acceso") + ":";
            lblIdioma.Text = LanguageManager.Translate("idioma") + ":";
            lblPasswordActual.Text = LanguageManager.Translate("contraseña_actual") + ":";
            lblPasswordNueva.Text = LanguageManager.Translate("contraseña_nueva") + ":";
            lblPasswordConfirmar.Text = LanguageManager.Translate("confirmar_contraseña") + ":";

            btnGuardar.Text = LanguageManager.Translate("guardar");
            btnCancelar.Text = LanguageManager.Translate("cancelar");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string idiomaSeleccionado = cboIdioma.SelectedValue?.ToString();
                string passwordActual = txtPasswordActual.Text;
                string passwordNueva = txtPasswordNueva.Text;
                string passwordConfirmar = txtPasswordConfirmar.Text;

                // Validar si se quiere cambiar la contraseña
                bool cambiarPassword = !string.IsNullOrWhiteSpace(passwordNueva);

                if (cambiarPassword)
                {
                    // Validar que se haya ingresado la contraseña actual
                    if (string.IsNullOrWhiteSpace(passwordActual))
                    {
                        MessageBox.Show(
                            LanguageManager.Translate("error_password_actual_requerida"),
                            LanguageManager.Translate("error_validacion"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        txtPasswordActual.Focus();
                        return;
                    }

                    // Validar que las contraseñas nuevas coincidan
                    if (passwordNueva != passwordConfirmar)
                    {
                        MessageBox.Show(
                            LanguageManager.Translate("error_passwords_no_coinciden"),
                            LanguageManager.Translate("error_validacion"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        txtPasswordConfirmar.Focus();
                        return;
                    }

                    // Validar longitud mínima
                    if (passwordNueva.Length < 6)
                    {
                        MessageBox.Show(
                            LanguageManager.Translate("error_password_min_length"),
                            LanguageManager.Translate("error_validacion"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        txtPasswordNueva.Focus();
                        return;
                    }
                }

                // Actualizar perfil
                UsuarioBLL.ActualizarPerfil(
                    _usuarioActual.IdUsuario,
                    idiomaSeleccionado,
                    cambiarPassword ? passwordNueva : null,
                    cambiarPassword ? passwordActual : null
                );

                // Cambiar idioma de la sesión actual usando LanguageManager (patrón Observer)
                if (!string.IsNullOrWhiteSpace(idiomaSeleccionado))
                {
                    LanguageManager.ChangeLanguage(idiomaSeleccionado);

                    // Actualizar el idioma en el usuario en sesión
                    _usuarioActual.IdiomaPreferido = idiomaSeleccionado;
                }

                MessageBox.Show(
                    LanguageManager.Translate("perfil_actualizado_exitosamente"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Limpiar campos de contraseña
                txtPasswordActual.Clear();
                txtPasswordNueva.Clear();
                txtPasswordConfirmar.Clear();

                // Recargar traducciones con el nuevo idioma
                AplicarTraducciones();
            }
            catch (ContraseñaInvalidaException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    LanguageManager.Translate("error_autenticacion"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPasswordActual.Focus();
            }
            catch (ValidacionException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    LanguageManager.Translate("error_validacion"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    LanguageManager.Translate("error_inesperado") + ": " + ex.Message,
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Clase auxiliar para el combo de idiomas
        private class IdiomaItem
        {
            public string Codigo { get; set; }
            public string Nombre { get; set; }
        }
    }
}
