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
    public class OrderReportController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: OrderReport
        public ActionResult Index()
        {
            ViewBag.Warehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.returnType = new SelectList(db.TransactionTypes.Where(x => x.UsedType == "R" && x.Status == "A"), "TransactionTypeID", "TransactionTypeName");
            ViewBag.custLits = new SelectList(db.Customers.OrderBy(x=>x.CustomerName), "CustomerID", "CustomerName");
            ViewBag.CustomerType = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeDescription");
            return View();
        }

        public ActionResult DBOrderDetails(string bDate, string eDate, Nullable<int> wId)
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
                List<spRPTOrderFromCustomer_Result> orderFromCustomer = db.spRPTOrderFromCustomer(d1, d2, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Order\DBOrder.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("OrderFromCustomerDataSet", orderFromCustomer);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTOrderFromCustomer_Result> orderFromCustomer = db.spRPTOrderFromCustomer(d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Order\DBOrder.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("OrderFromCustomerDataSet", orderFromCustomer);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }

        public ActionResult OrderVSDelivery(string bDate, string eDate, Nullable<int> wId)
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
            if (wId == 0)
            {
                List<spRPTOrderVsDelivery_Result> orderVSdelivery = db.spRPTOrderVsDelivery(d1, d2, null).ToList();
                
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Order\OrderVsDeliveryReport.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("OrderVsDelDataSet", orderVSdelivery);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTOrderVsDelivery_Result> orderVSdelivery = db.spRPTOrderVsDelivery(d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Order\OrderVsDeliveryReport.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("OrderVsDelDataSet", orderVSdelivery);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }

        public ActionResult CustomerWarehouseWiseSales(string bDate, string eDate, Nullable<int> wId, Nullable<int> custID)
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
            if (wId == 0)
            {
                List<spRPTCustomerWarehouseWiseSales_Result> customerWiseSales = db.spRPTCustomerWarehouseWiseSales(d1, d2, null,null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\CustomerWiseWareHouseWiseSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("CustomerWarehouseWiseSalesDataSet", customerWiseSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerWarehouseWiseSales_Result> customerWiseSales = db.spRPTCustomerWarehouseWiseSales(d1, d2, wId, custID).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\CustomerWiseWareHouseWiseSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("CustomerWarehouseWiseSalesDataSet", customerWiseSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }

        public ActionResult CustomerSKUWiseSales(string bDate, string eDate, Nullable<int> wId, Nullable<int> cType)
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
            if (wId == 0 && cType==0)
            {
                List<spRPTCustomerSKUWiseWarehouseWiseSales_Result> customerWiseSales = db.spRPTCustomerSKUWiseWarehouseWiseSales(null,d1, d2, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\CustomerSKUWiseSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("CustomerWiseSKUWiseSalesDataSet", customerWiseSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerSKUWiseWarehouseWiseSales_Result> customerWiseSales = db.spRPTCustomerSKUWiseWarehouseWiseSales(cType,d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\CustomerSKUWiseSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("CustomerWiseSKUWiseSalesDataSet", customerWiseSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }

        public ActionResult RateWiseSKUWiseSales(string bDate, string eDate, Nullable<int> wId)
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
            if (wId == 0)
            {
                List<spRPTProductRateWiseDailySales_Result> customerWiseSales = db.spRPTProductRateWiseDailySales(d1, d2, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SKUWise\SKUWiseRateWiseMRPSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("SKURateMRPWiseSalesDataSet", customerWiseSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTProductRateWiseDailySales_Result> customerWiseSales = db.spRPTProductRateWiseDailySales(d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SKUWise\SKUWiseRateWiseMRPSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("SKURateMRPWiseSalesDataSet", customerWiseSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }

        public ActionResult PMXCustomerSKUSales(string bDate, string eDate, Nullable<int> wId)
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
            if (wId == 0)
            {
                List<spRPTPMXCustomerSKUWiseWarehouseWiseSales_Result> PMXSales = db.spRPTPMXCustomerSKUWiseWarehouseWiseSales(d1, d2, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\PMXCustomerWiseSKUSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("POSTMixSalesSummaryDataSet", PMXSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTPMXCustomerSKUWiseWarehouseWiseSales_Result> PMXSales = db.spRPTPMXCustomerSKUWiseWarehouseWiseSales(d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\PMXCustomerWiseSKUSales.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("POSTMixSalesSummaryDataSet", PMXSales);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }

        //[HttpPost]
        //public ActionResult Invoice(int SOrderNo)
        //{
        //    ReportViewer reportViewer = new ReportViewer
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(50),
        //        Height = Unit.Percentage(50)
        //    };
        //    //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

        //    List<spRPTInvoice_Result> invoiceData = db.spRPTInvoice(SOrderNo).ToList();

        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\Invoice.rdlc";
        //    //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

        //    //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

        //    ReportDataSource rdc = new ReportDataSource("InvoiceDataSet", invoiceData);
        //    reportViewer.LocalReport.DataSources.Add(rdc);

        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.Visible = true;

        //    ViewBag.ReportViewer = reportViewer;
        //    return PartialView("DBOrderDetails");
        //}

        //public ActionResult LoadSheet(int SOrderNo, int Trip)
        //{
        //    ReportViewer reportViewer = new ReportViewer
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(50),
        //        Height = Unit.Percentage(50)
        //    };
        //    //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();

        //    List<LoadSheetCR_Result> loadsheetData = db.LoadSheetCR(SOrderNo, Trip).ToList();

        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Order\LDSheet.rdlc";
        //    //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

        //    //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

        //    ReportDataSource rdc = new ReportDataSource("LoadSheetDataSet", loadsheetData);
        //    reportViewer.LocalReport.DataSources.Add(rdc);

        //    reportViewer.LocalReport.Refresh();
        //    reportViewer.Visible = true;

        //    ViewBag.ReportViewer = reportViewer;
        //    return PartialView("DBOrderDetails");
        //}
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
        public ActionResult CustomerWiseReturn(string bDate, string eDate, Nullable<int> wId, Nullable<int> custID, Nullable<int> returnType)
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
            if (returnType == 13)
            {
                rptName = "Customer Wise Empty Return";
            }
            else if (returnType == 14)
            {
                rptName = "Customer Wise Filled Return";
            }
            else
            {
                rptName = "Customer Wise Market Return";
            }
            var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
            if (custID == 0)
            {

                List<spRPTCustomerWiseReturnDetails_Result> customerWiseReturn = db.spRPTCustomerWiseReturnDetails(wId, d1, d2, null, returnType).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\CustomerWiseReturnReport.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("ReportNameParameter", rptName);
                ReportParameter rp3 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });

                ReportDataSource rdc = new ReportDataSource("CustomerWiseReturnDataSet", customerWiseReturn);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                List<spRPTCustomerWiseReturnDetails_Result> customerWiseReturn = db.spRPTCustomerWiseReturnDetails(wId, d1, d2, custID, returnType).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\CustomerWiseReturnReport.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("ReportNameParameter", rptName);
                ReportParameter rp3 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3 });

                ReportDataSource rdc = new ReportDataSource("CustomerWiseReturnDataSet", customerWiseReturn);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }
        }
        public ActionResult SalesSUmmary76(string bDate, string eDate, Nullable<int> wId)
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
            if (wId == 0)
            {
                List<spRPTSalesSummaryWithAllInfo_76_Result> salesSummary76 = db.spRPTSalesSummaryWithAllInfo_76(d1, d2, null).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\SalesSummaryWithAllInfo.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("spRPTSalesSummaryWithAllInfo_76DataSet", salesSummary76);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTSalesSummaryWithAllInfo_76_Result> salesSummary76 = db.spRPTSalesSummaryWithAllInfo_76(d1, d2, wId).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\SalesSummaryWithAllInfo.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("spRPTSalesSummaryWithAllInfo_76DataSet", salesSummary76);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }
        public ActionResult ASMWiseSales38(Nullable<int> wId, string bDate, string eDate)
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
            if (wId == 0)
            {
                List<spRPTCustomerWarehouseWiseSales43_Result> ASMWiseSales43 = db.spRPTCustomerWarehouseWiseSales43(null,d1, d2).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\ASMWiseSales43.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", "All");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("ASMWiseSales43DataSet", ASMWiseSales43);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

            else
            {
                var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
                List<spRPTCustomerWarehouseWiseSales43_Result> ASMWiseSales43 = db.spRPTCustomerWarehouseWiseSales43(wId, d1, d2).ToList();
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Sales\ASMWiseSales43.rdlc";
                ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));
                ReportParameter rp2 = new ReportParameter("wName", v.WarehouseDescription.ToString());

                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2 });

                ReportDataSource rdc = new ReportDataSource("ASMWiseSales43DataSet", ASMWiseSales43);
                reportViewer.LocalReport.DataSources.Add(rdc);

                reportViewer.LocalReport.Refresh();
                reportViewer.Visible = true;

                ViewBag.ReportViewer = reportViewer;
                return PartialView("DBOrderDetails");
            }

        }
    }
}