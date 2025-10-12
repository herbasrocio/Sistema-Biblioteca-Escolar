namespace UI.WinUi.Administrador
{
    partial class EditarMaterial
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
            this.numCantidad = new System.Windows.Forms.NumericUpDown();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.comboBoxNivel = new System.Windows.Forms.ComboBox();
            this.lblNivel = new System.Windows.Forms.Label();
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
            this.groupBoxAcciones = new System.Windows.Forms.GroupBox();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.groupBoxDatosMaterial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).BeginInit();
            this.groupBoxAcciones.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBoxDatosMaterial
            //
            this.groupBoxDatosMaterial.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxDatosMaterial.Controls.Add(this.numCantidad);
            this.groupBoxDatosMaterial.Controls.Add(this.lblCantidad);
            this.groupBoxDatosMaterial.Controls.Add(this.comboBoxNivel);
            this.groupBoxDatosMaterial.Controls.Add(this.lblNivel);
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
            this.groupBoxDatosMaterial.Location = new System.Drawing.Point(30, 30);
            this.groupBoxDatosMaterial.Name = "groupBoxDatosMaterial";
            this.groupBoxDatosMaterial.Size = new System.Drawing.Size(310, 460);
            this.groupBoxDatosMaterial.TabIndex = 0;
            this.groupBoxDatosMaterial.TabStop = false;
            this.groupBoxDatosMaterial.Text = "Datos del Material";
            //
            // numCantidad
            //
            this.numCantidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numCantidad.Location = new System.Drawing.Point(140, 405);
            this.numCantidad.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCantidad.Name = "numCantidad";
            this.numCantidad.Size = new System.Drawing.Size(150, 23);
            this.numCantidad.TabIndex = 17;
            this.numCantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // lblCantidad
            //
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblCantidad.Location = new System.Drawing.Point(20, 408);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(59, 15);
            this.lblCantidad.TabIndex = 16;
            this.lblCantidad.Text = "Cantidad:";
            //
            // comboBoxNivel
            //
            this.comboBoxNivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNivel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxNivel.FormattingEnabled = true;
            this.comboBoxNivel.Location = new System.Drawing.Point(140, 360);
            this.comboBoxNivel.Name = "comboBoxNivel";
            this.comboBoxNivel.Size = new System.Drawing.Size(150, 23);
            this.comboBoxNivel.TabIndex = 15;
            //
            // lblNivel
            //
            this.lblNivel.AutoSize = true;
            this.lblNivel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNivel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblNivel.Location = new System.Drawing.Point(20, 363);
            this.lblNivel.Name = "lblNivel";
            this.lblNivel.Size = new System.Drawing.Size(38, 15);
            this.lblNivel.TabIndex = 14;
            this.lblNivel.Text = "Nivel:";
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
            // groupBoxAcciones
            //
            this.groupBoxAcciones.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxAcciones.Controls.Add(this.btnVolver);
            this.groupBoxAcciones.Controls.Add(this.btnEliminar);
            this.groupBoxAcciones.Controls.Add(this.btnGuardar);
            this.groupBoxAcciones.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxAcciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxAcciones.Location = new System.Drawing.Point(30, 505);
            this.groupBoxAcciones.Name = "groupBoxAcciones";
            this.groupBoxAcciones.Size = new System.Drawing.Size(310, 80);
            this.groupBoxAcciones.TabIndex = 1;
            this.groupBoxAcciones.TabStop = false;
            this.groupBoxAcciones.Text = "Acciones";
            //
            // btnVolver
            //
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(210, 28);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(85, 35);
            this.btnVolver.TabIndex = 2;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            //
            // btnEliminar
            //
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(113, 28);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(85, 35);
            this.btnEliminar.TabIndex = 1;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(15, 28);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(85, 35);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            //
            // EditarMaterial
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(370, 615);
            this.Controls.Add(this.groupBoxAcciones);
            this.Controls.Add(this.groupBoxDatosMaterial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EditarMaterial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editar Material";
            this.groupBoxDatosMaterial.ResumeLayout(false);
            this.groupBoxDatosMaterial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).EndInit();
            this.groupBoxAcciones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDatosMaterial;
        private System.Windows.Forms.NumericUpDown numCantidad;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.ComboBox comboBoxNivel;
        private System.Windows.Forms.Label lblNivel;
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
        private System.Windows.Forms.GroupBox groupBoxAcciones;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnVolver;
    }
}
