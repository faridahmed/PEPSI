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
    
    public partial class spRPTQRReport_Result
    {
        public int SLNO { get; set; }
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
        public Nullable<System.DateTime> CheckoutDateTime { get; set; }
        public byte[] QRImageIn { get; set; }
        public byte[] ORImageChallanTime { get; set; }
        public Nullable<System.DateTime> LoadingDateTime { get; set; }
        public Nullable<System.DateTime> InvoiceDateTime { get; set; }
        public string TAName { get; set; }
    }
}
