using DomainModel;
using DomainModel.Enums;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using System;
using System.Windows.Forms;
using UI.Helpers;
using BLL;

namespace UI.WinUi.Administrador
{
    public partial class EditarMaterial : Form
    {
        private Usuario _usuarioLogueado;
        private Material _materialActual;
        private MaterialBLL _materialBLL;

        public EditarMaterial()
        {
            InitializeComponent();
            _materialBLL = new MaterialBLL();
        }

        public EditarMaterial(Usuario usuario, Material material) : this()
        {
            _usuarioLogueado = usuario;
            _materialActual = material;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += EditarMaterial_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnVolver.Click += BtnVolver_Click;
            txtAnioPublicacion.KeyPress += TxtAnioPublicacion_KeyPress;
            txtAnioPublicacion.Leave += TxtAnioPublicacion_Leave;

            // Aplicar estilos del sistema
            AplicarEstilos();

            // Cargar enums en los ComboBox
            CargarTiposMaterial();
            CargarGeneros();
            CargarNiveles();

            // Cargar datos del material
            CargarDatosMaterial();

            // Verificar permisos de edición
            VerificarPermisosEdicion();
        }

        private void EditarMaterial_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = "Editar Material";
                groupBoxDatosMaterial.Text = LanguageManager.Translate("datos_material");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                lblTitulo.Text = LanguageManager.Translate("titulo");
                lblAutor.Text = LanguageManager.Translate("autor");
                lblTipo.Text = LanguageManager.Translate("tipo");
                lblGenero.Text = LanguageManager.Translate("genero");
                lblISBN.Text = "ISBN:";
                lblEditorial.Text = LanguageManager.Translate("editorial");
                lblAnioPublicacion.Text = LanguageManager.Translate("anio_publicacion");
                lblNivel.Text = "Nivel:";
                lblCantidad.Text = LanguageManager.Translate("cantidad");

                btnGuardar.Text = LanguageManager.Translate("guardar_cambios");
                btnEliminar.Text = LanguageManager.Translate("eliminar");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        #region CRUD Operations

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (!ValidarCampos())
                    return;

                // Actualizar propiedades de _materialActual con los valores del formulario
                _materialActual.Titulo = txtTitulo.Text.Trim();
                _materialActual.Autor = txtAutor.Text.Trim();
                _materialActual.Editorial = txtEditorial.Text.Trim();
                _materialActual.Tipo = (TipoMaterial)((dynamic)comboBoxTipo.SelectedItem).Value;
                _materialActual.Genero = comboBoxGenero.SelectedItem?.ToString();
                _materialActual.ISBN = txtISBN.Text.Trim();
                _materialActual.AnioPublicacion = int.TryParse(txtAnioPublicacion.Text, out int anio) ? (int?)anio : null;
                _materialActual.EdadRecomendada = comboBoxNivel.SelectedItem?.ToString();

                // Calcular la diferencia de cantidad para ajustar disponibles
                int diferenciaCantidad = (int)numCantidad.Value - _materialActual.CantidadTotal;
                _materialActual.CantidadTotal = (int)numCantidad.Value;
                _materialActual.CantidadDisponible += diferenciaCantidad;

                // Asegurar que disponibles no sea negativo
                if (_materialActual.CantidadDisponible < 0)
                    _materialActual.CantidadDisponible = 0;

                // Guardar en la base de datos
                _materialBLL.ActualizarMaterial(_materialActual);

                MessageBox.Show("Material actualizado exitosamente",
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cambios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show(
                    $"¿Está seguro que desea eliminar el material '{_materialActual.Titulo}'?\n\nEsta acción marcará el material como inactivo.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Eliminar (borrado lógico)
                    _materialBLL.EliminarMaterial(_materialActual);

                    MessageBox.Show("Material eliminado exitosamente",
                        LanguageManager.Translate("exito"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Helper Methods

        private void VerificarPermisosEdicion()
        {
            // Verificar si el usuario tiene el permiso EditarMaterial
            bool tienePermisoEditar = TienePermiso("EditarMaterial");

            if (!tienePermisoEditar)
            {
                // Deshabilitar todos los controles de edición
                txtTitulo.ReadOnly = true;
                txtAutor.ReadOnly = true;
                txtISBN.ReadOnly = true;
                txtEditorial.ReadOnly = true;
                txtAnioPublicacion.ReadOnly = true;
                comboBoxTipo.Enabled = false;
                comboBoxGenero.Enabled = false;
                comboBoxNivel.Enabled = false;
                numCantidad.Enabled = false;

                // Deshabilitar botones de acción
                btnGuardar.Enabled = false;
                btnEliminar.Enabled = false;

                // Cambiar el título del formulario
                this.Text = "Ver Material (Solo Lectura)";

                // Mostrar mensaje informativo
                MessageBox.Show(
                    "No tiene permisos para editar materiales.\nEl formulario se abrirá en modo de solo lectura.",
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

        private void AplicarEstilos()
        {
            // Aplicar estilo al formulario
            EstilosSistema.AplicarEstiloFormulario(this);

            // Aplicar estilos a los GroupBox
            EstilosSistema.AplicarEstiloGroupBox(groupBoxDatosMaterial);
            EstilosSistema.AplicarEstiloGroupBox(groupBoxAcciones);

            // Aplicar estilos a los Labels
            EstilosSistema.AplicarEstiloLabel(lblTitulo);
            EstilosSistema.AplicarEstiloLabel(lblAutor);
            EstilosSistema.AplicarEstiloLabel(lblTipo);
            EstilosSistema.AplicarEstiloLabel(lblGenero);
            EstilosSistema.AplicarEstiloLabel(lblISBN);
            EstilosSistema.AplicarEstiloLabel(lblEditorial);
            EstilosSistema.AplicarEstiloLabel(lblAnioPublicacion);
            EstilosSistema.AplicarEstiloLabel(lblNivel);
            EstilosSistema.AplicarEstiloLabel(lblCantidad);

            // Aplicar estilos a los TextBox
            EstilosSistema.AplicarEstiloTextBox(txtTitulo);
            EstilosSistema.AplicarEstiloTextBox(txtAutor);
            EstilosSistema.AplicarEstiloTextBox(txtISBN);
            EstilosSistema.AplicarEstiloTextBox(txtEditorial);
            EstilosSistema.AplicarEstiloTextBox(txtAnioPublicacion);

            // Aplicar estilos a los ComboBox
            EstilosSistema.AplicarEstiloComboBox(comboBoxTipo);
            EstilosSistema.AplicarEstiloComboBox(comboBoxGenero);
            EstilosSistema.AplicarEstiloComboBox(comboBoxNivel);

            // Aplicar estilos a los botones
            EstilosSistema.AplicarEstiloBotonPrimario(btnGuardar);
            EstilosSistema.AplicarEstiloBotonSecundario(btnEliminar);
            EstilosSistema.AplicarEstiloBotonSecundario(btnVolver);
        }

        private void CargarTiposMaterial()
        {
            comboBoxTipo.Items.Clear();
            comboBoxTipo.DisplayMember = "Text";
            comboBoxTipo.ValueMember = "Value";

            var items = new[] {
                new { Text = "Libro", Value = TipoMaterial.Libro },
                new { Text = "Manual", Value = TipoMaterial.Manual },
                new { Text = "Revista", Value = TipoMaterial.Revista }
            };

            foreach (var item in items)
            {
                comboBoxTipo.Items.Add(item);
            }
        }

        private void CargarGeneros()
        {
            comboBoxGenero.Items.Clear();

            var generos = new[] {
                "Fantasía",
                "Ciencia Ficción",
                "Aventura",
                "Misterio",
                "Romance",
                "Terror",
                "Histórico",
                "Educativo",
                "Biografía",
                "Poesía",
                "Drama",
                "Comedia",
                "Infantil",
                "Juvenil",
                "Técnico",
                "Científico",
                "Otro"
            };

            foreach (var genero in generos)
            {
                comboBoxGenero.Items.Add(genero);
            }
        }

        private void CargarNiveles()
        {
            comboBoxNivel.Items.Clear();

            var niveles = new[] {
                "Inicial",
                "Primario",
                "Secundario",
                "Universitario",
                "Todos"
            };

            foreach (var nivel in niveles)
            {
                comboBoxNivel.Items.Add(nivel);
            }
        }

        private void CargarDatosMaterial()
        {
            if (_materialActual == null)
                return;

            txtTitulo.Text = _materialActual.Titulo;
            txtAutor.Text = _materialActual.Autor;
            txtISBN.Text = _materialActual.ISBN;
            txtEditorial.Text = _materialActual.Editorial;
            txtAnioPublicacion.Text = _materialActual.AnioPublicacion?.ToString();
            numCantidad.Value = _materialActual.CantidadTotal;

            // Seleccionar tipo
            for (int i = 0; i < comboBoxTipo.Items.Count; i++)
            {
                var item = comboBoxTipo.Items[i];
                var tipo = item.GetType().GetProperty("Value").GetValue(item, null);
                if ((TipoMaterial)tipo == _materialActual.Tipo)
                {
                    comboBoxTipo.SelectedIndex = i;
                    break;
                }
            }

            // Seleccionar género
            int generoIndex = comboBoxGenero.Items.IndexOf(_materialActual.Genero);
            if (generoIndex >= 0)
                comboBoxGenero.SelectedIndex = generoIndex;

            // Seleccionar nivel
            int nivelIndex = comboBoxNivel.Items.IndexOf(_materialActual.EdadRecomendada);
            if (nivelIndex >= 0)
                comboBoxNivel.SelectedIndex = nivelIndex;
        }

        private bool ValidarCampos()
        {
            // Validar Título (obligatorio, mínimo 2 caracteres)
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return false;
            }

            if (txtTitulo.Text.Trim().Length < 2)
            {
                MessageBox.Show("El título debe tener al menos 2 caracteres", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return false;
            }

            // Validar Autor (obligatorio, mínimo 2 caracteres)
            if (string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                MessageBox.Show("El autor es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAutor.Focus();
                return false;
            }

            if (txtAutor.Text.Trim().Length < 2)
            {
                MessageBox.Show("El autor debe tener al menos 2 caracteres", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAutor.Focus();
                return false;
            }

            // Validar Tipo (obligatorio)
            if (comboBoxTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un tipo de material", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxTipo.Focus();
                return false;
            }

            // Validar Género (obligatorio)
            if (comboBoxGenero.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un género", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxGenero.Focus();
                return false;
            }

            // Validar Editorial (opcional, pero si se ingresa mínimo 2 caracteres)
            if (!string.IsNullOrWhiteSpace(txtEditorial.Text) && txtEditorial.Text.Trim().Length < 2)
            {
                MessageBox.Show("La editorial debe tener al menos 2 caracteres", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEditorial.Focus();
                return false;
            }

            // Validar Año de Publicación (obligatorio, 4 dígitos, rango válido)
            if (string.IsNullOrWhiteSpace(txtAnioPublicacion.Text))
            {
                MessageBox.Show("El año de publicación es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnioPublicacion.Focus();
                return false;
            }

            if (txtAnioPublicacion.Text.Length != 4)
            {
                MessageBox.Show("El año de publicación debe tener 4 dígitos", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnioPublicacion.Focus();
                return false;
            }

            if (int.TryParse(txtAnioPublicacion.Text, out int anio))
            {
                if (anio < 1900 || anio > DateTime.Now.Year + 1)
                {
                    MessageBox.Show($"El año de publicación debe estar entre 1900 y {DateTime.Now.Year + 1}", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAnioPublicacion.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("El año de publicación debe ser un número válido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnioPublicacion.Focus();
                return false;
            }

            // Validar Nivel (obligatorio)
            if (comboBoxNivel.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un nivel educativo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxNivel.Focus();
                return false;
            }

            // Validar Cantidad (debe ser mayor a 0)
            if (numCantidad.Value < 1)
            {
                MessageBox.Show("La cantidad debe ser al menos 1", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numCantidad.Focus();
                return false;
            }

            return true;
        }

        private void TxtAnioPublicacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números y teclas de control (backspace, delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtAnioPublicacion_Leave(object sender, EventArgs e)
        {
            // Validar que el año tenga 4 dígitos
            if (!string.IsNullOrWhiteSpace(txtAnioPublicacion.Text))
            {
                if (txtAnioPublicacion.Text.Length != 4)
                {
                    MessageBox.Show(
                        "El año de publicación debe tener exactamente 4 dígitos.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtAnioPublicacion.Focus();
                    return;
                }

                // Validar que sea un año razonable
                if (int.TryParse(txtAnioPublicacion.Text, out int anio))
                {
                    if (anio < 1900 || anio > 2100)
                    {
                        MessageBox.Show(
                            "El año de publicación debe estar entre 1900 y 2100.",
                            "Validación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        txtAnioPublicacion.Focus();
                    }
                }
            }
        }

        #endregion
    }
}
