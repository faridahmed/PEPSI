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
    
    public partial class CustomerLedgerMaster
    {
        public int CustomerID { get; set; }
        public Nullable<decimal> CurrentBalance { get; set; }
        public Nullable<decimal> CreditAmt { get; set; }
        public Nullable<decimal> ReturnAmt { get; set; }
        public Nullable<decimal> ReplacementPayable { get; set; }
        public Nullable<decimal> PaymentAmt { get; set; }
        public Nullable<decimal> InvoiceAmt { get; set; }
        public Nullable<decimal> TotalSequrityAmt { get; set; }
        public Nullable<decimal> BookedAmt { get; set; }
        public Nullable<decimal> BlockedAmt { get; set; }
        public Nullable<decimal> BankGuranteeAmt { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> OutstandingAmt { get; set; }
        public string CustomerStatus { get; set; }
        public string CheckingFlag { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<decimal> SequrityReceive { get; set; }
        public Nullable<decimal> SequrityRefund { get; set; }
        public Nullable<decimal> LoadUnloadCharge { get; set; }
        public Nullable<int> WarehouseID { get; set; }
    }
}
