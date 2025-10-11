using System.Drawing;
using System.Windows.Forms;

namespace UI.Helpers
{
    /// <summary>
    /// Clase estática con los colores y estilos del sistema
    /// Basado en el diseño del formulario Login
    /// </summary>
    public static class EstilosSistema
    {
        // Colores principales
        public static readonly Color ColorFondo = Color.FromArgb(236, 240, 241);
        public static readonly Color ColorPrimario = Color.FromArgb(52, 152, 219);
        public static readonly Color ColorTexto = Color.FromArgb(44, 62, 80);
        public static readonly Color ColorTextoSecundario = Color.FromArgb(127, 140, 141);
        public static readonly Color ColorBorde = Color.FromArgb(189, 195, 199);
        public static readonly Color ColorSeparador = Color.FromArgb(149, 165, 166);

        // Fuentes
        public static readonly Font FuenteTitulo = new Font("Segoe UI", 20.25F, FontStyle.Bold);
        public static readonly Font FuenteSubtitulo = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static readonly Font FuenteLabel = new Font("Segoe UI", 9.75F, FontStyle.Regular);
        public static readonly Font FuenteBoton = new Font("Segoe UI", 11.25F, FontStyle.Bold);
        public static readonly Font FuenteTexto = new Font("Segoe UI", 10.5F, FontStyle.Regular);
        public static readonly Font FuentePequeña = new Font("Segoe UI", 8.25F, FontStyle.Regular);

        /// <summary>
        /// Aplica el estilo del sistema a un formulario
        /// </summary>
        public static void AplicarEstiloFormulario(Form formulario)
        {
            formulario.BackColor = ColorFondo;
            formulario.Font = FuenteLabel;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un botón primario
        /// </summary>
        public static void AplicarEstiloBotonPrimario(Button boton)
        {
            boton.BackColor = ColorPrimario;
            boton.ForeColor = Color.White;
            boton.Font = FuenteBoton;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un botón secundario
        /// </summary>
        public static void AplicarEstiloBotonSecundario(Button boton)
        {
            boton.BackColor = Color.White;
            boton.ForeColor = ColorTexto;
            boton.Font = FuenteLabel;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderColor = ColorBorde;
            boton.FlatAppearance.BorderSize = 1;
            boton.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un TextBox
        /// </summary>
        public static void AplicarEstiloTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = FuenteTexto;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un Label de título
        /// </summary>
        public static void AplicarEstiloTitulo(Label label)
        {
            label.Font = FuenteTitulo;
            label.ForeColor = ColorTexto;
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un Label de subtítulo
        /// </summary>
        public static void AplicarEstiloSubtitulo(Label label)
        {
            label.Font = FuenteSubtitulo;
            label.ForeColor = ColorTexto;
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un Label normal
        /// </summary>
        public static void AplicarEstiloLabel(Label label)
        {
            label.Font = FuenteLabel;
            label.ForeColor = ColorTexto;
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un ComboBox
        /// </summary>
        public static void AplicarEstiloComboBox(ComboBox comboBox)
        {
            comboBox.Font = FuenteTexto;
            comboBox.FlatStyle = FlatStyle.Flat;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un DataGridView
        /// </summary>
        public static void AplicarEstiloDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.Font = FuenteLabel;

            // Estilo de encabezados
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorPrimario;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = ColorPrimario;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgv.ColumnHeadersHeight = 35;

            // Estilo de filas
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = ColorTexto;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241);
            dgv.DefaultCellStyle.SelectionForeColor = ColorTexto;
            dgv.DefaultCellStyle.Padding = new Padding(5);

            // Estilo de filas alternas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = ColorTexto;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = ColorTexto;

            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un GroupBox
        /// </summary>
        public static void AplicarEstiloGroupBox(GroupBox groupBox)
        {
            groupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            groupBox.ForeColor = ColorTexto;
        }

        /// <summary>
        /// Aplica el estilo del sistema a un Panel
        /// </summary>
        public static void AplicarEstiloPanel(Panel panel, bool esPanelBlanco = true)
        {
            if (esPanelBlanco)
            {
                panel.BackColor = Color.White;
                panel.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                panel.BackColor = ColorFondo;
            }
        }
    }
}
