using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPepsi.ViewModel
{

    public class StockVM
    {
        public int SLNo { get; set; }
        public int TransactionNo { get; set; }
        public int WarehouseID { get; set; }
        public int TransactionTypeID { get; set; }
        public Nullable<int> FromWarehouse { get; set; }
        public Nullable<int> ToWarehouse { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> VehicleID { get; set; }
        public Nullable<int> DriverID { get; set; }
        public Nullable<int> TAID { get; set; }
        public Nullable<decimal> FareAmnt { get; set; }
        public Nullable<decimal> TotCase { get; set; }
        public string StoreLocation { get; set; }
        public List<ProductTransactionDetail> ProdTrDtl { get; set; }
    }
    public class StockReceivedVM
    {

        public int TransactionNo { get; set; }
        public int WarehouseID { get; set; }
        public int TransactionTypeID { get; set; }
        public int FromWarehouse { get; set; }
        public int ToWarehouse { get; set; }
        public System.DateTime TransactionDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime tManufactureDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime tExpiryDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public string StoreLocation { get; set; }
        public List<ProductTransactionDetail> ProdTrDtl { get; set; }
    }
    public class OrderEntryVM
    {
        public int SLNo { get; set; }
        //public int SalesOrderNO { get; set; }
        public int CustomerID { get; set; }
        public decimal TotOrderCase { get; set; }
        public decimal TotOrderBox { get; set; }
        public string Remarks { get; set; }
        public int Delivered { get; set; }
        public Nullable<byte> WarehouseID { get; set; }
        public int EmptyBottleCase { get; set; }
        public Nullable<decimal> TotalOrderAmount { get; set; }
        public Nullable<System.DateTime> ExpectDeliverDate { get; set; }
        public List<OrderBankDetail> oderCustomer { get; set; }
    }
    public class LoadSheetVM
    {
        [Key]
        public int OrderID { get; set; }
        public Nullable<int> SalesOrderNo { get; set; }
        public Nullable<int> Trip { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public int CustomerID { get; set; }
        public int WarehouseID { get; set; }
        public int VehicleID { get; set; }
        public int SalesPersonID { get; set; }
        //public int DriverID { get; set; }
        public Nullable<int> DriverID { get; set; }
        public Nullable<int> ChallanNumber { get; set; }
        public string ChallanCancel { get; set; }
        public Nullable<System.DateTime> DeliveryChallanDate { get; set; }
        public string Delivered { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal SecurityAmount { get; set; }
        public decimal RebateAmount { get; set; }
        public decimal AgencyCommission { get; set; }
        public int ProductRateID { get; set; }
        public decimal SpecialCharge1Amount { get; set; }
        public decimal SpecialCharge2Amount { get; set; }
        public decimal SpecialCharge3Amount { get; set; }
        public int SpecialCharge1Type { get; set; }
        public int SpecialCharge2Type { get; set; }
        public int SpecialCharge3Type { get; set; }
        public string SpecialNote { get; set; }
        public string LoadSheetRemarks { get; set; }
        public string Cash { get; set; }
        public string IsCashSettlementPrepared { get; set; }
        public string DeliveredBy { get; set; }
        public Nullable<int> CashSettlementID { get; set; }
        public string CashSettlementPreparedBy { get; set; }
        public string ReadyForDelivery { get; set; }
        public string ReadyForDeliveryOptionGivenBy { get; set; }
        public string RebateWillBeGivenWithOrder { get; set; }
        public int OrderTypeID { get; set; }
        public string Transit { get; set; }
        public Nullable<int> CustomerExecutiveID { get; set; }
        public string Promotional { get; set; }
        public string Replacement { get; set; }
        public string InvoiceFromVATInvoiceNumber { get; set; }
        public int VATRateId { get; set; }
        public string VATChallanRequired { get; set; }
        public Nullable<decimal> FinishGoodsUnloadCharge { get; set; }
        public List<OrderDetailVM> orderdetail { get; set; }
        public int QRSL { get; set; }
    }
    public  class OrderDetailVM
    {
        public int WarehouseID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> SchemeID { get; set; }
        public decimal Quantity { get; set; }
        public decimal AlternateQuantity { get; set; }
        public decimal RebateQuantity { get; set; }
        public decimal AlternateRebateQuantity { get; set; }
        public Nullable<int> PlasticBoxQuantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> AlternateUnitPrice { get; set; }
        public Nullable<int> ReturnedEmptyQuantity { get; set; }
        public decimal AlternateReturnedEmptyQuantity { get; set; }
        public Nullable<decimal> AgencyCommission { get; set; }
        public Nullable<decimal> AlternateAgencyCommission { get; set; }
        public Nullable<decimal> SecurityDeposit { get; set; }
        public Nullable<decimal> AlternateSecurityDeposit { get; set; }
        public Nullable<decimal> PlasticBoxSecurity { get; set; }
        public Nullable<decimal> VATPrice { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<int> BatchNo { get; set; }
        public Nullable<System.DateTime> MfgDate { get; set; }
        public Nullable<System.DateTime> ExpDate { get; set; }
        public Nullable<decimal> MRPRate { get; set; }
    }
    public class ReturnmentVM
    {
        public int WarehouseID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public int ReturnmentID { get; set; }
        public Nullable<decimal> ReturnGoodsQty { get; set; }
        public Nullable<decimal> InAmount { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string Reasons { get; set; }
        public Nullable<int> RefNumber { get; set; }
        public Nullable<decimal> AdjustedAmt { get; set; }
        public string ApprovalFlag { get; set; }
        public Nullable<decimal> FareAmt { get; set; }
        public Nullable<int> TAID { get; set; }
        public string VechileNo { get; set; }
        public int ReturnTypeID { get; set; }
        //  public string CreateBy { get; set; }
        //  public Nullable<System.DateTime> CreateDate { get; set; }
        //  public Nullable<System.DateTime> ApprovedDate { get; set; }
        //  public string ApproveBy { get; set; }
        public List<ReturnmentDetail> listreturndetail { get; set; }
    }
    public class InvoiceVM
    {
        public int OrderID { get; set; }
        public Nullable<int> SalesOrderNo { get; set; }
        public Nullable<int> Trip { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public int CustomerID { get; set; }
        public int WarehouseID { get; set; }
        public int VehicleID { get; set; }
        public int SalesPersonID { get; set; }
        public int DriverID { get; set; }
        public Nullable<int> ChallanNumber { get; set; }
        public string ChallanCancel { get; set; }
        public Nullable<System.DateTime> DeliveryChallanDate { get; set; }
        public string Delivered { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal SecurityAmount { get; set; }
        public decimal RebateAmount { get; set; }
        public decimal AgencyCommission { get; set; }
        public int ProductRateID { get; set; }
        public decimal SpecialCharge1Amount { get; set; }
        public decimal SpecialCharge2Amount { get; set; }
        public decimal SpecialCharge3Amount { get; set; }
        public int SpecialCharge1Type { get; set; }
        public int SpecialCharge2Type { get; set; }
        public int SpecialCharge3Type { get; set; }
        public string SpecialNote { get; set; }
        public string LoadSheetRemarks { get; set; }
        public string Cash { get; set; }
        public string IsCashSettlementPrepared { get; set; }
        public string DeliveredBy { get; set; }
        public Nullable<int> CashSettlementID { get; set; }
        public string CashSettlementPreparedBy { get; set; }
        public string ReadyForDelivery { get; set; }
        public string ReadyForDeliveryOptionGivenBy { get; set; }
        public string RebateWillBeGivenWithOrder { get; set; }
        public int OrderTypeID { get; set; }
        public string Transit { get; set; }
        public Nullable<int> CustomerExecutiveID { get; set; }
        public string Promotional { get; set; }
        public string Replacement { get; set; }
        public string InvoiceFromVATInvoiceNumber { get; set; }
        public int VATRateId { get; set; }
        public string VATChallanRequired { get; set; }
        public Nullable<decimal> FinishGoodsUnloadCharge { get; set; }
        public Nullable<int> Helper1 { get; set; }
        public Nullable<int> Helper2 { get; set; }
        public Nullable<int> Helper3 { get; set; }
        public int TAID { get; set; }
        public string VechileNo { get; set; }
        public decimal FareAmnt { get; set; }
        public string AdvanceOrder { get; set; }
        public Nullable<decimal> TotalCases { get; set; }
        public int TotalPcs { get; set; }

        public List<InvoiceHelper> InvoiceHelperVM { get; set; }
        // public List<OrderDetail> orderdetail { get; set; }
        // public List<CashSettlementDetail> cashorderdetail { get; set; }
    }
    public class DeliveryControlVM
    {
        public int SLNo { get; set; }
        public int CustomerID { get; set; }
        public int WarehouseID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public double RcvlAmnt { get; set; }
        public bool IsRcvl { get; set; }
        public System.DateTime IsRcvlBDate { get; set; }
        public System.DateTime IsRcvlEDate { get; set; }
        public int OutsDays { get; set; }
        public bool IsODays { get; set; }
        public System.DateTime IsODaysBDate { get; set; }
        public System.DateTime IsODaysEDate { get; set; }
        public double TransitAmnt { get; set; }
        public bool IsTrAmnt { get; set; }
        public System.DateTime IsTrAmntBDate { get; set; }
        public System.DateTime IsTrAmntEDate { get; set; }
        public double TransitCase { get; set; }
        public bool IsTrCase { get; set; }
        public System.DateTime IsTrCaseBDate { get; set; }
        public System.DateTime IsTrCaseEDate { get; set; }
        public int TransitDays { get; set; }
        public bool IsTrDays { get; set; }
        public System.DateTime IsTrDaysBDate { get; set; }
        public System.DateTime IsTrDaysEDate { get; set; }
        public double BCAmnt { get; set; }
        public bool IsBCAmnt { get; set; }
        public System.DateTime IsBCAmntBDate { get; set; }
        public System.DateTime IsBCAmntEDate { get; set; }
        public double TotalOutstanding { get; set; }
        public bool IsTotalOutstanding { get; set; }
        public System.DateTime IsTotalOutstandingBDate { get; set; }
        public System.DateTime IsTotalOutstandingEDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public bool IsBGurated { get; set; }
        public string Remarks { get; set; }
        public string CustomerName { get; set; }
    }
    public class OrderApproveVM
    {
        public int WarehouseID { get; set; }
        public int ApprovedID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public string Status { get; set; }
        public string ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string Approve2nd { get; set; }
        public Nullable<System.DateTime> Approve2ndDate { get; set; }
    }
    public class PaymentTransactionVM
    {
        public int PaymentTransactionType { get; set; }
        public int CustomerID { get; set; }
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
       // public Nullable<int> DepositedByID { get; set; }
        public Nullable<bool> AdvanceAdjustment { get; set; }
        public int UserID { get; set; }
        public string ChallanNo { get; set; }
        public Nullable<int> BankID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> LocalBranchID { get; set; }
        public int CashSettlementID { get; set; }
    }
    public class ProductRateVM
    {
        [Key]
        public byte ProductRateID { get; set; }
        public string ProductRateDescription { get; set; }
        public int ProductID { get; set; }


        public List<ProductRateDetailVM> PRDVM { get; set; }
    }
    public class ProductRateDetailVM
    {
        public byte ProductRateID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal AlternateUnitPrice { get; set; }
        public decimal AgencyCommission { get; set; }
        public decimal AlternateAgencyCommission { get; set; }
        public decimal SecurityDeposit { get; set; }
        public decimal AlternateSecurityDeposit { get; set; }
        public decimal PlasticBoxSecurity { get; set; }
        public Nullable<decimal> MRPRate { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductRate ProductRate { get; set; }
    }
    public class ApprovedClaimVM
    {

        public List<ApprovedClaimDetailsVM> AppClaimDetails { get; set; }

    }
    public class ApprovedClaimDetailsVM
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
    public class MenuListVM
    {
        public int SLNo { get; set; }
        public int UserID { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> MainMenuID { get; set; }
    }
    public class MenuSubVM
    {
        public int Id { get; set; }
        public string SubMenu { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public Nullable<int> MainMenuId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> UserID { get; set; }
        public int WarehouseID { get; set; }
        public int MenuCode { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Status { get; set; }

        public virtual MenuMain MenuMain { get; set; }
        public virtual MenuRole MenuRole { get; set; }
        // public bool IsSelected { get; set; }
        public List<menudetailVM> menudetail { get; set; }


    }

    public class menudetailVM
    {
        public int Id { get; set; }
        public string SubMenu { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int MainMenuId { get; set; }
        public int RoleId { get; set; }
        public int UserID { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Status { get; set; }


        public int IsSelected { get; set; }
        public int WarehouseID { get; set; }
        public int MenuCode { get; set; }




    }
    public partial class TransportQRdataVM
    {
        public int SLNO { get; set; }
        public Nullable<int> WareHouseID { get; set; }
        public string Type { get; set; }
        public string VehicleType { get; set; }
        public string QRSLNo { get; set; }
        public int SLCount { get; set; }
        public System.DateTime QRGeneratingDate { get; set; }
        public int AgencyID { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNumber { get; set; }
        public int TruckCapacity { get; set; }
        public System.DateTime EntryDateTime { get; set; }
        public Nullable<System.DateTime> LoadingDateTime { get; set; }
        public Nullable<System.DateTime> InvoiceDateTime { get; set; }
        public Nullable<System.DateTime> CheckoutDateTime { get; set; }
        public byte[] QRImageIn { get; set; }
        public byte[] ORImageChallanTime { get; set; }
        public string FromWhere { get; set; }
        public Nullable<int> DeliveryChallanNumber { get; set; }
        public Nullable<int> SalesOrderNo { get; set; }
        public string EnteredBy { get; set; }
        public string ExitInfoEnteredBy { get; set; }
    }

    public class CustomerBGInfoVM
    {
        public int CustomerID { get; set; }
        // public string BankGuaranteeNo { get; set; }
        public string BGRefNo { get; set; }
        public string BankName { get; set; }
        public string BankBrName { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        // public Nullable<System.DateTime> AmendmentExiryDate { get; set; }
        public Nullable<decimal> BGAmount { get; set; }
        public Nullable<decimal> ActualBGAmt { get; set; }
        // public Nullable<decimal> AmendmentBGAmount { get; set; }
        // public string Status { get; set; }
        // public string CreateBy { get; set; }
        // public Nullable<System.DateTime> CreateDate { get; set; }
        // public string UpdateBy { get; set; }
        // public Nullable<System.DateTime> UpdateDate { get; set; }
    }
    public class CustomerCreditLimitVM
    {
        public int CustomerID { get; set; }
        public int CLRefNo { get; set; }
        public string CLNo { get; set; }
        public string ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> ActivateDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        // public Nullable<System.DateTime> AmendmentExiryDate { get; set; }
        public Nullable<decimal> CLAmount { get; set; }
        //public Nullable<decimal> AmendmentCLAmount { get; set; }
        //public string Status { get; set; }
        //public string CreateBy { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public string UpdateBy { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
    }
    public partial class TransportAgencyandFareSetupVM
    {
        public int Sl { get; set; }
        public int WarehouseID { get; set; }
        public int ChallanNumber { get; set; }
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
    }
    public class CustomerVM
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerShortName { get; set; }
        public string ProprietorsName { get; set; }
        public string ContactPersonsName { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddress3 { get; set; }
        public string CustomerAddress4 { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerFax { get; set; }
        public string CustomerMobilePhone { get; set; }
        public decimal CreditLimit { get; set; }
        public int CreditPeriodInDays { get; set; }
        public int ProductRateID { get; set; }
        public decimal OutstandingAmount { get; set; }
        public int SalesPersonID { get; set; }
        public int DriverID { get; set; }
        public int HelperID { get; set; }
        public int CustomerExecutiveID { get; set; }
        public int RegionID { get; set; }
        public int NoOfVisiCoolers { get; set; }
        public byte NoOfPMX { get; set; }
        public Nullable<int> CustomerTypeID { get; set; }
        public Nullable<System.DateTime> CustomerStartingDate { get; set; }
        public Nullable<System.DateTime> CustomerEndingDate { get; set; }
        public Nullable<decimal> Security { get; set; }
        public string VATRegistrationNumber { get; set; }
        public Nullable<int> VatChallanRequired { get; set; }
        public int VATRateId { get; set; }
        public string ClusterName { get; set; }
        public Nullable<int> AllowanceRateID { get; set; }
        public string AllowanceType { get; set; }
        public Nullable<int> WarehouseID { get; set; }
    }
    public class ReplacementMasterVM
    {
        public int ReplaceNo { get; set; }
        public int WarehouseID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<System.DateTime> ProcessfromDate { get; set; }
        public Nullable<System.DateTime> ProcessToDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> TotReturnAmt { get; set; }
        public Nullable<decimal> TotSalesAmt { get; set; }
        public Nullable<decimal> TotPayableAmt { get; set; }
        public Nullable<int> CalculationID { get; set; }
        public List<ReplaceDetail> replacedetail { get; set; }

    }
    public class ReplaceDisburseVM
    {
        public int WarehouseID { get; set; }
        public int CustomerID { get; set; }
        public int ReplaceDisburseID { get; set; }
        public Nullable<int> ReplaceRefNo { get; set; }
        public Nullable<int> LoadSheetNo { get; set; }

        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<decimal> TotCase { get; set; }
        public Nullable<decimal> TotBox { get; set; }
        public Nullable<decimal> TotDisburseAmt { get; set; }
        public Nullable<decimal> RemainingAmt { get; set; }
        public Nullable<decimal> AdjustAmt { get; set; }
        public Nullable<int> DriverID { get; set; }
        public string VehicleID { get; set; }
        public string Remarks { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        public List<ReplaceDisburseDetail> disbursedetail { get; set; }
    }
    public class EmptyMoveVM
    {
        //  public int SLNo { get; set; }
        public int WarehouseID { get; set; }
        public int TransactionNo { get; set; }
        public Nullable<decimal> TotalEmpty { get; set; }
        public Nullable<decimal> TotalBox { get; set; }
        public Nullable<int> TransactionType { get; set; }
        public Nullable<int> ToWarehouse { get; set; }
        public string Status { get; set; }
        public Nullable<int> DriverID { get; set; }
        public string VehicleNo { get; set; }
        public Nullable<int> ConversionFactor { get; set; }
        public string LocationID { get; set; }
        public string BinNo { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public List<EmptyMoveDetail> emptydetail { get; set; }

    }
    public class EmptyTransferVM
    {
        //  public int SLNo { get; set; }
        public int WarehouseID { get; set; }
        public int TransactionNo { get; set; }
        public Nullable<decimal> TotalEmpty { get; set; }
        public Nullable<decimal> TotalBox { get; set; }
        public Nullable<int> TransactionType { get; set; }
        public Nullable<int> ToWarehouse { get; set; }
        public Nullable<int> FromWarehouse { get; set; }
        //public string Status { get; set; }
        public Nullable<int> DriverID { get; set; }
       // public Nullable<string> VehicleNo { get; set; }
        public Nullable<int> ConversionFactor { get; set; }
        public string LocationID { get; set; }
        public string BinNo { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public List<EmptyMoveDetail> emptydetail { get; set; }

    }
    public class ReplacSalesVM
    {
        public int ReplaceNo { get; set; }
        public int WarehouseID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<System.DateTime> ProcessfromDate { get; set; }
        public Nullable<System.DateTime> ProcessToDate { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> TotReturnAmt { get; set; }
        public Nullable<decimal> TotSalesAmt { get; set; }
        public Nullable<decimal> TotPayableAmt { get; set; }
        public Nullable<int> CalculationID { get; set; }


    }
    public class ReplaceManualVM
    {
        public int CustomerID { get; set; }
        public int ReplaceID { get; set; }
        public Nullable<int> MonthName { get; set; }
        public Nullable<int> NoLevel { get; set; }
        public Nullable<int> NoCrown { get; set; }
        public Nullable<int> NoCan { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> PayableAmount { get; set; }
        public Nullable<int> GivenMethod { get; set; }
        // public string CreateBy { get; set; }
        //  public Nullable<System.DateTime> CreateDate { get; set; }
         public string Remarks { get; set; }
    }
}