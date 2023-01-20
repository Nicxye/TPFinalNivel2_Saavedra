using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmDetalles : Form
    {
        private Articulo seleccionado;
        public frmDetalles(Articulo seleccionado)
        {
            InitializeComponent();
            this.seleccionado = seleccionado;
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);

            }
            catch (Exception)
            {

                pbxArticulo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZKmSgKaggimHO3m0vDIZubBD2I4b4hdW4sORN1xJNHxrlNZmeHPrNna1KYfc6UkCOygU&usqp=CAU");
            }

        }

        private void frmDetalles_Load(object sender, EventArgs e)
        {
            CargarImagen(seleccionado.ImagenUrl);
            lblNombreArticulo.Text = seleccionado.Nombre;                 //La diferencia entre "label Articulo" y las demás
            lblPrecio.Text = "$" + seleccionado.Precio.ToString("0.00");  //es que las label sin "Articulo" son las que tienen negrita
            lblCodigoArticulo.Text = seleccionado.Codigo;                 //en la presentación de Forms, mientras que las Articulo contienen los datos del objeto.
            lblMarcaArticulo.Text = seleccionado.Marca.Descripcion;
            lblCategoriaArticulo.Text = seleccionado.Categoria.Descripcion;
            lblDescripcionArticulo.Text = seleccionado.Descripcion;
        }

    }
}
