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
    
    public partial class CustomerBGInfo
    {
        public int CustomerID { get; set; }
        public int BankGuaranteeNo { get; set; }
        public string BGRefNo { get; set; }
        public string BankName { get; set; }
        public string BankBrName { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> AmendmentExiryDate { get; set; }
        public Nullable<decimal> BGAmount { get; set; }
        public Nullable<decimal> ActualBGAmt { get; set; }
        public Nullable<decimal> AmendmentBGAmount { get; set; }
        public string Status { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
