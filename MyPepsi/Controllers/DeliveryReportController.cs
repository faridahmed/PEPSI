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
    public class DeliveryReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: DeliveryReport
        public ActionResult Index()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            return View();
        }
        public ActionResult DBDeliveryDetails(string bDate, string eDate, int wId,string wName)
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
                List<spRPTDelivery_Result> CustomerDelivery = db.spRPTDelivery(d1, d2, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\WarehoudeWiseDelivery.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("Warehouse", wName.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("WarehoudeWiseDeliveryDataSet", CustomerDelivery);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBDeliveryDetails");
            }

            else
            {
                List<spRPTDelivery_Result> CustomerDelivery = db.spRPTDelivery(d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\WarehoudeWiseDelivery.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                // ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("WarehoudeWiseDeliveryDataSet", CustomerDelivery);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBDeliveryDetails");
            }

        }
    }
}