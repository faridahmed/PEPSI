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
    
    public partial class spRPTOrderFromCustomer_Result
    {
        public Nullable<int> WarehouseID { get; set; }
        public string WarehouseDescription { get; set; }
        public int SalesOrderNO { get; set; }
        public System.DateTime SalesOrderDate { get; set; }
        public string RefOrderNO { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerPhone { get; set; }
        public int ProductID { get; set; }
        public string ProductDescription { get; set; }
        public int ConversionFactor { get; set; }
        public decimal SecurityDeposit { get; set; }
        public decimal AlternateSecurityDeposit { get; set; }
        public int BottleReturnable { get; set; }
        public decimal PlasticBoxSecurity { get; set; }
        public Nullable<decimal> OzConversion { get; set; }
        public Nullable<int> Expirydays { get; set; }
        public Nullable<bool> ShowInList { get; set; }
        public Nullable<decimal> OrderCase { get; set; }
        public Nullable<decimal> PlasticBox { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
    }
}
