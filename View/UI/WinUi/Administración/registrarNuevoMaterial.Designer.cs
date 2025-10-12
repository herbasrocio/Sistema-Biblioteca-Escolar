namespace UI.WinUi.Administrador
{
    partial class registrarNuevoMaterial
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxDatosMaterial = new System.Windows.Forms.GroupBox();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.numEjemplares = new System.Windows.Forms.NumericUpDown();
            this.lblEjemplares = new System.Windows.Forms.Label();
            this.comboBoxEdadRecomendada = new System.Windows.Forms.ComboBox();
            this.lblEdadRecomendada = new System.Windows.Forms.Label();
            this.txtAnioPublicacion = new System.Windows.Forms.TextBox();
            this.lblAnioPublicacion = new System.Windows.Forms.Label();
            this.txtEditorial = new System.Windows.Forms.TextBox();
            this.lblEditorial = new System.Windows.Forms.Label();
            this.comboBoxGenero = new System.Windows.Forms.ComboBox();
            this.lblGenero = new System.Windows.Forms.Label();
            this.comboBoxTipo = new System.Windows.Forms.ComboBox();
            this.txtISBN = new System.Windows.Forms.TextBox();
            this.txtAutor = new System.Windows.Forms.TextBox();
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblISBN = new System.Windows.Forms.Label();
            this.lblAutor = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvMateriales = new System.Windows.Forms.DataGridView();
            this.groupBoxAcciones = new System.Windows.Forms.GroupBox();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.groupBoxDatosMaterial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEjemplares)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriales)).BeginInit();
            this.groupBoxAcciones.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBoxDatosMaterial
            //
            this.groupBoxDatosMaterial.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxDatosMaterial.Controls.Add(this.txtDescripcion);
            this.groupBoxDatosMaterial.Controls.Add(this.lblDescripcion);
            this.groupBoxDatosMaterial.Controls.Add(this.numEjemplares);
            this.groupBoxDatosMaterial.Controls.Add(this.lblEjemplares);
            this.groupBoxDatosMaterial.Controls.Add(this.comboBoxEdadRecomendada);
            this.groupBoxDatosMaterial.Controls.Add(this.lblEdadRecomendada);
            this.groupBoxDatosMaterial.Controls.Add(this.txtAnioPublicacion);
            this.groupBoxDatosMaterial.Controls.Add(this.lblAnioPublicacion);
            this.groupBoxDatosMaterial.Controls.Add(this.txtEditorial);
            this.groupBoxDatosMaterial.Controls.Add(this.lblEditorial);
            this.groupBoxDatosMaterial.Controls.Add(this.comboBoxGenero);
            this.groupBoxDatosMaterial.Controls.Add(this.lblGenero);
            this.groupBoxDatosMaterial.Controls.Add(this.comboBoxTipo);
            this.groupBoxDatosMaterial.Controls.Add(this.txtISBN);
            this.groupBoxDatosMaterial.Controls.Add(this.txtAutor);
            this.groupBoxDatosMaterial.Controls.Add(this.txtTitulo);
            this.groupBoxDatosMaterial.Controls.Add(this.lblTipo);
            this.groupBoxDatosMaterial.Controls.Add(this.lblISBN);
            this.groupBoxDatosMaterial.Controls.Add(this.lblAutor);
            this.groupBoxDatosMaterial.Controls.Add(this.lblTitulo);
            this.groupBoxDatosMaterial.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxDatosMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxDatosMaterial.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDatosMaterial.Name = "groupBoxDatosMaterial";
            this.groupBoxDatosMaterial.Size = new System.Drawing.Size(310, 545);
            this.groupBoxDatosMaterial.TabIndex = 0;
            this.groupBoxDatosMaterial.TabStop = false;
            this.groupBoxDatosMaterial.Text = "Datos del Material";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescripcion.Location = new System.Drawing.Point(20, 455);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescripcion.Size = new System.Drawing.Size(270, 80);
            this.txtDescripcion.TabIndex = 19;
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblDescripcion.Location = new System.Drawing.Point(20, 435);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(72, 15);
            this.lblDescripcion.TabIndex = 18;
            this.lblDescripcion.Text = "Descripción:";
            //
            // numEjemplares
            //
            this.numEjemplares.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numEjemplares.Location = new System.Drawing.Point(140, 405);
            this.numEjemplares.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numEjemplares.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEjemplares.Name = "numEjemplares";
            this.numEjemplares.Size = new System.Drawing.Size(150, 23);
            this.numEjemplares.TabIndex = 17;
            this.numEjemplares.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // lblEjemplares
            //
            this.lblEjemplares.AutoSize = true;
            this.lblEjemplares.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEjemplares.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblEjemplares.Location = new System.Drawing.Point(20, 408);
            this.lblEjemplares.Name = "lblEjemplares";
            this.lblEjemplares.Size = new System.Drawing.Size(59, 15);
            this.lblEjemplares.TabIndex = 16;
            this.lblEjemplares.Text = "Cantidad:";
            //
            // comboBoxEdadRecomendada
            //
            this.comboBoxEdadRecomendada.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEdadRecomendada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxEdadRecomendada.FormattingEnabled = true;
            this.comboBoxEdadRecomendada.Location = new System.Drawing.Point(140, 360);
            this.comboBoxEdadRecomendada.Name = "comboBoxEdadRecomendada";
            this.comboBoxEdadRecomendada.Size = new System.Drawing.Size(150, 23);
            this.comboBoxEdadRecomendada.TabIndex = 15;
            //
            // lblEdadRecomendada
            //
            this.lblEdadRecomendada.AutoSize = true;
            this.lblEdadRecomendada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEdadRecomendada.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblEdadRecomendada.Location = new System.Drawing.Point(20, 363);
            this.lblEdadRecomendada.Name = "lblEdadRecomendada";
            this.lblEdadRecomendada.Size = new System.Drawing.Size(111, 15);
            this.lblEdadRecomendada.TabIndex = 14;
            this.lblEdadRecomendada.Text = "Edad Recomendada:";
            //
            // txtAnioPublicacion
            //
            this.txtAnioPublicacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAnioPublicacion.Location = new System.Drawing.Point(140, 315);
            this.txtAnioPublicacion.MaxLength = 4;
            this.txtAnioPublicacion.Name = "txtAnioPublicacion";
            this.txtAnioPublicacion.Size = new System.Drawing.Size(150, 23);
            this.txtAnioPublicacion.TabIndex = 13;
            //
            // lblAnioPublicacion
            //
            this.lblAnioPublicacion.AutoSize = true;
            this.lblAnioPublicacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAnioPublicacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblAnioPublicacion.Location = new System.Drawing.Point(20, 318);
            this.lblAnioPublicacion.Name = "lblAnioPublicacion";
            this.lblAnioPublicacion.Size = new System.Drawing.Size(108, 15);
            this.lblAnioPublicacion.TabIndex = 12;
            this.lblAnioPublicacion.Text = "Año de Publicación:";
            //
            // txtEditorial
            //
            this.txtEditorial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEditorial.Location = new System.Drawing.Point(140, 270);
            this.txtEditorial.Name = "txtEditorial";
            this.txtEditorial.Size = new System.Drawing.Size(150, 23);
            this.txtEditorial.TabIndex = 11;
            //
            // lblEditorial
            //
            this.lblEditorial.AutoSize = true;
            this.lblEditorial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEditorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblEditorial.Location = new System.Drawing.Point(20, 273);
            this.lblEditorial.Name = "lblEditorial";
            this.lblEditorial.Size = new System.Drawing.Size(53, 15);
            this.lblEditorial.TabIndex = 10;
            this.lblEditorial.Text = "Editorial:";
            //
            // comboBoxGenero
            //
            this.comboBoxGenero.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenero.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxGenero.FormattingEnabled = true;
            this.comboBoxGenero.Location = new System.Drawing.Point(140, 180);
            this.comboBoxGenero.Name = "comboBoxGenero";
            this.comboBoxGenero.Size = new System.Drawing.Size(150, 23);
            this.comboBoxGenero.TabIndex = 9;
            //
            // lblGenero
            //
            this.lblGenero.AutoSize = true;
            this.lblGenero.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGenero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblGenero.Location = new System.Drawing.Point(20, 183);
            this.lblGenero.Name = "lblGenero";
            this.lblGenero.Size = new System.Drawing.Size(48, 15);
            this.lblGenero.TabIndex = 8;
            this.lblGenero.Text = "Género:";
            //
            // comboBoxTipo
            //
            this.comboBoxTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxTipo.FormattingEnabled = true;
            this.comboBoxTipo.Location = new System.Drawing.Point(140, 135);
            this.comboBoxTipo.Name = "comboBoxTipo";
            this.comboBoxTipo.Size = new System.Drawing.Size(150, 23);
            this.comboBoxTipo.TabIndex = 7;
            //
            // txtISBN
            //
            this.txtISBN.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtISBN.Location = new System.Drawing.Point(140, 225);
            this.txtISBN.Name = "txtISBN";
            this.txtISBN.Size = new System.Drawing.Size(150, 23);
            this.txtISBN.TabIndex = 6;
            //
            // txtAutor
            //
            this.txtAutor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAutor.Location = new System.Drawing.Point(20, 90);
            this.txtAutor.Name = "txtAutor";
            this.txtAutor.Size = new System.Drawing.Size(270, 23);
            this.txtAutor.TabIndex = 5;
            //
            // txtTitulo
            //
            this.txtTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTitulo.Location = new System.Drawing.Point(20, 45);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(270, 23);
            this.txtTitulo.TabIndex = 4;
            //
            // lblTipo
            //
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblTipo.Location = new System.Drawing.Point(20, 138);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(33, 15);
            this.lblTipo.TabIndex = 3;
            this.lblTipo.Text = "Tipo:";
            //
            // lblISBN
            //
            this.lblISBN.AutoSize = true;
            this.lblISBN.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblISBN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblISBN.Location = new System.Drawing.Point(20, 228);
            this.lblISBN.Name = "lblISBN";
            this.lblISBN.Size = new System.Drawing.Size(35, 15);
            this.lblISBN.TabIndex = 2;
            this.lblISBN.Text = "ISBN:";
            //
            // lblAutor
            //
            this.lblAutor.AutoSize = true;
            this.lblAutor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAutor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblAutor.Location = new System.Drawing.Point(20, 70);
            this.lblAutor.Name = "lblAutor";
            this.lblAutor.Size = new System.Drawing.Size(40, 15);
            this.lblAutor.TabIndex = 1;
            this.lblAutor.Text = "Autor:";
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 25);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(40, 15);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Título:";
            //
            // dgvMateriales
            //
            this.dgvMateriales.AllowUserToAddRows = false;
            this.dgvMateriales.AllowUserToDeleteRows = false;
            this.dgvMateriales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMateriales.BackgroundColor = System.Drawing.Color.White;
            this.dgvMateriales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvMateriales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMateriales.Location = new System.Drawing.Point(340, 12);
            this.dgvMateriales.MultiSelect = false;
            this.dgvMateriales.Name = "dgvMateriales";
            this.dgvMateriales.ReadOnly = true;
            this.dgvMateriales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMateriales.Size = new System.Drawing.Size(640, 640);
            this.dgvMateriales.TabIndex = 1;
            //
            // groupBoxAcciones
            //
            this.groupBoxAcciones.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxAcciones.Controls.Add(this.btnVolver);
            this.groupBoxAcciones.Controls.Add(this.btnEliminar);
            this.groupBoxAcciones.Controls.Add(this.btnModificar);
            this.groupBoxAcciones.Controls.Add(this.btnGuardar);
            this.groupBoxAcciones.Controls.Add(this.btnNuevo);
            this.groupBoxAcciones.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxAcciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxAcciones.Location = new System.Drawing.Point(12, 573);
            this.groupBoxAcciones.Name = "groupBoxAcciones";
            this.groupBoxAcciones.Size = new System.Drawing.Size(310, 80);
            this.groupBoxAcciones.TabIndex = 2;
            this.groupBoxAcciones.TabStop = false;
            this.groupBoxAcciones.Text = "Acciones";
            //
            // btnVolver
            //
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(235, 28);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(60, 35);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            //
            // btnEliminar
            //
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(159, 28);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(70, 35);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            //
            // btnModificar
            //
            this.btnModificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.btnModificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnModificar.ForeColor = System.Drawing.Color.White;
            this.btnModificar.Location = new System.Drawing.Point(81, 28);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(72, 35);
            this.btnModificar.TabIndex = 2;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = false;
            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(157, 28);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(138, 35);
            this.btnGuardar.TabIndex = 1;
            this.btnGuardar.Text = "Guardar Material";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Visible = false;
            //
            // btnNuevo
            //
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(15, 28);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(60, 35);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            //
            // registrarNuevoMaterial
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1000, 670);
            this.Controls.Add(this.groupBoxAcciones);
            this.Controls.Add(this.dgvMateriales);
            this.Controls.Add(this.groupBoxDatosMaterial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "registrarNuevoMaterial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registrar material";
            this.Load += new System.EventHandler(this.registrarNuevoMaterial_Load);
            this.groupBoxDatosMaterial.ResumeLayout(false);
            this.groupBoxDatosMaterial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEjemplares)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriales)).EndInit();
            this.groupBoxAcciones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDatosMaterial;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.NumericUpDown numEjemplares;
        private System.Windows.Forms.Label lblEjemplares;
        private System.Windows.Forms.ComboBox comboBoxEdadRecomendada;
        private System.Windows.Forms.Label lblEdadRecomendada;
        private System.Windows.Forms.TextBox txtAnioPublicacion;
        private System.Windows.Forms.Label lblAnioPublicacion;
        private System.Windows.Forms.TextBox txtEditorial;
        private System.Windows.Forms.Label lblEditorial;
        private System.Windows.Forms.ComboBox comboBoxGenero;
        private System.Windows.Forms.Label lblGenero;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblAutor;
        private System.Windows.Forms.Label lblISBN;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.TextBox txtAutor;
        private System.Windows.Forms.TextBox txtISBN;
        private System.Windows.Forms.ComboBox comboBoxTipo;
        private System.Windows.Forms.DataGridView dgvMateriales;
        private System.Windows.Forms.GroupBox groupBoxAcciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnVolver;
    }
}
