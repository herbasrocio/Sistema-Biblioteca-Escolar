namespace UI.WinUi.Administrador
{
    partial class gestionAlumnos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGestionActual = new System.Windows.Forms.TabPage();
            this.separator = new System.Windows.Forms.Label();
            this.lblGrado = new System.Windows.Forms.Label();
            this.cmbGrado = new System.Windows.Forms.ComboBox();
            this.btnNuevoGrado = new System.Windows.Forms.Button();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.panelEstadisticas = new System.Windows.Forms.Panel();
            this.lblEstadisticas = new System.Windows.Forms.Label();
            this.lblListaAlumnos = new System.Windows.Forms.Label();
            this.dgvAlumnos = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupOperacionesAlumno = new System.Windows.Forms.GroupBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.groupOperacionesMasivas = new System.Windows.Forms.GroupBox();
            this.btnPromocionAlumnos = new System.Windows.Forms.Button();
            this.btnImportarExportar = new System.Windows.Forms.Button();
            this.btnEditarGrado = new System.Windows.Forms.Button();
            this.tabHistorial = new System.Windows.Forms.TabPage();
            this.lblAnioLectivoHistorial = new System.Windows.Forms.Label();
            this.cmbAnioLectivoHistorial = new System.Windows.Forms.ComboBox();
            this.lblGradoHistorial = new System.Windows.Forms.Label();
            this.cmbGradoHistorial = new System.Windows.Forms.ComboBox();
            this.btnLimpiarFiltrosHistorial = new System.Windows.Forms.Button();
            this.panelEstadisticasHistorial = new System.Windows.Forms.Panel();
            this.lblEstadisticasHistorial = new System.Windows.Forms.Label();
            this.lblListaHistorial = new System.Windows.Forms.Label();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.btnVerTrayectoria = new System.Windows.Forms.Button();
            this.menuImportarExportar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabControl.SuspendLayout();
            this.tabGestionActual.SuspendLayout();
            this.panelEstadisticas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).BeginInit();
            this.groupOperacionesAlumno.SuspendLayout();
            this.groupOperacionesMasivas.SuspendLayout();
            this.tabHistorial.SuspendLayout();
            this.panelEstadisticasHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGestionActual);
            this.tabControl.Controls.Add(this.tabHistorial);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 520);
            this.tabControl.TabIndex = 0;
            // 
            // tabGestionActual
            // 
            this.tabGestionActual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.tabGestionActual.Controls.Add(this.separator);
            this.tabGestionActual.Controls.Add(this.lblGrado);
            this.tabGestionActual.Controls.Add(this.cmbGrado);
            this.tabGestionActual.Controls.Add(this.btnNuevoGrado);
            this.tabGestionActual.Controls.Add(this.lblBuscar);
            this.tabGestionActual.Controls.Add(this.txtBuscar);
            this.tabGestionActual.Controls.Add(this.panelEstadisticas);
            this.tabGestionActual.Controls.Add(this.lblListaAlumnos);
            this.tabGestionActual.Controls.Add(this.dgvAlumnos);
            this.tabGestionActual.Controls.Add(this.lblTotal);
            this.tabGestionActual.Controls.Add(this.groupOperacionesAlumno);
            this.tabGestionActual.Controls.Add(this.groupOperacionesMasivas);
            this.tabGestionActual.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabGestionActual.Location = new System.Drawing.Point(4, 24);
            this.tabGestionActual.Name = "tabGestionActual";
            this.tabGestionActual.Padding = new System.Windows.Forms.Padding(3);
            this.tabGestionActual.Size = new System.Drawing.Size(792, 492);
            this.tabGestionActual.TabIndex = 0;
            this.tabGestionActual.Text = "Gestión Actual";
            // 
            // separator
            // 
            this.separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separator.Location = new System.Drawing.Point(20, 11);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(760, 2);
            this.separator.TabIndex = 1;
            // 
            // lblGrado
            // 
            this.lblGrado.AutoSize = true;
            this.lblGrado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGrado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblGrado.Location = new System.Drawing.Point(20, 31);
            this.lblGrado.Name = "lblGrado";
            this.lblGrado.Size = new System.Drawing.Size(93, 15);
            this.lblGrado.TabIndex = 2;
            this.lblGrado.Text = "Grado/División:";
            // 
            // cmbGrado
            // 
            this.cmbGrado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbGrado.FormattingEnabled = true;
            this.cmbGrado.Location = new System.Drawing.Point(125, 28);
            this.cmbGrado.Name = "cmbGrado";
            this.cmbGrado.Size = new System.Drawing.Size(140, 23);
            this.cmbGrado.TabIndex = 3;
            // 
            // btnNuevoGrado
            // 
            this.btnNuevoGrado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnNuevoGrado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNuevoGrado.FlatAppearance.BorderSize = 0;
            this.btnNuevoGrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevoGrado.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnNuevoGrado.ForeColor = System.Drawing.Color.White;
            this.btnNuevoGrado.Location = new System.Drawing.Point(275, 27);
            this.btnNuevoGrado.Name = "btnNuevoGrado";
            this.btnNuevoGrado.Size = new System.Drawing.Size(90, 25);
            this.btnNuevoGrado.TabIndex = 4;
            this.btnNuevoGrado.Text = "+ Grado";
            this.btnNuevoGrado.UseVisualStyleBackColor = false;
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblBuscar.Location = new System.Drawing.Point(20, 66);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(47, 15);
            this.lblBuscar.TabIndex = 5;
            this.lblBuscar.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBuscar.ForeColor = System.Drawing.Color.Gray;
            this.txtBuscar.Location = new System.Drawing.Point(125, 63);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(300, 23);
            this.txtBuscar.TabIndex = 6;
            this.txtBuscar.Text = "Buscar por nombre, apellido o DNI...";
            // 
            // panelEstadisticas
            // 
            this.panelEstadisticas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelEstadisticas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEstadisticas.Controls.Add(this.lblEstadisticas);
            this.panelEstadisticas.Location = new System.Drawing.Point(440, 61);
            this.panelEstadisticas.Name = "panelEstadisticas";
            this.panelEstadisticas.Size = new System.Drawing.Size(340, 50);
            this.panelEstadisticas.TabIndex = 7;
            // 
            // lblEstadisticas
            // 
            this.lblEstadisticas.AutoSize = true;
            this.lblEstadisticas.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblEstadisticas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblEstadisticas.Location = new System.Drawing.Point(10, 15);
            this.lblEstadisticas.Name = "lblEstadisticas";
            this.lblEstadisticas.Size = new System.Drawing.Size(108, 15);
            this.lblEstadisticas.TabIndex = 0;
            this.lblEstadisticas.Text = "📊 Total: 0 alumnos";
            // 
            // lblListaAlumnos
            // 
            this.lblListaAlumnos.AutoSize = true;
            this.lblListaAlumnos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblListaAlumnos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblListaAlumnos.Location = new System.Drawing.Point(20, 120);
            this.lblListaAlumnos.Name = "lblListaAlumnos";
            this.lblListaAlumnos.Size = new System.Drawing.Size(103, 15);
            this.lblListaAlumnos.TabIndex = 8;
            this.lblListaAlumnos.Text = "Lista de Alumnos:";
            // 
            // dgvAlumnos
            // 
            this.dgvAlumnos.AllowUserToAddRows = false;
            this.dgvAlumnos.AllowUserToDeleteRows = false;
            this.dgvAlumnos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlumnos.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlumnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlumnos.Location = new System.Drawing.Point(20, 145);
            this.dgvAlumnos.Name = "dgvAlumnos";
            this.dgvAlumnos.ReadOnly = true;
            this.dgvAlumnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlumnos.Size = new System.Drawing.Size(760, 250);
            this.dgvAlumnos.TabIndex = 9;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblTotal.Location = new System.Drawing.Point(580, 120);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 15);
            this.lblTotal.TabIndex = 10;
            // 
            // groupOperacionesAlumno
            // 
            this.groupOperacionesAlumno.Controls.Add(this.btnNuevo);
            this.groupOperacionesAlumno.Controls.Add(this.btnEditar);
            this.groupOperacionesAlumno.Controls.Add(this.btnEliminar);
            this.groupOperacionesAlumno.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupOperacionesAlumno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.groupOperacionesAlumno.Location = new System.Drawing.Point(250, 410);
            this.groupOperacionesAlumno.Name = "groupOperacionesAlumno";
            this.groupOperacionesAlumno.Size = new System.Drawing.Size(370, 70);
            this.groupOperacionesAlumno.TabIndex = 11;
            this.groupOperacionesAlumno.TabStop = false;
            this.groupOperacionesAlumno.Text = "Operaciones de Alumno";
            // 
            // btnNuevo
            // 
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnNuevo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNuevo.FlatAppearance.BorderSize = 0;
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(15, 25);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(110, 35);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "+ Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.Location = new System.Drawing.Point(130, 25);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(110, 35);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "✏ Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(245, 25);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(110, 35);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "🗑 Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            // 
            // groupOperacionesMasivas
            // 
            this.groupOperacionesMasivas.Controls.Add(this.btnPromocionAlumnos);
            this.groupOperacionesMasivas.Controls.Add(this.btnImportarExportar);
            this.groupOperacionesMasivas.Controls.Add(this.btnEditarGrado);
            this.groupOperacionesMasivas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupOperacionesMasivas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.groupOperacionesMasivas.Location = new System.Drawing.Point(20, 410);
            this.groupOperacionesMasivas.Name = "groupOperacionesMasivas";
            this.groupOperacionesMasivas.Size = new System.Drawing.Size(760, 70);
            this.groupOperacionesMasivas.TabIndex = 12;
            this.groupOperacionesMasivas.TabStop = false;
            this.groupOperacionesMasivas.Text = "Operaciones Masivas";
            // 
            // btnPromocionAlumnos
            // 
            this.btnPromocionAlumnos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnPromocionAlumnos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPromocionAlumnos.FlatAppearance.BorderSize = 0;
            this.btnPromocionAlumnos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPromocionAlumnos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPromocionAlumnos.ForeColor = System.Drawing.Color.White;
            this.btnPromocionAlumnos.Location = new System.Drawing.Point(580, 25);
            this.btnPromocionAlumnos.Name = "btnPromocionAlumnos";
            this.btnPromocionAlumnos.Size = new System.Drawing.Size(160, 35);
            this.btnPromocionAlumnos.TabIndex = 2;
            this.btnPromocionAlumnos.Text = "🔄 Promoción";
            this.btnPromocionAlumnos.UseVisualStyleBackColor = false;
            // 
            // btnImportarExportar
            // 
            this.btnImportarExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnImportarExportar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportarExportar.FlatAppearance.BorderSize = 0;
            this.btnImportarExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportarExportar.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnImportarExportar.ForeColor = System.Drawing.Color.White;
            this.btnImportarExportar.Location = new System.Drawing.Point(25, 25);
            this.btnImportarExportar.Name = "btnImportarExportar";
            this.btnImportarExportar.Size = new System.Drawing.Size(180, 35);
            this.btnImportarExportar.TabIndex = 1;
            this.btnImportarExportar.Text = "📂 Importar/Exportar ▼";
            this.btnImportarExportar.UseVisualStyleBackColor = false;
            // 
            // btnEditarGrado
            // 
            this.btnEditarGrado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.btnEditarGrado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditarGrado.FlatAppearance.BorderSize = 0;
            this.btnEditarGrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditarGrado.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnEditarGrado.ForeColor = System.Drawing.Color.White;
            this.btnEditarGrado.Location = new System.Drawing.Point(400, 25);
            this.btnEditarGrado.Name = "btnEditarGrado";
            this.btnEditarGrado.Size = new System.Drawing.Size(170, 35);
            this.btnEditarGrado.TabIndex = 0;
            this.btnEditarGrado.Text = "↔ Cambiar Grado";
            this.btnEditarGrado.UseVisualStyleBackColor = false;
            // 
            // tabHistorial
            // 
            this.tabHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.tabHistorial.Controls.Add(this.lblAnioLectivoHistorial);
            this.tabHistorial.Controls.Add(this.cmbAnioLectivoHistorial);
            this.tabHistorial.Controls.Add(this.lblGradoHistorial);
            this.tabHistorial.Controls.Add(this.cmbGradoHistorial);
            this.tabHistorial.Controls.Add(this.btnLimpiarFiltrosHistorial);
            this.tabHistorial.Controls.Add(this.panelEstadisticasHistorial);
            this.tabHistorial.Controls.Add(this.lblListaHistorial);
            this.tabHistorial.Controls.Add(this.dgvHistorial);
            this.tabHistorial.Controls.Add(this.btnVerTrayectoria);
            this.tabHistorial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabHistorial.Location = new System.Drawing.Point(4, 24);
            this.tabHistorial.Name = "tabHistorial";
            this.tabHistorial.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistorial.Size = new System.Drawing.Size(792, 492);
            this.tabHistorial.TabIndex = 1;
            this.tabHistorial.Text = "Historial por Año";
            // 
            // lblAnioLectivoHistorial
            // 
            this.lblAnioLectivoHistorial.AutoSize = true;
            this.lblAnioLectivoHistorial.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAnioLectivoHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblAnioLectivoHistorial.Location = new System.Drawing.Point(20, 20);
            this.lblAnioLectivoHistorial.Name = "lblAnioLectivoHistorial";
            this.lblAnioLectivoHistorial.Size = new System.Drawing.Size(76, 15);
            this.lblAnioLectivoHistorial.TabIndex = 0;
            this.lblAnioLectivoHistorial.Text = "Año Lectivo:";
            // 
            // cmbAnioLectivoHistorial
            // 
            this.cmbAnioLectivoHistorial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnioLectivoHistorial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbAnioLectivoHistorial.FormattingEnabled = true;
            this.cmbAnioLectivoHistorial.Location = new System.Drawing.Point(110, 17);
            this.cmbAnioLectivoHistorial.Name = "cmbAnioLectivoHistorial";
            this.cmbAnioLectivoHistorial.Size = new System.Drawing.Size(120, 23);
            this.cmbAnioLectivoHistorial.TabIndex = 1;
            // 
            // lblGradoHistorial
            // 
            this.lblGradoHistorial.AutoSize = true;
            this.lblGradoHistorial.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGradoHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblGradoHistorial.Location = new System.Drawing.Point(250, 20);
            this.lblGradoHistorial.Name = "lblGradoHistorial";
            this.lblGradoHistorial.Size = new System.Drawing.Size(44, 15);
            this.lblGradoHistorial.TabIndex = 2;
            this.lblGradoHistorial.Text = "Grado:";
            this.lblGradoHistorial.Click += new System.EventHandler(this.lblGradoHistorial_Click);
            // 
            // cmbGradoHistorial
            // 
            this.cmbGradoHistorial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradoHistorial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbGradoHistorial.FormattingEnabled = true;
            this.cmbGradoHistorial.Location = new System.Drawing.Point(305, 17);
            this.cmbGradoHistorial.Name = "cmbGradoHistorial";
            this.cmbGradoHistorial.Size = new System.Drawing.Size(140, 23);
            this.cmbGradoHistorial.TabIndex = 3;
            // 
            // btnLimpiarFiltrosHistorial
            // 
            this.btnLimpiarFiltrosHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnLimpiarFiltrosHistorial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiarFiltrosHistorial.FlatAppearance.BorderSize = 0;
            this.btnLimpiarFiltrosHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiarFiltrosHistorial.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnLimpiarFiltrosHistorial.ForeColor = System.Drawing.Color.White;
            this.btnLimpiarFiltrosHistorial.Location = new System.Drawing.Point(460, 16);
            this.btnLimpiarFiltrosHistorial.Name = "btnLimpiarFiltrosHistorial";
            this.btnLimpiarFiltrosHistorial.Size = new System.Drawing.Size(90, 25);
            this.btnLimpiarFiltrosHistorial.TabIndex = 4;
            this.btnLimpiarFiltrosHistorial.Text = "Limpiar";
            this.btnLimpiarFiltrosHistorial.UseVisualStyleBackColor = false;
            // 
            // panelEstadisticasHistorial
            // 
            this.panelEstadisticasHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelEstadisticasHistorial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEstadisticasHistorial.Controls.Add(this.lblEstadisticasHistorial);
            this.panelEstadisticasHistorial.Location = new System.Drawing.Point(20, 55);
            this.panelEstadisticasHistorial.Name = "panelEstadisticasHistorial";
            this.panelEstadisticasHistorial.Size = new System.Drawing.Size(760, 60);
            this.panelEstadisticasHistorial.TabIndex = 5;
            // 
            // lblEstadisticasHistorial
            // 
            this.lblEstadisticasHistorial.AutoSize = true;
            this.lblEstadisticasHistorial.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblEstadisticasHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblEstadisticasHistorial.Location = new System.Drawing.Point(10, 10);
            this.lblEstadisticasHistorial.Name = "lblEstadisticasHistorial";
            this.lblEstadisticasHistorial.Size = new System.Drawing.Size(156, 15);
            this.lblEstadisticasHistorial.TabIndex = 0;
            this.lblEstadisticasHistorial.Text = "📊 Seleccione un año lectivo";
            // 
            // lblListaHistorial
            // 
            this.lblListaHistorial.AutoSize = true;
            this.lblListaHistorial.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblListaHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblListaHistorial.Location = new System.Drawing.Point(20, 130);
            this.lblListaHistorial.Name = "lblListaHistorial";
            this.lblListaHistorial.Size = new System.Drawing.Size(124, 15);
            this.lblListaHistorial.TabIndex = 6;
            this.lblListaHistorial.Text = "Historial de Alumnos:";
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.AllowUserToAddRows = false;
            this.dgvHistorial.AllowUserToDeleteRows = false;
            this.dgvHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistorial.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new System.Drawing.Point(20, 155);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorial.Size = new System.Drawing.Size(760, 280);
            this.dgvHistorial.TabIndex = 7;
            // 
            // btnVerTrayectoria
            // 
            this.btnVerTrayectoria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnVerTrayectoria.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerTrayectoria.FlatAppearance.BorderSize = 0;
            this.btnVerTrayectoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerTrayectoria.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVerTrayectoria.ForeColor = System.Drawing.Color.White;
            this.btnVerTrayectoria.Location = new System.Drawing.Point(20, 445);
            this.btnVerTrayectoria.Name = "btnVerTrayectoria";
            this.btnVerTrayectoria.Size = new System.Drawing.Size(180, 35);
            this.btnVerTrayectoria.TabIndex = 8;
            this.btnVerTrayectoria.Text = "📋 Ver Trayectoria";
            this.btnVerTrayectoria.UseVisualStyleBackColor = false;
            // 
            // menuImportarExportar
            // 
            this.menuImportarExportar.Name = "menuImportarExportar";
            this.menuImportarExportar.Size = new System.Drawing.Size(61, 4);
            // 
            // gestionAlumnos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(800, 520);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "gestionAlumnos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestionar Alumnos";
            this.tabControl.ResumeLayout(false);
            this.tabGestionActual.ResumeLayout(false);
            this.tabGestionActual.PerformLayout();
            this.panelEstadisticas.ResumeLayout(false);
            this.panelEstadisticas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).EndInit();
            this.groupOperacionesAlumno.ResumeLayout(false);
            this.groupOperacionesMasivas.ResumeLayout(false);
            this.tabHistorial.ResumeLayout(false);
            this.tabHistorial.PerformLayout();
            this.panelEstadisticasHistorial.ResumeLayout(false);
            this.panelEstadisticasHistorial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGestionActual;
        private System.Windows.Forms.Label separator;
        private System.Windows.Forms.Label lblGrado;
        private System.Windows.Forms.ComboBox cmbGrado;
        private System.Windows.Forms.Button btnNuevoGrado;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Panel panelEstadisticas;
        private System.Windows.Forms.Label lblEstadisticas;
        private System.Windows.Forms.Label lblListaAlumnos;
        private System.Windows.Forms.DataGridView dgvAlumnos;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox groupOperacionesAlumno;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.GroupBox groupOperacionesMasivas;
        private System.Windows.Forms.Button btnPromocionAlumnos;
        private System.Windows.Forms.Button btnImportarExportar;
        private System.Windows.Forms.ContextMenuStrip menuImportarExportar;
        private System.Windows.Forms.Button btnEditarGrado;
        private System.Windows.Forms.TabPage tabHistorial;
        private System.Windows.Forms.Label lblAnioLectivoHistorial;
        private System.Windows.Forms.ComboBox cmbAnioLectivoHistorial;
        private System.Windows.Forms.Label lblGradoHistorial;
        private System.Windows.Forms.ComboBox cmbGradoHistorial;
        private System.Windows.Forms.Button btnLimpiarFiltrosHistorial;
        private System.Windows.Forms.Panel panelEstadisticasHistorial;
        private System.Windows.Forms.Label lblEstadisticasHistorial;
        private System.Windows.Forms.Label lblListaHistorial;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Button btnVerTrayectoria;
    }
}
