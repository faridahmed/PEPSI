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
    
    public partial class Payment
    {
        public int PaymentTransactionType { get; set; }
        public int CustomerID { get; set; }
        public int WarehouseID { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal PaymentReceived { get; set; }
        public string PaymentModeID { get; set; }
        public System.DateTime ActualPaymentEntryDate { get; set; }
        public string PaymentNote { get; set; }
        public int MoneyReceiptNo { get; set; }
        public Nullable<int> BankBranchID { get; set; }
        public string ChequeDDNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public Nullable<int> LocalBankBranchID { get; set; }
        public Nullable<int> DepositedByID { get; set; }
        public Nullable<bool> AdvanceAdjustment { get; set; }
        public int UserID { get; set; }
        public string ChallanNo { get; set; }
        public Nullable<int> BankID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> LocalBranchID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
