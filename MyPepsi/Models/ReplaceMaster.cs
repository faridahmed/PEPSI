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
    
    public partial class ReplaceMaster
    {
        public int ReplaceNo { get; set; }
        public int WarehouseID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<System.DateTime> ProcessfromDate { get; set; }
        public Nullable<System.DateTime> ProcessToDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> TotReturnAmt { get; set; }
        public Nullable<decimal> TotSalesAmt { get; set; }
        public Nullable<decimal> TotPayableAmt { get; set; }
        public Nullable<int> CalculationID { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> AdjustAmt { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
    }
}
