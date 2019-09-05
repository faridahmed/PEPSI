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
    
    public partial class spRPTProductBalance_WarehouseWise_Result
    {
        public int ProductID { get; set; }
        public string ProductDescription { get; set; }
        public int WarehouseID { get; set; }
        public string ProductShortName { get; set; }
        public string UnitMeasurement { get; set; }
        public string AlternateUnitMeasurement { get; set; }
        public int ConversionFactor { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AlternateUnitPrice { get; set; }
        public int BottleReturnable { get; set; }
        public string BottleSize { get; set; }
        public Nullable<int> PETOrGRB { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> OzConversion { get; set; }
        public int OnHandQuantity { get; set; }
        public int BurstBottleQuantity { get; set; }
        public int BreakageBottleQuantity { get; set; }
        public int PlasticBoxQuantity { get; set; }
        public int EmptyBottleQuantity { get; set; }
        public Nullable<bool> ShowInList { get; set; }
    }
}