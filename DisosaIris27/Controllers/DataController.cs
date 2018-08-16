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
        [Route("PostPreventa/{cliente}/{vendedor}")]
        public string PostPreventa(string cliente, int vendedor, [FromBody] ListaPreventas listaPreventas)
        {
            var preventa = new Preventa()
            {
                CodigoCliente = cliente.Trim(),
                VendedorId = vendedor,
                Fecha = DateTime.Today,
            };
            db.Preventas.Add(preventa);
            db.SaveChanges();

            var pedidos = listaPreventas.Listado; //array con productos
            foreach (PreventaDetalle pedido in pedidos)
            {
                var preventaDetalle = new PreventaDetalle()
                {
                    PreventaId = preventa.Id,
                    VendedorId = vendedor,
                    CodigoProducto = pedido.CodigoProducto,
                    Cantidad = pedido.Cantidad,
                    Precio = pedido.Precio,
                };
                var producto = db.Productos.Find(preventaDetalle.CodigoProducto);
                producto.Existencia = producto.Existencia - int.Parse(preventaDetalle.Cantidad.ToString());

                db.Entry(producto).State = System.Data.Entity.EntityState.Modified;
                db.PreventaDetalles.Add(preventaDetalle);
            }
            db.SaveChanges();
            return preventa.Id.ToString();
        }

        [HttpPost]
        [Route("PostCompra/{proveedor}/{comentario}/{fecha}/{credito}")]
        public string PostCompra(int proveedor, string comentario, DateTime fecha, bool credito , [FromBody] ListaCompras listaCompras)
        {
            var compraNueva = new Compra() { CodigoProveedor = proveedor, Comentario = comentario, Fecha = fecha, Credito = credito };
            db.Compras.Add(compraNueva);
            db.SaveChanges();
            var compras = listaCompras.Listado; //array con productos
            decimal total = 0;
            foreach (CompraDetalle compra in compras)
            {
                var compraDetalle = new CompraDetalle() { CompraId = compraNueva.Id, CodigoProducto = compra.CodigoProducto, Cantidad = compra.Cantidad, Costo = compra.Costo };
                total += compraDetalle.Costo.Value * compra.Cantidad.Value;
                var producto = db.Productos.Find(compraDetalle.CodigoProducto);
                producto.Existencia = producto.Existencia + int.Parse(compraDetalle.Cantidad.ToString());
                db.Entry(producto).State = System.Data.Entity.EntityState.Modified;
                db.CompraDetalles.Add(compraDetalle);
            }

            if (credito == true)
            {
                var _proveedor = db.Proveedors.Find(proveedor);
                _proveedor.Saldo = Convert.ToDecimal(_proveedor.Saldo) + total;
                db.Entry(_proveedor).State = System.Data.Entity.EntityState.Modified;
                
                var proveedorDetalle = new ProveedoresDetalle() { Fecha =fecha, ProveedorId=proveedor, Credito = total, Referencia = "Compra # " + compraNueva.Id };
                db.ProveedoresDetalles.Add(proveedorDetalle);
            }

            db.SaveChanges();
            return compraNueva.Id.ToString();
        }
    }

    public class ListaPreventas
    {
        public PreventaDetalle[] Listado { get; set; }
    }

    public class ListaCompras
    {
        public CompraDetalle[] Listado { get; set; }
    }
}
