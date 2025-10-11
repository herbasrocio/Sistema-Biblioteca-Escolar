using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
        private List<Alumno> _alumnosGrado;

        public gestionAlumnos()
        {
            InitializeComponent();
            _alumnoBLL = new AlumnoBLL();
            _alumnosGrado = new List<Alumno>();
        }

        public gestionAlumnos(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += GestionAlumnos_Load;
            btnImportarCSV.Click += BtnImportarCSV_Click;
            btnPlantilla.Click += BtnPlantilla_Click;
            btnExportarCSV.Click += BtnExportarCSV_Click;
            btnEditarGrado.Click += BtnEditarGrado_Click;
            btnNuevoGrado.Click += BtnNuevoGrado_Click;
            btnEliminarGrado.Click += BtnEliminarGrado_Click;
            btnNuevo.Click += BtnNuevo_Click;
            btnEditar.Click += BtnEditar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            cmbGrado.SelectedIndexChanged += CmbGrado_SelectedIndexChanged;

            // Configurar DataGridView
            dgvAlumnos.ReadOnly = true;
            dgvAlumnos.AllowUserToAddRows = false;
            dgvAlumnos.AllowUserToDeleteRows = false;
            dgvAlumnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlumnos.MultiSelect = true;
            dgvAlumnos.AutoGenerateColumns = true;

            ConfigurarEstiloDataGridView();
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
            AplicarTraducciones();
            CargarGrados();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestionar_alumnos");
                btnImportarCSV.Text = LanguageManager.Translate("importar_alumnos_csv");
                lblGrado.Text = LanguageManager.Translate("grado_division");
                lblListaAlumnos.Text = LanguageManager.Translate("alumnos");
                btnExportarCSV.Text = LanguageManager.Translate("exportar_alumnos_csv");
                btnEditarGrado.Text = LanguageManager.Translate("cambiar_grado");
                btnNuevo.Text = LanguageManager.Translate("nuevo");
                btnEditar.Text = LanguageManager.Translate("editar");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
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

        private void CargarGrados()
        {
            cmbGrado.Items.Clear();

            // Obtener todos los alumnos y extraer grados únicos
            try
            {
                var todosAlumnos = _alumnoBLL.ObtenerTodosAlumnos();
                var gradosUnicos = todosAlumnos
                    .Where(a => !string.IsNullOrEmpty(a.Grado) && !string.IsNullOrEmpty(a.Division))
                    .Select(a => FormatearGrado(a.Grado, a.Division))
                    .Distinct()
                    .OrderBy(g => g)
                    .ToList();

                // Si no hay grados, agregar algunos por defecto
                if (gradosUnicos.Count == 0)
                {
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
                    foreach (var grado in gradosUnicos)
                    {
                        cmbGrado.Items.Add(grado);
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

        private void CargarAlumnosPorGrado(string grado, string division)
        {
            try
            {
                var todosAlumnos = _alumnoBLL.ObtenerTodosAlumnos();
                _alumnosGrado = todosAlumnos.FindAll(a =>
                    a.Grado == grado &&
                    a.Division.Equals(division, StringComparison.OrdinalIgnoreCase));

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

                // Actualizar contador
                lblTotal.Text = $"Total: {_alumnosGrado.Count} alumno(s)";
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
                    sfd.Filter = "Archivos Excel (*.xlsx)|*.xlsx|Archivos CSV (*.csv)|*.csv";
                    sfd.FileName = $"Alumnos_{gradoSeleccionado}.xlsx";
                    sfd.Title = "Guardar archivo Excel";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string extension = Path.GetExtension(sfd.FileName).ToLower();
                        if (extension == ".xlsx")
                        {
                            ExportarAlumnosAExcel(sfd.FileName);
                        }
                        else
                        {
                            ExportarAlumnosACSV(sfd.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarAlumnosAExcel(string rutaArchivo)
        {
            try
            {
                // Crear archivo Excel usando OleDb
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={rutaArchivo};Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";

                // Crear el archivo si no existe
                if (File.Exists(rutaArchivo))
                    File.Delete(rutaArchivo);

                // Crear DataTable con los datos
                DataTable dt = new DataTable();
                dt.Columns.Add("Nombre", typeof(string));
                dt.Columns.Add("Apellido", typeof(string));
                dt.Columns.Add("DNI", typeof(string));
                dt.Columns.Add("Grado", typeof(string));
                dt.Columns.Add("División", typeof(string));

                foreach (var alumno in _alumnosGrado)
                {
                    dt.Rows.Add(alumno.Nombre, alumno.Apellido, alumno.DNI, alumno.Grado, alumno.Division);
                }

                // Crear archivo Excel vacío primero
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    // Crear la hoja
                    string createTable = "CREATE TABLE [Alumnos] (" +
                        "[Nombre] VARCHAR(100), " +
                        "[Apellido] VARCHAR(100), " +
                        "[DNI] VARCHAR(20), " +
                        "[Grado] VARCHAR(10), " +
                        "[División] VARCHAR(5))";

                    using (var cmd = new OleDbCommand(createTable, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Insertar datos
                    foreach (DataRow row in dt.Rows)
                    {
                        string insert = "INSERT INTO [Alumnos] ([Nombre], [Apellido], [DNI], [Grado], [División]) " +
                            $"VALUES ('{row["Nombre"].ToString().Replace("'", "''")}', " +
                            $"'{row["Apellido"].ToString().Replace("'", "''")}', " +
                            $"'{row["DNI"]}', " +
                            $"'{row["Grado"]}', " +
                            $"'{row["División"]}')";

                        using (var cmd = new OleDbCommand(insert, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show($"Archivo Excel exportado exitosamente:\n{rutaArchivo}\n\n" +
                    $"Total de alumnos: {_alumnosGrado.Count}", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar a Excel: {ex.Message}\n\n" +
                    "Asegúrese de tener instalado Microsoft Access Database Engine.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarAlumnosACSV(string rutaArchivo)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(rutaArchivo, false, System.Text.Encoding.UTF8))
                {
                    // Escribir encabezado
                    sw.WriteLine("Nombre,Apellido,DNI,Grado,Division");

                    // Escribir datos
                    foreach (var alumno in _alumnosGrado)
                    {
                        sw.WriteLine($"{alumno.Nombre},{alumno.Apellido},{alumno.DNI},{alumno.Grado},{alumno.Division}");
                    }
                }

                MessageBox.Show($"Archivo CSV exportado exitosamente:\n{rutaArchivo}\n\n" +
                    $"Total de alumnos: {_alumnosGrado.Count}", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al escribir el archivo CSV: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                            foreach (DataGridViewRow row in dgvAlumnos.SelectedRows)
                            {
                                Alumno alumno = (Alumno)row.DataBoundItem;
                                alumno.Grado = nuevoGrado;
                                alumno.Division = nuevaDivision;
                                _alumnoBLL.ActualizarAlumno(alumno);
                                actualizados++;
                            }

                            MessageBox.Show($"Se actualizaron {actualizados} alumno(s) al grado {nuevoGradoSeleccionado}",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Recargar la lista
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

                    _alumnoBLL.GuardarAlumno(formEditar.AlumnoEditado);

                    MessageBox.Show("Alumno creado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recargar lista
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

        private void BtnEliminarGrado_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbGrado.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe seleccionar un grado para eliminar", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string gradoSeleccionado = cmbGrado.SelectedItem.ToString();

                // Contar alumnos en el grado
                int cantidadAlumnos = _alumnosGrado.Count;

                var resultado = MessageBox.Show(
                    $"¿Está seguro de eliminar el grado {gradoSeleccionado}?\n\n" +
                    $"Esta acción eliminará los {cantidadAlumnos} alumno(s) de este grado.\n" +
                    "Esta operación NO se puede deshacer.",
                    "Confirmar eliminación de grado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    // Eliminar todos los alumnos del grado
                    foreach (var alumno in _alumnosGrado.ToList())
                    {
                        _alumnoBLL.EliminarAlumno(alumno);
                    }

                    // Remover grado del combo
                    cmbGrado.Items.Remove(gradoSeleccionado);

                    if (cmbGrado.Items.Count > 0)
                    {
                        cmbGrado.SelectedIndex = 0;
                    }
                    else
                    {
                        _alumnosGrado.Clear();
                        dgvAlumnos.DataSource = null;
                        lblTotal.Text = "Total: 0 alumno(s)";
                    }

                    MessageBox.Show($"Grado {gradoSeleccionado} y sus {cantidadAlumnos} alumno(s) eliminados exitosamente",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar grado: {ex.Message}", "Error",
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

        private void CrearPlantillaExcel(string rutaArchivo)
        {
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={rutaArchivo};Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";

            if (File.Exists(rutaArchivo))
                File.Delete(rutaArchivo);

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                string createTable = "CREATE TABLE [Alumnos] (" +
                    "[Nombre] VARCHAR(100), " +
                    "[Apellido] VARCHAR(100), " +
                    "[DNI] VARCHAR(20), " +
                    "[Grado] VARCHAR(10), " +
                    "[División] VARCHAR(5))";

                using (var cmd = new OleDbCommand(createTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Agregar filas de ejemplo
                string[] ejemplos = {
                    "('Juan', 'Pérez', '12345678', '1', 'A')",
                    "('María', 'González', '23456789', '1', 'A')",
                    "('Pedro', 'Rodríguez', '34567890', '2', 'B')"
                };

                foreach (var ejemplo in ejemplos)
                {
                    string insert = $"INSERT INTO [Alumnos] ([Nombre], [Apellido], [DNI], [Grado], [División]) VALUES {ejemplo}";
                    using (var cmd = new OleDbCommand(insert, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
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

        private void ProcesarImportacion(List<Alumno> alumnos, bool saltarDuplicados)
        {
            int importados = 0;
            int omitidos = 0;
            int errores = 0;

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

                    _alumnoBLL.GuardarAlumno(alumno);
                    importados++;
                }
                catch
                {
                    errores++;
                }
            }

            string mensaje = $"Importación completada:\n\n" +
                $"✓ Importados: {importados}\n" +
                (omitidos > 0 ? $"⊘ Omitidos (duplicados): {omitidos}\n" : "") +
                (errores > 0 ? $"✗ Errores: {errores}" : "");

            MessageBox.Show(mensaje, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Recargar lista
            CmbGrado_SelectedIndexChanged(null, EventArgs.Empty);
        }
    }
}
