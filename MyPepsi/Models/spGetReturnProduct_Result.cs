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
    
    public partial class spGetReturnProduct_Result
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress1 { get; set; }
        public int ProductRateID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AlternateUnitPrice { get; set; }
        public decimal AgencyCommission { get; set; }
        public decimal AlternateAgencyCommission { get; set; }
        public decimal SecurityDeposit { get; set; }
        public decimal AlternateSecurityDeposit { get; set; }
        public decimal PlasticBoxSecurity { get; set; }
        public Nullable<decimal> MRPRate { get; set; }
        public int ReturnmentID { get; set; }
        public int WarehouseID { get; set; }
        public Nullable<int> ReturnGoodsQty { get; set; }
        public Nullable<decimal> InAmount { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> ReturnQty { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Packsize { get; set; }
        public Nullable<int> BatchNo { get; set; }
        public string ProductDescription { get; set; }
    }
}
