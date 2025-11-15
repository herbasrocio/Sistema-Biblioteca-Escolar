using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DomainModel;
using ServicesSecurity.BLL;
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security.Composite;

namespace UI.WinUi.Administrador
{
    public partial class FrmGestionBackup : BaseForm
    {
        private readonly BackupBLL _backupBLL;
        private Usuario _usuarioLogueado;
        private List<Backup> _backupsActuales;

        public FrmGestionBackup()
        {
            InitializeComponent();
        }

        public FrmGestionBackup(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            _backupBLL = new BackupBLL();
        }

        private void FrmGestionBackup_Load(object sender, EventArgs e)
        {
            try
            {
                // Verificar permisos
                if (_usuarioLogueado == null || !_usuarioLogueado.TienePermiso("FrmGestionBackup"))
                {
                    MessageBox.Show(LanguageManager.Translate("sin_permisos"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                ConfigurarFormulario();
                ConfigurarDataGridView();
                CargarBackups();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void AplicarTraducciones()
        {
            this.Text = LanguageManager.Translate("backup_titulo");

            grpCrearBackup.Text = LanguageManager.Translate("backup_crear_grupo");
            lblBaseDatos.Text = LanguageManager.Translate("backup_base_datos") + ":";
            lblTipoBackup.Text = LanguageManager.Translate("backup_tipo") + ":";
            lblDescripcion.Text = LanguageManager.Translate("backup_descripcion") + ":";
            lblRutaDestino.Text = LanguageManager.Translate("backup_ruta_destino") + ":";

            btnCrearBackup.Text = LanguageManager.Translate("backup_crear");
            btnExaminar.Text = LanguageManager.Translate("backup_examinar");

            grpRestaurar.Text = LanguageManager.Translate("backup_restaurar_grupo");
            btnRestaurar.Text = LanguageManager.Translate("backup_restaurar");
            btnRestaurarExterno.Text = LanguageManager.Translate("backup_restaurar_externo");
            btnEliminar.Text = LanguageManager.Translate("backup_eliminar");
            btnRefrescar.Text = LanguageManager.Translate("backup_refrescar");

            btnVolver.Text = LanguageManager.Translate("volver");

            // ComboBox Base de Datos
            cboBaseDatos.Items.Clear();
            cboBaseDatos.Items.Add("SeguridadBiblioteca");
            cboBaseDatos.Items.Add("NegocioBiblioteca");
            cboBaseDatos.SelectedIndex = 0;

            // ComboBox Tipo
            cboTipoBackup.Items.Clear();
            cboTipoBackup.Items.Add(new ComboBoxItem { Text = LanguageManager.Translate("backup_tipo_full"), Value = "Full" });
            cboTipoBackup.Items.Add(new ComboBoxItem { Text = LanguageManager.Translate("backup_tipo_differential"), Value = "Differential" });
            cboTipoBackup.DisplayMember = "Text";
            cboTipoBackup.ValueMember = "Value";
            cboTipoBackup.SelectedIndex = 0;
        }

        private void ConfigurarFormulario()
        {
            // Mostrar ruta por defecto
            txtRutaDestino.Text = _backupBLL.RutaBackupsPorDefecto;
            txtRutaDestino.ReadOnly = true;

            // Mostrar espacio disponible
            long espacioMB = _backupBLL.ObtenerEspacioDisponibleMB();
            lblEspacioDisponible.Text = $"{LanguageManager.Translate("backup_espacio_disponible")}: {FormatearTamaño(espacioMB)}";
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        private void ConfigurarDataGridView()
        {
            dgvBackups.AutoGenerateColumns = false;
            dgvBackups.ReadOnly = true;
            dgvBackups.AllowUserToAddRows = false;
            dgvBackups.AllowUserToDeleteRows = false;
            dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBackups.MultiSelect = false;
            dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvBackups.Columns.Clear();

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IdBackup",
                HeaderText = "ID",
                DataPropertyName = "IdBackup",
                Width = 50,
                Visible = false
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FechaCreacion",
                HeaderText = LanguageManager.Translate("backup_fecha"),
                DataPropertyName = "FechaCreacion",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" },
                Width = 150
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreBaseDatos",
                HeaderText = LanguageManager.Translate("backup_base_datos"),
                DataPropertyName = "NombreBaseDatos",
                Width = 150
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TipoBackup",
                HeaderText = LanguageManager.Translate("backup_tipo"),
                DataPropertyName = "TipoBackup",
                Width = 100
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TamañoFormateado",
                HeaderText = LanguageManager.Translate("backup_tamaño"),
                DataPropertyName = "TamañoFormateado",
                Width = 100
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Estado",
                HeaderText = LanguageManager.Translate("backup_estado"),
                DataPropertyName = "Estado",
                Width = 100
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreUsuario",
                HeaderText = LanguageManager.Translate("backup_usuario"),
                DataPropertyName = "NombreUsuario",
                Width = 120
            });

            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreArchivo",
                HeaderText = LanguageManager.Translate("backup_archivo"),
                DataPropertyName = "NombreArchivo",
                Width = 250
            });

            // Colorear filas según estado
            dgvBackups.CellFormatting += DgvBackups_CellFormatting;
        }

        private void DgvBackups_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvBackups.Rows[e.RowIndex].DataBoundItem is Backup backup)
            {
                if (backup.Estado == "Fallido")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                    e.CellStyle.ForeColor = Color.DarkRed;
                }
                else if (backup.Estado == "Exitoso")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                }
                else if (backup.Estado == "En Proceso")
                {
                    e.CellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        private void CargarBackups()
        {
            try
            {
                _backupsActuales = _backupBLL.ObtenerTodosLosBackups();
                dgvBackups.DataSource = null;
                dgvBackups.DataSource = _backupsActuales;

                lblTotalBackups.Text = $"{LanguageManager.Translate("backup_total")}: {_backupsActuales.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar backups: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCrearBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (cboBaseDatos.SelectedItem == null)
                {
                    MessageBox.Show(LanguageManager.Translate("backup_seleccione_bd"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (cboTipoBackup.SelectedItem == null)
                {
                    MessageBox.Show(LanguageManager.Translate("backup_seleccione_tipo"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                string nombreBD = cboBaseDatos.SelectedItem.ToString();
                string tipoBackup = ((ComboBoxItem)cboTipoBackup.SelectedItem).Value;
                string descripcion = txtDescripcion.Text.Trim();
                string rutaDestino = string.IsNullOrEmpty(txtRutaDestino.Text) ? null : txtRutaDestino.Text;

                // Confirmar
                DialogResult result = MessageBox.Show(
                    string.Format(LanguageManager.Translate("backup_confirmar_crear"), nombreBD, tipoBackup),
                    LanguageManager.Translate("confirmacion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Mostrar progreso
                Cursor = Cursors.WaitCursor;
                btnCrearBackup.Enabled = false;

                // Crear backup
                Backup backup;
                if (tipoBackup == "Full")
                {
                    backup = _backupBLL.CrearBackup(nombreBD, _usuarioLogueado, descripcion, rutaDestino);
                }
                else
                {
                    backup = _backupBLL.CrearBackupDiferencial(nombreBD, _usuarioLogueado, descripcion, rutaDestino);
                }

                MessageBox.Show(
                    string.Format(LanguageManager.Translate("backup_creado_exitosamente"), backup.NombreArchivo, backup.TamañoFormateado),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Limpiar formulario
                txtDescripcion.Clear();
                cboTipoBackup.SelectedIndex = 0;

                // Refrescar lista
                CargarBackups();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear backup: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnCrearBackup.Enabled = true;
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBackups.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("backup_seleccione_restaurar"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Backup backupSeleccionado = (Backup)dgvBackups.SelectedRows[0].DataBoundItem;

                // Confirmar (ADVERTENCIA CRÍTICA)
                DialogResult result = MessageBox.Show(
                    string.Format(LanguageManager.Translate("backup_confirmar_restaurar"),
                        backupSeleccionado.NombreBaseDatos, backupSeleccionado.NombreArchivo),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                // Segunda confirmación
                result = MessageBox.Show(
                    LanguageManager.Translate("backup_confirmar_restaurar_segunda"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation);

                if (result != DialogResult.Yes)
                    return;

                // Mostrar progreso
                Cursor = Cursors.WaitCursor;
                btnRestaurar.Enabled = false;

                // Ejecutar restore
                _backupBLL.RestaurarBackup(backupSeleccionado.IdBackup, _usuarioLogueado);

                MessageBox.Show(
                    LanguageManager.Translate("backup_restaurado_exitosamente"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Advertir sobre reconexión
                MessageBox.Show(
                    LanguageManager.Translate("backup_reiniciar_aplicacion"),
                    LanguageManager.Translate("informacion"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restaurar backup: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnRestaurar.Enabled = true;
            }
        }

        private void btnRestaurarExterno_Click(object sender, EventArgs e)
        {
            try
            {
                // Abrir diálogo para seleccionar archivo
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = LanguageManager.Translate("backup_seleccionar_archivo"),
                    Filter = "Archivos de Backup (*.bak)|*.bak|Todos los archivos (*.*)|*.*",
                    FilterIndex = 1
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string rutaArchivo = openFileDialog.FileName;

                // Preguntar qué base de datos restaurar
                string nombreBD = cboBaseDatos.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(nombreBD))
                {
                    MessageBox.Show(LanguageManager.Translate("backup_seleccione_bd_restaurar"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Confirmar
                DialogResult result = MessageBox.Show(
                    string.Format(LanguageManager.Translate("backup_confirmar_restaurar_externo"),
                        nombreBD, Path.GetFileName(rutaArchivo)),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                // Segunda confirmación
                result = MessageBox.Show(
                    LanguageManager.Translate("backup_confirmar_restaurar_segunda"),
                    LanguageManager.Translate("advertencia"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation);

                if (result != DialogResult.Yes)
                    return;

                // Mostrar progreso
                Cursor = Cursors.WaitCursor;
                btnRestaurarExterno.Enabled = false;

                // Ejecutar restore
                _backupBLL.RestaurarDesdeArchivo(nombreBD, rutaArchivo, _usuarioLogueado);

                MessageBox.Show(
                    LanguageManager.Translate("backup_restaurado_exitosamente"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                MessageBox.Show(
                    LanguageManager.Translate("backup_reiniciar_aplicacion"),
                    LanguageManager.Translate("informacion"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restaurar backup: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnRestaurarExterno.Enabled = true;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBackups.SelectedRows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.Translate("backup_seleccione_eliminar"),
                        LanguageManager.Translate("error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Backup backupSeleccionado = (Backup)dgvBackups.SelectedRows[0].DataBoundItem;

                // Confirmar
                DialogResult result = MessageBox.Show(
                    string.Format(LanguageManager.Translate("backup_confirmar_eliminar"), backupSeleccionado.NombreArchivo),
                    LanguageManager.Translate("confirmacion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Eliminar
                _backupBLL.EliminarBackup(backupSeleccionado.IdBackup, _usuarioLogueado, true);

                MessageBox.Show(
                    LanguageManager.Translate("backup_eliminado_exitosamente"),
                    LanguageManager.Translate("exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Refrescar lista
                CargarBackups();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar backup: {ex.Message}",
                    LanguageManager.Translate("error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarBackups();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = LanguageManager.Translate("backup_seleccionar_carpeta");
                folderDialog.SelectedPath = _backupBLL.RutaBackupsPorDefecto;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtRutaDestino.Text = folderDialog.SelectedPath;

                    // Actualizar espacio disponible
                    long espacioMB = _backupBLL.ObtenerEspacioDisponibleMB(folderDialog.SelectedPath);
                    lblEspacioDisponible.Text = $"{LanguageManager.Translate("backup_espacio_disponible")}: {FormatearTamaño(espacioMB)}";
                }
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string FormatearTamaño(long tamañoMB)
        {
            if (tamañoMB < 1024)
                return $"{tamañoMB:N0} MB";
            else
                return $"{(tamañoMB / 1024.0):N2} GB";
        }
    }
}
