using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPepsi.ViewModel
{
    public class POSMVM
    {
        public class POSMItemVM
        {
            public int PosmID { get; set; }
            public string PosmName { get; set; }

        }
        public class POSMIssueVM
        {
            public List<POSMIssueDetailsVM> PODetails { get; set; }

        }
        public class POSMIssueDetailsVM
        {
            public int SL { get; set; }
            public Nullable<int> POSMChallanNo { get; set; }
            public Nullable<int> FromWarehouse { get; set; }
            public Nullable<System.DateTime> IssuingDate { get; set; }
            public Nullable<int> IssueTypeID { get; set; }
            public Nullable<int> DistributorID { get; set; }
            public Nullable<int> RefDChallan { get; set; }
            public Nullable<int> WareHousID { get; set; }
            public string TransferNote { get; set; }
            public Nullable<int> VehicleID { get; set; }
            public Nullable<int> DriverID { get; set; }
            public Nullable<int> POSItemID { get; set; }
            public Nullable<int> SchemeID { get; set; }
            public Nullable<int> IssuedQty { get; set; }
            public Nullable<decimal> IssuingUnitPrice { get; set; }
            public string Status { get; set; }
            public string IssuedBy { get; set; }
            public Nullable<System.DateTime> EntryDate { get; set; }
            public Nullable<System.DateTime> IssueReceivedDate { get; set; }


            public virtual Customer Customer { get; set; }
            public virtual POSMItem POSMItem { get; set; }
            public virtual POSMScheme POSMScheme { get; set; }
        }
        public class POSMReceiveVM
        {
            public List<PODetailsVM> PODetails { get; set; }
        }
        public class PODetailsVM
        {

            public int SL { get; set; }
            public Nullable<int> GRNO { get; set; }
            public Nullable<int> WarehouseID { get; set; }
            public System.DateTime ReceivingDate { get; set; }
            public int SchemeID { get; set; }
            public Nullable<int> SupplierID { get; set; }
            public string SuppRefNo { get; set; }
            public Nullable<System.DateTime> SupplierRefDate { get; set; }
            public int POSItem { get; set; }
            public string Description { get; set; }
            public Nullable<int> ReceivedQty { get; set; }
            public Nullable<decimal> UnitPrice { get; set; }
            public string WarrentyPeriod { get; set; }
            public string WarrentyType { get; set; }
            public string RcvBy { get; set; }
            public Nullable<System.DateTime> EntryDate { get; set; }

            public virtual POSMItem POSMItem { get; set; }
            public virtual POSMScheme POSMScheme { get; set; }
            public virtual Supplier Supplier { get; set; }
        }
        public class POSMScemeVM
        {
            public int SchemeID { get; set; }
            public string SchemeDescription { get; set; }
        }
        public class SupplierVM
        {
            public int SupplierID { get; set; }
            public string SupplierName { get; set; }
            public string Address { get; set; }
            public string PhoneNo { get; set; }


        }
        public class DBTargetVM
        {
            public List<DBTargetDetailVM> DBTargetDetails { get; set; }
        }
        public partial class DBTargetDetailVM
        {
            public int SL { get; set; }
            public string TargetID { get; set; }
            public string TargetDescription { get; set; }
            public Nullable<System.DateTime> StartDate { get; set; }
            public Nullable<System.DateTime> EndDate { get; set; }
            public Nullable<int> DBID { get; set; }
            public Nullable<int> SKUID { get; set; }
            public Nullable<int> TargetQty { get; set; }
            public Nullable<int> TargetTypeID { get; set; }
            public Nullable<System.DateTime> UploadedDate { get; set; }
            public Nullable<int> EnteredBy { get; set; }
            public Nullable<System.DateTime> EntryDate { get; set; }
            public Nullable<int> UpdatedBy { get; set; }
            public Nullable<System.DateTime> UpdatedDate { get; set; }
        }
    }
}