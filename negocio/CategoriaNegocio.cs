using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()
        {
            List<Categoria> listaCategorias = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            datos.setConsulta("Select Id, Descripcion from CATEGORIAS");
            datos.ejecutarLector();

            try
            {
                while (datos.Lector.Read())
                {
                    Categoria categ = new Categoria();
                    categ.Id = (int)datos.Lector["Id"];
                    categ.Descripcion = (string)datos.Lector["Descripcion"];

                    listaCategorias.Add(categ);
                }

                return listaCategorias;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
