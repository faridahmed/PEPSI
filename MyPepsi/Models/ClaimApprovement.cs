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
    
    public partial class ClaimApprovement
    {
        public int SLNo { get; set; }
        public int ApprovedID { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<byte> SchemeID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<decimal> ApproveQuantity { get; set; }
        public Nullable<decimal> ApproveAmount { get; set; }
        public string EneteredBy { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
    }
}
