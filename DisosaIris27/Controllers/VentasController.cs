using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DisosaIris27.Models;
using PagedList;

namespace DisosaIris27.Controllers
{
    public class VentasController : Controller
    {
        private disosadbEntities db = new disosadbEntities();

        // GET: Ventas
        public ActionResult Index(int? page)
        {
            if (!(int.Parse(Session["nivel"].ToString()) > 1)) { return RedirectToAction("Index", "Login"); }
            var ventas = db.Ventas.Include(v => v.Cliente).Include(v => v.Vendedor);
            ventas = ventas.OrderByDescending(v => v.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1); //if null... set 1
            return View(ventas.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Preventa()
        {
            ViewBag.Rutas = db.Rutas.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult GenerarVenta(int Id) => RedirectToAction("Create", new { id = Id });

        // GET: Ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: Ventas/Create
        public ActionResult Create(string rutas)
        {
            var listaRutas = rutas.Split(',');
            var preventas = new List<Preventa>();
            for (int i = 0; i < listaRutas.Length - 1; i++)
            {
                var rt = int.Parse(listaRutas[i]);
                preventas.AddRange(db.Preventas.Where(p => p.Cliente.Ruta == rt).ToList());
            }
            decimal? grantotal = 0;
            foreach (var preventa in preventas)
            {
                foreach (var item in preventa.PreventaDetalles)
                {
                    grantotal = grantotal + (item.Cantidad * item.Precio);
                }
            }
            preventas = preventas.OrderBy(p => p.Id).ToList();
            ViewBag.GranTotal = grantotal;
            return View(preventas);
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string deposito, string clientes, string vendedores, string devoluciones, string pedidos)
        {
            var listaPedidos = pedidos.Split(',');
            var listaClientes = clientes.Split(',');
            var listaVendedores = vendedores.Split(',');

            var ventas = new List<Venta>();
            for (int i = 0; i < listaPedidos.Length - 1; i++)
            {
                var venta = new Venta()
                {
                    CodigoCliente = listaClientes[i],
                    Fecha = DateTime.Now,
                    Deposito = deposito,
                    VendedorId = int.Parse(listaVendedores[i])
                };

                db.Ventas.Add(venta);
                db.SaveChanges();

                ventas.Add(venta);
            }

            var listaDevoluciones = devoluciones.Split(',');
            var dIndex = 0; //un índice para las devoluciones
            var pIndex = 0; //un índice para los pedidos
            foreach (var venta in ventas)
            {
                Preventa preventa = db.Preventas.Find(int.Parse(listaPedidos[pIndex]));
                foreach (var item in preventa.PreventaDetalles)
                {
                    var devolucion = int.Parse(listaDevoluciones[dIndex]);
                    var cantidad = item.Cantidad - devolucion;
                    var utitlidad = (item.Precio - item.Producto.Costo) * cantidad;
                    var detalle = new VentaDetalle()
                    {
                        VentaId = venta.Id,
                        Cantidad = cantidad,
                        CodigoProducto = item.CodigoProducto,
                        Precio = item.Precio,
                        Utilidad = utitlidad,
                    };
                    db.VentaDetalles.Add(detalle);                    

                    if (devolucion > 0)
                    {
                        var producto = item.Producto;
                        producto.Existencia = producto.Existencia + devolucion;
                        db.Entry(producto).State = EntityState.Modified;
                    }

                    dIndex++;
                }
                db.Preventas.Remove(preventa); // Eliminar Preventa
                pIndex++;
            }
            db.SaveChanges();
            return RedirectToAction("Index");

            //ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre", venta.CodigoCliente);
            //ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre", venta.VendedorId);
            //return View();
            //return View(venta);
        }

        // GET: Ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre", venta.CodigoCliente);
            ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre", venta.VendedorId);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,Deposito,VendedorId,CodigoCliente")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre", venta.CodigoCliente);
            ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre", venta.VendedorId);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venta venta = db.Ventas.Find(id);
            db.Ventas.Remove(venta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
