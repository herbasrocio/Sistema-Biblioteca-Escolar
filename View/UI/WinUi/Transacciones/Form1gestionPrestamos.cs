using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using UI.WinUi;

namespace UI.WinUi.Transacciones
{
    public partial class Form1gestionPrestamos : BaseForm
    {
        private Usuario _usuarioLogueado;
        private registrarPrestamo _formRegistrarPrestamo;
        private renovarPrestamo _formRenovarPrestamo;

        public Form1gestionPrestamos()
        {
            InitializeComponent();
        }

        public Form1gestionPrestamos(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            this.Load += GestionPrestamos_Load;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private void GestionPrestamos_Load(object sender, EventArgs e)
        {
            // AplicarTraducciones() se llama automáticamente desde BaseForm.Load
            ConfigurarVisibilidadPestañas();
            CargarPestañaActual();
        }

        private void ConfigurarVisibilidadPestañas()
        {
            // La pestaña de renovar préstamo se muestra siempre si tiene permiso de Gestión Préstamos
            // El permiso de renovar está unificado con Gestión Préstamos en Usuario.TienePermiso()
            // Por lo tanto, si llegó aquí, ya tiene permiso de préstamos
            // No ocultamos la pestaña de renovar
        }

        protected override void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("gestion_prestamos");
                tabRegistrarPrestamo.Text = LanguageManager.Translate("registrar_prestamo");
                tabRenovarPrestamo.Text = LanguageManager.Translate("renovar_prestamo");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar traducciones: {ex.Message}");
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPestañaActual();
        }

        private void CargarPestañaActual()
        {
            // Solo cargar el formulario de la pestaña seleccionada
            if (tabControl.SelectedTab == tabRegistrarPrestamo)
            {
                // Solo cargar si no está cargado ya
                if (tabRegistrarPrestamo.Controls.Count == 0)
                {
                    CargarFormularioRegistrarPrestamo();
                }
            }
            else if (tabControl.SelectedTab == tabRenovarPrestamo)
            {
                // Solo cargar si no está cargado ya
                if (tabRenovarPrestamo.Controls.Count == 0)
                {
                    CargarFormularioRenovarPrestamo();
                }
            }
        }

        private void CargarFormularioRegistrarPrestamo()
        {
            if (_formRegistrarPrestamo == null || _formRegistrarPrestamo.IsDisposed)
            {
                _formRegistrarPrestamo = new registrarPrestamo(_usuarioLogueado);
                _formRegistrarPrestamo.TopLevel = false;
                _formRegistrarPrestamo.FormBorderStyle = FormBorderStyle.None;
                _formRegistrarPrestamo.Dock = DockStyle.Fill;
            }

            tabRegistrarPrestamo.Controls.Add(_formRegistrarPrestamo);
            _formRegistrarPrestamo.Show();
        }

        private void CargarFormularioRenovarPrestamo()
        {
            // El permiso ya fue verificado en el menú principal
            // Renovar préstamo está unificado con Gestión Préstamos
            if (_formRenovarPrestamo == null || _formRenovarPrestamo.IsDisposed)
            {
                _formRenovarPrestamo = new renovarPrestamo(_usuarioLogueado);
                _formRenovarPrestamo.TopLevel = false;
                _formRenovarPrestamo.FormBorderStyle = FormBorderStyle.None;
                _formRenovarPrestamo.Dock = DockStyle.Fill;
            }

            tabRenovarPrestamo.Controls.Add(_formRenovarPrestamo);
            _formRenovarPrestamo.Show();
        }

        private bool TienePermiso(string nombrePatente)
        {
            // Usar el método centralizado del Usuario que maneja el bypass de Administrador
            return _usuarioLogueado?.TienePermiso(nombrePatente) ?? false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Limpiar recursos
            if (_formRegistrarPrestamo != null && !_formRegistrarPrestamo.IsDisposed)
                _formRegistrarPrestamo.Dispose();

            if (_formRenovarPrestamo != null && !_formRenovarPrestamo.IsDisposed)
                _formRenovarPrestamo.Dispose();

            base.OnFormClosing(e);
        }
    }
}
