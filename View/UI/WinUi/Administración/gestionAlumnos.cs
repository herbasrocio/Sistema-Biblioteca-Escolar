using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using BLL;
using DomainModel;

namespace UI.WinUi.Administrador
{
    public partial class gestionAlumnos : Form
    {
        private Usuario _usuarioLogueado;
        private AlumnoBLL _alumnoBLL;
        private InscripcionBLL _inscripcionBLL;
        private List<Alumno> _alumnosGrado;
        private int _anioLectivoSeleccionado;

        public gestionAlumnos()
        {
            InitializeComponent();
            _alumnoBLL = new AlumnoBLL();
            _inscripcionBLL = new InscripcionBLL();
            _alumnosGrado = new List<Alumno>();
            _anioLectivoSeleccionado = DateTime.Now.Year;
        }

        public gestionAlumnos(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += GestionAlumnos_Load;

            // Configurar menú Importar/Exportar
            ConfigurarMenuImportarExportar();

            // Eventos de botones
            btnEditarGrado.Click += BtnEditarGrado_Click;
            btnNuevoGrado.Click += BtnNuevoGrado_Click;
            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnPromocionAlumnos.Click += BtnPromocionAlumnos_Click;

            // Eventos de búsqueda
            cmbGrado.SelectedIndexChanged += CmbGrado_SelectedIndexChanged;
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
            txtBuscar.Enter += TxtBuscar_Enter;
            txtBuscar.Leave += TxtBuscar_Leave;

            // Configurar DataGridView
            dgvAlumnos.ReadOnly = true;
            dgvAlumnos.AllowUserToAddRows = false;
            dgvAlumnos.AllowUserToDeleteRows = false;
            dgvAlumnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlumnos.MultiSelect = true;
            dgvAlumnos.AutoGenerateColumns = true;

            ConfigurarEstiloDataGridView();

            // Configurar pestaña de historial
            ConfigurarPestañaHistorial();
        }

        private void ConfigurarEstiloDataGridView()
        {
            dgvAlumnos.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvAlumnos.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvAlumnos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 246, 247);
            dgvAlumnos.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvAlumnos.EnableHeadersVisualStyles = false;
            dgvAlumnos.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvAlumnos.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvAlumnos.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvAlumnos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAlumnos.GridColor = System.Drawing.Color.FromArgb(189, 195, 199);
        }

        private void GestionAlumnos_Load(object sender, EventArgs e)
        {
            // AgregarSelectorAnioLectivo(); // Ya no es necesario, los controles están en el Designer
            AplicarTraducciones();
            CargarGrados();
        }

        /// <summary>
        /// Agrega un selector de año lectivo encima del selector de grado
        /// </summary>
        private void AgregarSelectorAnioLectivo()
        {
            // Crear Label para Año Lectivo
            Label lblAnioLectivo = new Label
            {
                Text = "Año Lectivo:",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(44, 62, 80),
                Location = new System.Drawing.Point(20, 92),
                AutoSize = true,
                Name = "lblAnioLectivo"
            };

            // Crear ComboBox para Año Lectivo
            ComboBox cmbAnioLectivo = new ComboBox
            {
                Name = "cmbAnioLectivo",
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                Location = new System.Drawing.Point(120, 89),
                Size = new System.Drawing.Size(110, 23)
            };

            // Cargar años (5 años atrás hasta 1 año adelante)
            int anioActual = DateTime.Now.Year;
            for (int i = anioActual - 5; i <= anioActual + 1; i++)
            {
                cmbAnioLectivo.Items.Add(i);
            }
            cmbAnioLectivo.SelectedItem = anioActual;

            // Evento de cambio de año
            cmbAnioLectivo.SelectedIndexChanged += (s, ev) =>
            {
                _anioLectivoSeleccionado = (int)cmbAnioLectivo.SelectedItem;
                CargarGrados();
            };

            // Botón para crear nuevo año lectivo
            Button btnNuevoAnio = new Button
            {
                Text = "Nuevo Año",
                Location = new System.Drawing.Point(240, 88),
                Size = new System.Drawing.Size(90, 25),
                BackColor = System.Drawing.Color.FromArgb(39, 174, 96),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 8F),
                Cursor = Cursors.Hand
            };
            btnNuevoAnio.FlatAppearance.BorderSize = 0;
            btnNuevoAnio.Click += BtnNuevoAnio_Click;

            // Agregar controles al formulario
            this.Controls.Add(lblAnioLectivo);
            this.Controls.Add(cmbAnioLectivo);
            this.Controls.Add(btnNuevoAnio);

            // Ajustar posición de controles existentes (moverlos hacia abajo)
            lblGrado.Location = new System.Drawing.Point(20, 122);
            cmbGrado.Location = new System.Drawing.Point(120, 119);
        }

        /// <summary>
        /// Evento para crear un nuevo año lectivo
        /// </summary>
        private void BtnNuevoAnio_Click(object sender, EventArgs e)
        {
            try
            {
                using (Form formNuevoAnio = new Form())
                {
                    formNuevoAnio.Text = "Crear Nuevo Año Lectivo";
                    formNuevoAnio.Size = new System.Drawing.Size(350, 150);
                    formNuevoAnio.StartPosition = FormStartPosition.CenterParent;
                    formNuevoAnio.FormBorderStyle = FormBorderStyle.FixedDialog;
                    formNuevoAnio.MaximizeBox = false;
                    formNuevoAnio.MinimizeBox = false;

                    Label lblAnio = new Label
                    {
                        Text = "Año:",
                        Location = new System.Drawing.Point(20, 25),
                        AutoSize = true
                    };

                    NumericUpDown numAnio = new NumericUpDown
                    {
                        Location = new System.Drawing.Point(80, 22),
                        Size = new System.Drawing.Size(230, 23),
                        Minimum = 2020,
                        Maximum = 2050,
                        Value = DateTime.Now.Year + 1
                    };

                    Button btnAceptar = new Button
                    {
                        Text = "Crear",
                        Location = new System.Drawing.Point(140, 70),
                        Size = new System.Drawing.Size(80, 30),
                        DialogResult = DialogResult.OK
                    };

                    Button btnCancelar = new Button
                    {
                        Text = "Cancelar",
                        Location = new System.Drawing.Point(230, 70),
                        Size = new System.Drawing.Size(80, 30),
                        DialogResult = DialogResult.Cancel
                    };

                    formNuevoAnio.Controls.AddRange(new Control[] { lblAnio, numAnio, btnAceptar, btnCancelar });
                    formNuevoAnio.AcceptButton = btnAceptar;
                    formNuevoAnio.CancelButton = btnCancelar;

                    if (formNuevoAnio.ShowDialog() == DialogResult.OK)
                    {
                        int nuevoAnio = (int)numAnio.Value;

                        // Agregar el año al combo si no existe
                        ComboBox cmbAnioLectivo = this.Controls.Find("cmbAnioLectivo", false).FirstOrDefault() as ComboBox;
                        if (cmbAnioLectivo != null && !cmbAnioLectivo.Items.Contains(nuevoAnio))
                        {
                            cmbAnioLectivo.Items.Add(nuevoAnio);
                            cmbAnioLectivo.SelectedItem = nuevoAnio;

                            MessageBox.Show($"Año lectivo {nuevoAnio} creado exitosamente.\n\nAhora puede crear grados y agregar alumnos para este año.",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("El año lectivo ya existe.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear año lectivo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configura el menú contextual para Importar/Exportar
        /// </summary>
        private void ConfigurarMenuImportarExportar()
        {
            menuImportarExportar.Items.Clear();

            // Opción: Importar alumnos
            ToolStripMenuItem menuImportar = new ToolStripMenuItem("📥 Importar alumnos...");
            menuImportar.Click += BtnImportarCSV_Click;
            menuImportarExportar.Items.Add(menuImportar);

            // Opción: Exportar alumnos (CSV)
            ToolStripMenuItem menuExportar = new ToolStripMenuItem("📤 Exportar alumnos (CSV)...");
            menuExportar.Click += BtnExportarCSV_Click;
            menuImportarExportar.Items.Add(menuExportar);

            // Separador
            menuImportarExportar.Items.Add(new ToolStripSeparator());

            // Opción: Descargar plantilla
            ToolStripMenuItem menuPlantilla = new ToolStripMenuItem("📋 Descargar plantilla CSV");
            menuPlantilla.Click += BtnPlantilla_Click;
            menuImportarExportar.Items.Add(menuPlantilla);

            // Configurar el botón para mostrar el menú
            btnImportarExportar.Click += (s, e) =>
            {
                menuImportarExportar.Show(btnImportarExportar, new Point(0, btnImportarExportar.Height));
            };
        }

        /// <summary>
        /// Maneja el evento Enter del TextBox de búsqueda (quita el placeholder)
        /// </summary>
        private void TxtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text == "Buscar por nombre, apellido o DNI...")
            {
                txtBuscar.Text = "";
                txtBuscar.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// Maneja el evento Leave del TextBox de búsqueda (restaura el placeholder)
        /// </summary>
        private void TxtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = "Buscar por nombre, apellido o DNI...";
                txtBuscar.ForeColor = System.Drawing.Color.Gray;
            }
        }

        /// <summary>
        /// Filtra la lista de alumnos en tiempo real según el texto de búsqueda
        /// </summary>
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Ignorar si es el texto placeholder
            if (txtBuscar.Text == "Buscar por nombre, apellido o DNI...")
            {
                dgvAlumnos.DataSource = _alumnosGrado;
                return;
            }

            string filtro = txtBuscar.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                dgvAlumnos.DataSource = _alumnosGrado;
            }
            else
            {
                var alumnosFiltrados = _alumnosGrado.Where(a =>
                    a.Nombre.ToLower().Contains(filtro) ||
                    a.Apellido.ToLower().Contains(filtro) ||
                    a.DNI.Contains(filtro)
                ).ToList();

                dgvAlumnos.DataSource = alumnosFiltrados;
                ActualizarEstadisticas(alumnosFiltrados.Count);
            }

            // Reconfigurar columnas visibles después del filtro
            ConfigurarColumnasVisibles();
        }

        /// <summary>
        /// Actualiza el panel de estadísticas con información del grado actual
        /// </summary>
        private void ActualizarEstadisticas(int? totalFiltrado = null)
        {
            int total = totalFiltrado ?? _alumnosGrado.Count;

            lblEstadisticas.Text = $"📊 Total: {total} alumno(s) - Año {_anioLectivoSeleccionado}";

            // Si hay alumnos, mostrar más detalles
            if (_alumnosGrado.Count > 0)
            {
                if (totalFiltrado.HasValue && totalFiltrado.Value != _alumnosGrado.Count)
                {
                    lblEstadisticas.Text += $"\n(Mostrando {totalFiltrado} de {_alumnosGrado.Count})";
                }
            }
        }

        /// <summary>
        /// Configura qué columnas son visibles en el DataGridView
        /// </summary>
        private void ConfigurarColumnasVisibles()
        {
            if (dgvAlumnos.Columns.Count > 0)
            {
                if (dgvAlumnos.Columns.Contains("IdAlumno"))
                    dgvAlumnos.Columns["IdAlumno"].Visible = false;
                if (dgvAlumnos.Columns.Contains("FechaRegistro"))
                    dgvAlumnos.Columns["FechaRegistro"].Visible = false;
                if (dgvAlumnos.Columns.Contains("Grado"))
                    dgvAlumnos.Columns["Grado"].Visible = false;
                if (dgvAlumnos.Columns.Contains("Division"))
                    dgvAlumnos.Columns["Division"].Visible = false;
                if (dgvAlumnos.Columns.Contains("NombreCompleto"))
                    dgvAlumnos.Columns["NombreCompleto"].Visible = false;

                // Configurar orden de columnas visibles
                if (dgvAlumnos.Columns.Contains("Apellido"))
                {
                    dgvAlumnos.Columns["Apellido"].HeaderText = "Apellido";
                    dgvAlumnos.Columns["Apellido"].DisplayIndex = 0;
                }
                if (dgvAlumnos.Columns.Contains("Nombre"))
                {
                    dgvAlumnos.Columns["Nombre"].HeaderText = "Nombre";
                    dgvAlumnos.Columns["Nombre"].DisplayIndex = 1;
                }
                if (dgvAlumnos.Columns.Contains("DNI"))
                {
                    dgvAlumnos.Columns["DNI"].HeaderText = "DNI";
                    dgvAlumnos.Columns["DNI"].DisplayIndex = 2;
                }
            }
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestionar_alumnos");
                lblGrado.Text = LanguageManager.Translate("grado_division");
                lblBuscar.Text = LanguageManager.Translate("buscar") + ":";
                lblListaAlumnos.Text = LanguageManager.Translate("alumnos");
                btnEditarGrado.Text = "↔ " + LanguageManager.Translate("cambiar_grado");
                btnNuevo.Text = "+ " + LanguageManager.Translate("nuevo");
                btnEditar.Text = "✏ " + LanguageManager.Translate("editar");
                btnEliminar.Text = "🗑 " + LanguageManager.Translate("eliminar");
                btnPromocionAlumnos.Text = "🔄 " + LanguageManager.Translate("promocion");
                groupOperacionesAlumno.Text = LanguageManager.Translate("operaciones_alumno");
                groupOperacionesMasivas.Text = LanguageManager.Translate("operaciones_masivas");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }


        private string FormatearGrado(string grado, string division)
        {
            if (string.IsNullOrEmpty(grado) || string.IsNullOrEmpty(division))
                return string.Empty;

            // Limpiar cualquier carácter extraño del grado
            string gradoLimpio = new string(grado.Where(c => char.IsDigit(c) || char.IsLetter(c)).ToArray());

            int gradoNum;
            if (int.TryParse(gradoLimpio, out gradoNum))
            {
                string sufijo;
                switch (gradoNum)
                {
                    case 1:
                        sufijo = "ro";
                        break;
                    case 2:
                        sufijo = "do";
                        break;
                    case 3:
                        sufijo = "ro";
                        break;
                    case 7:
                        sufijo = "mo";
                        break;
                    default:
                        sufijo = "to";
                        break;
                }
                return $"{gradoNum}{sufijo} {division}";
            }

            return $"{gradoLimpio} {division}";
        }

        /// <summary>
        /// Carga los grados disponibles para el año lectivo seleccionado
        /// usando el sistema de Inscripciones
        /// </summary>
        private void CargarGrados()
        {
            cmbGrado.Items.Clear();

            try
            {
                // Obtener estadísticas de inscripciones para el año lectivo seleccionado
                var estadisticas = _inscripcionBLL.ObtenerEstadisticasPorAnio(_anioLectivoSeleccionado);

                if (estadisticas.Count == 0)
                {
                    // Si no hay inscripciones, agregar algunos grados por defecto para que el usuario pueda empezar
                    cmbGrado.Items.Add("1ro A");
                    cmbGrado.Items.Add("1ro B");
                    cmbGrado.Items.Add("1ro C");
                    cmbGrado.Items.Add("2do A");
                    cmbGrado.Items.Add("2do B");
                    cmbGrado.Items.Add("2do C");
                    cmbGrado.Items.Add("3ro A");
                    cmbGrado.Items.Add("3ro B");
                    cmbGrado.Items.Add("3ro C");
                    cmbGrado.Items.Add("4to A");
                    cmbGrado.Items.Add("4to B");
                    cmbGrado.Items.Add("4to C");
                    cmbGrado.Items.Add("5to A");
                    cmbGrado.Items.Add("5to B");
                    cmbGrado.Items.Add("5to C");
                    cmbGrado.Items.Add("6to A");
                    cmbGrado.Items.Add("6to B");
                    cmbGrado.Items.Add("6to C");
                    cmbGrado.Items.Add("7mo A");
                    cmbGrado.Items.Add("7mo B");
                    cmbGrado.Items.Add("7mo C");
                }
                else
                {
                    // Cargar grados desde las estadísticas de inscripciones
                    foreach (var est in estadisticas)
                    {
                        string gradoFormateado = FormatearGrado(est.Grado, est.Division);
                        cmbGrado.Items.Add(gradoFormateado);
                    }
                }

                if (cmbGrado.Items.Count > 0)
                {
                    cmbGrado.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                string errorDetallado = $"Error al cargar grados:\n\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorDetallado += $"\n\nDetalle: {ex.InnerException.Message}";
                }
                errorDetallado += $"\n\nStack Trace:\n{ex.StackTrace}";

                MessageBox.Show(errorDetallado, "Error Detallado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbGrado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGrado.SelectedIndex >= 0)
            {
                string gradoSeleccionado = cmbGrado.SelectedItem.ToString();
                var partes = gradoSeleccionado.Split(' ');

                if (partes.Length == 2)
                {
                    // Extraer solo el número del grado (ej: "1ro" -> "1", "2do" -> "2")
                    string gradoParte = partes[0];
                    string grado = new string(gradoParte.Where(char.IsDigit).ToArray());
                    string division = partes[1];

                    CargarAlumnosPorGrado(grado, division);
                }
            }
        }

        /// <summary>
        /// Carga los alumnos inscritos en un grado específico para el año lectivo seleccionado
        /// usando el sistema de Inscripciones
        /// </summary>
        private void CargarAlumnosPorGrado(string grado, string division)
        {
            try
            {
                _alumnosGrado.Clear();

                // Obtener inscripciones del grado para el año lectivo seleccionado
                var inscripciones = _inscripcionBLL.ObtenerInscripcionesPorGrado(
                    _anioLectivoSeleccionado, grado, division);

                // Cargar los alumnos correspondientes a esas inscripciones
                foreach (var inscripcion in inscripciones)
                {
                    var alumno = _alumnoBLL.ObtenerAlumnoPorId(inscripcion.IdAlumno);
                    if (alumno != null)
                    {
                        _alumnosGrado.Add(alumno);
                    }
                }

                dgvAlumnos.DataSource = null;
                dgvAlumnos.DataSource = _alumnosGrado;

                // Configurar columnas visibles
                if (dgvAlumnos.Columns.Count > 0)
                {
                    dgvAlumnos.Columns["IdAlumno"].Visible = false;
                    dgvAlumnos.Columns["FechaRegistro"].Visible = false;
                    dgvAlumnos.Columns["Grado"].Visible = false;
                    dgvAlumnos.Columns["Division"].Visible = false;

                    dgvAlumnos.Columns["Apellido"].HeaderText = "Apellido";
                    dgvAlumnos.Columns["Apellido"].DisplayIndex = 0;
                    dgvAlumnos.Columns["Nombre"].HeaderText = "Nombre";
                    dgvAlumnos.Columns["Nombre"].DisplayIndex = 1;
                    dgvAlumnos.Columns["DNI"].HeaderText = "DNI";
                    dgvAlumnos.Columns["DNI"].DisplayIndex = 2;

                    // Mostrar GradoCompleto
                    if (dgvAlumnos.Columns.Contains("GradoCompleto"))
                    {
                        dgvAlumnos.Columns["GradoCompleto"].HeaderText = "Grado";
                        dgvAlumnos.Columns["GradoCompleto"].DisplayIndex = 3;
                        dgvAlumnos.Columns["GradoCompleto"].Visible = true;
                    }

                    // Ocultar NombreCompleto si existe (es una propiedad calculada)
                    if (dgvAlumnos.Columns.Contains("NombreCompleto"))
                        dgvAlumnos.Columns["NombreCompleto"].Visible = false;
                }

                // Actualizar estadísticas
                ActualizarEstadisticas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImportarCSV_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Todos los archivos compatibles (*.csv;*.xlsx;*.xls)|*.csv;*.xlsx;*.xls|Archivos CSV (*.csv)|*.csv|Archivos Excel (*.xlsx;*.xls)|*.xlsx;*.xls";
                    ofd.FilterIndex = 1;
                    ofd.Title = "Seleccionar archivo de alumnos (CSV o Excel)";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string extension = Path.GetExtension(ofd.FileName).ToLower();

                        if (extension == ".xlsx" || extension == ".xls")
                        {
                            ImportarAlumnosDesdeExcel(ofd.FileName);
                        }
                        else if (extension == ".csv")
                        {
                            ImportarAlumnosDesdeCSV(ofd.FileName);
                        }
                        else
                        {
                            MessageBox.Show("Formato de archivo no soportado. Use Excel (.xlsx, .xls) o CSV (.csv)",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al importar archivo: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportarAlumnosDesdeCSV(string rutaArchivo)
        {
            try
            {
                int alumnosImportados = 0;
                int errores = 0;
                List<string> mensajesError = new List<string>();

                string[] lineas = File.ReadAllLines(rutaArchivo);

                // Saltar encabezado si existe
                int inicioLinea = lineas.Length > 0 && lineas[0].ToLower().Contains("nombre") ? 1 : 0;

                for (int i = inicioLinea; i < lineas.Length; i++)
                {
                    string linea = lineas[i].Trim();
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    // Detectar separador (punto y coma o coma)
                    char separador = linea.Contains(';') ? ';' : ',';
                    string[] campos = linea.Split(separador);

                    if (campos.Length < 3)
                    {
                        errores++;
                        mensajesError.Add($"Línea {i + 1}: Formato inválido (mínimo: Nombre, Apellido, DNI)");
                        continue;
                    }

                    try
                    {
                        Alumno alumno = new Alumno
                        {
                            IdAlumno = Guid.NewGuid(),
                            Nombre = campos[0].Trim(),
                            Apellido = campos[1].Trim(),
                            DNI = campos[2].Trim(),
                            Grado = campos.Length > 3 ? campos[3].Trim() : "",
                            Division = campos.Length > 4 ? campos[4].Trim().ToUpper() : "A"
                        };

                        if (string.IsNullOrWhiteSpace(alumno.Nombre) || string.IsNullOrWhiteSpace(alumno.Apellido))
                        {
                            errores++;
                            mensajesError.Add($"Línea {i + 1}: Nombre y Apellido son obligatorios");
                            continue;
                        }

                        alumnosImportados++;
                    }
                    catch (Exception ex)
                    {
                        errores++;
                        mensajesError.Add($"Línea {i + 1}: {ex.Message}");
                    }
                }

                if (alumnosImportados == 0)
                {
                    MessageBox.Show("No se encontraron alumnos válidos para importar.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Mostrar vista previa
                List<Alumno> alumnosParaImportar = new List<Alumno>();
                for (int i = inicioLinea; i < lineas.Length; i++)
                {
                    string linea = lineas[i].Trim();
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    // Detectar separador (punto y coma o coma)
                    char separador = linea.Contains(';') ? ';' : ',';
                    string[] campos = linea.Split(separador);
                    if (campos.Length >= 3)
                    {
                        Alumno alumno = new Alumno
                        {
                            IdAlumno = Guid.NewGuid(),
                            Nombre = campos[0].Trim(),
                            Apellido = campos[1].Trim(),
                            DNI = campos[2].Trim(),
                            Grado = campos.Length > 3 ? campos[3].Trim() : "",
                            Division = campos.Length > 4 ? campos[4].Trim().ToUpper() : "A"
                        };

                        if (!string.IsNullOrWhiteSpace(alumno.Nombre) && !string.IsNullOrWhiteSpace(alumno.Apellido))
                        {
                            alumnosParaImportar.Add(alumno);
                        }
                    }
                }

                MostrarVistaPreviaImportacion(alumnosParaImportar, mensajesError);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar el archivo CSV: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportarAlumnosDesdeExcel(string rutaArchivo)
        {
            try
            {
                List<Alumno> alumnosParaImportar = new List<Alumno>();
                List<string> mensajesError = new List<string>();

                string extension = Path.GetExtension(rutaArchivo).ToLower();
                string connectionString = "";

                // Determinar el string de conexión según la extensión
                if (extension == ".xlsx")
                {
                    connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={rutaArchivo};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
                }
                else if (extension == ".xls")
                {
                    connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={rutaArchivo};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                }

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Obtener la primera hoja
                        DataTable dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dtSchema == null || dtSchema.Rows.Count == 0)
                        {
                            MessageBox.Show("El archivo Excel no contiene hojas de trabajo", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string sheetName = dtSchema.Rows[0]["TABLE_NAME"].ToString();

                        // Leer datos de la hoja
                        string query = $"SELECT * FROM [{sheetName}]";
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Procesar cada fila
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow row = dt.Rows[i];

                                // Verificar que la fila no esté vacía
                                if (row.ItemArray.All(field => field == DBNull.Value || string.IsNullOrWhiteSpace(field?.ToString())))
                                    continue;

                                try
                                {
                                    Alumno alumno = new Alumno
                                    {
                                        IdAlumno = Guid.NewGuid(),
                                        Nombre = row["Nombre"]?.ToString().Trim() ?? "",
                                        Apellido = row["Apellido"]?.ToString().Trim() ?? "",
                                        DNI = row["DNI"]?.ToString().Trim() ?? "",
                                        Grado = row.Table.Columns.Contains("Grado") ? row["Grado"]?.ToString().Trim() ?? "" : "",
                                        Division = row.Table.Columns.Contains("División") ? row["División"]?.ToString().Trim().ToUpper() ?? "A" :
                                                   row.Table.Columns.Contains("Division") ? row["Division"]?.ToString().Trim().ToUpper() ?? "A" : "A"
                                    };

                                    if (string.IsNullOrWhiteSpace(alumno.Nombre) || string.IsNullOrWhiteSpace(alumno.Apellido))
                                    {
                                        mensajesError.Add($"Fila {i + 2}: Nombre y Apellido son obligatorios");
                                        continue;
                                    }

                                    alumnosParaImportar.Add(alumno);
                                }
                                catch (Exception ex)
                                {
                                    mensajesError.Add($"Fila {i + 2}: {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al conectar con el archivo Excel: {ex.Message}\n\n" +
                            "Asegúrese de tener instalado Microsoft Access Database Engine:\n" +
                            "- Para archivos .xlsx: Access Database Engine 2010 o superior\n" +
                            "- Para archivos .xls: Jet 4.0 (incluido en Windows)",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (alumnosParaImportar.Count == 0)
                {
                    MessageBox.Show("No se encontraron alumnos válidos para importar.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Mostrar vista previa
                MostrarVistaPreviaImportacion(alumnosParaImportar, mensajesError);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar el archivo Excel: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportarCSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (_alumnosGrado == null || _alumnosGrado.Count == 0)
                {
                    MessageBox.Show("No hay alumnos para exportar en el grado seleccionado", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    string gradoSeleccionado = cmbGrado.SelectedItem?.ToString().Replace("°", "").Replace(" ", "_") ?? "alumnos";
                    sfd.Filter = "Archivos CSV (*.csv)|*.csv";
                    sfd.FileName = $"Alumnos_{gradoSeleccionado}.csv";
                    sfd.Title = "Guardar archivo CSV";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ExportarAlumnosACSV(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ExportarAlumnosACSV(string rutaArchivo)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(rutaArchivo, false, System.Text.Encoding.UTF8))
                {
                    // BOM para UTF-8 (ayuda a Excel a detectar el encoding correcto)
                    sw.Write('\uFEFF');

                    // Escribir encabezado usando punto y coma como separador
                    // Excel en configuración regional de español abre automáticamente los CSV con ; en columnas
                    sw.WriteLine("Apellido;Nombre;DNI;Grado;División");

                    // Escribir datos
                    foreach (var alumno in _alumnosGrado)
                    {
                        sw.WriteLine($"{alumno.Apellido};{alumno.Nombre};{alumno.DNI};{alumno.Grado};{alumno.Division}");
                    }
                }

                MessageBox.Show($"Archivo CSV exportado exitosamente:\n{rutaArchivo}\n\n" +
                    $"Total de alumnos: {_alumnosGrado.Count}\n\n" +
                    $"✓ El archivo se abrirá automáticamente en Excel con columnas separadas.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir el archivo CSV: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cambia el grado de alumnos seleccionados actualizando su inscripción
        /// en el año lectivo seleccionado
        /// </summary>
        private void BtnEditarGrado_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAlumnos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar al menos un alumno para cambiar su grado", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mostrar diálogo para seleccionar nuevo grado
                using (Form frmGrado = new Form())
                {
                    frmGrado.Text = "Seleccionar Nuevo Grado";
                    frmGrado.Size = new System.Drawing.Size(350, 150);
                    frmGrado.StartPosition = FormStartPosition.CenterParent;
                    frmGrado.FormBorderStyle = FormBorderStyle.FixedDialog;
                    frmGrado.MaximizeBox = false;
                    frmGrado.MinimizeBox = false;

                    Label lblNuevoGrado = new Label
                    {
                        Text = "Nuevo Grado/División:",
                        Location = new System.Drawing.Point(20, 20),
                        AutoSize = true
                    };

                    ComboBox cmbNuevoGrado = new ComboBox
                    {
                        Location = new System.Drawing.Point(20, 45),
                        Size = new System.Drawing.Size(290, 23),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };

                    // Cargar grados en el combo
                    for (int grado = 1; grado <= 7; grado++)
                    {
                        foreach (string division in new[] { "A", "B", "C" })
                        {
                            cmbNuevoGrado.Items.Add($"{grado}° {division}");
                        }
                    }
                    cmbNuevoGrado.SelectedIndex = 0;

                    Button btnAceptar = new Button
                    {
                        Text = "Aceptar",
                        Location = new System.Drawing.Point(150, 80),
                        Size = new System.Drawing.Size(75, 30),
                        DialogResult = DialogResult.OK
                    };

                    Button btnCancelar = new Button
                    {
                        Text = "Cancelar",
                        Location = new System.Drawing.Point(235, 80),
                        Size = new System.Drawing.Size(75, 30),
                        DialogResult = DialogResult.Cancel
                    };

                    frmGrado.Controls.Add(lblNuevoGrado);
                    frmGrado.Controls.Add(cmbNuevoGrado);
                    frmGrado.Controls.Add(btnAceptar);
                    frmGrado.Controls.Add(btnCancelar);
                    frmGrado.AcceptButton = btnAceptar;
                    frmGrado.CancelButton = btnCancelar;

                    if (frmGrado.ShowDialog() == DialogResult.OK)
                    {
                        string nuevoGradoSeleccionado = cmbNuevoGrado.SelectedItem.ToString();
                        var partes = nuevoGradoSeleccionado.Split(' ');

                        if (partes.Length == 2)
                        {
                            string nuevoGrado = partes[0].Replace("°", "");
                            string nuevaDivision = partes[1];

                            int actualizados = 0;
                            int errores = 0;
                            List<string> mensajesError = new List<string>();

                            foreach (DataGridViewRow row in dgvAlumnos.SelectedRows)
                            {
                                Alumno alumno = (Alumno)row.DataBoundItem;

                                try
                                {
                                    // Obtener la inscripción activa del alumno para el año seleccionado
                                    var inscripcionActual = _inscripcionBLL.ObtenerInscripcionActiva(
                                        alumno.IdAlumno, _anioLectivoSeleccionado);

                                    if (inscripcionActual != null)
                                    {
                                        // Actualizar la inscripción con el nuevo grado/división
                                        inscripcionActual.Grado = nuevoGrado;
                                        inscripcionActual.Division = nuevaDivision;
                                        _inscripcionBLL.ActualizarInscripcion(inscripcionActual);

                                        // También actualizar el alumno para mantener compatibilidad
                                        alumno.Grado = nuevoGrado;
                                        alumno.Division = nuevaDivision;
                                        _alumnoBLL.ActualizarAlumno(alumno);

                                        actualizados++;
                                    }
                                    else
                                    {
                                        // Si no tiene inscripción activa, crear una nueva
                                        _inscripcionBLL.InscribirAlumno(
                                            alumno.IdAlumno,
                                            _anioLectivoSeleccionado,
                                            nuevoGrado,
                                            nuevaDivision);

                                        alumno.Grado = nuevoGrado;
                                        alumno.Division = nuevaDivision;
                                        _alumnoBLL.ActualizarAlumno(alumno);

                                        actualizados++;
                                    }
                                }
                                catch (Exception exAlumno)
                                {
                                    errores++;
                                    mensajesError.Add($"{alumno.NombreCompleto}: {exAlumno.Message}");
                                }
                            }

                            string mensaje = $"Se actualizaron {actualizados} alumno(s) al grado {nuevoGradoSeleccionado}";
                            if (errores > 0)
                            {
                                mensaje += $"\n\nErrores: {errores}\n" + string.Join("\n", mensajesError);
                            }

                            MessageBox.Show(mensaje,
                                errores > 0 ? "Advertencia" : "Éxito",
                                MessageBoxButtons.OK,
                                errores > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                            // Recargar la lista
                            CargarGrados(); // Recargar grados por si el nuevo grado no existía
                            CmbGrado_SelectedIndexChanged(null, EventArgs.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar grado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Crea un nuevo alumno y automáticamente lo inscribe en el año lectivo seleccionado
        /// </summary>
        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                editarAlumno formEditar = new editarAlumno();

                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    // Validar DNI duplicado
                    var existente = _alumnoBLL.ObtenerAlumnoPorDNI(formEditar.AlumnoEditado.DNI);
                    if (existente != null)
                    {
                        MessageBox.Show($"Ya existe un alumno con el DNI {formEditar.AlumnoEditado.DNI}",
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Guardar el alumno
                    _alumnoBLL.GuardarAlumno(formEditar.AlumnoEditado);

                    // Crear inscripción automáticamente para el año lectivo seleccionado
                    try
                    {
                        _inscripcionBLL.InscribirAlumno(
                            formEditar.AlumnoEditado.IdAlumno,
                            _anioLectivoSeleccionado,
                            formEditar.AlumnoEditado.Grado,
                            formEditar.AlumnoEditado.Division);

                        MessageBox.Show($"Alumno creado e inscrito exitosamente en el año {_anioLectivoSeleccionado}", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception exInscripcion)
                    {
                        // Si falla la inscripción, informar pero el alumno ya fue creado
                        MessageBox.Show($"Alumno creado, pero ocurrió un error al inscribirlo:\n{exInscripcion.Message}\n\nPuede inscribirlo manualmente.",
                            "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Recargar lista
                    CargarGrados(); // Recargar grados por si es necesario
                    CmbGrado_SelectedIndexChanged(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear alumno: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAlumnos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un alumno para editar", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Alumno alumnoSeleccionado = (Alumno)dgvAlumnos.SelectedRows[0].DataBoundItem;

                editarAlumno formEditar = new editarAlumno(alumnoSeleccionado);

                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    _alumnoBLL.ActualizarAlumno(formEditar.AlumnoEditado);

                    MessageBox.Show("Alumno actualizado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargar lista
                    CmbGrado_SelectedIndexChanged(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar alumno: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAlumnos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un alumno para eliminar", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Alumno alumnoSeleccionado = (Alumno)dgvAlumnos.SelectedRows[0].DataBoundItem;

                var resultado = MessageBox.Show(
                    $"¿Está seguro de eliminar al alumno {alumnoSeleccionado.NombreCompleto}?\n\n" +
                    "Esta acción dará de baja al alumno (no se eliminará físicamente)",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    _alumnoBLL.EliminarAlumno(alumnoSeleccionado);

                    MessageBox.Show("Alumno eliminado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargar lista
                    CmbGrado_SelectedIndexChanged(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar alumno: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Abre la ventana de Promoción de Alumnos para promocionar alumnos
        /// de un año lectivo al siguiente
        /// </summary>
        private void BtnPromocionAlumnos_Click(object sender, EventArgs e)
        {
            try
            {
                gestionPromocionAlumnos formPromocion = new gestionPromocionAlumnos(_usuarioLogueado);
                formPromocion.ShowDialog();

                // Recargar grados y alumnos por si hubo cambios
                CargarGrados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir ventana de promoción: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevoGrado_Click(object sender, EventArgs e)
        {
            try
            {
                // Solicitar grado y división
                using (Form formNuevoGrado = new Form())
                {
                    formNuevoGrado.Text = "Crear Nuevo Grado";
                    formNuevoGrado.Size = new System.Drawing.Size(400, 220);
                    formNuevoGrado.FormBorderStyle = FormBorderStyle.FixedDialog;
                    formNuevoGrado.StartPosition = FormStartPosition.CenterParent;
                    formNuevoGrado.MaximizeBox = false;
                    formNuevoGrado.MinimizeBox = false;

                    Label lblGrado = new Label() { Left = 20, Top = 20, Text = "Grado:", AutoSize = true };
                    ComboBox cmbGradoNuevo = new ComboBox() { Left = 100, Top = 17, Width = 250 };
                    cmbGradoNuevo.Items.Add("Jardín");
                    cmbGradoNuevo.Items.Add("Sala 3");
                    cmbGradoNuevo.Items.Add("Sala 4");
                    cmbGradoNuevo.Items.Add("Sala 5");
                    for (int i = 1; i <= 7; i++)
                    {
                        cmbGradoNuevo.Items.Add(i.ToString());
                    }

                    Label lblDivision = new Label() { Left = 20, Top = 55, Text = "Divisiones:", AutoSize = true };
                    CheckedListBox chkDivisiones = new CheckedListBox()
                    {
                        Left = 100,
                        Top = 52,
                        Width = 250,
                        Height = 80,
                        CheckOnClick = true
                    };
                    chkDivisiones.Items.AddRange(new[] { "A", "B", "C", "D", "E" });

                    Button btnAceptar = new Button() { Text = "Crear", Left = 150, Width = 100, Top = 145, DialogResult = DialogResult.OK };
                    btnAceptar.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
                    btnAceptar.ForeColor = System.Drawing.Color.White;
                    btnAceptar.FlatStyle = FlatStyle.Flat;
                    btnAceptar.FlatAppearance.BorderSize = 0;

                    Button btnCancelar = new Button() { Text = "Cancelar", Left = 260, Width = 100, Top = 145, DialogResult = DialogResult.Cancel };
                    btnCancelar.BackColor = System.Drawing.Color.FromArgb(127, 140, 141);
                    btnCancelar.ForeColor = System.Drawing.Color.White;
                    btnCancelar.FlatStyle = FlatStyle.Flat;
                    btnCancelar.FlatAppearance.BorderSize = 0;

                    formNuevoGrado.Controls.Add(lblGrado);
                    formNuevoGrado.Controls.Add(cmbGradoNuevo);
                    formNuevoGrado.Controls.Add(lblDivision);
                    formNuevoGrado.Controls.Add(chkDivisiones);
                    formNuevoGrado.Controls.Add(btnAceptar);
                    formNuevoGrado.Controls.Add(btnCancelar);
                    formNuevoGrado.AcceptButton = btnAceptar;
                    formNuevoGrado.CancelButton = btnCancelar;

                    if (formNuevoGrado.ShowDialog() == DialogResult.OK)
                    {
                        if (string.IsNullOrWhiteSpace(cmbGradoNuevo.Text))
                        {
                            MessageBox.Show("Debe especificar un grado", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (chkDivisiones.CheckedItems.Count == 0)
                        {
                            MessageBox.Show("Debe seleccionar al menos una división", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int creados = 0;
                        List<string> gradosCreados = new List<string>();

                        foreach (var division in chkDivisiones.CheckedItems)
                        {
                            string nuevoGrado = $"{cmbGradoNuevo.Text.Trim()}° {division.ToString()}";

                            // Verificar si ya existe
                            if (!cmbGrado.Items.Contains(nuevoGrado))
                            {
                                cmbGrado.Items.Add(nuevoGrado);
                                gradosCreados.Add(nuevoGrado);
                                creados++;
                            }
                        }

                        if (creados > 0)
                        {
                            // Seleccionar el primer grado creado
                            cmbGrado.SelectedItem = gradosCreados[0];

                            MessageBox.Show($"Se crearon {creados} grado(s) exitosamente:\n\n" +
                                string.Join("\n", gradosCreados) + "\n\nAhora puede agregar alumnos a estos grados.",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Todos los grados seleccionados ya existen.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear grado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Archivos CSV (*.csv)|*.csv";
                    sfd.FileName = "Plantilla_Alumnos.csv";
                    sfd.Title = "Guardar plantilla";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        CrearPlantillaCSV(sfd.FileName);

                        MessageBox.Show($"Plantilla creada exitosamente:\n{sfd.FileName}\n\n" +
                            "El archivo se abrirá correctamente en Excel con columnas separadas.\n\n" +
                            "Complete los datos de los alumnos y luego impórtelo usando el botón 'Importar alumnos'.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear plantilla: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CrearPlantillaCSV(string rutaArchivo)
        {
            using (StreamWriter sw = new StreamWriter(rutaArchivo, false, System.Text.Encoding.UTF8))
            {
                // Escribir encabezado (usando punto y coma para Excel)
                sw.WriteLine("Nombre;Apellido;DNI;Grado;Division");

                // Escribir filas de ejemplo
                sw.WriteLine("Juan;Pérez;12345678;1;A");
                sw.WriteLine("María;González;23456789;1;A");
                sw.WriteLine("Pedro;Rodríguez;34567890;2;B");
            }
        }


        private void MostrarVistaPreviaImportacion(List<Alumno> alumnosParaImportar, List<string> errores)
        {
            // Crear formulario de vista previa
            Form formPrevia = new Form
            {
                Text = "Vista Previa de Importación",
                Size = new System.Drawing.Size(800, 600),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable,
                MinimizeBox = false,
                MaximizeBox = true
            };

            // DataGridView para mostrar alumnos
            DataGridView dgvPrevia = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(740, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false
            };

            // Configurar columnas
            dgvPrevia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Apellido", HeaderText = "Apellido", Width = 150 });
            dgvPrevia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Nombre", Width = 150 });
            dgvPrevia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DNI", HeaderText = "DNI", Width = 100 });
            dgvPrevia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Grado", HeaderText = "Grado", Width = 80 });
            dgvPrevia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Division", HeaderText = "División", Width = 80 });
            dgvPrevia.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", Width = 150 });

            // Obtener DNIs que ya están en la base de datos
            var todosLosAlumnos = _alumnoBLL.ObtenerTodosAlumnos();
            var dnisExistentes = new HashSet<string>(todosLosAlumnos.Select(a => a.DNI));

            // Verificar duplicados dentro del archivo y contra la BD
            int duplicados = 0;
            var dnisEnArchivo = new HashSet<string>();

            foreach (var alumno in alumnosParaImportar)
            {
                int rowIndex = dgvPrevia.Rows.Add(alumno.Apellido, alumno.Nombre, alumno.DNI, alumno.Grado, alumno.Division);

                // Verificar si el DNI ya existe en la base de datos
                bool duplicadoEnBD = dnisExistentes.Contains(alumno.DNI);

                // Verificar si el DNI ya apareció antes en este archivo
                bool duplicadoEnArchivo = dnisEnArchivo.Contains(alumno.DNI);

                if (duplicadoEnBD || duplicadoEnArchivo)
                {
                    string razon = duplicadoEnBD ? "DNI ya existe en BD" : "DNI duplicado en archivo";
                    dgvPrevia.Rows[rowIndex].Cells["Estado"].Value = razon;
                    dgvPrevia.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 200, 200);
                    duplicados++;
                }
                else
                {
                    dgvPrevia.Rows[rowIndex].Cells["Estado"].Value = "OK";
                    dgvPrevia.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(200, 255, 200);
                }

                // Agregar el DNI al conjunto de DNIs procesados
                dnisEnArchivo.Add(alumno.DNI);
            }

            // Label de información
            Label lblInfo = new Label
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(740, 30),
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Text = $"Total a importar: {alumnosParaImportar.Count} | Duplicados: {duplicados} | Errores: {errores.Count}"
            };

            // Botones
            Button btnImportar = new Button
            {
                Text = "Importar todos",
                Location = new System.Drawing.Point(460, 480),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.FromArgb(39, 174, 96),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnImportar.FlatAppearance.BorderSize = 0;

            Button btnImportarSinDuplicados = new Button
            {
                Text = "Importar sin duplicados",
                Location = new System.Drawing.Point(280, 480),
                Size = new System.Drawing.Size(170, 40),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Retry
            };
            btnImportarSinDuplicados.FlatAppearance.BorderSize = 0;

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(610, 480),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.FromArgb(127, 140, 141),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            formPrevia.Controls.Add(lblInfo);
            formPrevia.Controls.Add(dgvPrevia);
            formPrevia.Controls.Add(btnImportar);
            formPrevia.Controls.Add(btnImportarSinDuplicados);
            formPrevia.Controls.Add(btnCancelar);

            DialogResult resultado = formPrevia.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                // Importar todos
                ProcesarImportacion(alumnosParaImportar, false);
            }
            else if (resultado == DialogResult.Retry)
            {
                // Importar solo sin duplicados
                ProcesarImportacion(alumnosParaImportar, true);
            }
        }

        /// <summary>
        /// Procesa la importación de alumnos y crea automáticamente sus inscripciones
        /// para el año lectivo seleccionado
        /// </summary>
        private void ProcesarImportacion(List<Alumno> alumnos, bool saltarDuplicados)
        {
            int importados = 0;
            int omitidos = 0;
            int errores = 0;
            int inscripciones = 0;
            List<string> mensajesError = new List<string>();

            foreach (var alumno in alumnos)
            {
                try
                {
                    var existente = _alumnoBLL.ObtenerAlumnoPorDNI(alumno.DNI);

                    if (existente != null && saltarDuplicados)
                    {
                        omitidos++;
                        continue;
                    }

                    // Guardar el alumno
                    _alumnoBLL.GuardarAlumno(alumno);
                    importados++;

                    // Crear inscripción automáticamente si el alumno tiene grado asignado
                    if (!string.IsNullOrWhiteSpace(alumno.Grado))
                    {
                        try
                        {
                            _inscripcionBLL.InscribirAlumno(
                                alumno.IdAlumno,
                                _anioLectivoSeleccionado,
                                alumno.Grado,
                                alumno.Division ?? "A");
                            inscripciones++;
                        }
                        catch (Exception exInscripcion)
                        {
                            mensajesError.Add($"{alumno.NombreCompleto}: Error al inscribir - {exInscripcion.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    errores++;
                    mensajesError.Add($"{alumno.NombreCompleto}: {ex.Message}");
                }
            }

            string mensaje = $"Importación completada:\n\n" +
                $"✓ Alumnos importados: {importados}\n" +
                $"✓ Inscripciones creadas: {inscripciones} (año {_anioLectivoSeleccionado})\n" +
                (omitidos > 0 ? $"⊘ Omitidos (duplicados): {omitidos}\n" : "") +
                (errores > 0 ? $"✗ Errores: {errores}\n" : "");

            if (mensajesError.Count > 0)
            {
                mensaje += "\n\nDetalles de errores:\n" + string.Join("\n", mensajesError.Take(5));
                if (mensajesError.Count > 5)
                    mensaje += $"\n... y {mensajesError.Count - 5} errores más";
            }

            MessageBox.Show(mensaje, "Resultado", MessageBoxButtons.OK,
                errores > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            // Recargar grados (por si se agregaron nuevos) y lista
            CargarGrados();
            CmbGrado_SelectedIndexChanged(null, EventArgs.Empty);
        }

        #region Métodos de Pestaña Historial

        /// <summary>
        /// Configura los controles y eventos de la pestaña Historial
        /// </summary>
        private void ConfigurarPestañaHistorial()
        {
            // Configurar combo de años lectivos
            CargarAniosLectivosHistorial();

            // Configurar DataGridView de historial
            dgvHistorial.ReadOnly = true;
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.MultiSelect = false;
            dgvHistorial.AutoGenerateColumns = false;

            // Configurar columnas del DataGridView de historial
            dgvHistorial.Columns.Clear();
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "Apellido", HeaderText = "Apellido", DataPropertyName = "Apellido", Width = 120 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 120 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "DNI", HeaderText = "DNI", DataPropertyName = "DNI", Width = 100 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "Grado", HeaderText = "Grado", Width = 80 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "Division", HeaderText = "División", Width = 80 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", Width = 100 });
            dgvHistorial.Columns.Add(new DataGridViewTextBoxColumn { Name = "FechaInscripcion", HeaderText = "Fecha Inscripción", Width = 130 });

            // Aplicar estilo al dgvHistorial
            ConfigurarEstiloDataGridViewHistorial();

            // Eventos
            cmbAnioLectivoHistorial.SelectedIndexChanged += CmbAnioLectivoHistorial_SelectedIndexChanged;
            cmbGradoHistorial.SelectedIndexChanged += CmbGradoHistorial_SelectedIndexChanged;
            btnLimpiarFiltrosHistorial.Click += BtnLimpiarFiltrosHistorial_Click;
            btnVerTrayectoria.Click += BtnVerTrayectoria_Click;

            // Aplicar traducciones
            AplicarTraduccionesHistorial();
        }

        private void ConfigurarEstiloDataGridViewHistorial()
        {
            dgvHistorial.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvHistorial.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvHistorial.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 246, 247);
            dgvHistorial.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvHistorial.EnableHeadersVisualStyles = false;
            dgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvHistorial.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvHistorial.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistorial.GridColor = System.Drawing.Color.FromArgb(189, 195, 199);
        }

        private void AplicarTraduccionesHistorial()
        {
            try
            {
                tabGestionActual.Text = LanguageManager.Translate("tab_gestion_actual");
                tabHistorial.Text = LanguageManager.Translate("tab_historial");
                lblAnioLectivoHistorial.Text = LanguageManager.Translate("anio_lectivo") + ":";
                lblGradoHistorial.Text = LanguageManager.Translate("grado_division") + ":";
                btnLimpiarFiltrosHistorial.Text = LanguageManager.Translate("limpiar");
                lblListaHistorial.Text = LanguageManager.Translate("historial_alumnos") + ":";
                btnVerTrayectoria.Text = "📋 " + LanguageManager.Translate("ver_trayectoria");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones del historial: {ex.Message}");
            }
        }

        /// <summary>
        /// Carga los años lectivos disponibles en el combo de la pestaña historial
        /// </summary>
        private void CargarAniosLectivosHistorial()
        {
            try
            {
                cmbAnioLectivoHistorial.Items.Clear();

                // Obtener todos los años lectivos con inscripciones
                var anios = _inscripcionBLL.ObtenerAniosLectivosDisponibles();

                if (anios.Count == 0)
                {
                    // Si no hay años, agregar el actual
                    cmbAnioLectivoHistorial.Items.Add(DateTime.Now.Year);
                }
                else
                {
                    foreach (var anio in anios.OrderByDescending(a => a))
                    {
                        cmbAnioLectivoHistorial.Items.Add(anio);
                    }
                }

                if (cmbAnioLectivoHistorial.Items.Count > 0)
                {
                    cmbAnioLectivoHistorial.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar años lectivos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbAnioLectivoHistorial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAnioLectivoHistorial.SelectedItem != null)
            {
                int anioSeleccionado = (int)cmbAnioLectivoHistorial.SelectedItem;
                CargarGradosHistorial(anioSeleccionado);
                CargarHistorialAlumnos(anioSeleccionado, null, null);
            }
        }

        /// <summary>
        /// Carga los grados disponibles para un año lectivo en la pestaña historial
        /// </summary>
        private void CargarGradosHistorial(int anio)
        {
            try
            {
                cmbGradoHistorial.Items.Clear();
                cmbGradoHistorial.Items.Add("Todos");

                var estadisticas = _inscripcionBLL.ObtenerEstadisticasPorAnio(anio);

                foreach (var est in estadisticas)
                {
                    string gradoFormateado = FormatearGrado(est.Grado, est.Division);
                    cmbGradoHistorial.Items.Add(gradoFormateado);
                }

                if (cmbGradoHistorial.Items.Count > 0)
                {
                    cmbGradoHistorial.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar grados: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbGradoHistorial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAnioLectivoHistorial.SelectedItem != null && cmbGradoHistorial.SelectedItem != null)
            {
                int anioSeleccionado = (int)cmbAnioLectivoHistorial.SelectedItem;
                string gradoSeleccionado = cmbGradoHistorial.SelectedItem.ToString();

                if (gradoSeleccionado == "Todos")
                {
                    CargarHistorialAlumnos(anioSeleccionado, null, null);
                }
                else
                {
                    var partes = gradoSeleccionado.Split(' ');
                    if (partes.Length == 2)
                    {
                        string grado = new string(partes[0].Where(char.IsDigit).ToArray());
                        string division = partes[1];
                        CargarHistorialAlumnos(anioSeleccionado, grado, division);
                    }
                }
            }
        }

        /// <summary>
        /// Carga el historial de alumnos inscritos en un año lectivo específico
        /// </summary>
        private void CargarHistorialAlumnos(int anio, string grado = null, string division = null)
        {
            try
            {
                List<Inscripcion> inscripciones;

                if (string.IsNullOrEmpty(grado))
                {
                    // Cargar todas las inscripciones del año
                    inscripciones = _inscripcionBLL.ObtenerInscripcionesPorAnio(anio);
                }
                else
                {
                    // Cargar inscripciones del grado específico
                    inscripciones = _inscripcionBLL.ObtenerInscripcionesPorGrado(anio, grado, division);
                }

                // Crear lista de objetos para mostrar en el grid
                var datosHistorial = new List<dynamic>();

                foreach (var inscripcion in inscripciones)
                {
                    var alumno = _alumnoBLL.ObtenerAlumnoPorId(inscripcion.IdAlumno);
                    if (alumno != null)
                    {
                        datosHistorial.Add(new
                        {
                            IdAlumno = alumno.IdAlumno,
                            Apellido = alumno.Apellido,
                            Nombre = alumno.Nombre,
                            DNI = alumno.DNI,
                            Grado = inscripcion.Grado,
                            Division = inscripcion.Division,
                            Estado = ObtenerTraduccionEstado(inscripcion.Estado),
                            FechaInscripcion = inscripcion.FechaInscripcion.ToString("dd/MM/yyyy")
                        });
                    }
                }

                dgvHistorial.DataSource = datosHistorial;
                ActualizarEstadisticasHistorial(datosHistorial.Count, anio, grado, division);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar historial: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerTraduccionEstado(string estado)
        {
            switch (estado?.ToLower())
            {
                case "activo":
                    return LanguageManager.Translate("estado_activo");
                case "finalizado":
                    return LanguageManager.Translate("estado_finalizado");
                case "abandonado":
                    return LanguageManager.Translate("estado_abandonado");
                default:
                    return estado;
            }
        }

        /// <summary>
        /// Actualiza el panel de estadísticas del historial
        /// </summary>
        private void ActualizarEstadisticasHistorial(int total, int anio, string grado = null, string division = null)
        {
            try
            {
                string texto = $"📊 {LanguageManager.Translate("total_alumnos")}: {total} - {LanguageManager.Translate("anio_lectivo")} {anio}";

                if (!string.IsNullOrEmpty(grado))
                {
                    texto += $" - Grado {FormatearGrado(grado, division)}";
                }

                // Obtener estadísticas por estado
                var inscripciones = string.IsNullOrEmpty(grado)
                    ? _inscripcionBLL.ObtenerInscripcionesPorAnio(anio)
                    : _inscripcionBLL.ObtenerInscripcionesPorGrado(anio, grado, division);

                var porEstado = inscripciones.GroupBy(i => i.Estado).Select(g => new { Estado = g.Key, Cantidad = g.Count() }).ToList();

                texto += "\n";
                foreach (var grupo in porEstado)
                {
                    texto += $"{ObtenerTraduccionEstado(grupo.Estado)}: {grupo.Cantidad}  ";
                }

                lblEstadisticasHistorial.Text = texto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estadísticas: {ex.Message}");
            }
        }

        private void BtnLimpiarFiltrosHistorial_Click(object sender, EventArgs e)
        {
            if (cmbGradoHistorial.Items.Count > 0)
            {
                cmbGradoHistorial.SelectedIndex = 0; // Seleccionar "Todos"
            }
        }

        /// <summary>
        /// Muestra la trayectoria completa de un alumno seleccionado
        /// </summary>
        private void BtnVerTrayectoria_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHistorial.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar un alumno para ver su trayectoria", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el IdAlumno del DataBoundItem (objeto dinámico)
                var filaDatos = dgvHistorial.SelectedRows[0].DataBoundItem;
                Guid idAlumno = (Guid)filaDatos.GetType().GetProperty("IdAlumno").GetValue(filaDatos, null);

                var alumno = _alumnoBLL.ObtenerAlumnoPorId(idAlumno);

                if (alumno == null)
                {
                    MessageBox.Show("No se pudo obtener la información del alumno", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Obtener todas las inscripciones del alumno
                var todasInscripciones = _inscripcionBLL.ObtenerInscripcionesPorAlumno(idAlumno);

                // Crear formulario de trayectoria
                Form frmTrayectoria = new Form
                {
                    Text = $"{LanguageManager.Translate("trayectoria_alumno")}: {alumno.NombreCompleto}",
                    Size = new System.Drawing.Size(700, 500),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.Sizable,
                    MinimizeBox = false,
                    MaximizeBox = true
                };

                // Label de información
                Label lblInfo = new Label
                {
                    Text = $"Alumno: {alumno.NombreCompleto} - DNI: {alumno.DNI}",
                    Location = new System.Drawing.Point(20, 20),
                    Size = new System.Drawing.Size(640, 25),
                    Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold)
                };

                // DataGridView
                DataGridView dgvTrayectoria = new DataGridView
                {
                    Location = new System.Drawing.Point(20, 60),
                    Size = new System.Drawing.Size(640, 350),
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoGenerateColumns = false
                };

                // Configurar columnas
                dgvTrayectoria.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AnioLectivo", HeaderText = "Año Lectivo", Width = 100 });
                dgvTrayectoria.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "GradoCompleto", HeaderText = "Grado/División", Width = 150 });
                dgvTrayectoria.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", HeaderText = "Estado", Width = 120 });
                dgvTrayectoria.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaInscripcion", HeaderText = "Fecha Inscripción", Width = 130 });

                // Configurar estilo
                dgvTrayectoria.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
                dgvTrayectoria.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dgvTrayectoria.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 246, 247);
                dgvTrayectoria.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
                dgvTrayectoria.EnableHeadersVisualStyles = false;
                dgvTrayectoria.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
                dgvTrayectoria.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dgvTrayectoria.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

                // Cargar datos
                var datosTrayectoria = todasInscripciones.OrderByDescending(i => i.AnioLectivo).Select(i => new
                {
                    AnioLectivo = i.AnioLectivo,
                    GradoCompleto = FormatearGrado(i.Grado, i.Division),
                    Estado = ObtenerTraduccionEstado(i.Estado),
                    FechaInscripcion = i.FechaInscripcion.ToString("dd/MM/yyyy")
                }).ToList();

                dgvTrayectoria.DataSource = datosTrayectoria;

                // Botón cerrar
                Button btnCerrar = new Button
                {
                    Text = "Cerrar",
                    Location = new System.Drawing.Point(580, 420),
                    Size = new System.Drawing.Size(80, 30),
                    DialogResult = DialogResult.OK
                };

                frmTrayectoria.Controls.Add(lblInfo);
                frmTrayectoria.Controls.Add(dgvTrayectoria);
                frmTrayectoria.Controls.Add(btnCerrar);

                frmTrayectoria.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar trayectoria: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void lblGradoHistorial_Click(object sender, EventArgs e)
        {

        }
    }
}
