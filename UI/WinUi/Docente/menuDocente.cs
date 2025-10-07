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
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.BLL;
using ServicesSecurity.Services;

namespace UI.WinUi.Docente
{
    public partial class menuDocente : Form
    {
        private Usuario _usuarioLogueado;

        public menuDocente()
        {
            InitializeComponent();
        }

        public menuDocente(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            CargarIdiomasEnComboBox();
            CargarDatosUsuario();
            ConfigurarEventos();
        }

        private void CargarIdiomasEnComboBox()
        {
            try
            {
                var idiomas = new Dictionary<string, string>
                {
                    { "es-AR", "Español" },
                    { "en-GB", "Inglés" }
                };

                comboBoxIdioma.DataSource = new BindingSource(idiomas, null);
                comboBoxIdioma.DisplayMember = "Value";
                comboBoxIdioma.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar idiomas: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosUsuario()
        {
            if (_usuarioLogueado != null)
            {
                ActualizarTextos();

                // Establecer el idioma preferido del usuario
                if (!string.IsNullOrEmpty(_usuarioLogueado.IdiomaPreferido))
                {
                    comboBoxIdioma.SelectedValue = _usuarioLogueado.IdiomaPreferido;
                }
                else
                {
                    comboBoxIdioma.SelectedValue = "es-AR";
                }
            }
        }

        private void ActualizarTextos()
        {
            if (_usuarioLogueado != null)
            {
                // Traducir textos del formulario
                this.Text = LanguageManager.Translate("menu_docente");
                label1.Text = LanguageManager.Translate("sistema_biblioteca");
                lblUsuario.Text = $"{LanguageManager.Translate("usuario")}: {_usuarioLogueado.Nombre}";

                string nombreRol = _usuarioLogueado.ObtenerNombreRol() ?? LanguageManager.Translate("sin_rol");
                lblPerfil.Text = $"{LanguageManager.Translate("perfil")}: {nombreRol}";

                lblIdioma.Text = LanguageManager.Translate("idioma") + ":";

                // Traducir botones
                btnCatalogo.Text = LanguageManager.Translate("catalogo");
                btnPrestamos.Text = LanguageManager.Translate("prestamos");
                btnDevoluciones.Text = LanguageManager.Translate("devoluciones");
            }
        }

        private void ConfigurarEventos()
        {
            comboBoxIdioma.SelectedIndexChanged += ComboBoxIdioma_SelectedIndexChanged;
        }

        private void ComboBoxIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxIdioma.SelectedValue != null && _usuarioLogueado != null)
                {
                    string nuevoIdioma = comboBoxIdioma.SelectedValue.ToString();

                    // Solo actualizar si cambió
                    if (nuevoIdioma != _usuarioLogueado.IdiomaPreferido)
                    {
                        // Guardar en la BD
                        UsuarioBLL.CambiarIdiomaPreferido(_usuarioLogueado.IdUsuario, nuevoIdioma);

                        // Actualizar el idioma de la aplicación
                        CultureInfo nuevaCultura = new CultureInfo(nuevoIdioma);
                        Thread.CurrentThread.CurrentCulture = nuevaCultura;
                        Thread.CurrentThread.CurrentUICulture = nuevaCultura;

                        // Actualizar el objeto del usuario
                        _usuarioLogueado.IdiomaPreferido = nuevoIdioma;

                        // Actualizar textos del formulario actual
                        ActualizarTextos();

                        MessageBox.Show(LanguageManager.Translate("idioma_actualizado"),
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar idioma: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
