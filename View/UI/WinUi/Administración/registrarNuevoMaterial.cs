using DomainModel.Enums;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using System;
using System.Windows.Forms;
using UI.Helpers;

namespace UI.WinUi.Administrador
{
    public partial class registrarNuevoMaterial : Form
    {
        private Usuario _usuarioLogueado;

        public registrarNuevoMaterial()
        {
            InitializeComponent();
        }

        public registrarNuevoMaterial(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += registrarNuevoMaterial_Load;
            btnNuevo.Click += BtnNuevo_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;
            txtAnioPublicacion.KeyPress += TxtAnioPublicacion_KeyPress;
            txtAnioPublicacion.Leave += TxtAnioPublicacion_Leave;

            // Aplicar estilos del sistema
            AplicarEstilos();

            // Cargar enums en los ComboBox
            CargarTiposMaterial();
            CargarGeneros();
            CargarEdadesRecomendadas();

            BloquearCampos();
            btnGuardar.Enabled = false;
        }

        private void registrarNuevoMaterial_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            // CargarMateriales();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("registrar_material");
                groupBoxDatosMaterial.Text = LanguageManager.Translate("datos_material");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                lblTitulo.Text = LanguageManager.Translate("titulo");
                lblAutor.Text = LanguageManager.Translate("autor");
                lblTipo.Text = LanguageManager.Translate("tipo");
                lblGenero.Text = LanguageManager.Translate("genero");
                lblISBN.Text = "ISBN:";
                lblEditorial.Text = LanguageManager.Translate("editorial");
                lblAnioPublicacion.Text = LanguageManager.Translate("anio_publicacion");
                lblEdadRecomendada.Text = LanguageManager.Translate("edad_recomendada");
                lblEjemplares.Text = LanguageManager.Translate("cantidad");
                lblDescripcion.Text = LanguageManager.Translate("descripcion");

                btnNuevo.Text = LanguageManager.Translate("nuevo");
                btnGuardar.Text = LanguageManager.Translate("guardar_material");
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
            btnGuardar.Visible = true;
            btnModificar.Visible = false;
            btnEliminar.Visible = false;
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
            btnGuardar.Visible = true;
            btnModificar.Visible = false;
            btnEliminar.Visible = false;
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

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Helper Methods

        private void AplicarEstilos()
        {
            // Aplicar estilo al formulario
            EstilosSistema.AplicarEstiloFormulario(this);

            // Aplicar estilos a los GroupBox
            EstilosSistema.AplicarEstiloGroupBox(groupBoxDatosMaterial);
            EstilosSistema.AplicarEstiloGroupBox(groupBoxAcciones);

            // Aplicar estilos a los Labels
            EstilosSistema.AplicarEstiloLabel(lblTitulo);
            EstilosSistema.AplicarEstiloLabel(lblAutor);
            EstilosSistema.AplicarEstiloLabel(lblTipo);
            EstilosSistema.AplicarEstiloLabel(lblGenero);
            EstilosSistema.AplicarEstiloLabel(lblISBN);
            EstilosSistema.AplicarEstiloLabel(lblEditorial);
            EstilosSistema.AplicarEstiloLabel(lblAnioPublicacion);
            EstilosSistema.AplicarEstiloLabel(lblEdadRecomendada);
            EstilosSistema.AplicarEstiloLabel(lblEjemplares);
            EstilosSistema.AplicarEstiloLabel(lblDescripcion);

            // Aplicar estilos a los TextBox
            EstilosSistema.AplicarEstiloTextBox(txtTitulo);
            EstilosSistema.AplicarEstiloTextBox(txtAutor);
            EstilosSistema.AplicarEstiloTextBox(txtISBN);
            EstilosSistema.AplicarEstiloTextBox(txtEditorial);
            EstilosSistema.AplicarEstiloTextBox(txtAnioPublicacion);
            EstilosSistema.AplicarEstiloTextBox(txtDescripcion);

            // Aplicar estilos a los ComboBox
            EstilosSistema.AplicarEstiloComboBox(comboBoxTipo);
            EstilosSistema.AplicarEstiloComboBox(comboBoxGenero);
            EstilosSistema.AplicarEstiloComboBox(comboBoxEdadRecomendada);

            // Aplicar estilos a los botones
            EstilosSistema.AplicarEstiloBotonPrimario(btnNuevo);
            EstilosSistema.AplicarEstiloBotonPrimario(btnGuardar);
            EstilosSistema.AplicarEstiloBotonSecundario(btnModificar);
            EstilosSistema.AplicarEstiloBotonSecundario(btnEliminar);
            EstilosSistema.AplicarEstiloBotonSecundario(btnVolver);

            // Aplicar estilos al DataGridView
            EstilosSistema.AplicarEstiloDataGridView(dgvMateriales);
        }

        private void CargarTiposMaterial()
        {
            comboBoxTipo.Items.Clear();
            comboBoxTipo.DisplayMember = "Text";
            comboBoxTipo.ValueMember = "Value";

            var items = new[] {
                new { Text = "Libro", Value = TipoMaterial.Libro },
                new { Text = "Manual", Value = TipoMaterial.Manual },
                new { Text = "Revista", Value = TipoMaterial.Revista }
            };

            foreach (var item in items)
            {
                comboBoxTipo.Items.Add(item);
            }
        }

        private void CargarGeneros()
        {
            comboBoxGenero.Items.Clear();

            var generos = new[] {
                "Fantasía",
                "Ciencia Ficción",
                "Aventura",
                "Misterio",
                "Romance",
                "Terror",
                "Histórico",
                "Educativo",
                "Biografía",
                "Poesía",
                "Drama",
                "Comedia",
                "Infantil",
                "Juvenil",
                "Técnico",
                "Científico",
                "Otro"
            };

            foreach (var genero in generos)
            {
                comboBoxGenero.Items.Add(genero);
            }
        }

        private void CargarEdadesRecomendadas()
        {
            comboBoxEdadRecomendada.Items.Clear();

            var edades = new[] {
                "0-3 años",
                "4-6 años",
                "7-9 años",
                "10-12 años",
                "13-15 años",
                "16-18 años",
                "18+ años",
                "Todas las edades"
            };

            foreach (var edad in edades)
            {
                comboBoxEdadRecomendada.Items.Add(edad);
            }
        }

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtISBN.Clear();
            txtEditorial.Clear();
            txtDescripcion.Clear();
            txtAnioPublicacion.Text = DateTime.Now.Year.ToString();
            comboBoxTipo.SelectedIndex = -1;
            comboBoxGenero.SelectedIndex = -1;
            comboBoxEdadRecomendada.SelectedIndex = -1;
            numEjemplares.Value = 1;
        }

        private void BloquearCampos()
        {
            txtTitulo.Enabled = false;
            txtAutor.Enabled = false;
            txtISBN.Enabled = false;
            txtEditorial.Enabled = false;
            txtDescripcion.Enabled = false;
            txtAnioPublicacion.Enabled = false;
            comboBoxTipo.Enabled = false;
            comboBoxGenero.Enabled = false;
            comboBoxEdadRecomendada.Enabled = false;
            numEjemplares.Enabled = false;
        }

        private void DesbloquearCampos()
        {
            txtTitulo.Enabled = true;
            txtAutor.Enabled = true;
            txtISBN.Enabled = true;
            txtEditorial.Enabled = true;
            txtDescripcion.Enabled = true;
            txtAnioPublicacion.Enabled = true;
            comboBoxTipo.Enabled = true;
            comboBoxGenero.Enabled = true;
            comboBoxEdadRecomendada.Enabled = true;
            numEjemplares.Enabled = true;
        }

        private void TxtAnioPublicacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números y teclas de control (backspace, delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtAnioPublicacion_Leave(object sender, EventArgs e)
        {
            // Validar que el año tenga 4 dígitos
            if (!string.IsNullOrWhiteSpace(txtAnioPublicacion.Text))
            {
                if (txtAnioPublicacion.Text.Length != 4)
                {
                    MessageBox.Show(
                        "El año de publicación debe tener exactamente 4 dígitos.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtAnioPublicacion.Focus();
                    return;
                }

                // Validar que sea un año razonable
                if (int.TryParse(txtAnioPublicacion.Text, out int anio))
                {
                    if (anio < 1900 || anio > 2100)
                    {
                        MessageBox.Show(
                            "El año de publicación debe estar entre 1900 y 2100.",
                            "Validación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        txtAnioPublicacion.Focus();
                    }
                }
            }
        }

        #endregion
    }
}
