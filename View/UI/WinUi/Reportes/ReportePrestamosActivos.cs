using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLL;
using Model.DomainModel.DTOs;
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security.Composite;
using Services;

namespace UI.WinUi.Reportes
{
    public partial class ReportePrestamosActivos : Form
    {
        private readonly ReporteBLL _reporteBLL;
        private readonly ExportService _exportService;
        private Usuario _usuarioLogueado;
        private List<ReportePrestamo> _prestamosActuales;

        public ReportePrestamosActivos()
        {
            InitializeComponent();
        }

        public ReportePrestamosActivos(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _reporteBLL = new ReporteBLL();
            _exportService = new ExportService();
        }

        private void ReportePrestamosActivos_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar permisos
                if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("reportePrestamosActivos"))
                {
                    MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                ConfigurarFormulario();
                ConfigurarDataGridView();
                GenerarReporte();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            // Configurar títulos y traducciones
            this.Text = LanguageManager.Translate("reporte_prestamos_activos");
            lblDesde.Text = LanguageManager.Translate("fecha_desde") + ":";
            lblHasta.Text = LanguageManager.Translate("fecha_hasta") + ":";
            lblEstado.Text = LanguageManager.Translate("estado") + ":";
            btnGenerar.Text = LanguageManager.Translate("generar_reporte");
            btnExportarCsv.Text = LanguageManager.Translate("exportar_csv");
            btnImprimir.Text = LanguageManager.Translate("imprimir");
            btnVolver.Text = LanguageManager.Translate("volver");

            // Configurar combo de estados
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("TODOS");
            cmbEstado.Items.Add("VIGENTE");
            cmbEstado.Items.Add("POR VENCER");
            cmbEstado.Items.Add("VENCIDO");
            cmbEstado.SelectedIndex = 0;

            // Configurar DateTimePickers
            // IMPORTANTE: establecer Value ANTES de MaxDate para evitar error
            // si el valor por defecto del control está fuera del rango que vamos a establecer
            dtpDesde.Value = DateTime.Now.AddDays(-30);
            dtpHasta.Value = DateTime.Now;

            // Ahora sí podemos establecer los límites
            dtpDesde.MaxDate = DateTime.Now;
            dtpHasta.MaxDate = DateTime.Now;
        }

        private void ConfigurarDataGridView()
        {
            dgvReporte.AutoGenerateColumns = false;
            dgvReporte.ReadOnly = true;
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.MultiSelect = false;

            // Configurar columnas
            dgvReporte.Columns.Clear();

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Alumno",
                HeaderText = LanguageManager.Translate("alumno"),
                DataPropertyName = "Alumno",
                Width = 150
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DNI",
                HeaderText = "DNI",
                DataPropertyName = "DNI",
                Width = 100
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Titulo",
                HeaderText = LanguageManager.Translate("titulo"),
                DataPropertyName = "Titulo",
                Width = 200
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Autor",
                HeaderText = LanguageManager.Translate("autor"),
                DataPropertyName = "Autor",
                Width = 120
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CodigoEjemplar",
                HeaderText = LanguageManager.Translate("codigo_ejemplar"),
                DataPropertyName = "CodigoEjemplar",
                Width = 100
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FechaPrestamo",
                HeaderText = LanguageManager.Translate("fecha_prestamo"),
                DataPropertyName = "FechaPrestamo",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FechaDevolucionEsperada",
                HeaderText = LanguageManager.Translate("fecha_devolucion"),
                DataPropertyName = "FechaDevolucionEsperada",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DiasRestantes",
                HeaderText = LanguageManager.Translate("dias_restantes"),
                DataPropertyName = "DiasRestantes",
                Width = 80
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EstadoPrestamo",
                HeaderText = LanguageManager.Translate("estado"),
                DataPropertyName = "EstadoPrestamo",
                Width = 100
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Grado",
                HeaderText = LanguageManager.Translate("grado"),
                DataPropertyName = "Grado",
                Width = 60
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Division",
                HeaderText = LanguageManager.Translate("division"),
                DataPropertyName = "Division",
                Width = 60
            });

            // Configurar estilos
            dgvReporte.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvReporte.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvReporte.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            // Aplicar formato condicional por estado
            dgvReporte.CellFormatting += DgvReporte_CellFormatting;
        }

        private void DgvReporte_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReporte.Columns[e.ColumnIndex].Name == "EstadoPrestamo" && e.Value != null)
            {
                string estado = e.Value.ToString();
                switch (estado)
                {
                    case "VENCIDO":
                        e.CellStyle.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
                        e.CellStyle.ForeColor = System.Drawing.Color.White;
                        break;
                    case "POR VENCER":
                        e.CellStyle.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        break;
                    case "VIGENTE":
                        e.CellStyle.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
                        e.CellStyle.ForeColor = System.Drawing.Color.White;
                        break;
                }
            }
        }

        private void GenerarReporte()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Obtener filtros
                DateTime? fechaDesde = dtpDesde.Checked ? (DateTime?)dtpDesde.Value : null;
                DateTime? fechaHasta = dtpHasta.Checked ? (DateTime?)dtpHasta.Value : null;
                string estado = cmbEstado.SelectedItem.ToString() == "TODOS" ? null : cmbEstado.SelectedItem.ToString();

                // Obtener datos
                _prestamosActuales = _reporteBLL.ObtenerReportePrestamosActivos(fechaDesde, fechaHasta, estado);

                // Mostrar en grid
                dgvReporte.DataSource = null;
                dgvReporte.DataSource = _prestamosActuales;

                // Actualizar estadísticas
                ActualizarEstadisticas();

                Cursor = Cursors.Default;

                if (_prestamosActuales.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("no_hay_datos"),
                        LanguageManager.Translate("informacion"),
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
            int total = _prestamosActuales.Count;
            int vigentes = _prestamosActuales.FindAll(p => p.EstadoPrestamo == "VIGENTE").Count;
            int porVencer = _prestamosActuales.FindAll(p => p.EstadoPrestamo == "POR VENCER").Count;
            int vencidos = _prestamosActuales.FindAll(p => p.EstadoPrestamo == "VENCIDO").Count;

            lblEstadisticas.Text = $"Total: {total} | " +
                                   $"{LanguageManager.Translate("vigente")}: {vigentes} | " +
                                   $"{LanguageManager.Translate("por_vencer")}: {porVencer} | " +
                                   $"{LanguageManager.Translate("vencido")}: {vencidos}";
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            GenerarReporte();
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            try
            {
                if (_prestamosActuales == null || _prestamosActuales.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("no_hay_datos"),
                        LanguageManager.Translate("informacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = $"ReportePrestamos_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                    Title = LanguageManager.Translate("exportar_csv")
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    _exportService.ExportarPrestamosCsv(_prestamosActuales, saveDialog.FileName);
                    MessageBox.Show(LanguageManager.Translate("exportacion_exitosa"),
                        LanguageManager.Translate("exito"),
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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // TODO: Implementar impresión con ReportViewer después de instalar paquete NuGet
            MessageBox.Show("Para habilitar la impresión, instale el paquete NuGet:\n\n" +
                          "Microsoft.ReportingServices.ReportViewerControl.WinForms\n\n" +
                          "Versión 150.1484.0",
                          "Función no disponible",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
