using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DisosaIris27.Models;

namespace DisosaIris27.Controllers
{
    public class GastosController : Controller
    {
        private disosadbEntities db = new disosadbEntities();

        // GET: Gastos
        public ActionResult Index()
        {
            if (!(int.Parse(Session["nivel"].ToString()) > 2)) { return RedirectToAction("Index", "Login"); }
            return View(db.Gastos.ToList());
        }

        // GET: Gastos/Details/5
        public ActionResult Details(int? id, DateTime? fechaInicial, DateTime? fechaFinal)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gastos.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }

            if (fechaInicial == null) { fechaInicial = DateTime.Now; }
            if (fechaFinal == null) { fechaFinal = DateTime.Now; }

            ViewBag.fechaInicial = fechaInicial;
            ViewBag.fechaFinal = fechaFinal;

            var detalles = gasto.DetalleGastos
                .Where(d => d.Fecha.Value >= new DateTime(fechaInicial.Value.Year, fechaInicial.Value.Month, fechaInicial.Value.Day)).ToList();

            detalles = detalles
                .Where(d => d.Fecha.Value <= new DateTime(fechaFinal.Value.Year, fechaFinal.Value.Month, fechaFinal.Value.Day)).ToList();

            //var detalles = (from d in gasto.DetalleGastos
            //                where ( d.Fecha <= fechaFinal) && (d.Fecha >= fechaInicial)
            //                select d).ToList();

            gasto.DetalleGastos = detalles;
            return View(gasto);
        }

        // GET: Gastos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gastos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Nombre")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                db.Gastos.Add(gasto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gasto);
        }

        // GET: Gastos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gastos.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            return View(gasto);
        }

        // POST: Gastos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Nombre")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gasto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gasto);
        }

        // GET: Gastos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gastos.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gasto gasto = db.Gastos.Find(id);
            db.Gastos.Remove(gasto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Registrar(int? success)
        {
            if (!(int.Parse(Session["nivel"].ToString()) > 1)) { return RedirectToAction("Index", "Login"); }
            ViewBag.Message = "";
            if (success != null) { ViewBag.Message = "El gasto fue registrado exitosamente."; }

            ViewBag.Gastos = db.Gastos.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(DateTime fecha, int gastoId, string comentario, decimal monto)
        {
            var detalle = new DetalleGasto()
            {
                Fecha = fecha,
                Comentario = comentario,
                GastoId = gastoId,
                Monto = monto
            };
            db.DetalleGastos.Add(detalle);
            db.SaveChanges();
            return RedirectToAction("Registrar", new { success = 1 });
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
