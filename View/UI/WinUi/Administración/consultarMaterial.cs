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
                lblTitulo.Text = LanguageManager.Translate("titulo");
                lblAutor.Text = LanguageManager.Translate("autor");
                lblTipo.Text = LanguageManager.Translate("tipo");
                lblGenero.Text = LanguageManager.Translate("genero");
                lblNivel.Text = "Nivel:";
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
            bool tienePermisoEditar = TienePermiso("EditarMaterial");
            btnEditar.Visible = tienePermisoEditar;

            // Mostrar/ocultar botón Gestionar Ejemplares según permisos
            bool tienePermisoGestionarEjemplares = TienePermiso("GestionarEjemplares");
            btnGestionarEjemplares.Visible = tienePermisoGestionarEjemplares;
        }

        private void CargarComboBoxes()
        {
            // Cargar tipos de material
            cmbTipo.Items.Clear();
            cmbTipo.Items.Add("Todos");
            cmbTipo.Items.Add("Libro");
            cmbTipo.Items.Add("Revista");
            cmbTipo.Items.Add("Manual");
            cmbTipo.SelectedIndex = 0;

            // Cargar géneros
            cmbGenero.Items.Clear();
            cmbGenero.Items.Add("Todos");
            cmbGenero.Items.Add("Fantasía");
            cmbGenero.Items.Add("Ciencia Ficción");
            cmbGenero.Items.Add("Aventura");
            cmbGenero.Items.Add("Misterio");
            cmbGenero.Items.Add("Romance");
            cmbGenero.Items.Add("Terror");
            cmbGenero.Items.Add("Histórico");
            cmbGenero.Items.Add("Educativo");
            cmbGenero.Items.Add("Biografía");
            cmbGenero.Items.Add("Poesía");
            cmbGenero.Items.Add("Drama");
            cmbGenero.Items.Add("Comedia");
            cmbGenero.Items.Add("Infantil");
            cmbGenero.Items.Add("Juvenil");
            cmbGenero.Items.Add("Técnico");
            cmbGenero.Items.Add("Científico");
            cmbGenero.Items.Add("Otro");
            cmbGenero.SelectedIndex = 0;

            // Cargar niveles educativos
            cmbNivel.Items.Clear();
            cmbNivel.Items.Add("Todos");
            cmbNivel.Items.Add("Inicial");
            cmbNivel.Items.Add("Primario");
            cmbNivel.Items.Add("Secundario");
            cmbNivel.Items.Add("Universitario");
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
            dgvMateriales.Columns["EdadRecomendada"].Visible = false;
            dgvMateriales.Columns["Descripcion"].Visible = false;

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
                colPrestada.HeaderText = "Cant. Prestada";
                colPrestada.ReadOnly = true;
                dgvMateriales.Columns.Add(colPrestada);
            }

            if (!dgvMateriales.Columns.Contains("CantidadEnReparacion"))
            {
                DataGridViewTextBoxColumn colEnReparacion = new DataGridViewTextBoxColumn();
                colEnReparacion.Name = "CantidadEnReparacion";
                colEnReparacion.HeaderText = "Cant. En Reparación";
                colEnReparacion.ReadOnly = true;
                dgvMateriales.Columns.Add(colEnReparacion);
            }

            if (!dgvMateriales.Columns.Contains("CantidadNoDisponible"))
            {
                DataGridViewTextBoxColumn colNoDisponible = new DataGridViewTextBoxColumn();
                colNoDisponible.Name = "CantidadNoDisponible";
                colNoDisponible.HeaderText = "Cant. No Disponible";
                colNoDisponible.ReadOnly = true;
                dgvMateriales.Columns.Add(colNoDisponible);
            }

            dgvMateriales.Columns["CantidadPrestada"].DisplayIndex = 9;
            dgvMateriales.Columns["CantidadEnReparacion"].DisplayIndex = 10;
            dgvMateriales.Columns["CantidadNoDisponible"].DisplayIndex = 11;

            // Calcular cantidades por estado para cada fila
            EjemplarBLL ejemplarBLL = new EjemplarBLL();
            foreach (DataGridViewRow row in dgvMateriales.Rows)
            {
                if (row.DataBoundItem is Material material)
                {
                    List<Ejemplar> ejemplares = ejemplarBLL.ObtenerEjemplaresPorMaterial(material.IdMaterial);

                    int cantidadPrestada = ejemplares.Count(e => e.Estado == EstadoMaterial.Prestado);
                    int cantidadEnReparacion = ejemplares.Count(e => e.Estado == EstadoMaterial.EnReparacion);
                    int cantidadNoDisponible = ejemplares.Count(e => e.Estado == EstadoMaterial.NoDisponible);

                    row.Cells["CantidadPrestada"].Value = cantidadPrestada;
                    row.Cells["CantidadEnReparacion"].Value = cantidadEnReparacion;
                    row.Cells["CantidadNoDisponible"].Value = cantidadNoDisponible;
                }
            }

            // Configurar encabezados de columnas
            dgvMateriales.Columns["Titulo"].HeaderText = "Título";
            dgvMateriales.Columns["Autor"].HeaderText = "Autor";
            dgvMateriales.Columns["Editorial"].HeaderText = "Editorial";
            dgvMateriales.Columns["Tipo"].HeaderText = "Tipo";
            dgvMateriales.Columns["Genero"].HeaderText = "Género";
            dgvMateriales.Columns["ISBN"].HeaderText = "ISBN";
            dgvMateriales.Columns["AnioPublicacion"].HeaderText = "Año Publicación";
            dgvMateriales.Columns["CantidadTotal"].HeaderText = "Cant. Total";
            dgvMateriales.Columns["CantidadDisponible"].HeaderText = "Cant. Disp.";

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

                lblResultados.Text = $"Resultados encontrados: {materiales.Count}";
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
                string tipo = cmbTipo.SelectedItem?.ToString();
                string genero = cmbGenero.SelectedItem?.ToString();
                string nivel = cmbNivel.SelectedItem?.ToString();

                // Obtener todos los materiales según los filtros básicos
                List<Material> materiales = _materialBLL.BuscarMateriales(titulo, autor, tipo);

                // Aplicar filtros adicionales
                if (!string.IsNullOrEmpty(genero) && genero != "Todos")
                {
                    materiales = materiales.Where(m => m.Genero != null && m.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(nivel) && nivel != "Todos")
                {
                    materiales = materiales.Where(m => m.EdadRecomendada != null && m.EdadRecomendada.Contains(nivel)).ToList();
                }

                dgvMateriales.DataSource = materiales;

                ConfigurarColumnasVisibles();

                lblResultados.Text = $"Resultados encontrados: {materiales.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                if (!TienePermiso("EditarMaterial"))
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
                if (!TienePermiso("GestionarEjemplares"))
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la gestión de ejemplares: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dgvMateriales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
