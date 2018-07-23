using DisosaIris27.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisosaIris27.Controllers
{
    public class PickingController : Controller
    {
        // GET: Picking
        private disosadbEntities db = new disosadbEntities();
        public ActionResult Index(int? ruta)
        {
            List<Preventa> preventas = new List<Preventa>();
            if (ruta != null)
                preventas = db.Preventas.Where(p => p.Cliente.Ruta == ruta).ToList();

            ViewBag.Rutas = db.Rutas.ToList();
            ViewBag.NombreRuta = "";
            ViewBag.Ruta = ruta;
            if (ruta != null)
                ViewBag.NombreRuta = db.Rutas.Find(ruta).Nombre;

            return View(preventas);
        }

        public ActionResult Details(int ruta)
        {
            List<Preventa> pedidos = new List<Preventa>();
            pedidos = db.Preventas.Where(p => p.Cliente.Ruta == ruta).ToList();
            var lista = new List<PreventaDetalle>();
            foreach (var pedido in pedidos)
            {
                foreach (var producto in pedido.PreventaDetalles)
                {
                    lista.Add(producto);
                }
            }

            var listaFinal = lista.GroupBy(p => p.CodigoProducto)
                        .Select(group => new
                        {
                            CodigoProducto = group.Key,
                            Cantidad = group.Sum(p => p.Cantidad),
                            Nombre = ObtenerNombreProducto(group.Key),
                            Total = group.Sum(p => p.Cantidad * p.Precio)
                        })
                        .OrderBy(x => x.CodigoProducto);

            ViewBag.Total = 0;
            var ls = new List<PreventaDetalle>();

            foreach (var line in listaFinal)
            {
                var p = new PreventaDetalle()
                {
                    CodigoProducto = line.CodigoProducto,
                    Cantidad = line.Cantidad,
                    Producto = new Producto() { Codigo = line.CodigoProducto.Value, Nombre = line.Nombre },
                    Precio = line.Total,
                };
                ViewBag.Total += line.Total;
                ls.Add(p);
            }

            ViewBag.NombreRuta = "";
            ViewBag.Ruta = ruta;
            ViewBag.NombreRuta = db.Rutas.Find(ruta).Nombre;

            return View(ls);
        }

        public string ObtenerNombreProducto(int? codigo)
        {
            return db.Productos.Find(codigo).Nombre;
        }
    }
}