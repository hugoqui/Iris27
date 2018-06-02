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
    public class PreventasController : Controller
    {
        private disosadbEntities db = new disosadbEntities();

        // GET: Preventas
        public ActionResult Index(int? page)
        {
            var preventas = db.Preventas.Include(p => p.Cliente).Include(p => p.Vendedor);
            preventas = preventas.OrderByDescending(p => p.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1); //if null... set 1
            return View(preventas.ToPagedList(pageNumber, pageSize));
        }

        // GET: Preventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preventa preventa = db.Preventas.Find(id);
            if (preventa == null)
            {
                return HttpNotFound();
            }
            return View(preventa);
        }

        // GET: Preventas/Create
        public ActionResult Create()
        {
            ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre");
            ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre");
            return View();
        }

        // POST: Preventas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,CodigoCliente,VendedorId")] Preventa preventa)
        {
            if (ModelState.IsValid)
            {
                db.Preventas.Add(preventa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre", preventa.CodigoCliente);
            ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre", preventa.VendedorId);
            return View(preventa);
        }

        // GET: Preventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preventa preventa = db.Preventas.Find(id);
            if (preventa == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre", preventa.CodigoCliente);
            ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre", preventa.VendedorId);
            return View(preventa);
        }

        // POST: Preventas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,CodigoCliente,VendedorId")] Preventa preventa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preventa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodigoCliente = new SelectList(db.Clientes, "Codigo", "Nombre", preventa.CodigoCliente);
            ViewBag.VendedorId = new SelectList(db.Vendedors, "Id", "Nombre", preventa.VendedorId);
            return View(preventa);
        }

        // GET: Preventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preventa preventa = db.Preventas.Find(id);
            if (preventa == null)
            {
                return HttpNotFound();
            }
            return View(preventa);
        }

        // POST: Preventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Preventa preventa = db.Preventas.Find(id);
            db.Preventas.Remove(preventa);
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
