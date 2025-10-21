using System;
using System.Collections.Generic;
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
            CargarPrestamosActivos();
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
        /// Carga todos los préstamos activos y vencidos en el DataGridView
        /// </summary>
        private void CargarPrestamosActivos()
        {
            try
            {
                var prestamosActivos = _prestamoBLL.ObtenerPrestamosActivosYVencidos();

                if (prestamosActivos.Count == 0)
                {
                    MessageBox.Show("No hay préstamos activos en este momento", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvPrestamos.DataSource = null;
                    lblResultados.Text = "No hay préstamos pendientes";
                    return;
                }

                // Crear lista de detalles con información completa
                List<PrestamoDetalle> detalles = new List<PrestamoDetalle>();

                foreach (var prestamo in prestamosActivos)
                {
                    // Obtener datos relacionados
                    var material = _materialBLL.ObtenerMaterialPorId(prestamo.IdMaterial);
                    var alumno = _alumnoBLL.ObtenerAlumnoPorId(prestamo.IdAlumno);

                    string codigoEjemplar = "N/A";
                    int numeroEjemplar = 0;
                    string ubicacion = "No registrada";

                    if (prestamo.IdEjemplar != Guid.Empty)
                    {
                        var ejemplar = _ejemplarBLL.ObtenerEjemplarPorId(prestamo.IdEjemplar);
                        if (ejemplar != null)
                        {
                            codigoEjemplar = ejemplar.CodigoEjemplar ?? "N/A";
                            numeroEjemplar = ejemplar.NumeroEjemplar;
                            ubicacion = ejemplar.Ubicacion ?? "No registrada";
                        }
                    }

                    int diasAtraso = prestamo.DiasRestantes() < 0 ? Math.Abs(prestamo.DiasRestantes()) : 0;

                    detalles.Add(new PrestamoDetalle
                    {
                        IdPrestamo = prestamo.IdPrestamo,
                        Alumno = alumno?.NombreCompleto ?? "Desconocido",
                        TituloMaterial = material?.Titulo ?? "Material no encontrado",
                        CodigoEjemplar = codigoEjemplar,
                        NumeroEjemplar = numeroEjemplar,
                        Ubicacion = ubicacion,
                        FechaPrestamo = prestamo.FechaPrestamo,
                        FechaDevolucionPrevista = prestamo.FechaDevolucionPrevista,
                        DiasAtraso = diasAtraso,
                        Estado = prestamo.Estado,
                        EstaVencido = prestamo.EstaAtrasado(),
                        PrestamoOriginal = prestamo
                    });
                }

                dgvPrestamos.DataSource = detalles;
                ConfigurarColumnasDataGridView();

                // Colorear filas vencidas
                foreach (DataGridViewRow row in dgvPrestamos.Rows)
                {
                    var detalle = (PrestamoDetalle)row.DataBoundItem;
                    if (detalle.EstaVencido)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 220, 220);
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(180, 0, 0);
                        row.DefaultCellStyle.Font = new System.Drawing.Font(dgvPrestamos.Font, System.Drawing.FontStyle.Bold);
                    }
                }

                lblResultados.Text = $"Préstamos activos: {prestamosActivos.Count} | Vencidos: {detalles.Count(d => d.EstaVencido)}";
                lblResultados.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar préstamos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configura las columnas del DataGridView
        /// </summary>
        private void ConfigurarColumnasDataGridView()
        {
            if (dgvPrestamos.Columns.Count > 0)
            {
                dgvPrestamos.Columns["IdPrestamo"].Visible = false;
                dgvPrestamos.Columns["PrestamoOriginal"].Visible = false;
                dgvPrestamos.Columns["Ubicacion"].Visible = false;
                dgvPrestamos.Columns["EstaVencido"].Visible = false;

                dgvPrestamos.Columns["Alumno"].HeaderText = LanguageManager.Translate("alumno");
                dgvPrestamos.Columns["Alumno"].Width = 150;
                dgvPrestamos.Columns["Alumno"].DisplayIndex = 0;

                dgvPrestamos.Columns["TituloMaterial"].HeaderText = LanguageManager.Translate("material");
                dgvPrestamos.Columns["TituloMaterial"].Width = 150;
                dgvPrestamos.Columns["TituloMaterial"].DisplayIndex = 1;

                dgvPrestamos.Columns["NumeroEjemplar"].HeaderText = "Ej.#";
                dgvPrestamos.Columns["NumeroEjemplar"].Width = 45;
                dgvPrestamos.Columns["NumeroEjemplar"].DisplayIndex = 2;
                dgvPrestamos.Columns["NumeroEjemplar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPrestamos.Columns["CodigoEjemplar"].HeaderText = LanguageManager.Translate("codigo_ejemplar");
                dgvPrestamos.Columns["CodigoEjemplar"].Width = 110;
                dgvPrestamos.Columns["CodigoEjemplar"].DisplayIndex = 3;

                dgvPrestamos.Columns["FechaDevolucionPrevista"].HeaderText = "F. Vencimiento";
                dgvPrestamos.Columns["FechaDevolucionPrevista"].Width = 100;
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DisplayIndex = 4;
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPrestamos.Columns["DiasAtraso"].HeaderText = "Días Atraso";
                dgvPrestamos.Columns["DiasAtraso"].Width = 40;
                dgvPrestamos.Columns["DiasAtraso"].DisplayIndex = 5;
                dgvPrestamos.Columns["DiasAtraso"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPrestamos.Columns["Estado"].HeaderText = LanguageManager.Translate("estado");
                dgvPrestamos.Columns["Estado"].Width = 75;
                dgvPrestamos.Columns["Estado"].DisplayIndex = 6;
                dgvPrestamos.Columns["Estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPrestamos.Columns["FechaPrestamo"].Visible = false;
            }
        }

        // Este método ya no se usa - los préstamos se cargan automáticamente al abrir la ventana
        // Se mantiene para compatibilidad con el diseñador de formularios si existe el botón

        private void DgvPrestamos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrestamos.SelectedRows.Count > 0)
            {
                var detalle = (PrestamoDetalle)dgvPrestamos.SelectedRows[0].DataBoundItem;
                var prestamo = detalle.PrestamoOriginal;

                // Calcular días de atraso
                int diasRestantes = prestamo.DiasRestantes();

                if (diasRestantes < 0)
                {
                    lblEstado.Text = $"Estado: ATRASADO ({Math.Abs(diasRestantes)} días)";
                    lblEstado.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
                }
                else
                {
                    lblEstado.Text = $"Estado: Al día ({diasRestantes} días restantes)";
                    lblEstado.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
                }

                lblFechaPrestamo.Text = $"Fecha préstamo: {prestamo.FechaPrestamo:dd/MM/yyyy}";
                lblFechaDevolucionPrevista.Text = $"Devolución prevista: {prestamo.FechaDevolucionPrevista:dd/MM/yyyy}";

                // Mostrar ubicación del ejemplar desde el detalle ya cargado
                if (!string.IsNullOrEmpty(detalle.Ubicacion) && detalle.Ubicacion != "No registrada")
                {
                    lblUbicacion.Text = $"UBICAR EN: {detalle.Ubicacion.ToUpper()}  |  Código: {detalle.CodigoEjemplar}";
                    lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    lblUbicacion.Text = $"Código: {detalle.CodigoEjemplar}  |  Ubicación: No registrada";
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

                var detalleSeleccionado = (PrestamoDetalle)dgvPrestamos.SelectedRows[0].DataBoundItem;
                var prestamo = detalleSeleccionado.PrestamoOriginal;

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
                int diasAtraso = devolucion.DiasDeAtraso();
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
                mensaje += $"Material: {detalleSeleccionado.TituloMaterial}\n";
                mensaje += $"Ejemplar #{detalleSeleccionado.NumeroEjemplar}\n";
                mensaje += $"Código: {detalleSeleccionado.CodigoEjemplar}\n";

                if (!string.IsNullOrEmpty(detalleSeleccionado.Ubicacion) && detalleSeleccionado.Ubicacion != "No registrada")
                {
                    mensaje += $"\nUBICAR EN: {detalleSeleccionado.Ubicacion.ToUpper()}\n";
                }

                mensaje += "\n═══════════════════════════════════════\n";
                mensaje += "Por favor, devolver el material a su ubicación.";

                MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recargar la lista de préstamos activos
                CargarPrestamosActivos();

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
