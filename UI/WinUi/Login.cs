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
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using ServicesSecurity.BLL;
using UI.WinUi.Administrador;

namespace UI
{
    public partial class Login : Form
    {
        private bool contraseñaVisible = false;

        public Login()
        {
            InitializeComponent();
            AplicarTraducciones();
            this.btnIngresar.Click += BtnIngresar_Click;
            this.btnRecuperarContraseña.Click += BtnRecuperarContraseña_Click;
            this.lnkEspañol.LinkClicked += LnkEspañol_LinkClicked;
            this.lnkEnglish.LinkClicked += LnkEnglish_LinkClicked;
            this.txtContraseña.KeyPress += TxtContraseña_KeyPress;
            this.btnMostrarContraseña.Click += BtnMostrarContraseña_Click;
            this.Load += Login_Load;
        }

        private void BtnMostrarContraseña_Click(object sender, EventArgs e)
        {
            contraseñaVisible = !contraseñaVisible;
            if (contraseñaVisible)
            {
                txtContraseña.PasswordChar = '\0';
                btnMostrarContraseña.Text = "👁";
            }
            else
            {
                txtContraseña.PasswordChar = '●';
                btnMostrarContraseña.Text = "👁";
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsuario.Focus();
        }

        private void TxtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnIngresar_Click(sender, e);
            }
        }

        private void LnkEspañol_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CambiarIdioma("es-AR");
        }

        private void LnkEnglish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CambiarIdioma("en-GB");
        }

        private void CambiarIdioma(string cultura)
        {
            try
            {
                CultureInfo nuevaCultura = new CultureInfo(cultura);
                Thread.CurrentThread.CurrentCulture = nuevaCultura;
                Thread.CurrentThread.CurrentUICulture = nuevaCultura;

                AplicarTraducciones();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar idioma: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarTraducciones()
        {
            // Traducir título del formulario
            this.Text = LanguageManager.Translate("login");

            // Traducir labels
            lblUsuario.Text = LanguageManager.Translate("usuario") + ":";
            lblContraseña.Text = LanguageManager.Translate("contraseña") + ":";
            lblMenu.Text = LanguageManager.Translate("sistema_biblioteca");

            // Traducir botones
            btnIngresar.Text = LanguageManager.Translate("ingresar");
            btnRecuperarContraseña.Text = LanguageManager.Translate("recuperar_contraseña");
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos usando ValidationBLL
                ValidationBLL.ValidarCredencialesLogin(txtUsuario.Text, txtContraseña.Text);

                // Intentar login usando LoginService
                Usuario usuarioLogueado = LoginService.Login(txtUsuario.Text.Trim(), txtContraseña.Text);

                // Aplicar idioma preferido del usuario
                if (!string.IsNullOrEmpty(usuarioLogueado.IdiomaPreferido))
                {
                    CambiarIdioma(usuarioLogueado.IdiomaPreferido);
                }

                // Login exitoso - redirigir según el rol
                RedirigirPorRol(usuarioLogueado);
            }
            catch (ValidacionException vex)
            {
                // Errores de validación (campos vacíos, etc)
                MessageBox.Show(vex.Message, LanguageManager.Translate("error_validacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UsuarioNoEncontradoException uex)
            {
                // Usuario no existe
                MessageBox.Show(uex.Message, LanguageManager.Translate("error_autenticacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.Clear();
                txtUsuario.Focus();
            }
            catch (ContraseñaInvalidaException cex)
            {
                // Contraseña incorrecta
                MessageBox.Show(cex.Message, LanguageManager.Translate("error_autenticacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.Clear();
                txtContraseña.Focus();
            }
            catch (AutenticacionException aex)
            {
                // Otros errores de autenticación
                MessageBox.Show(aex.Message, LanguageManager.Translate("error_autenticacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Errores generales no controlados
                MessageBox.Show(LanguageManager.Translate("error_inesperado") + ": " + ex.Message,
                    LanguageManager.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RedirigirPorRol(Usuario usuario)
        {
            // Obtener el nombre del rol desde la Familia de permisos
            string nombreRol = usuario.ObtenerNombreRol();

            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                MessageBox.Show(LanguageManager.Translate("usuario_sin_rol"),
                    LanguageManager.Translate("error_autorizacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (nombreRol.ToUpper())
            {
                case "DOCENTE":
                    UI.WinUi.Docente.menuDocente menuDoc = new UI.WinUi.Docente.menuDocente(usuario);
                    menuDoc.Show();
                    this.Hide();
                    break;

                case "ADMINISTRADOR":
                    menuAdministrador menuAdmin = new menuAdministrador(usuario);
                    menuAdmin.Show();
                    this.Hide();
                    break;

                default:
                    MessageBox.Show(LanguageManager.Translate("rol_no_reconocido"),
                        LanguageManager.Translate("error_autorizacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void BtnRecuperarContraseña_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = LanguageManager.Translate("contactar_admin_recuperacion");
                string titulo = LanguageManager.Translate("recuperar_contraseña");

                MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Si falla la traducción, mostrar mensaje por defecto
                MessageBox.Show(
                    "Contactar con el administrador del sistema para la recuperación de contraseña.",
                    "Recuperar Contraseña",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnMostrarContraseña_Click_1(object sender, EventArgs e)
        {

        }
    }
}
