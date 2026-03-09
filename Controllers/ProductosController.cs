using Elfie.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VeterinariaWeb.Models;

namespace VeterinariaWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly string cadenaconexion = "Server=localhost\\SQLEXPRESS;Database=veterinaria;user id=sa;password=sqladmin;TrustServerCertificate=true";
        //clean code
        public IActionResult Index()
        {
            var listaProductos = obtenerProductos();
            return View(listaProductos);


        }

        public IActionResult Detail(int id)
        {
            var productoBuscado = obtenerProductoPorId(id);
            return View(productoBuscado);
        }

        #region .Private methods .
        private List<Producto> obtenerProductos()
        {
            var listaProductos = new List<Producto>();
            //todo : leer datos de la BD
            using (var conexion = new SqlConnection(cadenaconexion))
            {
                using (var comando = new SqlCommand("Select * from Productos", conexion))
                {
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector != null && lector.HasRows)
                        {
                            while (lector.Read())
                            {
                                listaProductos.Add(new Producto
                                {
                                    ID = lector.GetInt32(0),
                                    Nombre = lector.GetString(1),
                                    Descripcion = lector.GetString(2)
                                });
                            }
                        }
                    }
                }
                    

            }
                return listaProductos;
        }
        private Producto obtenerProductoPorId(int id)
        {
            var producto = new Producto();
            using (var conexion = new SqlConnection(cadenaconexion))
            {

                using (var comando = new SqlCommand("Select * FROM Productos WHERE ID = @ID",conexion))
                {
                    comando.Parameters.AddWithValue("@ID", id);
                    conexion.Open();
                    using (var lector = comando.ExecuteReader())
                    {
                        if(lector !=null && lector.HasRows)
                        {
                            lector.Read();
                            producto = new Producto()
                            {
                                ID = lector.GetInt32(0),
                                Nombre = lector.GetString(1),
                                Descripcion = lector.GetString(2)
                            };
                        }
                    }
                }

            }
                return producto;
        }
        #endregion
    }
}
