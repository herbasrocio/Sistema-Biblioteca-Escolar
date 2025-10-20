using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using DomainModel.Enums;
using ServicesSecurity.Services;

namespace UI.WinUi.Transacciones
{
    public partial class SeleccionarEjemplar : Form
    {
        private readonly EjemplarBLL _ejemplarBLL;
        private readonly Material _material;
        public Ejemplar EjemplarSeleccionado { get; private set; }

        public SeleccionarEjemplar(Material material)
        {
            InitializeComponent();
            _ejemplarBLL = new EjemplarBLL();
            _material = material;

            ConfigurarIdioma();
            CargarEjemplares();
        }

        private void ConfigurarIdioma()
        {
            this.Text = LanguageManager.Translate("seleccionar_ejemplar");
            lblTitulo.Text = _material.Titulo;
            lblInfo.Text = $"{LanguageManager.Translate("autor")}: {_material.Autor}";
            btnAceptar.Text = LanguageManager.Translate("aceptar");
            btnCancelar.Text = LanguageManager.Translate("cancelar");
            lblSeleccione.Text = LanguageManager.Translate("seleccione_ejemplar_disponible");
        }

        private void CargarEjemplares()
        {
            try
            {
                var ejemplares = _ejemplarBLL.ObtenerEjemplaresPorMaterial(_material.IdMaterial);

                // Filtrar solo ejemplares disponibles
                var ejemplaresDisponibles = ejemplares
                    .Where(e => e.Estado == EstadoMaterial.Disponible)
                    .OrderBy(e => e.NumeroEjemplar)
                    .ToList();

                if (!ejemplaresDisponibles.Any())
                {
                    MessageBox.Show(
                        LanguageManager.Translate("no_ejemplares_disponibles"),
                        LanguageManager.Translate("informacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                CargarDataGridView(ejemplaresDisponibles);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CargarDataGridView(List<Ejemplar> ejemplares)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IdEjemplar", typeof(Guid));
                dt.Columns.Add("NumeroEjemplar", typeof(int));
                dt.Columns.Add("CodigoEjemplar", typeof(string));
                dt.Columns.Add("Ubicacion", typeof(string));
                dt.Columns.Add("Estado", typeof(string));

                foreach (var ejemplar in ejemplares)
                {
                    DataRow row = dt.NewRow();
                    row["IdEjemplar"] = ejemplar.IdEjemplar;
                    row["NumeroEjemplar"] = ejemplar.NumeroEjemplar;
                    row["CodigoEjemplar"] = ejemplar.CodigoEjemplar ?? "";
                    row["Ubicacion"] = ejemplar.Ubicacion ?? "";
                    row["Estado"] = TraducirEstado(ejemplar.Estado);
                    dt.Rows.Add(row);
                }

                dgvEjemplares.DataSource = dt;
                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ejemplares: {ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            try
            {
                // Validar que el DataGridView esté inicializado
                if (dgvEjemplares == null)
                {
                    MessageBox.Show("Error: dgvEjemplares es null", "Error de inicialización");
                    return;
                }

                if (dgvEjemplares.Columns == null)
                {
                    MessageBox.Show("Error: dgvEjemplares.Columns es null", "Error de inicialización");
                    return;
                }

                // Ocultar columna ID
                if (dgvEjemplares.Columns.Contains("IdEjemplar"))
                    dgvEjemplares.Columns["IdEjemplar"].Visible = false;

                // Configurar Número de Ejemplar
                if (dgvEjemplares.Columns.Contains("NumeroEjemplar"))
                {
                    var col = dgvEjemplares.Columns["NumeroEjemplar"];
                    string textoTraducido = LanguageManager.Translate("numero_ejemplar");
                    if (!string.IsNullOrEmpty(textoTraducido))
                        col.HeaderText = textoTraducido;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = 80;
                }

                // Configurar Código de Ejemplar
                if (dgvEjemplares.Columns.Contains("CodigoEjemplar"))
                {
                    var col = dgvEjemplares.Columns["CodigoEjemplar"];
                    string textoTraducido = LanguageManager.Translate("codigo_ejemplar");
                    if (!string.IsNullOrEmpty(textoTraducido))
                        col.HeaderText = textoTraducido;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = 150;
                }

                // Configurar Ubicación
                if (dgvEjemplares.Columns.Contains("Ubicacion"))
                {
                    var col = dgvEjemplares.Columns["Ubicacion"];
                    string textoTraducido = LanguageManager.Translate("ubicacion");
                    if (!string.IsNullOrEmpty(textoTraducido))
                        col.HeaderText = textoTraducido;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.Width = 120;
                }

                // Configurar Estado
                if (dgvEjemplares.Columns.Contains("Estado"))
                {
                    var col = dgvEjemplares.Columns["Estado"];
                    string textoTraducido = LanguageManager.Translate("estado");
                    if (!string.IsNullOrEmpty(textoTraducido))
                        col.HeaderText = textoTraducido;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Seleccionar la primera fila por defecto
                if (dgvEjemplares.Rows.Count > 0)
                {
                    dgvEjemplares.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar columnas: {ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string TraducirEstado(EstadoMaterial estado)
        {
            switch (estado)
            {
                case EstadoMaterial.Disponible:
                    return LanguageManager.Translate("estado_disponible");
                case EstadoMaterial.Prestado:
                    return LanguageManager.Translate("estado_prestado");
                case EstadoMaterial.EnReparacion:
                    return LanguageManager.Translate("estado_en_reparacion");
                case EstadoMaterial.NoDisponible:
                    return LanguageManager.Translate("estado_no_disponible");
                default:
                    return estado.ToString();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvEjemplares.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    LanguageManager.Translate("seleccione_ejemplar"),
                    LanguageManager.Translate("validacion"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                DataGridViewRow row = dgvEjemplares.SelectedRows[0];
                Guid idEjemplar = (Guid)row.Cells["IdEjemplar"].Value;

                EjemplarSeleccionado = _ejemplarBLL.ObtenerEjemplarPorId(idEjemplar);

                if (EjemplarSeleccionado != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Translate("error")}: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dgvEjemplares_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnAceptar_Click(sender, e);
            }
        }
    }
}
