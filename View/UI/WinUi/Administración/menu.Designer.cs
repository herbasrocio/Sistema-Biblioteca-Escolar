namespace UI.WinUi.Administrador
{
    partial class menu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.usuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.permisosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catalogoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultarMaterialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarMaterialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alumnosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prestamosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devolucionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBienvenida = new System.Windows.Forms.Panel();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.lblNombreUsuario = new System.Windows.Forms.Label();
            this.lblRolUsuario = new System.Windows.Forms.Label();
            this.lblTituloPrincipal = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panelBienvenida.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usuariosToolStripMenuItem,
            this.permisosToolStripMenuItem,
            this.catalogoToolStripMenuItem,
            this.alumnosToolStripMenuItem,
            this.prestamosToolStripMenuItem,
            this.devolucionesToolStripMenuItem,
            this.reportesToolStripMenuItem,
            this.cerrarSesionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(863, 31);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // usuariosToolStripMenuItem
            // 
            this.usuariosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            this.usuariosToolStripMenuItem.Size = new System.Drawing.Size(74, 23);
            this.usuariosToolStripMenuItem.Text = "Usuarios";
            this.usuariosToolStripMenuItem.Click += new System.EventHandler(this.usuariosToolStripMenuItem_Click);
            // 
            // permisosToolStripMenuItem
            // 
            this.permisosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.permisosToolStripMenuItem.Name = "permisosToolStripMenuItem";
            this.permisosToolStripMenuItem.Size = new System.Drawing.Size(75, 23);
            this.permisosToolStripMenuItem.Text = "Permisos";
            this.permisosToolStripMenuItem.Click += new System.EventHandler(this.permisosToolStripMenuItem_Click);
            //
            // catalogoToolStripMenuItem
            //
            this.catalogoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consultarMaterialToolStripMenuItem,
            this.registrarMaterialToolStripMenuItem});
            this.catalogoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catalogoToolStripMenuItem.Name = "catalogoToolStripMenuItem";
            this.catalogoToolStripMenuItem.Size = new System.Drawing.Size(76, 23);
            this.catalogoToolStripMenuItem.Text = "Catálogo";
            //
            // consultarMaterialToolStripMenuItem
            //
            this.consultarMaterialToolStripMenuItem.Name = "consultarMaterialToolStripMenuItem";
            this.consultarMaterialToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.consultarMaterialToolStripMenuItem.Text = "Consultar Material";
            this.consultarMaterialToolStripMenuItem.Click += new System.EventHandler(this.consultarMaterialToolStripMenuItem_Click);
            //
            // registrarMaterialToolStripMenuItem
            //
            this.registrarMaterialToolStripMenuItem.Name = "registrarMaterialToolStripMenuItem";
            this.registrarMaterialToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.registrarMaterialToolStripMenuItem.Text = "Registrar Material";
            this.registrarMaterialToolStripMenuItem.Click += new System.EventHandler(this.registrarMaterialToolStripMenuItem_Click);
            // 
            // alumnosToolStripMenuItem
            // 
            this.alumnosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.alumnosToolStripMenuItem.Name = "alumnosToolStripMenuItem";
            this.alumnosToolStripMenuItem.Size = new System.Drawing.Size(75, 23);
            this.alumnosToolStripMenuItem.Text = "Alumnos";
            this.alumnosToolStripMenuItem.Click += new System.EventHandler(this.alumnosToolStripMenuItem_Click);
            // 
            // prestamosToolStripMenuItem
            // 
            this.prestamosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.prestamosToolStripMenuItem.Name = "prestamosToolStripMenuItem";
            this.prestamosToolStripMenuItem.Size = new System.Drawing.Size(85, 23);
            this.prestamosToolStripMenuItem.Text = "Préstamos";
            this.prestamosToolStripMenuItem.Click += new System.EventHandler(this.prestamosToolStripMenuItem_Click);
            // 
            // devolucionesToolStripMenuItem
            // 
            this.devolucionesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.devolucionesToolStripMenuItem.Name = "devolucionesToolStripMenuItem";
            this.devolucionesToolStripMenuItem.Size = new System.Drawing.Size(102, 23);
            this.devolucionesToolStripMenuItem.Text = "Devoluciones";
            this.devolucionesToolStripMenuItem.Click += new System.EventHandler(this.devolucionesToolStripMenuItem_Click);
            // 
            // reportesToolStripMenuItem
            // 
            this.reportesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            this.reportesToolStripMenuItem.Size = new System.Drawing.Size(75, 23);
            this.reportesToolStripMenuItem.Text = "Reportes";
            this.reportesToolStripMenuItem.Click += new System.EventHandler(this.reportesToolStripMenuItem_Click);
            // 
            // cerrarSesionToolStripMenuItem
            //
            this.cerrarSesionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
            this.cerrarSesionToolStripMenuItem.Size = new System.Drawing.Size(102, 23);
            this.cerrarSesionToolStripMenuItem.Text = "Cerrar Sesión";
            this.cerrarSesionToolStripMenuItem.Click += new System.EventHandler(this.cerrarSesionToolStripMenuItem_Click);
            // 
            // panelBienvenida
            // 
            this.panelBienvenida.BackColor = System.Drawing.Color.White;
            this.panelBienvenida.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBienvenida.Controls.Add(this.lblBienvenida);
            this.panelBienvenida.Controls.Add(this.lblNombreUsuario);
            this.panelBienvenida.Controls.Add(this.lblRolUsuario);
            this.panelBienvenida.Location = new System.Drawing.Point(206, 140);
            this.panelBienvenida.Name = "panelBienvenida";
            this.panelBienvenida.Size = new System.Drawing.Size(450, 120);
            this.panelBienvenida.TabIndex = 2;
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lblBienvenida.Location = new System.Drawing.Point(0, 20);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(450, 25);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "¡Bienvenido!";
            this.lblBienvenida.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNombreUsuario
            // 
            this.lblNombreUsuario.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNombreUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblNombreUsuario.Location = new System.Drawing.Point(0, 55);
            this.lblNombreUsuario.Name = "lblNombreUsuario";
            this.lblNombreUsuario.Size = new System.Drawing.Size(450, 20);
            this.lblNombreUsuario.TabIndex = 1;
            this.lblNombreUsuario.Text = "Usuario: Admin";
            this.lblNombreUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRolUsuario
            // 
            this.lblRolUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRolUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblRolUsuario.Location = new System.Drawing.Point(0, 80);
            this.lblRolUsuario.Name = "lblRolUsuario";
            this.lblRolUsuario.Size = new System.Drawing.Size(450, 19);
            this.lblRolUsuario.TabIndex = 2;
            this.lblRolUsuario.Text = "Rol: Administrador";
            this.lblRolUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTituloPrincipal
            // 
            this.lblTituloPrincipal.AutoSize = true;
            this.lblTituloPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.lblTituloPrincipal.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTituloPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTituloPrincipal.Location = new System.Drawing.Point(206, 60);
            this.lblTituloPrincipal.Name = "lblTituloPrincipal";
            this.lblTituloPrincipal.Size = new System.Drawing.Size(408, 45);
            this.lblTituloPrincipal.TabIndex = 1;
            this.lblTituloPrincipal.Text = "Sistema Biblioteca Escolar";
            // 
            // menu
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(863, 500);
            this.Controls.Add(this.panelBienvenida);
            this.Controls.Add(this.lblTituloPrincipal);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "menu";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelBienvenida.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem usuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem permisosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem catalogoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultarMaterialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registrarMaterialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alumnosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prestamosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem devolucionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarSesionToolStripMenuItem;
        private System.Windows.Forms.Panel panelBienvenida;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Label lblNombreUsuario;
        private System.Windows.Forms.Label lblRolUsuario;
        private System.Windows.Forms.Label lblTituloPrincipal;
    }
}