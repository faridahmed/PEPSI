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
    public class POSMReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: POSMReport
        public ActionResult Index()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            return View();
        }
        public ActionResult POSMReceiveIssue(string bDate, string eDate, Nullable<int> wId)
        {
            DateTime d1 = Convert.ToDateTime(bDate);
            DateTime d2 = Convert.ToDateTime(eDate);
            //if (wId == 0)
            //{
            //    getwID = wId.GetValueOrDefault(); ;
            //}
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID ==wId select x).FirstOrDefault();
            if (wId == 0)
            {
                List<spRPTPOSMReceiveIsuueDateWise_Result> posmRecIss = db.spRPTPOSMReceiveIsuueDateWise(d1, d2, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\POSM\POSMReceiveIssueDateWise.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("POSMReceiveIssueDataSet", posmRecIss);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("POSMView");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTPOSMReceiveIsuueDateWise_Result> posmRecIss = db.spRPTPOSMReceiveIsuueDateWise(d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\POSM\POSMReceiveIssueDateWise.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("POSMReceiveIssueDataSet", posmRecIss);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("POSMView");
            }

        }

        public ActionResult POSMOnhandStock(Nullable<int> wId)
        {
            //if (wId == 0)
            //{
            //    getwID = wId.GetValueOrDefault(); ;
            //}
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID ==wId select x).FirstOrDefault();
            if (wId == 0)
            {
                List<spRPTPOSMStockOnHand_Result> posmOnHandStock = db.spRPTPOSMStockOnHand(null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\POSM\POSMStockOnhandReport.rdlc";
                //ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp1 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("POSMOnHandStockDataset", posmOnHandStock);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("POSMView");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTPOSMStockOnHand_Result> posmOnHandStock = db.spRPTPOSMStockOnHand(wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\POSM\POSMStockOnhandReport.rdlc";
                ReportParameter rp1 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("POSMOnHandStockDataset", posmOnHandStock);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("POSMView");
            }

        }

    }
}