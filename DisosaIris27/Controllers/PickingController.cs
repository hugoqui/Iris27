using DisosaIris27.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DisosaIris27.Controllers
{
    public class PickingController : Controller
    {
        // GET: Picking
        private disosadbEntities db = new disosadbEntities();
        public ActionResult Index(string rutas)
        {
            if (!(int.Parse(Session["nivel"].ToString()) > 0)) { return RedirectToAction("Index", "Login"); }
            var preventas = new List<Preventa>(); ;
            var nombreRutas = new List<string>();

            if (rutas != null && rutas != "")
            {
                var listaRutas = rutas.Split(',');
                for (int i = 0; i < listaRutas.Length - 1; i++)
                {
                    var rt = int.Parse(listaRutas[i]);
                    preventas.AddRange(db.Preventas.Where(p => p.Cliente.Ruta == rt).ToList());
                    nombreRutas.Add(db.Rutas.Find(rt).Nombre);
                }
            }

            ViewBag.ListaRutas = rutas;
            ViewBag.Rutas = db.Rutas.ToList();
            ViewBag.NombreRutas = nombreRutas;
            preventas = preventas.OrderBy(p => p.Id).ToList();
            return View(preventas);
        }

        public ActionResult Details(string rutas)
        {
            List<Preventa> pedidos = new List<Preventa>();
            var nombreRutas = new List<string>();
            var listaRutas = rutas.Split(',');
            for (int i = 0; i < listaRutas.Length - 1; i++)
            {
                var rt = int.Parse(listaRutas[i]);
                nombreRutas.Add(db.Rutas.Find(rt).Nombre);
                pedidos.AddRange(db.Preventas.Where(p => p.Cliente.Ruta == rt).ToList());
            }
                        
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
            
            ViewBag.Rutas = rutas;
            ViewBag.NombreRutas = nombreRutas;

            return View(ls);
        }

        public string ObtenerNombreProducto(int? codigo)
        {
            return db.Productos.Find(codigo).Nombre;
        }
    }
}