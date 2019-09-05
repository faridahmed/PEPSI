using Microsoft.Reporting.WebForms;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using OnBarcode.Barcode;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class TransportQRController : Controller
    {
        // GET: TransportQR

        private PEPSIEntities db = new PEPSIEntities();
        public ActionResult Index()
        {
            ViewBag.TrAgency = new SelectList(db.TransportAgencySetups.OrderBy(x => x.TAName), "TAID", "TAName");
            ViewBag.TrCapacity = new SelectList(db.TransportCapacityCases, "SlNo", "CapacityCases");
            var V = db.TransportQRDatas.Max(p => p.SLNO);
            ViewBag.PKID = (V + 1);

            var sql = "SELECT TOP(1) ISNULL(SLCount, 0) + 1 AS CountSL FROM dbo.TransportQRData WHERE(Type = 'Finished Goods') AND(MONTH(GETDATE()) = MONTH(GETDATE())) AND(YEAR(GETDATE()) = YEAR(GETDATE())) ORDER BY SLNO DESC";
            //"SELECT TOP (1) ISNULL(SLCount, 0) + 1 AS CountSL FROM dbo.TransportQRData WHERE(Type = 'Finished Goods') AND(MONTH(EntryDateTime) = MONTH(GETDATE())) AND(YEAR(EntryDateTime) = YEAR(GETDATE())) ORDER BY SLNO DESC";
            var total = db.Database.SqlQuery<int>(sql).First().ToString();
            if (total == null)
            {
                total = 1.ToString();
            }
            ViewBag.QRSL = total;

            DateTime dt; dt = DateTime.Now;
            ViewBag.QRSLCount = dt.ToString("MMM") + "-" + total;

            return View();
        }


        [HttpPost]
        public ActionResult generateQR(string t1, string t2, TransportQRdataVM trqrvm)
        {
            bool status = false;
            string mes = "";
            try
            {
                GenerateQrcode(t1, t2);
                TransportQRData tqrD = new TransportQRData();
                {
                    byte[] image = System.IO.File.ReadAllBytes(t2);

                    tqrD.Type = trqrvm.Type;
                    tqrD.VehicleType = trqrvm.VehicleType;
                    tqrD.QRSLNo = trqrvm.QRSLNo;
                    tqrD.SLCount = trqrvm.SLCount;
                    tqrD.QRGeneratingDate = DateTime.Now;
                    tqrD.AgencyID = trqrvm.AgencyID;
                    tqrD.VehicleNumber = trqrvm.VehicleNumber;
                    tqrD.DriverName = trqrvm.DriverName;
                    tqrD.DriverMobileNumber = trqrvm.DriverMobileNumber;
                    tqrD.TruckCapacity = trqrvm.TruckCapacity;
                    tqrD.EntryDateTime = DateTime.Now;
                    tqrD.QRImageIn = image;
                    tqrD.WareHouseID = trqrvm.WareHouseID;
                    tqrD.EnteredBy= User.Identity.Name;

                }
                db.TransportQRDatas.Add(tqrD);
                db.SaveChanges();
                status = true;
                //return Json(status= status, mes = mes, JsonRequestBehavior.AllowGet);
                return Json(new { status = status, mes = mes, MaxJsonLength = 10000000, JsonRequestBehavior.AllowGet });
            }

            catch (Exception ex)
            {

                return Json(new { status = "error", message = "Product ID Not Found" });
            }
        }

        private void GenerateQrcode(string _data, string _filename)
        {
            QRCode qrcode = new QRCode();
            qrcode.Data = _data;
            qrcode.DataMode = QRCodeDataMode.Byte;
            qrcode.UOM = UnitOfMeasure.PIXEL;
            qrcode.X = 3;
            qrcode.LeftMargin = 5;
            qrcode.RightMargin = 5;
            qrcode.TopMargin = 5;
            qrcode.BottomMargin = 5;
            qrcode.Resolution = 90;
            qrcode.Rotate = Rotate.Rotate0;
            qrcode.ImageFormat = ImageFormat.Jpeg;
            qrcode.drawBarcode(_filename);
        }
        public ActionResult TransportQRReport(Nullable<int> slNumber)
        {
            //int slNumber = 4;
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<spRPTQRReport_Result> qrData = db.spRPTQRReport(slNumber).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\QR\QRReport.rdlc";
            //ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));



            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });



            ReportDataSource rdc = new ReportDataSource("QRReportDataSet", qrData);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;
            reportViewer.LocalReport.EnableExternalImages = true;
            ViewBag.ReportViewer = reportViewer;
            return PartialView("TransportQRReport");
        }
        [HttpPost]
        public ActionResult saveQRCheckout(int slNumber)
        {
            bool status = false;
            string mes = "";
            try
            {
                TransportQRData tqrD = new TransportQRData();
                {
                    TransportQRData qd = db.TransportQRDatas.SingleOrDefault(x => x.SLNO == slNumber);
                    qd.CheckoutDateTime = System.DateTime.Now;
                    qd.ExitInfoEnteredBy= User.Identity.Name;
                    db.Entry(qd).State = EntityState.Modified;

                }
                db.SaveChanges();
                status = true;
                //return Json(status= status, mes = mes, JsonRequestBehavior.AllowGet);
                return Json(new { status = status, mes = mes, MaxJsonLength = 10000000, JsonRequestBehavior.AllowGet });
            }

            catch (Exception ex)
            {

                return Json(new { status = "error", message = "Problem to update checkout data!!" });
            }
        }
    }
}