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
    
    public partial class spRPTCreditLimit_Result
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddress3 { get; set; }
        public string CustomerAddress4 { get; set; }
        public double RcvlAmnt { get; set; }
        public System.DateTime IsRcvlBDate { get; set; }
        public System.DateTime IsRcvlEDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public bool IsBGurated { get; set; }
        public string Remarks { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseDescription { get; set; }
    }
}
