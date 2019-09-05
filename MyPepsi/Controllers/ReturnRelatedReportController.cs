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
    public class ReturnRelatedReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: ReturnRelatedReport
        public ActionResult Index()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.movementType = new SelectList(db.TransactionTypes.Where(x => x.UsedType == "E" && x.Status == "A"), "TransactionTypeID", "TransactionTypeName");
            return View();
        }
        public ActionResult EmptyBottleMOvement(Nullable<int> wId, string bDate, string eDate,  Nullable<int> TrID)
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
            string rptName;
            if (TrID == 28)
            {
                rptName = "Empty Receive";
            }
            else if (TrID == 29)
            {
                rptName = "Empty Transffer";
            }
            else if (TrID == 30)
            {
                rptName = "Empty Received From Customer";
            }
            else if (TrID == 31)
            {
                rptName = "Empty Issue to Production";
            }
            else if (TrID == 32)
            {
                rptName = "Empty Adjust/Destroy";
            }
            else if (TrID == 33)
            {
                rptName = "Empty Transfer Received";
            }
            else if (TrID == 34)
            {
                rptName = "Empty Opening";
            }
            else  
            {
                rptName = "Empty Receive";
            }
            var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
            if (wId == 0)
            {

                List<spRPTEmptyMovement_Result> emptyBottleMovement = db.spRPTEmptyMovement(null, d1, d2, TrID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\EmptyBottle\EmptyBottleMovement.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("ReportNameParameter", rptName);
                ReportParameter rp3 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });

                ReportDataSource rdc = new ReportDataSource("EmptyBottleMovementDataSet", emptyBottleMovement);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("EmptyBottleMovement");
            }

            else
            {
                List<spRPTEmptyMovement_Result> emptyBottleMovement = db.spRPTEmptyMovement(wId, d1, d2, TrID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\EmptyBottle\EmptyBottleMovement.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("ReportNameParameter", rptName);
                ReportParameter rp3 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });

                ReportDataSource rdc = new ReportDataSource("EmptyBottleMovementDataSet", emptyBottleMovement);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("EmptyBottleMovement");
            }
        }       
        public ActionResult EmptyStock(string trDate, Nullable<int> wId)
        {
            DateTime d1 = Convert.ToDateTime(trDate);

            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //
            if (wId == 0)
            {
                List<spREmptyStockAsOnDate_Result> stockAsOn = db.spREmptyStockAsOnDate(d1, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\EmptyBottle\StockAsOnDate.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("spREmptyStockAsOnDate", stockAsOn);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("EmptyBottleMovement");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spREmptyStockAsOnDate_Result> stockAsOn = db.spREmptyStockAsOnDate(d1, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\EmptyBottle\StockAsOnDate.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("spREmptyStockAsOnDate", stockAsOn);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("EmptyBottleMovement");
            }
        }

    }
}