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
    
    public partial class spRPTCustomerCurrentLedgerDetailAsOnDate_Result
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string TransactionNo { get; set; }
        public Nullable<int> TransactionTypeID { get; set; }
        public string DebitCredit { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> DrAmount { get; set; }
        public Nullable<decimal> CrAmount { get; set; }
        public Nullable<int> RefNumber { get; set; }
        public string Narration { get; set; }
        public string WarehouseName { get; set; }
    }
}
