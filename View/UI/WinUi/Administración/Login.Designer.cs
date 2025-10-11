namespace UI
{
    partial class Login
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblContraseña = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtContraseña = new System.Windows.Forms.TextBox();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.lblMenu = new System.Windows.Forms.Label();
            this.btnRecuperarContraseña = new System.Windows.Forms.Button();
            this.lnkEspañol = new System.Windows.Forms.LinkLabel();
            this.lnkEnglish = new System.Windows.Forms.LinkLabel();
            this.lblSeparadorIdioma = new System.Windows.Forms.Label();
            this.btnMostrarContraseña = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblUsuario.Location = new System.Drawing.Point(85, 140);
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(56, 17);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario:";
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.BackColor = System.Drawing.Color.Transparent;
            this.lblContraseña.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContraseña.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblContraseña.Location = new System.Drawing.Point(85, 210);
            this.lblContraseña.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(77, 17);
            this.lblContraseña.TabIndex = 1;
            this.lblContraseña.Text = "Contraseña:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(88, 165);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(280, 26);
            this.txtUsuario.TabIndex = 0;
            // 
            // txtContraseña
            // 
            this.txtContraseña.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContraseña.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContraseña.Location = new System.Drawing.Point(88, 235);
            this.txtContraseña.Margin = new System.Windows.Forms.Padding(2);
            this.txtContraseña.Name = "txtContraseña";
            this.txtContraseña.PasswordChar = '●';
            this.txtContraseña.Size = new System.Drawing.Size(245, 26);
            this.txtContraseña.TabIndex = 1;
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnIngresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIngresar.FlatAppearance.BorderSize = 0;
            this.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresar.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ForeColor = System.Drawing.Color.White;
            this.btnIngresar.Location = new System.Drawing.Point(88, 285);
            this.btnIngresar.Margin = new System.Windows.Forms.Padding(2);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(280, 45);
            this.btnIngresar.TabIndex = 2;
            this.btnIngresar.Text = "INICIAR SESIÓN";
            this.btnIngresar.UseVisualStyleBackColor = false;
            // 
            // lblMenu
            // 
            this.lblMenu.AutoSize = true;
            this.lblMenu.BackColor = System.Drawing.Color.Transparent;
            this.lblMenu.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblMenu.Location = new System.Drawing.Point(45, 70);
            this.lblMenu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMenu.Name = "lblMenu";
            this.lblMenu.Size = new System.Drawing.Size(351, 37);
            this.lblMenu.TabIndex = 5;
            this.lblMenu.Text = "Sistema Biblioteca Escolar";
            // 
            // btnRecuperarContraseña
            // 
            this.btnRecuperarContraseña.BackColor = System.Drawing.Color.Transparent;
            this.btnRecuperarContraseña.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecuperarContraseña.FlatAppearance.BorderSize = 0;
            this.btnRecuperarContraseña.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRecuperarContraseña.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRecuperarContraseña.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecuperarContraseña.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecuperarContraseña.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRecuperarContraseña.Location = new System.Drawing.Point(88, 345);
            this.btnRecuperarContraseña.Margin = new System.Windows.Forms.Padding(2);
            this.btnRecuperarContraseña.Name = "btnRecuperarContraseña";
            this.btnRecuperarContraseña.Size = new System.Drawing.Size(280, 28);
            this.btnRecuperarContraseña.TabIndex = 3;
            this.btnRecuperarContraseña.Text = "¿Olvidó su contraseña?";
            this.btnRecuperarContraseña.UseVisualStyleBackColor = false;
            // 
            // lnkEspañol
            // 
            this.lnkEspañol.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lnkEspañol.AutoSize = true;
            this.lnkEspañol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkEspañol.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkEspañol.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lnkEspañol.Location = new System.Drawing.Point(175, 485);
            this.lnkEspañol.Name = "lnkEspañol";
            this.lnkEspañol.Size = new System.Drawing.Size(48, 15);
            this.lnkEspañol.TabIndex = 4;
            this.lnkEspañol.TabStop = true;
            this.lnkEspañol.Text = "Español";
            this.lnkEspañol.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            // 
            // lnkEnglish
            // 
            this.lnkEnglish.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lnkEnglish.AutoSize = true;
            this.lnkEnglish.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkEnglish.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkEnglish.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lnkEnglish.Location = new System.Drawing.Point(237, 485);
            this.lnkEnglish.Name = "lnkEnglish";
            this.lnkEnglish.Size = new System.Drawing.Size(45, 15);
            this.lnkEnglish.TabIndex = 5;
            this.lnkEnglish.TabStop = true;
            this.lnkEnglish.Text = "English";
            this.lnkEnglish.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            // 
            // lblSeparadorIdioma
            // 
            this.lblSeparadorIdioma.AutoSize = true;
            this.lblSeparadorIdioma.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeparadorIdioma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.lblSeparadorIdioma.Location = new System.Drawing.Point(225, 485);
            this.lblSeparadorIdioma.Name = "lblSeparadorIdioma";
            this.lblSeparadorIdioma.Size = new System.Drawing.Size(10, 15);
            this.lblSeparadorIdioma.TabIndex = 6;
            this.lblSeparadorIdioma.Text = "|";
            // 
            // btnMostrarContraseña
            // 
            this.btnMostrarContraseña.BackColor = System.Drawing.Color.White;
            this.btnMostrarContraseña.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMostrarContraseña.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.btnMostrarContraseña.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMostrarContraseña.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMostrarContraseña.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.btnMostrarContraseña.Location = new System.Drawing.Point(333, 235);
            this.btnMostrarContraseña.Name = "btnMostrarContraseña";
            this.btnMostrarContraseña.Size = new System.Drawing.Size(35, 26);
            this.btnMostrarContraseña.TabIndex = 7;
            this.btnMostrarContraseña.Text = "👁";
            this.btnMostrarContraseña.UseVisualStyleBackColor = false;
            this.btnMostrarContraseña.Click += new System.EventHandler(this.btnMostrarContraseña_Click_1);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(450, 520);
            this.Controls.Add(this.btnMostrarContraseña);
            this.Controls.Add(this.lblSeparadorIdioma);
            this.Controls.Add(this.lnkEnglish);
            this.Controls.Add(this.lnkEspañol);
            this.Controls.Add(this.btnRecuperarContraseña);
            this.Controls.Add(this.lblMenu);
            this.Controls.Add(this.btnIngresar);
            this.Controls.Add(this.txtContraseña);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblContraseña);
            this.Controls.Add(this.lblUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblContraseña;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtContraseña;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Label lblMenu;
        private System.Windows.Forms.Button btnRecuperarContraseña;
        private System.Windows.Forms.LinkLabel lnkEspañol;
        private System.Windows.Forms.LinkLabel lnkEnglish;
        private System.Windows.Forms.Label lblSeparadorIdioma;
        private System.Windows.Forms.Button btnMostrarContraseña;
    }
}

