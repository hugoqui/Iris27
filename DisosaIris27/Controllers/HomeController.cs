using System.Data;
using System.Data.Entity;
using System.Linq;
using DisosaIris27.Models;
using System.Web.Mvc;
using System.Collections.Generic;

namespace DisosaIris27.Controllers
{
    public class HomeController : Controller
    {
        private disosadbEntities db = new disosadbEntities();
        // GET: Home
        public ActionResult Index()
        {
            if (!(int.Parse(Session["nivel"].ToString()) > 0)) { return RedirectToAction("Index", "Login"); }
            return View();
        }


        public ActionResult Hugo()
        {
            var productos = db.Productos.ToList();
            var lista = new List<AutoComplete>();
            productos.ForEach(p => lista.Add(new AutoComplete() { label = p.Nombre, value = p.Codigo.ToString() }));
            ViewBag.productos = lista;
            return View();
        }
    }
}