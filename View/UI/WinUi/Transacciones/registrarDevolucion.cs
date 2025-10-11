using System;
using System.Collections.Generic;
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
            CargarAlumnos();
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

        private void CargarAlumnos()
        {
            try
            {
                var alumnos = _alumnoBLL.ObtenerTodosAlumnos();
                cmbAlumno.DataSource = null;
                cmbAlumno.DisplayMember = "NombreCompleto";
                cmbAlumno.ValueMember = "IdAlumno";
                cmbAlumno.DataSource = alumnos;
                cmbAlumno.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAlumno.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe seleccionar un alumno", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var alumno = (Alumno)cmbAlumno.SelectedItem;
                var prestamosActivos = _prestamoBLL.ObtenerPorAlumno(alumno.IdAlumno);

                // Filtrar solo activos y atrasados
                var prestamosParaDevolver = prestamosActivos.FindAll(p => p.Estado == "Activo" || p.Estado == "Atrasado");

                if (prestamosParaDevolver.Count == 0)
                {
                    MessageBox.Show($"El alumno {alumno.NombreCompleto} no tiene préstamos activos", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvPrestamos.DataSource = null;
                    return;
                }

                dgvPrestamos.DataSource = prestamosParaDevolver;

                // Configurar columnas
                if (dgvPrestamos.Columns.Count > 0)
                {
                    dgvPrestamos.Columns["IdPrestamo"].Visible = false;
                    dgvPrestamos.Columns["IdMaterial"].Visible = false;
                    dgvPrestamos.Columns["IdAlumno"].Visible = false;
                    dgvPrestamos.Columns["IdUsuario"].Visible = false;
                    dgvPrestamos.Columns["Material"].Visible = false;
                    dgvPrestamos.Columns["Alumno"].Visible = false;

                    dgvPrestamos.Columns["FechaPrestamo"].HeaderText = "Fecha Préstamo";
                    dgvPrestamos.Columns["FechaDevolucionPrevista"].HeaderText = "Devolución Prevista";
                    dgvPrestamos.Columns["Estado"].HeaderText = "Estado";
                }

                lblResultados.Text = $"Préstamos activos: {prestamosParaDevolver.Count}";
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
                var prestamo = (Prestamo)dgvPrestamos.SelectedRows[0].DataBoundItem;

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
            }
            else
            {
                lblEstado.Text = "";
                lblFechaPrestamo.Text = "";
                lblFechaDevolucionPrevista.Text = "";
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

                var prestamo = (Prestamo)dgvPrestamos.SelectedRows[0].DataBoundItem;

                Devolucion devolucion = new Devolucion
                {
                    IdPrestamo = prestamo.IdPrestamo,
                    FechaDevolucion = DateTime.Now,
                    IdUsuario = _usuarioLogueado.IdUsuario,
                    Observaciones = txtObservaciones.Text.Trim(),
                    Prestamo = prestamo
                };

                _devolucionBLL.RegistrarDevolucion(devolucion);

                int diasAtraso = devolucion.DiasDeAtraso();
                string mensaje = diasAtraso > 0
                    ? $"Devolución registrada con {diasAtraso} día(s) de atraso"
                    : "Devolución registrada exitosamente a tiempo";

                MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                BtnBuscar_Click(sender, e); // Recargar lista
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
            cmbAlumno.SelectedIndex = -1;
            txtObservaciones.Clear();
            dgvPrestamos.DataSource = null;
            lblResultados.Text = "";
            lblEstado.Text = "";
            lblFechaPrestamo.Text = "";
            lblFechaDevolucionPrevista.Text = "";
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
