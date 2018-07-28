using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisosaIris27.Models
{
    public class VentaPorDia
    {
        public DateTime Fecha { get; set; }
        public string Vendedor { get; set; }
        public decimal? Total { get; set; }
    }
}