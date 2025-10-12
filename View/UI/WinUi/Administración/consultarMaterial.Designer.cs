namespace UI.WinUi.Administrador
{
    partial class consultarMaterial
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
            this.groupBoxFiltros = new System.Windows.Forms.GroupBox();
            this.cmbNivel = new System.Windows.Forms.ComboBox();
            this.cmbGenero = new System.Windows.Forms.ComboBox();
            this.lblNivel = new System.Windows.Forms.Label();
            this.lblGenero = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.txtAutor = new System.Windows.Forms.TextBox();
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblAutor = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvMateriales = new System.Windows.Forms.DataGridView();
            this.btnVolver = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnGestionarEjemplares = new System.Windows.Forms.Button();
            this.lblResultados = new System.Windows.Forms.Label();
            this.groupBoxFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriales)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxFiltros
            // 
            this.groupBoxFiltros.BackColor = System.Drawing.Color.White;
            this.groupBoxFiltros.Controls.Add(this.cmbNivel);
            this.groupBoxFiltros.Controls.Add(this.cmbGenero);
            this.groupBoxFiltros.Controls.Add(this.lblNivel);
            this.groupBoxFiltros.Controls.Add(this.lblGenero);
            this.groupBoxFiltros.Controls.Add(this.btnLimpiar);
            this.groupBoxFiltros.Controls.Add(this.btnBuscar);
            this.groupBoxFiltros.Controls.Add(this.cmbTipo);
            this.groupBoxFiltros.Controls.Add(this.txtAutor);
            this.groupBoxFiltros.Controls.Add(this.txtTitulo);
            this.groupBoxFiltros.Controls.Add(this.lblTipo);
            this.groupBoxFiltros.Controls.Add(this.lblAutor);
            this.groupBoxFiltros.Controls.Add(this.lblTitulo);
            this.groupBoxFiltros.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.groupBoxFiltros.Location = new System.Drawing.Point(30, 30);
            this.groupBoxFiltros.Name = "groupBoxFiltros";
            this.groupBoxFiltros.Size = new System.Drawing.Size(1280, 146);
            this.groupBoxFiltros.TabIndex = 0;
            this.groupBoxFiltros.TabStop = false;
            this.groupBoxFiltros.Text = "Filtros de Búsqueda";
            // 
            // cmbNivel
            // 
            this.cmbNivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNivel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbNivel.FormattingEnabled = true;
            this.cmbNivel.Location = new System.Drawing.Point(1080, 35);
            this.cmbNivel.Name = "cmbNivel";
            this.cmbNivel.Size = new System.Drawing.Size(160, 25);
            this.cmbNivel.TabIndex = 10;
            // 
            // cmbGenero
            // 
            this.cmbGenero.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenero.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbGenero.FormattingEnabled = true;
            this.cmbGenero.Location = new System.Drawing.Point(850, 35);
            this.cmbGenero.Name = "cmbGenero";
            this.cmbGenero.Size = new System.Drawing.Size(180, 25);
            this.cmbGenero.TabIndex = 8;
            // 
            // lblNivel
            // 
            this.lblNivel.AutoSize = true;
            this.lblNivel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblNivel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblNivel.Location = new System.Drawing.Point(1035, 38);
            this.lblNivel.Name = "lblNivel";
            this.lblNivel.Size = new System.Drawing.Size(40, 17);
            this.lblNivel.TabIndex = 11;
            this.lblNivel.Text = "Nivel:";
            // 
            // lblGenero
            // 
            this.lblGenero.AutoSize = true;
            this.lblGenero.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblGenero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblGenero.Location = new System.Drawing.Point(795, 38);
            this.lblGenero.Name = "lblGenero";
            this.lblGenero.Size = new System.Drawing.Size(54, 17);
            this.lblGenero.TabIndex = 9;
            this.lblGenero.Text = "Género:";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(675, 85);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(110, 35);
            this.btnLimpiar.TabIndex = 7;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(545, 85);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 35);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            // 
            // cmbTipo
            // 
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbTipo.FormattingEnabled = true;
            this.cmbTipo.Location = new System.Drawing.Point(590, 35);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(200, 25);
            this.cmbTipo.TabIndex = 5;
            // 
            // txtAutor
            // 
            this.txtAutor.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtAutor.Location = new System.Drawing.Point(340, 35);
            this.txtAutor.Name = "txtAutor";
            this.txtAutor.Size = new System.Drawing.Size(200, 25);
            this.txtAutor.TabIndex = 4;
            // 
            // txtTitulo
            // 
            this.txtTitulo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtTitulo.Location = new System.Drawing.Point(90, 35);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(200, 25);
            this.txtTitulo.TabIndex = 3;
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblTipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTipo.Location = new System.Drawing.Point(545, 38);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(37, 17);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Tipo:";
            // 
            // lblAutor
            // 
            this.lblAutor.AutoSize = true;
            this.lblAutor.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblAutor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblAutor.Location = new System.Drawing.Point(295, 38);
            this.lblAutor.Name = "lblAutor";
            this.lblAutor.Size = new System.Drawing.Size(43, 17);
            this.lblAutor.TabIndex = 1;
            this.lblAutor.Text = "Autor:";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTitulo.Location = new System.Drawing.Point(40, 38);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(43, 17);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Título:";
            // 
            // dgvMateriales
            // 
            this.dgvMateriales.AllowUserToAddRows = false;
            this.dgvMateriales.AllowUserToDeleteRows = false;
            this.dgvMateriales.BackgroundColor = System.Drawing.Color.White;
            this.dgvMateriales.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvMateriales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMateriales.Location = new System.Drawing.Point(30, 225);
            this.dgvMateriales.MultiSelect = false;
            this.dgvMateriales.Name = "dgvMateriales";
            this.dgvMateriales.ReadOnly = true;
            this.dgvMateriales.RowHeadersWidth = 51;
            this.dgvMateriales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMateriales.Size = new System.Drawing.Size(1280, 300);
            this.dgvMateriales.TabIndex = 1;
            this.dgvMateriales.TabStop = false;
            this.dgvMateriales.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMateriales_CellContentClick);
            // 
            // btnVolver
            // 
            this.btnVolver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnVolver.FlatAppearance.BorderSize = 0;
            this.btnVolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVolver.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnVolver.ForeColor = System.Drawing.Color.White;
            this.btnVolver.Location = new System.Drawing.Point(1200, 550);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(110, 35);
            this.btnVolver.TabIndex = 3;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.Location = new System.Drawing.Point(30, 550);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(130, 35);
            this.btnEditar.TabIndex = 2;
            this.btnEditar.Text = "Editar Material";
            this.btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnGestionarEjemplares
            // 
            this.btnGestionarEjemplares.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnGestionarEjemplares.FlatAppearance.BorderSize = 0;
            this.btnGestionarEjemplares.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGestionarEjemplares.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGestionarEjemplares.ForeColor = System.Drawing.Color.White;
            this.btnGestionarEjemplares.Location = new System.Drawing.Point(180, 550);
            this.btnGestionarEjemplares.Name = "btnGestionarEjemplares";
            this.btnGestionarEjemplares.Size = new System.Drawing.Size(160, 35);
            this.btnGestionarEjemplares.TabIndex = 4;
            this.btnGestionarEjemplares.Text = "Gestionar Ejemplares";
            this.btnGestionarEjemplares.UseVisualStyleBackColor = false;
            // 
            // lblResultados
            // 
            this.lblResultados.AutoSize = true;
            this.lblResultados.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblResultados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lblResultados.Location = new System.Drawing.Point(30, 193);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new System.Drawing.Size(170, 17);
            this.lblResultados.TabIndex = 3;
            this.lblResultados.Text = "Resultados encontrados: 0";
            // 
            // consultarMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1340, 600);
            this.Controls.Add(this.lblResultados);
            this.Controls.Add(this.btnGestionarEjemplares);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.dgvMateriales);
            this.Controls.Add(this.groupBoxFiltros);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "consultarMaterial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consultar Material";
            this.groupBoxFiltros.ResumeLayout(false);
            this.groupBoxFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMateriales)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFiltros;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblAutor;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblGenero;
        private System.Windows.Forms.Label lblNivel;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.TextBox txtAutor;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.ComboBox cmbGenero;
        private System.Windows.Forms.ComboBox cmbNivel;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.DataGridView dgvMateriales;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnGestionarEjemplares;
        private System.Windows.Forms.Label lblResultados;
    }
}
