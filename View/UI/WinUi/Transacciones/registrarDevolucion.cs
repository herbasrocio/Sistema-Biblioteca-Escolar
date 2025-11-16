using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Services.DomainModel.Security.Composite;
using Services.Services;
using BLL;
using DomainModel;
using UI.WinUi;

namespace UI.WinUi.Transacciones
{
    public partial class registrarDevolucion : BaseForm
    {
        private Usuario _usuarioLogueado;
        private PrestamoBLL _prestamoBLL;
        private DevolucionBLL _devolucionBLL;
        private AlumnoBLL _alumnoBLL;
        private MaterialBLL _materialBLL;
        private EjemplarBLL _ejemplarBLL;
        private InscripcionBLL _inscripcionBLL;
        private BitacoraOperacionesBLL _bitacoraBLL;
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
            _bitacoraBLL = new BitacoraOperacionesBLL();
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
            // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
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

        protected override void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("registrar_devolucion");
                lblTitulo.Text = LanguageManager.Translate("prestamos_activos_y_vencidos");
                groupBoxBusqueda.Text = LanguageManager.Translate("buscar_prestamo");
                lblBuscarAlumno.Text = LanguageManager.Translate("nombre_alumno_label");
                lblBuscarTitulo.Text = LanguageManager.Translate("titulo_material_label");
                lblBuscarEjemplar.Text = LanguageManager.Translate("codigo_ejemplar_label");
                groupBoxDatos.Text = LanguageManager.Translate("datos_devolucion");
                lblObservaciones.Text = LanguageManager.Translate("observaciones");
                btnRegistrar.Text = LanguageManager.Translate("registrar_devolucion");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnVolver.Text = LanguageManager.Translate("volver");

                // Actualizar textos dinámicos
                ActualizarTextosDinamicos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void ActualizarTextosDinamicos()
        {
            // Recargar la búsqueda para actualizar el texto de resultados
            if (dgvPrestamos.DataSource != null)
            {
                BuscarYCargarPrestamos();
            }

            // Actualizar el texto de estado y ubicación si hay un préstamo seleccionado
            if (dgvPrestamos.SelectedRows.Count > 0)
            {
                DgvPrestamos_SelectionChanged(dgvPrestamos, EventArgs.Empty);
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
                    lblResultados.Text = LanguageManager.Translate("no_prestamos_encontrados");
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

                string textoResultados = LanguageManager.Translate("resultados_prestamos");
                string textoPrestamos = LanguageManager.Translate("prestamos");
                string textoVencidos = LanguageManager.Translate("vencidos");
                lblResultados.Text = $"{textoResultados}: {totalPrestamos} {textoPrestamos} | {textoVencidos}: {prestamosVencidos}";
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

                dgvPrestamos.Columns["CodigoEjemplar"].HeaderText = LanguageManager.Translate("codigo_ejemplar");
                dgvPrestamos.Columns["CodigoEjemplar"].Width = 125;
                dgvPrestamos.Columns["CodigoEjemplar"].DisplayIndex = 2;

                dgvPrestamos.Columns["FechaDevolucionPrevista"].HeaderText = LanguageManager.Translate("fecha_vencimiento_abr");
                dgvPrestamos.Columns["FechaDevolucionPrevista"].Width = 90;
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DisplayIndex = 3;
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Format = "dd/MM/yy";
                dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPrestamos.Columns["DiasAtraso"].HeaderText = LanguageManager.Translate("dias_atraso_abr");
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
                    string textoEstado = LanguageManager.Translate("estado");
                    string textoAtrasado = LanguageManager.Translate("atrasado");
                    string textoDias = LanguageManager.Translate("dias");
                    lblEstado.Text = $"{textoEstado}: {textoAtrasado.ToUpper()} ({diasAtraso} {textoDias})";
                    lblEstado.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
                }
                else
                {
                    string textoEstado = LanguageManager.Translate("estado");
                    string textoAlDia = LanguageManager.Translate("al_dia");
                    string textoDiasRestantes = LanguageManager.Translate("dias_restantes");
                    lblEstado.Text = $"{textoEstado}: {textoAlDia} ({diasRestantes} {textoDiasRestantes})";
                    lblEstado.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
                }

                string textoFechaPrestamo = LanguageManager.Translate("fecha_prestamo");
                string textoDevolucionPrevista = LanguageManager.Translate("devolucion_prevista");
                lblFechaPrestamo.Text = $"{textoFechaPrestamo}: {fechaPrestamo:dd/MM/yyyy}";
                lblFechaDevolucionPrevista.Text = $"{textoDevolucionPrevista}: {fechaDevolucionPrevista:dd/MM/yyyy}";

                // Mostrar ubicación del ejemplar
                if (!string.IsNullOrEmpty(ubicacion) && ubicacion != "No registrada")
                {
                    string textoUbicar = LanguageManager.Translate("ubicar_en");
                    string textoCodigo = LanguageManager.Translate("codigo");
                    lblUbicacion.Text = $"{textoUbicar.ToUpper()}: {ubicacion.ToUpper()}  |  {textoCodigo}: {codigoEjemplar}";
                    lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    string textoCodigo = LanguageManager.Translate("codigo");
                    string textoUbicacion = LanguageManager.Translate("ubicacion");
                    string textoNoRegistrada = LanguageManager.Translate("no_registrada");
                    lblUbicacion.Text = $"{textoCodigo}: {codigoEjemplar}  |  {textoUbicacion}: {textoNoRegistrada}";
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
                    mensajeConfirmacion += $"ATENCIÓN: {diasAtraso} DÍA(S) DE ATRASO\n\n";
                }
                else
                {
                    mensajeConfirmacion += "Devolución a tiempo\n\n";
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

                // Registrar en bitácora
                _bitacoraBLL.RegistrarOperacion(new BitacoraOperaciones
                {
                    IdUsuario = _usuarioLogueado.IdUsuario,
                    NombreUsuario = _usuarioLogueado.Nombre,
                    TipoOperacion = "Creacion",
                    Modulo = "Devoluciones",
                    Accion = "Registrar devolución",
                    EntidadAfectada = "Devolucion",
                    IdEntidad = null,
                    Detalle = $"Devolución registrada: Material '{tituloMaterial}' de alumno '{nombreAlumno}'. Ejemplar #{numeroEjemplar} ({codigoEjemplar}). {(diasAtraso > 0 ? $"CON ATRASO: {diasAtraso} día(s)" : "A tiempo")}"
                });

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
                    encabezado = "DEVOLUCIÓN REGISTRADA A TIEMPO\n\n";
                }

                string mensaje = encabezado;
                mensaje += "═══════════════════════════════════════\n\n";
                mensaje += $"Material: {tituloMaterial}\n";
                mensaje += $"Ejemplar #{numeroEjemplar}\n";
                mensaje += $"Código: {codigoEjemplar}\n";

                if (!string.IsNullOrEmpty(ubicacion) && ubicacion != "No registrada")
                {
                    string textoUbicar = LanguageManager.Translate("ubicar_en");
                    mensaje += $"\n{textoUbicar.ToUpper()}: {ubicacion.ToUpper()}\n";
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
