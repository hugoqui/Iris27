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
    
    public partial class CompraDetalle
    {
        public int Id { get; set; }
        public Nullable<int> CodigoProducto { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public Nullable<int> CompraId { get; set; }
        public Nullable<decimal> Costo { get; set; }
    
        public virtual Compra Compra { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
