using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using Model.DomainModel.DTOs;
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security.Composite;
using Services;
using UI.WinUi;

namespace UI.WinUi.Reportes
{
    public partial class ReporteUsoPorGrado : BaseForm
    {
        private readonly ReporteBLL _reporteBLL;
        private readonly ExportService _exportService;
        private Usuario _usuarioLogueado;
        private List<Model.DomainModel.DTOs.ReporteUsoPorGrado> _reportesActuales;

        // Controles del formulario
        private DataGridView dgvReporte;
        private Label lblTitulo;
        private Label lblAnio;
        private NumericUpDown nudAnio;
        private Button btnGenerar;
        private Button btnExportarCsv;
        private Button btnVolver;
        private Label lblEstadisticas;
        private Panel panelControles;

        public ReporteUsoPorGrado()
        {
            InitializeComponent();
        }

        public ReporteUsoPorGrado(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _reporteBLL = new ReporteBLL();
            _exportService = new ExportService();
        }

        private void InitializeComponent()
        {
            this.dgvReporte = new DataGridView();
            this.lblTitulo = new Label();
            this.lblAnio = new Label();
            this.nudAnio = new NumericUpDown();
            this.btnGenerar = new Button();
            this.btnExportarCsv = new Button();
            this.btnVolver = new Button();
            this.lblEstadisticas = new Label();
            this.panelControles = new Panel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnio)).BeginInit();
            this.panelControles.SuspendLayout();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(400, 25);
            this.lblTitulo.Text = "";  // Se establecerá en AplicarTraducciones()

            // panelControles
            this.panelControles.BorderStyle = BorderStyle.FixedSingle;
            this.panelControles.Location = new Point(20, 60);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new Size(1340, 60);

            // lblAnio
            this.lblAnio.AutoSize = true;
            this.lblAnio.Location = new Point(10, 20);
            this.lblAnio.Name = "lblAnio";
            this.lblAnio.Text = "";  // Se establecerá en AplicarTraducciones()

            // nudAnio
            this.nudAnio.Location = new Point(100, 18);
            this.nudAnio.Minimum = 2000;
            this.nudAnio.Maximum = DateTime.Now.Year + 1;
            this.nudAnio.Value = DateTime.Now.Year;
            this.nudAnio.Name = "nudAnio";
            this.nudAnio.Width = 80;

            // btnGenerar
            this.btnGenerar.BackColor = Color.FromArgb(52, 152, 219);
            this.btnGenerar.FlatStyle = FlatStyle.Flat;
            this.btnGenerar.ForeColor = Color.White;
            this.btnGenerar.Location = new Point(200, 15);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new Size(120, 30);
            this.btnGenerar.Text = "";  // Se establecerá en AplicarTraducciones()
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new EventHandler(this.btnGenerar_Click);

            // btnExportarCsv
            this.btnExportarCsv.BackColor = Color.FromArgb(46, 204, 113);
            this.btnExportarCsv.FlatStyle = FlatStyle.Flat;
            this.btnExportarCsv.ForeColor = Color.White;
            this.btnExportarCsv.Location = new Point(340, 15);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new Size(120, 30);
            this.btnExportarCsv.Text = "";  // Se establecerá en AplicarTraducciones()
            this.btnExportarCsv.UseVisualStyleBackColor = false;
            this.btnExportarCsv.Click += new EventHandler(this.btnExportarCsv_Click);

            // btnVolver
            this.btnVolver.BackColor = Color.FromArgb(149, 165, 166);
            this.btnVolver.FlatStyle = FlatStyle.Flat;
            this.btnVolver.ForeColor = Color.White;
            this.btnVolver.Location = new Point(480, 15);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new Size(120, 30);
            this.btnVolver.Text = "";  // Se establecerá en AplicarTraducciones()
            this.btnVolver.UseVisualStyleBackColor = false;
            this.btnVolver.Click += new EventHandler(this.btnVolver_Click);

            this.panelControles.Controls.Add(this.lblAnio);
            this.panelControles.Controls.Add(this.nudAnio);
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
            this.dgvReporte.Size = new Size(1340, 400);

            // lblEstadisticas
            this.lblEstadisticas.AutoSize = true;
            this.lblEstadisticas.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblEstadisticas.Location = new Point(20, 550);
            this.lblEstadisticas.Name = "lblEstadisticas";
            this.lblEstadisticas.Text = "Total: 0";

            // ReporteUsoPorGrado
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1380, 600);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.dgvReporte);
            this.Controls.Add(this.lblEstadisticas);
            this.Name = "ReporteUsoPorGrado";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "";  // Se establecerá en AplicarTraducciones()
            this.Load += new EventHandler(this.ReporteUsoPorGrado_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnio)).EndInit();
            this.panelControles.ResumeLayout(false);
            this.panelControles.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ReporteUsoPorGrado_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar permisos
                if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("consultarReportes"))
                {
                    MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
                ConfigurarDataGridView();
                GenerarReporte();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void AplicarTraducciones()
        {
            this.Text = LanguageManager.Translate("reporte_uso_por_grado");
            lblTitulo.Text = LanguageManager.Translate("reporte_uso_por_grado");
            lblAnio.Text = LanguageManager.Translate("anio_lectivo") + ":";
            btnGenerar.Text = LanguageManager.Translate("generar_reporte");
            btnExportarCsv.Text = LanguageManager.Translate("exportar_csv");
            btnVolver.Text = LanguageManager.Translate("volver");

            // Reconfigurar columnas si ya están creadas
            if (dgvReporte.Columns.Count > 0)
            {
                if (dgvReporte.Columns["Grado"] != null)
                    dgvReporte.Columns["Grado"].HeaderText = LanguageManager.Translate("grado");
                if (dgvReporte.Columns["Division"] != null)
                    dgvReporte.Columns["Division"].HeaderText = LanguageManager.Translate("division");
                if (dgvReporte.Columns["CantidadAlumnos"] != null)
                    dgvReporte.Columns["CantidadAlumnos"].HeaderText = LanguageManager.Translate("cantidad_alumnos");
                if (dgvReporte.Columns["CantidadPrestamos"] != null)
                    dgvReporte.Columns["CantidadPrestamos"].HeaderText = LanguageManager.Translate("cantidad_prestamos");
                if (dgvReporte.Columns["PromedioPrestamosPorAlumno"] != null)
                    dgvReporte.Columns["PromedioPrestamosPorAlumno"].HeaderText = LanguageManager.Translate("promedio_prestamos_alumno");
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvReporte.AutoGenerateColumns = false;
            dgvReporte.Columns.Clear();

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "Grado", HeaderText = "Grado", DataPropertyName = "Grado", Width = 80 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "Division", HeaderText = "División", DataPropertyName = "Division", Width = 80 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "CantidadAlumnos", HeaderText = "Alumnos", DataPropertyName = "CantidadAlumnos", Width = 80 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalPrestamos", HeaderText = "Total Préstamos", DataPropertyName = "TotalPrestamos", Width = 100 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrestamosActivos", HeaderText = "Activos", DataPropertyName = "PrestamosActivos", Width = 80 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrestamosDevueltos", HeaderText = "Devueltos", DataPropertyName = "PrestamosDevueltos", Width = 80 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "PrestamosVencidos", HeaderText = "Vencidos", DataPropertyName = "PrestamosVencidos", Width = 80 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "GeneroMasPrestado", HeaderText = "Género + Prestado", DataPropertyName = "GeneroMasPrestado", Width = 150 });
            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaterialMasPrestado", HeaderText = "Material + Prestado", DataPropertyName = "MaterialMasPrestado", Width = 200 });

            dgvReporte.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvReporte.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReporte.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void GenerarReporte()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                int anio = (int)nudAnio.Value;
                _reportesActuales = _reporteBLL.ObtenerReporteUsoPorGrado(anio);

                dgvReporte.DataSource = null;
                dgvReporte.DataSource = _reportesActuales;

                ActualizarEstadisticas();

                Cursor = Cursors.Default;

                if (_reportesActuales.Count == 0)
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
            int totalGrados = _reportesActuales.Count;
            int totalAlumnos = 0;
            int totalPrestamos = 0;

            foreach (var r in _reportesActuales)
            {
                totalAlumnos += r.CantidadAlumnos;
                totalPrestamos += r.TotalPrestamos;
            }

            lblEstadisticas.Text = $"Total grados/divisiones: {totalGrados} | " +
                                   $"Total alumnos: {totalAlumnos} | " +
                                   $"Total préstamos: {totalPrestamos}";
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            GenerarReporte();
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            try
            {
                if (_reportesActuales == null || _reportesActuales.Count == 0)
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
                    FileName = $"ReporteUsoPorGrado_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                    Title = "Exportar a CSV"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    _exportService.ExportarUsoPorGradoCsv(_reportesActuales, saveDialog.FileName);
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
