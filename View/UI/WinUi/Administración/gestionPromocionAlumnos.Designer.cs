namespace UI.WinUi.Administrador
{
    partial class gestionPromocionAlumnos
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
            this.lblAnioActual = new System.Windows.Forms.Label();
            this.numAnioActual = new System.Windows.Forms.NumericUpDown();
            this.lblAnioSiguiente = new System.Windows.Forms.Label();
            this.numAnioSiguiente = new System.Windows.Forms.NumericUpDown();
            this.btnCargarEstadisticas = new System.Windows.Forms.Button();
            this.dgvEstadisticas = new System.Windows.Forms.DataGridView();
            this.lblResumen = new System.Windows.Forms.Label();
            this.grpPromocionGrado = new System.Windows.Forms.GroupBox();
            this.lblGradoActual = new System.Windows.Forms.Label();
            this.cmbGradoActual = new System.Windows.Forms.ComboBox();
            this.lblDivisionActual = new System.Windows.Forms.Label();
            this.txtDivisionActual = new System.Windows.Forms.TextBox();
            this.lblGradoNuevo = new System.Windows.Forms.Label();
            this.cmbGradoNuevo = new System.Windows.Forms.ComboBox();
            this.lblDivisionNueva = new System.Windows.Forms.Label();
            this.txtDivisionNueva = new System.Windows.Forms.TextBox();
            this.btnPromocionarGrado = new System.Windows.Forms.Button();
            this.btnPromocionMasiva = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numAnioActual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnioSiguiente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstadisticas)).BeginInit();
            this.grpPromocionGrado.SuspendLayout();
            this.SuspendLayout();
            //
            // lblAnioActual
            //
            this.lblAnioActual.AutoSize = true;
            this.lblAnioActual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnioActual.Location = new System.Drawing.Point(30, 30);
            this.lblAnioActual.Name = "lblAnioActual";
            this.lblAnioActual.Size = new System.Drawing.Size(76, 17);
            this.lblAnioActual.TabIndex = 0;
            this.lblAnioActual.Text = "Año Actual:";
            //
            // numAnioActual
            //
            this.numAnioActual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numAnioActual.Location = new System.Drawing.Point(140, 28);
            this.numAnioActual.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numAnioActual.Minimum = new decimal(new int[] {
            2020,
            0,
            0,
            0});
            this.numAnioActual.Name = "numAnioActual";
            this.numAnioActual.Size = new System.Drawing.Size(120, 25);
            this.numAnioActual.TabIndex = 1;
            this.numAnioActual.Value = new decimal(new int[] {
            2025,
            0,
            0,
            0});
            //
            // lblAnioSiguiente
            //
            this.lblAnioSiguiente.AutoSize = true;
            this.lblAnioSiguiente.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnioSiguiente.Location = new System.Drawing.Point(300, 30);
            this.lblAnioSiguiente.Name = "lblAnioSiguiente";
            this.lblAnioSiguiente.Size = new System.Drawing.Size(95, 17);
            this.lblAnioSiguiente.TabIndex = 2;
            this.lblAnioSiguiente.Text = "Año Siguiente:";
            //
            // numAnioSiguiente
            //
            this.numAnioSiguiente.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numAnioSiguiente.Location = new System.Drawing.Point(410, 28);
            this.numAnioSiguiente.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numAnioSiguiente.Minimum = new decimal(new int[] {
            2020,
            0,
            0,
            0});
            this.numAnioSiguiente.Name = "numAnioSiguiente";
            this.numAnioSiguiente.Size = new System.Drawing.Size(120, 25);
            this.numAnioSiguiente.TabIndex = 3;
            this.numAnioSiguiente.Value = new decimal(new int[] {
            2026,
            0,
            0,
            0});
            //
            // btnCargarEstadisticas
            //
            this.btnCargarEstadisticas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnCargarEstadisticas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargarEstadisticas.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargarEstadisticas.ForeColor = System.Drawing.Color.White;
            this.btnCargarEstadisticas.Location = new System.Drawing.Point(570, 22);
            this.btnCargarEstadisticas.Name = "btnCargarEstadisticas";
            this.btnCargarEstadisticas.Size = new System.Drawing.Size(180, 35);
            this.btnCargarEstadisticas.TabIndex = 4;
            this.btnCargarEstadisticas.Text = "Cargar Estadísticas";
            this.btnCargarEstadisticas.UseVisualStyleBackColor = false;
            //
            // dgvEstadisticas
            //
            this.dgvEstadisticas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEstadisticas.Location = new System.Drawing.Point(30, 80);
            this.dgvEstadisticas.Name = "dgvEstadisticas";
            this.dgvEstadisticas.Size = new System.Drawing.Size(720, 250);
            this.dgvEstadisticas.TabIndex = 5;
            //
            // lblResumen
            //
            this.lblResumen.AutoSize = true;
            this.lblResumen.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumen.Location = new System.Drawing.Point(30, 340);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(200, 17);
            this.lblResumen.TabIndex = 6;
            this.lblResumen.Text = "Total de alumnos inscriptos: 0";
            //
            // grpPromocionGrado
            //
            this.grpPromocionGrado.Controls.Add(this.btnPromocionarGrado);
            this.grpPromocionGrado.Controls.Add(this.txtDivisionNueva);
            this.grpPromocionGrado.Controls.Add(this.lblDivisionNueva);
            this.grpPromocionGrado.Controls.Add(this.cmbGradoNuevo);
            this.grpPromocionGrado.Controls.Add(this.lblGradoNuevo);
            this.grpPromocionGrado.Controls.Add(this.txtDivisionActual);
            this.grpPromocionGrado.Controls.Add(this.lblDivisionActual);
            this.grpPromocionGrado.Controls.Add(this.cmbGradoActual);
            this.grpPromocionGrado.Controls.Add(this.lblGradoActual);
            this.grpPromocionGrado.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPromocionGrado.Location = new System.Drawing.Point(30, 380);
            this.grpPromocionGrado.Name = "grpPromocionGrado";
            this.grpPromocionGrado.Size = new System.Drawing.Size(720, 150);
            this.grpPromocionGrado.TabIndex = 7;
            this.grpPromocionGrado.TabStop = false;
            this.grpPromocionGrado.Text = "Promoción por Grado";
            //
            // lblGradoActual
            //
            this.lblGradoActual.AutoSize = true;
            this.lblGradoActual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGradoActual.Location = new System.Drawing.Point(20, 35);
            this.lblGradoActual.Name = "lblGradoActual";
            this.lblGradoActual.Size = new System.Drawing.Size(88, 17);
            this.lblGradoActual.TabIndex = 0;
            this.lblGradoActual.Text = "Grado Actual:";
            //
            // cmbGradoActual
            //
            this.cmbGradoActual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradoActual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGradoActual.FormattingEnabled = true;
            this.cmbGradoActual.Location = new System.Drawing.Point(130, 32);
            this.cmbGradoActual.Name = "cmbGradoActual";
            this.cmbGradoActual.Size = new System.Drawing.Size(100, 25);
            this.cmbGradoActual.TabIndex = 1;
            //
            // lblDivisionActual
            //
            this.lblDivisionActual.AutoSize = true;
            this.lblDivisionActual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDivisionActual.Location = new System.Drawing.Point(250, 35);
            this.lblDivisionActual.Name = "lblDivisionActual";
            this.lblDivisionActual.Size = new System.Drawing.Size(101, 17);
            this.lblDivisionActual.TabIndex = 2;
            this.lblDivisionActual.Text = "División Actual:";
            //
            // txtDivisionActual
            //
            this.txtDivisionActual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDivisionActual.Location = new System.Drawing.Point(360, 32);
            this.txtDivisionActual.MaxLength = 10;
            this.txtDivisionActual.Name = "txtDivisionActual";
            this.txtDivisionActual.Size = new System.Drawing.Size(100, 25);
            this.txtDivisionActual.TabIndex = 3;
            this.txtDivisionActual.Text = "A";
            //
            // lblGradoNuevo
            //
            this.lblGradoNuevo.AutoSize = true;
            this.lblGradoNuevo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGradoNuevo.Location = new System.Drawing.Point(20, 75);
            this.lblGradoNuevo.Name = "lblGradoNuevo";
            this.lblGradoNuevo.Size = new System.Drawing.Size(90, 17);
            this.lblGradoNuevo.TabIndex = 4;
            this.lblGradoNuevo.Text = "Grado Nuevo:";
            //
            // cmbGradoNuevo
            //
            this.cmbGradoNuevo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradoNuevo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGradoNuevo.FormattingEnabled = true;
            this.cmbGradoNuevo.Location = new System.Drawing.Point(130, 72);
            this.cmbGradoNuevo.Name = "cmbGradoNuevo";
            this.cmbGradoNuevo.Size = new System.Drawing.Size(100, 25);
            this.cmbGradoNuevo.TabIndex = 5;
            //
            // lblDivisionNueva
            //
            this.lblDivisionNueva.AutoSize = true;
            this.lblDivisionNueva.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDivisionNueva.Location = new System.Drawing.Point(250, 75);
            this.lblDivisionNueva.Name = "lblDivisionNueva";
            this.lblDivisionNueva.Size = new System.Drawing.Size(103, 17);
            this.lblDivisionNueva.TabIndex = 6;
            this.lblDivisionNueva.Text = "División Nueva:";
            //
            // txtDivisionNueva
            //
            this.txtDivisionNueva.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDivisionNueva.Location = new System.Drawing.Point(360, 72);
            this.txtDivisionNueva.MaxLength = 10;
            this.txtDivisionNueva.Name = "txtDivisionNueva";
            this.txtDivisionNueva.Size = new System.Drawing.Size(100, 25);
            this.txtDivisionNueva.TabIndex = 7;
            this.txtDivisionNueva.Text = "A";
            //
            // btnPromocionarGrado
            //
            this.btnPromocionarGrado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnPromocionarGrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPromocionarGrado.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromocionarGrado.ForeColor = System.Drawing.Color.White;
            this.btnPromocionarGrado.Location = new System.Drawing.Point(510, 35);
            this.btnPromocionarGrado.Name = "btnPromocionarGrado";
            this.btnPromocionarGrado.Size = new System.Drawing.Size(180, 60);
            this.btnPromocionarGrado.TabIndex = 8;
            this.btnPromocionarGrado.Text = "Promocionar Grado";
            this.btnPromocionarGrado.UseVisualStyleBackColor = false;
            //
            // btnPromocionMasiva
            //
            this.btnPromocionMasiva.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnPromocionMasiva.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPromocionMasiva.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromocionMasiva.ForeColor = System.Drawing.Color.White;
            this.btnPromocionMasiva.Location = new System.Drawing.Point(30, 550);
            this.btnPromocionMasiva.Name = "btnPromocionMasiva";
            this.btnPromocionMasiva.Size = new System.Drawing.Size(720, 50);
            this.btnPromocionMasiva.TabIndex = 8;
            this.btnPromocionMasiva.Text = "PROMOCIÓN MASIVA DE TODOS LOS GRADOS";
            this.btnPromocionMasiva.UseVisualStyleBackColor = false;
            //
            // gestionPromocionAlumnos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(780, 620);
            this.Controls.Add(this.btnPromocionMasiva);
            this.Controls.Add(this.grpPromocionGrado);
            this.Controls.Add(this.lblResumen);
            this.Controls.Add(this.dgvEstadisticas);
            this.Controls.Add(this.btnCargarEstadisticas);
            this.Controls.Add(this.numAnioSiguiente);
            this.Controls.Add(this.lblAnioSiguiente);
            this.Controls.Add(this.numAnioActual);
            this.Controls.Add(this.lblAnioActual);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "gestionPromocionAlumnos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Promoción de Alumnos";
            ((System.ComponentModel.ISupportInitialize)(this.numAnioActual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAnioSiguiente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstadisticas)).EndInit();
            this.grpPromocionGrado.ResumeLayout(false);
            this.grpPromocionGrado.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAnioActual;
        private System.Windows.Forms.NumericUpDown numAnioActual;
        private System.Windows.Forms.Label lblAnioSiguiente;
        private System.Windows.Forms.NumericUpDown numAnioSiguiente;
        private System.Windows.Forms.Button btnCargarEstadisticas;
        private System.Windows.Forms.DataGridView dgvEstadisticas;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.GroupBox grpPromocionGrado;
        private System.Windows.Forms.Label lblGradoActual;
        private System.Windows.Forms.ComboBox cmbGradoActual;
        private System.Windows.Forms.Label lblDivisionActual;
        private System.Windows.Forms.TextBox txtDivisionActual;
        private System.Windows.Forms.Label lblGradoNuevo;
        private System.Windows.Forms.ComboBox cmbGradoNuevo;
        private System.Windows.Forms.Label lblDivisionNueva;
        private System.Windows.Forms.TextBox txtDivisionNueva;
        private System.Windows.Forms.Button btnPromocionarGrado;
        private System.Windows.Forms.Button btnPromocionMasiva;
    }
}
