using System;
using System.Windows.Forms;
using DomainModel;
using ServicesSecurity.Services;

namespace UI.WinUi.Administrador
{
    public partial class editarAlumno : BaseForm
    {
        public Alumno AlumnoEditado { get; private set; }
        private bool _esNuevo;

        public editarAlumno(Alumno alumno = null)
        {
            InitializeComponent();
            _esNuevo = (alumno == null);

            if (_esNuevo)
            {
                AlumnoEditado = new Alumno
                {
                    IdAlumno = Guid.NewGuid()
                };
            }
            else
            {
                AlumnoEditado = alumno;
                CargarDatos();
            }

            ConfigurarFormulario();
            // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
        }

        protected override void AplicarTraducciones()
        {
            // Traducir título según modo
            this.Text = _esNuevo
                ? LanguageManager.Translate("nuevo_alumno")
                : LanguageManager.Translate("editar_alumno");

            // Traducir botones
            btnGuardar.Text = LanguageManager.Translate("guardar");
            btnCancelar.Text = LanguageManager.Translate("cancelar");

            // Traducir labels (si existen en el Designer)
            // Nota: Si los labels no están definidos en el Designer, se pueden agregar después
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
                    MessageBox.Show(LanguageManager.Translate("nombre_obligatorio"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("apellido_obligatorio"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtApellido.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDNI.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("dni_obligatorio"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDNI.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbGrado.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("grado_obligatorio"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbGrado.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbDivision.Text))
                {
                    MessageBox.Show(LanguageManager.Translate("division_obligatoria"),
                        LanguageManager.Translate("validacion"),
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
                MessageBox.Show($"{LanguageManager.Translate("error_guardar")}: {ex.Message}",
                    LanguageManager.Translate("error"),
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
