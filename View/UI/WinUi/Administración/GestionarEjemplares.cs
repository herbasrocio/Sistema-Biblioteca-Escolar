using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL;
using DomainModel;
using DomainModel.Enums;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using UI.Helpers;

namespace UI.WinUi.Administrador
{
    public partial class GestionarEjemplares : Form
    {
        private Usuario _usuarioLogueado;
        private Material _material;
        private EjemplarBLL _ejemplarBLL;
        private MaterialBLL _materialBLL;

        public GestionarEjemplares()
        {
            InitializeComponent();
            _ejemplarBLL = new EjemplarBLL();
            _materialBLL = new MaterialBLL();
        }

        public GestionarEjemplares(Usuario usuario, Material material) : this()
        {
            _usuarioLogueado = usuario;
            _material = material;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += GestionarEjemplares_Load;
            btnAgregar.Click += BtnAgregar_Click;
            btnEditar.Click += BtnEditar_Click;
            btnCambiarEstado.Click += BtnCambiarEstado_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;

            // Configurar DataGridView
            dgvEjemplares.ReadOnly = true;
            dgvEjemplares.AllowUserToAddRows = false;
            dgvEjemplares.AllowUserToDeleteRows = false;
            dgvEjemplares.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEjemplares.MultiSelect = false;

            // Configurar altura de filas
            dgvEjemplares.RowTemplate.Height = 35;
            dgvEjemplares.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Aplicar estilos
            AplicarEstilos();

            // Cargar información del material
            CargarInfoMaterial();

            // Verificar permisos
            ConfigurarPermisosEdicion();
        }

        private void GestionarEjemplares_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarEjemplares();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestionar_ejemplares");
                lblTituloForm.Text = LanguageManager.Translate("gestionar_ejemplares");
                groupBoxInfoMaterial.Text = LanguageManager.Translate("datos_material");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                lblTituloLabel.Text = LanguageManager.Translate("titulo") + ":";
                lblAutorLabel.Text = LanguageManager.Translate("autor") + ":";

                btnAgregar.Text = LanguageManager.Translate("agregar_ejemplar");
                btnEditar.Text = LanguageManager.Translate("editar");
                btnCambiarEstado.Text = LanguageManager.Translate("cambiar_estado");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void AplicarEstilos()
        {
            // Aplicar estilo al formulario
            EstilosSistema.AplicarEstiloFormulario(this);

            // Aplicar estilos a GroupBox
            EstilosSistema.AplicarEstiloGroupBox(groupBoxInfoMaterial);
            EstilosSistema.AplicarEstiloGroupBox(groupBoxAcciones);

            // Aplicar estilos a Labels
            EstilosSistema.AplicarEstiloLabel(lblTituloLabel);
            EstilosSistema.AplicarEstiloLabel(lblAutorLabel);
            EstilosSistema.AplicarEstiloLabel(lblTituloValor);
            EstilosSistema.AplicarEstiloLabel(lblAutorValor);
            EstilosSistema.AplicarEstiloSubtitulo(lblTituloForm);

            // Aplicar estilos a DataGridView
            EstilosSistema.AplicarEstiloDataGridView(dgvEjemplares);

            // Aplicar estilos a botones
            EstilosSistema.AplicarEstiloBotonPrimario(btnAgregar);
            EstilosSistema.AplicarEstiloBotonPrimario(btnEditar);
            EstilosSistema.AplicarEstiloBotonPrimario(btnCambiarEstado);

            // Botón eliminar con color rojo
            btnEliminar.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Cursor = Cursors.Hand;

            EstilosSistema.AplicarEstiloBotonSecundario(btnVolver);
        }

        private void CargarInfoMaterial()
        {
            if (_material == null)
                return;

            lblTituloValor.Text = _material.Titulo;
            lblAutorValor.Text = _material.Autor;
        }

        private void CargarEjemplares()
        {
            try
            {
                if (_material == null)
                    return;

                // Refrescar los datos del material desde la BD para asegurar consistencia
                RefrescarMaterial();

                List<Ejemplar> ejemplares = _ejemplarBLL.ObtenerEjemplaresPorMaterial(_material.IdMaterial);

                // Crear DataTable para mostrar en el DataGridView
                DataTable dt = new DataTable();
                dt.Columns.Add("IdEjemplar", typeof(Guid));
                dt.Columns.Add("NumeroEjemplar", typeof(int));
                dt.Columns.Add("CodigoEjemplar", typeof(string));
                dt.Columns.Add("Estado", typeof(string));
                dt.Columns.Add("Ubicacion", typeof(string));

                foreach (Ejemplar ejemplar in ejemplares)
                {
                    DataRow row = dt.NewRow();
                    row["IdEjemplar"] = ejemplar.IdEjemplar;
                    row["NumeroEjemplar"] = ejemplar.NumeroEjemplar;
                    row["CodigoEjemplar"] = ejemplar.CodigoEjemplar ?? "";
                    row["Estado"] = TraducirEstado(ejemplar.Estado);
                    row["Ubicacion"] = ejemplar.Ubicacion ?? "";
                    dt.Rows.Add(row);
                }

                dgvEjemplares.DataSource = dt;

                // Configurar columnas
                ConfigurarColumnasDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ejemplares: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Refresca los datos del material desde la base de datos
        /// para asegurar que siempre tengamos la información actualizada
        /// </summary>
        private void RefrescarMaterial()
        {
            try
            {
                if (_material != null && _material.IdMaterial != Guid.Empty)
                {
                    Material materialActualizado = _materialBLL.ObtenerMaterialPorId(_material.IdMaterial);
                    if (materialActualizado != null)
                    {
                        _material = materialActualizado;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al refrescar material: {ex.Message}");
            }
        }

        private void ConfigurarColumnasDataGridView()
        {
            if (dgvEjemplares.Columns.Count == 0)
                return;

            // Ocultar columna ID
            dgvEjemplares.Columns["IdEjemplar"].Visible = false;

            // Configurar encabezados
            dgvEjemplares.Columns["NumeroEjemplar"].HeaderText = LanguageManager.Translate("numero_ejemplar");
            dgvEjemplares.Columns["CodigoEjemplar"].HeaderText = LanguageManager.Translate("codigo_ejemplar");
            dgvEjemplares.Columns["Estado"].HeaderText = LanguageManager.Translate("estado");
            dgvEjemplares.Columns["Ubicacion"].HeaderText = LanguageManager.Translate("ubicacion");

            // Configurar anchos fijos para cada columna (Total: 820px de 840px disponibles)
            dgvEjemplares.Columns["NumeroEjemplar"].Width = 100;
            dgvEjemplares.Columns["NumeroEjemplar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgvEjemplares.Columns["CodigoEjemplar"].Width = 200;
            dgvEjemplares.Columns["CodigoEjemplar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgvEjemplares.Columns["Estado"].Width = 180;
            dgvEjemplares.Columns["Estado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgvEjemplares.Columns["Ubicacion"].Width = 340;
            dgvEjemplares.Columns["Ubicacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        private string TraducirEstado(EstadoMaterial estado)
        {
            switch (estado)
            {
                case EstadoMaterial.Disponible:
                    return LanguageManager.Translate("estado_disponible");
                case EstadoMaterial.Prestado:
                    return LanguageManager.Translate("estado_prestado");
                case EstadoMaterial.EnReparacion:
                    return LanguageManager.Translate("estado_en_reparacion");
                case EstadoMaterial.NoDisponible:
                    return LanguageManager.Translate("estado_no_disponible");
                default:
                    return estado.ToString();
            }
        }

        private EstadoMaterial ObtenerEstadoDesdeTexto(string textoEstado)
        {
            if (textoEstado == LanguageManager.Translate("estado_disponible"))
                return EstadoMaterial.Disponible;
            if (textoEstado == LanguageManager.Translate("estado_prestado"))
                return EstadoMaterial.Prestado;
            if (textoEstado == LanguageManager.Translate("estado_en_reparacion"))
                return EstadoMaterial.EnReparacion;
            if (textoEstado == LanguageManager.Translate("estado_no_disponible"))
                return EstadoMaterial.NoDisponible;

            return EstadoMaterial.Disponible;
        }

        #region Event Handlers

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Calcular el próximo número de ejemplar
                List<Ejemplar> ejemplares = _ejemplarBLL.ObtenerEjemplaresPorMaterial(_material.IdMaterial);
                int proximoNumero = ejemplares.Any() ? ejemplares.Max(ej => ej.NumeroEjemplar) + 1 : 1;

                // Generar código de barras automático
                string codigoBarrasGenerado = GenerarCodigoEjemplar(_material.IdMaterial, proximoNumero);

                // Crear formulario de diálogo para agregar ejemplar
                Form dialogForm = new Form();
                dialogForm.Text = LanguageManager.Translate("agregar_ejemplar");
                dialogForm.Size = new Size(500, 330);
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogForm.MaximizeBox = false;
                dialogForm.MinimizeBox = false;
                EstilosSistema.AplicarEstiloFormulario(dialogForm);

                // Controles
                Label lblNumero = new Label { Text = LanguageManager.Translate("numero_ejemplar") + ":", Location = new Point(20, 20), AutoSize = true };
                TextBox txtNumero = new TextBox { Text = proximoNumero.ToString(), Location = new Point(20, 45), Width = 440, ReadOnly = true };

                Label lblCodigo = new Label { Text = LanguageManager.Translate("codigo_barras") + ":", Location = new Point(20, 80), AutoSize = true };
                TextBox txtCodigo = new TextBox { Text = codigoBarrasGenerado, Location = new Point(20, 105), Width = 340 };
                Button btnGenerarCodigo = new Button { Text = "Generar", Location = new Point(370, 103), Width = 90, Height = 27 };

                Label lblUbicacion = new Label { Text = LanguageManager.Translate("ubicacion") + ":", Location = new Point(20, 140), AutoSize = true };
                TextBox txtUbicacion = new TextBox { Location = new Point(20, 165), Width = 440 };

                Button btnGuardar = new Button { Text = LanguageManager.Translate("guardar"), Location = new Point(250, 240), Width = 100, Height = 35 };
                Button btnCancelar = new Button { Text = LanguageManager.Translate("cancelar"), Location = new Point(360, 240), Width = 100, Height = 35 };

                // Aplicar estilos
                EstilosSistema.AplicarEstiloLabel(lblNumero);
                EstilosSistema.AplicarEstiloLabel(lblCodigo);
                EstilosSistema.AplicarEstiloLabel(lblUbicacion);
                EstilosSistema.AplicarEstiloTextBox(txtNumero);
                EstilosSistema.AplicarEstiloTextBox(txtCodigo);
                EstilosSistema.AplicarEstiloTextBox(txtUbicacion);
                EstilosSistema.AplicarEstiloBotonSecundario(btnGenerarCodigo);
                EstilosSistema.AplicarEstiloBotonPrimario(btnGuardar);
                EstilosSistema.AplicarEstiloBotonSecundario(btnCancelar);

                // Evento para regenerar código
                btnGenerarCodigo.Click += (s, ev) =>
                {
                    txtCodigo.Text = GenerarCodigoEjemplar(_material.IdMaterial, proximoNumero);
                };

                // Eventos
                btnGuardar.Click += (s, ev) =>
                {
                    try
                    {
                        // Validar que el código de barras no esté vacío
                        if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                        {
                            MessageBox.Show("El código de barras es obligatorio",
                                LanguageManager.Translate("validacion"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        // Validar que el código de barras sea único
                        if (ValidarCodigoEjemplarUnico(txtCodigo.Text.Trim(), Guid.Empty))
                        {
                            MessageBox.Show("El código de barras ya existe. Por favor ingrese uno diferente o genere uno nuevo.",
                                LanguageManager.Translate("validacion"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        Ejemplar nuevoEjemplar = new Ejemplar
                        {
                            IdMaterial = _material.IdMaterial,
                            NumeroEjemplar = proximoNumero,
                            CodigoEjemplar = txtCodigo.Text.Trim(),
                            Ubicacion = txtUbicacion.Text.Trim(),
                            Observaciones = string.Empty,
                            Estado = EstadoMaterial.Disponible
                        };

                        _ejemplarBLL.GuardarEjemplar(nuevoEjemplar);

                        MessageBox.Show(LanguageManager.Translate("ejemplar_guardado"),
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        dialogForm.DialogResult = DialogResult.OK;
                        dialogForm.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancelar.Click += (s, ev) => dialogForm.Close();

                // Agregar controles
                dialogForm.Controls.Add(lblNumero);
                dialogForm.Controls.Add(txtNumero);
                dialogForm.Controls.Add(lblCodigo);
                dialogForm.Controls.Add(txtCodigo);
                dialogForm.Controls.Add(btnGenerarCodigo);
                dialogForm.Controls.Add(lblUbicacion);
                dialogForm.Controls.Add(txtUbicacion);
                dialogForm.Controls.Add(btnGuardar);
                dialogForm.Controls.Add(btnCancelar);

                // Mostrar diálogo
                if (dialogForm.ShowDialog() == DialogResult.OK)
                {
                    CargarEjemplares();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar ejemplar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que haya una fila seleccionada
                if (dgvEjemplares.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_ejemplar"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ejemplar seleccionado
                Guid idEjemplar = (Guid)dgvEjemplares.SelectedRows[0].Cells["IdEjemplar"].Value;
                Ejemplar ejemplar = _ejemplarBLL.ObtenerEjemplarPorId(idEjemplar);

                if (ejemplar == null)
                {
                    MessageBox.Show("No se pudo cargar el ejemplar",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crear formulario de diálogo para editar ejemplar
                Form dialogForm = new Form();
                dialogForm.Text = LanguageManager.Translate("editar");
                dialogForm.Size = new Size(500, 330);
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogForm.MaximizeBox = false;
                dialogForm.MinimizeBox = false;
                EstilosSistema.AplicarEstiloFormulario(dialogForm);

                // Controles
                Label lblNumero = new Label { Text = LanguageManager.Translate("numero_ejemplar") + ":", Location = new Point(20, 20), AutoSize = true };
                TextBox txtNumero = new TextBox { Text = ejemplar.NumeroEjemplar.ToString(), Location = new Point(20, 45), Width = 440, ReadOnly = true };

                Label lblCodigo = new Label { Text = LanguageManager.Translate("codigo_barras") + ":", Location = new Point(20, 80), AutoSize = true };
                TextBox txtCodigo = new TextBox { Text = ejemplar.CodigoEjemplar, Location = new Point(20, 105), Width = 340 };
                Button btnGenerarCodigoEdit = new Button { Text = "Generar", Location = new Point(370, 103), Width = 90, Height = 27 };

                Label lblUbicacion = new Label { Text = LanguageManager.Translate("ubicacion") + ":", Location = new Point(20, 140), AutoSize = true };
                TextBox txtUbicacion = new TextBox { Text = ejemplar.Ubicacion, Location = new Point(20, 165), Width = 440 };

                Button btnGuardar = new Button { Text = LanguageManager.Translate("guardar_cambios"), Location = new Point(250, 240), Width = 100, Height = 35 };
                Button btnCancelar = new Button { Text = LanguageManager.Translate("cancelar"), Location = new Point(360, 240), Width = 100, Height = 35 };

                // Aplicar estilos
                EstilosSistema.AplicarEstiloLabel(lblNumero);
                EstilosSistema.AplicarEstiloLabel(lblCodigo);
                EstilosSistema.AplicarEstiloLabel(lblUbicacion);
                EstilosSistema.AplicarEstiloTextBox(txtNumero);
                EstilosSistema.AplicarEstiloTextBox(txtCodigo);
                EstilosSistema.AplicarEstiloTextBox(txtUbicacion);
                EstilosSistema.AplicarEstiloBotonSecundario(btnGenerarCodigoEdit);
                EstilosSistema.AplicarEstiloBotonPrimario(btnGuardar);
                EstilosSistema.AplicarEstiloBotonSecundario(btnCancelar);

                // Evento para regenerar código
                btnGenerarCodigoEdit.Click += (s, ev) =>
                {
                    txtCodigo.Text = GenerarCodigoEjemplar(_material.IdMaterial, ejemplar.NumeroEjemplar);
                };

                // Eventos
                btnGuardar.Click += (s, ev) =>
                {
                    try
                    {
                        // Validar que el código de barras no esté vacío
                        if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                        {
                            MessageBox.Show("El código de barras es obligatorio",
                                LanguageManager.Translate("validacion"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        // Validar que el código de barras sea único (excluyendo el ejemplar actual)
                        if (ValidarCodigoEjemplarUnico(txtCodigo.Text.Trim(), ejemplar.IdEjemplar))
                        {
                            MessageBox.Show("El código de barras ya existe. Por favor ingrese uno diferente o genere uno nuevo.",
                                LanguageManager.Translate("validacion"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        ejemplar.CodigoEjemplar = txtCodigo.Text.Trim();
                        ejemplar.Ubicacion = txtUbicacion.Text.Trim();
                        ejemplar.Observaciones = string.Empty;

                        _ejemplarBLL.ActualizarEjemplar(ejemplar);

                        MessageBox.Show(LanguageManager.Translate("ejemplar_guardado"),
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        dialogForm.DialogResult = DialogResult.OK;
                        dialogForm.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancelar.Click += (s, ev) => dialogForm.Close();

                // Agregar controles
                dialogForm.Controls.Add(lblNumero);
                dialogForm.Controls.Add(txtNumero);
                dialogForm.Controls.Add(lblCodigo);
                dialogForm.Controls.Add(txtCodigo);
                dialogForm.Controls.Add(btnGenerarCodigoEdit);
                dialogForm.Controls.Add(lblUbicacion);
                dialogForm.Controls.Add(txtUbicacion);
                dialogForm.Controls.Add(btnGuardar);
                dialogForm.Controls.Add(btnCancelar);

                // Mostrar diálogo
                if (dialogForm.ShowDialog() == DialogResult.OK)
                {
                    CargarEjemplares();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar ejemplar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCambiarEstado_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que haya una fila seleccionada
                if (dgvEjemplares.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_ejemplar"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ejemplar seleccionado
                Guid idEjemplar = (Guid)dgvEjemplares.SelectedRows[0].Cells["IdEjemplar"].Value;
                Ejemplar ejemplar = _ejemplarBLL.ObtenerEjemplarPorId(idEjemplar);

                if (ejemplar == null)
                {
                    MessageBox.Show("No se pudo cargar el ejemplar",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // VALIDACIÓN: No permitir cambiar estado si está prestado
                if (ejemplar.Estado == EstadoMaterial.Prestado)
                {
                    MessageBox.Show(
                        "No se puede cambiar el estado de un ejemplar prestado.\n\n" +
                        "Para cambiar su estado, primero debe registrarse la devolución del préstamo.",
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Crear formulario de diálogo para cambiar estado
                Form dialogForm = new Form();
                dialogForm.Text = LanguageManager.Translate("cambiar_estado");
                dialogForm.Size = new Size(400, 280);
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogForm.MaximizeBox = false;
                dialogForm.MinimizeBox = false;
                EstilosSistema.AplicarEstiloFormulario(dialogForm);

                // Controles
                Label lblEstado = new Label { Text = LanguageManager.Translate("seleccionar_estado_nuevo") + ":", Location = new Point(20, 20), AutoSize = true };
                ComboBox cmbEstado = new ComboBox { Location = new Point(20, 45), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };

                // IMPORTANTE: Solo cargar estados que se pueden asignar manualmente
                // El estado "Prestado" solo se asigna automáticamente al registrar un préstamo
                cmbEstado.Items.Add(LanguageManager.Translate("estado_disponible"));
                cmbEstado.Items.Add(LanguageManager.Translate("estado_en_reparacion"));
                cmbEstado.Items.Add(LanguageManager.Translate("estado_no_disponible"));

                // Mensaje informativo
                Label lblInfo = new Label
                {
                    Text = "Nota: El estado 'Prestado' solo se asigna\nautomáticamente al registrar un préstamo.",
                    Location = new Point(20, 80),
                    AutoSize = true,
                    ForeColor = System.Drawing.Color.FromArgb(127, 140, 141),
                    Font = new Font("Segoe UI", 8.25F, FontStyle.Italic)
                };

                // Seleccionar el estado actual si es uno de los permitidos
                string estadoActualTraducido = TraducirEstado(ejemplar.Estado);
                if (cmbEstado.Items.Contains(estadoActualTraducido))
                {
                    cmbEstado.SelectedItem = estadoActualTraducido;
                }
                else
                {
                    // Si el estado actual no está en la lista (porque es "Prestado"), seleccionar el primero
                    cmbEstado.SelectedIndex = 0;
                }

                Button btnGuardar = new Button { Text = LanguageManager.Translate("guardar_cambios"), Location = new Point(150, 190), Width = 100, Height = 35 };
                Button btnCancelar = new Button { Text = LanguageManager.Translate("cancelar"), Location = new Point(260, 190), Width = 100, Height = 35 };

                // Aplicar estilos
                EstilosSistema.AplicarEstiloLabel(lblEstado);
                EstilosSistema.AplicarEstiloComboBox(cmbEstado);
                EstilosSistema.AplicarEstiloBotonPrimario(btnGuardar);
                EstilosSistema.AplicarEstiloBotonSecundario(btnCancelar);

                // Eventos
                btnGuardar.Click += (s, ev) =>
                {
                    try
                    {
                        if (cmbEstado.SelectedItem == null)
                        {
                            MessageBox.Show("Debe seleccionar un estado",
                                LanguageManager.Translate("validacion"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        EstadoMaterial nuevoEstado = ObtenerEstadoDesdeTexto(cmbEstado.SelectedItem.ToString());

                        // Validación adicional: Nunca permitir asignar "Prestado" manualmente
                        if (nuevoEstado == EstadoMaterial.Prestado)
                        {
                            MessageBox.Show(
                                "El estado 'Prestado' no puede asignarse manualmente.\n" +
                                "Este estado se asigna automáticamente al registrar un préstamo.",
                                LanguageManager.Translate("validacion"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        _ejemplarBLL.CambiarEstado(idEjemplar, nuevoEstado);

                        MessageBox.Show(LanguageManager.Translate("estado_actualizado"),
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        dialogForm.DialogResult = DialogResult.OK;
                        dialogForm.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cambiar estado: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancelar.Click += (s, ev) => dialogForm.Close();

                // Agregar controles
                dialogForm.Controls.Add(lblEstado);
                dialogForm.Controls.Add(cmbEstado);
                dialogForm.Controls.Add(lblInfo);
                dialogForm.Controls.Add(btnGuardar);
                dialogForm.Controls.Add(btnCancelar);

                // Mostrar diálogo
                if (dialogForm.ShowDialog() == DialogResult.OK)
                {
                    CargarEjemplares();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar estado: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que haya una fila seleccionada
                if (dgvEjemplares.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("seleccione_ejemplar"),
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el ejemplar seleccionado
                Guid idEjemplar = (Guid)dgvEjemplares.SelectedRows[0].Cells["IdEjemplar"].Value;
                Ejemplar ejemplar = _ejemplarBLL.ObtenerEjemplarPorId(idEjemplar);

                if (ejemplar == null)
                {
                    MessageBox.Show("No se pudo cargar el ejemplar",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirmar eliminación
                DialogResult confirmacion = MessageBox.Show(
                    LanguageManager.Translate("confirmar_eliminacion_ejemplar"),
                    LanguageManager.Translate("eliminar"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    _ejemplarBLL.EliminarEjemplar(ejemplar);

                    MessageBox.Show(LanguageManager.Translate("ejemplar_eliminado"),
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    CargarEjemplares();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar ejemplar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            // Devolver OK para que se actualice la ventana de consultar material
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Genera un código de barras único para un ejemplar
        /// Formato: BIB-{últimos 8 caracteres del IdMaterial}-{NumeroEjemplar:000}
        /// </summary>
        private string GenerarCodigoEjemplar(Guid idMaterial, int numeroEjemplar)
        {
            // Tomar los últimos 8 caracteres del GUID (sin guiones)
            string idCorto = idMaterial.ToString("N").Substring(24, 8).ToUpper();

            // Formatear el número de ejemplar con 3 dígitos
            string numeroFormateado = numeroEjemplar.ToString("D3");

            // Generar código: BIB-XXXXXXXX-NNN
            return $"BIB-{idCorto}-{numeroFormateado}";
        }

        /// <summary>
        /// Valida que un código de barras sea único en toda la biblioteca
        /// </summary>
        /// <param name="codigoBarras">Código a validar</param>
        /// <param name="idEjemplarActual">ID del ejemplar actual (para edición), Guid.Empty para nuevo</param>
        /// <returns>True si el código ya existe, False si es único</returns>
        private bool ValidarCodigoEjemplarUnico(string codigoBarras, Guid idEjemplarActual)
        {
            try
            {
                // Obtener todos los ejemplares de este material
                List<Ejemplar> ejemplares = _ejemplarBLL.ObtenerEjemplaresPorMaterial(_material.IdMaterial);

                // Verificar si existe otro ejemplar con el mismo código
                return ejemplares.Any(ej =>
                    ej.CodigoEjemplar != null &&
                    ej.CodigoEjemplar.Equals(codigoBarras, StringComparison.OrdinalIgnoreCase) &&
                    ej.IdEjemplar != idEjemplarActual);
            }
            catch
            {
                // En caso de error, asumir que no es único (seguridad)
                return false;
            }
        }

        #endregion

        #region Permission Management

        private void ConfigurarPermisosEdicion()
        {
            // Verificar si el usuario tiene el permiso Gestionar Ejemplares
            bool tienePermiso = TienePermiso("Gestionar Ejemplares");

            if (!tienePermiso)
            {
                // Deshabilitar botones de acción
                btnAgregar.Enabled = false;
                btnEditar.Enabled = false;
                btnCambiarEstado.Enabled = false;
                btnEliminar.Enabled = false;

                // Mostrar mensaje informativo
                MessageBox.Show(
                    "No tiene permisos para gestionar ejemplares.\nEl formulario se abrirá en modo de solo lectura.",
                    "Acceso Limitado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private bool TienePermiso(string nombrePatente)
        {
            if (_usuarioLogueado?.Permisos == null)
                return false;

            foreach (var componente in _usuarioLogueado.Permisos)
            {
                if (TienePermisoRecursivo(componente, nombrePatente))
                    return true;
            }

            return false;
        }

        private bool TienePermisoRecursivo(ServicesSecurity.DomainModel.Security.Composite.Component componente, string nombrePatente)
        {
            if (componente == null)
                return false;

            // Si es una Patente, verificar si coincide con el nombre
            if (componente is Patente patente)
            {
                return patente.MenuItemName != null && patente.MenuItemName.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase);
            }

            // Si es una Familia, buscar recursivamente en sus hijos
            if (componente is Familia familia)
            {
                foreach (var hijo in familia.GetChildrens())
                {
                    if (hijo != null && TienePermisoRecursivo(hijo, nombrePatente))
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}
