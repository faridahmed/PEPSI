//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyPepsi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReplaceDisburseDetail
    {
        public int WarehouseID { get; set; }
        public int CustomerID { get; set; }
        public int ReplaceDisburseID { get; set; }
        public int ProductID { get; set; }
        public Nullable<decimal> CaseQty { get; set; }
        public Nullable<decimal> BoxQty { get; set; }
        public Nullable<int> BatchNo { get; set; }
        public Nullable<System.DateTime> ManufactureDate { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> AlternateUnitPrice { get; set; }
        public Nullable<int> SchemeID { get; set; }
        public Nullable<decimal> TotAmount { get; set; }
    }
}
