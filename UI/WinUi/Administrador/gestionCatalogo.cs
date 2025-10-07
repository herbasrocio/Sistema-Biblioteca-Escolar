using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;

namespace UI.WinUi.Administrador
{
    public partial class gestionCatalogo : Form
    {
        private Usuario _usuarioLogueado;

        public gestionCatalogo()
        {
            InitializeComponent();
        }

        public gestionCatalogo(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += GestionCatalogo_Load;
            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;

            BloquearCampos();
            btnGuardar.Enabled = false;
        }

        private void GestionCatalogo_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            // CargarMateriales();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestion_catalogo");
                groupBoxDatosMaterial.Text = LanguageManager.Translate("datos_material");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                lblTitulo.Text = LanguageManager.Translate("titulo");
                lblAutor.Text = LanguageManager.Translate("autor");
                lblISBN.Text = "ISBN:";
                lblTipo.Text = LanguageManager.Translate("tipo");
                lblCantidad.Text = LanguageManager.Translate("cantidad");

                btnNuevo.Text = LanguageManager.Translate("nuevo");
                btnGuardar.Text = LanguageManager.Translate("guardar");
                btnModificar.Text = LanguageManager.Translate("editar");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        #region CRUD Operations

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            DesbloquearCampos();
            btnGuardar.Enabled = true;
            txtTitulo.Focus();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Implementar lógica de guardado
                MessageBox.Show("Funcionalidad pendiente de implementación",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            DesbloquearCampos();
            btnGuardar.Enabled = true;
            txtTitulo.Focus();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show(
                    "¿Está seguro que desea eliminar este material?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // TODO: Implementar lógica de eliminación
                    MessageBox.Show("Funcionalidad pendiente de implementación",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helpers

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtISBN.Clear();
            comboBoxTipo.SelectedIndex = -1;
            numCantidad.Value = 0;
        }

        private void BloquearCampos()
        {
            txtTitulo.Enabled = false;
            txtAutor.Enabled = false;
            txtISBN.Enabled = false;
            comboBoxTipo.Enabled = false;
            numCantidad.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtTitulo.Enabled = true;
            txtAutor.Enabled = true;
            txtISBN.Enabled = true;
            comboBoxTipo.Enabled = true;
            numCantidad.Enabled = true;
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
