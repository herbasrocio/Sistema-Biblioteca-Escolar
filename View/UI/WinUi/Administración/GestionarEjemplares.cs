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

        public GestionarEjemplares()
        {
            InitializeComponent();
            _ejemplarBLL = new EjemplarBLL();
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

                List<Ejemplar> ejemplares = _ejemplarBLL.ObtenerEjemplaresPorMaterial(_material.IdMaterial);

                // Crear DataTable para mostrar en el DataGridView
                DataTable dt = new DataTable();
                dt.Columns.Add("IdEjemplar", typeof(Guid));
                dt.Columns.Add("NumeroEjemplar", typeof(int));
                dt.Columns.Add("CodigoBarras", typeof(string));
                dt.Columns.Add("Estado", typeof(string));
                dt.Columns.Add("Ubicacion", typeof(string));
                dt.Columns.Add("Observaciones", typeof(string));

                foreach (Ejemplar ejemplar in ejemplares)
                {
                    DataRow row = dt.NewRow();
                    row["IdEjemplar"] = ejemplar.IdEjemplar;
                    row["NumeroEjemplar"] = ejemplar.NumeroEjemplar;
                    row["CodigoBarras"] = ejemplar.CodigoBarras ?? "";
                    row["Estado"] = TraducirEstado(ejemplar.Estado);
                    row["Ubicacion"] = ejemplar.Ubicacion ?? "";
                    row["Observaciones"] = ejemplar.Observaciones ?? "";
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

        private void ConfigurarColumnasDataGridView()
        {
            if (dgvEjemplares.Columns.Count == 0)
                return;

            // Ocultar columna ID
            dgvEjemplares.Columns["IdEjemplar"].Visible = false;

            // Configurar encabezados
            dgvEjemplares.Columns["NumeroEjemplar"].HeaderText = LanguageManager.Translate("numero_ejemplar");
            dgvEjemplares.Columns["CodigoBarras"].HeaderText = LanguageManager.Translate("codigo_barras");
            dgvEjemplares.Columns["Estado"].HeaderText = LanguageManager.Translate("estado");
            dgvEjemplares.Columns["Ubicacion"].HeaderText = LanguageManager.Translate("ubicacion");
            dgvEjemplares.Columns["Observaciones"].HeaderText = LanguageManager.Translate("observaciones");

            // Configurar anchos
            dgvEjemplares.Columns["NumeroEjemplar"].Width = 100;
            dgvEjemplares.Columns["CodigoBarras"].Width = 150;
            dgvEjemplares.Columns["Estado"].Width = 120;
            dgvEjemplares.Columns["Ubicacion"].Width = 150;
            dgvEjemplares.Columns["Observaciones"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

                // Crear formulario de diálogo para agregar ejemplar
                Form dialogForm = new Form();
                dialogForm.Text = LanguageManager.Translate("agregar_ejemplar");
                dialogForm.Size = new Size(500, 400);
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogForm.MaximizeBox = false;
                dialogForm.MinimizeBox = false;
                EstilosSistema.AplicarEstiloFormulario(dialogForm);

                // Controles
                Label lblNumero = new Label { Text = LanguageManager.Translate("numero_ejemplar") + ":", Location = new Point(20, 20), AutoSize = true };
                TextBox txtNumero = new TextBox { Text = proximoNumero.ToString(), Location = new Point(20, 45), Width = 440, ReadOnly = true };

                Label lblCodigo = new Label { Text = LanguageManager.Translate("codigo_barras") + ":", Location = new Point(20, 80), AutoSize = true };
                TextBox txtCodigo = new TextBox { Location = new Point(20, 105), Width = 440 };

                Label lblUbicacion = new Label { Text = LanguageManager.Translate("ubicacion") + ":", Location = new Point(20, 140), AutoSize = true };
                TextBox txtUbicacion = new TextBox { Location = new Point(20, 165), Width = 440 };

                Label lblObservaciones = new Label { Text = LanguageManager.Translate("observaciones") + ":", Location = new Point(20, 200), AutoSize = true };
                TextBox txtObservaciones = new TextBox { Location = new Point(20, 225), Width = 440, Height = 60, Multiline = true };

                Button btnGuardar = new Button { Text = LanguageManager.Translate("guardar"), Location = new Point(250, 310), Width = 100, Height = 35 };
                Button btnCancelar = new Button { Text = LanguageManager.Translate("cancelar"), Location = new Point(360, 310), Width = 100, Height = 35 };

                // Aplicar estilos
                EstilosSistema.AplicarEstiloLabel(lblNumero);
                EstilosSistema.AplicarEstiloLabel(lblCodigo);
                EstilosSistema.AplicarEstiloLabel(lblUbicacion);
                EstilosSistema.AplicarEstiloLabel(lblObservaciones);
                EstilosSistema.AplicarEstiloTextBox(txtNumero);
                EstilosSistema.AplicarEstiloTextBox(txtCodigo);
                EstilosSistema.AplicarEstiloTextBox(txtUbicacion);
                EstilosSistema.AplicarEstiloTextBox(txtObservaciones);
                EstilosSistema.AplicarEstiloBotonPrimario(btnGuardar);
                EstilosSistema.AplicarEstiloBotonSecundario(btnCancelar);

                // Eventos
                btnGuardar.Click += (s, ev) =>
                {
                    try
                    {
                        Ejemplar nuevoEjemplar = new Ejemplar
                        {
                            IdMaterial = _material.IdMaterial,
                            NumeroEjemplar = proximoNumero,
                            CodigoBarras = txtCodigo.Text.Trim(),
                            Ubicacion = txtUbicacion.Text.Trim(),
                            Observaciones = txtObservaciones.Text.Trim(),
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
                dialogForm.Controls.Add(lblUbicacion);
                dialogForm.Controls.Add(txtUbicacion);
                dialogForm.Controls.Add(lblObservaciones);
                dialogForm.Controls.Add(txtObservaciones);
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
                dialogForm.Size = new Size(500, 400);
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogForm.MaximizeBox = false;
                dialogForm.MinimizeBox = false;
                EstilosSistema.AplicarEstiloFormulario(dialogForm);

                // Controles
                Label lblNumero = new Label { Text = LanguageManager.Translate("numero_ejemplar") + ":", Location = new Point(20, 20), AutoSize = true };
                TextBox txtNumero = new TextBox { Text = ejemplar.NumeroEjemplar.ToString(), Location = new Point(20, 45), Width = 440, ReadOnly = true };

                Label lblCodigo = new Label { Text = LanguageManager.Translate("codigo_barras") + ":", Location = new Point(20, 80), AutoSize = true };
                TextBox txtCodigo = new TextBox { Text = ejemplar.CodigoBarras, Location = new Point(20, 105), Width = 440 };

                Label lblUbicacion = new Label { Text = LanguageManager.Translate("ubicacion") + ":", Location = new Point(20, 140), AutoSize = true };
                TextBox txtUbicacion = new TextBox { Text = ejemplar.Ubicacion, Location = new Point(20, 165), Width = 440 };

                Label lblObservaciones = new Label { Text = LanguageManager.Translate("observaciones") + ":", Location = new Point(20, 200), AutoSize = true };
                TextBox txtObservaciones = new TextBox { Text = ejemplar.Observaciones, Location = new Point(20, 225), Width = 440, Height = 60, Multiline = true };

                Button btnGuardar = new Button { Text = LanguageManager.Translate("guardar_cambios"), Location = new Point(250, 310), Width = 100, Height = 35 };
                Button btnCancelar = new Button { Text = LanguageManager.Translate("cancelar"), Location = new Point(360, 310), Width = 100, Height = 35 };

                // Aplicar estilos
                EstilosSistema.AplicarEstiloLabel(lblNumero);
                EstilosSistema.AplicarEstiloLabel(lblCodigo);
                EstilosSistema.AplicarEstiloLabel(lblUbicacion);
                EstilosSistema.AplicarEstiloLabel(lblObservaciones);
                EstilosSistema.AplicarEstiloTextBox(txtNumero);
                EstilosSistema.AplicarEstiloTextBox(txtCodigo);
                EstilosSistema.AplicarEstiloTextBox(txtUbicacion);
                EstilosSistema.AplicarEstiloTextBox(txtObservaciones);
                EstilosSistema.AplicarEstiloBotonPrimario(btnGuardar);
                EstilosSistema.AplicarEstiloBotonSecundario(btnCancelar);

                // Eventos
                btnGuardar.Click += (s, ev) =>
                {
                    try
                    {
                        ejemplar.CodigoBarras = txtCodigo.Text.Trim();
                        ejemplar.Ubicacion = txtUbicacion.Text.Trim();
                        ejemplar.Observaciones = txtObservaciones.Text.Trim();

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
                dialogForm.Controls.Add(lblUbicacion);
                dialogForm.Controls.Add(txtUbicacion);
                dialogForm.Controls.Add(lblObservaciones);
                dialogForm.Controls.Add(txtObservaciones);
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

                // Crear formulario de diálogo para cambiar estado
                Form dialogForm = new Form();
                dialogForm.Text = LanguageManager.Translate("cambiar_estado");
                dialogForm.Size = new Size(400, 200);
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogForm.MaximizeBox = false;
                dialogForm.MinimizeBox = false;
                EstilosSistema.AplicarEstiloFormulario(dialogForm);

                // Controles
                Label lblEstado = new Label { Text = LanguageManager.Translate("seleccionar_estado_nuevo") + ":", Location = new Point(20, 20), AutoSize = true };
                ComboBox cmbEstado = new ComboBox { Location = new Point(20, 45), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };

                // Cargar estados
                cmbEstado.Items.Add(LanguageManager.Translate("estado_disponible"));
                cmbEstado.Items.Add(LanguageManager.Translate("estado_prestado"));
                cmbEstado.Items.Add(LanguageManager.Translate("estado_en_reparacion"));
                cmbEstado.Items.Add(LanguageManager.Translate("estado_no_disponible"));

                // Seleccionar el estado actual
                cmbEstado.SelectedItem = TraducirEstado(ejemplar.Estado);

                Button btnGuardar = new Button { Text = LanguageManager.Translate("guardar_cambios"), Location = new Point(150, 110), Width = 100, Height = 35 };
                Button btnCancelar = new Button { Text = LanguageManager.Translate("cancelar"), Location = new Point(260, 110), Width = 100, Height = 35 };

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

        #region Permission Management

        private void ConfigurarPermisosEdicion()
        {
            // Verificar si el usuario tiene el permiso GestionarEjemplares
            bool tienePermiso = TienePermiso("GestionarEjemplares");

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
