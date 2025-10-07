namespace UI.WinUi.Administrador
{
    partial class menuAdministrador
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
            this.lblSistemaBiblio = new System.Windows.Forms.Label();
            this.lblUsuarioLogeado = new System.Windows.Forms.Label();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.comboBoxIdioma = new System.Windows.Forms.ComboBox();
            this.btnGestionUsuarios = new System.Windows.Forms.Button();
            this.btnReportes = new System.Windows.Forms.Button();
            this.btnCatalogo = new System.Windows.Forms.Button();
            this.btnGestionarAlumnos = new System.Windows.Forms.Button();
            this.btnPrestamos = new System.Windows.Forms.Button();
            this.btnDevoluciones = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSistemaBiblio
            // 
            this.lblSistemaBiblio.AutoSize = true;
            this.lblSistemaBiblio.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSistemaBiblio.Location = new System.Drawing.Point(267, 46);
            this.lblSistemaBiblio.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSistemaBiblio.Name = "lblSistemaBiblio";
            this.lblSistemaBiblio.Size = new System.Drawing.Size(326, 29);
            this.lblSistemaBiblio.TabIndex = 0;
            this.lblSistemaBiblio.Text = "Sistema Biblioteca Escolar";
            this.lblSistemaBiblio.Click += new System.EventHandler(this.lblVeterinariaVetCare_Click);
            // 
            // lblUsuarioLogeado
            // 
            this.lblUsuarioLogeado.AutoSize = true;
            this.lblUsuarioLogeado.Location = new System.Drawing.Point(14, 17);
            this.lblUsuarioLogeado.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsuarioLogeado.Name = "lblUsuarioLogeado";
            this.lblUsuarioLogeado.Size = new System.Drawing.Size(49, 13);
            this.lblUsuarioLogeado.TabIndex = 1;
            this.lblUsuarioLogeado.Text = "Usuario: ";
            // 
            // lblPerfil
            // 
            this.lblPerfil.AutoSize = true;
            this.lblPerfil.Location = new System.Drawing.Point(14, 46);
            this.lblPerfil.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.Size = new System.Drawing.Size(99, 13);
            this.lblPerfil.TabIndex = 2;
            this.lblPerfil.Text = "Perfil: Administrador";
            // 
            // lblIdioma
            // 
            this.lblIdioma.AutoSize = true;
            this.lblIdioma.Location = new System.Drawing.Point(14, 76);
            this.lblIdioma.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Size = new System.Drawing.Size(41, 13);
            this.lblIdioma.TabIndex = 4;
            this.lblIdioma.Text = "Idioma:";
            // 
            // comboBoxIdioma
            // 
            this.comboBoxIdioma.FormattingEnabled = true;
            this.comboBoxIdioma.Location = new System.Drawing.Point(68, 73);
            this.comboBoxIdioma.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxIdioma.Name = "comboBoxIdioma";
            this.comboBoxIdioma.Size = new System.Drawing.Size(120, 21);
            this.comboBoxIdioma.TabIndex = 5;
            // 
            // btnGestionUsuarios
            // 
            this.btnGestionUsuarios.Location = new System.Drawing.Point(17, 153);
            this.btnGestionUsuarios.Margin = new System.Windows.Forms.Padding(2);
            this.btnGestionUsuarios.Name = "btnGestionUsuarios";
            this.btnGestionUsuarios.Size = new System.Drawing.Size(85, 41);
            this.btnGestionUsuarios.TabIndex = 3;
            this.btnGestionUsuarios.Text = "Gestion Usuarios";
            this.btnGestionUsuarios.UseVisualStyleBackColor = true;
            // 
            // btnReportes
            // 
            this.btnReportes.Location = new System.Drawing.Point(540, 153);
            this.btnReportes.Margin = new System.Windows.Forms.Padding(2);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(77, 41);
            this.btnReportes.TabIndex = 6;
            this.btnReportes.Text = "Reportes";
            this.btnReportes.UseVisualStyleBackColor = true;
            // 
            // btnCatalogo
            // 
            this.btnCatalogo.Location = new System.Drawing.Point(122, 153);
            this.btnCatalogo.Name = "btnCatalogo";
            this.btnCatalogo.Size = new System.Drawing.Size(85, 41);
            this.btnCatalogo.TabIndex = 7;
            this.btnCatalogo.Text = "Catalogo";
            this.btnCatalogo.UseVisualStyleBackColor = true;
            // 
            // btnGestionarAlumnos
            // 
            this.btnGestionarAlumnos.Location = new System.Drawing.Point(228, 153);
            this.btnGestionarAlumnos.Name = "btnGestionarAlumnos";
            this.btnGestionarAlumnos.Size = new System.Drawing.Size(85, 41);
            this.btnGestionarAlumnos.TabIndex = 8;
            this.btnGestionarAlumnos.Text = "Gestionar Alumnos";
            this.btnGestionarAlumnos.UseVisualStyleBackColor = true;
            // 
            // btnPrestamos
            // 
            this.btnPrestamos.Location = new System.Drawing.Point(333, 153);
            this.btnPrestamos.Name = "btnPrestamos";
            this.btnPrestamos.Size = new System.Drawing.Size(85, 41);
            this.btnPrestamos.TabIndex = 9;
            this.btnPrestamos.Text = "Prestamos";
            this.btnPrestamos.UseVisualStyleBackColor = true;
            // 
            // btnDevoluciones
            // 
            this.btnDevoluciones.Location = new System.Drawing.Point(439, 153);
            this.btnDevoluciones.Name = "btnDevoluciones";
            this.btnDevoluciones.Size = new System.Drawing.Size(85, 41);
            this.btnDevoluciones.TabIndex = 10;
            this.btnDevoluciones.Text = "Devoluciones";
            this.btnDevoluciones.UseVisualStyleBackColor = true;
            // 
            // menuAdministrador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 235);
            this.Controls.Add(this.btnDevoluciones);
            this.Controls.Add(this.btnPrestamos);
            this.Controls.Add(this.btnGestionarAlumnos);
            this.Controls.Add(this.btnCatalogo);
            this.Controls.Add(this.btnReportes);
            this.Controls.Add(this.comboBoxIdioma);
            this.Controls.Add(this.lblIdioma);
            this.Controls.Add(this.btnGestionUsuarios);
            this.Controls.Add(this.lblPerfil);
            this.Controls.Add(this.lblUsuarioLogeado);
            this.Controls.Add(this.lblSistemaBiblio);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "menuAdministrador";
            this.Text = "menuAdministrador";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSistemaBiblio;
        private System.Windows.Forms.Label lblUsuarioLogeado;
        private System.Windows.Forms.Label lblPerfil;
        private System.Windows.Forms.Label lblIdioma;
        private System.Windows.Forms.ComboBox comboBoxIdioma;
        private System.Windows.Forms.Button btnGestionUsuarios;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.Button btnCatalogo;
        private System.Windows.Forms.Button btnGestionarAlumnos;
        private System.Windows.Forms.Button btnPrestamos;
        private System.Windows.Forms.Button btnDevoluciones;
    }
}