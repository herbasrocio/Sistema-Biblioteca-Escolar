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

namespace UI.WinUi.Transacciones
{
    public partial class Form1gestionPrestamos : Form
    {
        private Usuario _usuarioLogueado;
        private registrarPrestamo _formRegistrarPrestamo;
        private registrarDevolucion _formRegistrarDevolucion;
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
            AplicarTraducciones();
            CargarPestañaActual();
        }

        private void AplicarTraducciones()
        {
            try
            {
                this.Text = LanguageManager.Translate("prestamos");
                tabRegistrarPrestamo.Text = LanguageManager.Translate("registrar_prestamo");
                tabRegistrarDevolucion.Text = LanguageManager.Translate("registrar_devolucion");
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
            // Limpiar controles de todas las pestañas
            tabRegistrarPrestamo.Controls.Clear();
            tabRegistrarDevolucion.Controls.Clear();
            tabRenovarPrestamo.Controls.Clear();

            // Cargar el formulario correspondiente según la pestaña seleccionada
            if (tabControl.SelectedTab == tabRegistrarPrestamo)
            {
                CargarFormularioRegistrarPrestamo();
            }
            else if (tabControl.SelectedTab == tabRegistrarDevolucion)
            {
                CargarFormularioRegistrarDevolucion();
            }
            else if (tabControl.SelectedTab == tabRenovarPrestamo)
            {
                CargarFormularioRenovarPrestamo();
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

        private void CargarFormularioRegistrarDevolucion()
        {
            if (_formRegistrarDevolucion == null || _formRegistrarDevolucion.IsDisposed)
            {
                _formRegistrarDevolucion = new registrarDevolucion(_usuarioLogueado);
                _formRegistrarDevolucion.TopLevel = false;
                _formRegistrarDevolucion.FormBorderStyle = FormBorderStyle.None;
                _formRegistrarDevolucion.Dock = DockStyle.Fill;
            }

            tabRegistrarDevolucion.Controls.Add(_formRegistrarDevolucion);
            _formRegistrarDevolucion.Show();
        }

        private void CargarFormularioRenovarPrestamo()
        {
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Limpiar recursos
            if (_formRegistrarPrestamo != null && !_formRegistrarPrestamo.IsDisposed)
                _formRegistrarPrestamo.Dispose();

            if (_formRegistrarDevolucion != null && !_formRegistrarDevolucion.IsDisposed)
                _formRegistrarDevolucion.Dispose();

            if (_formRenovarPrestamo != null && !_formRenovarPrestamo.IsDisposed)
                _formRenovarPrestamo.Dispose();

            base.OnFormClosing(e);
        }
    }
}
