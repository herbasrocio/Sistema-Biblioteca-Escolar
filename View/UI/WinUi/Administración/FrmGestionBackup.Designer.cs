namespace UI.WinUi.Administrador
{
    partial class FrmGestionBackup
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
            this.grpCrearBackup = new System.Windows.Forms.GroupBox();
            this.lblEspacioDisponible = new System.Windows.Forms.Label();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.txtRutaDestino = new System.Windows.Forms.TextBox();
            this.lblRutaDestino = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.cboTipoBackup = new System.Windows.Forms.ComboBox();
            this.lblTipoBackup = new System.Windows.Forms.Label();
            this.cboBaseDatos = new System.Windows.Forms.ComboBox();
            this.lblBaseDatos = new System.Windows.Forms.Label();
            this.btnCrearBackup = new System.Windows.Forms.Button();
            this.grpRestaurar = new System.Windows.Forms.GroupBox();
            this.dgvBackups = new System.Windows.Forms.DataGridView();
            this.lblTotalBackups = new System.Windows.Forms.Label();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnRestaurarExterno = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnVolver = new System.Windows.Forms.Button();
            this.grpCrearBackup.SuspendLayout();
            this.grpRestaurar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).BeginInit();
            this.SuspendLayout();
            //
            // grpCrearBackup
            //
            this.grpCrearBackup.Controls.Add(this.lblEspacioDisponible);
            this.grpCrearBackup.Controls.Add(this.btnExaminar);
            this.grpCrearBackup.Controls.Add(this.txtRutaDestino);
            this.grpCrearBackup.Controls.Add(this.lblRutaDestino);
            this.grpCrearBackup.Controls.Add(this.txtDescripcion);
            this.grpCrearBackup.Controls.Add(this.lblDescripcion);
            this.grpCrearBackup.Controls.Add(this.cboTipoBackup);
            this.grpCrearBackup.Controls.Add(this.lblTipoBackup);
            this.grpCrearBackup.Controls.Add(this.cboBaseDatos);
            this.grpCrearBackup.Controls.Add(this.lblBaseDatos);
            this.grpCrearBackup.Controls.Add(this.btnCrearBackup);
            this.grpCrearBackup.Location = new System.Drawing.Point(12, 12);
            this.grpCrearBackup.Name = "grpCrearBackup";
            this.grpCrearBackup.Size = new System.Drawing.Size(960, 180);
            this.grpCrearBackup.TabIndex = 0;
            this.grpCrearBackup.TabStop = false;
            this.grpCrearBackup.Text = "Crear Backup";
            //
            // lblEspacioDisponible
            //
            this.lblEspacioDisponible.AutoSize = true;
            this.lblEspacioDisponible.Location = new System.Drawing.Point(16, 150);
            this.lblEspacioDisponible.Name = "lblEspacioDisponible";
            this.lblEspacioDisponible.Size = new System.Drawing.Size(150, 13);
            this.lblEspacioDisponible.TabIndex = 10;
            this.lblEspacioDisponible.Text = "Espacio disponible: 0 GB";
            //
            // btnExaminar
            //
            this.btnExaminar.Location = new System.Drawing.Point(610, 115);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(80, 23);
            this.btnExaminar.TabIndex = 9;
            this.btnExaminar.Text = "Examinar...";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            //
            // txtRutaDestino
            //
            this.txtRutaDestino.Location = new System.Drawing.Point(120, 117);
            this.txtRutaDestino.Name = "txtRutaDestino";
            this.txtRutaDestino.ReadOnly = true;
            this.txtRutaDestino.Size = new System.Drawing.Size(480, 20);
            this.txtRutaDestino.TabIndex = 8;
            //
            // lblRutaDestino
            //
            this.lblRutaDestino.AutoSize = true;
            this.lblRutaDestino.Location = new System.Drawing.Point(16, 120);
            this.lblRutaDestino.Name = "lblRutaDestino";
            this.lblRutaDestino.Size = new System.Drawing.Size(80, 13);
            this.lblRutaDestino.TabIndex = 7;
            this.lblRutaDestino.Text = "Ruta Destino:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(120, 85);
            this.txtDescripcion.MaxLength = 500;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(480, 20);
            this.txtDescripcion.TabIndex = 6;
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(16, 88);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(75, 13);
            this.lblDescripcion.TabIndex = 5;
            this.lblDescripcion.Text = "Descripción:";
            //
            // cboTipoBackup
            //
            this.cboTipoBackup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoBackup.FormattingEnabled = true;
            this.cboTipoBackup.Location = new System.Drawing.Point(120, 53);
            this.cboTipoBackup.Name = "cboTipoBackup";
            this.cboTipoBackup.Size = new System.Drawing.Size(250, 21);
            this.cboTipoBackup.TabIndex = 4;
            //
            // lblTipoBackup
            //
            this.lblTipoBackup.AutoSize = true;
            this.lblTipoBackup.Location = new System.Drawing.Point(16, 56);
            this.lblTipoBackup.Name = "lblTipoBackup";
            this.lblTipoBackup.Size = new System.Drawing.Size(35, 13);
            this.lblTipoBackup.TabIndex = 3;
            this.lblTipoBackup.Text = "Tipo:";
            //
            // cboBaseDatos
            //
            this.cboBaseDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaseDatos.FormattingEnabled = true;
            this.cboBaseDatos.Location = new System.Drawing.Point(120, 25);
            this.cboBaseDatos.Name = "cboBaseDatos";
            this.cboBaseDatos.Size = new System.Drawing.Size(250, 21);
            this.cboBaseDatos.TabIndex = 2;
            //
            // lblBaseDatos
            //
            this.lblBaseDatos.AutoSize = true;
            this.lblBaseDatos.Location = new System.Drawing.Point(16, 28);
            this.lblBaseDatos.Name = "lblBaseDatos";
            this.lblBaseDatos.Size = new System.Drawing.Size(85, 13);
            this.lblBaseDatos.TabIndex = 1;
            this.lblBaseDatos.Text = "Base de Datos:";
            //
            // btnCrearBackup
            //
            this.btnCrearBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrearBackup.Location = new System.Drawing.Point(720, 25);
            this.btnCrearBackup.Name = "btnCrearBackup";
            this.btnCrearBackup.Size = new System.Drawing.Size(220, 50);
            this.btnCrearBackup.TabIndex = 0;
            this.btnCrearBackup.Text = "Crear Backup";
            this.btnCrearBackup.UseVisualStyleBackColor = true;
            this.btnCrearBackup.Click += new System.EventHandler(this.btnCrearBackup_Click);
            //
            // grpRestaurar
            //
            this.grpRestaurar.Controls.Add(this.dgvBackups);
            this.grpRestaurar.Controls.Add(this.lblTotalBackups);
            this.grpRestaurar.Location = new System.Drawing.Point(12, 200);
            this.grpRestaurar.Name = "grpRestaurar";
            this.grpRestaurar.Size = new System.Drawing.Size(960, 340);
            this.grpRestaurar.TabIndex = 1;
            this.grpRestaurar.TabStop = false;
            this.grpRestaurar.Text = "Backups Disponibles";
            //
            // dgvBackups
            //
            this.dgvBackups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBackups.Location = new System.Drawing.Point(19, 25);
            this.dgvBackups.Name = "dgvBackups";
            this.dgvBackups.Size = new System.Drawing.Size(920, 280);
            this.dgvBackups.TabIndex = 0;
            //
            // lblTotalBackups
            //
            this.lblTotalBackups.AutoSize = true;
            this.lblTotalBackups.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalBackups.Location = new System.Drawing.Point(16, 315);
            this.lblTotalBackups.Name = "lblTotalBackups";
            this.lblTotalBackups.Size = new System.Drawing.Size(50, 13);
            this.lblTotalBackups.TabIndex = 1;
            this.lblTotalBackups.Text = "Total: 0";
            //
            // btnRestaurar
            //
            this.btnRestaurar.Location = new System.Drawing.Point(31, 550);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(150, 35);
            this.btnRestaurar.TabIndex = 2;
            this.btnRestaurar.Text = "Restaurar Seleccionado";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            //
            // btnRestaurarExterno
            //
            this.btnRestaurarExterno.Location = new System.Drawing.Point(195, 550);
            this.btnRestaurarExterno.Name = "btnRestaurarExterno";
            this.btnRestaurarExterno.Size = new System.Drawing.Size(150, 35);
            this.btnRestaurarExterno.TabIndex = 3;
            this.btnRestaurarExterno.Text = "Restaurar desde Archivo";
            this.btnRestaurarExterno.UseVisualStyleBackColor = true;
            this.btnRestaurarExterno.Click += new System.EventHandler(this.btnRestaurarExterno_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.Location = new System.Drawing.Point(359, 550);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(150, 35);
            this.btnEliminar.TabIndex = 4;
            this.btnEliminar.Text = "Eliminar Backup";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // btnRefrescar
            //
            this.btnRefrescar.Location = new System.Drawing.Point(523, 550);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(150, 35);
            this.btnRefrescar.TabIndex = 5;
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            //
            // btnVolver
            //
            this.btnVolver.Location = new System.Drawing.Point(822, 550);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(150, 35);
            this.btnVolver.TabIndex = 6;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            //
            // FrmGestionBackup
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 601);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.btnRefrescar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnRestaurarExterno);
            this.Controls.Add(this.btnRestaurar);
            this.Controls.Add(this.grpRestaurar);
            this.Controls.Add(this.grpCrearBackup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmGestionBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Backup y Restore";
            this.Load += new System.EventHandler(this.FrmGestionBackup_Load);
            this.grpCrearBackup.ResumeLayout(false);
            this.grpCrearBackup.PerformLayout();
            this.grpRestaurar.ResumeLayout(false);
            this.grpRestaurar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCrearBackup;
        private System.Windows.Forms.Button btnCrearBackup;
        private System.Windows.Forms.ComboBox cboBaseDatos;
        private System.Windows.Forms.Label lblBaseDatos;
        private System.Windows.Forms.ComboBox cboTipoBackup;
        private System.Windows.Forms.Label lblTipoBackup;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtRutaDestino;
        private System.Windows.Forms.Label lblRutaDestino;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.Label lblEspacioDisponible;
        private System.Windows.Forms.GroupBox grpRestaurar;
        private System.Windows.Forms.DataGridView dgvBackups;
        private System.Windows.Forms.Label lblTotalBackups;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnRestaurarExterno;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Button btnVolver;
    }
}
