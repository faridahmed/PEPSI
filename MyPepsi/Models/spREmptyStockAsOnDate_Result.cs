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
    
    public partial class spREmptyStockAsOnDate_Result
    {
        public int WarehouseID { get; set; }
        public string WarehouseDescription { get; set; }
        public int ProductID { get; set; }
        public int ConversionFactor { get; set; }
        public string ProductDescription { get; set; }
        public Nullable<decimal> openqty { get; set; }
        public Nullable<decimal> openpls { get; set; }
        public Nullable<decimal> recqty { get; set; }
        public Nullable<decimal> recpls { get; set; }
        public Nullable<decimal> custrecqty { get; set; }
        public Nullable<decimal> custrecpls { get; set; }
        public Nullable<decimal> tranqty { get; set; }
        public Nullable<decimal> trpls { get; set; }
        public Nullable<decimal> trfqty { get; set; }
        public Nullable<decimal> trfpls { get; set; }
        public Nullable<decimal> prodqty { get; set; }
        public Nullable<decimal> prodpls { get; set; }
        public Nullable<decimal> desqty { get; set; }
        public Nullable<decimal> despls { get; set; }
        public Nullable<decimal> eprqty { get; set; }
        public Nullable<decimal> eprpls { get; set; }
        public Nullable<decimal> stockqty { get; set; }
        public Nullable<decimal> stockpls { get; set; }
    }
}