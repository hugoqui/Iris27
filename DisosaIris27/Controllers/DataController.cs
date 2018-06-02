using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DisosaIris27.Models;
using PagedList;

namespace DisosaIris27.Controllers
{
    [RoutePrefix("api/data")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

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
            var _producto = db.Productos.Find(codigo);
            var producto = new Producto()
            {
                Codigo = _producto.Codigo,
                Costo = _producto.Costo,
                Existencia = _producto.Existencia,
                Nombre = _producto.Nombre,
                Precio = _producto.Precio,
                ProveedorId = _producto.ProveedorId,
                Proveedor = new Proveedor() { Codigo = _producto.Proveedor.Codigo, Nombre = _producto.Proveedor.Nombre }
            };
            return producto;
        }

        [HttpPost]
        [Route("PostPreventa/{data}")]
        public string PostPreventa(string data)
        {
            var lines = data.Split('!');
            var clientId = lines[0].Split(',')[0];
            var vendedorId = lines[0].Split(',')[1];

            var preventa = new Preventa()
            {
                CodigoCliente = clientId,
                VendedorId = int.Parse(vendedorId),
                Fecha = DateTime.Today,
            };

            db.Preventas.Add(preventa);
            db.SaveChanges();

            var preventaDetalles = new List<PreventaDetalle>();
            for (int i = 1; i < lines.Length - 1; i++)
            {
                var preventaDetalle = new PreventaDetalle()
                {
                    PreventaId = preventa.Id,
                    VendedorId = int.Parse(vendedorId),
                    CodigoProducto = int.Parse(lines[i].Split(',')[0]),
                    Cantidad = int.Parse(lines[i].Split(',')[1]),
                    Precio = Convert.ToDecimal(lines[i].Split(',')[2]),
                };

                var producto = db.Productos.Find(preventaDetalle.CodigoProducto);
                producto.Existencia = producto.Existencia - int.Parse(preventaDetalle.Cantidad.ToString());
                db.Entry(producto).State = System.Data.Entity.EntityState.Modified;
                db.PreventaDetalles.Add(preventaDetalle);
            }

            db.SaveChanges();
            return preventa.Id.ToString();
        }
    }

}
