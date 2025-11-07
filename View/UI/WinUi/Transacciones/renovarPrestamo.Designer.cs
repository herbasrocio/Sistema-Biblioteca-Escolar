namespace UI.WinUi.Transacciones
{
    partial class renovarPrestamo
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
            this.groupBoxBusqueda = new System.Windows.Forms.GroupBox();
            this.lblBuscarEjemplar = new System.Windows.Forms.Label();
            this.txtBuscarEjemplar = new System.Windows.Forms.TextBox();
            this.lblBuscarTitulo = new System.Windows.Forms.Label();
            this.txtBuscarTitulo = new System.Windows.Forms.TextBox();
            this.lblBuscarAlumno = new System.Windows.Forms.Label();
            this.txtBuscarAlumno = new System.Windows.Forms.TextBox();
            this.dgvPrestamos = new System.Windows.Forms.DataGridView();
            this.groupBoxDatos = new System.Windows.Forms.GroupBox();
            this.lblNuevaFechaDevolucion = new System.Windows.Forms.Label();
            this.txtNuevaFechaDevolucion = new System.Windows.Forms.TextBox();
            this.lblDiasExtension = new System.Windows.Forms.Label();
            this.numDiasExtension = new System.Windows.Forms.NumericUpDown();
            this.lblRenovaciones = new System.Windows.Forms.Label();
            this.txtRenovaciones = new System.Windows.Forms.TextBox();
            this.lblFechaDevolucionActual = new System.Windows.Forms.Label();
            this.txtFechaDevolucionActual = new System.Windows.Forms.TextBox();
            this.btnRenovar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.lblTotalPrestamos = new System.Windows.Forms.Label();
            this.groupBoxBusqueda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos)).BeginInit();
            this.groupBoxDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiasExtension)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxBusqueda
            // 
            this.groupBoxBusqueda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarEjemplar);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarEjemplar);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarTitulo);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarTitulo);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarAlumno);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarAlumno);
            this.groupBoxBusqueda.Controls.Add(this.dgvPrestamos);
            this.groupBoxBusqueda.Location = new System.Drawing.Point(12, 12);
            this.groupBoxBusqueda.Name = "groupBoxBusqueda";
            this.groupBoxBusqueda.Size = new System.Drawing.Size(838, 212);
            this.groupBoxBusqueda.TabIndex = 0;
            this.groupBoxBusqueda.TabStop = false;
            this.groupBoxBusqueda.Text = "Buscar Préstamos";
            // 
            // lblBuscarEjemplar
            // 
            this.lblBuscarEjemplar.AutoSize = true;
            this.lblBuscarEjemplar.Location = new System.Drawing.Point(508, 18);
            this.lblBuscarEjemplar.Name = "lblBuscarEjemplar";
            this.lblBuscarEjemplar.Size = new System.Drawing.Size(86, 13);
            this.lblBuscarEjemplar.TabIndex = 6;
            this.lblBuscarEjemplar.Text = "Código Ejemplar:";
            // 
            // txtBuscarEjemplar
            // 
            this.txtBuscarEjemplar.Location = new System.Drawing.Point(511, 35);
            this.txtBuscarEjemplar.Name = "txtBuscarEjemplar";
            this.txtBuscarEjemplar.Size = new System.Drawing.Size(306, 20);
            this.txtBuscarEjemplar.TabIndex = 5;
            // 
            // lblBuscarTitulo
            // 
            this.lblBuscarTitulo.AutoSize = true;
            this.lblBuscarTitulo.Location = new System.Drawing.Point(263, 18);
            this.lblBuscarTitulo.Name = "lblBuscarTitulo";
            this.lblBuscarTitulo.Size = new System.Drawing.Size(38, 13);
            this.lblBuscarTitulo.TabIndex = 4;
            this.lblBuscarTitulo.Text = "Título:";
            // 
            // txtBuscarTitulo
            // 
            this.txtBuscarTitulo.Location = new System.Drawing.Point(266, 35);
            this.txtBuscarTitulo.Name = "txtBuscarTitulo";
            this.txtBuscarTitulo.Size = new System.Drawing.Size(227, 20);
            this.txtBuscarTitulo.TabIndex = 3;
            // 
            // lblBuscarAlumno
            // 
            this.lblBuscarAlumno.AutoSize = true;
            this.lblBuscarAlumno.Location = new System.Drawing.Point(15, 18);
            this.lblBuscarAlumno.Name = "lblBuscarAlumno";
            this.lblBuscarAlumno.Size = new System.Drawing.Size(45, 13);
            this.lblBuscarAlumno.TabIndex = 2;
            this.lblBuscarAlumno.Text = "Alumno:";
            // 
            // txtBuscarAlumno
            // 
            this.txtBuscarAlumno.Location = new System.Drawing.Point(18, 35);
            this.txtBuscarAlumno.Name = "txtBuscarAlumno";
            this.txtBuscarAlumno.Size = new System.Drawing.Size(230, 20);
            this.txtBuscarAlumno.TabIndex = 1;
            // 
            // dgvPrestamos
            // 
            this.dgvPrestamos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPrestamos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrestamos.Location = new System.Drawing.Point(15, 65);
            this.dgvPrestamos.Name = "dgvPrestamos";
            this.dgvPrestamos.Size = new System.Drawing.Size(804, 141);
            this.dgvPrestamos.TabIndex = 0;
            // 
            // groupBoxDatos
            // 
            this.groupBoxDatos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDatos.Controls.Add(this.lblNuevaFechaDevolucion);
            this.groupBoxDatos.Controls.Add(this.txtNuevaFechaDevolucion);
            this.groupBoxDatos.Controls.Add(this.lblDiasExtension);
            this.groupBoxDatos.Controls.Add(this.numDiasExtension);
            this.groupBoxDatos.Controls.Add(this.lblRenovaciones);
            this.groupBoxDatos.Controls.Add(this.txtRenovaciones);
            this.groupBoxDatos.Controls.Add(this.lblFechaDevolucionActual);
            this.groupBoxDatos.Controls.Add(this.txtFechaDevolucionActual);
            this.groupBoxDatos.Location = new System.Drawing.Point(10, 233);
            this.groupBoxDatos.Name = "groupBoxDatos";
            this.groupBoxDatos.Size = new System.Drawing.Size(840, 70);
            this.groupBoxDatos.TabIndex = 1;
            this.groupBoxDatos.TabStop = false;
            this.groupBoxDatos.Text = "Datos de Renovación";
            // 
            // lblNuevaFechaDevolucion
            // 
            this.lblNuevaFechaDevolucion.AutoSize = true;
            this.lblNuevaFechaDevolucion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblNuevaFechaDevolucion.Location = new System.Drawing.Point(519, 19);
            this.lblNuevaFechaDevolucion.Name = "lblNuevaFechaDevolucion";
            this.lblNuevaFechaDevolucion.Size = new System.Drawing.Size(169, 15);
            this.lblNuevaFechaDevolucion.TabIndex = 15;
            this.lblNuevaFechaDevolucion.Text = "Nueva Fecha Devolución:";
            // 
            // txtNuevaFechaDevolucion
            // 
            this.txtNuevaFechaDevolucion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.txtNuevaFechaDevolucion.ForeColor = System.Drawing.Color.Green;
            this.txtNuevaFechaDevolucion.Location = new System.Drawing.Point(519, 39);
            this.txtNuevaFechaDevolucion.Name = "txtNuevaFechaDevolucion";
            this.txtNuevaFechaDevolucion.ReadOnly = true;
            this.txtNuevaFechaDevolucion.Size = new System.Drawing.Size(300, 21);
            this.txtNuevaFechaDevolucion.TabIndex = 14;
            this.txtNuevaFechaDevolucion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDiasExtension
            // 
            this.lblDiasExtension.AutoSize = true;
            this.lblDiasExtension.Location = new System.Drawing.Point(367, 21);
            this.lblDiasExtension.Name = "lblDiasExtension";
            this.lblDiasExtension.Size = new System.Drawing.Size(96, 13);
            this.lblDiasExtension.TabIndex = 13;
            this.lblDiasExtension.Text = "Días de extensión:";
            // 
            // numDiasExtension
            // 
            this.numDiasExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.numDiasExtension.Location = new System.Drawing.Point(367, 41);
            this.numDiasExtension.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numDiasExtension.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDiasExtension.Name = "numDiasExtension";
            this.numDiasExtension.Size = new System.Drawing.Size(98, 21);
            this.numDiasExtension.TabIndex = 12;
            this.numDiasExtension.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.numDiasExtension.ValueChanged += new System.EventHandler(this.NumDiasExtension_ValueChanged);
            // 
            // lblRenovaciones
            // 
            this.lblRenovaciones.AutoSize = true;
            this.lblRenovaciones.Location = new System.Drawing.Point(189, 24);
            this.lblRenovaciones.Name = "lblRenovaciones";
            this.lblRenovaciones.Size = new System.Drawing.Size(129, 13);
            this.lblRenovaciones.TabIndex = 11;
            this.lblRenovaciones.Text = "Renovaciones realizadas:";
            // 
            // txtRenovaciones
            // 
            this.txtRenovaciones.Location = new System.Drawing.Point(189, 41);
            this.txtRenovaciones.Name = "txtRenovaciones";
            this.txtRenovaciones.ReadOnly = true;
            this.txtRenovaciones.Size = new System.Drawing.Size(145, 20);
            this.txtRenovaciones.TabIndex = 10;
            this.txtRenovaciones.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRenovaciones.TextChanged += new System.EventHandler(this.txtRenovaciones_TextChanged);
            // 
            // lblFechaDevolucionActual
            // 
            this.lblFechaDevolucionActual.AutoSize = true;
            this.lblFechaDevolucionActual.Location = new System.Drawing.Point(18, 24);
            this.lblFechaDevolucionActual.Name = "lblFechaDevolucionActual";
            this.lblFechaDevolucionActual.Size = new System.Drawing.Size(130, 13);
            this.lblFechaDevolucionActual.TabIndex = 9;
            this.lblFechaDevolucionActual.Text = "Fecha Devolución Actual:";
            // 
            // txtFechaDevolucionActual
            // 
            this.txtFechaDevolucionActual.Location = new System.Drawing.Point(18, 41);
            this.txtFechaDevolucionActual.Name = "txtFechaDevolucionActual";
            this.txtFechaDevolucionActual.ReadOnly = true;
            this.txtFechaDevolucionActual.Size = new System.Drawing.Size(138, 20);
            this.txtFechaDevolucionActual.TabIndex = 8;
            this.txtFechaDevolucionActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnRenovar
            // 
            this.btnRenovar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRenovar.Enabled = false;
            this.btnRenovar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenovar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRenovar.ForeColor = System.Drawing.Color.White;
            this.btnRenovar.Location = new System.Drawing.Point(176, 309);
            this.btnRenovar.Name = "btnRenovar";
            this.btnRenovar.Size = new System.Drawing.Size(180, 35);
            this.btnRenovar.TabIndex = 2;
            this.btnRenovar.Text = "Renovar Préstamo";
            this.btnRenovar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnLimpiar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnLimpiar.Location = new System.Drawing.Point(376, 309);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(150, 35);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // btnVolver
            // 
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnVolver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnVolver.Location = new System.Drawing.Point(546, 309);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(150, 35);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            // 
            // lblTotalPrestamos
            // 
            this.lblTotalPrestamos.AutoSize = true;
            this.lblTotalPrestamos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.lblTotalPrestamos.Location = new System.Drawing.Point(9, 458);
            this.lblTotalPrestamos.Name = "lblTotalPrestamos";
            this.lblTotalPrestamos.Size = new System.Drawing.Size(112, 13);
            this.lblTotalPrestamos.TabIndex = 7;
            this.lblTotalPrestamos.Text = "Total préstamos: 0";
            // 
            // renovarPrestamo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1000, 460);
            this.Controls.Add(this.lblTotalPrestamos);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnRenovar);
            this.Controls.Add(this.groupBoxDatos);
            this.Controls.Add(this.groupBoxBusqueda);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1000, 460);
            this.MinimumSize = new System.Drawing.Size(1000, 460);
            this.Name = "renovarPrestamo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Renovar Préstamo";
            this.groupBoxBusqueda.ResumeLayout(false);
            this.groupBoxBusqueda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos)).EndInit();
            this.groupBoxDatos.ResumeLayout(false);
            this.groupBoxDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiasExtension)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxBusqueda;
        private System.Windows.Forms.DataGridView dgvPrestamos;
        private System.Windows.Forms.Label lblBuscarAlumno;
        private System.Windows.Forms.TextBox txtBuscarAlumno;
        private System.Windows.Forms.Label lblBuscarTitulo;
        private System.Windows.Forms.TextBox txtBuscarTitulo;
        private System.Windows.Forms.Label lblBuscarEjemplar;
        private System.Windows.Forms.TextBox txtBuscarEjemplar;
        private System.Windows.Forms.GroupBox groupBoxDatos;
        private System.Windows.Forms.Label lblFechaDevolucionActual;
        private System.Windows.Forms.TextBox txtFechaDevolucionActual;
        private System.Windows.Forms.Label lblRenovaciones;
        private System.Windows.Forms.TextBox txtRenovaciones;
        private System.Windows.Forms.Label lblDiasExtension;
        private System.Windows.Forms.NumericUpDown numDiasExtension;
        private System.Windows.Forms.Label lblNuevaFechaDevolucion;
        private System.Windows.Forms.TextBox txtNuevaFechaDevolucion;
        private System.Windows.Forms.Button btnRenovar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label lblTotalPrestamos;
    }
}
