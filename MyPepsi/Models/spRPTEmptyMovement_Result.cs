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
    
    public partial class spRPTEmptyMovement_Result
    {
        public int WarehouseID { get; set; }
        public int TransactionNo { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<decimal> TotalEmpty { get; set; }
        public Nullable<decimal> TotalBox { get; set; }
        public Nullable<int> ToWarehouse { get; set; }
        public Nullable<int> DriverID { get; set; }
        public string VehicleNo { get; set; }
        public string BinNo { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<decimal> QuantityCase { get; set; }
        public Nullable<decimal> Boxes { get; set; }
        public string WarehouseDesc { get; set; }
        public string ToWarehouseDesc { get; set; }
        public string DriverName { get; set; }
        public Nullable<int> TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
        public int ProductID { get; set; }
        public string ProductDescription { get; set; }
    }
}
