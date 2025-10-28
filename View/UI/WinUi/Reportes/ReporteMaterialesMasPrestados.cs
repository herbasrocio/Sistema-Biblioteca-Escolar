using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using Model.DomainModel.DTOs;
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security.Composite;
using Services;

namespace UI.WinUi.Reportes
{
    public partial class ReporteMaterialesMasPrestados : Form
    {
        private readonly ReporteBLL _reporteBLL;
        private readonly ExportService _exportService;
        private Usuario _usuarioLogueado;
        private List<ReporteEstadisticaMaterial> _materialesActuales;

        // Controles del formulario
        private DataGridView dgvReporte;
        private Label lblTitulo;
        private Label lblTop;
        private NumericUpDown nudTop;
        private Button btnGenerar;
        private Button btnExportarCsv;
        private Button btnVolver;
        private Label lblEstadisticas;
        private Panel panelControles;

        public ReporteMaterialesMasPrestados()
        {
            InitializeComponent();
        }

        public ReporteMaterialesMasPrestados(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _reporteBLL = new ReporteBLL();
            _exportService = new ExportService();
        }

        private void InitializeComponent()
        {
            this.dgvReporte = new DataGridView();
            this.lblTitulo = new Label();
            this.lblTop = new Label();
            this.nudTop = new NumericUpDown();
            this.btnGenerar = new Button();
            this.btnExportarCsv = new Button();
            this.btnVolver = new Button();
            this.lblEstadisticas = new Label();
            this.panelControles = new Panel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTop)).BeginInit();
            this.panelControles.SuspendLayout();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(350, 25);
            this.lblTitulo.Text = "Reporte de Materiales Más Prestados";

            // panelControles
            this.panelControles.BorderStyle = BorderStyle.FixedSingle;
            this.panelControles.Location = new Point(20, 60);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new Size(1140, 60);

            // lblTop
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new Point(10, 20);
            this.lblTop.Name = "lblTop";
            this.lblTop.Text = "Top:";

            // nudTop
            this.nudTop.Location = new Point(120, 18);
            this.nudTop.Minimum = 5;
            this.nudTop.Maximum = 100;
            this.nudTop.Value = 20;
            this.nudTop.Name = "nudTop";
            this.nudTop.Width = 80;

            // btnGenerar
            this.btnGenerar.BackColor = Color.FromArgb(52, 152, 219);
            this.btnGenerar.FlatStyle = FlatStyle.Flat;
            this.btnGenerar.ForeColor = Color.White;
            this.btnGenerar.Location = new Point(220, 15);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new Size(120, 30);
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new EventHandler(this.btnGenerar_Click);

            // btnExportarCsv
            this.btnExportarCsv.BackColor = Color.FromArgb(46, 204, 113);
            this.btnExportarCsv.FlatStyle = FlatStyle.Flat;
            this.btnExportarCsv.ForeColor = Color.White;
            this.btnExportarCsv.Location = new Point(360, 15);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new Size(120, 30);
            this.btnExportarCsv.Text = "Exportar CSV";
            this.btnExportarCsv.UseVisualStyleBackColor = false;
            this.btnExportarCsv.Click += new EventHandler(this.btnExportarCsv_Click);

            // btnVolver
            this.btnVolver.BackColor = Color.FromArgb(149, 165, 166);
            this.btnVolver.FlatStyle = FlatStyle.Flat;
            this.btnVolver.ForeColor = Color.White;
            this.btnVolver.Location = new Point(500, 15);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new Size(120, 30);
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            this.btnVolver.Click += new EventHandler(this.btnVolver_Click);

            this.panelControles.Controls.Add(this.lblTop);
            this.panelControles.Controls.Add(this.nudTop);
            this.panelControles.Controls.Add(this.btnGenerar);
            this.panelControles.Controls.Add(this.btnExportarCsv);
            this.panelControles.Controls.Add(this.btnVolver);

            // dgvReporte
            this.dgvReporte.AllowUserToAddRows = false;
            this.dgvReporte.AllowUserToDeleteRows = false;
            this.dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReporte.Location = new Point(20, 140);
            this.dgvReporte.Name = "dgvReporte";
            this.dgvReporte.ReadOnly = true;
            this.dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReporte.Size = new Size(1140, 400);

            // lblEstadisticas
            this.lblEstadisticas.AutoSize = true;
            this.lblEstadisticas.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblEstadisticas.Location = new Point(20, 550);
            this.lblEstadisticas.Name = "lblEstadisticas";
            this.lblEstadisticas.Text = "Total: 0";

            // ReporteMaterialesMasPrestados
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1180, 600);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.dgvReporte);
            this.Controls.Add(this.lblEstadisticas);
            this.Name = "ReporteMaterialesMasPrestados";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Reporte de Materiales Más Prestados";
            this.Load += new EventHandler(this.ReporteMaterialesMasPrestados_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTop)).EndInit();
            this.panelControles.ResumeLayout(false);
            this.panelControles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ReporteMaterialesMasPrestados_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar permisos
                if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("reporteMaterialesMasPrestados"))
                {
                    MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                ConfigurarDataGridView();
                GenerarReporte();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvReporte.AutoGenerateColumns = false;
            dgvReporte.Columns.Clear();

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Titulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                Width = 250
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Autor",
                HeaderText = "Autor",
                DataPropertyName = "Autor",
                Width = 150
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Genero",
                HeaderText = "Género",
                DataPropertyName = "Genero",
                Width = 120
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Nivel",
                HeaderText = "Nivel",
                DataPropertyName = "Nivel",
                Width = 100
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalEjemplares",
                HeaderText = "Total Ejemplares",
                DataPropertyName = "TotalEjemplares",
                Width = 80
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrestamosUltimoMes",
                HeaderText = "Préstamos Último Mes",
                DataPropertyName = "PrestamosUltimoMes",
                Width = 120
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrestamosUltimoAnio",
                HeaderText = "Préstamos Último Año",
                DataPropertyName = "PrestamosUltimoAnio",
                Width = 120
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalPrestamos",
                HeaderText = "Total Préstamos",
                DataPropertyName = "TotalPrestamos",
                Width = 100
            });

            dgvReporte.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvReporte.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReporte.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void GenerarReporte()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                int top = (int)nudTop.Value;
                _materialesActuales = _reporteBLL.ObtenerReporteMaterialesMasPrestados(top);

                dgvReporte.DataSource = null;
                dgvReporte.DataSource = _materialesActuales;

                ActualizarEstadisticas();

                Cursor = Cursors.Default;

                if (_materialesActuales.Count == 0)
                {
                    MessageBox.Show("No hay datos para mostrar",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Error al generar reporte: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarEstadisticas()
        {
            int total = _materialesActuales.Count;
            int totalPrestamos = 0;
            int totalEjemplares = 0;

            foreach (var m in _materialesActuales)
            {
                totalPrestamos += m.TotalPrestamos;
                totalEjemplares += m.TotalEjemplares;
            }

            lblEstadisticas.Text = $"Total materiales: {total} | " +
                                   $"Total préstamos: {totalPrestamos} | " +
                                   $"Total ejemplares: {totalEjemplares}";
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            GenerarReporte();
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            try
            {
                if (_materialesActuales == null || _materialesActuales.Count == 0)
                {
                    MessageBox.Show("No hay datos para exportar",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = $"ReporteMaterialesMasPrestados_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                    Title = "Exportar a CSV"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    _exportService.ExportarEstadisticasMaterialesCsv(_materialesActuales, saveDialog.FileName);
                    MessageBox.Show("Exportación exitosa",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
