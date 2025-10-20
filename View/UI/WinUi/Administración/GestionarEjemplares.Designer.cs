namespace UI.WinUi.Administrador
{
    partial class GestionarEjemplares
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
            this.groupBoxInfoMaterial = new System.Windows.Forms.GroupBox();
            this.lblAutorValor = new System.Windows.Forms.Label();
            this.lblTituloValor = new System.Windows.Forms.Label();
            this.lblAutorLabel = new System.Windows.Forms.Label();
            this.lblTituloLabel = new System.Windows.Forms.Label();
            this.dgvEjemplares = new System.Windows.Forms.DataGridView();
            this.groupBoxAcciones = new System.Windows.Forms.GroupBox();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCambiarEstado = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.lblTituloForm = new System.Windows.Forms.Label();
            this.groupBoxInfoMaterial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjemplares)).BeginInit();
            this.groupBoxAcciones.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBoxInfoMaterial
            //
            this.groupBoxInfoMaterial.Controls.Add(this.lblAutorValor);
            this.groupBoxInfoMaterial.Controls.Add(this.lblTituloValor);
            this.groupBoxInfoMaterial.Controls.Add(this.lblAutorLabel);
            this.groupBoxInfoMaterial.Controls.Add(this.lblTituloLabel);
            this.groupBoxInfoMaterial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxInfoMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.groupBoxInfoMaterial.Location = new System.Drawing.Point(20, 60);
            this.groupBoxInfoMaterial.Name = "groupBoxInfoMaterial";
            this.groupBoxInfoMaterial.Size = new System.Drawing.Size(840, 80);
            this.groupBoxInfoMaterial.TabIndex = 0;
            this.groupBoxInfoMaterial.TabStop = false;
            this.groupBoxInfoMaterial.Text = "Información del Material";
            //
            // lblAutorValor
            //
            this.lblAutorValor.AutoSize = true;
            this.lblAutorValor.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblAutorValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblAutorValor.Location = new System.Drawing.Point(560, 35);
            this.lblAutorValor.Name = "lblAutorValor";
            this.lblAutorValor.Size = new System.Drawing.Size(18, 19);
            this.lblAutorValor.TabIndex = 3;
            this.lblAutorValor.Text = "...";
            //
            // lblTituloValor
            //
            this.lblTituloValor.AutoSize = true;
            this.lblTituloValor.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblTituloValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTituloValor.Location = new System.Drawing.Point(80, 35);
            this.lblTituloValor.Name = "lblTituloValor";
            this.lblTituloValor.Size = new System.Drawing.Size(18, 19);
            this.lblTituloValor.TabIndex = 2;
            this.lblTituloValor.Text = "...";
            //
            // lblAutorLabel
            //
            this.lblAutorLabel.AutoSize = true;
            this.lblAutorLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblAutorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblAutorLabel.Location = new System.Drawing.Point(500, 35);
            this.lblAutorLabel.Name = "lblAutorLabel";
            this.lblAutorLabel.Size = new System.Drawing.Size(48, 17);
            this.lblAutorLabel.TabIndex = 1;
            this.lblAutorLabel.Text = "Autor:";
            //
            // lblTituloLabel
            //
            this.lblTituloLabel.AutoSize = true;
            this.lblTituloLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTituloLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTituloLabel.Location = new System.Drawing.Point(20, 35);
            this.lblTituloLabel.Name = "lblTituloLabel";
            this.lblTituloLabel.Size = new System.Drawing.Size(50, 17);
            this.lblTituloLabel.TabIndex = 0;
            this.lblTituloLabel.Text = "Título:";
            //
            // dgvEjemplares
            //
            this.dgvEjemplares.AllowUserToAddRows = false;
            this.dgvEjemplares.AllowUserToDeleteRows = false;
            this.dgvEjemplares.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvEjemplares.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.dgvEjemplares.BackgroundColor = System.Drawing.Color.White;
            this.dgvEjemplares.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEjemplares.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvEjemplares.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvEjemplares.ColumnHeadersHeight = 35;
            this.dgvEjemplares.EnableHeadersVisualStyles = false;
            this.dgvEjemplares.Location = new System.Drawing.Point(20, 160);
            this.dgvEjemplares.MultiSelect = false;
            this.dgvEjemplares.Name = "dgvEjemplares";
            this.dgvEjemplares.ReadOnly = true;
            this.dgvEjemplares.RowHeadersVisible = false;
            this.dgvEjemplares.RowTemplate.Height = 35;
            this.dgvEjemplares.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEjemplares.Size = new System.Drawing.Size(840, 320);
            this.dgvEjemplares.TabIndex = 1;
            //
            // groupBoxAcciones
            //
            this.groupBoxAcciones.Controls.Add(this.btnVolver);
            this.groupBoxAcciones.Controls.Add(this.btnEliminar);
            this.groupBoxAcciones.Controls.Add(this.btnCambiarEstado);
            this.groupBoxAcciones.Controls.Add(this.btnEditar);
            this.groupBoxAcciones.Controls.Add(this.btnAgregar);
            this.groupBoxAcciones.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxAcciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.groupBoxAcciones.Location = new System.Drawing.Point(20, 500);
            this.groupBoxAcciones.Name = "groupBoxAcciones";
            this.groupBoxAcciones.Size = new System.Drawing.Size(840, 120);
            this.groupBoxAcciones.TabIndex = 2;
            this.groupBoxAcciones.TabStop = false;
            this.groupBoxAcciones.Text = "Acciones";
            //
            // btnVolver
            //
            this.btnVolver.BackColor = System.Drawing.Color.White;
            this.btnVolver.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVolver.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.btnVolver.FlatAppearance.BorderSize = 1;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnVolver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnVolver.Location = new System.Drawing.Point(680, 40);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(130, 50);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            //
            // btnEliminar
            //
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Location = new System.Drawing.Point(500, 40);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(160, 50);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar Ejemplar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            //
            // btnCambiarEstado
            //
            this.btnCambiarEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnCambiarEstado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCambiarEstado.FlatAppearance.BorderSize = 0;
            this.btnCambiarEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarEstado.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnCambiarEstado.ForeColor = System.Drawing.Color.White;
            this.btnCambiarEstado.Location = new System.Drawing.Point(340, 40);
            this.btnCambiarEstado.Name = "btnCambiarEstado";
            this.btnCambiarEstado.Size = new System.Drawing.Size(150, 50);
            this.btnCambiarEstado.TabIndex = 2;
            this.btnCambiarEstado.Text = "Cambiar Estado";
            this.btnCambiarEstado.UseVisualStyleBackColor = false;
            //
            // btnEditar
            //
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.Location = new System.Drawing.Point(180, 40);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(150, 50);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar Ejemplar";
            this.btnEditar.UseVisualStyleBackColor = false;
            //
            // btnAgregar
            //
            this.btnAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(20, 40);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(150, 50);
            this.btnAgregar.TabIndex = 0;
            this.btnAgregar.Text = "Agregar Ejemplar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            //
            // lblTituloForm
            //
            this.lblTituloForm.AutoSize = true;
            this.lblTituloForm.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTituloForm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTituloForm.Location = new System.Drawing.Point(20, 20);
            this.lblTituloForm.Name = "lblTituloForm";
            this.lblTituloForm.Size = new System.Drawing.Size(195, 25);
            this.lblTituloForm.TabIndex = 3;
            this.lblTituloForm.Text = "Gestionar Ejemplares";
            //
            // GestionarEjemplares
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(880, 650);
            this.Controls.Add(this.lblTituloForm);
            this.Controls.Add(this.groupBoxAcciones);
            this.Controls.Add(this.dgvEjemplares);
            this.Controls.Add(this.groupBoxInfoMaterial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "GestionarEjemplares";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestionar Ejemplares";
            this.groupBoxInfoMaterial.ResumeLayout(false);
            this.groupBoxInfoMaterial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjemplares)).EndInit();
            this.groupBoxAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxInfoMaterial;
        private System.Windows.Forms.Label lblAutorValor;
        private System.Windows.Forms.Label lblTituloValor;
        private System.Windows.Forms.Label lblAutorLabel;
        private System.Windows.Forms.Label lblTituloLabel;
        private System.Windows.Forms.DataGridView dgvEjemplares;
        private System.Windows.Forms.GroupBox groupBoxAcciones;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCambiarEstado;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Label lblTituloForm;
    }
}
