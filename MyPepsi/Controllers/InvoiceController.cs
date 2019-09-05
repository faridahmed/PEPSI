using Microsoft.Reporting.WebForms;
using MyPepsi.InGeneral;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
//using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();

        private int chNumber = 0, csID = 0, SalesOrdNo = 0;

        //private SqlConnection con;
        // GET: Invoice
        public ActionResult Index()
        {
            var w = (from yt in db.UserLogins
                     where yt.UserID.ToString() == User.Identity.Name
                     select new { yt.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(t => t.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;

            var loadOrder = db.spGetOrderInfo(wn.WarehouseID).Select(s => new { Text = s.CustomerName + " , " + s.OrderID + " , " + s.SalesOrderNo /* + s.OrderDate + " , "*/ , Value = s.OrderID }).ToList();

            //ViewBag.WarehouseIDList = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.forDeliveryChallan = new SelectList(loadOrder, "Value", "Text");
            ViewBag.HelperLists1 = new SelectList(db.Helpers, "HelperID", "HelperName", "0");
            ViewBag.HelperLists2 = new SelectList(db.Helpers, "HelperID", "HelperName", "0");
            ViewBag.HelperLists3 = new SelectList(db.Helpers, "HelperID", "HelperName", "0");
            ViewBag.TransportAgencyList = new SelectList(db.TransportAgencySetups.Where(x => x.EligibleForCarringGoods == 1), "TAID", "TAName", "0");

            return View(new InvoiceVM());
            //  return View();
        }
        public JsonResult AddInvoiceData(InvoiceVM Inv)
        {
            var w = (from yt in db.UserLogins
                     where yt.UserID.ToString() == User.Identity.Name
                     select new { yt.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(t => t.WarehouseID == w.WorkStationID).FirstOrDefault();
            bool status = false;
            string mes = "";
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "6000000");
            string s3 = string.Concat(s1 + "5000000");
            int MaxChallanNo = Convert.ToInt32(s2);
            int MaxCashID = Convert.ToInt32(s3);
            var maxchno = (from n in db.Orders where n.WarehouseID == w.WorkStationID select n.ChallanNumber).DefaultIfEmpty(MaxChallanNo).Max();
            var maxCasettleId = (from n in db.CashSettlements where n.WarehouseID == w.WorkStationID select n.CashSettlementID).DefaultIfEmpty(MaxCashID).Max();
            var maxprodtr = (from j in db.ProductTransactions where j.WarehouseID == w.WorkStationID select j.TransactionNo).Max() + 1;
            chNumber = Convert.ToInt32(maxchno + 1);
            csID = Convert.ToInt32(maxCasettleId + 1);
            int v = chNumber;
            if (ModelState.IsValid)
            {
                try
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {

                        //  var V = db.Orders.Max(p => p.ChallanNumber);
                        CommonChk cm = new CommonChk();
                        {

                            if (cm.chkDuplicateChallan(Inv.OrderID) == true)
                            {
                                //status = false;


                                var vr = db.Orders.SingleOrDefault(x => x.OrderID == Inv.OrderID);
                                CashSettlement cs = new CashSettlement();
                                {
                                    // var vr = db.Orders.SingleOrDefault(x => x.OrderID == Inv.OrderID);
                                    cs.CashSettlementID = csID;
                                    cs.SalesPersonID = vr.SalesPersonID;
                                    cs.DeliveryChallanDate = DateTime.Today; //vr.DeliveryChallanDate;
                                    cs.CustomerID = vr.CustomerID;
                                    cs.DeliveryChallanNumbers = chNumber;// Convert.ToInt32(vr.ChallanNumber)+1;
                                    cs.WarehouseID = vr.WarehouseID;
                                    cs.VehicleID = vr.VehicleID;
                                    cs.CashSettlementDate = DateTime.Today;
                                    cs.RebateWillBeGivenWithOrder = vr.RebateWillBeGivenWithOrder;
                                    cs.SpecialNoteFromOrder = vr.SpecialNote;
                                    cs.SpecialInstruction = Inv.SpecialNote;
                                    cs.OrderID = vr.OrderID;
                                    cs.ProductRateID = vr.ProductRateID;
                                    cs.DayendFinished = "N";
                                    cs.DayendDay = DateTime.Now;
                                    cs.OrderTypeID = vr.OrderTypeID;
                                    cs.CustomerExecutiveID = vr.CustomerExecutiveID;
                                    cs.DriverID = vr.DriverID;
                                    cs.Promotional = vr.Promotional;
                                    cs.CheckedByShipping = "Y";
                                    cs.AgencyCommission = Inv.AgencyCommission;// vr.AgencyCommission;
                                    cs.TotalSpecialCharges = vr.SpecialCharge1Amount;
                                    cs.SecurityAmount = Inv.SecurityAmount; //vr
                                    cs.DrinksAmount = (Inv.SalesAmount - Inv.AgencyCommission); //(vr.SalesAmount - vr.AgencyCommission);
                                    cs.FinishGoodsUnloadCharge = Inv.FinishGoodsUnloadCharge; //vr
                                    cs.CreateBy = User.Identity.Name;
                                    cs.CraeteDate = DateTime.Now;

                                    // dc.CashSettlements.Add(cs);
                                }
                                dc.CashSettlements.Add(cs);
                                // Product Transaction
                                ProductTransaction pt = new ProductTransaction();
                                {
                                    pt.WarehouseID = Inv.WarehouseID;
                                    pt.TransactionNo = maxprodtr;
                                    pt.TransactionDate = DateTime.Today;
                                    pt.TransactionTypeID = 7;
                                    pt.FromWarehouse = Inv.WarehouseID;
                                    pt.ReferenceNo = csID.ToString();
                                    pt.Remarks = Inv.SpecialNote;
                                    pt.Status = "I";
                                    pt.CreatedBy = User.Identity.Name;
                                    pt.CreatedDate = DateTime.Now;
                                }
                                dc.ProductTransactions.Add(pt);

                                // Helper
                                InvoiceHelper pm = new InvoiceHelper();
                                {
                                    pm.OrderID = Inv.OrderID;
                                    pm.HelperID1 = Inv.Helper1;
                                    pm.HelperID2 = Inv.Helper2;
                                    pm.HelperID3 = Inv.Helper2;
                                    dc.InvoiceHelpers.Add(pm);
                                }
                                // Customer Ledger
                                CustomerLedgerDetail cld = new CustomerLedgerDetail();
                                {
                                    cld.WarehouseID = vr.WarehouseID;
                                    cld.CustomerID = vr.CustomerID;
                                    cld.TransactionNo = "INV" + csID.ToString();
                                    cld.CrAmount = 0;
                                    cld.TransactionTypeID = 15;
                                    cld.TransactionDate = DateTime.Today;
                                    cld.RefNumber1 = csID.ToString();
                                    cld.DrAmount = (Inv.SalesAmount + Inv.SecurityAmount) - (Inv.AgencyCommission + (decimal)Inv.FinishGoodsUnloadCharge);// (Inv.SalesAmount - Inv.AgencyCommission); //(vr.SalesAmount - vr.AgencyCommission);
                                    cld.DebitCredit = "D";
                                    cld.Narration = "Invoice Value#" + cld.DrAmount + " for Customer" + cld.CustomerID;
                                    cld.CreateBy = User.Identity.Name;
                                    cld.CreateDate = System.DateTime.Now;
                                }
                                dc.CustomerLedgerDetails.Add(cld);
                                // update Customer Ledgermaster
                                // Farid Correction 03dec2018
                                //decimal amountofdr = (Inv.SalesAmount - Inv.AgencyCommission);
                                //decimal bokamt = amountofdr + Inv.SecurityAmount;
                                //var clmbf = db.spCustomerBalanceUpdate(vr.CustomerID, "F", bokamt);
                                //var clm = db.spCustomerBalanceUpdate(vr.CustomerID, "I", amountofdr);
                                //var security = db.spCustomerBalanceUpdate(vr.CustomerID, "S", Inv.SecurityAmount);
                                //decimal vInvAmt = (Inv.SalesAmount - Inv.AgencyCommission)- (decimal)Inv.FinishGoodsUnloadCharge;
                                decimal vInvAmt = Inv.SalesAmount - (Inv.AgencyCommission);  //(decimal)Inv.FinishGoodsUnloadCharge
                                decimal vBookAmt = Inv.SalesAmount + Inv.SecurityAmount - Inv.AgencyCommission;
                                decimal vCurBal = (Inv.SalesAmount + Inv.SecurityAmount) - (Inv.AgencyCommission + (decimal)Inv.FinishGoodsUnloadCharge);
                                decimal vChargeAmount = (decimal)Inv.FinishGoodsUnloadCharge;
                                decimal vSecurityAmt = Inv.SecurityAmount;
                                decimal vRebateAmt = 0;
                                if (Inv.RebateAmount > 0)
                                {
                                    decimal vRebate = (Inv.RebateAmount);
                                    vRebateAmt = vRebate;
                                }
                                else
                                {
                                    decimal vRebate = Inv.RebateAmount;
                                    vRebateAmt = vRebate;
                                }

                                decimal vFare = Inv.FareAmnt;


                                db.spUpdateDBsInvoiceValue(
                                    vr.CustomerID,
                                    "I",
                                    vCurBal,
                                    vCurBal,
                                    vInvAmt,
                                    vBookAmt,
                                    vSecurityAmt,
                                    vChargeAmount,
                                    vFare,
                                    vRebateAmt,
                                    chNumber
                                    );
                                // Farid Correction
                                //  var clm = db.spCustomerBalanceUpdate(vr.CustomerID, "I", bokamt);// with sequirity amount
                                // Add Fare Amount
                                if (Inv.VehicleID == 1)
                                {
                                    //need to adjust without
                                    var cus = cm.GetCustomerName(vr.CustomerID, "C");
                                    var cusAdd = db.Customers.Where(x => x.CustomerID == vr.CustomerID).FirstOrDefault();
                                    var c = db.spInsertHireTruckFare(Inv.WarehouseID, chNumber, Inv.TAID, DateTime.Today, cus, cusAdd.CustomerAddress1, Inv.VechileNo, Inv.FareAmnt, 0, Inv.TotalCases, "No", User.Identity.Name, DateTime.Now, "Hired Truck");

                                }

                                var ODetails = db.OrderDetails.Where(x => x.WarehouseID == Inv.WarehouseID && x.OrderID == Inv.OrderID);
                                foreach (var z in ODetails.ToList())
                                {
                                    CashSettlementDetail csd = new CashSettlementDetail()
                                    {
                                        WarehouseID = Inv.WarehouseID,
                                        CashSettlementID = csID,
                                        ProductID = z.ProductID,
                                        SchemeID = (int)z.SchemeID,
                                        UnitPrice = (decimal)z.UnitPrice,
                                        AlternateUnitPrice = (decimal)z.AlternateUnitPrice,
                                        Quantity = (int)z.Quantity,
                                        AlternateQuantity = (int)z.AlternateQuantity,
                                        PlasticBoxQuantity = (int)z.PlasticBoxQuantity,
                                        RebateQuantity = (int)z.RebateQuantity,
                                        AlternateRebateQuantity = (int)z.AlternateRebateQuantity,
                                        AgencyCommission = (decimal)z.AgencyCommission,
                                        AlternateAgencyCommission = (decimal)z.AlternateAgencyCommission,
                                        SecurityDeposit = (decimal)z.SecurityDeposit,
                                        AlternateSecurityDeposit = (decimal)z.AlternateSecurityDeposit,
                                        PlasticBoxSecurity = (decimal)z.PlasticBoxSecurity,
                                        MRPRate = z.MRPRate

                                    };
                                    dc.CashSettlementDetails.Add(csd);
                                    var totpc = (from x in db.Products where x.ProductID == z.ProductID select x).FirstOrDefault();
                                    int TotQty = (int)(z.Quantity + ((z.AlternateQuantity + z.AlternateRebateQuantity) * totpc.ConversionFactor) + z.RebateQuantity);

                                    StockBatchDetail sbd = new StockBatchDetail()
                                    {
                                        WarehouseID = Inv.WarehouseID,
                                        TransactionNo = csID,
                                        TransactionDate = System.DateTime.Today,
                                        ProductID = z.ProductID,
                                        Quantity = TotQty,//(int)z.AlternateQuantity for rebate and case to pcs,
                                        PlasticBoxQuantity = z.PlasticBoxQuantity,
                                        TransactionType = "Invoice Creation",
                                        BatchNo = (int)z.BatchNo,
                                        CreateBy = User.Identity.Name,
                                        CreateDate = System.DateTime.Now,


                                    };
                                    dc.StockBatchDetails.Add(sbd);
                                    //var cc = (int)z.Quantity;
                                    //var xxx = (int)z.PlasticBoxQuantity;
                                    //var cc = CommonChk
                                    // CommonChk cm = new CommonChk();

                                    //int qty = cm.CasetoPcs(z.ProductID, (int)z.AlternateQuantity); // off for rebate
                                    int qty = TotQty;
                                    //int qty = cm.CasetoPcs(z.ProductID, TotQty); Farid Correction
                                    ProductTransactionDetail ptd = new ProductTransactionDetail();
                                    {
                                        ptd.TransactionNo = maxprodtr;
                                        ptd.WarehouseID = Inv.WarehouseID;
                                        ptd.ProductID = z.ProductID;
                                        ptd.Quantity = qty;// (int)z.AlternateQuantity;
                                        ptd.PlasticBoxQuantity = (int)z.PlasticBoxQuantity;
                                        //BatchNo = z.BatchNo,
                                        //ExpiryDate = z.ExpDate,
                                        //ManufactureDate = z.MfgDate,
                                        ptd.EmptyBottleQuantity = 0;
                                        ptd.BurstBottleQuantity = 0;
                                        ptd.BreakageBottleQuantity = 0;
                                        dc.ProductTransactionDetails.Add(ptd);

                                    };


                                    // Product Stock Maintaince

                                    // int allProductsQty;

                                    //var Pr = (from x in db.Products where x.ProductID == z.ProductID select x).FirstOrDefault();
                                    // allProductsQty = (int)(z.Quantity + ((z.AlternateQuantity + z.AlternateRebateQuantity) * Pr.ConversionFactor) + z.RebateQuantity);
                                    // SqlCommand com = new SqlCommand("spProductBalanceDecrease", con);
                                    // off for holding stock against loadsheet
                                    //var decreaseprodbal = db.spProductBalanceDecrease(z.ProductID,
                                    //    vr.WarehouseID,
                                    //    qty,
                                    //    0,
                                    //    0,
                                    //    0,
                                    //    0);
                                    var decreaseprodbal = db.spStockBookedandBookedFree(z.ProductID, vr.WarehouseID, qty, "D");


                                }
                                // Update Order
                                var upby = User.Identity.Name;
                                var orderdata = db.spOrderUpdateAfterInvoice(
                                    Inv.WarehouseID,
                                    Inv.OrderID,
                                    "Y",
                                    DateTime.Today,
                                    Inv.SalesAmount,
                                    Inv.SecurityAmount,
                                    Inv.RebateAmount,
                                    Inv.AgencyCommission,
                                    chNumber,
                                    upby,
                                    csID,
                                    "Y",
                                    Inv.FinishGoodsUnloadCharge,
                                    upby,
                                    DateTime.Now,
                                    Inv.AdvanceOrder
                              );
                                // con.Close();
                                //Transport QR Data Tauhid
                                //salesOrderNo will be OrderDate
                                try
                                {
                                    var getSONo = (from x in db.Orders where x.OrderID == Inv.OrderID select x).FirstOrDefault();

                                    SalesOrdNo = Convert.ToInt32(getSONo.OrderID);
                                    TransportQRData qd = db.TransportQRDatas.SingleOrDefault(x => x.SalesOrderNo == SalesOrdNo);
                                    qd.InvoiceDateTime = System.DateTime.Now;
                                    qd.DeliveryChallanNumber = chNumber;
                                    dc.Entry(qd).State = EntityState.Modified;
                                }
                                catch (Exception ex)
                                {


                                }
                                dc.SaveChanges();
                                //Invoice Print
                                var getSNo = (from x in db.Orders where x.ChallanNumber == chNumber select x).FirstOrDefault();
                                SalesOrdNo = Convert.ToInt32(getSNo.SalesOrderNo);
                                // SalesOrdNo = (int)Inv.SalesOrderNo;
                                //if (SalesOrdNo!=null)
                                //{

                                //    return RedirectToAction("Invoce", "OrderReport");
                                //}
                                status = true;
                                //int v = chNumber;
                                Dispose();
                                // status = true;
                                //return new JsonResult { Data = new { status = status, mes = mes } };
                            }
                            else
                            {
                                status = false;
                                v = 0;
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes, v = v, SNo = SalesOrdNo } };
            //return RedirectToAction("Invoice", "ReprintController", new { @InvNo = v });
        }


        public JsonResult GetOrderData(int orderID, int warehouseID)
        {
            // string CName;
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var V = db.spGetOrderList(orderID, warehouseID);//(from x in db.Orders where x.DeliveryChallanNumber == chNumber select x).FirstOrDefault();

                //foreach (var a in V)
                //{

                //}
                // CName=V.
                return new JsonResult { Data = V, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "CustomerID ID Not Found" });
                //throw ex;
            }
        }
        public JsonResult GetOrderDataDetail(int orderID, int warehouseID)
        {
            // string CName;
            try
            {
                // db.Configuration.ProxyCreationEnabled = false;
                var V = db.spGetOrderDataGrid(orderID, warehouseID).ToList();//(from x in db.Orders where x.DeliveryChallanNumber == chNumber select x).FirstOrDefault();
                return new JsonResult { Data = V, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "CustomerID ID Not Found" });
                //throw ex;
            }
        }
        public JsonResult GetAgencyFareAmount(int custID)
        {
            //  decimal rate1, rate2, rate3;

            try
            {

                var a = (from x in db.TransportFareSetups where x.CustomerID == custID && x.IsActive == 1 select x).FirstOrDefault();
                return new JsonResult { Data = a, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        public JsonResult CheckStockData(int warehouseID, int prID, int allCSqty, int allPSqty)
        {
            bool result = false;
            int totalQty;
            string Message = "";// = "your message here !";
                                // return Json(Message, JsonRequestBehaviour.AllowGet);

            db.Configuration.ProxyCreationEnabled = false;
            var V = (from x in db.ProductBalances where x.WarehouseID == warehouseID && x.ProductID == prID select x).FirstOrDefault();
            var W = (from x in db.Products where x.ProductID == prID select x).FirstOrDefault();
            //var V = (from x in db.ProductBalances where x.WarehouseID == warehouseID && prID.Contains(x.ProductID) select x).FirstOrDefault();
            totalQty = (allCSqty * W.ConversionFactor) + allPSqty;
            int onhandbook = (int)V.OnHandQuantity + (int)V.BookedQuantity;
            if (totalQty <= onhandbook)
            {
                result = true;
                //Message = "Success1";
                //return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                result = false;
                Message = V.ProductID + " Stock Qty are:" + onhandbook / W.ConversionFactor + " Cases";

            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            //con = new SqlConnection(constr);

        }
        public JsonResult SendSMStoDB(string phoneno, string sms)
        {
            bool status = false;
            string mes = "";

            HttpWebRequest request;
            HttpWebResponse response = null;
            String url;
            String host;

            if (ModelState.IsValid)
            {
                try
                {
                    //  var customerphone = db.Customers.Where(x=>x.CustomerID == )
                    status = true;
                    host = "https://vas.banglalinkgsm.com/sendSMS/sendSMS?msisdn=" + phoneno + "&message=" + sms + "&userID=" + "tranbevltd" + "&passwd=" + "92b034a909c050209caac7d5ccc0bf64" + "&sender=" + "8801969900240" + "";
                    url = host;
                    request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "sms Not Send" });
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };
        }
        //public JsonResult GetInvoiceNumberWareHouseWise(int OrdID)
        //{
        //    using (PEPSIEntities dc = new PEPSIEntities())
        //    {

        //        try
        //        {
        //            var invNumber = db.Orders.Where(x => x.OrderID == OrdID && x.Delivered == "Y");

        //            return new JsonResult { Data = invNumber, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { status = "error", message = "Customer Not Found" });
        //            // throw ex;
        //        }
        //    }
        //}

        //[HttpPost]
        //public ActionResult Invoice(int ordID)
        //{
        //    ReportViewer reportViewer = new ReportViewer
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(50),
        //        Height = Unit.Percentage(50)
        //    };
        //    try
        //    {
        //        //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
        //        List<spRPTInvoice_Result> invoiceData = db.spRPTInvoice(ordID).ToList();
        //        //List<spRPTInvoiceNew_Result> invoiceData = db.spRPTInvoiceNew(SOrderNo).ToList();

        //        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Reprint\Invoice.rdlc";
        //        //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

        //        //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

        //        ReportDataSource rdc = new ReportDataSource("InvoiceDataSet", invoiceData);
        //        reportViewer.LocalReport.DataSources.Add(rdc);

        //        reportViewer.LocalReport.Refresh();
        //        reportViewer.Visible = true;

        //        ViewBag.ReportViewer = reportViewer;
        //    }
        //    catch (Exception EX)
        //    {
        //        string message = EX.Message;
        //    }
        //    return PartialView("Invoice");

        //}

        //[HttpPost]
        //public void Invoice(int InvNo)
        //{
        //    ReportViewer reportViewer = new ReportViewer
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(50),
        //        Height = Unit.Percentage(50)
        //    };
        //    try
        //    {
        //        //var v = (from x in db.Warehouses where x.WarehouseID == wId select x).FirstOrDefault();
        //        List<spRPTInvoice_Result> invoiceData = db.spRPTInvoice(InvNo).ToList();
        //        //List<spRPTInvoiceNew_Result> invoiceData = db.spRPTInvoiceNew(SOrderNo).ToList();

        //        reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Reprint\Invoice.rdlc";
        //        //ReportParameter rp1 = new ReportParameter("wName", v.WarehouseDescription.ToString());

        //        //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });

        //        ReportDataSource rdc = new ReportDataSource("InvoiceDataSet", invoiceData);
        //        reportViewer.LocalReport.DataSources.Add(rdc);

        //        reportViewer.LocalReport.Refresh();
        //        reportViewer.Visible = true;

        //        ViewBag.ReportViewer = reportViewer;
        //    }
        //    catch (Exception EX)
        //    {
        //        string message = EX.Message;
        //    }
        //    Redirect("ReprintReport/Invoice");
        //   // return PartialView("Invoice");

        //}
    }
}