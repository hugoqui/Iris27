using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisosaIris27.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        static List<string> miLista = new List<string>();
        public ActionResult Hugo( string nombre)
        {
            miLista.Add(nombre);
            if (nombre == "")
            {
                miLista = new List<string>();
            }
            return View(miLista);
        }
    }
}