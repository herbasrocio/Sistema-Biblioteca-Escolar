using System;
using System.Linq;
using System.Windows.Forms;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;

namespace UI.WinUi.Administrador
{
    public partial class menu : Form
    {
        private Usuario _usuarioLogueado;

        // Nombres de las patentes que controlan cada opción del menú
        private const string PATENTE_USUARIOS = "GestionUsuarios";
        private const string PATENTE_PERMISOS = "GestionPermisos";
        private const string PATENTE_CATALOGO = "GestionCatalogo";
        private const string PATENTE_ALUMNOS = "GestionAlumnos";
        private const string PATENTE_PRESTAMOS = "GestionPrestamos";
        private const string PATENTE_DEVOLUCIONES = "GestionDevoluciones";
        private const string PATENTE_REPORTES = "ConsultarReportes";

        public menu()
        {
            InitializeComponent();
        }

        public menu(Usuario usuario) : this()
        {
            _usuarioLogueado = usuario;
            ActualizarTextos();
            ConfigurarVisibilidadPorPermisos();
        }

        private void ActualizarTextos()
        {
            // Traducir textos del formulario
            this.Text = LanguageManager.Translate("menu_principal");

            // Traducir menú
            usuariosToolStripMenuItem.Text = LanguageManager.Translate("gestion_usuarios");
            permisosToolStripMenuItem.Text = LanguageManager.Translate("gestion_permisos");
            catalogoToolStripMenuItem.Text = LanguageManager.Translate("gestion_catalogo");
            alumnosToolStripMenuItem.Text = LanguageManager.Translate("gestionar_alumnos");
            prestamosToolStripMenuItem.Text = LanguageManager.Translate("prestamos");
            devolucionesToolStripMenuItem.Text = LanguageManager.Translate("devoluciones");
            reportesToolStripMenuItem.Text = LanguageManager.Translate("reportes");
            cerrarSesionToolStripMenuItem.Text = LanguageManager.Translate("cerrar_sesion");
        }

        private void ConfigurarVisibilidadPorPermisos()
        {
            // Configurar visibilidad de cada opción del menú según permisos
            usuariosToolStripMenuItem.Visible = TienePermiso(PATENTE_USUARIOS);
            permisosToolStripMenuItem.Visible = TienePermiso(PATENTE_PERMISOS);
            catalogoToolStripMenuItem.Visible = TienePermiso(PATENTE_CATALOGO);
            alumnosToolStripMenuItem.Visible = TienePermiso(PATENTE_ALUMNOS);
            prestamosToolStripMenuItem.Visible = TienePermiso(PATENTE_PRESTAMOS);
            devolucionesToolStripMenuItem.Visible = TienePermiso(PATENTE_DEVOLUCIONES);
            reportesToolStripMenuItem.Visible = TienePermiso(PATENTE_REPORTES);
        }

        private bool TienePermiso(string nombrePatente)
        {
            if (_usuarioLogueado?.Permisos == null)
                return false;

            foreach (var componente in _usuarioLogueado.Permisos)
            {
                if (TienePermisoRecursivo(componente, nombrePatente))
                    return true;
            }

            return false;
        }

        private bool TienePermisoRecursivo(Component componente, string nombrePatente)
        {
            if (componente == null)
                return false;

            // Si es una Patente, verificar si coincide con el nombre
            if (componente is Patente patente)
            {
                return patente.MenuItemName != null && patente.MenuItemName.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase);
            }

            // Si es una Familia, buscar recursivamente en sus hijos
            if (componente is Familia familia)
            {
                foreach (var hijo in familia.GetChildrens())
                {
                    if (hijo != null && TienePermisoRecursivo(hijo, nombrePatente))
                        return true;
                }
            }

            return false;
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                gestionUsuarios formGestion = new gestionUsuarios(_usuarioLogueado);
                formGestion.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                gestionPermisos formPermisos = new gestionPermisos(_usuarioLogueado);
                formPermisos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de permisos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void catalogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                gestionCatalogo formCatalogo = new gestionCatalogo(_usuarioLogueado);
                formCatalogo.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir gestión de catálogo: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void alumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                LanguageManager.Translate("funcionalidad_no_implementada"),
                LanguageManager.Translate("informacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void prestamosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                LanguageManager.Translate("funcionalidad_no_implementada"),
                LanguageManager.Translate("informacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void devolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                LanguageManager.Translate("funcionalidad_no_implementada"),
                LanguageManager.Translate("informacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                LanguageManager.Translate("funcionalidad_no_implementada"),
                LanguageManager.Translate("informacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var resultado = MessageBox.Show(
                    LanguageManager.Translate("confirmar_cerrar_sesion"),
                    LanguageManager.Translate("cerrar_sesion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Cerrar este formulario
                    this.Close();

                    // Mostrar el formulario de login nuevamente
                    Login loginForm = new Login();
                    loginForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
