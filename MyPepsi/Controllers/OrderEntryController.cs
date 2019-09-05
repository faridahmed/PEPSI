using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class OrderEntryController : Controller
    {


        private PEPSIEntities db = new PEPSIEntities();
        // GET: OrderEntry
        public ActionResult Order()
        {
            //int v = 0;            

            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            //ViewBag.WarehouseID = new SelectList(db.Warehouses.Where(x=>x.WarehouseID==wn.WarehouseID).OrderBy(x => x.WarehouseID), "WarehouseID", "WarehouseDescription");
            ViewBag.CustomerID = new SelectList(db.Customers.Where(x => x.WarehouseID == wn.WarehouseID && x.ActiveStatus.ToUpper().Trim()=="A").OrderBy(x => x.CustomerName), "CustomerID", "CustomerName");
            //var maxSalesOrderNO = (from n in db.OrderBankSummaries
            //                       where n.SalesOrderNO == (db.OrderBankSummaries.Where(x => x.WarehouseID == wn.WarehouseID).Max(x => x.SalesOrderNO))
            //                       select new { n.SalesOrderNO }).FirstOrDefault();
            ViewBag.TotOrderCase = 0;
            ViewBag.TotPlasticBox = 0;
            //ViewBag.SalesOrderNO = (maxSalesOrderNO.SalesOrderNO) + 1;
            return View("Order");

        }
        [HttpPost]
        public ActionResult OrderEntrySave(OrderEntryVM D)
        {
            bool status = false;
            string mes = "";
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();

            //  var PTID = (from x in db.OrderBankSummaries select x.SalesOrderNO).DefaultIfEmpty(610000000).Max();
            //  PIDMax = (PTID + 1);
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "9000000");
            int OrderNo = Convert.ToInt32(s2);
            var maxSalesOrderNO = (from n in db.OrderBankSummaries where n.WarehouseID == w.WorkStationID select n.SalesOrderNO).DefaultIfEmpty(OrderNo).Max();

            // var batch = (from x in dc.StockBatches where x.ProductID == i.ProductID && x.WarehouseID == S.WarehouseID select x.BatchNo).DefaultIfEmpty(100).Max() + 1;
            //Max(x => x.SalesOrderNO))

            var maxsalno = maxSalesOrderNO + 1;
            int v = maxsalno;
            DateTime date2 = Convert.ToDateTime(D.ExpectDeliverDate);
            if (ModelState.IsValid)
            {
                using (PEPSIEntities dc = new PEPSIEntities())
                {

                    OrderBankSummary dbo = new OrderBankSummary
                    {

                        SalesOrderNO = maxsalno,
                        WarehouseID = D.WarehouseID,
                        CustomerID = D.CustomerID,
                        OrderCase = D.TotOrderCase,
                        OrderBox = D.TotOrderBox,
                        SalesOrderDate = DateTime.Today,
                        PartialDelivered = "N",
                        RefOrderNO = D.Remarks,
                        Status = "A",
                        Remarks = D.Remarks,
                        Delivered = "N",
                        ExpectDeliverDate = date2,// DateTime.Today.AddDays(1),
                        TotalOrderAmount = D.TotalOrderAmount,
                        EmptyBottleCase = D.EmptyBottleCase,
                        CreateDate = DateTime.Now,
                        CreateBy = User.Identity.Name

                    };
                    dc.OrderBankSummaries.Add(dbo);
                    foreach (var i in D.oderCustomer)
                    {
                        OrderBankDetail obd = new OrderBankDetail();
                        obd.WarehouseID = D.WarehouseID;
                        obd.SalesOrderNO = maxsalno;
                        obd.CustomerID = D.CustomerID;
                        obd.ProductID = i.ProductID;
                        obd.OrderCase = i.OrderCase;
                        obd.PlasticBox = i.PlasticBox;
                        obd.Amount = i.Amount;
                        //obd. = DateTime.Now.Date;
                        obd.CreateBy = User.Identity.Name;
                        obd.CreateDate = DateTime.Now;
                        dc.OrderBankDetails.Add(obd);
                    }
                    dc.SaveChanges();
                    status = true;
                    dc.Dispose();
                }
                ModelState.Clear();
            }
            else

            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes, v = v } };
        }
        //Product Description found against ID
        public JsonResult GetProductDesc(int idProduct)
        {
            string PName;

            try
            {
                // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Products where x.ProductID == idProduct && x.Status == "A" select x).FirstOrDefault();
                    PName = V.ProductDescription;
                }

                return new JsonResult { Data = PName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product Description Not Found" });
                // throw ex;
            }
        }
        //Get Plastic Box Status
        public JsonResult GetPlasticData(int idProduct)
        {
            int PB;
            try
            {

                var V = (from x in db.Products where x.ProductID == idProduct && x.Status == "A" select x).FirstOrDefault();

                PB = V.BottleReturnable;
                return new JsonResult { Data = PB, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                // throw ex;
            }
        }
        [HttpGet]
        public JsonResult getCustomerCurrentBalance(int pcustomerID)
        {
            decimal curbal = 0;
            try
            {
                var c = (from x in db.CustomerLedgerMasters where x.CustomerID == pcustomerID select x).FirstOrDefault();
                curbal = (decimal)c.CurrentBalance;
                return new JsonResult { Data = curbal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch
            {
                return Json(new { status = "error", message = "Customer ID is Not Found..." });
            }
        }
        [HttpGet]
        public JsonResult GetCustomerOutstanding(int custId)
        {
            using (PEPSIEntities dc = new PEPSIEntities())
            {
                // decimal TotalReceiveable = 0, TotalPayment = 0, TransitAmount = 0, TransitBottleAmount = 0, ActualOutstanding = 0, TotalCrLimit = 0;
                decimal amt = 0;
                decimal actualout = 0;
                try
                {

                    var ActualOutstanding = db.CustomerLedgerMasters.Where(x => x.CustomerID == custId).FirstOrDefault();
                    // actualout = ((decimal)ActualOutstanding.PaymentAmt + (decimal)ActualOutstanding.ReturnAmt) - (decimal)ActualOutstanding.InvoiceAmt;
                    amt = (decimal)ActualOutstanding.CurrentBalance + (decimal)ActualOutstanding.BookedAmt;
                    return new JsonResult { Data = amt, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Customer Outstanding" });
                    // throw ex;
                }
            }
        }
        [HttpGet]
        public JsonResult GetCustomerActualOutstanding(int custId)
        {
            using (PEPSIEntities dc = new PEPSIEntities())
            {

                decimal actualout = 0;
                try
                {
                    var ActualOutstanding = db.CustomerLedgerMasters.Where(x => x.CustomerID == custId).FirstOrDefault();
                    actualout = ((decimal)ActualOutstanding.PaymentAmt + (decimal)ActualOutstanding.ReturnAmt) - (decimal)ActualOutstanding.InvoiceAmt;

                    return new JsonResult { Data = actualout, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Customer Outstanding" });
                    // throw ex;
                }
            }
        }
        //Get Ordred Value
        [HttpGet]
        public JsonResult GetOrderValue(int custID, int idProduct, decimal tOrder, int totalPB)
        {
            decimal OAmount = 0;
            try
            {

                var V = (from x in db.Customers where x.CustomerID == custID select x).FirstOrDefault();
                //var W = (from y in db.ProductRateDetails
                //         where y.ProductRateID == V.ProductRateID && y.ProductID == idProduct
                //         select y).FirstOrDefault();

                var data = db.ProductRateDetails.Where(x => x.ProductRateID == V.ProductRateID && x.ProductID == idProduct);

                if (data != null)
                {
                    var x = data.ToList();
                    foreach (var W in x)
                    {
                        OAmount = ((tOrder * W.AlternateUnitPrice) - (tOrder * W.AlternateAgencyCommission)) + ((tOrder * W.AlternateSecurityDeposit) + (totalPB * W.PlasticBoxSecurity));
                    }
                }
                else
                {
                    OAmount = 0;
                }
                return new JsonResult { Data = OAmount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


                //return Json(new { success = true, Data = OAmount },
                //            JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                // throw ex;
            }
        }

        // Approval System
        public ActionResult Approval()
        {
            //int v = 0; 
            var maxApprovedID = (from n in db.OrderApproves
                                 where n.ApprovedID == (db.OrderApproves.Max(x => x.ApprovedID))
                                 select new { n.ApprovedID }).FirstOrDefault();
            ViewBag.WarehouseID = new SelectList(db.Warehouses.OrderBy(x => x.WarehouseID), "WarehouseID", "WarehouseDescription");
            ViewBag.CustomerID = new SelectList(db.Customers.OrderBy(x => x.CustomerName), "CustomerID", "CustomerName");
            ViewBag.TotalOrderCase = 0;
            ViewBag.TotalPlasticBox = 0;
            ViewBag.ApprovedID = (maxApprovedID.ApprovedID) + 1;
            return View("Approval");

        }
        //   [HttpGet]
        public JsonResult GetOrderList(int pCust)
        {
            var ordertList = db.OrderBankSummaries.Where(x => x.CustomerID == pCust && x.Status != "C").ToList();
            List<SelectListItem> c = new List<SelectListItem>();

            c.Add(new SelectListItem { Text = "--Select order--", Value = "0" });
            if (ordertList != null)
            {
                foreach (var l in ordertList)
                {
                    c.Add(new SelectListItem { Text = l.SalesOrderNO.ToString(), Value = l.SalesOrderNO.ToString() });

                }
            }
            return Json(new SelectList(c, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult GetCustomerList(int pWarehouse)
        {
            // var CustList = db.OrderBankSummaries.Where(x => x.WarehouseID == pWarehouse).Distinct().ToList();
            var CustList = (from x in db.Customers.Where(x => x.WarehouseID == pWarehouse)
                            join y in db.OrderBankSummaries on x.CustomerID equals y.CustomerID
                            select x).ToList();

            List<SelectListItem> c = new List<SelectListItem>();

            c.Add(new SelectListItem { Text = "0", Value = "0" });
            if (CustList != null)
            {
                foreach (var l in CustList)
                {
                    c.Add(new SelectListItem { Text = l.CustomerName.ToString(), Value = l.CustomerID.ToString() });

                }
            }
            return Json(new SelectList(c, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        [HttpPost]
        public ActionResult ApproveSave(OrderApproveVM R)
        {
            //private PEPSIEntities db = new PEPSIEntities();
            var maxApprovedID = (from n in db.OrderApproves
                                 where n.ApprovedID == (db.OrderApproves.Max(x => x.ApprovedID))
                                 select new { n.ApprovedID }).FirstOrDefault();
            int maxApp = (maxApprovedID.ApprovedID) + 1;
            bool status = false;
            string mes = "";
            int v = maxApp;// R.ApprovedID;
            try
            {
                OrderApprove oa = new OrderApprove();
                oa.WarehouseID = R.WarehouseID;
                oa.OrderID = R.OrderID;
                oa.ApprovedID = maxApp;// R.ApprovedID,
                oa.ApproveBy = User.Identity.Name;
                oa.ApproveDate = System.DateTime.Today;
                oa.Status = "A";
                db.OrderApproves.Add(oa);

                //Update Order Status
                var os = db.OrderBankSummaries.SingleOrDefault(x => x.SalesOrderNO == R.OrderID && x.WarehouseID == R.WarehouseID && x.CustomerID == R.CustomerID);
                os.Status = "A";
                db.SaveChanges();
                status = true;
                db.Dispose();
                return new JsonResult { Data = new { status = status, mes = mes, v = v } };
            }
            catch (Exception ex)
            {
                return Json(new { status = status, message = "Not Save" });
            }
        }
        [HttpGet]
        public JsonResult GetProductDetail(int psalesorderno)
        {

            try
            {

                var allOrderInfo = db.spGetSalesOrder(psalesorderno);

                return new JsonResult { Data = allOrderInfo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "SalesOrder No ID Not Found" });
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult CancelData(OrderApproveVM R)
        {
            //private PEPSIEntities db = new PEPSIEntities();
            var maxApprovedID = (from n in db.OrderApproves
                                 where n.ApprovedID == (db.OrderApproves.Max(x => x.ApprovedID))
                                 select new { n.ApprovedID }).FirstOrDefault();
            int maxApp = (maxApprovedID.ApprovedID) + 1;
            bool status = false;
            string mes = "";
            int v = maxApp;// R.ApprovedID;
            try
            {
                OrderApprove oa = new OrderApprove();
                oa.WarehouseID = R.WarehouseID;
                oa.OrderID = R.OrderID;
                oa.ApprovedID = maxApp;// R.ApprovedID,
                oa.ApproveBy = User.Identity.Name;
                oa.ApproveDate = System.DateTime.Today;
                oa.Status = "C";
                db.OrderApproves.Add(oa);

                //Update Order Status
                var os = db.OrderBankSummaries.SingleOrDefault(x => x.SalesOrderNO == R.OrderID && x.WarehouseID == R.WarehouseID && x.CustomerID == R.CustomerID);
                os.Status = "C";
                db.SaveChanges();
                status = true;
                db.Dispose();
                return new JsonResult { Data = new { status = status, mes = mes, v = v } };
            }
            catch (Exception ex)
            {
                return Json(new { status = status, message = "Not Save" });
            }
        }

    }
}