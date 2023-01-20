using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using dominio;
using System.Media;

namespace Presentacion
{
    public partial class VistaPrincipal : Form
    {
        private List<Articulo> listaArticuloForms;
        public VistaPrincipal()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregar = new frmAgregar();
            agregar.Text = "Agregando artículo";
            agregar.ShowDialog();
            CargarDatos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (ValidarFiltro())
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;

                if (criterio == "Entre")
                {
                    string filtroSecundario = txtPrecioSecundario.Text;
                    dgvArticulos.DataSource = negocio.FiltrarArticulos(filtro, filtroSecundario);
                }
                else
                    dgvArticulos.DataSource = negocio.FiltrarArticulos(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (dgvArticulos.CurrentCell != null)
                {
                    Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    DialogResult respuesta = MessageBox.Show("¿De verdad quieres eliminar " + seleccionado.Nombre + "? Esta acción es irreversible.", "Eliminando artículo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (respuesta == DialogResult.Yes)
                    {
                        negocio.EliminarArticulo(seleccionado);
                        MessageBox.Show("Eliminado exitosamente.");
                        CargarDatos();
                    }
                }
                else
                {
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("No tiene ningún artículo seleccionado en la grilla", "Error.");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            MostrarElementos();
        }
        
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentCell != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmAgregar agregar = new frmAgregar(seleccionado);
                agregar.Text = "Modificando artículo";
                agregar.ShowDialog();
                CargarDatos();
            }
            else
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("No tiene ningún artículo seleccionado en la grilla", "Error.");
            }
        }
        
        private void CargarDatos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticuloForms = negocio.Listar();
                dgvArticulos.DataSource = listaArticuloForms;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);

            }
            catch (Exception)
            {

                pbxImagen.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZKmSgKaggimHO3m0vDIZubBD2I4b4hdW4sORN1xJNHxrlNZmeHPrNna1KYfc6UkCOygU&usqp=CAU");
            }

        }

        private void CargarImagenLogo()
        {
            try
            {
                pbxLogo.Image = Properties.Resources.mercadoliebre;

            }
            catch (Exception)
            {

                pbxLogo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZKmSgKaggimHO3m0vDIZubBD2I4b4hdW4sORN1xJNHxrlNZmeHPrNna1KYfc6UkCOygU&usqp=CAU");
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Menos de");
                cboCriterio.Items.Add("Entre");
                cboCriterio.Items.Add("Más de");

            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Nombre exacto");
                cboCriterio.Items.Add("Nombre posible");
            }
        }

        private void cboCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCriterio.SelectedItem.ToString();
            if (opcion == "Entre")
            {
                btnBuscar.Location = new Point(426, 630);
                lblEntre.Visible = true;
                txtPrecioSecundario.Visible = true;
            }
            else
            {
                btnBuscar.Location = new Point(426, 606);
                lblEntre.Visible = false;
                txtPrecioSecundario.Visible = false;
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.ImagenUrl);
                lblNombre.Text = seleccionado.Nombre;
                lblPrecio.Text = "$" + seleccionado.Precio.ToString("0.00");
                lblMarca.Text = seleccionado.Marca.Descripcion;
                lblCategoria.Text = seleccionado.Categoria.Descripcion;
            }
        }

        private void lblDetalles_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentCell != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmDetalles detalles = new frmDetalles(seleccionado);
                detalles.ShowDialog();
            }
            else
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("No tiene ningún artículo seleccionado en la grilla", "Error.");
            }
        }
        
        private void MostrarElementos()             //Muestra los elementos de filtro avanzado.
        {
            lblCampo.Visible = true;
            cboCampo.Visible = true;
            lblCriterio.Visible = true;
            cboCriterio.Visible = true;
            lblBusqueda.Visible = true;
            txtFiltro.Visible = true;
            btnBuscar.Visible = true;
        }

        private void OcultarColumas()               //Oculta columnas del DataGridView principal.
        {
            dgvArticulos.Columns["Codigo"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            dgvArticulos.Columns["Marca"].Visible = false;
            dgvArticulos.Columns["Categoria"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void OcultarElementos()             //Oculta los elementos de filtro avanzado.
        {
            lblCampo.Visible = false;
            cboCampo.Visible = false;
            lblCriterio.Visible = false;
            cboCriterio.Visible = false;
            lblBusqueda.Visible = false;
            txtFiltro.Visible = false;
            btnBuscar.Visible = false;
            lblEntre.Visible = false;
            txtPrecioSecundario.Visible = false;
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            string campo = cboCampo.SelectedItem.ToString();

            if (campo == "Precio")
            {
                if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 44 && e.KeyChar != 46)
                {
                    SystemSounds.Exclamation.Play();
                    e.Handled = true;
                }
            }
        }

        private void txtPrecioSecundario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 44 && e.KeyChar != 46)
            {
                SystemSounds.Exclamation.Play();
                e.Handled = true;
            }
        }

        private bool ValidarFiltro()
        {
            if (cboCampo.SelectedIndex < 0 || cboCriterio.SelectedIndex < 0)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Se requiere seleccionar un campo y un criterio a filtrar.", "No se pudo filtrar");
                return true;
            }
            if (cboCampo.SelectedItem == "Precio" && txtFiltro.Text == "")
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("No se ingresaron valores.", "No se pudo filtrar");
                return true;
            }
            if (txtPrecioSecundario.Visible == true && (txtPrecioSecundario.Text == "" || txtFiltro.Text == ""))
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Debe ingresar valores en ambos campos.", "No se pudo filtrar");
                return true;
            }

            return false;
        }

        private void VistaPrincipal_Load(object sender, EventArgs e)
        {
            CargarDatos();
            CargarImagenLogo();
            OcultarColumas();
            OcultarElementos();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoría");
            cboCampo.Items.Add("Precio");
        }

    }
}
