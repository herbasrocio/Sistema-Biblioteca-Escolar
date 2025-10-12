using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using BLL;
using DomainModel;

namespace UI.WinUi.Administrador
{
    public partial class gestionPromocionAlumnos : Form
    {
        private Usuario _usuarioLogueado;
        private InscripcionBLL _inscripcionBLL;
        private int _anioActual;
        private int _anioSiguiente;

        public gestionPromocionAlumnos()
        {
            InitializeComponent();
        }

        public gestionPromocionAlumnos(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _inscripcionBLL = new InscripcionBLL();
            _anioActual = DateTime.Now.Year;
            _anioSiguiente = _anioActual + 1;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += GestionPromocionAlumnos_Load;
            btnCargarEstadisticas.Click += BtnCargarEstadisticas_Click;
            btnPromocionarGrado.Click += BtnPromocionarGrado_Click;
            btnPromocionMasiva.Click += BtnPromocionMasiva_Click;

            // Configurar NumericUpDown para años
            numAnioActual.Value = _anioActual;
            numAnioSiguiente.Value = _anioSiguiente;
            numAnioActual.Minimum = 2020;
            numAnioActual.Maximum = 2100;
            numAnioSiguiente.Minimum = 2020;
            numAnioSiguiente.Maximum = 2100;

            numAnioActual.ValueChanged += (s, e) => {
                _anioActual = (int)numAnioActual.Value;
                numAnioSiguiente.Value = _anioActual + 1;
            };

            numAnioSiguiente.ValueChanged += (s, e) => {
                _anioSiguiente = (int)numAnioSiguiente.Value;
            };

            // Configurar DataGridView
            dgvEstadisticas.ReadOnly = true;
            dgvEstadisticas.AllowUserToAddRows = false;
            dgvEstadisticas.AllowUserToDeleteRows = false;
            dgvEstadisticas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstadisticas.MultiSelect = false;
            dgvEstadisticas.AutoGenerateColumns = false;

            ConfigurarColumnasDataGridView();
            ConfigurarEstiloDataGridView();

            // Configurar ComboBox de Grados
            CargarGrados();
        }

        private void ConfigurarColumnasDataGridView()
        {
            dgvEstadisticas.Columns.Clear();

            dgvEstadisticas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Grado",
                HeaderText = "Grado",
                DataPropertyName = "Grado",
                Width = 100
            });

            dgvEstadisticas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Division",
                HeaderText = "División",
                DataPropertyName = "Division",
                Width = 100
            });

            dgvEstadisticas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CantidadAlumnos",
                HeaderText = "Cantidad de Alumnos",
                DataPropertyName = "CantidadAlumnos",
                Width = 150
            });
        }

        private void ConfigurarEstiloDataGridView()
        {
            dgvEstadisticas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvEstadisticas.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEstadisticas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 247);
            dgvEstadisticas.RowsDefaultCellStyle.BackColor = Color.White;
            dgvEstadisticas.EnableHeadersVisualStyles = false;
            dgvEstadisticas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvEstadisticas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEstadisticas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvEstadisticas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEstadisticas.GridColor = Color.FromArgb(189, 195, 199);
        }

        private void CargarGrados()
        {
            cmbGradoActual.Items.Clear();
            cmbGradoNuevo.Items.Clear();

            for (int i = 1; i <= 7; i++)
            {
                cmbGradoActual.Items.Add(i.ToString());
                cmbGradoNuevo.Items.Add(i.ToString());
            }

            if (cmbGradoActual.Items.Count > 0)
                cmbGradoActual.SelectedIndex = 0;
        }

        private void GestionPromocionAlumnos_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarEstadisticas();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("promocion_alumnos");
                lblAnioActual.Text = LanguageManager.Translate("anio_actual");
                lblAnioSiguiente.Text = LanguageManager.Translate("anio_siguiente");
                btnCargarEstadisticas.Text = LanguageManager.Translate("cargar_estadisticas");
                grpPromocionGrado.Text = LanguageManager.Translate("promocion_por_grado");
                lblGradoActual.Text = LanguageManager.Translate("grado_actual");
                lblDivisionActual.Text = LanguageManager.Translate("division_actual");
                lblGradoNuevo.Text = LanguageManager.Translate("grado_nuevo");
                lblDivisionNueva.Text = LanguageManager.Translate("division_nueva");
                btnPromocionarGrado.Text = LanguageManager.Translate("promocionar_grado");
                btnPromocionMasiva.Text = LanguageManager.Translate("promocion_masiva");
            }
            catch (Exception)
            {
                // Si falta alguna traducción, usar texto por defecto
                this.Text = "Promoción de Alumnos";
                lblAnioActual.Text = "Año Actual:";
                lblAnioSiguiente.Text = "Año Siguiente:";
                btnCargarEstadisticas.Text = "Cargar Estadísticas";
                grpPromocionGrado.Text = "Promoción por Grado";
                lblGradoActual.Text = "Grado Actual:";
                lblDivisionActual.Text = "División Actual:";
                lblGradoNuevo.Text = "Grado Nuevo:";
                lblDivisionNueva.Text = "División Nueva:";
                btnPromocionarGrado.Text = "Promocionar Grado";
                btnPromocionMasiva.Text = "Promoción Masiva de Todos los Grados";
            }
        }

        private void BtnCargarEstadisticas_Click(object sender, EventArgs e)
        {
            CargarEstadisticas();
        }

        private void CargarEstadisticas()
        {
            try
            {
                var estadisticas = _inscripcionBLL.ObtenerEstadisticasPorAnio(_anioActual);

                dgvEstadisticas.DataSource = null;
                dgvEstadisticas.DataSource = estadisticas;

                // Mostrar resumen
                int totalAlumnos = estadisticas.Sum(e => e.CantidadAlumnos);
                lblResumen.Text = $"Total de alumnos inscriptos en {_anioActual}: {totalAlumnos}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar estadísticas: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnPromocionarGrado_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar selección
                if (cmbGradoActual.SelectedIndex == -1 || cmbGradoNuevo.SelectedIndex == -1)
                {
                    MessageBox.Show(
                        "Debe seleccionar el grado actual y el grado nuevo",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                string gradoActual = cmbGradoActual.SelectedItem.ToString();
                string divisionActual = string.IsNullOrWhiteSpace(txtDivisionActual.Text) ? null : txtDivisionActual.Text.Trim();
                string gradoNuevo = cmbGradoNuevo.SelectedItem.ToString();
                string divisionNueva = string.IsNullOrWhiteSpace(txtDivisionNueva.Text) ? null : txtDivisionNueva.Text.Trim();

                // Confirmación
                string mensaje = $"¿Está seguro de promocionar a los alumnos de {gradoActual}° {divisionActual ?? "todas las divisiones"} " +
                                $"del año {_anioActual} al grado {gradoNuevo}° {divisionNueva ?? "(mantener división)"} del año {_anioSiguiente}?";

                DialogResult confirmacion = MessageBox.Show(
                    mensaje,
                    "Confirmar Promoción",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmacion != DialogResult.Yes)
                    return;

                // Realizar promoción
                Cursor = Cursors.WaitCursor;
                ResultadoPromocion resultado = _inscripcionBLL.PromocionarAlumnosPorGrado(
                    _anioActual,
                    _anioSiguiente,
                    gradoActual,
                    divisionActual,
                    gradoNuevo,
                    divisionNueva
                );
                Cursor = Cursors.Default;

                if (resultado.Exitoso)
                {
                    MessageBox.Show(
                        resultado.Mensaje,
                        "Promoción Exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Recargar estadísticas
                    CargarEstadisticas();
                }
                else
                {
                    MessageBox.Show(
                        $"Error en la promoción: {resultado.Mensaje}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Error al promocionar grado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnPromocionMasiva_Click(object sender, EventArgs e)
        {
            try
            {
                // Confirmación con advertencia
                string mensaje = $"ATENCIÓN: Esta operación promocionará TODOS los alumnos del año {_anioActual} al año {_anioSiguiente} " +
                                $"según el siguiente esquema:\n\n" +
                                $"1° → 2°\n" +
                                $"2° → 3°\n" +
                                $"3° → 4°\n" +
                                $"4° → 5°\n" +
                                $"5° → 6°\n" +
                                $"6° → 7°\n" +
                                $"7° → EGRESADOS\n\n" +
                                $"Esta operación NO se puede deshacer. ¿Desea continuar?";

                DialogResult confirmacion = MessageBox.Show(
                    mensaje,
                    "Confirmar Promoción Masiva",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirmacion != DialogResult.Yes)
                    return;

                // Segunda confirmación
                DialogResult segundaConfirmacion = MessageBox.Show(
                    "¿Está completamente seguro? Esta es su última oportunidad para cancelar.",
                    "Confirmación Final",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (segundaConfirmacion != DialogResult.Yes)
                    return;

                // Realizar promoción masiva
                Cursor = Cursors.WaitCursor;
                ResultadoPromocion resultado = _inscripcionBLL.PromocionarTodosLosAlumnos(_anioActual, _anioSiguiente);
                Cursor = Cursors.Default;

                if (resultado.Exitoso)
                {
                    string mensajeResultado = $"Promoción Masiva Completada Exitosamente\n\n" +
                                            $"Alumnos promovidos: {resultado.AlumnosPromovidos}\n" +
                                            $"Egresados: {resultado.Egresados}\n" +
                                            $"Total procesados: {resultado.AlumnosFinalizados}";

                    MessageBox.Show(
                        mensajeResultado,
                        "Promoción Exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Recargar estadísticas
                    CargarEstadisticas();
                }
                else
                {
                    MessageBox.Show(
                        $"Error en la promoción masiva: {resultado.Mensaje}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Error al realizar promoción masiva: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
