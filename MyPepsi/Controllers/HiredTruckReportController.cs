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
    public class HiredTruckReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: HiredTruckReport
        public ActionResult Index()
        {
            var w = (from yt in db.UserLogins
                     where yt.UserID.ToString() == User.Identity.Name
                     select new { yt.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(t => t.WarehouseID == w.WorkStationID).FirstOrDefault();
            //ViewBag.WarehouseID = wn.WarehouseID;
            //ViewBag.WarehouseIDLogin = wn.WarehouseDescription;

            ViewBag.trAgency = new SelectList(db.TransportAgencySetups.OrderBy(x => x.TAName), "TAID", "TAName");


            ViewBag.warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            return View();
        }

        public ActionResult invoiceWise(string bDate, string eDate, Nullable<int> wId, Nullable<int> trID, string st)
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
            string ReportName;
            if(st=="No")
            {
                ReportName = "Hire Truck Supporting Report";
            }
            else
            {
                ReportName = "Hire Truck BILL Report";
            }
            if (trID == 0)
            {
                List<spRPTHreTruckFare_Result> htFareData = db.spRPTHreTruckFare(d1, d2,wId, null,st).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\HireTruck\HireTruckFareReport.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("rptName", ReportName);

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1,rp2 });

                ReportDataSource rdc = new ReportDataSource("HireTruckFareReportDataSet", htFareData);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("invoiceWise");
            }

            else
            {
                //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTHreTruckFare_Result> htFareData = db.spRPTHreTruckFare(d1, d2, wId, trID, st).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\HireTruck\HireTruckFareReport.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));

                ReportParameter rp2 = new ReportParameter("rptName", ReportName);

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });
                ReportDataSource rdc = new ReportDataSource("HireTruckFareReportDataSet", htFareData);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("invoiceWise");
            }

        }

        public ActionResult acknowledgementWise(string bDate, string eDate, Nullable<int> wId, Nullable<int> trID, string st)
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
            string ReportName;
            if (st == "No")
            {
                ReportName = "Hire Truck Supporting Report";
            }
            else
            {
                ReportName = "Hire Truck BILL Report";
            }
            if (trID == 0)
            {
                List<spRPTHreTruckFareAcknowledgementWise_Result> acknowdgement = db.spRPTHreTruckFareAcknowledgementWise(d1, d2, wId, null, st).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\HireTruck\HireTruckFareReportAcknowledgement.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("rptName", ReportName);

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("HireTruckFareReportAcknowledgementDataSet", acknowdgement);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("invoiceWise");
            }

            else if (trID != 0)
            {
                //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTHreTruckFareAcknowledgementWise_Result> acknowdgement = db.spRPTHreTruckFareAcknowledgementWise(d1, d2, wId, trID, st).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\HireTruck\HireTruckFareReportAcknowledgement.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));

                ReportParameter rp2 = new ReportParameter("rptName", ReportName);

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });
                ReportDataSource rdc = new ReportDataSource("HireTruckFareReportAcknowledgementDataSet", acknowdgement);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("invoiceWise");
            }
             else 
            {
                //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTHreTruckFareAcknowledgementWise_Result> acknowdgement = db.spRPTHreTruckFareAcknowledgementWise(d1, d2, wId, null, st).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\HireTruck\HireTruckFareReportAcknowledgement.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));

                ReportParameter rp2 = new ReportParameter("rptName", ReportName);

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });
                ReportDataSource rdc = new ReportDataSource("HireTruckFareReportAcknowledgementDataSet", acknowdgement);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("invoiceWise");
            }

        }

        public ActionResult hireTruckMovement(string bDate, string eDate, Nullable<int> wId, Nullable<int> trID)
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
            //string ReportName;
            //if (trID == "No")
            //{
            //    ReportName = "Hire Truck Supporting Report";
            //}
            //else
            //{
            //    ReportName = "Hire Truck BILL Report";
            //}
            if (trID == 0)
            {
                List<spRPTruckMovementQR_Result> htMovementData = db.spRPTruckMovementQR(d1, d2, wId, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\HireTruck\HireTruckMovementTiming.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("HireTruckMovementTimingDataSet", htMovementData);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("invoiceWise");
            }

            else
            {
                //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTruckMovementQR_Result> htMovementData = db.spRPTruckMovementQR(d1, d2, wId, trID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\HireTruck\HireTruckMovementTiming.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("HireTruckMovementTimingDataSet", htMovementData);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("invoiceWise");
            }

        }

    }
}