using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using Services.Services;
using Services.DomainModel.Security.Composite;
using UI.WinUi;

namespace UI.WinUi.Bitacoras
{
    public partial class ConsultarBitacoraSeguridad : BaseForm
    {
        private readonly BitacoraSeguridadBLL _bitacoraAdminBLL;
        private Usuario _usuarioLogueado;
        private List<BitacoraSeguridad> _registrosActuales;

        public ConsultarBitacoraSeguridad()
        {
            InitializeComponent();
        }

        public ConsultarBitacoraSeguridad(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _bitacoraAdminBLL = new BitacoraSeguridadBLL();
        }

        private void ConsultarBitacoraSeguridad_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar permisos
                if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("consultarBitacoraSeguridad"))
                {
                    MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
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

        protected override void AplicarTraducciones()
        {
            // Configurar títulos y traducciones
            this.Text = LanguageManager.Translate("bitacora_admin_titulo");
            lblDesde.Text = LanguageManager.Translate("bitacora_fecha_inicio") + ":";
            lblHasta.Text = LanguageManager.Translate("bitacora_fecha_fin") + ":";
            lblCriticidad.Text = LanguageManager.Translate("bitacora_admin_gravedad") + ":";
            lblBuscar.Text = LanguageManager.Translate("bitacora_buscar") + ":";

            btnFiltrar.Text = LanguageManager.Translate("bitacora_filtrar");
            btnVolver.Text = LanguageManager.Translate("volver");

            // Recargar ComboBox con traducciones
            cboCriticidad.Items.Clear();
            cboCriticidad.Items.Add(new ComboBoxItem { Text = LanguageManager.Translate("bitacora_todos"), Value = null });
            cboCriticidad.Items.Add(new ComboBoxItem { Text = LanguageManager.Translate("bitacora_gravedad_bajo"), Value = "Bajo" });
            cboCriticidad.Items.Add(new ComboBoxItem { Text = LanguageManager.Translate("bitacora_gravedad_medio"), Value = "Medio" });
            cboCriticidad.Items.Add(new ComboBoxItem { Text = LanguageManager.Translate("bitacora_gravedad_alto"), Value = "Alto" });
            cboCriticidad.DisplayMember = "Text";
            cboCriticidad.ValueMember = "Value";
            cboCriticidad.SelectedIndex = 0;
        }

        private void ConfigurarFormulario()
        {
            // Configurar DateTimePickers - últimos 30 días por defecto
            dtpDesde.Value = DateTime.Now.AddDays(-30);
            dtpHasta.Value = DateTime.Now;
            dtpDesde.MaxDate = DateTime.Now;
            dtpHasta.MaxDate = DateTime.Now;
        }

        // Clase auxiliar para ComboBox
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
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

            // Fecha - Formato más compacto
            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = LanguageManager.Translate("bitacora_fecha"),
                Name = "colFecha",
                FillWeight = 12,
                MinimumWidth = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM HH:mm" },
                ReadOnly = true
            });

            // Usuario
            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreUsuario",
                HeaderText = LanguageManager.Translate("bitacora_usuario"),
                Name = "colUsuario",
                FillWeight = 10,
                MinimumWidth = 80,
                ReadOnly = true
            });

            // Acción + Detalle (columna más ancha con más información)
            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Accion",
                HeaderText = LanguageManager.Translate("bitacora_accion"),
                Name = "colAccion",
                FillWeight = 60,
                MinimumWidth = 200,
                ReadOnly = true
            });

            // Gravedad
            dgvBitacora.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Gravedad",
                HeaderText = LanguageManager.Translate("bitacora_admin_gravedad"),
                Name = "colGravedad",
                FillWeight = 8,
                MinimumWidth = 70,
                ReadOnly = true
            });

            // Agregar evento para mostrar detalle al hacer doble clic
            dgvBitacora.CellDoubleClick += DgvBitacora_CellDoubleClick;

            // Agregar evento para combinar Acción + Detalle
            dgvBitacora.CellFormatting += DgvBitacora_CellFormatting;
        }

        private void DgvBitacora_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Combinar Acción + Detalle en la columna de Acción
            if (dgvBitacora.Columns[e.ColumnIndex].Name == "colAccion" && e.RowIndex >= 0)
            {
                var registro = dgvBitacora.Rows[e.RowIndex].DataBoundItem as BitacoraSeguridad;
                if (registro != null)
                {
                    // Combinar acción con un resumen del detalle
                    string accion = registro.Accion ?? "";
                    string detalle = registro.Detalle ?? "";

                    // Si el detalle tiene contenido, agregar los primeros 100 caracteres
                    if (!string.IsNullOrWhiteSpace(detalle))
                    {
                        string resumenDetalle = detalle.Length > 100
                            ? detalle.Substring(0, 100) + "..."
                            : detalle;

                        e.Value = $"{accion} - {resumenDetalle}";
                    }
                    else
                    {
                        e.Value = accion;
                    }

                    e.FormattingApplied = true;
                }
            }
        }

        private void CargarRegistros()
        {
            try
            {
                // Obtener valores de filtros
                DateTime fechaDesde = dtpDesde.Value.Date;
                DateTime fechaHasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1); // Hasta 23:59:59

                // Obtener gravedad seleccionada
                string gravedadFiltro = null;
                if (cboCriticidad.SelectedItem is ComboBoxItem gravedadItem)
                {
                    gravedadFiltro = gravedadItem.Value;
                }

                // Obtener registros con filtros
                _registrosActuales = _bitacoraAdminBLL.ObtenerConFiltros(
                    fechaInicio: fechaDesde,
                    fechaFin: fechaHasta,
                    tipoEvento: null,
                    gravedad: gravedadFiltro,
                    modulo: null
                );

                // Aplicar filtro de búsqueda por texto si hay texto ingresado
                if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    string textoBusqueda = txtBuscar.Text.ToLower();
                    _registrosActuales = _registrosActuales.Where(r =>
                        (r.NombreUsuario != null && r.NombreUsuario.ToLower().Contains(textoBusqueda)) ||
                        (r.Accion != null && r.Accion.ToLower().Contains(textoBusqueda)) ||
                        (r.Gravedad != null && r.Gravedad.ToLower().Contains(textoBusqueda)) ||
                        (r.Detalle != null && r.Detalle.ToLower().Contains(textoBusqueda))
                    ).ToList();
                }

                dgvBitacora.DataSource = null;
                dgvBitacora.DataSource = _registrosActuales;

                // Actualizar contador
                lblRegistros.Text = $"Registros encontrados: {_registrosActuales.Count}";

                // Colorear filas según gravedad
                ColorearFilasPorGravedad();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar registros: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ColorearFilasPorGravedad()
        {
            foreach (DataGridViewRow row in dgvBitacora.Rows)
            {
                if (row.DataBoundItem is BitacoraSeguridad registro)
                {
                    switch (registro.Gravedad)
                    {
                        case "Alto":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                            break;
                        case "Medio":
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case "Bajo":
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
                var registro = dgvBitacora.Rows[e.RowIndex].DataBoundItem as BitacoraSeguridad;
                if (registro != null)
                {
                    MostrarDetalle(registro);
                }
            }
        }

        private void MostrarDetalle(BitacoraSeguridad registro)
        {
            string detalle = $"Fecha: {registro.Fecha:dd/MM/yyyy HH:mm:ss}\n" +
                           $"Usuario: {registro.NombreUsuario ?? "N/A"}\n" +
                           $"Acción: {registro.Accion}\n" +
                           $"Gravedad: {registro.Gravedad}\n\n" +
                           $"Detalle:\n{registro.Detalle ?? "Sin detalles adicionales"}";

            MessageBox.Show(detalle, "Detalle del Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CargarRegistros();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
