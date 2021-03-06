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
    
    public partial class CashSettlementDetail
    {
        public int WarehouseID { get; set; }
        public int CashSettlementID { get; set; }
        public int ProductID { get; set; }
        public int SchemeID { get; set; }
        public int Quantity { get; set; }
        public int AlternateQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AlternateUnitPrice { get; set; }
        public int PlasticBoxQuantity { get; set; }
        public int RebateQuantity { get; set; }
        public int AlternateRebateQuantity { get; set; }
        public decimal GrossSalesAmount { get; set; }
        public decimal AlternateGrossSalesAmount { get; set; }
        public decimal RebateAmount { get; set; }
        public decimal AlternateRebateAmount { get; set; }
        public decimal AgencyCommission { get; set; }
        public decimal AlternateAgencyCommission { get; set; }
        public int ReturnedQuantity { get; set; }
        public int AlternateReturnedQuantity { get; set; }
        public int ReturnedPlasticBoxQuantity { get; set; }
        public int ReturnedBurstQuantity { get; set; }
        public int ReturnedBreakageQuantity { get; set; }
        public int ReturnedFilledQuantity { get; set; }
        public int AlternateReturnedFilledQuantity { get; set; }
        public decimal SecurityDeposit { get; set; }
        public decimal AlternateSecurityDeposit { get; set; }
        public decimal PlasticBoxSecurity { get; set; }
        public int ReturnedRebateQuantity { get; set; }
        public int AlternateReturnedRebateQuantity { get; set; }
        public int AlternateReturnedBurstQuantity { get; set; }
        public int AlternateReturnedBreakageQuantity { get; set; }
        public Nullable<bool> ShowInList { get; set; }
        public decimal VATPrice { get; set; }
        public decimal VATAmount { get; set; }
        public decimal ReturnedSecurityDeposit { get; set; }
        public decimal ReturnedAlternateSecurityDeposit { get; set; }
        public decimal ReturnedPlasticBoxSecurity { get; set; }
        public Nullable<decimal> MRPRate { get; set; }
    }
}
