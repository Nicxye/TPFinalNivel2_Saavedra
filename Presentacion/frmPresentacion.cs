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

namespace Presentacion
{
    public partial class VistaPrincipal : Form
    {
        public List<Articulo> listaArticuloForms;
        public VistaPrincipal()
        {
            InitializeComponent();
        }

        private void VistaPrincipal_Load(object sender, EventArgs e)
        {
            cargarDatos();
            cargarImagenLogo();
            ocultarColumas();
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
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
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
                pbxLogo.Load(imagen);

            }
            catch (Exception)
            {

                pbxLogo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZKmSgKaggimHO3m0vDIZubBD2I4b4hdW4sORN1xJNHxrlNZmeHPrNna1KYfc6UkCOygU&usqp=CAU");
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregar = new frmAgregar();
            agregar.ShowDialog();
            cargarDatos();
        }
    }
}
