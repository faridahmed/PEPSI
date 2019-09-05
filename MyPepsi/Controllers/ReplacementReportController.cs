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
    public class ReplacementReportController : Controller
    {
        // GET: ReplacementReport
        private PEPSIEntities db = new PEPSIEntities();
        public ActionResult Index()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.custLits = new SelectList(db.Customers, "CustomerID", "CustomerName");
            return View();
        }
        public JsonResult GetCustomerListsWareHouseWise(int wId)
        {
            using (PEPSIEntities dc = new PEPSIEntities())
            {

                try
                {
                    var custLists = db.Customers.Where(x => x.WarehouseID == wId && x.ActiveStatus == "A");

                    return new JsonResult { Data = custLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Customer Not Found" });
                    // throw ex;
                }
            }
        }
        public ActionResult ReplacmentDetails(string bDate, string eDate, Nullable<int> wId, Nullable<int> cId)
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
            // var v = (from x in db.Warehouses where x.WarehouseID ==wId select x).FirstOrDefault();
            //if (wId != 0 && cId != 0)
            //{
                List<spRPTReplacementDetails_Result> replacementDetailsData = db.spRPTReplacementDetails(d1, d2, wId, cId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Replacement\ReplacementDetails.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("ReplacementDetailsDataSet", replacementDetailsData);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("ReplaceDetalsView");
            }


    }
}
