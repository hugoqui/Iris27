using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DisosaIris27.Models;

namespace DisosaIris27.Controllers
{
    public class CatalogoController : ApiController
    {
        private disosadbEntities db = new disosadbEntities();

        // GET: api/Catalogo
        public List<Catalogo> GetProductos()
        {
            var catalogo = new List<Catalogo>();
            foreach (var item in db.Productos)
            {
                var c = new Catalogo() { Codigo = item.Codigo, Nombre = item.Nombre };
                catalogo.Add(c);
            }
            return catalogo;
        }

    }
}