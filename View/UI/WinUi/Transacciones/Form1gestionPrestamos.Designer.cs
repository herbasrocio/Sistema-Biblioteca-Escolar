namespace UI.WinUi.Transacciones
{
    partial class Form1gestionPrestamos
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabRegistrarPrestamo = new System.Windows.Forms.TabPage();
            this.tabRenovarPrestamo = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabRegistrarPrestamo);
            this.tabControl.Controls.Add(this.tabRenovarPrestamo);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1020, 500);
            this.tabControl.TabIndex = 0;
            //
            // tabRegistrarPrestamo
            //
            this.tabRegistrarPrestamo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.tabRegistrarPrestamo.Location = new System.Drawing.Point(4, 26);
            this.tabRegistrarPrestamo.Name = "tabRegistrarPrestamo";
            this.tabRegistrarPrestamo.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegistrarPrestamo.Size = new System.Drawing.Size(1012, 470);
            this.tabRegistrarPrestamo.TabIndex = 0;
            this.tabRegistrarPrestamo.Text = "Registrar Préstamo";
            //
            // tabRenovarPrestamo
            //
            this.tabRenovarPrestamo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.tabRenovarPrestamo.Location = new System.Drawing.Point(4, 26);
            this.tabRenovarPrestamo.Name = "tabRenovarPrestamo";
            this.tabRenovarPrestamo.Padding = new System.Windows.Forms.Padding(3);
            this.tabRenovarPrestamo.Size = new System.Drawing.Size(1012, 470);
            this.tabRenovarPrestamo.TabIndex = 1;
            this.tabRenovarPrestamo.Text = "Renovar Préstamo";
            //
            // Form1gestionPrestamos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1020, 500);
            this.Controls.Add(this.tabControl);
            this.MaximumSize = new System.Drawing.Size(1036, 539);
            this.MinimumSize = new System.Drawing.Size(1036, 539);
            this.Name = "Form1gestionPrestamos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Préstamos";
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabRegistrarPrestamo;
        private System.Windows.Forms.TabPage tabRenovarPrestamo;
    }
}