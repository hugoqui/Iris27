using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
            var ventas = db.Ventas.Include(v => v.Cliente).Include(v => v.Vendedor);
            ventas = ventas.OrderByDescending(v => v.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1); //if null... set 1
            return View(ventas.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Preventa()
        {
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
        public ActionResult Create(int? id)
        {
            Preventa preventa = new Preventa();
            if (id != null)
            {
                preventa = db.Preventas.Find(id);
            }
            decimal? grantotal = 0;
            foreach (var item in preventa.PreventaDetalles)
            {
                grantotal = grantotal + (item.Cantidad * item.Precio);
            }
            ViewBag.GranTotal = grantotal;

            return View(preventa);
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,Deposito,VendedorId,CodigoCliente")] Venta venta, string devoluciones)
        {
            int preventaId = int.Parse((TempData["Picking"]).ToString());
            var listaDevoluciones = devoluciones.Split(',');

            if (ModelState.IsValid)
            {
                db.Ventas.Add(venta);
                db.SaveChanges();

                Preventa preventa = db.Preventas.Find(preventaId);

                var n = 0;
                foreach (var item in preventa.PreventaDetalles)
                {                    
                    var devolucion = int.Parse(listaDevoluciones[n]);
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
                    n++;

                    if (devolucion>0)
                    {
                        var producto = item.Producto;
                        producto.Existencia = producto.Existencia + devolucion;
                        db.Entry(producto).State = EntityState.Modified;
                    }
                }

                db.Preventas.Remove(preventa); // Eliminar Preventa
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre", venta.CodigoCliente);
            ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre", venta.VendedorId);
            return View(venta);
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
