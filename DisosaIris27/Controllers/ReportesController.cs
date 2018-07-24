using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisosaIris27.Models;
using PagedList;

namespace DisosaIris27.Controllers
{
    public class ReportesController : Controller
    {
        private disosadbEntities db = new disosadbEntities();
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ventas(string cliente, int? vendedor, DateTime? fechaInicio, DateTime? fechaFinal, int? page)
        {
            ViewBag.NombreCliente = " * todos";
            ViewBag.NombreVendedor = " * todos";
            var ventas = new List<Venta>();

            if (fechaInicio == null) { ViewBag.FechaInicio = DateTime.Now.ToString("yyyy-MM-dd"); }
            else { ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd"); }

            if (fechaFinal == null) { ViewBag.FechaFinal = DateTime.Now.ToString("yyyy-MM-dd"); }
            else { ViewBag.FechaFinal = fechaFinal.Value.ToString("yyyy-MM-dd"); }

            if (cliente != null && vendedor!=null && fechaInicio != null && fechaFinal != null)
            {
                ventas = db.Ventas.Where(v => (v.Fecha >= fechaInicio && v.Fecha <= fechaFinal)).ToList();
                if (cliente != "*") // 0 significa que son todos los clientes
                {
                    ViewBag.NombreCliente = db.Clientes.Find(cliente.Trim()).Nombre ;
                    
                    ventas = ventas.Where(v => v.CodigoCliente == cliente).ToList();
                }
                if (vendedor > 0)
                {
                    ViewBag.NombreVendedor = db.Vendedors.Find(vendedor).Nombre;
                    ventas = ventas.Where(v => v.VendedorId == vendedor).ToList();
                }
            }
            
            ViewBag.Cliente = cliente;
            ViewBag.Clientes = db.Clientes.ToList();
            ViewBag.Vendedor = vendedor;
            ViewBag.Vendedores = db.Vendedors.ToList();
            ViewBag.Conteo = ventas.Count;

            ViewBag.Total = ventas.Sum(v => (v.VentaDetalles.Sum(d => d.Precio * d.Cantidad)));

            int pageSize = 20;
            int pageNumber = (page ?? 1); //if null... set 1
            ventas = ventas.OrderByDescending(v => v.Id).ToList();
            return View(ventas.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Compras(int? proveedor, DateTime? fechaInicio, DateTime? fechaFinal, int? page)
        {
            var compras = new List<Compra>();

            if (fechaInicio == null) { ViewBag.FechaInicio = DateTime.Now.ToString("yyyy-MM-dd"); }
            else { ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd"); }

            if (fechaFinal == null) { ViewBag.FechaFinal = DateTime.Now.ToString("yyyy-MM-dd"); }
            else { ViewBag.FechaFinal = fechaFinal.Value.ToString("yyyy-MM-dd"); }

            ViewBag.Proveedor = proveedor;

            if (proveedor != null && fechaInicio != null && fechaFinal != null)
            {
                compras = db.Compras.Where(v => (v.Fecha >= fechaInicio && v.Fecha <= fechaFinal)).ToList();
                if (proveedor != 0) // 0 significa que son todos los clientes
                {
                    compras = compras.Where(c => c.CodigoProveedor == proveedor).ToList();
                }
            }
            ViewBag.Proveedores = db.Proveedors.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1); //if null... set 1
            compras = compras.OrderByDescending(c => c.Id).ToList();
            return View(compras.ToPagedList(pageNumber, pageSize));
        }


    }
}