namespace UI.WinUi.Reportes
{
    partial class ReportePrestamosActivos
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelControles = new System.Windows.Forms.Panel();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblDesde = new System.Windows.Forms.Label();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.btnExportarCsv = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.dgvReporte = new System.Windows.Forms.DataGridView();
            this.lblEstadisticas = new System.Windows.Forms.Label();
            this.panelControles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(300, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reporte de Préstamos Activos";
            //
            // panelControles
            //
            this.panelControles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelControles.Controls.Add(this.cmbEstado);
            this.panelControles.Controls.Add(this.lblEstado);
            this.panelControles.Controls.Add(this.dtpHasta);
            this.panelControles.Controls.Add(this.lblHasta);
            this.panelControles.Controls.Add(this.dtpDesde);
            this.panelControles.Controls.Add(this.lblDesde);
            this.panelControles.Controls.Add(this.btnGenerar);
            this.panelControles.Controls.Add(this.btnExportarCsv);
            this.panelControles.Controls.Add(this.btnVolver);
            this.panelControles.Location = new System.Drawing.Point(20, 60);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(1140, 80);
            this.panelControles.TabIndex = 1;
            //
            // lblDesde
            //
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(10, 10);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(80, 15);
            this.lblDesde.TabIndex = 0;
            this.lblDesde.Text = "Fecha Desde:";
            //
            // dtpDesde
            //
            this.dtpDesde.Checked = false;
            this.dtpDesde.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(10, 30);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.ShowCheckBox = true;
            this.dtpDesde.Size = new System.Drawing.Size(150, 23);
            this.dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(180, 10);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(76, 15);
            this.lblHasta.TabIndex = 2;
            this.lblHasta.Text = "Fecha Hasta:";
            //
            // dtpHasta
            //
            this.dtpHasta.Checked = false;
            this.dtpHasta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(180, 30);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.ShowCheckBox = true;
            this.dtpHasta.Size = new System.Drawing.Size(150, 23);
            this.dtpHasta.TabIndex = 3;
            //
            // lblEstado
            //
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(350, 10);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(47, 15);
            this.lblEstado.TabIndex = 4;
            this.lblEstado.Text = "Estado:";
            //
            // cmbEstado
            //
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(350, 30);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(150, 23);
            this.cmbEstado.TabIndex = 5;
            //
            // btnGenerar
            //
            this.btnGenerar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.ForeColor = System.Drawing.Color.White;
            this.btnGenerar.Location = new System.Drawing.Point(520, 25);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(120, 30);
            this.btnGenerar.TabIndex = 6;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            //
            // btnExportarCsv
            //
            this.btnExportarCsv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnExportarCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportarCsv.Location = new System.Drawing.Point(660, 25);
            this.btnExportarCsv.Name = "btnExportarCsv";
            this.btnExportarCsv.Size = new System.Drawing.Size(120, 30);
            this.btnExportarCsv.TabIndex = 7;
            this.btnExportarCsv.Text = "Exportar CSV";
            this.btnExportarCsv.UseVisualStyleBackColor = false;
            this.btnExportarCsv.Click += new System.EventHandler(this.btnExportarCsv_Click);
            //
            // btnVolver
            //
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(800, 25);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(120, 30);
            this.btnVolver.TabIndex = 8;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            //
            // dgvReporte
            //
            this.dgvReporte.AllowUserToAddRows = false;
            this.dgvReporte.AllowUserToDeleteRows = false;
            this.dgvReporte.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReporte.Location = new System.Drawing.Point(20, 160);
            this.dgvReporte.Name = "dgvReporte";
            this.dgvReporte.ReadOnly = true;
            this.dgvReporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReporte.Size = new System.Drawing.Size(1140, 400);
            this.dgvReporte.TabIndex = 2;
            //
            // lblEstadisticas
            //
            this.lblEstadisticas.AutoSize = true;
            this.lblEstadisticas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstadisticas.Location = new System.Drawing.Point(20, 570);
            this.lblEstadisticas.Name = "lblEstadisticas";
            this.lblEstadisticas.Size = new System.Drawing.Size(400, 19);
            this.lblEstadisticas.TabIndex = 3;
            this.lblEstadisticas.Text = "Total: 0 | Vigentes: 0 | Por Vencer: 0 | Vencidos: 0";
            //
            // ReportePrestamosActivos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 610);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.panelControles);
            this.Controls.Add(this.dgvReporte);
            this.Controls.Add(this.lblEstadisticas);
            this.Name = "ReportePrestamosActivos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de Préstamos Activos";
            this.Load += new System.EventHandler(this.ReportePrestamosActivos_Load);
            this.panelControles.ResumeLayout(false);
            this.panelControles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelControles;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Button btnExportarCsv;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.DataGridView dgvReporte;
        private System.Windows.Forms.Label lblEstadisticas;
    }
}
