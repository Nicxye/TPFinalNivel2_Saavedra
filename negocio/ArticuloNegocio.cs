using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public void AgregarArticulo(Articulo nuevoArticulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio)");
                datos.SetParametros("@Codigo", nuevoArticulo.Codigo);
                datos.SetParametros("@Nombre", nuevoArticulo.Nombre);
                datos.SetParametros("@Descripcion", nuevoArticulo.Descripcion);
                datos.SetParametros("@IdMarca", nuevoArticulo.Marca.Id);
                datos.SetParametros("@IdCategoria", nuevoArticulo.Categoria.Id);
                datos.SetParametros("@ImagenUrl", nuevoArticulo.ImagenUrl);
                datos.SetParametros("@Precio", nuevoArticulo.Precio);
                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarArticulo(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetConsulta("Delete from ARTICULOS where @Id = Id");
                datos.SetParametros("@Id", articulo.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Articulo> FiltrarArticulos(string campo, string criterio, string filtro)        //Para filtro avanzado.
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, M.Descripcion as Marca, C.Descripcion as Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdMarca = M.Id and A.IdCategoria = C.Id and ";

                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Nombre exacto":
                            consulta += "Nombre = '" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Nombre exacto":
                            consulta += "M.Descripcion = '" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Categoría")
                {
                    switch (criterio)
                    {
                        case "Nombre exacto":
                            consulta += "C.Descripcion = '" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Menos de":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio > " + filtro;
                            break;
                    }
                }

                datos.SetConsulta(consulta);
                datos.EjecutarLector();

                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();

                    articulo.Id = (int)datos.Lector["Id"];
                    articulo.Codigo = (string)datos.Lector["Codigo"];
                    articulo.Nombre = (string)datos.Lector["Nombre"];
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];
                    articulo.Precio = Math.Round((decimal)datos.Lector["Precio"], 3);

                    articulo.Marca = new Marca();
                    articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    articulo.Marca.Descripcion = (string)datos.Lector["Marca"];

                    articulo.Categoria = new Categoria();
                    articulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    articulo.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        articulo.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    lista.Add(articulo);
                }

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Articulo> FiltrarArticulos(string filtro, string filtroSecundario)      //Exclusivo para filtro con campo "Precio" y criterio "Entre"
            {
                List<Articulo> lista = new List<Articulo>();
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    string consulta = "Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, M.Descripcion as Marca, C.Descripcion as Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdMarca = M.Id and A.IdCategoria = C.Id and Precio > " + filtro + " and Precio < " + filtroSecundario;

                    datos.SetConsulta(consulta);
                    datos.EjecutarLector();

                    while (datos.Lector.Read())
                    {
                        Articulo articulo = new Articulo();

                        articulo.Id = (int)datos.Lector["Id"];
                        articulo.Codigo = (string)datos.Lector["Codigo"];
                        articulo.Nombre = (string)datos.Lector["Nombre"];
                        articulo.Descripcion = (string)datos.Lector["Descripcion"];
                        articulo.Precio = Math.Round((decimal)datos.Lector["Precio"], 3);

                        articulo.Marca = new Marca();
                        articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                        articulo.Marca.Descripcion = (string)datos.Lector["Marca"];

                        articulo.Categoria = new Categoria();
                        articulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                        articulo.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                        if (!(datos.Lector["ImagenUrl"] is DBNull))
                            articulo.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                        lista.Add(articulo);
                    }

                    return lista;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    datos.CerrarConexion();
                }
            }

        public List<Articulo> Listar()
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetConsulta("Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, C.Descripcion as Categoria, M.Descripcion as Marca \r\nfrom ARTICULOS A, MARCAS M, CATEGORIAS C \r\nwhere IdMarca = M.Id and IdCategoria = C.Id");
                datos.EjecutarLector();

                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();

                    articulo.Id = (int)datos.Lector["Id"];
                    articulo.Codigo = (string)datos.Lector["Codigo"];
                    articulo.Nombre = (string)datos.Lector["Nombre"];
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];
                    articulo.Precio = Math.Round((decimal)datos.Lector["Precio"], 3);

                    articulo.Marca = new Marca();
                    articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    articulo.Marca.Descripcion = (string)datos.Lector["Marca"];

                    articulo.Categoria = new Categoria();
                    articulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    articulo.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        articulo.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    listaArticulos.Add(articulo);
                }

                return listaArticulos;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ModificarArticulo(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetConsulta("Update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @Precio where Id = @Id");
                datos.SetParametros("@Codigo", articulo.Codigo);
                datos.SetParametros("@Nombre", articulo.Nombre);
                datos.SetParametros("@Descripcion", articulo.Descripcion);
                datos.SetParametros("@IdMarca", articulo.Marca.Id);
                datos.SetParametros("@IdCategoria", articulo.Categoria.Id);
                datos.SetParametros("@ImagenUrl", articulo.ImagenUrl);
                datos.SetParametros("@Precio", articulo.Precio);
                datos.SetParametros("@Id", articulo.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
