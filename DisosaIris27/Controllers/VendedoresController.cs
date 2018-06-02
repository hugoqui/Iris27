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
    public class VendedoresController : Controller
    {
        private disosadbEntities db = new disosadbEntities();

        // GET: Vendedores
        public ActionResult Index()
        {
            var vendedors = db.Vendedors.Include(v => v.GrupoVendedor);
            return View(vendedors.ToList());
        }

        // GET: Vendedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedors.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // GET: Vendedores/Create
        public ActionResult Create()
        {
            ViewBag.Grupo = new SelectList(db.GrupoVendedors, "Id", "Nombre");
            return View();
        }

        // POST: Vendedores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Grupo")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Vendedors.Add(vendedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Grupo = new SelectList(db.GrupoVendedors, "Id", "Nombre", vendedor.Grupo);
            return View(vendedor);
        }

        // GET: Vendedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedors.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Grupo = new SelectList(db.GrupoVendedors, "Id", "Nombre", vendedor.Grupo);
            return View(vendedor);
        }

        // POST: Vendedores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Grupo")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Grupo = new SelectList(db.GrupoVendedors, "Id", "Nombre", vendedor.Grupo);
            return View(vendedor);
        }

        // GET: Vendedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedors.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendedor vendedor = db.Vendedors.Find(id);
            db.Vendedors.Remove(vendedor);
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
