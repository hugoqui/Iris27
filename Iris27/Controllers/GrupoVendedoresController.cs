using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Iris27.Models;

namespace Iris27.Controllers
{
    public class GrupoVendedoresController : Controller
    {
        private disosadbEntities db = new disosadbEntities();

        // GET: GrupoVendedores
        public ActionResult Index()
        {
            return View(db.GrupoVendedors.ToList());
        }

        // GET: GrupoVendedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrupoVendedor grupoVendedor = db.GrupoVendedors.Find(id);
            if (grupoVendedor == null)
            {
                return HttpNotFound();
            }
            return View(grupoVendedor);
        }

        // GET: GrupoVendedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrupoVendedores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] GrupoVendedor grupoVendedor)
        {
            if (ModelState.IsValid)
            {
                db.GrupoVendedors.Add(grupoVendedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grupoVendedor);
        }

        // GET: GrupoVendedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrupoVendedor grupoVendedor = db.GrupoVendedors.Find(id);
            if (grupoVendedor == null)
            {
                return HttpNotFound();
            }
            return View(grupoVendedor);
        }

        // POST: GrupoVendedores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] GrupoVendedor grupoVendedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupoVendedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grupoVendedor);
        }

        // GET: GrupoVendedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrupoVendedor grupoVendedor = db.GrupoVendedors.Find(id);
            if (grupoVendedor == null)
            {
                return HttpNotFound();
            }
            return View(grupoVendedor);
        }

        // POST: GrupoVendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GrupoVendedor grupoVendedor = db.GrupoVendedors.Find(id);
            db.GrupoVendedors.Remove(grupoVendedor);
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
