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
    
    public partial class spRPTCustomerWiseReturnDetails_Result
    {
        public int ReturnmentID { get; set; }
        public Nullable<decimal> ReturnGoodsQty { get; set; }
        public Nullable<decimal> InAmount { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string Reasons { get; set; }
        public Nullable<int> RefNumber { get; set; }
        public Nullable<int> ReturnTypeID { get; set; }
        public Nullable<decimal> AdjustedAmt { get; set; }
        public string ApprovalFlag { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string ApproveBy { get; set; }
        public int WarehouseID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<decimal> ReturnQty { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Packsize { get; set; }
        public Nullable<int> BatchNo { get; set; }
        public string Reason { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string ProductDescription { get; set; }
        public string WarehouseDescription { get; set; }
        public Nullable<decimal> RebateQty { get; set; }
        public Nullable<decimal> RebatePlastic { get; set; }
        public Nullable<int> SchemeID { get; set; }
    }
}
