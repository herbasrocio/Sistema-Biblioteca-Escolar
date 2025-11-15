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
using UI.WinUi;
using BLL;

namespace UI
{
    public partial class Login : BaseForm
    {
        private bool contraseñaVisible = false;
        private readonly BitacoraSeguridadBLL _bitacoraSeguridadBLL;

        public Login()
        {
            InitializeComponent();
            _bitacoraSeguridadBLL = new BitacoraSeguridadBLL();

            // IMPORTANTE: Establecer español como idioma por defecto al iniciar
            CambiarIdioma("es-AR");

            // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
            this.btnIngresar.Click += BtnIngresar_Click;
            this.btnRecuperarContraseña.Click += BtnRecuperarContraseña_Click;
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

        private void CambiarIdioma(string cultura)
        {
            try
            {
                // Usar LanguageManager para cambiar el idioma (patrón Observer)
                // Esto notificará automáticamente a todos los formularios abiertos
                // BaseForm se encargará de llamar a AplicarTraducciones() automáticamente
                LanguageManager.ChangeLanguage(cultura);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar idioma: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void AplicarTraducciones()
        {
            // Traducir título del formulario
            this.Text = LanguageManager.Translate("login");

            // Traducir labels
            lblUsuario.Text = LanguageManager.Translate("usuario") + ":";
            lblContraseña.Text = LanguageManager.Translate("contraseña") + ":";
            lblMenu.Text = LanguageManager.Translate("sistema_biblioteca");

            // Centrar el label del título después de cambiar el texto
            CentrarLabel(lblMenu);

            // Traducir botones
            btnIngresar.Text = LanguageManager.Translate("ingresar");
            btnRecuperarContraseña.Text = LanguageManager.Translate("recuperar_contraseña");
        }

        /// <summary>
        /// Centra un label horizontalmente en el formulario
        /// </summary>
        private void CentrarLabel(System.Windows.Forms.Label label)
        {
            label.Left = (this.ClientSize.Width - label.Width) / 2;
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos usando ValidationBLL
                ServicesSecurity.BLL.ValidationBLL.ValidarCredencialesLogin(txtUsuario.Text, txtContraseña.Text);

                // Intentar login usando LoginService
                Usuario usuarioLogueado = LoginService.Login(txtUsuario.Text.Trim(), txtContraseña.Text);

                // Registrar login exitoso en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Login",
                    accion: "Inicio de sesión exitoso",
                    detalle: $"Usuario '{txtUsuario.Text.Trim()}' ingresó al sistema",
                    idUsuario: usuarioLogueado.IdUsuario,
                    nombreUsuario: usuarioLogueado.Nombre,
                    gravedad: "Bajo"
                );

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
                // Usuario no existe - registrar en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Login",
                    accion: "Intento de login con usuario inexistente",
                    detalle: $"Se intentó acceder con el usuario '{txtUsuario.Text.Trim()}' que no existe en el sistema",
                    nombreUsuario: txtUsuario.Text.Trim(),
                    gravedad: "Medio"
                );

                MessageBox.Show(uex.Message, LanguageManager.Translate("error_autenticacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.Clear();
                txtUsuario.Focus();
            }
            catch (ContraseñaInvalidaException cex)
            {
                // Contraseña incorrecta - registrar en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Login",
                    accion: "Intento de login con contraseña incorrecta",
                    detalle: $"Usuario '{txtUsuario.Text.Trim()}' intentó ingresar con contraseña incorrecta",
                    nombreUsuario: txtUsuario.Text.Trim(),
                    gravedad: "Alto"
                );

                MessageBox.Show(cex.Message, LanguageManager.Translate("error_autenticacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContraseña.Clear();
                txtContraseña.Focus();
            }
            catch (AutenticacionException aex)
            {
                // Otros errores de autenticación - registrar en bitácora
                _bitacoraSeguridadBLL.RegistrarEventoSeguridad(
                    modulo: "Login",
                    accion: "Error de autenticación",
                    detalle: $"Error de autenticación para usuario '{txtUsuario.Text.Trim()}': {aex.Message}",
                    nombreUsuario: txtUsuario.Text.Trim(),
                    gravedad: "Alto"
                );

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
            // El idioma se obtiene del perfil del usuario guardado en la BD
            // Si no tiene idioma configurado, se usa español por defecto
            string idiomaAUsar = usuario.IdiomaPreferido ?? "es-AR";
            CambiarIdioma(idiomaAUsar);

            // Verificar que el usuario tenga un rol asignado
            string nombreRol = usuario.ObtenerNombreRol();
            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                MessageBox.Show(LanguageManager.Translate("usuario_sin_rol"),
                    LanguageManager.Translate("error_autorizacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Redirigir al menú (único para todos los roles)
            // El menú mostrará/ocultará opciones según los permisos del usuario
            menu menuForm = new menu(usuario);
            menuForm.Show();
            this.Hide();
        }

        private void BtnRecuperarContraseña_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = LanguageManager.Translate("contactar_admin_recuperacion");
                string titulo = LanguageManager.Translate("recuperar_contraseña");

                MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
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
