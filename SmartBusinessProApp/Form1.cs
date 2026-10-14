using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartBusinessProApp.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SmartBusinessProApp
{
    public partial class Form1 : Form
    {
        private Panel pnlHeader = null!;
        private Panel pnlFooter = null!;
        private Panel pnlCentral = null!;
        private Label lblTitulo = null!;
        private Label lblSubtitulo = null!;
        private Label lblInfoEstado = null!;
        private Button btnCargar = null!;
        private Button btnLimpiar = null!;

        public Form1()
        {
            InitializeComponent();
            ConfigurarDisenoLimpio();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void ConfigurarDisenoLimpio()
        {
            // 1. Ventana Principal
            this.Text = "SmartBusinessPro | Control de Inventario";
            this.Size = new Size(1000, 680);
            this.MinimumSize = new Size(900, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 250, 252);

            // 2. ENCABEZADO MÁS ALTO Y SEPARADO
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 95, // Mayor altura para separar el título del subtítulo
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            lblTitulo = new Label
            {
                Text = "SmartBusinessPro",
                Font = new Font("Segoe UI", 18, FontStyle.Bold), // Título más grande
                ForeColor = Color.FromArgb(15, 23, 42),
                AutoSize = true
            };

            lblSubtitulo = new Label
            {
                Text = "Catálogo general de productos y existencias en base de datos",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(lblSubtitulo);
            pnlHeader.Resize += (s, e) => CentrarHeader();
            CentrarHeader();

            // 3. BARRA INFERIOR (PIE DE PÁGINA)
            pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 75,
                BackColor = Color.White,
                Padding = new Padding(25, 15, 25, 15)
            };

            lblInfoEstado = new Label
            {
                Text = "Listo para consultar",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true,
                Location = new Point(25, 26)
            };

            btnCargar = CrearBoton("   Consultar Productos   ", Color.FromArgb(30, 58, 138), Color.White);
            btnLimpiar = CrearBoton("   Limpiar   ", Color.FromArgb(226, 232, 240), Color.FromArgb(30, 41, 59));

            btnCargar.Click += (s, e) => CargarProductos();
            btnLimpiar.Click += (s, e) => LimpiarTabla();

            FlowLayoutPanel pnlBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 0)
            };
            pnlBotones.Controls.Add(btnLimpiar);
            pnlBotones.Controls.Add(btnCargar);

            pnlFooter.Controls.Add(lblInfoEstado);
            pnlFooter.Controls.Add(pnlBotones);

            // 4. PANEL CENTRAL CON DATAGRIDVIEW
            pnlCentral = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25, 15, 25, 15),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            if (dgvProductos == null)
            {
                dgvProductos = new DataGridView();
            }

            dgvProductos.Dock = DockStyle.Fill;
            AplicarEstiloTabla(dgvProductos);

            pnlCentral.Controls.Add(dgvProductos);

            // Integración de controles
            this.Controls.Add(pnlCentral);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlHeader);
        }

        private void CentrarHeader()
        {
            if (lblTitulo != null && lblSubtitulo != null && pnlHeader != null)
            {
                // Mayor distancia vertical entre el título y subtítulo
                lblTitulo.Location = new Point((pnlHeader.Width - lblTitulo.Width) / 2, 16);
                lblSubtitulo.Location = new Point((pnlHeader.Width - lblSubtitulo.Width) / 2, 52);
            }
        }

        private Button CrearBoton(string texto, Color backColor, Color foreColor)
        {
            Button btn = new Button
            {
                Text = texto,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Height = 44, // Botón más alto y cómodo
                AutoSize = true,
                Margin = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand,
                Padding = new Padding(18, 0, 18, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void AplicarEstiloTabla(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 48; // Filas más altas con espacio desahogado

            // ENCABEZADO AZUL GRANDE Y AMPLIO
            dgv.ColumnHeadersHeight = 55; // Recuadro azul bastante más grande
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59), // Pizarra/Azul oscuro
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold), // Letras del encabezado más grandes
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 18, 0) // Más espacio interno para despegar textos
            };

            // Celdas de contenido
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                Font = new Font("Segoe UI", 10f), // Letras de datos un poco más grandes
                SelectionBackColor = Color.FromArgb(224, 242, 254),
                SelectionForeColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(18, 0, 18, 0)
            };

            // Filas alternadas suaves
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
        }

        private void CargarProductos()
        {
            try
            {
                using (var context = new SmartBusinessContext())
                {
                    var listaProductos = context.Productos
                        .Include(p => p.Categoria)
                        .Select(p => new
                        {
                            Nombre = p.Nombre,
                            Precio = p.Precio,
                            Existencia = p.Stock,
                            Categoria = p.Categoria.Nombre
                        })
                        .ToList();

                    dgvProductos.DataSource = listaProductos;
                    FormatearColumnas();
                    dgvProductos.ClearSelection();

                    lblInfoEstado.Text = $"✅ Conexión exitosa  |  {listaProductos.Count} productos cargados";
                    lblInfoEstado.ForeColor = Color.FromArgb(22, 101, 52);
                }
            }
            catch (SqlException ex)
            {
                lblInfoEstado.Text = "❌ Error al conectar con SQL Server";
                lblInfoEstado.ForeColor = Color.FromArgb(185, 28, 28);
                MessageBox.Show(
                    $"Error al comunicar con SQL Server:\n{ex.Message}",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                lblInfoEstado.Text = "⚠️ Error inesperado";
                lblInfoEstado.ForeColor = Color.FromArgb(180, 83, 9);
                MessageBox.Show(
                    $"Ocurrió un error inesperado:\n{ex.Message}",
                    "Error General",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void LimpiarTabla()
        {
            dgvProductos.DataSource = null;
            lblInfoEstado.Text = "Tabla limpia | Listo para consultar";
            lblInfoEstado.ForeColor = Color.FromArgb(71, 85, 105);
        }

        private void FormatearColumnas()
        {
            if (dgvProductos.Columns["Precio"] != null)
            {
                dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "$#,##0.00";
                dgvProductos.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvProductos.Columns["Existencia"] != null)
            {
                dgvProductos.Columns["Existencia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
    }
}