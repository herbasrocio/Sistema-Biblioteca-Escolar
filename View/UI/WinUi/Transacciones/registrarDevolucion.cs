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
            public string TituloMaterial { get; set; }
            public string CodigoEjemplar { get; set; }
            public int NumeroEjemplar { get; set; }
            public string Ubicacion { get; set; }
            public DateTime FechaPrestamo { get; set; }
            public DateTime FechaDevolucionPrevista { get; set; }
            public string Estado { get; set; }
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
            btnBuscar.Click += BtnBuscar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVolver.Click += BtnVolver_Click;
            dgvPrestamos.SelectionChanged += DgvPrestamos_SelectionChanged;
            cmbGradoDivision.SelectedIndexChanged += CmbGradoDivision_SelectedIndexChanged;

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
            CargarGrados();
            LimpiarCampos();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("registrar_devolucion");
                groupBoxBusqueda.Text = LanguageManager.Translate("buscar_prestamo");
                groupBoxDatos.Text = LanguageManager.Translate("datos_devolucion");
                lblAlumno.Text = LanguageManager.Translate("alumno");
                lblObservaciones.Text = LanguageManager.Translate("observaciones");
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnRegistrar.Text = LanguageManager.Translate("registrar");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void CargarGrados()
        {
            try
            {
                cmbGradoDivision.Items.Clear();
                cmbGradoDivision.Items.Add("-- Seleccione grado --");

                // Obtener estadísticas del año actual
                int anioActual = DateTime.Now.Year;
                var estadisticas = _inscripcionBLL.ObtenerEstadisticasPorAnio(anioActual);

                foreach (var est in estadisticas)
                {
                    string gradoFormateado = FormatearGrado(est.Grado, est.Division);
                    cmbGradoDivision.Items.Add(gradoFormateado);
                }

                if (cmbGradoDivision.Items.Count > 0)
                {
                    cmbGradoDivision.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar grados: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatearGrado(string grado, string division)
        {
            if (string.IsNullOrEmpty(grado) || string.IsNullOrEmpty(division))
                return string.Empty;

            int gradoNum;
            if (int.TryParse(grado, out gradoNum))
            {
                string sufijo;
                switch (gradoNum)
                {
                    case 1: sufijo = "ro"; break;
                    case 2: sufijo = "do"; break;
                    case 3: sufijo = "ro"; break;
                    case 7: sufijo = "mo"; break;
                    default: sufijo = "to"; break;
                }
                return $"{gradoNum}{sufijo} {division}";
            }
            return $"{grado} {division}";
        }

        private void CmbGradoDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGradoDivision.SelectedIndex > 0)
            {
                string gradoSeleccionado = cmbGradoDivision.SelectedItem.ToString();
                var partes = gradoSeleccionado.Split(' ');

                if (partes.Length == 2)
                {
                    string grado = new string(partes[0].Where(char.IsDigit).ToArray());
                    string division = partes[1];
                    CargarAlumnosPorGrado(grado, division);
                }
            }
            else
            {
                cmbAlumno.DataSource = null;
                cmbAlumno.Items.Clear();
                cmbAlumno.Items.Add("-- Seleccione un grado primero --");
                cmbAlumno.SelectedIndex = 0;
            }
        }

        private void CargarAlumnosPorGrado(string grado, string division)
        {
            try
            {
                _alumnosGrado.Clear();

                int anioActual = DateTime.Now.Year;
                var inscripciones = _inscripcionBLL.ObtenerInscripcionesPorGrado(anioActual, grado, division);

                foreach (var inscripcion in inscripciones)
                {
                    var alumno = _alumnoBLL.ObtenerAlumnoPorId(inscripcion.IdAlumno);
                    if (alumno != null)
                    {
                        _alumnosGrado.Add(alumno);
                    }
                }

                // Ordenar por apellido
                _alumnosGrado = _alumnosGrado.OrderBy(a => a.Apellido).ThenBy(a => a.Nombre).ToList();

                // Configurar ComboBox con DataSource
                cmbAlumno.DataSource = null;
                cmbAlumno.Items.Clear();
                cmbAlumno.Items.Add("-- Todos los alumnos --");

                foreach (var alumno in _alumnosGrado)
                {
                    cmbAlumno.Items.Add(alumno.NombreCompleto);
                }

                if (cmbAlumno.Items.Count > 0)
                {
                    cmbAlumno.SelectedIndex = 0; // Seleccionar "Todos"
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que se haya seleccionado un grado
                if (cmbGradoDivision.SelectedIndex <= 0)
                {
                    MessageBox.Show("Debe seleccionar un grado", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<Prestamo> prestamosParaDevolver = new List<Prestamo>();

                // Si seleccionó "Todos los alumnos"
                if (cmbAlumno.SelectedIndex == 0)
                {
                    // Buscar préstamos de todos los alumnos del grado
                    foreach (var alumno in _alumnosGrado)
                    {
                        var prestamosAlumno = _prestamoBLL.ObtenerPorAlumno(alumno.IdAlumno);
                        var prestamosActivos = prestamosAlumno.FindAll(p => p.Estado == "Activo" || p.Estado == "Atrasado");
                        prestamosParaDevolver.AddRange(prestamosActivos);
                    }
                }
                else
                {
                    // Buscar préstamos de un alumno específico
                    int indiceAlumno = cmbAlumno.SelectedIndex - 1; // -1 porque el índice 0 es "Todos"
                    if (indiceAlumno >= 0 && indiceAlumno < _alumnosGrado.Count)
                    {
                        var alumno = _alumnosGrado[indiceAlumno];
                        var prestamosAlumno = _prestamoBLL.ObtenerPorAlumno(alumno.IdAlumno);
                        prestamosParaDevolver = prestamosAlumno.FindAll(p => p.Estado == "Activo" || p.Estado == "Atrasado");
                    }
                }

                if (prestamosParaDevolver.Count == 0)
                {
                    string mensaje = cmbAlumno.SelectedIndex == 0
                        ? "No hay préstamos activos para este grado"
                        : $"{cmbAlumno.SelectedItem} no tiene préstamos activos";

                    MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvPrestamos.DataSource = null;
                    return;
                }

                // Crear lista de detalles con información completa
                List<PrestamoDetalle> detalles = new List<PrestamoDetalle>();

                foreach (var prestamo in prestamosParaDevolver)
                {
                    // Obtener material
                    var material = _materialBLL.ObtenerMaterialPorId(prestamo.IdMaterial);

                    // Obtener ejemplar (si existe) - IMPORTANTE: usar el IdEjemplar del préstamo
                    string codigoEjemplar = "N/A";
                    int numeroEjemplar = 0;
                    string ubicacion = "No registrada";

                    // DEBUG: Agregar logging para investigar
                    string debugInfo = $"DEBUG - Préstamo ID: {prestamo.IdPrestamo}\n" +
                                      $"Material: {material?.Titulo ?? "null"}\n" +
                                      $"IdEjemplar en Préstamo: {prestamo.IdEjemplar}\n";

                    if (prestamo.IdEjemplar != Guid.Empty)
                    {
                        var ejemplar = _ejemplarBLL.ObtenerEjemplarPorId(prestamo.IdEjemplar);
                        if (ejemplar != null)
                        {
                            codigoEjemplar = ejemplar.CodigoEjemplar ?? "N/A";
                            numeroEjemplar = ejemplar.NumeroEjemplar;
                            ubicacion = ejemplar.Ubicacion ?? "No registrada";

                            debugInfo += $"Ejemplar encontrado:\n" +
                                        $"  - NumeroEjemplar: {numeroEjemplar}\n" +
                                        $"  - CodigoEjemplar: {codigoEjemplar}\n" +
                                        $"  - Ubicacion: {ubicacion}\n";
                        }
                        else
                        {
                            debugInfo += "Ejemplar NO encontrado en BD\n";
                        }
                    }
                    else
                    {
                        debugInfo += "IdEjemplar está vacío (Guid.Empty)\n";
                    }

                    // Mostrar debug info en consola (aparecerá en la ventana de Output de Visual Studio)
                    System.Diagnostics.Debug.WriteLine(debugInfo);
                    Console.WriteLine(debugInfo);

                    detalles.Add(new PrestamoDetalle
                    {
                        IdPrestamo = prestamo.IdPrestamo,
                        TituloMaterial = material?.Titulo ?? "Material no encontrado",
                        CodigoEjemplar = codigoEjemplar,
                        NumeroEjemplar = numeroEjemplar,
                        Ubicacion = ubicacion,
                        FechaPrestamo = prestamo.FechaPrestamo,
                        FechaDevolucionPrevista = prestamo.FechaDevolucionPrevista,
                        Estado = prestamo.Estado,
                        PrestamoOriginal = prestamo
                    });
                }

                dgvPrestamos.DataSource = detalles;

                // Configurar columnas
                if (dgvPrestamos.Columns.Count > 0)
                {
                    dgvPrestamos.Columns["IdPrestamo"].Visible = false;
                    dgvPrestamos.Columns["PrestamoOriginal"].Visible = false;
                    dgvPrestamos.Columns["Ubicacion"].Visible = false; // Mostrar en panel lateral

                    dgvPrestamos.Columns["TituloMaterial"].HeaderText = LanguageManager.Translate("material");
                    dgvPrestamos.Columns["TituloMaterial"].Width = 200;
                    dgvPrestamos.Columns["TituloMaterial"].DisplayIndex = 0;

                    dgvPrestamos.Columns["NumeroEjemplar"].HeaderText = "Ej.#";
                    dgvPrestamos.Columns["NumeroEjemplar"].Width = 45;
                    dgvPrestamos.Columns["NumeroEjemplar"].DisplayIndex = 1;
                    dgvPrestamos.Columns["NumeroEjemplar"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                    dgvPrestamos.Columns["NumeroEjemplar"].HeaderCell.Style.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

                    dgvPrestamos.Columns["CodigoEjemplar"].HeaderText = LanguageManager.Translate("codigo_ejemplar");
                    dgvPrestamos.Columns["CodigoEjemplar"].Width = 130;
                    dgvPrestamos.Columns["CodigoEjemplar"].DisplayIndex = 2;

                    dgvPrestamos.Columns["FechaPrestamo"].HeaderText = "F. Préstamo";
                    dgvPrestamos.Columns["FechaPrestamo"].Width = 90;
                    dgvPrestamos.Columns["FechaPrestamo"].DisplayIndex = 3;
                    dgvPrestamos.Columns["FechaPrestamo"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPrestamos.Columns["FechaPrestamo"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

                    dgvPrestamos.Columns["FechaDevolucionPrevista"].HeaderText = "F. Devolución Prevista";
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].Width = 130;
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].DisplayIndex = 4;
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

                    dgvPrestamos.Columns["Estado"].HeaderText = LanguageManager.Translate("estado");
                    dgvPrestamos.Columns["Estado"].Width = 75;
                    dgvPrestamos.Columns["Estado"].DisplayIndex = 5;
                    dgvPrestamos.Columns["Estado"].DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                }

                lblResultados.Text = $"📋 Préstamos activos: {prestamosParaDevolver.Count}";
                lblResultados.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
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
                    lblUbicacion.Text = $"📦  UBICAR EN: {detalle.Ubicacion.ToUpper()}  |  Código: {detalle.CodigoEjemplar}";
                    lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    lblUbicacion.Text = $"📦  Código: {detalle.CodigoEjemplar}  |  Ubicación: No registrada";
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
                    encabezado = $"⚠ DEVOLUCIÓN CON {diasAtraso} DÍA(S) DE ATRASO\n\n";
                }
                else
                {
                    titulo = "Devolución Registrada";
                    encabezado = "✓ DEVOLUCIÓN REGISTRADA A TIEMPO\n\n";
                }

                string mensaje = encabezado;
                mensaje += "═══════════════════════════════════════\n\n";
                mensaje += $"📚 Material: {detalleSeleccionado.TituloMaterial}\n";
                mensaje += $"📖 Ejemplar #{detalleSeleccionado.NumeroEjemplar}\n";
                mensaje += $"🔖 Código: {detalleSeleccionado.CodigoEjemplar}\n";

                if (!string.IsNullOrEmpty(detalleSeleccionado.Ubicacion) && detalleSeleccionado.Ubicacion != "No registrada")
                {
                    mensaje += $"\n📦 UBICAR EN: {detalleSeleccionado.Ubicacion.ToUpper()}\n";
                }

                mensaje += "\n═══════════════════════════════════════\n";
                mensaje += "Por favor, devolver el material a su ubicación.";

                MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar campos sin recargar la búsqueda
                LimpiarCampos();
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
            if (cmbGradoDivision.Items.Count > 0)
                cmbGradoDivision.SelectedIndex = 0;

            cmbAlumno.DataSource = null;
            cmbAlumno.Items.Clear();
            cmbAlumno.Items.Add("-- Seleccione un grado primero --");
            cmbAlumno.SelectedIndex = 0;

            txtObservaciones.Clear();
            dgvPrestamos.DataSource = null;
            lblResultados.Text = "";
            lblEstado.Text = "";
            lblFechaPrestamo.Text = "";
            lblFechaDevolucionPrevista.Text = "";
            lblUbicacion.Text = "";
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
