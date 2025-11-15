namespace UI.WinUi.Administrador
{
    partial class FrmMiPerfil
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
            this.grpDatosUsuario = new System.Windows.Forms.GroupBox();
            this.txtUltimoAcceso = new System.Windows.Forms.TextBox();
            this.lblUltimoAcceso = new System.Windows.Forms.Label();
            this.txtRol = new System.Windows.Forms.TextBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtNombreUsuario = new System.Windows.Forms.TextBox();
            this.lblNombreUsuario = new System.Windows.Forms.Label();
            this.grpPreferencias = new System.Windows.Forms.GroupBox();
            this.cboIdioma = new System.Windows.Forms.ComboBox();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.grpCambiarPassword = new System.Windows.Forms.GroupBox();
            this.txtPasswordConfirmar = new System.Windows.Forms.TextBox();
            this.lblPasswordConfirmar = new System.Windows.Forms.Label();
            this.txtPasswordNueva = new System.Windows.Forms.TextBox();
            this.lblPasswordNueva = new System.Windows.Forms.Label();
            this.txtPasswordActual = new System.Windows.Forms.TextBox();
            this.lblPasswordActual = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.grpDatosUsuario.SuspendLayout();
            this.grpPreferencias.SuspendLayout();
            this.grpCambiarPassword.SuspendLayout();
            this.SuspendLayout();
            //
            // grpDatosUsuario
            //
            this.grpDatosUsuario.Controls.Add(this.txtUltimoAcceso);
            this.grpDatosUsuario.Controls.Add(this.lblUltimoAcceso);
            this.grpDatosUsuario.Controls.Add(this.txtRol);
            this.grpDatosUsuario.Controls.Add(this.lblRol);
            this.grpDatosUsuario.Controls.Add(this.txtEmail);
            this.grpDatosUsuario.Controls.Add(this.lblEmail);
            this.grpDatosUsuario.Controls.Add(this.txtNombreUsuario);
            this.grpDatosUsuario.Controls.Add(this.lblNombreUsuario);
            this.grpDatosUsuario.Location = new System.Drawing.Point(12, 12);
            this.grpDatosUsuario.Name = "grpDatosUsuario";
            this.grpDatosUsuario.Size = new System.Drawing.Size(460, 150);
            this.grpDatosUsuario.TabIndex = 0;
            this.grpDatosUsuario.TabStop = false;
            this.grpDatosUsuario.Text = "Datos del Usuario";
            //
            // txtUltimoAcceso
            //
            this.txtUltimoAcceso.Location = new System.Drawing.Point(150, 111);
            this.txtUltimoAcceso.Name = "txtUltimoAcceso";
            this.txtUltimoAcceso.ReadOnly = true;
            this.txtUltimoAcceso.Size = new System.Drawing.Size(290, 20);
            this.txtUltimoAcceso.TabIndex = 7;
            //
            // lblUltimoAcceso
            //
            this.lblUltimoAcceso.AutoSize = true;
            this.lblUltimoAcceso.Location = new System.Drawing.Point(20, 114);
            this.lblUltimoAcceso.Name = "lblUltimoAcceso";
            this.lblUltimoAcceso.Size = new System.Drawing.Size(79, 13);
            this.lblUltimoAcceso.TabIndex = 6;
            this.lblUltimoAcceso.Text = "Último Acceso:";
            //
            // txtRol
            //
            this.txtRol.Location = new System.Drawing.Point(150, 81);
            this.txtRol.Name = "txtRol";
            this.txtRol.ReadOnly = true;
            this.txtRol.Size = new System.Drawing.Size(290, 20);
            this.txtRol.TabIndex = 5;
            //
            // lblRol
            //
            this.lblRol.AutoSize = true;
            this.lblRol.Location = new System.Drawing.Point(20, 84);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(26, 13);
            this.lblRol.TabIndex = 4;
            this.lblRol.Text = "Rol:";
            //
            // txtEmail
            //
            this.txtEmail.Location = new System.Drawing.Point(150, 51);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(290, 20);
            this.txtEmail.TabIndex = 3;
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(20, 54);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email:";
            //
            // txtNombreUsuario
            //
            this.txtNombreUsuario.Location = new System.Drawing.Point(150, 21);
            this.txtNombreUsuario.Name = "txtNombreUsuario";
            this.txtNombreUsuario.ReadOnly = true;
            this.txtNombreUsuario.Size = new System.Drawing.Size(290, 20);
            this.txtNombreUsuario.TabIndex = 1;
            //
            // lblNombreUsuario
            //
            this.lblNombreUsuario.AutoSize = true;
            this.lblNombreUsuario.Location = new System.Drawing.Point(20, 24);
            this.lblNombreUsuario.Name = "lblNombreUsuario";
            this.lblNombreUsuario.Size = new System.Drawing.Size(99, 13);
            this.lblNombreUsuario.TabIndex = 0;
            this.lblNombreUsuario.Text = "Nombre de Usuario:";
            //
            // grpPreferencias
            //
            this.grpPreferencias.Controls.Add(this.cboIdioma);
            this.grpPreferencias.Controls.Add(this.lblIdioma);
            this.grpPreferencias.Location = new System.Drawing.Point(12, 168);
            this.grpPreferencias.Name = "grpPreferencias";
            this.grpPreferencias.Size = new System.Drawing.Size(460, 70);
            this.grpPreferencias.TabIndex = 1;
            this.grpPreferencias.TabStop = false;
            this.grpPreferencias.Text = "Preferencias";
            //
            // cboIdioma
            //
            this.cboIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIdioma.FormattingEnabled = true;
            this.cboIdioma.Location = new System.Drawing.Point(150, 28);
            this.cboIdioma.Name = "cboIdioma";
            this.cboIdioma.Size = new System.Drawing.Size(290, 21);
            this.cboIdioma.TabIndex = 1;
            //
            // lblIdioma
            //
            this.lblIdioma.AutoSize = true;
            this.lblIdioma.Location = new System.Drawing.Point(20, 31);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Size = new System.Drawing.Size(41, 13);
            this.lblIdioma.TabIndex = 0;
            this.lblIdioma.Text = "Idioma:";
            //
            // grpCambiarPassword
            //
            this.grpCambiarPassword.Controls.Add(this.txtPasswordConfirmar);
            this.grpCambiarPassword.Controls.Add(this.lblPasswordConfirmar);
            this.grpCambiarPassword.Controls.Add(this.txtPasswordNueva);
            this.grpCambiarPassword.Controls.Add(this.lblPasswordNueva);
            this.grpCambiarPassword.Controls.Add(this.txtPasswordActual);
            this.grpCambiarPassword.Controls.Add(this.lblPasswordActual);
            this.grpCambiarPassword.Location = new System.Drawing.Point(12, 244);
            this.grpCambiarPassword.Name = "grpCambiarPassword";
            this.grpCambiarPassword.Size = new System.Drawing.Size(460, 130);
            this.grpCambiarPassword.TabIndex = 2;
            this.grpCambiarPassword.TabStop = false;
            this.grpCambiarPassword.Text = "Cambiar Contraseña (Opcional)";
            //
            // txtPasswordConfirmar
            //
            this.txtPasswordConfirmar.Location = new System.Drawing.Point(150, 91);
            this.txtPasswordConfirmar.Name = "txtPasswordConfirmar";
            this.txtPasswordConfirmar.PasswordChar = '●';
            this.txtPasswordConfirmar.Size = new System.Drawing.Size(290, 20);
            this.txtPasswordConfirmar.TabIndex = 5;
            //
            // lblPasswordConfirmar
            //
            this.lblPasswordConfirmar.AutoSize = true;
            this.lblPasswordConfirmar.Location = new System.Drawing.Point(20, 94);
            this.lblPasswordConfirmar.Name = "lblPasswordConfirmar";
            this.lblPasswordConfirmar.Size = new System.Drawing.Size(114, 13);
            this.lblPasswordConfirmar.TabIndex = 4;
            this.lblPasswordConfirmar.Text = "Confirmar Contraseña:";
            //
            // txtPasswordNueva
            //
            this.txtPasswordNueva.Location = new System.Drawing.Point(150, 61);
            this.txtPasswordNueva.Name = "txtPasswordNueva";
            this.txtPasswordNueva.PasswordChar = '●';
            this.txtPasswordNueva.Size = new System.Drawing.Size(290, 20);
            this.txtPasswordNueva.TabIndex = 3;
            //
            // lblPasswordNueva
            //
            this.lblPasswordNueva.AutoSize = true;
            this.lblPasswordNueva.Location = new System.Drawing.Point(20, 64);
            this.lblPasswordNueva.Name = "lblPasswordNueva";
            this.lblPasswordNueva.Size = new System.Drawing.Size(101, 13);
            this.lblPasswordNueva.TabIndex = 2;
            this.lblPasswordNueva.Text = "Nueva Contraseña:";
            //
            // txtPasswordActual
            //
            this.txtPasswordActual.Location = new System.Drawing.Point(150, 31);
            this.txtPasswordActual.Name = "txtPasswordActual";
            this.txtPasswordActual.PasswordChar = '●';
            this.txtPasswordActual.Size = new System.Drawing.Size(290, 20);
            this.txtPasswordActual.TabIndex = 1;
            //
            // lblPasswordActual
            //
            this.lblPasswordActual.AutoSize = true;
            this.lblPasswordActual.Location = new System.Drawing.Point(20, 34);
            this.lblPasswordActual.Name = "lblPasswordActual";
            this.lblPasswordActual.Size = new System.Drawing.Size(96, 13);
            this.lblPasswordActual.TabIndex = 0;
            this.lblPasswordActual.Text = "Contraseña Actual:";
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(290, 390);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(90, 30);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(386, 390);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 30);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // FrmMiPerfil
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 432);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.grpCambiarPassword);
            this.Controls.Add(this.grpPreferencias);
            this.Controls.Add(this.grpDatosUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMiPerfil";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mi Perfil";
            this.grpDatosUsuario.ResumeLayout(false);
            this.grpDatosUsuario.PerformLayout();
            this.grpPreferencias.ResumeLayout(false);
            this.grpPreferencias.PerformLayout();
            this.grpCambiarPassword.ResumeLayout(false);
            this.grpCambiarPassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDatosUsuario;
        private System.Windows.Forms.TextBox txtNombreUsuario;
        private System.Windows.Forms.Label lblNombreUsuario;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtRol;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.TextBox txtUltimoAcceso;
        private System.Windows.Forms.Label lblUltimoAcceso;
        private System.Windows.Forms.GroupBox grpPreferencias;
        private System.Windows.Forms.ComboBox cboIdioma;
        private System.Windows.Forms.Label lblIdioma;
        private System.Windows.Forms.GroupBox grpCambiarPassword;
        private System.Windows.Forms.TextBox txtPasswordActual;
        private System.Windows.Forms.Label lblPasswordActual;
        private System.Windows.Forms.TextBox txtPasswordNueva;
        private System.Windows.Forms.Label lblPasswordNueva;
        private System.Windows.Forms.TextBox txtPasswordConfirmar;
        private System.Windows.Forms.Label lblPasswordConfirmar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
