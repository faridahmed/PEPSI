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
    
    public partial class spRPTProductTransaction_WarehouseWise_Result
    {
        public int TransactionNo { get; set; }
        public int WarehouseID { get; set; }
        public int TransactionTypeID { get; set; }
        public Nullable<int> FromWarehouse { get; set; }
        public Nullable<int> ToWarehouse { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string ReferenceNo { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> systembatch { get; set; }
        public Nullable<System.DateTime> ManufactureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public decimal Quantity { get; set; }
        public string ProductDescription { get; set; }
        public string Warehousename { get; set; }
        public string Towarehousename { get; set; }
        public string FromWarehousename { get; set; }
        public string actualbatchno { get; set; }
        public int ConversionFactor { get; set; }
        public Nullable<decimal> OzConversion { get; set; }
    }
}
