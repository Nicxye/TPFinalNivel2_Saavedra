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

        private void VistaPrincipal_Load(object sender, EventArgs e)
        {
            cargarDatos();
            cargarImagenLogo();
            ocultarColumas();
            ocultarElementos();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoría");
            cboCampo.Items.Add("Precio");
        }

        private void cargarDatos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticuloForms = negocio.listar();
                dgvArticulos.DataSource = listaArticuloForms;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumas()
        {
            dgvArticulos.Columns["Codigo"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            dgvArticulos.Columns["Marca"].Visible = false;
            dgvArticulos.Columns["Categoria"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void ocultarElementos()
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

        private void mostrarElementos()
        {
            lblCampo.Visible = true;
            cboCampo.Visible = true;
            lblCriterio.Visible = true;
            cboCriterio.Visible = true;
            lblBusqueda.Visible = true;
            txtFiltro.Visible = true;
            btnBuscar.Visible = true;
        }

        private void cargarImagenLogo()
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
        private void cargarImagen(string imagen)
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregar = new frmAgregar();
            agregar.Text = "Agregando artículo";
            agregar.ShowDialog();
            cargarDatos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAgregar agregar = new frmAgregar(seleccionado);
            agregar.Text = "Modificando artículo";
            agregar.ShowDialog();
            cargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                DialogResult respuesta = MessageBox.Show("¿De verdad quieres eliminar " + seleccionado.Nombre + "? Esta acción es permanente.", "Eliminando artículo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    negocio.eliminarArticulo(seleccionado);
                    MessageBox.Show("Eliminado exitosamente.");
                    cargarDatos();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            mostrarElementos();
        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0 || cboCriterio.SelectedIndex < 0)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("Se requiere seleccionar un campo y un criterio a filtrar.");
                return true;
            }

            return false;
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (validarFiltro())
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                dgvArticulos.DataSource = negocio.filtrarArticulos(campo, criterio, filtro);

                if (criterio == "Entre")
                {
                    string filtroSecundario = txtPrecioSecundario.Text;
                    dgvArticulos.DataSource = negocio.filtrarArticulos(filtro, filtroSecundario);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
                lblNombre.Text = seleccionado.Nombre;
                lblPrecio.Text = seleccionado.Precio.ToString();
                lblMarca.Text = seleccionado.Marca.Descripcion;
                lblCategoria.Text = seleccionado.Categoria.Descripcion;
            }
        }
    }
}
