using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using BLL;
using DomainModel;
using DomainModel.Enums;

namespace UI.WinUi.Administrador
{
    public partial class consultarMaterial : Form
    {
        private Usuario _usuarioLogueado;
        private MaterialBLL _materialBLL;

        public consultarMaterial()
        {
            InitializeComponent();
            _materialBLL = new MaterialBLL();
        }

        public consultarMaterial(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += ConsultarMaterial_Load;
            btnBuscar.Click += BtnBuscar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            btnVolver.Click += BtnVolver_Click;
            btnEditar.Click += BtnEditar_Click;
            btnGestionarEjemplares.Click += BtnGestionarEjemplares_Click;

            // Configurar DataGridView para solo lectura
            dgvMateriales.ReadOnly = true;
            dgvMateriales.AllowUserToAddRows = false;
            dgvMateriales.AllowUserToDeleteRows = false;
            dgvMateriales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMateriales.MultiSelect = false;

            // Agregar evento para traducir valores de celdas
            dgvMateriales.CellFormatting += DgvMateriales_CellFormatting;

            ConfigurarEstiloDataGridView();

            // Configurar visibilidad del botón según permisos
            ConfigurarPermisos();
        }

        private void ConfigurarEstiloDataGridView()
        {
            // Colores de selección
            dgvMateriales.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvMateriales.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            // Estilo alternado de filas
            dgvMateriales.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 246, 247);
            dgvMateriales.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;

            // Estilo del header
            dgvMateriales.EnableHeadersVisualStyles = false;
            dgvMateriales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvMateriales.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvMateriales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dgvMateriales.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Borde y líneas
            dgvMateriales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMateriales.GridColor = System.Drawing.Color.FromArgb(189, 195, 199);
        }

        private void ConsultarMaterial_Load(object sender, EventArgs e)
        {
            AplicarTraducciones();
            CargarComboBoxes();
            CargarTodosMateriales();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("consultar_material");
                groupBoxFiltros.Text = LanguageManager.Translate("filtros_busqueda");
                lblTitulo.Text = LanguageManager.Translate("titulo") + ":";
                lblAutor.Text = LanguageManager.Translate("autor") + ":";
                lblTipo.Text = LanguageManager.Translate("tipo") + ":";
                lblGenero.Text = LanguageManager.Translate("genero") + ":";
                lblNivel.Text = LanguageManager.Translate("nivel") + ":";
                btnBuscar.Text = LanguageManager.Translate("buscar");
                btnLimpiar.Text = LanguageManager.Translate("limpiar");
                btnEditar.Text = LanguageManager.Translate("editar_material");
                btnGestionarEjemplares.Text = LanguageManager.Translate("gestionar_ejemplares");
                btnVolver.Text = LanguageManager.Translate("volver");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void ConfigurarPermisos()
        {
            // Mostrar/ocultar botón Editar según permisos
            bool tienePermisoEditar = TienePermiso("Editar Material");
            btnEditar.Visible = tienePermisoEditar;

            // Mostrar/ocultar botón Gestionar Ejemplares según permisos
            bool tienePermisoGestionarEjemplares = TienePermiso("Gestionar Ejemplares");
            btnGestionarEjemplares.Visible = tienePermisoGestionarEjemplares;
        }

        private void CargarComboBoxes()
        {
            // Cargar tipos de material
            cmbTipo.Items.Clear();
            cmbTipo.Items.Add(LanguageManager.Translate("todos"));
            cmbTipo.Items.Add(LanguageManager.Translate("libro"));
            cmbTipo.Items.Add(LanguageManager.Translate("revista"));
            cmbTipo.Items.Add(LanguageManager.Translate("manual"));
            cmbTipo.SelectedIndex = 0;

            // Cargar géneros
            cmbGenero.Items.Clear();
            cmbGenero.Items.Add(LanguageManager.Translate("todos"));
            cmbGenero.Items.Add(LanguageManager.Translate("fantasia"));
            cmbGenero.Items.Add(LanguageManager.Translate("ciencia_ficcion"));
            cmbGenero.Items.Add(LanguageManager.Translate("aventura"));
            cmbGenero.Items.Add(LanguageManager.Translate("misterio"));
            cmbGenero.Items.Add(LanguageManager.Translate("romance"));
            cmbGenero.Items.Add(LanguageManager.Translate("terror"));
            cmbGenero.Items.Add(LanguageManager.Translate("historico"));
            cmbGenero.Items.Add(LanguageManager.Translate("educativo"));
            cmbGenero.Items.Add(LanguageManager.Translate("biografia"));
            cmbGenero.Items.Add(LanguageManager.Translate("poesia"));
            cmbGenero.Items.Add(LanguageManager.Translate("drama"));
            cmbGenero.Items.Add(LanguageManager.Translate("comedia"));
            cmbGenero.Items.Add(LanguageManager.Translate("infantil"));
            cmbGenero.Items.Add(LanguageManager.Translate("juvenil"));
            cmbGenero.Items.Add(LanguageManager.Translate("tecnico"));
            cmbGenero.Items.Add(LanguageManager.Translate("cientifico"));
            cmbGenero.Items.Add(LanguageManager.Translate("otro"));
            cmbGenero.SelectedIndex = 0;

            // Cargar niveles educativos
            cmbNivel.Items.Clear();
            cmbNivel.Items.Add(LanguageManager.Translate("todos"));
            cmbNivel.Items.Add(LanguageManager.Translate("inicial"));
            cmbNivel.Items.Add(LanguageManager.Translate("primario"));
            cmbNivel.Items.Add(LanguageManager.Translate("secundario"));
            cmbNivel.Items.Add(LanguageManager.Translate("universitario"));
            cmbNivel.SelectedIndex = 0;
        }

        private void ConfigurarColumnasVisibles()
        {
            if (dgvMateriales.Columns.Count == 0)
                return;

            // Ocultar columnas que no queremos mostrar
            dgvMateriales.Columns["IdMaterial"].Visible = false;
            dgvMateriales.Columns["FechaRegistro"].Visible = false;
            dgvMateriales.Columns["Activo"].Visible = false;

            // Ocultar columna Nivel (solo la usamos en filtros)
            if (dgvMateriales.Columns.Contains("Nivel"))
                dgvMateriales.Columns["Nivel"].Visible = false;

            // Configurar orden de columnas visibles
            dgvMateriales.Columns["Titulo"].DisplayIndex = 0;
            dgvMateriales.Columns["Autor"].DisplayIndex = 1;
            dgvMateriales.Columns["Editorial"].DisplayIndex = 2;
            dgvMateriales.Columns["Tipo"].DisplayIndex = 3;
            dgvMateriales.Columns["Genero"].DisplayIndex = 4;
            dgvMateriales.Columns["ISBN"].DisplayIndex = 5;
            dgvMateriales.Columns["AnioPublicacion"].DisplayIndex = 6;
            dgvMateriales.Columns["CantidadTotal"].DisplayIndex = 7;
            dgvMateriales.Columns["CantidadDisponible"].DisplayIndex = 8;

            // Agregar columnas calculadas para cada estado
            if (!dgvMateriales.Columns.Contains("CantidadPrestada"))
            {
                DataGridViewTextBoxColumn colPrestada = new DataGridViewTextBoxColumn();
                colPrestada.Name = "CantidadPrestada";
                colPrestada.HeaderText = LanguageManager.Translate("cant_prestada");
                colPrestada.ReadOnly = true;
                dgvMateriales.Columns.Add(colPrestada);
            }

            if (!dgvMateriales.Columns.Contains("CantidadEnReparacion"))
            {
                DataGridViewTextBoxColumn colEnReparacion = new DataGridViewTextBoxColumn();
                colEnReparacion.Name = "CantidadEnReparacion";
                colEnReparacion.HeaderText = LanguageManager.Translate("cant_en_reparacion");
                colEnReparacion.ReadOnly = true;
                dgvMateriales.Columns.Add(colEnReparacion);
            }

            if (!dgvMateriales.Columns.Contains("CantidadNoDisponible"))
            {
                DataGridViewTextBoxColumn colNoDisponible = new DataGridViewTextBoxColumn();
                colNoDisponible.Name = "CantidadNoDisponible";
                colNoDisponible.HeaderText = LanguageManager.Translate("cant_no_disponible");
                colNoDisponible.ReadOnly = true;
                dgvMateriales.Columns.Add(colNoDisponible);
            }

            dgvMateriales.Columns["CantidadPrestada"].DisplayIndex = 9;
            dgvMateriales.Columns["CantidadEnReparacion"].DisplayIndex = 10;
            dgvMateriales.Columns["CantidadNoDisponible"].DisplayIndex = 11;

            // OPTIMIZACIÓN: Cargar todos los ejemplares en una sola consulta
            // para evitar el problema N+1 (N consultas para N materiales)
            EjemplarBLL ejemplarBLL = new EjemplarBLL();
            List<Ejemplar> todosLosEjemplares = ejemplarBLL.ObtenerTodosEjemplares();

            // Agrupar ejemplares por IdMaterial en memoria para acceso rápido
            var ejemplaresPorMaterial = todosLosEjemplares
                .GroupBy(e => e.IdMaterial)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Calcular cantidades por estado para cada fila
            foreach (DataGridViewRow row in dgvMateriales.Rows)
            {
                if (row.DataBoundItem is Material material)
                {
                    // Obtener ejemplares desde el diccionario (en memoria, sin consulta adicional)
                    List<Ejemplar> ejemplares = ejemplaresPorMaterial.ContainsKey(material.IdMaterial)
                        ? ejemplaresPorMaterial[material.IdMaterial]
                        : new List<Ejemplar>();

                    int cantidadPrestada = ejemplares.Count(e => e.Estado == EstadoMaterial.Prestado);
                    int cantidadEnReparacion = ejemplares.Count(e => e.Estado == EstadoMaterial.EnReparacion);
                    int cantidadNoDisponible = ejemplares.Count(e => e.Estado == EstadoMaterial.NoDisponible);

                    row.Cells["CantidadPrestada"].Value = cantidadPrestada;
                    row.Cells["CantidadEnReparacion"].Value = cantidadEnReparacion;
                    row.Cells["CantidadNoDisponible"].Value = cantidadNoDisponible;
                }
            }

            // Configurar encabezados de columnas
            dgvMateriales.Columns["Titulo"].HeaderText = LanguageManager.Translate("titulo");
            dgvMateriales.Columns["Autor"].HeaderText = LanguageManager.Translate("autor");
            dgvMateriales.Columns["Editorial"].HeaderText = LanguageManager.Translate("editorial");
            dgvMateriales.Columns["Tipo"].HeaderText = LanguageManager.Translate("tipo");
            dgvMateriales.Columns["Genero"].HeaderText = LanguageManager.Translate("genero");
            dgvMateriales.Columns["ISBN"].HeaderText = "ISBN";
            dgvMateriales.Columns["AnioPublicacion"].HeaderText = LanguageManager.Translate("anio_publicacion_col");
            dgvMateriales.Columns["CantidadTotal"].HeaderText = LanguageManager.Translate("cant_total");
            dgvMateriales.Columns["CantidadDisponible"].HeaderText = LanguageManager.Translate("cant_disp");

            // Ajustar ancho de columnas
            dgvMateriales.Columns["Titulo"].Width = 190;
            dgvMateriales.Columns["Autor"].Width = 150;
            dgvMateriales.Columns["Editorial"].Width = 150;
            dgvMateriales.Columns["Tipo"].Width = 80;
            dgvMateriales.Columns["Genero"].Width = 90;
            dgvMateriales.Columns["ISBN"].Width = 100;
            dgvMateriales.Columns["AnioPublicacion"].Width = 90;
            dgvMateriales.Columns["CantidadTotal"].Width = 70;
            dgvMateriales.Columns["CantidadDisponible"].Width = 70;
            dgvMateriales.Columns["CantidadPrestada"].Width = 70;
            dgvMateriales.Columns["CantidadEnReparacion"].Width = 70;
            dgvMateriales.Columns["CantidadNoDisponible"].Width = 70;
        }

        private void CargarTodosMateriales()
        {
            try
            {
                List<Material> materiales = _materialBLL.ObtenerTodosMateriales();
                dgvMateriales.DataSource = materiales;

                ConfigurarColumnasVisibles();

                lblResultados.Text = $"{LanguageManager.Translate("resultados_encontrados")}: {materiales.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar materiales: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string titulo = txtTitulo.Text.Trim();
                string autor = txtAutor.Text.Trim();
                string tipoTraducido = cmbTipo.SelectedItem?.ToString();
                string generoTraducido = cmbGenero.SelectedItem?.ToString();
                string nivelTraducido = cmbNivel.SelectedItem?.ToString();

                // Convertir valores traducidos a valores originales para la búsqueda
                string tipo = ConvertirTipoAOriginal(tipoTraducido);
                string genero = ConvertirGeneroAOriginal(generoTraducido);
                string nivel = ConvertirNivelAOriginal(nivelTraducido);

                // Obtener todos los materiales según los filtros básicos
                List<Material> materiales = _materialBLL.BuscarMateriales(titulo, autor, tipo);

                // Aplicar filtros adicionales
                if (!string.IsNullOrEmpty(genero) && genero != "Todos")
                {
                    materiales = materiales.Where(m => m.Genero != null && m.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(nivel) && nivel != "Todos")
                {
                    materiales = materiales.Where(m => m.Nivel != null && m.Nivel.Contains(nivel)).ToList();
                }

                dgvMateriales.DataSource = materiales;

                ConfigurarColumnasVisibles();

                lblResultados.Text = $"{LanguageManager.Translate("resultados_encontrados")}: {materiales.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento que se dispara al formatear cada celda del DataGridView
        /// Permite traducir los valores de Tipo y Género sin modificar los datos subyacentes
        /// </summary>
        private void DgvMateriales_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Solo procesar si hay un valor
                if (e.Value == null)
                    return;

                // Obtener el nombre de la columna
                string columnName = dgvMateriales.Columns[e.ColumnIndex].Name;

                // Traducir columna Tipo
                if (columnName == "Tipo")
                {
                    string tipoOriginal = e.Value.ToString();
                    string tipoTraducido = TraducirTipoDesdeOriginal(tipoOriginal);
                    e.Value = tipoTraducido;
                    e.FormattingApplied = true;
                }
                // Traducir columna Género
                else if (columnName == "Genero")
                {
                    string generoOriginal = e.Value.ToString();
                    string generoTraducido = TraducirGeneroDesdeOriginal(generoOriginal);
                    e.Value = generoTraducido;
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al formatear celda: {ex.Message}");
            }
        }

        /// <summary>
        /// Traduce un tipo de material desde el valor original (español en BD) al idioma actual
        /// </summary>
        private string TraducirTipoDesdeOriginal(string tipoOriginal)
        {
            if (string.IsNullOrEmpty(tipoOriginal))
                return tipoOriginal;

            // Mapear tipos originales a claves de traducción
            switch (tipoOriginal.ToLower())
            {
                case "libro":
                    return LanguageManager.Translate("libro");
                case "revista":
                    return LanguageManager.Translate("revista");
                case "manual":
                    return LanguageManager.Translate("manual");
                default:
                    return tipoOriginal;
            }
        }

        /// <summary>
        /// Traduce un género desde el valor original (español en BD) al idioma actual
        /// </summary>
        private string TraducirGeneroDesdeOriginal(string generoOriginal)
        {
            if (string.IsNullOrEmpty(generoOriginal))
                return generoOriginal;

            // Mapear géneros originales a claves de traducción
            switch (generoOriginal.ToLower())
            {
                case "fantasía":
                case "fantasia":
                    return LanguageManager.Translate("fantasia");
                case "ciencia ficción":
                case "ciencia ficcion":
                    return LanguageManager.Translate("ciencia_ficcion");
                case "aventura":
                    return LanguageManager.Translate("aventura");
                case "misterio":
                    return LanguageManager.Translate("misterio");
                case "romance":
                    return LanguageManager.Translate("romance");
                case "terror":
                    return LanguageManager.Translate("terror");
                case "histórico":
                case "historico":
                    return LanguageManager.Translate("historico");
                case "educativo":
                    return LanguageManager.Translate("educativo");
                case "biografía":
                case "biografia":
                    return LanguageManager.Translate("biografia");
                case "poesía":
                case "poesia":
                    return LanguageManager.Translate("poesia");
                case "drama":
                    return LanguageManager.Translate("drama");
                case "comedia":
                    return LanguageManager.Translate("comedia");
                case "infantil":
                    return LanguageManager.Translate("infantil");
                case "juvenil":
                    return LanguageManager.Translate("juvenil");
                case "técnico":
                case "tecnico":
                    return LanguageManager.Translate("tecnico");
                case "científico":
                case "cientifico":
                    return LanguageManager.Translate("cientifico");
                case "otro":
                    return LanguageManager.Translate("otro");
                case "policial":
                    return LanguageManager.Translate("policial");
                case "teatral":
                    return LanguageManager.Translate("teatral");
                case "novela":
                    return LanguageManager.Translate("novela");
                default:
                    return generoOriginal;
            }
        }

        /// <summary>
        /// Convierte el tipo traducido al valor original de la base de datos
        /// </summary>
        private string ConvertirTipoAOriginal(string tipoTraducido)
        {
            if (string.IsNullOrEmpty(tipoTraducido))
                return "Todos";

            // Comparar con las traducciones
            if (tipoTraducido == LanguageManager.Translate("todos")) return "Todos";
            if (tipoTraducido == LanguageManager.Translate("libro")) return "Libro";
            if (tipoTraducido == LanguageManager.Translate("revista")) return "Revista";
            if (tipoTraducido == LanguageManager.Translate("manual")) return "Manual";

            return tipoTraducido;
        }

        /// <summary>
        /// Convierte el género traducido al valor original de la base de datos
        /// </summary>
        private string ConvertirGeneroAOriginal(string generoTraducido)
        {
            if (string.IsNullOrEmpty(generoTraducido))
                return "Todos";

            // Comparar con las traducciones
            if (generoTraducido == LanguageManager.Translate("todos")) return "Todos";
            if (generoTraducido == LanguageManager.Translate("fantasia")) return "Fantasía";
            if (generoTraducido == LanguageManager.Translate("ciencia_ficcion")) return "Ciencia Ficción";
            if (generoTraducido == LanguageManager.Translate("aventura")) return "Aventura";
            if (generoTraducido == LanguageManager.Translate("misterio")) return "Misterio";
            if (generoTraducido == LanguageManager.Translate("romance")) return "Romance";
            if (generoTraducido == LanguageManager.Translate("terror")) return "Terror";
            if (generoTraducido == LanguageManager.Translate("historico")) return "Histórico";
            if (generoTraducido == LanguageManager.Translate("educativo")) return "Educativo";
            if (generoTraducido == LanguageManager.Translate("biografia")) return "Biografía";
            if (generoTraducido == LanguageManager.Translate("poesia")) return "Poesía";
            if (generoTraducido == LanguageManager.Translate("drama")) return "Drama";
            if (generoTraducido == LanguageManager.Translate("comedia")) return "Comedia";
            if (generoTraducido == LanguageManager.Translate("infantil")) return "Infantil";
            if (generoTraducido == LanguageManager.Translate("juvenil")) return "Juvenil";
            if (generoTraducido == LanguageManager.Translate("tecnico")) return "Técnico";
            if (generoTraducido == LanguageManager.Translate("cientifico")) return "Científico";
            if (generoTraducido == LanguageManager.Translate("otro")) return "Otro";

            return generoTraducido;
        }

        /// <summary>
        /// Convierte el nivel traducido al valor original de la base de datos
        /// </summary>
        private string ConvertirNivelAOriginal(string nivelTraducido)
        {
            if (string.IsNullOrEmpty(nivelTraducido))
                return "Todos";

            // Comparar con las traducciones
            if (nivelTraducido == LanguageManager.Translate("todos")) return "Todos";
            if (nivelTraducido == LanguageManager.Translate("inicial")) return "Inicial";
            if (nivelTraducido == LanguageManager.Translate("primario")) return "Primario";
            if (nivelTraducido == LanguageManager.Translate("secundario")) return "Secundario";
            if (nivelTraducido == LanguageManager.Translate("universitario")) return "Universitario";

            return nivelTraducido;
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            cmbTipo.SelectedIndex = 0;
            cmbGenero.SelectedIndex = 0;
            cmbNivel.SelectedIndex = 0;
            CargarTodosMateriales();
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que haya una fila seleccionada
                if (dgvMateriales.SelectedRows.Count == 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar un material para editar.",
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Verificar si tiene permisos de edición
                if (!TienePermiso("Editar Material"))
                {
                    MessageBox.Show(
                        "No tiene permisos para editar materiales.\nContacte al administrador del sistema.",
                        LanguageManager.Translate("error_autorizacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el material seleccionado
                Material materialSeleccionado = dgvMateriales.SelectedRows[0].DataBoundItem as Material;

                if (materialSeleccionado == null)
                    return;

                // Guardar el ID del material seleccionado para restaurar la selección después
                Guid idMaterialSeleccionado = materialSeleccionado.IdMaterial;

                // Abrir formulario de edición
                EditarMaterial formEditar = new EditarMaterial(_usuarioLogueado, materialSeleccionado);
                DialogResult resultado = formEditar.ShowDialog();

                // Si se guardaron cambios, recargar la grilla
                if (resultado == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtTitulo.Text) &&
                        string.IsNullOrWhiteSpace(txtAutor.Text) &&
                        cmbTipo.SelectedIndex == 0 &&
                        cmbGenero.SelectedIndex == 0 &&
                        cmbNivel.SelectedIndex == 0)
                    {
                        CargarTodosMateriales();
                    }
                    else
                    {
                        BtnBuscar_Click(sender, e);
                    }

                    // Restaurar la selección del material
                    RestaurarSeleccionMaterial(idMaterialSeleccionado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el editor: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGestionarEjemplares_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que haya una fila seleccionada
                if (dgvMateriales.SelectedRows.Count == 0)
                {
                    MessageBox.Show(
                        "Debe seleccionar un material para gestionar sus ejemplares.",
                        LanguageManager.Translate("validacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Verificar si tiene permisos
                if (!TienePermiso("Gestionar Ejemplares"))
                {
                    MessageBox.Show(
                        "No tiene permisos para gestionar ejemplares.\nContacte al administrador del sistema.",
                        LanguageManager.Translate("error_autorizacion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el material seleccionado
                Material materialSeleccionado = dgvMateriales.SelectedRows[0].DataBoundItem as Material;

                if (materialSeleccionado == null)
                    return;

                // Guardar el ID del material seleccionado para restaurar la selección después
                Guid idMaterialSeleccionado = materialSeleccionado.IdMaterial;

                // Abrir formulario de gestión de ejemplares
                GestionarEjemplares formEjemplares = new GestionarEjemplares(_usuarioLogueado, materialSeleccionado);
                DialogResult resultado = formEjemplares.ShowDialog();

                // Si se guardaron cambios, recargar la grilla
                if (resultado == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtTitulo.Text) &&
                        string.IsNullOrWhiteSpace(txtAutor.Text) &&
                        cmbTipo.SelectedIndex == 0 &&
                        cmbGenero.SelectedIndex == 0 &&
                        cmbNivel.SelectedIndex == 0)
                    {
                        CargarTodosMateriales();
                    }
                    else
                    {
                        BtnBuscar_Click(sender, e);
                    }

                    // Restaurar la selección del material
                    RestaurarSeleccionMaterial(idMaterialSeleccionado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la gestión de ejemplares: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Restaura la selección de un material en el DataGridView después de recargar los datos
        /// </summary>
        /// <param name="idMaterial">ID del material a seleccionar</param>
        private void RestaurarSeleccionMaterial(Guid idMaterial)
        {
            try
            {
                // Buscar el material en el DataGridView
                foreach (DataGridViewRow row in dgvMateriales.Rows)
                {
                    if (row.DataBoundItem is Material material)
                    {
                        if (material.IdMaterial == idMaterial)
                        {
                            // Limpiar selección actual
                            dgvMateriales.ClearSelection();

                            // Seleccionar la fila encontrada
                            row.Selected = true;

                            // Hacer scroll para que la fila sea visible
                            dgvMateriales.FirstDisplayedScrollingRowIndex = row.Index;

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al restaurar selección: {ex.Message}");
            }
        }

        private bool TienePermiso(string nombrePatente)
        {
            // Usar el método centralizado del Usuario que maneja el bypass de Administrador
            return _usuarioLogueado?.TienePermiso(nombrePatente) ?? false;
        }

        private void dgvMateriales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
