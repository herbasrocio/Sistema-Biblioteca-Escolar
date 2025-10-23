using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using BLL;
using DomainModel;

namespace UI.WinUi.Transacciones
{
    public partial class registrarDevolucion : Form
    {
        private Usuario _usuarioLogueado;
        private PrestamoBLL _prestamoBLL;
        private DevolucionBLL _devolucionBLL;
        private AlumnoBLL _alumnoBLL;
        private MaterialBLL _materialBLL;
        private EjemplarBLL _ejemplarBLL;
        private InscripcionBLL _inscripcionBLL;
        private List<Alumno> _alumnosGrado;
        private Timer _searchTimer;
        private const int SEARCH_DELAY = 500; // 500ms de delay para búsqueda en tiempo real

        // Clase auxiliar para mostrar préstamos con detalles
        private class PrestamoDetalle
        {
            public Guid IdPrestamo { get; set; }
            public string Alumno { get; set; }
            public string TituloMaterial { get; set; }
            public string CodigoEjemplar { get; set; }
            public int NumeroEjemplar { get; set; }
            public string Ubicacion { get; set; }
            public DateTime FechaPrestamo { get; set; }
            public DateTime FechaDevolucionPrevista { get; set; }
            public int DiasAtraso { get; set; }
            public string Estado { get; set; }
            public bool EstaVencido { get; set; }
            public Prestamo PrestamoOriginal { get; set; }
        }

        public registrarDevolucion()
        {
            InitializeComponent();
        }

        public registrarDevolucion(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _prestamoBLL = new PrestamoBLL();
            _devolucionBLL = new DevolucionBLL();
            _alumnoBLL = new AlumnoBLL();
            _materialBLL = new MaterialBLL();
            _ejemplarBLL = new EjemplarBLL();
            _inscripcionBLL = new InscripcionBLL();
            _alumnosGrado = new List<Alumno>();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += RegistrarDevolucion_Load;
            btnRegistrar.Click += BtnRegistrar_Click;
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

            ConfigurarEstiloDataGridView();
        }

        private void ConfigurarEstiloDataGridView()
        {
            dgvPrestamos.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvPrestamos.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvPrestamos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 246, 247);
            dgvPrestamos.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvPrestamos.EnableHeadersVisualStyles = false;
            dgvPrestamos.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvPrestamos.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvPrestamos.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvPrestamos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPrestamos.GridColor = System.Drawing.Color.FromArgb(189, 195, 199);
        }

        private void RegistrarDevolucion_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            BuscarYCargarPrestamos();
        }

        /// <summary>
        /// Evento que se dispara cuando el usuario escribe en los campos de búsqueda
        /// </summary>
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Detener el timer anterior
            _searchTimer.Stop();
            // Reiniciar el timer (espera 500ms después de que el usuario deja de escribir)
            _searchTimer.Start();
        }

        /// <summary>
        /// Evento del timer que ejecuta la búsqueda después del delay
        /// </summary>
        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            BuscarYCargarPrestamos();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("registrar_devolucion");
                groupBoxDatos.Text = LanguageManager.Translate("datos_devolucion");
                lblObservaciones.Text = LanguageManager.Translate("observaciones");
                btnRegistrar.Text = LanguageManager.Translate("registrar_devolucion");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca y carga préstamos activos con filtros de búsqueda en tiempo real
        /// </summary>
        private void BuscarYCargarPrestamos()
        {
            try
            {
                string nombreAlumno = txtBuscarAlumno.Text.Trim();
                string tituloMaterial = txtBuscarTitulo.Text.Trim();
                string codigoEjemplar = txtBuscarEjemplar.Text.Trim();

                // Si los campos están vacíos, pasar null para obtener todos los préstamos
                if (string.IsNullOrWhiteSpace(nombreAlumno)) nombreAlumno = null;
                if (string.IsNullOrWhiteSpace(tituloMaterial)) tituloMaterial = null;
                if (string.IsNullOrWhiteSpace(codigoEjemplar)) codigoEjemplar = null;

                // Buscar préstamos usando el nuevo método optimizado
                DataTable dtPrestamos = _prestamoBLL.BuscarPrestamosActivos(nombreAlumno, tituloMaterial, codigoEjemplar);

                if (dtPrestamos.Rows.Count == 0)
                {
                    dgvPrestamos.DataSource = null;
                    lblResultados.Text = "No se encontraron préstamos con los criterios especificados";
                    lblResultados.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
                    return;
                }

                // Configurar DataGridView con el DataTable directamente
                dgvPrestamos.DataSource = dtPrestamos;
                ConfigurarColumnasDataGridViewOptimizado();

                // Colorear filas vencidas
                foreach (DataGridViewRow row in dgvPrestamos.Rows)
                {
                    if (row.DataBoundItem is DataRowView dataRow)
                    {
                        bool estaVencido = Convert.ToBoolean(dataRow["EstaVencido"]);
                        if (estaVencido)
                        {
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 220, 220);
                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(180, 0, 0);
                            row.DefaultCellStyle.Font = new System.Drawing.Font(dgvPrestamos.Font, System.Drawing.FontStyle.Bold);
                        }
                    }
                }

                int totalPrestamos = dtPrestamos.Rows.Count;
                int prestamosVencidos = dtPrestamos.AsEnumerable().Count(row => Convert.ToBoolean(row["EstaVencido"]));

                lblResultados.Text = $"Resultados: {totalPrestamos} préstamos | Vencidos: {prestamosVencidos}";
                lblResultados.ForeColor = totalPrestamos > 0 ? System.Drawing.Color.FromArgb(39, 174, 96) : System.Drawing.Color.FromArgb(127, 140, 141);
                lblResultados.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar préstamos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carga todos los préstamos activos y vencidos en el DataGridView (mantener para compatibilidad)
        /// </summary>
        private void CargarPrestamosActivos()
        {
            txtBuscarAlumno.Clear();
            txtBuscarTitulo.Clear();
            txtBuscarEjemplar.Clear();
            BuscarYCargarPrestamos();
        }

        /// <summary>
        /// Configura las columnas del DataGridView (versión optimizada para DataTable)
        /// </summary>
        private void ConfigurarColumnasDataGridViewOptimizado()
        {
            if (dgvPrestamos.Columns.Count > 0)
            {
                // Ocultar columnas técnicas
                dgvPrestamos.Columns["IdPrestamo"].Visible = false;
                dgvPrestamos.Columns["IdMaterial"].Visible = false;
                dgvPrestamos.Columns["IdEjemplar"].Visible = false;
                dgvPrestamos.Columns["IdAlumno"].Visible = false;
                dgvPrestamos.Columns["IdUsuario"].Visible = false;
                dgvPrestamos.Columns["FechaPrestamo"].Visible = false;
                dgvPrestamos.Columns["EstaVencido"].Visible = false;
                dgvPrestamos.Columns["Autor"].Visible = false;
                dgvPrestamos.Columns["DNIAlumno"].Visible = false;
                dgvPrestamos.Columns["Ubicacion"].Visible = false;
                dgvPrestamos.Columns["DiasRestantes"].Visible = false;

                // Configurar columnas visibles (ajustadas para evitar scroll horizontal)
                dgvPrestamos.Columns["NombreAlumno"].HeaderText = LanguageManager.Translate("alumno");
                dgvPrestamos.Columns["NombreAlumno"].Width = 140;
                dgvPrestamos.Columns["NombreAlumno"].DisplayIndex = 0;

                dgvPrestamos.Columns["TituloMaterial"].HeaderText = LanguageManager.Translate("material");
                dgvPrestamos.Columns["TituloMaterial"].Width = 180;
                dgvPrestamos.Columns["TituloMaterial"].DisplayIndex = 1;

                dgvPrestamos.Columns["NumeroEjemplar"].Visible = false; // Ocultar columna no relevante

                dgvPrestamos.Columns["CodigoEjemplar"].HeaderText = "Código";
                dgvPrestamos.Columns["CodigoEjemplar"].Width = 125;
                dgvPrestamos.Columns["CodigoEjemplar"].DisplayIndex = 2;

                dgvPrestamos.Columns["FechaDevolucionPrevista"].HeaderText = "F. Venc.";
                dgvPrestamos.Columns["FechaDevolucionPrevista"].Width = 90;
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DisplayIndex = 3;
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Format = "dd/MM/yy";
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPrestamos.Columns["DiasAtraso"].HeaderText = "Días";
                dgvPrestamos.Columns["DiasAtraso"].Width = 50;
                dgvPrestamos.Columns["DiasAtraso"].DisplayIndex = 4;
                dgvPrestamos.Columns["DiasAtraso"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPrestamos.Columns["Estado"].HeaderText = LanguageManager.Translate("estado");
                dgvPrestamos.Columns["Estado"].Width = 90;
                dgvPrestamos.Columns["Estado"].DisplayIndex = 5;
                dgvPrestamos.Columns["Estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Configura las columnas del DataGridView (mantener para compatibilidad)
        /// </summary>
        private void ConfigurarColumnasDataGridView()
        {
            ConfigurarColumnasDataGridViewOptimizado();
        }

        // Este método ya no se usa - los préstamos se cargan automáticamente al abrir la ventana
        // Se mantiene para compatibilidad con el diseñador de formularios si existe el botón

        private void DgvPrestamos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrestamos.SelectedRows.Count > 0 && dgvPrestamos.SelectedRows[0].DataBoundItem is DataRowView dataRow)
            {
                DateTime fechaPrestamo = Convert.ToDateTime(dataRow["FechaPrestamo"]);
                DateTime fechaDevolucionPrevista = Convert.ToDateTime(dataRow["FechaDevolucionPrevista"]);
                int diasRestantes = Convert.ToInt32(dataRow["DiasRestantes"]);
                int diasAtraso = Convert.ToInt32(dataRow["DiasAtraso"]);
                string codigoEjemplar = dataRow["CodigoEjemplar"].ToString();
                string ubicacion = dataRow["Ubicacion"].ToString();
                bool estaVencido = Convert.ToBoolean(dataRow["EstaVencido"]);

                // Mostrar estado
                if (estaVencido)
                {
                    lblEstado.Text = $"Estado: ATRASADO ({diasAtraso} días)";
                    lblEstado.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
                }
                else
                {
                    lblEstado.Text = $"Estado: Al día ({diasRestantes} días restantes)";
                    lblEstado.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
                }

                lblFechaPrestamo.Text = $"Fecha préstamo: {fechaPrestamo:dd/MM/yyyy}";
                lblFechaDevolucionPrevista.Text = $"Devolución prevista: {fechaDevolucionPrevista:dd/MM/yyyy}";

                // Mostrar ubicación del ejemplar
                if (!string.IsNullOrEmpty(ubicacion) && ubicacion != "No registrada")
                {
                    lblUbicacion.Text = $"UBICAR EN: {ubicacion.ToUpper()}  |  Código: {codigoEjemplar}";
                    lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    lblUbicacion.Text = $"Código: {codigoEjemplar}  |  Ubicación: No registrada";
                    lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
                }
            }
            else
            {
                lblEstado.Text = "";
                lblFechaPrestamo.Text = "";
                lblFechaDevolucionPrevista.Text = "";
                lblUbicacion.Text = "";
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPrestamos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un préstamo para devolver", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!(dgvPrestamos.SelectedRows[0].DataBoundItem is DataRowView dataRow))
                {
                    MessageBox.Show("Error al obtener los datos del préstamo seleccionado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Obtener datos del DataRow
                Guid idPrestamo = (Guid)dataRow["IdPrestamo"];
                string nombreAlumno = dataRow["NombreAlumno"].ToString();
                string tituloMaterial = dataRow["TituloMaterial"].ToString();
                int numeroEjemplar = Convert.ToInt32(dataRow["NumeroEjemplar"]);
                string codigoEjemplar = dataRow["CodigoEjemplar"].ToString();
                string ubicacion = dataRow["Ubicacion"].ToString();
                int diasAtraso = Convert.ToInt32(dataRow["DiasAtraso"]);
                DateTime fechaPrestamo = Convert.ToDateTime(dataRow["FechaPrestamo"]);
                DateTime fechaDevolucionPrevista = Convert.ToDateTime(dataRow["FechaDevolucionPrevista"]);

                // ═══════════════════════════════════════════════════════════════
                // CONFIRMACIÓN ANTES DE REGISTRAR DEVOLUCIÓN
                // ═══════════════════════════════════════════════════════════════

                string mensajeConfirmacion = "¿CONFIRMAR DEVOLUCIÓN?\n\n";
                mensajeConfirmacion += "═══════════════════════════════════════\n\n";
                mensajeConfirmacion += $"Alumno: {nombreAlumno}\n";
                mensajeConfirmacion += $"Material: {tituloMaterial}\n";
                mensajeConfirmacion += $"Ejemplar: #{numeroEjemplar} ({codigoEjemplar})\n\n";
                mensajeConfirmacion += $"Fecha préstamo: {fechaPrestamo:dd/MM/yyyy}\n";
                mensajeConfirmacion += $"Fecha vencimiento: {fechaDevolucionPrevista:dd/MM/yyyy}\n\n";

                if (diasAtraso > 0)
                {
                    mensajeConfirmacion += $"⚠️  ATENCIÓN: {diasAtraso} DÍA(S) DE ATRASO\n\n";
                }
                else
                {
                    mensajeConfirmacion += "✓ Devolución a tiempo\n\n";
                }

                if (!string.IsNullOrEmpty(ubicacion) && ubicacion != "No registrada")
                {
                    mensajeConfirmacion += $"Ubicar en: {ubicacion}\n\n";
                }

                mensajeConfirmacion += "═══════════════════════════════════════\n\n";
                mensajeConfirmacion += "¿Desea registrar esta devolución?";

                DialogResult confirmacion = MessageBox.Show(
                    mensajeConfirmacion,
                    "Confirmar Devolución",
                    MessageBoxButtons.YesNo,
                    diasAtraso > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2  // Por defecto en "No" para mayor seguridad
                );

                if (confirmacion != DialogResult.Yes)
                {
                    return; // Usuario canceló
                }

                // ═══════════════════════════════════════════════════════════════
                // PROCEDER CON EL REGISTRO
                // ═══════════════════════════════════════════════════════════════

                // Obtener el préstamo completo desde BLL
                var prestamo = _prestamoBLL.ObtenerPrestamoPorId(idPrestamo);
                if (prestamo == null)
                {
                    MessageBox.Show("No se encontró el préstamo seleccionado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Devolucion devolucion = new Devolucion
                {
                    IdPrestamo = prestamo.IdPrestamo,
                    FechaDevolucion = DateTime.Now,
                    IdUsuario = _usuarioLogueado.IdUsuario,
                    Observaciones = txtObservaciones.Text.Trim(),
                    Prestamo = prestamo
                };

                _devolucionBLL.RegistrarDevolucion(devolucion);

                // Construir mensaje de éxito con formato mejorado
                string titulo;
                string encabezado;

                if (diasAtraso > 0)
                {
                    titulo = "Devolución Registrada - CON ATRASO";
                    encabezado = $"DEVOLUCIÓN CON {diasAtraso} DÍA(S) DE ATRASO\n\n";
                }
                else
                {
                    titulo = "Devolución Registrada";
                    encabezado = "✓ DEVOLUCIÓN REGISTRADA A TIEMPO\n\n";
                }

                string mensaje = encabezado;
                mensaje += "═══════════════════════════════════════\n\n";
                mensaje += $"Material: {tituloMaterial}\n";
                mensaje += $"Ejemplar #{numeroEjemplar}\n";
                mensaje += $"Código: {codigoEjemplar}\n";

                if (!string.IsNullOrEmpty(ubicacion) && ubicacion != "No registrada")
                {
                    mensaje += $"\nUBICAR EN: {ubicacion.ToUpper()}\n";
                }

                mensaje += "\n═══════════════════════════════════════\n";
                mensaje += "Por favor, devolver el material a su ubicación.";

                MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar la lista de préstamos activos con los filtros actuales
                BuscarYCargarPrestamos();

                // Limpiar observaciones
                txtObservaciones.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar devolución: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtObservaciones.Clear();
            CargarPrestamosActivos();
            lblEstado.Text = "";
            lblFechaPrestamo.Text = "";
            lblFechaDevolucionPrevista.Text = "";
            lblUbicacion.Text = "";
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPrestamos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
