using System;
using System.Windows.Forms;
using DomainModel;

namespace UI.WinUi.Administrador
{
    public partial class editarAlumno : Form
    {
        public Alumno AlumnoEditado { get; private set; }
        private bool _esNuevo;

        public editarAlumno(Alumno alumno = null)
        {
            InitializeComponent();
            _esNuevo = (alumno == null);

            if (_esNuevo)
            {
                this.Text = "Nuevo Alumno";
                AlumnoEditado = new Alumno
                {
                    IdAlumno = Guid.NewGuid()
                };
            }
            else
            {
                this.Text = "Editar Alumno";
                AlumnoEditado = alumno;
                CargarDatos();
            }

            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            // Cargar grados
            for (int i = 1; i <= 7; i++)
            {
                cmbGrado.Items.Add(i.ToString());
            }

            // Cargar divisiones
            cmbDivision.Items.AddRange(new[] { "A", "B", "C" });
        }

        private void CargarDatos()
        {
            txtNombre.Text = AlumnoEditado.Nombre;
            txtApellido.Text = AlumnoEditado.Apellido;
            txtDNI.Text = AlumnoEditado.DNI;
            cmbGrado.Text = AlumnoEditado.Grado;
            cmbDivision.Text = AlumnoEditado.Division;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre es obligatorio", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    MessageBox.Show("El apellido es obligatorio", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtApellido.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDNI.Text))
                {
                    MessageBox.Show("El DNI es obligatorio", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDNI.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbGrado.Text))
                {
                    MessageBox.Show("Debe ingresar un grado", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbGrado.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbDivision.Text))
                {
                    MessageBox.Show("Debe ingresar una división", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbDivision.Focus();
                    return;
                }

                // Actualizar datos
                AlumnoEditado.Nombre = txtNombre.Text.Trim();
                AlumnoEditado.Apellido = txtApellido.Text.Trim();
                AlumnoEditado.DNI = txtDNI.Text.Trim();
                AlumnoEditado.Grado = cmbGrado.Text.Trim();
                AlumnoEditado.Division = cmbDivision.Text.Trim().ToUpper();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
