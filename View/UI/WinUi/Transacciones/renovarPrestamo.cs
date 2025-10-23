using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using BLL;
using DomainModel;

namespace UI.WinUi.Transacciones
{
    public partial class renovarPrestamo : Form
    {
        private Usuario _usuarioLogueado;
        private PrestamoBLL _prestamoBLL;
        private AlumnoBLL _alumnoBLL;
        private MaterialBLL _materialBLL;
        private EjemplarBLL _ejemplarBLL;
        private Timer _searchTimer;
        private const int SEARCH_DELAY = 500;
        private const int MAX_RENOVACIONES = 3;
        private const int MAX_DIAS_ATRASO = 7;
        private const int DIAS_EXTENSION_DEFAULT = 14;

        public renovarPrestamo()
        {
            InitializeComponent();
        }

        public renovarPrestamo(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _prestamoBLL = new PrestamoBLL();
            _alumnoBLL = new AlumnoBLL();
            _materialBLL = new MaterialBLL();
            _ejemplarBLL = new EjemplarBLL();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += RenovarPrestamo_Load;
            btnRenovar.Click += BtnRenovar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVolver.Click += BtnVolver_Click;
            dgvPrestamos.SelectionChanged += DgvPrestamos_SelectionChanged;

            // Configurar búsqueda en tiempo real
            txtBuscarAlumno.TextChanged += TxtBuscar_TextChanged;
            txtBuscarTitulo.TextChanged += TxtBuscar_TextChanged;
            txtBuscarEjemplar.TextChanged += TxtBuscar_TextChanged;

            // Configurar Timer para búsqueda con delay
            _searchTimer = new Timer();
            _searchTimer.Interval = SEARCH_DELAY;
            _searchTimer.Tick += SearchTimer_Tick;

            // Configurar DataGridView
            dgvPrestamos.ReadOnly = true;
            dgvPrestamos.AllowUserToAddRows = false;
            dgvPrestamos.AllowUserToDeleteRows = false;
            dgvPrestamos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPrestamos.MultiSelect = false;
            dgvPrestamos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Configurar NumericUpDown
            numDiasExtension.Minimum = 1;
            numDiasExtension.Maximum = 60;
            numDiasExtension.Value = DIAS_EXTENSION_DEFAULT;

            ConfigurarEstiloDataGridView();
        }

        private void ConfigurarEstiloDataGridView()
        {
            dgvPrestamos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvPrestamos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPrestamos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 247);
            dgvPrestamos.RowsDefaultCellStyle.BackColor = Color.White;
            dgvPrestamos.EnableHeadersVisualStyles = false;
            dgvPrestamos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvPrestamos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPrestamos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvPrestamos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPrestamos.GridColor = Color.FromArgb(189, 195, 199);
        }

        private void RenovarPrestamo_Load(object sender, EventArgs e)
        {
            // Verificar permisos (usa el mismo permiso que el menú Préstamos)
            if (!TienePermiso("Gestión Préstamos"))
            {
                MessageBox.Show(
                    LanguageManager.Translate("sin_permisos"),
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            AplicarTraducciones();
            BuscarYCargarPrestamos();
            LimpiarFormulario();
        }

        private bool TienePermiso(string nombrePatente)
        {
            // Usar el método centralizado del Usuario que maneja el bypass de Administrador
            return _usuarioLogueado?.TienePermiso(nombrePatente) ?? false;
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("renovar_prestamo");
                groupBoxBusqueda.Text = LanguageManager.Translate("buscar_prestamos");
                lblBuscarAlumno.Text = LanguageManager.Translate("buscar_alumno");
                lblBuscarTitulo.Text = LanguageManager.Translate("buscar_titulo");
                lblBuscarEjemplar.Text = LanguageManager.Translate("buscar_codigo_ejemplar");
                groupBoxDatos.Text = LanguageManager.Translate("datos_renovacion");
                lblFechaDevolucionActual.Text = LanguageManager.Translate("fecha_devolucion_actual");
                lblRenovaciones.Text = LanguageManager.Translate("renovaciones_realizadas");
                lblDiasExtension.Text = LanguageManager.Translate("dias_extension");
                lblNuevaFechaDevolucion.Text = LanguageManager.Translate("nueva_fecha_devolucion");
                lblObservaciones.Text = LanguageManager.Translate("observaciones");
                btnRenovar.Text = LanguageManager.Translate("renovar_prestamo");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnVolver.Text = LanguageManager.Translate("volver");

                // Columnas del DataGridView (solo las visibles)
                if (dgvPrestamos.Columns.Count > 0)
                {
                    if (dgvPrestamos.Columns.Contains("NombreAlumno"))
                        dgvPrestamos.Columns["NombreAlumno"].HeaderText = LanguageManager.Translate("alumno");
                    if (dgvPrestamos.Columns.Contains("TituloMaterial"))
                        dgvPrestamos.Columns["TituloMaterial"].HeaderText = LanguageManager.Translate("titulo");
                    if (dgvPrestamos.Columns.Contains("CodigoEjemplar"))
                        dgvPrestamos.Columns["CodigoEjemplar"].HeaderText = LanguageManager.Translate("codigo_ejemplar");
                    if (dgvPrestamos.Columns.Contains("Estado"))
                        dgvPrestamos.Columns["Estado"].HeaderText = LanguageManager.Translate("estado");
                    if (dgvPrestamos.Columns.Contains("FechaPrestamo"))
                        dgvPrestamos.Columns["FechaPrestamo"].HeaderText = LanguageManager.Translate("fecha_prestamo");
                    if (dgvPrestamos.Columns.Contains("FechaDevolucionPrevista"))
                        dgvPrestamos.Columns["FechaDevolucionPrevista"].HeaderText = LanguageManager.Translate("fecha_devolucion");
                    if (dgvPrestamos.Columns.Contains("DiasRestantes"))
                        dgvPrestamos.Columns["DiasRestantes"].HeaderText = LanguageManager.Translate("dias_restantes");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            BuscarYCargarPrestamos();
        }

        private void BuscarYCargarPrestamos()
        {
            try
            {
                string nombreAlumno = string.IsNullOrWhiteSpace(txtBuscarAlumno.Text) ? null : txtBuscarAlumno.Text.Trim();
                string tituloMaterial = string.IsNullOrWhiteSpace(txtBuscarTitulo.Text) ? null : txtBuscarTitulo.Text.Trim();
                string codigoEjemplar = string.IsNullOrWhiteSpace(txtBuscarEjemplar.Text) ? null : txtBuscarEjemplar.Text.Trim();

                DataTable dt = _prestamoBLL.BuscarPrestamosActivos(nombreAlumno, tituloMaterial, codigoEjemplar);

                // Configurar columnas antes de asignar el DataSource
                dgvPrestamos.DataSource = dt;

                // Debug: Verificar qué columnas existen
                System.Diagnostics.Debug.WriteLine("Columnas disponibles:");
                foreach (DataGridViewColumn col in dgvPrestamos.Columns)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {col.Name}");
                }

                // Ocultar TODAS las columnas primero
                foreach (DataGridViewColumn col in dgvPrestamos.Columns)
                {
                    col.Visible = false;
                }

                // Mostrar SOLO las columnas necesarias en el orden especificado
                int displayIndex = 0;

                if (dgvPrestamos.Columns.Contains("NombreAlumno"))
                {
                    dgvPrestamos.Columns["NombreAlumno"].Visible = true;
                    dgvPrestamos.Columns["NombreAlumno"].HeaderText = "Alumno";
                    dgvPrestamos.Columns["NombreAlumno"].Width = 150;
                    dgvPrestamos.Columns["NombreAlumno"].DisplayIndex = displayIndex++;
                }

                if (dgvPrestamos.Columns.Contains("TituloMaterial"))
                {
                    dgvPrestamos.Columns["TituloMaterial"].Visible = true;
                    dgvPrestamos.Columns["TituloMaterial"].HeaderText = "Título";
                    dgvPrestamos.Columns["TituloMaterial"].Width = 200;
                    dgvPrestamos.Columns["TituloMaterial"].DisplayIndex = displayIndex++;
                }

                if (dgvPrestamos.Columns.Contains("CodigoEjemplar"))
                {
                    dgvPrestamos.Columns["CodigoEjemplar"].Visible = true;
                    dgvPrestamos.Columns["CodigoEjemplar"].HeaderText = "Código";
                    dgvPrestamos.Columns["CodigoEjemplar"].Width = 130;
                    dgvPrestamos.Columns["CodigoEjemplar"].DisplayIndex = displayIndex++;
                }

                if (dgvPrestamos.Columns.Contains("Estado"))
                {
                    dgvPrestamos.Columns["Estado"].Visible = true;
                    dgvPrestamos.Columns["Estado"].HeaderText = "Estado";
                    dgvPrestamos.Columns["Estado"].Width = 80;
                    dgvPrestamos.Columns["Estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPrestamos.Columns["Estado"].DisplayIndex = displayIndex++;
                }

                if (dgvPrestamos.Columns.Contains("FechaPrestamo"))
                {
                    dgvPrestamos.Columns["FechaPrestamo"].Visible = true;
                    dgvPrestamos.Columns["FechaPrestamo"].HeaderText = "F. Préstamo";
                    dgvPrestamos.Columns["FechaPrestamo"].Width = 95;
                    dgvPrestamos.Columns["FechaPrestamo"].DefaultCellStyle.Format = "dd/MM/yy";
                    dgvPrestamos.Columns["FechaPrestamo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPrestamos.Columns["FechaPrestamo"].DisplayIndex = displayIndex++;
                }

                if (dgvPrestamos.Columns.Contains("FechaDevolucionPrevista"))
                {
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].Visible = true;
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].HeaderText = "F. Devolución";
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].Width = 95;
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Format = "dd/MM/yy";
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].DisplayIndex = displayIndex++;
                }

                if (dgvPrestamos.Columns.Contains("DiasRestantes"))
                {
                    dgvPrestamos.Columns["DiasRestantes"].Visible = true;
                    dgvPrestamos.Columns["DiasRestantes"].HeaderText = "Días Restantes";
                    dgvPrestamos.Columns["DiasRestantes"].DisplayIndex = displayIndex++;
                    dgvPrestamos.Columns["DiasRestantes"].Width = 80;
                }

                if (dgvPrestamos.Columns.Contains("CantidadRenovaciones"))
                {
                    System.Diagnostics.Debug.WriteLine(">>> Configurando columna CantidadRenovaciones con Width=50");
                    dgvPrestamos.Columns["CantidadRenovaciones"].Visible = true;
                    dgvPrestamos.Columns["CantidadRenovaciones"].HeaderText = "Cant. Renov.";
                    dgvPrestamos.Columns["CantidadRenovaciones"].DisplayIndex = displayIndex++;
                    dgvPrestamos.Columns["CantidadRenovaciones"].Width = 50;
                    dgvPrestamos.Columns["CantidadRenovaciones"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(">>> Columna CantidadRenovaciones NO EXISTE");
                }

                if (dgvPrestamos.Columns.Contains("FechaUltimaRenovacion"))
                {
                    System.Diagnostics.Debug.WriteLine(">>> Configurando columna FechaUltimaRenovacion con Width=90");
                    dgvPrestamos.Columns["FechaUltimaRenovacion"].Visible = true;
                    dgvPrestamos.Columns["FechaUltimaRenovacion"].HeaderText = "F. Ult. Renov.";
                    dgvPrestamos.Columns["FechaUltimaRenovacion"].DisplayIndex = displayIndex++;
                    dgvPrestamos.Columns["FechaUltimaRenovacion"].Width = 90;
                    dgvPrestamos.Columns["FechaUltimaRenovacion"].DefaultCellStyle.Format = "dd/MM/yy";
                    dgvPrestamos.Columns["FechaUltimaRenovacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(">>> Columna FechaUltimaRenovacion NO EXISTE");
                }

                // Debug: Mostrar anchos finales de columnas visibles
                System.Diagnostics.Debug.WriteLine("\nAnchos finales de columnas visibles:");
                foreach (DataGridViewColumn col in dgvPrestamos.Columns)
                {
                    if (col.Visible)
                    {
                        System.Diagnostics.Debug.WriteLine($"  {col.Name}: {col.Width}px (Header: {col.HeaderText})");
                    }
                }

                // Aplicar colores según estado
                foreach (DataGridViewRow row in dgvPrestamos.Rows)
                {
                    if (row.Cells["EstaVencido"] != null && Convert.ToBoolean(row.Cells["EstaVencido"].Value))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                    }
                    else if (row.Cells["DiasRestantes"] != null)
                    {
                        int diasRestantes = Convert.ToInt32(row.Cells["DiasRestantes"].Value);
                        if (diasRestantes <= 2)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 205);
                        }
                    }

                    // Resaltar préstamos que ya tienen muchas renovaciones
                    if (row.Cells["CantidadRenovaciones"] != null)
                    {
                        int renovaciones = Convert.ToInt32(row.Cells["CantidadRenovaciones"].Value);
                        if (renovaciones >= MAX_RENOVACIONES)
                        {
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(200, 0, 0);
                            row.DefaultCellStyle.Font = new Font(dgvPrestamos.Font, FontStyle.Bold);
                        }
                    }
                }

                lblTotalPrestamos.Text = $"{LanguageManager.Translate("total_prestamos")}: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar préstamos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvPrestamos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrestamos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPrestamos.SelectedRows[0];
                CargarDatosPrestamo(row);
            }
        }

        private void CargarDatosPrestamo(DataGridViewRow row)
        {
            try
            {
                // Ya no necesitamos cargar alumno, material, código ejemplar porque están en la tabla
                DateTime fechaDevolucionActual = Convert.ToDateTime(row.Cells["FechaDevolucionPrevista"].Value);
                txtFechaDevolucionActual.Text = fechaDevolucionActual.ToString("dd/MM/yyyy");

                int cantidadRenovaciones = row.Cells["CantidadRenovaciones"].Value != DBNull.Value
                    ? Convert.ToInt32(row.Cells["CantidadRenovaciones"].Value)
                    : 0;

                txtRenovaciones.Text = $"{cantidadRenovaciones} / {MAX_RENOVACIONES}";

                // Cambiar color si alcanzó el límite
                if (cantidadRenovaciones >= MAX_RENOVACIONES)
                {
                    txtRenovaciones.ForeColor = Color.Red;
                    txtRenovaciones.Font = new Font(txtRenovaciones.Font, FontStyle.Bold);
                }
                else
                {
                    txtRenovaciones.ForeColor = Color.Black;
                    txtRenovaciones.Font = new Font(txtRenovaciones.Font, FontStyle.Regular);
                }

                // Mostrar si está vencido
                bool estaVencido = Convert.ToBoolean(row.Cells["EstaVencido"].Value);
                if (estaVencido)
                {
                    int diasAtraso = Convert.ToInt32(row.Cells["DiasAtraso"].Value);
                    lblAdvertencia.Text = $"{LanguageManager.Translate("prestamo_vencido")}: {diasAtraso} {LanguageManager.Translate("dias_atraso")}";
                    lblAdvertencia.ForeColor = Color.Red;
                    lblAdvertencia.Visible = true;
                }
                else
                {
                    lblAdvertencia.Visible = false;
                }

                // Calcular nueva fecha
                CalcularNuevaFecha();

                // Habilitar/deshabilitar botón renovar
                btnRenovar.Enabled = cantidadRenovaciones < MAX_RENOVACIONES;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcularNuevaFecha()
        {
            DateTime nuevaFecha = DateTime.Now.Date.AddDays((int)numDiasExtension.Value);
            txtNuevaFechaDevolucion.Text = nuevaFecha.ToString("dd/MM/yyyy");
        }

        private void NumDiasExtension_ValueChanged(object sender, EventArgs e)
        {
            CalcularNuevaFecha();
        }

        private void BtnRenovar_Click(object sender, EventArgs e)
        {
            if (dgvPrestamos.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    LanguageManager.Translate("seleccionar_prestamo"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Obtener datos de la fila seleccionada para el mensaje de confirmación
            DataGridViewRow row = dgvPrestamos.SelectedRows[0];
            string alumno = row.Cells["NombreAlumno"].Value?.ToString() ?? "";
            string material = row.Cells["TituloMaterial"].Value?.ToString() ?? "";

            DialogResult confirmacion = MessageBox.Show(
                $"{LanguageManager.Translate("confirmar_renovacion")}\n\n" +
                $"{LanguageManager.Translate("alumno")}: {alumno}\n" +
                $"{LanguageManager.Translate("material")}: {material}\n" +
                $"{LanguageManager.Translate("nueva_fecha_devolucion")}: {txtNuevaFechaDevolucion.Text}",
                LanguageManager.Translate("confirmar"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
                return;

            try
            {
                Guid idPrestamo = (Guid)row.Cells["IdPrestamo"].Value;
                int diasExtension = (int)numDiasExtension.Value;
                string observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();

                _prestamoBLL.RenovarPrestamo(
                    idPrestamo,
                    diasExtension,
                    _usuarioLogueado.IdUsuario,
                    MAX_RENOVACIONES,
                    MAX_DIAS_ATRASO,
                    observaciones);

                MessageBox.Show(
                    LanguageManager.Translate("renovacion_exitosa"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Recargar datos
                BuscarYCargarPrestamos();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{LanguageManager.Translate("error_renovar_prestamo")}\n\n{ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtFechaDevolucionActual.Clear();
            txtRenovaciones.Clear();
            txtNuevaFechaDevolucion.Clear();
            txtObservaciones.Clear();
            numDiasExtension.Value = DIAS_EXTENSION_DEFAULT;
            lblAdvertencia.Visible = false;
            btnRenovar.Enabled = false;
            dgvPrestamos.ClearSelection();
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtRenovaciones_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
