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
    
    public partial class Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producto()
        {
            this.PreventaDetalles = new HashSet<PreventaDetalle>();
            this.VentaDetalles = new HashSet<VentaDetalle>();
            this.CompraDetalles = new HashSet<CompraDetalle>();
        }
    
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public int Existencia { get; set; }
        public Nullable<int> ProveedorId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreventaDetalle> PreventaDetalles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompraDetalle> CompraDetalles { get; set; }
        public virtual Proveedor Proveedor { get; set; }
    }
}
