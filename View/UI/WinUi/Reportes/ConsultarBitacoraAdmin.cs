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
    public partial class ConsultarBitacoraAdmin : Form
    {
        private readonly BitacoraAdminBLL _bitacoraAdminBLL;
        private Usuario _usuarioLogueado;
        private List<BitacoraAdmin> _registrosActuales;

        public ConsultarBitacoraAdmin()
        {
            InitializeComponent();
        }

        public ConsultarBitacoraAdmin(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _bitacoraAdminBLL = new BitacoraAdminBLL();
        }

        private void ConsultarBitacoraAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar permisos
                if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("consultarBitacoraAdmin"))
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
            this.Text = LanguageManager.Translate("bitacora_admin_titulo");
            lblDesde.Text = LanguageManager.Translate("bitacora_fecha_inicio") + ":";
            lblHasta.Text = LanguageManager.Translate("bitacora_fecha_fin") + ":";
            lblTipoEvento.Text = LanguageManager.Translate("bitacora_admin_tipo_evento") + ":";
            lblCriticidad.Text = LanguageManager.Translate("bitacora_admin_criticidad") + ":";
            lblModulo.Text = LanguageManager.Translate("bitacora_modulo") + ":";

            btnFiltrar.Text = LanguageManager.Translate("bitacora_filtrar");
            btnLimpiar.Text = LanguageManager.Translate("bitacora_limpiar");
            btnVolver.Text = LanguageManager.Translate("volver");

            // Configurar combo de Tipo de Evento
            cmbTipoEvento.Items.Clear();
            cmbTipoEvento.Items.Add("TODOS");
            cmbTipoEvento.Items.Add("Error");
            cmbTipoEvento.Items.Add("Seguridad");
            cmbTipoEvento.Items.Add("CambioCritico");
            cmbTipoEvento.SelectedIndex = 0;

            // Configurar combo de Criticidad
            cmbCriticidad.Items.Clear();
            cmbCriticidad.Items.Add("TODOS");
            cmbCriticidad.Items.Add("Baja");
            cmbCriticidad.Items.Add("Media");
            cmbCriticidad.Items.Add("Alta");
            cmbCriticidad.Items.Add("Critica");
            cmbCriticidad.SelectedIndex = 0;

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
                DataPropertyName = "TipoEvento",
                HeaderText = LanguageManager.Translate("bitacora_admin_tipo_evento"),
                Name = "colTipoEvento",
                Width = 120,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Criticidad",
                HeaderText = LanguageManager.Translate("bitacora_admin_criticidad"),
                Name = "colCriticidad",
                Width = 80,
                ReadOnly = true
            });

            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Modulo",
                HeaderText = LanguageManager.Translate("bitacora_modulo"),
                Name = "colModulo",
                Width = 150,
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
                DataPropertyName = "DireccionIP",
                HeaderText = LanguageManager.Translate("bitacora_admin_direccion_ip"),
                Name = "colDireccionIP",
                Width = 120,
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

                string tipoEvento = cmbTipoEvento.SelectedItem?.ToString();
                if (tipoEvento == "TODOS") tipoEvento = null;

                string criticidad = cmbCriticidad.SelectedItem?.ToString();
                if (criticidad == "TODOS") criticidad = null;

                string modulo = string.IsNullOrWhiteSpace(txtModulo.Text) ? null : txtModulo.Text.Trim();

                _registrosActuales = _bitacoraAdminBLL.ObtenerConFiltros(
                    fechaInicio: fechaDesde,
                    fechaFin: fechaHasta,
                    tipoEvento: tipoEvento,
                    criticidad: criticidad,
                    modulo: modulo
                );

                dgvBitacora.DataSource = null;
                dgvBitacora.DataSource = _registrosActuales;

                // Actualizar contador
                lblRegistros.Text = $"Registros encontrados: {_registrosActuales.Count}";

                // Colorear filas según criticidad
                ColorearFilasPorCriticidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar registros: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearFilasPorCriticidad()
        {
            foreach (DataGridViewRow row in dgvBitacora.Rows)
            {
                if (row.DataBoundItem is BitacoraAdmin registro)
                {
                    switch (registro.Criticidad)
                    {
                        case "Critica":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                            break;
                        case "Alta":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                            break;
                        case "Media":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case "Baja":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            break;
                    }
                }
            }
        }

        private void DgvBitacora_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var registro = dgvBitacora.Rows[e.RowIndex].DataBoundItem as BitacoraAdmin;
                if (registro != null)
                {
                    MostrarDetalle(registro);
                }
            }
        }

        private void MostrarDetalle(BitacoraAdmin registro)
        {
            string detalle = $"ID Bitácora: {registro.IdBitacora}\n\n" +
                           $"Fecha: {registro.Fecha:dd/MM/yyyy HH:mm:ss}\n" +
                           $"Usuario: {registro.NombreUsuario ?? "N/A"}\n" +
                           $"Tipo de Evento: {registro.TipoEvento}\n" +
                           $"Criticidad: {registro.Criticidad}\n" +
                           $"Módulo: {registro.Modulo}\n" +
                           $"Acción: {registro.Accion}\n" +
                           $"Dirección IP: {registro.DireccionIP ?? "N/A"}\n\n" +
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
            cmbTipoEvento.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
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
