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
    public partial class registrarPrestamo : Form
    {
        private Usuario _usuarioLogueado;
        private MaterialBLL _materialBLL;
        private AlumnoBLL _alumnoBLL;
        private PrestamoBLL _prestamoBLL;
        private InscripcionBLL _inscripcionBLL;
        private EjemplarBLL _ejemplarBLL;
        private BitacoraBibliotecarioBLL _bitacoraBLL;
        private List<MaterialDetalle> _materialesFiltrados;
        private List<Alumno> _alumnosGrado;
        private Ejemplar _ejemplarSeleccionado; // Ejemplar seleccionado por el usuario
        private bool _permitirSeleccion = false; // Controlar cuándo se puede abrir el diálogo
        private Timer _searchTimer; // Timer para búsqueda con delay
        private const int SEARCH_DELAY = 500; // Delay en milisegundos

        // Clase auxiliar para mostrar materiales con ubicación
        private class MaterialDetalle
        {
            public Guid IdMaterial { get; set; }
            public string Titulo { get; set; }
            public string Autor { get; set; }
            public string Tipo { get; set; }
            public string Genero { get; set; }
            public int CantidadDisponible { get; set; }
        }

        public registrarPrestamo()
        {
            InitializeComponent();
        }

        public registrarPrestamo(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _materialBLL = new MaterialBLL();
            _alumnoBLL = new AlumnoBLL();
            _prestamoBLL = new PrestamoBLL();
            _inscripcionBLL = new InscripcionBLL();
            _ejemplarBLL = new EjemplarBLL();
            _bitacoraBLL = new BitacoraBibliotecarioBLL();
            _materialesFiltrados = new List<MaterialDetalle>();
            _alumnosGrado = new List<Alumno>();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += RegistrarPrestamo_Load;
            btnConfirmarPrestamo.Click += BtnConfirmarPrestamo_Click;
            cmbGradoDivision.SelectedIndexChanged += CmbGradoDivision_SelectedIndexChanged;
            cmbAlumno.SelectedIndexChanged += CmbAlumno_SelectedIndexChanged;
            cmbFiltrarPor.SelectedIndexChanged += CmbFiltrarPor_SelectedIndexChanged;
            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            // Configurar Timer para búsqueda con delay
            _searchTimer = new Timer();
            _searchTimer.Interval = SEARCH_DELAY;
            _searchTimer.Tick += SearchTimer_Tick;
            dgvMateriales.SelectionChanged += DgvMateriales_SelectionChanged;

            // Configurar DataGridView
            ConfigurarDataGridView();

            // Configurar DateTimePickers
            dtpFechaPrestamo.Value = DateTime.Now;
            dtpFechaDevolucion.MinDate = DateTime.Now.AddDays(1);
            dtpFechaDevolucion.Value = DateTime.Now.AddDays(7); // Por defecto 7 días
        }

        private void ConfigurarDataGridView()
        {
            dgvMateriales.AutoGenerateColumns = false;
            dgvMateriales.Columns.Clear();

            // Columna Título
            dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Titulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                Width = 150
            });

            // Columna Autor
            dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Autor",
                HeaderText = "Autor",
                DataPropertyName = "Autor",
                Width = 100
            });

            // Columna Tipo
            dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tipo",
                HeaderText = "Tipo",
                DataPropertyName = "Tipo",
                Width = 70
            });

            // Columna Género
            dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Genero",
                HeaderText = "Género",
                DataPropertyName = "Genero",
                Width = 90
            });

            // Columna Disponibles
            dgvMateriales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CantidadDisponible",
                HeaderText = "Disp.",
                DataPropertyName = "CantidadDisponible",
                Width = 60
            });

            // Estilo del DataGridView
            dgvMateriales.EnableHeadersVisualStyles = false;
            dgvMateriales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvMateriales.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvMateriales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvMateriales.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
            dgvMateriales.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            dgvMateriales.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
        }

        private void RegistrarPrestamo_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarGrados();
            CargarOpcionesFiltro();
            LimpiarCampos();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("registrar_prestamo");
                lblGradoDivision.Text = LanguageManager.Translate("grado_division");
                lblAlumno.Text = LanguageManager.Translate("alumno");
                lblMaterial.Text = LanguageManager.Translate("material");
                lblFiltrarPor.Text = LanguageManager.Translate("filtrar_por");
                lblBuscar.Text = LanguageManager.Translate("buscar");
                lblSeleccionarMaterial.Text = LanguageManager.Translate("seleccionar_material");
                lblFechaPrestamo.Text = LanguageManager.Translate("fecha_prestamo");
                lblFechaDevolucion.Text = LanguageManager.Translate("fecha_devolucion");
                btnConfirmarPrestamo.Text = LanguageManager.Translate("confirmar_prestamo");
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
                cmbAlumno.Items.Add("-- Elegi un grado para cargar --");
                cmbAlumno.SelectedIndex = 0;
            }
        }

        private void CmbAlumno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidarEstadoAlumno();
        }

        private void ValidarEstadoAlumno()
        {
            // Limpiar advertencias previas
            lblUbicacion.Text = "";
            lblUbicacion.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            if (cmbAlumno.SelectedIndex <= 0 || cmbAlumno.SelectedValue == null)
                return;

            try
            {
                Guid idAlumno = (Guid)cmbAlumno.SelectedValue;
                var todosPrestamos = _prestamoBLL.ObtenerPorAlumno(idAlumno);
                var prestamosActivos = todosPrestamos.Where(p => p.Estado == "Activo" || p.Estado == "Atrasado").ToList();

                int cantidadActivos = prestamosActivos.Count;
                int cantidadVencidos = prestamosActivos.Count(p => p.FechaDevolucionPrevista < DateTime.Now);

                if (cantidadVencidos > 0)
                {
                    lblUbicacion.Text = $"⚠ Alumno tiene {cantidadVencidos} préstamo(s) vencido(s)";
                    lblUbicacion.ForeColor = System.Drawing.Color.Red;
                }
                else if (cantidadActivos >= 5)
                {
                    lblUbicacion.Text = $"⚠ Alumno alcanzó el límite de préstamos ({cantidadActivos}/5)";
                    lblUbicacion.ForeColor = System.Drawing.Color.Orange;
                }
                else if (cantidadActivos > 0)
                {
                    lblUbicacion.Text = $"ℹ Préstamos activos: {cantidadActivos}/5";
                    lblUbicacion.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
                }
                else
                {
                    lblUbicacion.Text = "✓ Sin préstamos activos";
                    lblUbicacion.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al validar estado del alumno: {ex.Message}");
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
                cmbAlumno.DisplayMember = "NombreCompleto";
                cmbAlumno.ValueMember = "IdAlumno";
                cmbAlumno.DataSource = _alumnosGrado;

                if (_alumnosGrado.Count == 0)
                {
                    cmbAlumno.DataSource = null;
                    cmbAlumno.Items.Clear();
                    cmbAlumno.Items.Add("-- No hay alumnos en este grado --");
                    cmbAlumno.SelectedIndex = 0;
                }
                else
                {
                    cmbAlumno.SelectedIndex = -1; // No seleccionar ninguno por defecto
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarOpcionesFiltro()
        {
            cmbFiltrarPor.Items.Clear();
            cmbFiltrarPor.Items.Add(LanguageManager.Translate("titulo"));
            cmbFiltrarPor.Items.Add(LanguageManager.Translate("autor"));
            cmbFiltrarPor.Items.Add(LanguageManager.Translate("tipo"));
            cmbFiltrarPor.Items.Add(LanguageManager.Translate("genero"));
            cmbFiltrarPor.SelectedIndex = 0;
        }

        private void CmbFiltrarPor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpiar búsqueda y recargar
            txtBuscar.Clear();
            CargarMateriales();
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Detener el timer si está corriendo
            _searchTimer.Stop();
            // Iniciar el timer nuevamente
            _searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            // Detener el timer
            _searchTimer.Stop();
            // Ejecutar la búsqueda
            CargarMateriales();
        }

        private void CargarMateriales()
        {
            try
            {
                // Desactivar temporalmente la selección automática
                _permitirSeleccion = false;

                var todosMateriales = _materialBLL.ObtenerTodosMateriales();

                // Filtrar solo materiales con ejemplares disponibles y obtener ubicación
                _materialesFiltrados = new List<MaterialDetalle>();

                foreach (var material in todosMateriales.Where(m => m.CantidadDisponible > 0))
                {
                    _materialesFiltrados.Add(new MaterialDetalle
                    {
                        IdMaterial = material.IdMaterial,
                        Titulo = material.Titulo,
                        Autor = material.Autor,
                        Tipo = material.Tipo.ToString(),
                        Genero = material.Genero,
                        CantidadDisponible = material.CantidadDisponible
                    });
                }

                // Aplicar filtro de búsqueda
                if (!string.IsNullOrWhiteSpace(txtBuscar.Text) && cmbFiltrarPor.SelectedIndex >= 0)
                {
                    string busqueda = txtBuscar.Text.ToLower();
                    int filtroSeleccionado = cmbFiltrarPor.SelectedIndex;

                    _materialesFiltrados = _materialesFiltrados.Where(m =>
                    {
                        switch (filtroSeleccionado)
                        {
                            case 0: // Título
                                return m.Titulo != null && m.Titulo.ToLower().Contains(busqueda);
                            case 1: // Autor
                                return m.Autor != null && m.Autor.ToLower().Contains(busqueda);
                            case 2: // Tipo
                                return m.Tipo.ToLower().Contains(busqueda);
                            case 3: // Género
                                return m.Genero != null && m.Genero.ToLower().Contains(busqueda);
                            default:
                                return true;
                        }
                    }).ToList();
                }

                // Cargar en el DataGridView
                dgvMateriales.DataSource = null;
                dgvMateriales.DataSource = _materialesFiltrados.OrderBy(m => m.Titulo).ToList();

                // Reactivar la selección después de un breve delay
                System.Threading.Tasks.Task.Delay(100).ContinueWith(t =>
                {
                    if (!this.IsDisposed)
                    {
                        this.Invoke((MethodInvoker)(() => _permitirSeleccion = true));
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materiales: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMateriales_SelectionChanged(object sender, EventArgs e)
        {
            // Solo abrir diálogo si la selección está permitida (usuario hace clic manualmente)
            if (!_permitirSeleccion)
                return;

            if (dgvMateriales.SelectedRows.Count > 0)
            {
                var materialDetalle = (MaterialDetalle)dgvMateriales.SelectedRows[0].DataBoundItem;

                if (materialDetalle != null)
                {
                    // Abrir diálogo para seleccionar ejemplar
                    AbrirDialogoSeleccionEjemplar(materialDetalle);
                }
            }
            else
            {
                _ejemplarSeleccionado = null;
                lblUbicacion.Text = "";
            }
        }

        private void AbrirDialogoSeleccionEjemplar(MaterialDetalle materialDetalle)
        {
            try
            {
                // Obtener el material completo
                var material = _materialBLL.ObtenerMaterialPorId(materialDetalle.IdMaterial);

                if (material == null)
                {
                    lblUbicacion.Text = "Error: Material no encontrado";
                    return;
                }

                // Abrir diálogo de selección de ejemplar
                using (var dialogo = new SeleccionarEjemplar(material))
                {
                    if (dialogo.ShowDialog() == DialogResult.OK)
                    {
                        _ejemplarSeleccionado = dialogo.EjemplarSeleccionado;

                        // Mostrar información del ejemplar seleccionado
                        if (_ejemplarSeleccionado != null)
                        {
                            lblUbicacion.Text = $"📦 Buscar en: {_ejemplarSeleccionado.Ubicacion} | Código: {_ejemplarSeleccionado.CodigoEjemplar}";
                        }
                    }
                    else
                    {
                        // Usuario canceló - deseleccionar el material
                        dgvMateriales.ClearSelection();
                        _ejemplarSeleccionado = null;
                        lblUbicacion.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar ejemplar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ejemplarSeleccionado = null;
                lblUbicacion.Text = "";
            }
        }

        private void BtnConfirmarPrestamo_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (cmbGradoDivision.SelectedIndex <= 0)
                {
                    MessageBox.Show("Debe seleccionar un grado", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbAlumno.SelectedIndex < 0 || cmbAlumno.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un alumno", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvMateriales.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un material", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar que se haya seleccionado un ejemplar específico
                if (_ejemplarSeleccionado == null)
                {
                    MessageBox.Show("Debe seleccionar un ejemplar específico del material", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtpFechaDevolucion.Value <= dtpFechaPrestamo.Value)
                {
                    MessageBox.Show("La fecha de devolución debe ser posterior a la fecha de préstamo",
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener alumno y material seleccionados
                var alumno = (Alumno)cmbAlumno.SelectedItem;
                var materialDetalle = (MaterialDetalle)dgvMateriales.SelectedRows[0].DataBoundItem;

                // Crear préstamo con el ejemplar específico seleccionado
                Prestamo prestamo = new Prestamo
                {
                    IdMaterial = materialDetalle.IdMaterial,
                    IdAlumno = alumno.IdAlumno,
                    IdUsuario = _usuarioLogueado.IdUsuario,
                    IdEjemplar = _ejemplarSeleccionado.IdEjemplar,
                    FechaPrestamo = dtpFechaPrestamo.Value,
                    FechaDevolucionPrevista = dtpFechaDevolucion.Value,
                    Estado = "Activo"
                };

                _prestamoBLL.RegistrarPrestamo(prestamo);

                // Registrar en bitácora
                _bitacoraBLL.RegistrarOperacion(new BitacoraBibliotecario
                {
                    IdUsuario = _usuarioLogueado.IdUsuario,
                    NombreUsuario = _usuarioLogueado.Nombre,
                    TipoOperacion = "Creacion",
                    Modulo = "Préstamos",
                    Accion = "Registrar préstamo",
                    EntidadAfectada = "Prestamo",
                    IdEntidad = null,
                    Detalle = $"Préstamo registrado: Material '{materialDetalle.Titulo}' a alumno '{alumno.NombreCompleto}'. Ejemplar #{_ejemplarSeleccionado.NumeroEjemplar} ({_ejemplarSeleccionado.CodigoEjemplar}). Devolución prevista: {dtpFechaDevolucion.Value:dd/MM/yyyy}"
                });

                // Construir mensaje de éxito con formato mejorado
                string mensaje = "✓ PRÉSTAMO REGISTRADO EXITOSAMENTE\n\n";
                mensaje += "═══════════════════════════════════════\n\n";
                mensaje += $"📚 Material: {materialDetalle.Titulo}\n";
                mensaje += $"👤 Alumno: {alumno.NombreCompleto}\n\n";
                mensaje += "DATOS DEL EJEMPLAR:\n";
                mensaje += $"  • Ejemplar #{_ejemplarSeleccionado.NumeroEjemplar}\n";
                mensaje += $"  • Código: {_ejemplarSeleccionado.CodigoEjemplar}\n";

                if (!string.IsNullOrEmpty(_ejemplarSeleccionado.Ubicacion))
                {
                    mensaje += $"  • Ubicación: {_ejemplarSeleccionado.Ubicacion}\n";
                }

                mensaje += $"\n📅 Fecha de préstamo: {dtpFechaPrestamo.Value:dd/MM/yyyy}\n";
                mensaje += $"📅 Devolución prevista: {dtpFechaDevolucion.Value:dd/MM/yyyy}\n\n";
                mensaje += "═══════════════════════════════════════\n";
                mensaje += "Por favor, entregar el material al alumno.";

                MessageBox.Show(
                    mensaje,
                    "Préstamo Registrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarCampos();
                CargarMateriales(); // Actualizar disponibilidad
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar préstamo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            // Desactivar selección mientras se limpian los campos
            _permitirSeleccion = false;

            if (cmbGradoDivision.Items.Count > 0)
                cmbGradoDivision.SelectedIndex = 0;

            cmbAlumno.DataSource = null;
            cmbAlumno.Items.Clear();
            cmbAlumno.Items.Add("-- Elegi un grado para cargar --");
            cmbAlumno.SelectedIndex = 0;

            if (cmbFiltrarPor.Items.Count > 0)
                cmbFiltrarPor.SelectedIndex = 0;

            txtBuscar.Clear();
            dgvMateriales.DataSource = null;
            dtpFechaPrestamo.Value = DateTime.Now;
            dtpFechaDevolucion.Value = DateTime.Now.AddDays(7);

            _ejemplarSeleccionado = null;
            lblUbicacion.Text = "";

            CargarMateriales();
            // _permitirSeleccion se activará automáticamente en CargarMateriales()
        }
    }
}
