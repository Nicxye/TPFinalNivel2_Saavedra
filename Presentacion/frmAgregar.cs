using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmAgregar : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog imagen = null;
        public frmAgregar()
        {
            InitializeComponent();
        }
        public frmAgregar(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificado exitosamente";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                if (ValidarCampos())        //No permite avanzar si se cumple.
                    return;

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (articulo.Id != 0)       //Verifica si existe el artículo en la base de datos. Si "true", modifica en lugar de agregar.
                {
                    negocio.ModificarArticulo(articulo);
                    MessageBox.Show(articulo.Nombre + " modificado exitosamente");
                }
                else
                {
                    negocio.AgregarArticulo(articulo);
                    LimpiarTexto();
                    MessageBox.Show("Agregado exitosamente.");
                }
                if (imagen != null && txtImagenUrl.Text.ToUpper().Contains("HTTP"))
                    File.Copy(imagen.FileName, ConfigurationManager.AppSettings["images-folder"] + imagen.SafeFileName);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            imagen = new OpenFileDialog();
            imagen.Filter = "jpg|*.jpg; |png|*.png";

            if (imagen.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = imagen.FileName;
                CargarImagen(imagen.FileName);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
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

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    lblAgregar.Text = "MODIFICAR ARTÍCULO";
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    CargarImagen(articulo.ImagenUrl);
                    cboMarca.SelectedItem = articulo.Marca.Id;
                    cboCategoria.SelectedItem = articulo.Categoria.Id;
                    txtPrecio.Text = articulo.Precio.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void LimpiarTexto()         //Limpia los campos una vez se haya realizado una operación con éxito.
        {
            txtCodigo.Clear();
            txtDescripcion.Clear();
            txtImagenUrl.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            cboMarca.SelectedIndex = 0;
            cboCategoria.SelectedIndex = 0;
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 44 && e.KeyChar != 46)
            {
                SystemSounds.Exclamation.Play();
                e.Handled = true;
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            try
            {
                CargarImagen(txtImagenUrl.Text);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValidarCampos()            //Regresa "true" si no se ingresó Nombre o Precio.
        {
            lblNombre.ForeColor = Color.Black;
            lblPrecio.ForeColor = Color.Black;

            if (txtNombre.Text == "" || txtPrecio.Text == "")
            {
                if (txtNombre.Text == "")
                    lblNombre.ForeColor = Color.Red;
                if (txtPrecio.Text == "")
                    lblPrecio.ForeColor = Color.Red;
                SystemSounds.Exclamation.Play();
                return true;
            }

            return false;
        }
    }
}
