using DisosaIris27.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisosaIris27.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private disosadbEntities db = new disosadbEntities();
        public ActionResult Index()
        {
            try
            {
                if (int.Parse(Session["nivel"].ToString()) < 3 && int.Parse(Session["nivel"].ToString()) > 0)
                {
                    ViewBag.Error = "No tiene permisos suficientes para esta acción. Inicie sesión como administrador.";
                }
            }
            catch (Exception)
            {
                
            }
            //Session["nivel"] = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario datos)
        {
            var usuario = db.Usuarios.Where(d => d.Usuario1.Trim() == datos.Usuario1.Trim()).ToList();
            if (usuario.Count == 0)
            {
                ViewBag.Error = "Error al ingresar los datos, por favor vuelta a intentarlo.";
                return View();
            }
            else
            {
                if (usuario[0].Password.Trim() == datos.Password.Trim())
                {
                    Session["usuario"] = usuario[0].Usuario1;
                    Session["nivel"] = usuario[0].Nivel;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Error al ingresar los datos, por favor vuelta a intentarlo.";
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session["nivel"] = 0;
            Session["usuario"] = "";
            return Index();
        }
    }
}