namespace UI.WinUi.Transacciones
{
    partial class registrarDevolucion
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvPrestamos = new System.Windows.Forms.DataGridView();
            this.lblResultados = new System.Windows.Forms.Label();
            this.groupBoxBusqueda = new System.Windows.Forms.GroupBox();
            this.txtBuscarAlumno = new System.Windows.Forms.TextBox();
            this.txtBuscarTitulo = new System.Windows.Forms.TextBox();
            this.txtBuscarEjemplar = new System.Windows.Forms.TextBox();
            this.lblBuscarAlumno = new System.Windows.Forms.Label();
            this.lblBuscarTitulo = new System.Windows.Forms.Label();
            this.lblBuscarEjemplar = new System.Windows.Forms.Label();
            this.groupBoxDatos = new System.Windows.Forms.GroupBox();
            this.lblUbicacion = new System.Windows.Forms.Label();
            this.lblFechaDevolucionPrevista = new System.Windows.Forms.Label();
            this.lblFechaPrestamo = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos)).BeginInit();
            this.groupBoxBusqueda.SuspendLayout();
            this.groupBoxDatos.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(260, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "📚 Préstamos Activos y Vencidos";
            //
            // groupBoxBusqueda
            //
            this.groupBoxBusqueda.BackColor = System.Drawing.Color.White;
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarAlumno);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarAlumno);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarTitulo);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarTitulo);
            this.groupBoxBusqueda.Controls.Add(this.lblBuscarEjemplar);
            this.groupBoxBusqueda.Controls.Add(this.txtBuscarEjemplar);
            this.groupBoxBusqueda.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBoxBusqueda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.groupBoxBusqueda.Location = new System.Drawing.Point(20, 45);
            this.groupBoxBusqueda.Name = "groupBoxBusqueda";
            this.groupBoxBusqueda.Size = new System.Drawing.Size(760, 110);
            this.groupBoxBusqueda.TabIndex = 1;
            this.groupBoxBusqueda.TabStop = false;
            this.groupBoxBusqueda.Text = "🔍 Buscar Préstamo";
            //
            // lblBuscarAlumno
            //
            this.lblBuscarAlumno.AutoSize = true;
            this.lblBuscarAlumno.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblBuscarAlumno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblBuscarAlumno.Location = new System.Drawing.Point(20, 30);
            this.lblBuscarAlumno.Name = "lblBuscarAlumno";
            this.lblBuscarAlumno.Size = new System.Drawing.Size(108, 17);
            this.lblBuscarAlumno.TabIndex = 0;
            this.lblBuscarAlumno.Text = "Nombre Alumno:";
            //
            // txtBuscarAlumno
            //
            this.txtBuscarAlumno.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtBuscarAlumno.Location = new System.Drawing.Point(20, 50);
            this.txtBuscarAlumno.Name = "txtBuscarAlumno";
            this.txtBuscarAlumno.Size = new System.Drawing.Size(230, 25);
            this.txtBuscarAlumno.TabIndex = 1;
            //
            // lblBuscarTitulo
            //
            this.lblBuscarTitulo.AutoSize = true;
            this.lblBuscarTitulo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblBuscarTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblBuscarTitulo.Location = new System.Drawing.Point(270, 30);
            this.lblBuscarTitulo.Name = "lblBuscarTitulo";
            this.lblBuscarTitulo.Size = new System.Drawing.Size(96, 17);
            this.lblBuscarTitulo.TabIndex = 2;
            this.lblBuscarTitulo.Text = "Título Material:";
            //
            // txtBuscarTitulo
            //
            this.txtBuscarTitulo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtBuscarTitulo.Location = new System.Drawing.Point(270, 50);
            this.txtBuscarTitulo.Name = "txtBuscarTitulo";
            this.txtBuscarTitulo.Size = new System.Drawing.Size(230, 25);
            this.txtBuscarTitulo.TabIndex = 3;
            //
            // lblBuscarEjemplar
            //
            this.lblBuscarEjemplar.AutoSize = true;
            this.lblBuscarEjemplar.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblBuscarEjemplar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblBuscarEjemplar.Location = new System.Drawing.Point(520, 30);
            this.lblBuscarEjemplar.Name = "lblBuscarEjemplar";
            this.lblBuscarEjemplar.Size = new System.Drawing.Size(116, 17);
            this.lblBuscarEjemplar.TabIndex = 4;
            this.lblBuscarEjemplar.Text = "Código Ejemplar:";
            //
            // txtBuscarEjemplar
            //
            this.txtBuscarEjemplar.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtBuscarEjemplar.Location = new System.Drawing.Point(520, 50);
            this.txtBuscarEjemplar.Name = "txtBuscarEjemplar";
            this.txtBuscarEjemplar.Size = new System.Drawing.Size(220, 25);
            this.txtBuscarEjemplar.TabIndex = 5;
            //
            // dgvPrestamos
            //
            this.dgvPrestamos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrestamos.Location = new System.Drawing.Point(20, 190);
            this.dgvPrestamos.Name = "dgvPrestamos";
            this.dgvPrestamos.Size = new System.Drawing.Size(760, 210);
            this.dgvPrestamos.TabIndex = 3;
            this.dgvPrestamos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrestamos_CellContentClick);
            //
            // lblResultados
            //
            this.lblResultados.AutoSize = true;
            this.lblResultados.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblResultados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblResultados.Location = new System.Drawing.Point(20, 165);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new System.Drawing.Size(0, 17);
            this.lblResultados.TabIndex = 2;
            // 
            // groupBoxDatos
            // 
            this.groupBoxDatos.BackColor = System.Drawing.Color.White;
            this.groupBoxDatos.Controls.Add(this.lblUbicacion);
            this.groupBoxDatos.Controls.Add(this.lblFechaDevolucionPrevista);
            this.groupBoxDatos.Controls.Add(this.lblFechaPrestamo);
            this.groupBoxDatos.Controls.Add(this.lblEstado);
            this.groupBoxDatos.Controls.Add(this.txtObservaciones);
            this.groupBoxDatos.Controls.Add(this.lblObservaciones);
            this.groupBoxDatos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxDatos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.groupBoxDatos.Location = new System.Drawing.Point(20, 415);
            this.groupBoxDatos.Name = "groupBoxDatos";
            this.groupBoxDatos.Size = new System.Drawing.Size(760, 150);
            this.groupBoxDatos.TabIndex = 4;
            this.groupBoxDatos.TabStop = false;
            this.groupBoxDatos.Text = "Datos de Devolución";
            // 
            // lblUbicacion
            // 
            this.lblUbicacion.AutoSize = true;
            this.lblUbicacion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblUbicacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lblUbicacion.Location = new System.Drawing.Point(30, 70);
            this.lblUbicacion.Name = "lblUbicacion";
            this.lblUbicacion.Size = new System.Drawing.Size(0, 17);
            this.lblUbicacion.TabIndex = 3;
            // 
            // lblFechaDevolucionPrevista
            // 
            this.lblFechaDevolucionPrevista.AutoSize = true;
            this.lblFechaDevolucionPrevista.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic);
            this.lblFechaDevolucionPrevista.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblFechaDevolucionPrevista.Location = new System.Drawing.Point(400, 50);
            this.lblFechaDevolucionPrevista.Name = "lblFechaDevolucionPrevista";
            this.lblFechaDevolucionPrevista.Size = new System.Drawing.Size(0, 17);
            this.lblFechaDevolucionPrevista.TabIndex = 2;
            // 
            // lblFechaPrestamo
            // 
            this.lblFechaPrestamo.AutoSize = true;
            this.lblFechaPrestamo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic);
            this.lblFechaPrestamo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblFechaPrestamo.Location = new System.Drawing.Point(30, 50);
            this.lblFechaPrestamo.Name = "lblFechaPrestamo";
            this.lblFechaPrestamo.Size = new System.Drawing.Size(0, 17);
            this.lblFechaPrestamo.TabIndex = 1;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.lblEstado.Location = new System.Drawing.Point(30, 30);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(0, 17);
            this.lblEstado.TabIndex = 0;
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtObservaciones.Location = new System.Drawing.Point(150, 95);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(580, 40);
            this.txtObservaciones.TabIndex = 5;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblObservaciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblObservaciones.Location = new System.Drawing.Point(30, 98);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(97, 17);
            this.lblObservaciones.TabIndex = 4;
            this.lblObservaciones.Text = "Observaciones:";
            // 
            // btnRegistrar
            //
            this.btnRegistrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnRegistrar.FlatAppearance.BorderSize = 0;
            this.btnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.Location = new System.Drawing.Point(20, 580);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(180, 35);
            this.btnRegistrar.TabIndex = 5;
            this.btnRegistrar.Text = "Registrar Devolución";
            this.btnRegistrar.UseVisualStyleBackColor = false;
            //
            // btnLimpiar
            //
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(310, 580);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(180, 35);
            this.btnLimpiar.TabIndex = 6;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            //
            // btnVolver
            //
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(600, 580);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(180, 35);
            this.btnVolver.TabIndex = 7;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            // 
            // registrarDevolucion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(800, 635);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.groupBoxDatos);
            this.Controls.Add(this.dgvPrestamos);
            this.Controls.Add(this.lblResultados);
            this.Controls.Add(this.groupBoxBusqueda);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "registrarDevolucion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registrar Devolución";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos)).EndInit();
            this.groupBoxBusqueda.ResumeLayout(false);
            this.groupBoxBusqueda.PerformLayout();
            this.groupBoxDatos.ResumeLayout(false);
            this.groupBoxDatos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvPrestamos;
        private System.Windows.Forms.Label lblResultados;
        private System.Windows.Forms.GroupBox groupBoxBusqueda;
        private System.Windows.Forms.TextBox txtBuscarAlumno;
        private System.Windows.Forms.TextBox txtBuscarTitulo;
        private System.Windows.Forms.TextBox txtBuscarEjemplar;
        private System.Windows.Forms.Label lblBuscarAlumno;
        private System.Windows.Forms.Label lblBuscarTitulo;
        private System.Windows.Forms.Label lblBuscarEjemplar;
        private System.Windows.Forms.GroupBox groupBoxDatos;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblFechaPrestamo;
        private System.Windows.Forms.Label lblFechaDevolucionPrevista;
        private System.Windows.Forms.Label lblUbicacion;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnVolver;
    }
}
