using Microsoft.Reporting.WebForms;
using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: QRReport
        public ActionResult Index()
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

            var clients = db.Customers.Where(x => x.WarehouseID == wn.WarehouseID)
                .Select(s => new
                {
                    Text = s.CustomerName + " , " + s.CustomerAddress1,
                    Value = s.CustomerID
                })
                .ToList();

            ViewBag.CustomerLists = new SelectList(clients, "Value", "Text").OrderBy(s => s.Text);

            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.PaymentMode = new SelectList(db.PaymentModes, "PaymentModeID", "PaymentModeDescription");

            return View();
        }

        public ActionResult PaymentStatusForNormalCustomer(string bDate, string eDate)
        {
            DateTime d1 = Convert.ToDateTime(bDate);
            DateTime d2 = Convert.ToDateTime(eDate);
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<spRPTPaymentStatusWithMoneyReceiptNormalCustomer_Result> paymentStatus = db.spRPTPaymentStatusWithMoneyReceiptNormalCustomer(d1, d2).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\PaymentCollection\PaymentStatus.rdlc";
            ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));



            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });



            ReportDataSource rdc = new ReportDataSource("PaymentStatusNormal", paymentStatus);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("PaymentStatusForNormalCustomer");
        }

        public ActionResult PaymentStatusForNormalCustomerWarehouseWise(string bDate, string eDate, int warehouseId)
        {
            DateTime d1 = Convert.ToDateTime(bDate);
            DateTime d2 = Convert.ToDateTime(eDate);
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<spRPTPaymentStatusWithMoneyReceiptNormalCustomer_WarehouseWise_Result> paymentStatusWarehouseWise = db.spRPTPaymentStatusWithMoneyReceiptNormalCustomer_WarehouseWise(d1, d2, warehouseId).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\PaymentCollection\PaymentStatusDepotWise.rdlc";
            ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
            ReportParameter rp2 = new ReportParameter("Warehouse", warehouseId.ToString());

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

            ReportDataSource rdc = new ReportDataSource("PaymentStatusWarehouseWise", paymentStatusWarehouseWise);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("PaymentStatusForNormalCustomer");
        }

        public ActionResult PaymentStatusForNormalCustomerPModeWise(string bDate, string eDate, int warehouseId, string paymentModeId)
        {
            DateTime d1 = Convert.ToDateTime(bDate);
            DateTime d2 = Convert.ToDateTime(eDate);
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<spRPTPaymentStatusWithMoneyReceiptNormalCustomer_PaymentModeWise_Result> paymentStatusCollectionModeWise = db.spRPTPaymentStatusWithMoneyReceiptNormalCustomer_PaymentModeWise(d1, d2, warehouseId, paymentModeId).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\PaymentCollection\PaymentStatusPaymentModeWise.rdlc";
            ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));


            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1});

            ReportDataSource rdc = new ReportDataSource("PaymentStatusPaymentModeWise", paymentStatusCollectionModeWise);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("PaymentStatusForNormalCustomer");
        }

        public ActionResult PaymentStatusForNormalCustomerWise(string bDate, string eDate, int customerId)
        {
            DateTime d1 = Convert.ToDateTime(bDate);
            DateTime d2 = Convert.ToDateTime(eDate);
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<spRPTPaymentStatusWithMoneyReceiptNormalCustomerWise_Result> paymentStatusCustomerWise = db.spRPTPaymentStatusWithMoneyReceiptNormalCustomerWise(d1, d2, customerId).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\PaymentCollection\PaymentStatusCustomerWise.rdlc";
            ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
            ReportParameter rp2 = new ReportParameter("custId", customerId.ToString());

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

            ReportDataSource rdc = new ReportDataSource("PaymentStatusCustomerWise", paymentStatusCustomerWise);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("PaymentStatusForNormalCustomer");
        }

        //public ActionResult exportReport()
        //{
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/Reports"), "CrystalReport1.rpt"));
        //    //rd.SetDataSource(db.ProductTransferNoteReport.ToList());
        //    // crystalReport.SetParameterValue("ProductID", txtProductID.Text);
        //    //var PTID2 = db.ProductTransactions.Max(p => p.ProductTransactionNumber);
        //    //rd.SetParameterValue("@ProductTransactionNumber", PTID2);
        //    //rd.SetParameterValue("@ProductTransactionID", 2);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "application/pdf", "Employee_list.pdf");
        //    }
        //    catch
        //    {
        //        throw;
        //    }
    }
}
