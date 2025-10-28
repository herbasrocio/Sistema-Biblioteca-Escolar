using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security.Composite;

namespace UI.WinUi.Reportes
{
    public partial class ConsultarBitacoraBibliotecario : Form
    {
        private readonly BitacoraBibliotecarioBLL _bitacoraBibliotecarioBLL;
        private Usuario _usuarioLogueado;
        private List<BitacoraBibliotecario> _registrosActuales;

        public ConsultarBitacoraBibliotecario()
        {
            InitializeComponent();
        }

        public ConsultarBitacoraBibliotecario(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _bitacoraBibliotecarioBLL = new BitacoraBibliotecarioBLL();
        }

        private void ConsultarBitacoraBibliotecario_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar permisos
                if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("consultarBitacoraBibliotecario"))
                {
                    MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                ConfigurarFormulario();
                ConfigurarDataGridView();
                CargarRegistros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            // Configurar títulos y traducciones
            this.Text = LanguageManager.Translate("bitacora_bibliotecario_titulo");
            lblDesde.Text = LanguageManager.Translate("bitacora_fecha_inicio") + ":";
            lblHasta.Text = LanguageManager.Translate("bitacora_fecha_fin") + ":";
            lblTipoOperacion.Text = LanguageManager.Translate("bitacora_bibliotecario_tipo_operacion") + ":";
            lblEntidad.Text = LanguageManager.Translate("bitacora_bibliotecario_entidad_afectada") + ":";
            lblModulo.Text = LanguageManager.Translate("bitacora_modulo") + ":";

            btnFiltrar.Text = LanguageManager.Translate("bitacora_filtrar");
            btnLimpiar.Text = LanguageManager.Translate("bitacora_limpiar");
            btnVolver.Text = LanguageManager.Translate("volver");

            // Configurar combo de Tipo de Operación
            cmbTipoOperacion.Items.Clear();
            cmbTipoOperacion.Items.Add("TODOS");
            cmbTipoOperacion.Items.Add("Prestamo");
            cmbTipoOperacion.Items.Add("Devolucion");
            cmbTipoOperacion.Items.Add("Renovacion");
            cmbTipoOperacion.Items.Add("ConsultaMaterial");
            cmbTipoOperacion.Items.Add("GestionMaterial");
            cmbTipoOperacion.Items.Add("GestionAlumno");
            cmbTipoOperacion.Items.Add("GestionEjemplar");
            cmbTipoOperacion.SelectedIndex = 0;

            // Configurar combo de Entidad Afectada
            cmbEntidad.Items.Clear();
            cmbEntidad.Items.Add("TODOS");
            cmbEntidad.Items.Add("Material");
            cmbEntidad.Items.Add("Ejemplar");
            cmbEntidad.Items.Add("Alumno");
            cmbEntidad.Items.Add("Prestamo");
            cmbEntidad.Items.Add("Devolucion");
            cmbEntidad.SelectedIndex = 0;

            // Configurar DateTimePickers - últimos 30 días por defecto
            dtpDesde.Value = DateTime.Now.AddDays(-30);
            dtpHasta.Value = DateTime.Now;
            dtpDesde.MaxDate = DateTime.Now;
            dtpHasta.MaxDate = DateTime.Now;

            // TextBox módulo vacío (filtro opcional)
            txtModulo.Text = "";
        }

        private void ConfigurarDataGridView()
        {
            dgvBitacora.AutoGenerateColumns = false;
            dgvBitacora.ReadOnly = true;
            dgvBitacora.AllowUserToAddRows = false;
            dgvBitacora.AllowUserToDeleteRows = false;
            dgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBitacora.MultiSelect = false;
            dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas
            dgvBitacora.Columns.Clear();

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdBitacora",
                HeaderText = "ID",
                Name = "colId",
                Width = 60,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = LanguageManager.Translate("bitacora_fecha"),
                Name = "colFecha",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm:ss" },
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreUsuario",
                HeaderText = LanguageManager.Translate("bitacora_usuario"),
                Name = "colUsuario",
                Width = 120,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TipoOperacion",
                HeaderText = LanguageManager.Translate("bitacora_bibliotecario_tipo_operacion"),
                Name = "colTipoOperacion",
                Width = 120,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Modulo",
                HeaderText = LanguageManager.Translate("bitacora_modulo"),
                Name = "colModulo",
                Width = 180,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Accion",
                HeaderText = LanguageManager.Translate("bitacora_accion"),
                Name = "colAccion",
                Width = 200,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EntidadAfectada",
                HeaderText = LanguageManager.Translate("bitacora_bibliotecario_entidad_afectada"),
                Name = "colEntidadAfectada",
                Width = 120,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdEntidad",
                HeaderText = LanguageManager.Translate("bitacora_bibliotecario_id_entidad"),
                Name = "colIdEntidad",
                Width = 80,
                ReadOnly = true
            });

            // Agregar evento para mostrar detalle al hacer doble clic
            dgvBitacora.CellDoubleClick += DgvBitacora_CellDoubleClick;
        }

        private void CargarRegistros()
        {
            try
            {
                // Obtener registros con filtros actuales
                DateTime fechaDesde = dtpDesde.Value.Date;
                DateTime fechaHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1); // Hasta 23:59:59

                string tipoOperacion = cmbTipoOperacion.SelectedItem?.ToString();
                if (tipoOperacion == "TODOS") tipoOperacion = null;

                string entidadAfectada = cmbEntidad.SelectedItem?.ToString();
                if (entidadAfectada == "TODOS") entidadAfectada = null;

                string modulo = string.IsNullOrWhiteSpace(txtModulo.Text) ? null : txtModulo.Text.Trim();

                _registrosActuales = _bitacoraBibliotecarioBLL.ObtenerConFiltros(
                    fechaInicio: fechaDesde,
                    fechaFin: fechaHasta,
                    tipoOperacion: tipoOperacion,
                    entidadAfectada: entidadAfectada,
                    modulo: modulo
                );

                dgvBitacora.DataSource = null;
                dgvBitacora.DataSource = _registrosActuales;

                // Actualizar contador
                lblRegistros.Text = $"Registros encontrados: {_registrosActuales.Count}";

                // Colorear filas según tipo de operación
                ColorearFilasPorTipoOperacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar registros: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearFilasPorTipoOperacion()
        {
            foreach (DataGridViewRow row in dgvBitacora.Rows)
            {
                if (row.DataBoundItem is BitacoraBibliotecario registro)
                {
                    switch (registro.TipoOperacion)
                    {
                        case "Prestamo":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                            break;
                        case "Devolucion":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        case "Renovacion":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case "GestionMaterial":
                        case "GestionAlumno":
                        case "GestionEjemplar":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan;
                            break;
                        case "ConsultaMaterial":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
                            break;
                    }
                }
            }
        }

        private void DgvBitacora_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var registro = dgvBitacora.Rows[e.RowIndex].DataBoundItem as BitacoraBibliotecario;
                if (registro != null)
                {
                    MostrarDetalle(registro);
                }
            }
        }

        private void MostrarDetalle(BitacoraBibliotecario registro)
        {
            string detalle = $"ID Bitácora: {registro.IdBitacora}\n\n" +
                           $"Fecha: {registro.Fecha:dd/MM/yyyy HH:mm:ss}\n" +
                           $"Usuario: {registro.NombreUsuario ?? "N/A"}\n" +
                           $"Tipo de Operación: {registro.TipoOperacion}\n" +
                           $"Módulo: {registro.Modulo}\n" +
                           $"Acción: {registro.Accion}\n" +
                           $"Entidad Afectada: {registro.EntidadAfectada ?? "N/A"}\n" +
                           $"ID Entidad: {registro.IdEntidad?.ToString() ?? "N/A"}\n\n" +
                           $"Detalle:\n{registro.Detalle ?? "Sin detalles adicionales"}";

            MessageBox.Show(detalle, "Detalle del Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarRegistros();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Restablecer filtros
            dtpDesde.Value = DateTime.Now.AddDays(-30);
            dtpHasta.Value = DateTime.Now;
            cmbTipoOperacion.SelectedIndex = 0;
            cmbEntidad.SelectedIndex = 0;
            txtModulo.Text = "";

            // Recargar datos
            CargarRegistros();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
