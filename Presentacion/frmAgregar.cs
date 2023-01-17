﻿using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            try
            {
                cargarImagen(txtImagenUrl.Text);
            }
            catch (Exception)
            {

                throw;
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (articulo.Id != 0)
                {
                    negocio.modificarArticulo(articulo);
                    MessageBox.Show(articulo.Nombre + " modificado exitosamente");
                }
                else
                {
                    negocio.agregarArticulo(articulo);
                    limpiarTexto();
                    MessageBox.Show("Agregado exitosamente.");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
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

        private void limpiarTexto()
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
            {
                if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46)
                {
                    SystemSounds.Exclamation.Play();
                    e.Handled = true;
                }

            }
        }
    }
}
