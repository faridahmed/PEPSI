using Microsoft.Reporting.WebForms;
using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class ReprintReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: ReprintReport
        public ActionResult Index()
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

            ViewBag.soNumber = new SelectList(db.Orders.Where(x => x.WarehouseID == wn.WarehouseID).OrderByDescending(x => x.OrderID), "OrderID", "OrderID");
            ViewBag.invNumber = new SelectList(db.Orders.Where(x => x.WarehouseID == wn.WarehouseID && x.Delivered=="Y").OrderByDescending(x => x.ChallanNumber), "ChallanNumber", "ChallanNumber");
            ViewBag.MR = new SelectList(db.Payments.Where(x => x.WarehouseID == wn.WarehouseID).OrderByDescending(x => x.MoneyReceiptNo), "MoneyReceiptNo", "MoneyReceiptNo");
            ViewBag.TrNumber = new SelectList(db.ProductTransactions.Where(x => x.WarehouseID == wn.WarehouseID && x.TransactionTypeID == 2).OrderByDescending(x => x.TransactionNo), "TransactionNo", "TransactionNo");
            ViewBag.POSMInvoiceNumber = new SelectList(db.POSMIssues.Where(x => x.FromWarehouse == wn.WarehouseID ).OrderByDescending(x => x.POSMChallanNo).Select(x => x.POSMChallanNo).Distinct());
            ViewBag.ReplaceInvoiceNumber = new SelectList(db.ReplaceDisburseMasters.Where(x => x.WarehouseID == wn.WarehouseID).OrderByDescending(x => x.ReplaceDisburseID).Select(x => x.ReplaceDisburseID).Distinct());
            ViewBag.EmptyTr = new SelectList(db.EmptyMoveMasters.Where(x => x.WarehouseID == wn.WarehouseID && x.TransactionType==29).Select(x => x.TransactionNo).Distinct());
            // ViewBag.soLNumber = new SelectList(db.Orders.Where(x => x.WarehouseID == wn.WarehouseID).OrderByDescending(x => x.SalesOrderNo), "SalesOrderNo", "SalesOrderNo");
            return View();
        }
        [HttpPost]
        public ActionResult Invoice(int InvNo, string typeInv)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            try { 
            //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
           List<spRPTInvoice_Result> invoiceData = db.spRPTInvoice(InvNo).ToList();
                //List<spRPTInvoiceNew_Result> invoiceData = db.spRPTInvoiceNew(SOrderNo).ToList();
       
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Reprint\Invoice.rdlc";

                if(typeInv=="Reprint Invoice")
                { 
                ReportParameter rp1 = new ReportParameter("Inv", "Reprint Invoice");

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });
                }
                else
                {
                    ReportParameter rp1 = new ReportParameter("Inv", "Invoice");

                    reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                }
                ReportDataSource rdc = new ReportDataSource("InvoiceDataSet", invoiceData);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            }
            catch (Exception EX)
            {
                string message = EX.Message;
            }
            return PartialView("Invoice");

        }

        public ActionResult LoadSheet(int SOrderNo)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

            List<LoadSheetCR_Result> loadsheetData = db.LoadSheetCR(SOrderNo).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Reprint\LDSheet.rdlc";
            //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

            ReportDataSource rdc = new ReportDataSource("LoadSheetDataSet", loadsheetData);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("LoadSheet");
        }
        public ActionResult ProductTransferNote(int trNo, int trId)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

            List<spRPTProductTransferNote_Result> ptrNote = db.spRPTProductTransferNote(trNo, trId).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Stock\ProductTransferNote.rdlc";
            //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

            ReportDataSource rdc = new ReportDataSource("ProductTransferNoteDataset", ptrNote);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("LoadSheet");
        }

        public ActionResult MoneyReceipt(int mrNumber)
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();
            Nullable<int> wId = wn.WarehouseID;
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

            List<sprptMoneyreceiptCR_Result> mrData = db.sprptMoneyreceiptCR(wId, mrNumber).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Reprint\MoneyReceipt.rdlc";
            //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

            ReportDataSource rdc = new ReportDataSource("MRDataSet", mrData);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("MoneyReceipt");
        }
        [HttpPost]
        public ActionResult POSMInvoice(int POInv)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
            List<spRPTPOSMRInvoice_Result> POSMinvoiceData = db.spRPTPOSMRInvoice(POInv).ToList();
            //List<spRPTInvoiceNew_Result> invoiceData = db.spRPTInvoiceNew(SOrderNo).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Reprint\POSMInvoice.rdlc";
            //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

            ReportDataSource rdc = new ReportDataSource("POSMInvoiceDataSet", POSMinvoiceData);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Invoice");
        }

        public ActionResult ReplacementInvoice(int ReplaceInv)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
            List<spRPTReplaceInvoice_Result>ReplaceinvoiceData = db.spRPTReplaceInvoice(ReplaceInv).ToList();
            //List<spRPTInvoiceNew_Result> invoiceData = db.spRPTInvoiceNew(SOrderNo).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Reprint\ReplacementInvoice.rdlc";
            ReportParameter rp1 = new ReportParameter("RInv", "Reprint Replcaement Invoice");

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

            ReportDataSource rdc = new ReportDataSource("ReplacementInvoiceDataSet", ReplaceinvoiceData);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Invoice");
        }
        public ActionResult EmptyTransferNote(int trNo)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

            List<spREmptyTransfer_Result> ptrNote = db.spREmptyTransfer(trNo).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\EmptyBottle\EmptyTransfer.rdlc";
            //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

            ReportDataSource rdc = new ReportDataSource("EmptyTransferNote", ptrNote);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("LoadSheet");
        }
    }
}