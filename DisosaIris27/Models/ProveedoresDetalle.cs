//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DisosaIris27.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProveedoresDetalle
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public Nullable<decimal> Abono { get; set; }
        public Nullable<decimal> Credito { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Referencia { get; set; }
    
        public virtual Proveedor Proveedor { get; set; }
    }
}
