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
    
    public partial class spRPTPaymentStatusWithMoneyReceiptNormalCustomer_WarehouseWise_Result
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int MoneyReceiptNo { get; set; }
        public int PaymentTransactionType { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal PaymentReceived { get; set; }
        public string PaymentModeID { get; set; }
        public System.DateTime ActualPaymentEntryDate { get; set; }
        public string PaymentNote { get; set; }
        public Nullable<int> CustomerTypeID { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddress3 { get; set; }
        public string CustomerAddress4 { get; set; }
        public string ChequeDDNo { get; set; }
        public string UserName { get; set; }
        public string PaymentModeDescription { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public string WarehouseDescription { get; set; }
        public int UserID { get; set; }
    }
}
