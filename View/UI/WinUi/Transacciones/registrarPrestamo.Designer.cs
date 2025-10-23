namespace UI.WinUi.Transacciones
{
    partial class registrarPrestamo
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
            this.lblGradoDivision = new System.Windows.Forms.Label();
            this.cmbGradoDivision = new System.Windows.Forms.ComboBox();
            this.lblAlumno = new System.Windows.Forms.Label();
            this.cmbAlumno = new System.Windows.Forms.ComboBox();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.lblFiltrarPor = new System.Windows.Forms.Label();
            this.cmbFiltrarPor = new System.Windows.Forms.ComboBox();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblSeleccionarMaterial = new System.Windows.Forms.Label();
            this.dgvMateriales = new System.Windows.Forms.DataGridView();
            this.lblFechaPrestamo = new System.Windows.Forms.Label();
            this.dtpFechaPrestamo = new System.Windows.Forms.DateTimePicker();
            this.lblFechaDevolucion = new System.Windows.Forms.Label();
            this.dtpFechaDevolucion = new System.Windows.Forms.DateTimePicker();
            this.lblUbicacion = new System.Windows.Forms.Label();
            this.btnConfirmarPrestamo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriales)).BeginInit();
            this.SuspendLayout();
            // 
            // lblGradoDivision
            //
            this.lblGradoDivision.AutoSize = true;
            this.lblGradoDivision.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblGradoDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblGradoDivision.Location = new System.Drawing.Point(30, 25);
            this.lblGradoDivision.Name = "lblGradoDivision";
            this.lblGradoDivision.Size = new System.Drawing.Size(113, 17);
            this.lblGradoDivision.TabIndex = 0;
            this.lblGradoDivision.Text = "Grado / División:";
            //
            // cmbGradoDivision
            //
            this.cmbGradoDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradoDivision.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbGradoDivision.FormattingEnabled = true;
            this.cmbGradoDivision.Location = new System.Drawing.Point(155, 22);
            this.cmbGradoDivision.Name = "cmbGradoDivision";
            this.cmbGradoDivision.Size = new System.Drawing.Size(200, 25);
            this.cmbGradoDivision.TabIndex = 1;
            //
            // lblAlumno
            //
            this.lblAlumno.AutoSize = true;
            this.lblAlumno.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblAlumno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblAlumno.Location = new System.Drawing.Point(385, 25);
            this.lblAlumno.Name = "lblAlumno";
            this.lblAlumno.Size = new System.Drawing.Size(63, 17);
            this.lblAlumno.TabIndex = 2;
            this.lblAlumno.Text = "Alumno:";
            //
            // cmbAlumno
            //
            this.cmbAlumno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlumno.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbAlumno.FormattingEnabled = true;
            this.cmbAlumno.Location = new System.Drawing.Point(460, 22);
            this.cmbAlumno.Name = "cmbAlumno";
            this.cmbAlumno.Size = new System.Drawing.Size(390, 25);
            this.cmbAlumno.TabIndex = 3;
            //
            // lblMaterial
            //
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lblMaterial.Location = new System.Drawing.Point(30, 70);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(157, 20);
            this.lblMaterial.TabIndex = 4;
            this.lblMaterial.Text = "Búsqueda de Material";
            //
            // lblFiltrarPor
            //
            this.lblFiltrarPor.AutoSize = true;
            this.lblFiltrarPor.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblFiltrarPor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblFiltrarPor.Location = new System.Drawing.Point(30, 108);
            this.lblFiltrarPor.Name = "lblFiltrarPor";
            this.lblFiltrarPor.Size = new System.Drawing.Size(68, 17);
            this.lblFiltrarPor.TabIndex = 5;
            this.lblFiltrarPor.Text = "Filtrar por:";
            //
            // cmbFiltrarPor
            //
            this.cmbFiltrarPor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltrarPor.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbFiltrarPor.FormattingEnabled = true;
            this.cmbFiltrarPor.Location = new System.Drawing.Point(105, 105);
            this.cmbFiltrarPor.Name = "cmbFiltrarPor";
            this.cmbFiltrarPor.Size = new System.Drawing.Size(200, 25);
            this.cmbFiltrarPor.TabIndex = 6;
            //
            // lblBuscar
            //
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblBuscar.Location = new System.Drawing.Point(335, 108);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(51, 17);
            this.lblBuscar.TabIndex = 7;
            this.lblBuscar.Text = "Buscar:";
            //
            // txtBuscar
            //
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtBuscar.Location = new System.Drawing.Point(395, 105);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(455, 25);
            this.txtBuscar.TabIndex = 8;
            // 
            // lblSeleccionarMaterial
            //
            this.lblSeleccionarMaterial.AutoSize = true;
            this.lblSeleccionarMaterial.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSeleccionarMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblSeleccionarMaterial.Location = new System.Drawing.Point(30, 150);
            this.lblSeleccionarMaterial.Name = "lblSeleccionarMaterial";
            this.lblSeleccionarMaterial.Size = new System.Drawing.Size(226, 17);
            this.lblSeleccionarMaterial.TabIndex = 9;
            this.lblSeleccionarMaterial.Text = "Seleccione el material a prestar:";
            // 
            // dgvMateriales
            //
            this.dgvMateriales.AllowUserToAddRows = false;
            this.dgvMateriales.AllowUserToDeleteRows = false;
            this.dgvMateriales.AllowUserToResizeRows = false;
            this.dgvMateriales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMateriales.BackgroundColor = System.Drawing.Color.White;
            this.dgvMateriales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvMateriales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMateriales.Location = new System.Drawing.Point(30, 175);
            this.dgvMateriales.MultiSelect = false;
            this.dgvMateriales.Name = "dgvMateriales";
            this.dgvMateriales.ReadOnly = true;
            this.dgvMateriales.RowHeadersVisible = false;
            this.dgvMateriales.RowTemplate.Height = 25;
            this.dgvMateriales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMateriales.Size = new System.Drawing.Size(820, 250);
            this.dgvMateriales.TabIndex = 10;
            // 
            // lblFechaPrestamo
            // 
            this.lblFechaPrestamo.AutoSize = true;
            this.lblFechaPrestamo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFechaPrestamo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblFechaPrestamo.Location = new System.Drawing.Point(175, 454);
            this.lblFechaPrestamo.Name = "lblFechaPrestamo";
            this.lblFechaPrestamo.Size = new System.Drawing.Size(107, 15);
            this.lblFechaPrestamo.TabIndex = 12;
            this.lblFechaPrestamo.Text = "Fecha de préstamo";
            // 
            // dtpFechaPrestamo
            // 
            this.dtpFechaPrestamo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaPrestamo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaPrestamo.Location = new System.Drawing.Point(290, 451);
            this.dtpFechaPrestamo.Name = "dtpFechaPrestamo";
            this.dtpFechaPrestamo.Size = new System.Drawing.Size(150, 23);
            this.dtpFechaPrestamo.TabIndex = 13;
            // 
            // lblFechaDevolucion
            // 
            this.lblFechaDevolucion.AutoSize = true;
            this.lblFechaDevolucion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFechaDevolucion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblFechaDevolucion.Location = new System.Drawing.Point(475, 454);
            this.lblFechaDevolucion.Name = "lblFechaDevolucion";
            this.lblFechaDevolucion.Size = new System.Drawing.Size(116, 15);
            this.lblFechaDevolucion.TabIndex = 14;
            this.lblFechaDevolucion.Text = "Fecha de devolución";
            // 
            // dtpFechaDevolucion
            // 
            this.dtpFechaDevolucion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFechaDevolucion.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaDevolucion.Location = new System.Drawing.Point(595, 451);
            this.dtpFechaDevolucion.Name = "dtpFechaDevolucion";
            this.dtpFechaDevolucion.Size = new System.Drawing.Size(150, 23);
            this.dtpFechaDevolucion.TabIndex = 15;
            // 
            // lblUbicacion
            // 
            this.lblUbicacion.AutoSize = true;
            this.lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblUbicacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lblUbicacion.Location = new System.Drawing.Point(592, 457);
            this.lblUbicacion.Name = "lblUbicacion";
            this.lblUbicacion.Size = new System.Drawing.Size(0, 17);
            this.lblUbicacion.TabIndex = 11;
            // 
            // btnConfirmarPrestamo
            // 
            this.btnConfirmarPrestamo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnConfirmarPrestamo.FlatAppearance.BorderSize = 0;
            this.btnConfirmarPrestamo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarPrestamo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnConfirmarPrestamo.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarPrestamo.Location = new System.Drawing.Point(332, 494);
            this.btnConfirmarPrestamo.Name = "btnConfirmarPrestamo";
            this.btnConfirmarPrestamo.Size = new System.Drawing.Size(240, 40);
            this.btnConfirmarPrestamo.TabIndex = 16;
            this.btnConfirmarPrestamo.Text = "Confirmar préstamo";
            this.btnConfirmarPrestamo.UseVisualStyleBackColor = false;
            // 
            // registrarPrestamo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(886, 550);
            this.Controls.Add(this.lblUbicacion);
            this.Controls.Add(this.btnConfirmarPrestamo);
            this.Controls.Add(this.dtpFechaDevolucion);
            this.Controls.Add(this.lblFechaDevolucion);
            this.Controls.Add(this.dtpFechaPrestamo);
            this.Controls.Add(this.lblFechaPrestamo);
            this.Controls.Add(this.dgvMateriales);
            this.Controls.Add(this.lblSeleccionarMaterial);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.cmbFiltrarPor);
            this.Controls.Add(this.lblFiltrarPor);
            this.Controls.Add(this.lblMaterial);
            this.Controls.Add(this.cmbAlumno);
            this.Controls.Add(this.lblAlumno);
            this.Controls.Add(this.cmbGradoDivision);
            this.Controls.Add(this.lblGradoDivision);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "registrarPrestamo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registrar préstamo";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriales)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGradoDivision;
        private System.Windows.Forms.ComboBox cmbGradoDivision;
        private System.Windows.Forms.Label lblAlumno;
        private System.Windows.Forms.ComboBox cmbAlumno;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.Label lblFiltrarPor;
        private System.Windows.Forms.ComboBox cmbFiltrarPor;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label lblSeleccionarMaterial;
        private System.Windows.Forms.DataGridView dgvMateriales;
        private System.Windows.Forms.Label lblFechaPrestamo;
        private System.Windows.Forms.DateTimePicker dtpFechaPrestamo;
        private System.Windows.Forms.Label lblFechaDevolucion;
        private System.Windows.Forms.DateTimePicker dtpFechaDevolucion;
        private System.Windows.Forms.Label lblUbicacion;
        private System.Windows.Forms.Button btnConfirmarPrestamo;
    }
}
