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
using BLL;
using DomainModel;

namespace UI.WinUi.Administrador
{
    public partial class consultarMaterial : Form
    {
        private Usuario _usuarioLogueado;
        private MaterialBLL _materialBLL;

        public consultarMaterial()
        {
            InitializeComponent();
            _materialBLL = new MaterialBLL();
        }

        public consultarMaterial(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += ConsultarMaterial_Load;
            btnBuscar.Click += BtnBuscar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVolver.Click += BtnVolver_Click;

            // Configurar DataGridView para solo lectura
            dgvMateriales.ReadOnly = true;
            dgvMateriales.AllowUserToAddRows = false;
            dgvMateriales.AllowUserToDeleteRows = false;
            dgvMateriales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMateriales.MultiSelect = false;

            ConfigurarEstiloDataGridView();
        }

        private void ConfigurarEstiloDataGridView()
        {
            // Colores de selección
            dgvMateriales.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvMateriales.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            // Estilo alternado de filas
            dgvMateriales.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 246, 247);
            dgvMateriales.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;

            // Estilo del header
            dgvMateriales.EnableHeadersVisualStyles = false;
            dgvMateriales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvMateriales.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvMateriales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvMateriales.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Borde y líneas
            dgvMateriales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMateriales.GridColor = System.Drawing.Color.FromArgb(189, 195, 199);
        }

        private void ConsultarMaterial_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarComboBoxes();
            CargarTodosMateriales();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("consultar_material");
                groupBoxFiltros.Text = LanguageManager.Translate("filtros_busqueda");
                lblTitulo.Text = LanguageManager.Translate("titulo");
                lblAutor.Text = LanguageManager.Translate("autor");
                lblTipo.Text = LanguageManager.Translate("tipo");
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void CargarComboBoxes()
        {
            // Cargar tipos de material
            cmbTipo.Items.Clear();
            cmbTipo.Items.Add("Todos");
            cmbTipo.Items.Add("Libro");
            cmbTipo.Items.Add("Revista");
            cmbTipo.Items.Add("Manual");
            cmbTipo.SelectedIndex = 0;
        }

        private void CargarTodosMateriales()
        {
            try
            {
                List<Material> materiales = _materialBLL.ObtenerTodosMateriales();
                dgvMateriales.DataSource = materiales;

                // Configurar columnas
                if (dgvMateriales.Columns.Count > 0)
                {
                    dgvMateriales.Columns["IdMaterial"].Visible = false;
                    dgvMateriales.Columns["FechaRegistro"].Visible = false;
                    dgvMateriales.Columns["Activo"].Visible = false;

                    dgvMateriales.Columns["Titulo"].HeaderText = "Título";
                    dgvMateriales.Columns["Autor"].HeaderText = "Autor";
                    dgvMateriales.Columns["Editorial"].HeaderText = "Editorial";
                    dgvMateriales.Columns["Tipo"].HeaderText = "Tipo";
                    dgvMateriales.Columns["Genero"].HeaderText = "Género";
                    dgvMateriales.Columns["CantidadTotal"].HeaderText = "Cantidad Total";
                    dgvMateriales.Columns["CantidadDisponible"].HeaderText = "Disponibles";
                }

                lblResultados.Text = $"Resultados encontrados: {materiales.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materiales: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string titulo = txtTitulo.Text.Trim();
                string autor = txtAutor.Text.Trim();
                string tipo = cmbTipo.SelectedItem?.ToString();

                List<Material> materiales = _materialBLL.BuscarMateriales(titulo, autor, tipo);
                dgvMateriales.DataSource = materiales;

                // Configurar columnas
                if (dgvMateriales.Columns.Count > 0)
                {
                    dgvMateriales.Columns["IdMaterial"].Visible = false;
                    dgvMateriales.Columns["FechaRegistro"].Visible = false;
                    dgvMateriales.Columns["Activo"].Visible = false;

                    dgvMateriales.Columns["Titulo"].HeaderText = "Título";
                    dgvMateriales.Columns["Autor"].HeaderText = "Autor";
                    dgvMateriales.Columns["Editorial"].HeaderText = "Editorial";
                    dgvMateriales.Columns["Tipo"].HeaderText = "Tipo";
                    dgvMateriales.Columns["Genero"].HeaderText = "Género";
                    dgvMateriales.Columns["CantidadTotal"].HeaderText = "Cantidad Total";
                    dgvMateriales.Columns["CantidadDisponible"].HeaderText = "Disponibles";
                }

                lblResultados.Text = $"Resultados encontrados: {materiales.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            cmbTipo.SelectedIndex = 0;
            CargarTodosMateriales();
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
