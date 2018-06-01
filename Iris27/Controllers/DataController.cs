using Iris27.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Iris27.Controllers
{
    [RoutePrefix("api/data")]

    public class DataController : ApiController
    {
        private disosadbEntities db = new disosadbEntities();

        [HttpGet]
        [Route("ObtenerNombreCliente/{codigo}")]
        public string ObtenerNombreCliente(string codigo)
        {
            try
            {
                var cliente = db.Clientes.Find(codigo);
                if (cliente != null)
                {
                    return cliente.Nombre;
                }
                else
                {
                    return "No encontrado";
                }
            }
            catch (Exception)
            {
                return "No encontrado";
                throw;
            }
        }

        [HttpGet]
        [Route("ObtenerProducto/{codigo}")]
        public Producto ObtenerProducto(int codigo)
        {
            var producto = db.Productos.Find(codigo);
            producto.PreventaDetalles = null;
            producto.Proveedor = new Proveedor() { Codigo = producto.Proveedor.Codigo, Nombre = producto.Proveedor.Nombre, Productos=null };
            producto.VentaDetalles = null;
            return producto;
        }

        [HttpPost]
        [Route("GuardarPicking/{picking}")]
        public string ObtenerProducto(dynamic picking)
        {
            return "Hecho";
        }
    }
}
