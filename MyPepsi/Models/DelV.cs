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
    
    public partial class DelV
    {
        public Nullable<int> SalesOrderNo { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> AlternateQuantity { get; set; }
        public Nullable<int> PlasticBoxQuantity { get; set; }
        public string Delivered { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> ChallanNumber { get; set; }
        public Nullable<System.DateTime> DeliveryChallanDate { get; set; }
        public int WarehouseID { get; set; }
        public Nullable<int> RebateQuantity { get; set; }
        public Nullable<decimal> AlternateRebateQuantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> AlternateUnitPrice { get; set; }
        public Nullable<decimal> AgencyCommission { get; set; }
        public Nullable<decimal> AlternateAgencyCommission { get; set; }
    }
}
