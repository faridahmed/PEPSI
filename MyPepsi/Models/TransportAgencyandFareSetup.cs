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
    
    public partial class TransportAgencyandFareSetup
    {
        public int Sl { get; set; }
        public int WarehouseID { get; set; }
        public int ChallanNumber { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> ChallanDate { get; set; }
        public int TAID { get; set; }
        public string VechileNo { get; set; }
        public decimal FareAmnt { get; set; }
        public decimal ExtraAmount { get; set; }
        public Nullable<decimal> TotalCases { get; set; }
        public Nullable<System.DateTime> AcknowledgementDate { get; set; }
        public string Status { get; set; }
        public string EnteredBy { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> ActualAcknowledgementDate { get; set; }
        public Nullable<System.DateTime> EnteredDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string EmptyReturn { get; set; }
        public string Address { get; set; }
    }
}
