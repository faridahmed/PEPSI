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
    
    public partial class spRPTPOSMReceiveIsuueDateWise_Result
    {
        public Nullable<int> GRChNo { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierName { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string PosmName { get; set; }
        public Nullable<int> SchemeID { get; set; }
        public string SchemeDescription { get; set; }
        public Nullable<int> ReceivedQty { get; set; }
        public Nullable<decimal> RcvRate { get; set; }
        public Nullable<int> IssuedQty { get; set; }
        public Nullable<decimal> IssuRate { get; set; }
        public Nullable<int> FreshProductReturn { get; set; }
        public Nullable<int> DefectiveProductReturn { get; set; }
        public Nullable<int> DestroyedProduct { get; set; }
        public string EnteredBy { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
    }
}