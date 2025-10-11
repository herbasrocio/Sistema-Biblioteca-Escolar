namespace UI.WinUi.Administrador
{
    partial class gestionAlumnos
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
            this.btnImportarCSV = new System.Windows.Forms.Button();
            this.btnPlantilla = new System.Windows.Forms.Button();
            this.lblGrado = new System.Windows.Forms.Label();
            this.cmbGrado = new System.Windows.Forms.ComboBox();
            this.lblListaAlumnos = new System.Windows.Forms.Label();
            this.dgvAlumnos = new System.Windows.Forms.DataGridView();
            this.btnExportarCSV = new System.Windows.Forms.Button();
            this.btnEditarGrado = new System.Windows.Forms.Button();
            this.separator = new System.Windows.Forms.Label();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnNuevoGrado = new System.Windows.Forms.Button();
            this.btnEliminarGrado = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).BeginInit();
            this.SuspendLayout();
            //
            // btnImportarCSV
            //
            this.btnImportarCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnImportarCSV.FlatAppearance.BorderSize = 0;
            this.btnImportarCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportarCSV.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnImportarCSV.ForeColor = System.Drawing.Color.White;
            this.btnImportarCSV.Location = new System.Drawing.Point(240, 20);
            this.btnImportarCSV.Name = "btnImportarCSV";
            this.btnImportarCSV.Size = new System.Drawing.Size(160, 40);
            this.btnImportarCSV.TabIndex = 0;
            this.btnImportarCSV.Text = "Importar alumnos";
            this.btnImportarCSV.UseVisualStyleBackColor = false;
            //
            // btnPlantilla
            //
            this.btnPlantilla.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnPlantilla.FlatAppearance.BorderSize = 0;
            this.btnPlantilla.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlantilla.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnPlantilla.ForeColor = System.Drawing.Color.White;
            this.btnPlantilla.Location = new System.Drawing.Point(410, 20);
            this.btnPlantilla.Name = "btnPlantilla";
            this.btnPlantilla.Size = new System.Drawing.Size(140, 40);
            this.btnPlantilla.TabIndex = 14;
            this.btnPlantilla.Text = "Descargar plantilla";
            this.btnPlantilla.UseVisualStyleBackColor = false;
            //
            // separator
            //
            this.separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separator.Location = new System.Drawing.Point(20, 75);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(760, 2);
            this.separator.TabIndex = 1;
            //
            // lblGrado
            //
            this.lblGrado.AutoSize = true;
            this.lblGrado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGrado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblGrado.Location = new System.Drawing.Point(20, 95);
            this.lblGrado.Name = "lblGrado";
            this.lblGrado.Size = new System.Drawing.Size(99, 15);
            this.lblGrado.TabIndex = 2;
            this.lblGrado.Text = "Grado / División:";
            //
            // cmbGrado
            //
            this.cmbGrado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbGrado.FormattingEnabled = true;
            this.cmbGrado.Location = new System.Drawing.Point(125, 92);
            this.cmbGrado.Name = "cmbGrado";
            this.cmbGrado.Size = new System.Drawing.Size(160, 23);
            this.cmbGrado.TabIndex = 3;
            //
            // lblListaAlumnos
            //
            this.lblListaAlumnos.AutoSize = true;
            this.lblListaAlumnos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblListaAlumnos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblListaAlumnos.Location = new System.Drawing.Point(20, 130);
            this.lblListaAlumnos.Name = "lblListaAlumnos";
            this.lblListaAlumnos.Size = new System.Drawing.Size(59, 15);
            this.lblListaAlumnos.TabIndex = 4;
            this.lblListaAlumnos.Text = "Alumnos:";
            //
            // dgvAlumnos
            //
            this.dgvAlumnos.AllowUserToAddRows = false;
            this.dgvAlumnos.AllowUserToDeleteRows = false;
            this.dgvAlumnos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlumnos.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlumnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlumnos.Location = new System.Drawing.Point(20, 150);
            this.dgvAlumnos.MultiSelect = true;
            this.dgvAlumnos.Name = "dgvAlumnos";
            this.dgvAlumnos.ReadOnly = true;
            this.dgvAlumnos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlumnos.Size = new System.Drawing.Size(760, 280);
            this.dgvAlumnos.TabIndex = 5;
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblTotal.Location = new System.Drawing.Point(630, 130);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 15);
            this.lblTotal.TabIndex = 6;
            //
            // btnNuevo
            //
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnNuevo.FlatAppearance.BorderSize = 0;
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Location = new System.Drawing.Point(20, 445);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(110, 35);
            this.btnNuevo.TabIndex = 7;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            //
            // btnEditar
            //
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.Location = new System.Drawing.Point(140, 445);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(110, 35);
            this.btnEditar.TabIndex = 8;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            //
            // btnEliminar
            //
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(260, 445);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(110, 35);
            this.btnEliminar.TabIndex = 9;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            //
            // btnExportarCSV
            //
            this.btnExportarCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnExportarCSV.FlatAppearance.BorderSize = 0;
            this.btnExportarCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarCSV.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnExportarCSV.ForeColor = System.Drawing.Color.White;
            this.btnExportarCSV.Location = new System.Drawing.Point(520, 445);
            this.btnExportarCSV.Name = "btnExportarCSV";
            this.btnExportarCSV.Size = new System.Drawing.Size(120, 35);
            this.btnExportarCSV.TabIndex = 10;
            this.btnExportarCSV.Text = "Exportar Excel";
            this.btnExportarCSV.UseVisualStyleBackColor = false;
            //
            // btnNuevoGrado
            //
            this.btnNuevoGrado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnNuevoGrado.FlatAppearance.BorderSize = 0;
            this.btnNuevoGrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevoGrado.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnNuevoGrado.ForeColor = System.Drawing.Color.White;
            this.btnNuevoGrado.Location = new System.Drawing.Point(295, 90);
            this.btnNuevoGrado.Name = "btnNuevoGrado";
            this.btnNuevoGrado.Size = new System.Drawing.Size(70, 25);
            this.btnNuevoGrado.TabIndex = 12;
            this.btnNuevoGrado.Text = "+ Grado";
            this.btnNuevoGrado.UseVisualStyleBackColor = false;
            //
            // btnEliminarGrado
            //
            this.btnEliminarGrado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnEliminarGrado.FlatAppearance.BorderSize = 0;
            this.btnEliminarGrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminarGrado.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnEliminarGrado.ForeColor = System.Drawing.Color.White;
            this.btnEliminarGrado.Location = new System.Drawing.Point(370, 90);
            this.btnEliminarGrado.Name = "btnEliminarGrado";
            this.btnEliminarGrado.Size = new System.Drawing.Size(70, 25);
            this.btnEliminarGrado.TabIndex = 13;
            this.btnEliminarGrado.Text = "- Grado";
            this.btnEliminarGrado.UseVisualStyleBackColor = false;
            //
            // btnEditarGrado
            //
            this.btnEditarGrado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.btnEditarGrado.FlatAppearance.BorderSize = 0;
            this.btnEditarGrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditarGrado.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnEditarGrado.ForeColor = System.Drawing.Color.White;
            this.btnEditarGrado.Location = new System.Drawing.Point(650, 445);
            this.btnEditarGrado.Name = "btnEditarGrado";
            this.btnEditarGrado.Size = new System.Drawing.Size(130, 35);
            this.btnEditarGrado.TabIndex = 11;
            this.btnEditarGrado.Text = "Cambiar grado";
            this.btnEditarGrado.UseVisualStyleBackColor = false;
            //
            // gestionAlumnos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btnPlantilla);
            this.Controls.Add(this.btnNuevoGrado);
            this.Controls.Add(this.btnEliminarGrado);
            this.Controls.Add(this.btnEditarGrado);
            this.Controls.Add(this.btnExportarCSV);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dgvAlumnos);
            this.Controls.Add(this.lblListaAlumnos);
            this.Controls.Add(this.cmbGrado);
            this.Controls.Add(this.lblGrado);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.btnImportarCSV);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "gestionAlumnos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestionar Alumnos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImportarCSV;
        private System.Windows.Forms.Button btnPlantilla;
        private System.Windows.Forms.Label separator;
        private System.Windows.Forms.Label lblGrado;
        private System.Windows.Forms.ComboBox cmbGrado;
        private System.Windows.Forms.Label lblListaAlumnos;
        private System.Windows.Forms.DataGridView dgvAlumnos;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnExportarCSV;
        private System.Windows.Forms.Button btnEditarGrado;
        private System.Windows.Forms.Button btnNuevoGrado;
        private System.Windows.Forms.Button btnEliminarGrado;
    }
}
