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
    public class StockReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: StockReport
        public ActionResult Index()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.TrType = new SelectList(db.TransactionTypes.Where(x => x.TransactionTypeID == 1 || x.TransactionTypeID == 2 || x.TransactionTypeID == 3 || x.TransactionTypeID == 7), "TransactionTypeID", "TransactionTypeName");
            return View();
        }
        public ActionResult ProductBalanceWarehouseWise(int wId)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

            List<spRPTProductBalance_WarehouseWise_Result> pBalanceWarehouseWise = db.spRPTProductBalance_WarehouseWise(wId).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Stock\StockStatusWarehouseWise.rdlc";
            ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

            ReportDataSource rdc = new ReportDataSource("StockStatusWarehouseWiseDataset", pBalanceWarehouseWise);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("ProductBalanceWarehouseWise");
        }

        //public ActionResult ProductTransferNote(int trNo, int trId)
        //{
        //    ReportViewer reportViewer = new ReportViewer
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(50),
        //        Height = Unit.Percentage(50)
        //    };
        //    //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

        //    List<spRPTProductTransferNote_Result> ptrNote = db.spRPTProductTransferNote(trNo, trId).ToList();

        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Stock\ProductTransferNote.rdlc";
        //    //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

        //    //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

        //    ReportDataSource rdc = new ReportDataSource("ProductTransferNoteDataset", ptrNote);
        //    reportViewer.LocalReport.DataSources.Add(rdc);

        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.Visible = true;

        //    ViewBag.ReportViewer = reportViewer;
        //    return PartialView("ProductBalanceWarehouseWise");
        //}
        public ActionResult StockAsOnDate(string trDate, Nullable<int> wId)
        {
            DateTime d1 = Convert.ToDateTime(trDate);

            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            // var v = (from x in db.Warehouses where x.WarehouseID ==wId select x).FirstOrDefault();
            if (wId == 0)
            {
                List<spRPTStockAsOnDate_Result> stockAsOn = db.spRPTStockAsOnDate(d1, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Stock\StockAsOnDate.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("StockAsOnDateDataSet", stockAsOn);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("ProductBalanceWarehouseWise");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTStockAsOnDate_Result> stockAsOn = db.spRPTStockAsOnDate(d1, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Stock\StockAsOnDate.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("StockAsOnDateDataSet", stockAsOn);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("ProductBalanceWarehouseWise");
            }
        }
        public ActionResult ProductTransactionRpt(string bDate, string eDate, Nullable<int> wId, Nullable<int> Tr)
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
            string rptName;
            if (Tr == 1)
            {
                rptName = "Receive Product";
            }
            else if (Tr == 2)
            {
                rptName = "Transfer To other Warehouse";
            }
            else
            {
                rptName = "Invoice";
            }

            var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

            List<spRPTProductTransaction_WarehouseWise_Result> pBalanceWarehouseWise = db.spRPTProductTransaction_WarehouseWise(wId,d1,d2, Tr).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Stock\ProductTransactionMovement.rdlc";
            ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());
            ReportParameter rp2 = new ReportParameter("ReportNameParameter", rptName);

            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1,rp2 });

            ReportDataSource rdc = new ReportDataSource("SpTransactionReportDataSet", pBalanceWarehouseWise);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("ProductBalanceWarehouseWise");
        }
    }
}