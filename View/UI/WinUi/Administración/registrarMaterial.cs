using DomainModel;
using DomainModel.Enums;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using System;
using System.Transactions;
using System.Windows.Forms;
using UI.Helpers;
using BLL;

namespace UI.WinUi.Administrador
{
    public partial class RegistrarMaterial : Form
    {
        private Usuario _usuarioLogueado;
        private MaterialBLL _materialBLL;
        private EjemplarBLL _ejemplarBLL;

        public RegistrarMaterial()
        {
            InitializeComponent();
        }

        public RegistrarMaterial(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _materialBLL = new MaterialBLL();
            _ejemplarBLL = new EjemplarBLL();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += RegistrarMaterial_Load;
            btnGuardar.Click += BtnGuardar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVolver.Click += BtnVolver_Click;
            txtAnioPublicacion.KeyPress += TxtAnioPublicacion_KeyPress;
            txtAnioPublicacion.Leave += TxtAnioPublicacion_Leave;

            // Aplicar estilos del sistema
            AplicarEstilos();

            // Cargar enums en los ComboBox
            CargarTiposMaterial();
            CargarGeneros();
            CargarNiveles();

            // Inicializar con valores por defecto
            LimpiarCampos();
        }

        private void RegistrarMaterial_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("registrar_material");
                groupBoxDatosMaterial.Text = LanguageManager.Translate("datos_material");
                groupBoxAcciones.Text = LanguageManager.Translate("acciones");

                lblTitulo.Text = LanguageManager.Translate("titulo");
                lblAutor.Text = LanguageManager.Translate("autor");
                lblTipo.Text = LanguageManager.Translate("tipo");
                lblGenero.Text = LanguageManager.Translate("genero");
                lblISBN.Text = "ISBN:";
                lblEditorial.Text = LanguageManager.Translate("editorial");
                lblAnioPublicacion.Text = LanguageManager.Translate("anio_publicacion");
                lblNivel.Text = LanguageManager.Translate("nivel") + ":";
                lblCantidad.Text = LanguageManager.Translate("cantidad");
                lblUbicacion.Text = LanguageManager.Translate("ubicacion") + " (" + LanguageManager.Translate("opcional") + "):";

                btnGuardar.Text = LanguageManager.Translate("guardar_material");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnVolver.Text = LanguageManager.Translate("volver");

                // Centrar botones después de cambiar el texto
                CentrarBotones();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        /// <summary>
        /// Centra los botones horizontalmente dentro de su GroupBox contenedor
        /// </summary>
        private void CentrarBotones()
        {
            // Obtener el ancho disponible del contenedor de los botones
            int anchoDisponible = 0;

            // Si los botones están dentro del GroupBox de acciones
            if (btnGuardar.Parent != null)
            {
                anchoDisponible = btnGuardar.Parent.ClientSize.Width;

                // Calcular espacio total ocupado por los 3 botones + espacios entre ellos
                int espacioEntreBotones = 10;
                int anchoTotal = btnGuardar.Width + btnLimpiar.Width + btnVolver.Width + (espacioEntreBotones * 2);

                // Calcular punto de inicio para centrar
                int inicioCentrado = (anchoDisponible - anchoTotal) / 2;

                // Posicionar botones centrados
                btnGuardar.Left = inicioCentrado;
                btnLimpiar.Left = btnGuardar.Right + espacioEntreBotones;
                btnVolver.Left = btnLimpiar.Right + espacioEntreBotones;
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

                // Crear objeto Material con los datos del formulario
                Material nuevoMaterial = new Material
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Autor = txtAutor.Text.Trim(),
                    Editorial = txtEditorial.Text.Trim(),
                    Tipo = (TipoMaterial)((dynamic)comboBoxTipo.SelectedItem).Value,
                    Genero = comboBoxGenero.SelectedItem?.ToString(),
                    ISBN = txtISBN.Text.Trim(),
                    AnioPublicacion = int.TryParse(txtAnioPublicacion.Text, out int anio) ? (int?)anio : null,
                    Nivel = comboBoxNivel.SelectedItem?.ToString(),
                    CantidadTotal = (int)numCantidad.Value,
                    CantidadDisponible = (int)numCantidad.Value // Inicialmente todo está disponible
                };

                string ubicacionGeneral = txtUbicacion.Text.Trim(); // Ubicación opcional para todos los ejemplares
                int cantidadCreada = 0;

                // Usar TransactionScope para garantizar atomicidad
                // Si cualquier operación falla, todo se revierte automáticamente
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        // Guardar el material en la base de datos
                        _materialBLL.GuardarMaterial(nuevoMaterial);

                        // Crear automáticamente los ejemplares según la cantidad especificada
                        for (int i = 1; i <= nuevoMaterial.CantidadTotal; i++)
                        {
                            Ejemplar nuevoEjemplar = new Ejemplar
                            {
                                IdMaterial = nuevoMaterial.IdMaterial,
                                NumeroEjemplar = i,
                                CodigoEjemplar = GenerarCodigoEjemplar(nuevoMaterial.IdMaterial, i),
                                Estado = EstadoMaterial.Disponible,
                                Ubicacion = ubicacionGeneral,
                                Observaciones = string.Empty
                            };

                            _ejemplarBLL.GuardarEjemplar(nuevoEjemplar);
                            cantidadCreada++;
                        }

                        // Si todo salió bien, confirmar la transacción
                        transaction.Complete();

                        MessageBox.Show($"Material registrado exitosamente.\n{cantidadCreada} ejemplar(es) creado(s) automáticamente.",
                            LanguageManager.Translate("exito"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LimpiarCampos();
                        txtTitulo.Focus();
                    }
                    catch (Exception exTransaccion)
                    {
                        // Si hay algún error, la transacción se revierte automáticamente
                        // No se llama a transaction.Complete(), por lo que se hace rollback
                        throw new Exception($"Error durante el registro: {exTransaccion.Message}\n\nLa operación completa ha sido cancelada.", exTransaccion);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            txtTitulo.Focus();
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Helper Methods

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
            EstilosSistema.AplicarEstiloLabel(lblUbicacion);

            // Aplicar estilos a los TextBox
            EstilosSistema.AplicarEstiloTextBox(txtTitulo);
            EstilosSistema.AplicarEstiloTextBox(txtAutor);
            EstilosSistema.AplicarEstiloTextBox(txtISBN);
            EstilosSistema.AplicarEstiloTextBox(txtEditorial);
            EstilosSistema.AplicarEstiloTextBox(txtAnioPublicacion);
            EstilosSistema.AplicarEstiloTextBox(txtUbicacion);

            // Aplicar estilos a los ComboBox
            EstilosSistema.AplicarEstiloComboBox(comboBoxTipo);
            EstilosSistema.AplicarEstiloComboBox(comboBoxGenero);
            EstilosSistema.AplicarEstiloComboBox(comboBoxNivel);

            // Aplicar estilos a los botones
            EstilosSistema.AplicarEstiloBotonSecundario(btnGuardar);
            EstilosSistema.AplicarEstiloBotonSecundario(btnLimpiar);
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
                "Fantasia",
                "Ciencia Ficcion",
                "Aventura",
                "Misterio",
                "Romance",
                "Terror",
                "Historico",
                "Educativo",
                "Biografia",
                "Poesia",
                "Drama",
                "Comedia",
                "Infantil",
                "Juvenil",
                "Tecnico",
                "Cientifico",
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
                "Universitario"
            };

            foreach (var nivel in niveles)
            {
                comboBoxNivel.Items.Add(nivel);
            }
        }

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtISBN.Clear();
            txtEditorial.Clear();
            txtAnioPublicacion.Text = DateTime.Now.Year.ToString();
            txtUbicacion.Clear();
            comboBoxTipo.SelectedIndex = -1;
            comboBoxGenero.SelectedIndex = -1;
            comboBoxNivel.SelectedIndex = -1;
            numCantidad.Value = 1;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                MessageBox.Show("El autor es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAutor.Focus();
                return false;
            }

            if (comboBoxTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un tipo de material", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxTipo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAnioPublicacion.Text) || txtAnioPublicacion.Text.Length != 4)
            {
                MessageBox.Show("El año de publicación debe tener 4 dígitos", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAnioPublicacion.Focus();
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

        /// <summary>
        /// Genera un código de barras único para un ejemplar
        /// Formato: BIB-{8 caracteres del ID del Material}-{NumeroEjemplar con 3 dígitos}
        /// Ejemplo: BIB-73C6CDD0-001
        /// </summary>
        private string GenerarCodigoEjemplar(Guid idMaterial, int numeroEjemplar)
        {
            // Tomar los últimos 8 caracteres del GUID del material (sin guiones)
            string idCorto = idMaterial.ToString("N").Substring(24, 8).ToUpper();

            // Formatear el número de ejemplar con 3 dígitos (001, 002, etc.)
            string numeroFormateado = numeroEjemplar.ToString("D3");

            // Retornar el código con formato BIB-XXXXXXXX-###
            return $"BIB-{idCorto}-{numeroFormateado}";
        }

        #endregion
    }
}
