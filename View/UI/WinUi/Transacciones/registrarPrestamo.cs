using System;
using System.Collections.Generic;
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
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += RegistrarPrestamo_Load;
            btnRegistrar.Click += BtnRegistrar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVolver.Click += BtnVolver_Click;
            cmbMaterial.SelectedIndexChanged += CmbMaterial_SelectedIndexChanged;
            cmbAlumno.SelectedIndexChanged += CmbAlumno_SelectedIndexChanged;

            // Configurar DateTimePicker
            dtpFechaDevolucionPrevista.MinDate = DateTime.Now.AddDays(1);
            dtpFechaDevolucionPrevista.Value = DateTime.Now.AddDays(7); // Por defecto 7 días
        }

        private void RegistrarPrestamo_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarMateriales();
            CargarAlumnos();
            LimpiarCampos();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("registrar_prestamo");
                groupBoxDatos.Text = LanguageManager.Translate("datos_prestamo");
                lblMaterial.Text = LanguageManager.Translate("material");
                lblAlumno.Text = LanguageManager.Translate("alumno");
                lblFechaDevolucion.Text = LanguageManager.Translate("fecha_devolucion_prevista");
                btnRegistrar.Text = LanguageManager.Translate("registrar");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void CargarMateriales()
        {
            try
            {
                var materiales = _materialBLL.ObtenerTodosMateriales();

                // Filtrar solo materiales con disponibilidad
                var materialesDisponibles = materiales.FindAll(m => m.CantidadDisponible > 0);

                cmbMaterial.DataSource = null;
                cmbMaterial.DisplayMember = "Titulo";
                cmbMaterial.ValueMember = "IdMaterial";
                cmbMaterial.DataSource = materialesDisponibles;
                cmbMaterial.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materiales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void CmbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaterial.SelectedIndex >= 0)
            {
                var material = (Material)cmbMaterial.SelectedItem;
                lblDisponibles.Text = $"Disponibles: {material.CantidadDisponible} de {material.CantidadTotal}";
                lblAutor.Text = $"Autor: {material.Autor}";
            }
            else
            {
                lblDisponibles.Text = "";
                lblAutor.Text = "";
            }
        }

        private void CmbAlumno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAlumno.SelectedIndex >= 0)
            {
                var alumno = (Alumno)cmbAlumno.SelectedItem;
                lblGrado.Text = $"Grado: {alumno.Grado} {alumno.Division}";
                lblDNI.Text = $"DNI: {alumno.DNI}";

                // Verificar préstamos activos
                var prestamosActivos = _prestamoBLL.ObtenerPorAlumno(alumno.IdAlumno);
                int cantidadActivos = prestamosActivos.FindAll(p => p.Estado == "Activo" || p.Estado == "Atrasado").Count;
                lblPrestamosActivos.Text = $"Préstamos activos: {cantidadActivos}";
            }
            else
            {
                lblGrado.Text = "";
                lblDNI.Text = "";
                lblPrestamosActivos.Text = "";
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMaterial.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe seleccionar un material", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbAlumno.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe seleccionar un alumno", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var material = (Material)cmbMaterial.SelectedItem;
                var alumno = (Alumno)cmbAlumno.SelectedItem;

                Prestamo prestamo = new Prestamo
                {
                    IdMaterial = material.IdMaterial,
                    IdAlumno = alumno.IdAlumno,
                    IdUsuario = _usuarioLogueado.IdUsuario,
                    FechaPrestamo = DateTime.Now,
                    FechaDevolucionPrevista = dtpFechaDevolucionPrevista.Value,
                    Estado = "Activo"
                };

                _prestamoBLL.RegistrarPrestamo(prestamo);

                MessageBox.Show(
                    $"Préstamo registrado exitosamente\n\nMaterial: {material.Titulo}\nAlumno: {alumno.NombreCompleto}\nDevolución prevista: {dtpFechaDevolucionPrevista.Value:dd/MM/yyyy}",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LimpiarCampos();
                CargarMateriales(); // Recargar para actualizar disponibilidad
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar préstamo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            cmbMaterial.SelectedIndex = -1;
            cmbAlumno.SelectedIndex = -1;
            dtpFechaDevolucionPrevista.Value = DateTime.Now.AddDays(7);
            lblDisponibles.Text = "";
            lblAutor.Text = "";
            lblGrado.Text = "";
            lblDNI.Text = "";
            lblPrestamosActivos.Text = "";
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
