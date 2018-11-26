//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bonheur.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Purchase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Purchase()
        {
            this.PurchaseItems = new HashSet<PurchaseItem>();
        }
    
        public int PurchaseID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int SupplierID { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsFinalize { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}