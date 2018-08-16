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

        
        public ActionResult Hugo(string rmail, string rcompany, string raddress, string rpais, string rtel, string rhash, string ridpalpay, string rpriority, string idproduct)
        {
            var a = raddress;
            var b = rpais;

            return View();
        }
    }
}