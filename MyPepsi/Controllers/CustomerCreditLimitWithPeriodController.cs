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
    public class CustomerCreditLimitWithPeriodController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: CustomerCreditLimitWithPeriod
        public ActionResult Index()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.CustomerType= new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeDescription");
            //ViewBag.Customer = new SelectList(db.Customers.Where(s=>s.ActiveStatus=="A"), "CustomerID", "CustomerName");

            var clients = db.Customers.OrderBy(x => x.CustomerName).Where(x => x.ActiveStatus =="A")
    .Select(s => new
    {
        Text = s.CustomerName + " , " + s.CustomerAddress1,
        Value = s.CustomerID
    })
    .ToList();
            ViewBag.Customer = new SelectList(clients, "Value", "Text");

            return View();
        }
        public ActionResult CustomerCreditLimitStatus(int wId,string wName)
        {
            //DateTime d1 = Convert.ToDateTime(bDate);
            //DateTime d2 = Convert.ToDateTime(eDate);
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
                List<spRPTCreditLimit_Result> creditLimitStatus = db.spRPTCreditLimit(null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerCreditLimitWithPeriod.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse", wName.ToString());
                // ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("CustomerCreditLimitWithPeriodDataSet", creditLimitStatus);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

            else
            {
                List<spRPTCreditLimit_Result> creditLimitStatus = db.spRPTCreditLimit(wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerCreditLimitWithPeriod.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse", wName.ToString());
                // ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("CustomerCreditLimitWithPeriodDataSet", creditLimitStatus);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

        }
        public ActionResult CustomerCurrentLedger(Nullable<int> wId, Nullable<int> cId, Nullable<int> cType)
        {
            //DateTime d1 = Convert.ToDateTime(bDate);
            //DateTime d2 = Convert.ToDateTime(eDate);\
            if (cId == null)
            {
                cId = 0;
            }
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            //var v = (from x in db.Warehouses where x.WarehouseID ==wId select x).FirstOrDefault();
            if (wId == 0 && cId==0)
            {
                List<spRPTCustomerCurrentLedger_Result> CustomerCurrentLedger = db.spRPTCustomerCurrentLedger(null,null,null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerCurrentLedger.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse", "All");
                // ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("CustomerCurrentLedgerDataSet", CustomerCurrentLedger);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

            else if (wId != 0 && cId == 0 && cType==0)
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerCurrentLedger_Result> CustomerCurrentLedger = db.spRPTCustomerCurrentLedger(wId, null,null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerCurrentLedger.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse", v.WarehouseDescription.ToString());
                // ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("CustomerCurrentLedgerDataSet", CustomerCurrentLedger);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

            else if (wId != 0 && cId == 0 && cType != 0)
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerCurrentLedger_Result> CustomerCurrentLedger = db.spRPTCustomerCurrentLedger(wId, null, cType).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerCurrentLedger.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse", v.WarehouseDescription.ToString());
                // ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("CustomerCurrentLedgerDataSet", CustomerCurrentLedger);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerCurrentLedger_Result> CustomerCurrentLedger = db.spRPTCustomerCurrentLedger(wId,cId, cType).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerCurrentLedger.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse", v.WarehouseDescription.ToString());
                // ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("CustomerCurrentLedgerDataSet", CustomerCurrentLedger);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

        }

        public JsonResult GetCustomerListsWareHouseWise(int wId)
        {
            using (PEPSIEntities dc = new PEPSIEntities())
            {

                try
                {
                    var custLists = db.Customers.Where(x => x.WarehouseID == wId && x.ActiveStatus=="A");

                    return new JsonResult { Data = custLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Customer Not Found" });
                    // throw ex;
                }
            }
        }

        public ActionResult CustomerLedgerSummaryAsOnDate(Nullable<int> wId, Nullable<int> cId, string eDate)
        {
            DateTime d1 = Convert.ToDateTime(eDate);
            //DateTime d2 = Convert.ToDateTime(eDate);\
            if (cId == null)
            {
                cId = 0;
            }
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            var v = (from x in db.Warehouses where x.WarehouseID ==wId select x).FirstOrDefault();
            if (wId == 0 && cId == 0)
            {
                //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerCurrentLedgerAsOnDate_Result> CustomerLedgerAsOnDate = db.spRPTCustomerCurrentLedgerAsOnDate(null, null, d1).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerLedgerAsOnDateSummary.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse","All");
                ReportParameter rp2 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1,rp2 });

                ReportDataSource rdc = new ReportDataSource("CustomerLedgerAsOnDateSummaryDataSet", CustomerLedgerAsOnDate);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

            else if (wId != 0 && cId == 0)
            {
           List<spRPTCustomerCurrentLedgerAsOnDate_Result> CustomerLedgerAsOnDate = db.spRPTCustomerCurrentLedgerAsOnDate(wId, null, d1).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerLedgerAsOnDateSummary.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse",v.WarehouseDescription.ToString());
                ReportParameter rp2 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("CustomerLedgerAsOnDateSummaryDataSet", CustomerLedgerAsOnDate);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }


            else
            {
                List<spRPTCustomerCurrentLedgerAsOnDate_Result> CustomerLedgerAsOnDate = db.spRPTCustomerCurrentLedgerAsOnDate(wId, cId, d1).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerLedgerAsOnDateSummary.rdlc";
                ReportParameter rp1 = new ReportParameter("Warehouse", v.WarehouseDescription.ToString());
                ReportParameter rp2 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("CustomerLedgerAsOnDateSummaryDataSet", CustomerLedgerAsOnDate);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

        }

        public ActionResult CustomerLedgerDetailsAsOnDate(Nullable<int> wId, Nullable<int> cId, string eDate)
        {
            DateTime d1 = Convert.ToDateTime(eDate);
            //DateTime d2 = Convert.ToDateTime(eDate);\
            if (cId == null)
            {
                cId = 0;
            }
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };
            var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
            if (wId == 0 && cId == 0)
            {
                //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerCurrentLedgerDetailAsOnDate_Result> CustomerLedgerDetailAsOnDate = db.spRPTCustomerCurrentLedgerDetailAsOnDate(null, null, d1).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerLedgerDetailAsOnDate.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1});

                ReportDataSource rdc = new ReportDataSource("CustomerLedgerDetailAsOnDateDataSet", CustomerLedgerDetailAsOnDate);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

            else if (wId != 0 && cId == 0)
            {
                List<spRPTCustomerCurrentLedgerDetailAsOnDate_Result> CustomerLedgerDetailAsOnDate = db.spRPTCustomerCurrentLedgerDetailAsOnDate(wId, null, d1).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerLedgerDetailAsOnDate.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

                ReportDataSource rdc = new ReportDataSource("CustomerLedgerDetailAsOnDateDataSet", CustomerLedgerDetailAsOnDate);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }


            else
            {
                List<spRPTCustomerCurrentLedgerDetailAsOnDate_Result> CustomerLedgerDetailAsOnDate = db.spRPTCustomerCurrentLedgerDetailAsOnDate(wId, cId, d1).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Customer\CustomerLedgerDetailAsOnDate.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy"));

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1});

                ReportDataSource rdc = new ReportDataSource("CustomerLedgerDetailAsOnDateDataSet", CustomerLedgerDetailAsOnDate);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("CustomerCreditLimitStatus");
            }

        }

    }
}