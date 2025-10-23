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
            this.lblTotalPrestamos = new System.Windows.Forms.Label();
            this.lblBuscarEjemplar = new System.Windows.Forms.Label();
            this.txtBuscarEjemplar = new System.Windows.Forms.TextBox();
            this.lblBuscarTitulo = new System.Windows.Forms.Label();
            this.txtBuscarTitulo = new System.Windows.Forms.TextBox();
            this.lblBuscarAlumno = new System.Windows.Forms.Label();
            this.txtBuscarAlumno = new System.Windows.Forms.TextBox();
            this.dgvPrestamos = new System.Windows.Forms.DataGridView();
            this.groupBoxDatos = new System.Windows.Forms.GroupBox();
            this.lblAdvertencia = new System.Windows.Forms.Label();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
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
            this.groupBoxBusqueda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos)).BeginInit();
            this.groupBoxDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiasExtension)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxBusqueda
            // 
            this.groupBoxBusqueda.Controls.Add(this.lblTotalPrestamos);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarEjemplar);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarEjemplar);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarTitulo);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarTitulo);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarAlumno);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarAlumno);
            this.groupBoxBusqueda.Controls.Add(this.dgvPrestamos);
            this.groupBoxBusqueda.Location = new System.Drawing.Point(12, 12);
            this.groupBoxBusqueda.Name = "groupBoxBusqueda";
            this.groupBoxBusqueda.Size = new System.Drawing.Size(980, 350);
            this.groupBoxBusqueda.TabIndex = 0;
            this.groupBoxBusqueda.TabStop = false;
            this.groupBoxBusqueda.Text = "Buscar Préstamos";
            // 
            // lblTotalPrestamos
            // 
            this.lblTotalPrestamos.AutoSize = true;
            this.lblTotalPrestamos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalPrestamos.Location = new System.Drawing.Point(15, 320);
            this.lblTotalPrestamos.Name = "lblTotalPrestamos";
            this.lblTotalPrestamos.Size = new System.Drawing.Size(126, 15);
            this.lblTotalPrestamos.TabIndex = 7;
            this.lblTotalPrestamos.Text = "Total préstamos: 0";
            // 
            // lblBuscarEjemplar
            // 
            this.lblBuscarEjemplar.AutoSize = true;
            this.lblBuscarEjemplar.Location = new System.Drawing.Point(620, 28);
            this.lblBuscarEjemplar.Name = "lblBuscarEjemplar";
            this.lblBuscarEjemplar.Size = new System.Drawing.Size(86, 13);
            this.lblBuscarEjemplar.TabIndex = 6;
            this.lblBuscarEjemplar.Text = "Código Ejemplar:";
            // 
            // txtBuscarEjemplar
            // 
            this.txtBuscarEjemplar.Location = new System.Drawing.Point(623, 45);
            this.txtBuscarEjemplar.Name = "txtBuscarEjemplar";
            this.txtBuscarEjemplar.Size = new System.Drawing.Size(340, 20);
            this.txtBuscarEjemplar.TabIndex = 5;
            // 
            // lblBuscarTitulo
            // 
            this.lblBuscarTitulo.AutoSize = true;
            this.lblBuscarTitulo.Location = new System.Drawing.Point(320, 28);
            this.lblBuscarTitulo.Name = "lblBuscarTitulo";
            this.lblBuscarTitulo.Size = new System.Drawing.Size(38, 13);
            this.lblBuscarTitulo.TabIndex = 4;
            this.lblBuscarTitulo.Text = "Título:";
            // 
            // txtBuscarTitulo
            // 
            this.txtBuscarTitulo.Location = new System.Drawing.Point(323, 45);
            this.txtBuscarTitulo.Name = "txtBuscarTitulo";
            this.txtBuscarTitulo.Size = new System.Drawing.Size(280, 20);
            this.txtBuscarTitulo.TabIndex = 3;
            // 
            // lblBuscarAlumno
            // 
            this.lblBuscarAlumno.AutoSize = true;
            this.lblBuscarAlumno.Location = new System.Drawing.Point(15, 28);
            this.lblBuscarAlumno.Name = "lblBuscarAlumno";
            this.lblBuscarAlumno.Size = new System.Drawing.Size(45, 13);
            this.lblBuscarAlumno.TabIndex = 2;
            this.lblBuscarAlumno.Text = "Alumno:";
            // 
            // txtBuscarAlumno
            // 
            this.txtBuscarAlumno.Location = new System.Drawing.Point(18, 45);
            this.txtBuscarAlumno.Name = "txtBuscarAlumno";
            this.txtBuscarAlumno.Size = new System.Drawing.Size(280, 20);
            this.txtBuscarAlumno.TabIndex = 1;
            // 
            // dgvPrestamos
            // 
            this.dgvPrestamos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrestamos.Location = new System.Drawing.Point(18, 80);
            this.dgvPrestamos.Name = "dgvPrestamos";
            this.dgvPrestamos.Size = new System.Drawing.Size(945, 230);
            this.dgvPrestamos.TabIndex = 0;
            // 
            // groupBoxDatos
            // 
            this.groupBoxDatos.Controls.Add(this.lblAdvertencia);
            this.groupBoxDatos.Controls.Add(this.lblObservaciones);
            this.groupBoxDatos.Controls.Add(this.txtObservaciones);
            this.groupBoxDatos.Controls.Add(this.lblNuevaFechaDevolucion);
            this.groupBoxDatos.Controls.Add(this.txtNuevaFechaDevolucion);
            this.groupBoxDatos.Controls.Add(this.lblDiasExtension);
            this.groupBoxDatos.Controls.Add(this.numDiasExtension);
            this.groupBoxDatos.Controls.Add(this.lblRenovaciones);
            this.groupBoxDatos.Controls.Add(this.txtRenovaciones);
            this.groupBoxDatos.Controls.Add(this.lblFechaDevolucionActual);
            this.groupBoxDatos.Controls.Add(this.txtFechaDevolucionActual);
            this.groupBoxDatos.Location = new System.Drawing.Point(12, 368);
            this.groupBoxDatos.Name = "groupBoxDatos";
            this.groupBoxDatos.Size = new System.Drawing.Size(980, 180);
            this.groupBoxDatos.TabIndex = 1;
            this.groupBoxDatos.TabStop = false;
            this.groupBoxDatos.Text = "Datos de Renovación";
            // 
            // lblAdvertencia
            // 
            this.lblAdvertencia.AutoSize = true;
            this.lblAdvertencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblAdvertencia.ForeColor = System.Drawing.Color.Red;
            this.lblAdvertencia.Location = new System.Drawing.Point(520, 25);
            this.lblAdvertencia.Name = "lblAdvertencia";
            this.lblAdvertencia.Size = new System.Drawing.Size(141, 15);
            this.lblAdvertencia.TabIndex = 18;
            this.lblAdvertencia.Text = "⚠ Préstamo vencido";
            this.lblAdvertencia.Visible = false;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(520, 25);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(81, 13);
            this.lblObservaciones.TabIndex = 17;
            this.lblObservaciones.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(523, 41);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(440, 120);
            this.txtObservaciones.TabIndex = 16;
            // 
            // lblNuevaFechaDevolucion
            // 
            this.lblNuevaFechaDevolucion.AutoSize = true;
            this.lblNuevaFechaDevolucion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblNuevaFechaDevolucion.Location = new System.Drawing.Point(270, 100);
            this.lblNuevaFechaDevolucion.Name = "lblNuevaFechaDevolucion";
            this.lblNuevaFechaDevolucion.Size = new System.Drawing.Size(169, 15);
            this.lblNuevaFechaDevolucion.TabIndex = 15;
            this.lblNuevaFechaDevolucion.Text = "Nueva Fecha Devolución:";
            // 
            // txtNuevaFechaDevolucion
            // 
            this.txtNuevaFechaDevolucion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.txtNuevaFechaDevolucion.ForeColor = System.Drawing.Color.Green;
            this.txtNuevaFechaDevolucion.Location = new System.Drawing.Point(270, 118);
            this.txtNuevaFechaDevolucion.Name = "txtNuevaFechaDevolucion";
            this.txtNuevaFechaDevolucion.ReadOnly = true;
            this.txtNuevaFechaDevolucion.Size = new System.Drawing.Size(230, 23);
            this.txtNuevaFechaDevolucion.TabIndex = 14;
            this.txtNuevaFechaDevolucion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDiasExtension
            // 
            this.lblDiasExtension.AutoSize = true;
            this.lblDiasExtension.Location = new System.Drawing.Point(15, 100);
            this.lblDiasExtension.Name = "lblDiasExtension";
            this.lblDiasExtension.Size = new System.Drawing.Size(96, 13);
            this.lblDiasExtension.TabIndex = 13;
            this.lblDiasExtension.Text = "Días de extensión:";
            // 
            // numDiasExtension
            // 
            this.numDiasExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.numDiasExtension.Location = new System.Drawing.Point(15, 118);
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
            this.numDiasExtension.Size = new System.Drawing.Size(230, 23);
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
            this.lblRenovaciones.Location = new System.Drawing.Point(270, 25);
            this.lblRenovaciones.Name = "lblRenovaciones";
            this.lblRenovaciones.Size = new System.Drawing.Size(129, 13);
            this.lblRenovaciones.TabIndex = 11;
            this.lblRenovaciones.Text = "Renovaciones realizadas:";
            // 
            // txtRenovaciones
            // 
            this.txtRenovaciones.Location = new System.Drawing.Point(270, 41);
            this.txtRenovaciones.Name = "txtRenovaciones";
            this.txtRenovaciones.ReadOnly = true;
            this.txtRenovaciones.Size = new System.Drawing.Size(230, 20);
            this.txtRenovaciones.TabIndex = 10;
            this.txtRenovaciones.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRenovaciones.TextChanged += new System.EventHandler(this.txtRenovaciones_TextChanged);
            // 
            // lblFechaDevolucionActual
            // 
            this.lblFechaDevolucionActual.AutoSize = true;
            this.lblFechaDevolucionActual.Location = new System.Drawing.Point(15, 25);
            this.lblFechaDevolucionActual.Name = "lblFechaDevolucionActual";
            this.lblFechaDevolucionActual.Size = new System.Drawing.Size(130, 13);
            this.lblFechaDevolucionActual.TabIndex = 9;
            this.lblFechaDevolucionActual.Text = "Fecha Devolución Actual:";
            // 
            // txtFechaDevolucionActual
            // 
            this.txtFechaDevolucionActual.Location = new System.Drawing.Point(15, 41);
            this.txtFechaDevolucionActual.Name = "txtFechaDevolucionActual";
            this.txtFechaDevolucionActual.ReadOnly = true;
            this.txtFechaDevolucionActual.Size = new System.Drawing.Size(230, 20);
            this.txtFechaDevolucionActual.TabIndex = 8;
            this.txtFechaDevolucionActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnRenovar
            //
            this.btnRenovar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRenovar.Enabled = false;
            this.btnRenovar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenovar.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnRenovar.ForeColor = System.Drawing.Color.White;
            this.btnRenovar.Location = new System.Drawing.Point(12, 560);
            this.btnRenovar.Name = "btnRenovar";
            this.btnRenovar.Size = new System.Drawing.Size(240, 45);
            this.btnRenovar.TabIndex = 2;
            this.btnRenovar.Text = "Renovar Préstamo";
            this.btnRenovar.UseVisualStyleBackColor = false;
            // 
            // btnLimpiar
            //
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular);
            this.btnLimpiar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnLimpiar.Location = new System.Drawing.Point(270, 560);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(240, 45);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // btnVolver
            //
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular);
            this.btnVolver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnVolver.Location = new System.Drawing.Point(752, 560);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(240, 45);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            //
            // renovarPrestamo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1004, 620);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnRenovar);
            this.Controls.Add(this.groupBoxDatos);
            this.Controls.Add(this.groupBoxBusqueda);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
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
        private System.Windows.Forms.Label lblTotalPrestamos;
        private System.Windows.Forms.GroupBox groupBoxDatos;
        private System.Windows.Forms.Label lblFechaDevolucionActual;
        private System.Windows.Forms.TextBox txtFechaDevolucionActual;
        private System.Windows.Forms.Label lblRenovaciones;
        private System.Windows.Forms.TextBox txtRenovaciones;
        private System.Windows.Forms.Label lblDiasExtension;
        private System.Windows.Forms.NumericUpDown numDiasExtension;
        private System.Windows.Forms.Label lblNuevaFechaDevolucion;
        private System.Windows.Forms.TextBox txtNuevaFechaDevolucion;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label lblAdvertencia;
        private System.Windows.Forms.Button btnRenovar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnVolver;
    }
}
