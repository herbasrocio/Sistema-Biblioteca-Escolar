namespace UI.WinUi.Docente
{
    partial class menuDocente
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.comboBoxIdioma = new System.Windows.Forms.ComboBox();
            this.btnCatalogo = new System.Windows.Forms.Button();
            this.btnPrestamos = new System.Windows.Forms.Button();
            this.btnDevoluciones = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(193, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sistema Biblioteca Escolar";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(10, 14);
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(49, 13);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Text = "Usuario: ";
            // 
            // lblPerfil
            // 
            this.lblPerfil.AutoSize = true;
            this.lblPerfil.Location = new System.Drawing.Point(10, 38);
            this.lblPerfil.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.Size = new System.Drawing.Size(77, 13);
            this.lblPerfil.TabIndex = 2;
            this.lblPerfil.Text = "Perfil: Docente";
            // 
            // lblIdioma
            // 
            this.lblIdioma.AutoSize = true;
            this.lblIdioma.Location = new System.Drawing.Point(10, 64);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Size = new System.Drawing.Size(41, 13);
            this.lblIdioma.TabIndex = 3;
            this.lblIdioma.Text = "Idioma:";
            // 
            // comboBoxIdioma
            // 
            this.comboBoxIdioma.FormattingEnabled = true;
            this.comboBoxIdioma.Location = new System.Drawing.Point(63, 60);
            this.comboBoxIdioma.Name = "comboBoxIdioma";
            this.comboBoxIdioma.Size = new System.Drawing.Size(101, 21);
            this.comboBoxIdioma.TabIndex = 4;
            // 
            // btnCatalogo
            // 
            this.btnCatalogo.Location = new System.Drawing.Point(86, 123);
            this.btnCatalogo.Name = "btnCatalogo";
            this.btnCatalogo.Size = new System.Drawing.Size(101, 35);
            this.btnCatalogo.TabIndex = 5;
            this.btnCatalogo.Text = "Catalogo";
            this.btnCatalogo.UseVisualStyleBackColor = true;
            // 
            // btnPrestamos
            // 
            this.btnPrestamos.Location = new System.Drawing.Point(224, 123);
            this.btnPrestamos.Name = "btnPrestamos";
            this.btnPrestamos.Size = new System.Drawing.Size(101, 35);
            this.btnPrestamos.TabIndex = 6;
            this.btnPrestamos.Text = "Prestamos";
            this.btnPrestamos.UseVisualStyleBackColor = true;
            // 
            // btnDevoluciones
            // 
            this.btnDevoluciones.Location = new System.Drawing.Point(367, 123);
            this.btnDevoluciones.Name = "btnDevoluciones";
            this.btnDevoluciones.Size = new System.Drawing.Size(101, 35);
            this.btnDevoluciones.TabIndex = 7;
            this.btnDevoluciones.Text = "Devoluciones";
            this.btnDevoluciones.UseVisualStyleBackColor = true;
            // 
            // menuDocente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 192);
            this.Controls.Add(this.btnDevoluciones);
            this.Controls.Add(this.btnPrestamos);
            this.Controls.Add(this.btnCatalogo);
            this.Controls.Add(this.comboBoxIdioma);
            this.Controls.Add(this.lblIdioma);
            this.Controls.Add(this.lblPerfil);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "menuDocente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menú Docente - Sistema Biblioteca Escolar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblPerfil;
        private System.Windows.Forms.Label lblIdioma;
        private System.Windows.Forms.ComboBox comboBoxIdioma;
        private System.Windows.Forms.Button btnCatalogo;
        private System.Windows.Forms.Button btnPrestamos;
        private System.Windows.Forms.Button btnDevoluciones;
    }
}