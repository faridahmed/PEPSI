using MyPepsi.InGeneral;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class LoadSheetController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: LoadSheet
        public ActionResult Index()
        {
            var w = (from yt in db.UserLogins
                     where yt.UserID.ToString() == User.Identity.Name
                     select new { yt.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(t => t.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;

            // var x = (from i in db.OrderBankSummaries where i.Status=="A" select i).FirstOrDefault();
            // var y = (from i in db.OrderBankDetails select i).FirstOrDefault(); 
            var InStatus = new string[] { "A", "P" }; // not show full loadsheet
            ViewBag.SalesOrderNO = new SelectList((from s in db.OrderBankSummaries
                                                   join cust in db.Customers
                                                   on s.CustomerID equals cust.CustomerID
                                                   where s.WarehouseID == cust.WarehouseID && s.WarehouseID == w.WorkStationID && InStatus.Contains(s.Status)
                                                   orderby s.SalesOrderNO descending
                                                   select new
                                                   {
                                                       SalesOrderNO = s.SalesOrderNO,
                                                       CustomerID = cust.CustomerName + " (" + s.CustomerID + ") " + s.SalesOrderNO
                                                   }),
                 "SalesOrderNO", "CustomerID", null);
            ViewBag.VehicleList = new SelectList(db.Vehicles.Where(x => x.WarehouseID == w.WorkStationID), "VehicleID", "VehicleRegistrationNo", "0");
            ViewBag.DriverID = new SelectList(db.Drivers.Where(x => x.WorkStation == w.WorkStationID && x.Status == "A").ToList(), "DriverID", "DriverName", "0");
            //ViewBag.SchemeList = new SelectList(db.RebateSchemes, "SchemeID", "SchemeDescription");
            // f ViewBag.OrdertypeList = new SelectList(db.OrderTypes, "OrderTypeID", "OrderTypeDescription");
            var myList = new List<SelectListItem>();
            myList.Add(new SelectListItem { Text = "Full", Value = "1" });
            myList.Add(new SelectListItem { Text = "Partial", Value = "2" });
            // add as many items as you want...

            ViewBag.OrdertypeList = myList;
            //ViewBag.WarehouseID = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            // f ViewBag.SalesPersonID = new SelectList(db.SalesPersons, "SalesPersonID", "SalesPersonName");
            //ViewBag.CustomerExecutiveID = new SelectList(db.CustomerExecutives, "CustomerExecutiveID", "CustomerExecutiveDescription");
            //var maxorderid = (from n in db.Orders
            //                  where n.OrderID == (db.Orders.Max(xo => xo.OrderID))
            //                  select new { n.OrderID }).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "8000000");
            int OrderNo = Convert.ToInt32(s2);
            //var orderID = (from n in db.Orders where n.WarehouseID == w.WorkStationID select n.OrderID).DefaultIfEmpty(OrderNo).Max();
            //ViewBag.OrderID = orderID + 1;

            return View();
        }
        [HttpPost]
        public ActionResult CreateLoadSheet(LoadSheetVM A)
        {
            var w = (from yt in db.UserLogins
                     where yt.UserID.ToString() == User.Identity.Name
                     select new { yt.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(t => t.WarehouseID == w.WorkStationID).FirstOrDefault();

            bool status = true;
            string mes = "";
            // int maxsl = 0;

            decimal unitprice = 0;
            string pstatus = "";
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "8000000");
            int OrderNo = Convert.ToInt32(s2);

            var orderID = (from n in db.Orders where n.WarehouseID == w.WorkStationID select n.OrderID).DefaultIfEmpty(OrderNo).Max();

            //var maxorderid = (from n in db.Orders
            //                  where n.OrderID == (db.Orders.Max(xo => xo.OrderID))
            //                  select new { n.OrderID }).FirstOrDefault();
            var customerData = (from i in db.Customers where (i.CustomerID == A.CustomerID) select i).FirstOrDefault();

            var mindeliverychallan = db.Orders
                                    .Where(p => p.WarehouseID == A.WarehouseID) // not consider salesorderno p.SalesOrderNo == A.SalesOrderNo &&
                                    .Min(p => p.ChallanNumber);
            //var maxsl = db.Orders
            //                    .Where(p => p.WarehouseID == A.WarehouseID)
            //                    .Max(p => p.OrderID);
            int orderno = orderID + 1;
            int v = orderno;
            // Checking challan negetive 
            if (mindeliverychallan >= 0)
            {
                mindeliverychallan = -1;
            }
            else
            {
                mindeliverychallan = mindeliverychallan - 1;
            }
            if (A.VehicleID == 1)
            {
                A.DriverID = 0;

            }
            if (ModelState.IsValid)
            {

                //  try
                //  {
                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var getorderdate = dc.OrderBankSummaries.Where(x => x.WarehouseID == A.WarehouseID && x.SalesOrderNO == A.SalesOrderNo).FirstOrDefault();
                    Order dbo = new Order
                    {
                        //Add orders table
                        //Here Id   Primary Key of DB table auto increase
                        // SLNo = maxsl,
                        SalesOrderNo = A.SalesOrderNo,
                        //CustomerExecutive = (A.CustomerExecutiveID),
                        OrderID = orderno,//A.OrderID,
                        //OrderDate = A.OrderDate,//DateTime.Today,
                        OrderDate = getorderdate.SalesOrderDate,
                        WarehouseID = A.WarehouseID,
                        DriverID = (int)A.DriverID,
                        CustomerID = A.CustomerID,
                        VehicleID = A.VehicleID,
                        SalesPersonID = customerData.SalesPersonID,
                        //Trip = 0,
                        //SalesAmount = 0,
                        //SecurityAmount = 0,
                        //RebateAmount = 0,
                        //AgencyCommission = 0,
                        Trip = 1,
                        OrderTypeID = A.OrderTypeID,
                        ProductRateID = customerData.ProductRateID,
                        CustomerExecutiveID = customerData.CustomerExecutiveID,// A.CustomerExecutiveID,
                        LoadSheetRemarks = A.LoadSheetRemarks,
                        Delivered = "N",
                        Cash = "N",
                        IsCashSettlementPrepared = "N",
                        Transit = "N",
                        Promotional = "N",
                        InvoiceFromVATInvoiceNumber = "N",
                        VATChallanRequired = "Y",
                        ReadyForDelivery = "Y",
                        AdvanceOrder = "N",
                        //RebateWillBeGivenWithOrder = A.RebateWillBeGivenWithOrder,
                        //SSMA_TimeStamp =Convert.ToByte(DateTime.Now),
                        //  SSMA_TimeStamp = null,
                        CreateBy = User.Identity.Name,
                        CreateDate = System.DateTime.Now

                    };

                    dc.Orders.Add(dbo);
                    //  status = true;

                    CommonChk c = new CommonChk();
                    foreach (var j in A.orderdetail)
                    {
                        //checking  stock balace                  
                        if (c.stockchk(j.ProductID, A.WarehouseID, (int)j.AlternateQuantity) == true)
                        {
                            //status = false;
                        }
                        else
                        {
                            status = false;
                        };
                    };
                    if (status == true)
                    {
                        foreach (var i in A.orderdetail)
                        {
                            // Add order detail 
                            OrderDetail dod = new OrderDetail();
                            // var d = (from x in db.spGetSalesOrder(A.SalesOrderNo) where x.ProductID == i.ProductID select x).FirstOrDefault();

                            var d = (from n in db.spGetSalesOrderNewProduct(A.SalesOrderNo, i.ProductID) where n.CustomerID == A.CustomerID select n).FirstOrDefault();
                            var batch = (from x in db.StockBatches where x.ProductID == i.ProductID && x.WarehouseID == A.WarehouseID && x.Status == "A" select x).FirstOrDefault();
                            // Id Primary key Auto generate
                            var q = (from x in db.Products where x.ProductID == i.ProductID select x).FirstOrDefault();
                            // unitprice = q.UnitPrice;

                            int s = q.ConversionFactor;
                            decimal qt = Math.Round((decimal)(i.AlternateQuantity) * s, 2);
                            decimal rqty = Math.Round((decimal)(i.AlternateRebateQuantity) * s, 2);
                            int quantity = (Convert.ToInt32(Math.Round(qt)) % s);
                            int casesqty = (Convert.ToInt32(Math.Round(qt)) / s);
                            int rebateqty = (Convert.ToInt32(Math.Round(rqty)) % s);
                            int rebatecase = (Convert.ToInt32(Math.Round(rqty)) / s);
                            int totbookedqty = (int)qt + (int)rqty;
                            dod.WarehouseID = A.WarehouseID;
                            dod.OrderID = orderno;// A.OrderID;
                            dod.ProductID = i.ProductID;
                            //dod.Quantity = i.Quantity;
                            dod.SchemeID = (int)i.SchemeID;
                            dod.Quantity = quantity;
                            dod.AlternateQuantity = casesqty;
                            dod.RebateQuantity = rebateqty;
                            dod.AlternateRebateQuantity = rebatecase;
                            dod.AgencyCommission = d.AgencyCommission;
                            dod.AlternateAgencyCommission = d.AlternateAgencyCommission;
                            dod.UnitPrice = d.UnitPrice;
                            dod.AlternateUnitPrice = d.AlternateUnitPrice;
                            dod.SecurityDeposit = d.SecurityDeposit;
                            dod.AlternateSecurityDeposit = d.AlternateSecurityDeposit;
                            dod.PlasticBoxQuantity = (int)i.PlasticBoxQuantity;
                            dod.PlasticBoxSecurity = d.PlasticBoxSecurity;
                            dod.BatchNo = batch.BatchNo;
                            dod.MfgDate = batch.ManufacturDate;
                            dod.ExpDate = batch.ExpiryDate;
                            dod.MRPRate = d.MRPRate;
                            dc.OrderDetails.Add(dod);

                            //Add scheme infomation
                            OrderSchemeDetail os = new OrderSchemeDetail();
                            os.OrderID = orderno;// A.OrderID;
                            os.ProductID = i.ProductID;
                            os.SchemeID = (int)i.SchemeID;
                            os.WarehouseID = A.WarehouseID;
                            dc.OrderSchemeDetails.Add(os);
                            // Booked Product
                            var bookedproduct = db.spStockBookedandBookedFree(
                                                     i.ProductID,
                                                    A.WarehouseID,
                                                    totbookedqty,
                                                    "B");
                        }

                    }

                    //db.SaveChanges();
                    //  status = true;
                    if (status == true)
                    {
                        //update                     

                        if (A.OrderTypeID == 1)
                        {
                            pstatus = "C";
                        }
                        else
                        {
                            pstatus = "P";
                        }
                        var ob = db.spOrderBankSumUpdate(A.SalesOrderNo, A.WarehouseID, pstatus);  //Where(x => x.SalesOrderNO == A.SalesOrderNo && x.WarehouseID == A.WarehouseID && x.CustomerID == A.CustomerID).FirstOrDefault();
                        var bookamtupdate = db.spCustomerBalanceUpdate(A.CustomerID, "B", A.SalesAmount);                                                                          // db.Entry(ob).State = EntityState.Modified;
                                                                                                                                                                                   //Transport QR Data                                                                         //Ad QR LoadSheet Timing                                                                       //Add scheme infomation
                        try
                        {
                            TransportQRData qd = db.TransportQRDatas.SingleOrDefault(x => x.SLNO == A.QRSL);
                            qd.LoadingDateTime = System.DateTime.Now;
                            qd.SalesOrderNo = orderno;
                            dc.Entry(qd).State = EntityState.Modified;
                        }
                        catch (Exception ex)
                        {


                        }
                        dc.SaveChanges();
                        status = true;
                        dc.Dispose();
                    }
                    else
                    {
                        status = false;
                        v = 0000;
                    }
                }
            }
            // catch (Exception ex)
            // {

            //   return new JsonResult { Data = new { status = false, mes = "NO Save", v = v } };
            //  return JsonResult { Data = new { status = "error", message = "Not Found" }};
            //}
            return new JsonResult { Data = new { status = status, mes = mes, v = v } };
        }
        public JsonResult GetgrossCalculation(int custID, int PrID, decimal orderqty, int plasticBox, decimal rbtQty)
        {
            try
            {
                decimal orderValue = 0;
                // decimal agencycommision = 0;
                int rateID;
                // List<ProductRateDetailVM> prDetail
                db.Configuration.ProxyCreationEnabled = false;
                var RateID = (from x in db.Customers where x.CustomerID == custID select x).FirstOrDefault();
                rateID = Convert.ToInt32(RateID.ProductRateID);
                var result = (from x in db.ProductRateDetails where x.ProductRateID == rateID && x.ProductID == PrID select x).FirstOrDefault();
                orderValue = Convert.ToDecimal(((orderqty * result.AlternateUnitPrice) - (orderqty * result.AlternateAgencyCommission)) + ((orderqty + rbtQty) * result.AlternateSecurityDeposit) + (plasticBox * result.PlasticBoxSecurity));
                //var genericResult = result.AlternateAgencyCommission* qty;
                //agencycommision = result.AlternateAgencyCommission * rbtQty;
                return new JsonResult { Data = orderValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Not Found" });

            }
        }
        public JsonResult getProductDesc(int PrID)
        {
            try
            {

                string prodesc = "N/A";
                decimal price = 0;
                var prod = (from x in db.Products where x.ProductID == PrID select x).FirstOrDefault();
                prodesc = prod.ProductDescription;
                price = prod.AlternateUnitPrice;

                // return new JsonResult { Data = orderValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                return new JsonResult { Data = new { prodesc = prodesc, price = price }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Not Found" });

            }
        }
        [HttpGet]
        public JsonResult GetProductDetail(int psalesorderno)
        {

            try
            {

                var allOrderInfo = db.spGetSalesOrderSum(psalesorderno);
                // var allOrderInfo = db.spGetSalesOrder(psalesorderno);     

                return new JsonResult { Data = allOrderInfo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "SalesOrder No ID Not Found" });
                throw ex;
            }
        }
        [HttpGet]
        public JsonResult GetCustomerOutstanding(int custId)
        {
            using (PEPSIEntities dc = new PEPSIEntities())
            {
                // decimal TotalReceiveable = 0, TotalPayment = 0, TransitAmount = 0, TransitBottleAmount = 0, ActualOutstanding = 0, TotalCrLimit = 0;
                decimal amt = 0;
                try
                {
                    // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.
                    //var outstanding = db.GetCustomerCurrentOutstanding(custId);
                    var ActualOutstanding = db.CustomerLedgerMasters.Where(x => x.CustomerID == custId).FirstOrDefault();
                    decimal bookamt = (decimal)ActualOutstanding.BookedAmt;
                    if (bookamt >= 0)
                    {
                        amt = (decimal)ActualOutstanding.CurrentBalance + (decimal)ActualOutstanding.BookedAmt;
                    }
                    else
                    {
                        amt = (decimal)ActualOutstanding.CurrentBalance;
                    }
                    //foreach (var i in outstanding)
                    //{
                    //    TotalReceiveable = Convert.ToDecimal((i.TotalAmount) + ((i.FinishGoodsUnloadCharge + i.EmptyReturnLoadCharge) - i.ReturnFGCharge));
                    //    TotalPayment = Convert.ToDecimal(i.PaymentReceived);
                    //    TransitAmount = Convert.ToDecimal(i.TotalTransitAmount);
                    //    TransitBottleAmount = Convert.ToDecimal(i.TransitBottleSecurityAmount);
                    //    TotalCrLimit = Math.Round(Convert.ToDecimal(TotalCrLimit) + (Convert.ToDecimal(i.RcvlAmnt)), 2);
                    //    //off TotalCrLimit = Convert.ToDecimal(TotalCrLimit+i.RcvlAmnt);
                    //    ActualOutstanding = Math.Round(Convert.ToDecimal((((TotalReceiveable + TransitAmount + TransitBottleAmount) - TotalPayment)) + (-TotalCrLimit)), 2);

                    //}

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
        public JsonResult getAmount(decimal pAmt)
        {
            // amounttoword a = new amounttoword();
            decimal am = 0;
            am = pAmt;
            return new JsonResult { Data = am, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        // Dropdown for Batch
        [HttpPost]
        public ActionResult GetBatch(int inproduct)
        {
            //List<SelectListItem> drop = new List<SelectListItem>
            //{
            //    new SelectListItem{Value="Superman",Text="Superman"},
            //     new SelectListItem{Value="Batman",Text="Batman"},
            //      new SelectListItem{Value="Wonderwoman",Text="Wonderwoman"}
            //};

            // var batchlist = db.StockBatches.Where(x => x.ProductID == inproduct).ToList();
            var batchlist = (from x in db.StockBatches where x.ProductID == inproduct select x).ToList();
            return Json(batchlist);
        }
        //[HttpGet]
        //public ActionResult GetWarehouseWiseBatch(int warehouseID, int productID)
        //{
        //    var data = db.StockBatches.Where(x => x.WarehouseID == warehouseID && x.ProductID == productID).Select(x => new { id = x.BatchNo, name = x.BatchRefNo }).ToList();

        //    return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        public JsonResult getStock(int ProductId, int pWarehouse, int pCases)
        {
            int StockBalance = 0;
            int OnHandQty = 0;
            double conversionFactor = 0;
            double piecesToCase = 0;

            try
            {
                if (pWarehouse != 0)
                {
                    var P = (from x in db.Products where x.ProductID == ProductId select x).FirstOrDefault();
                    var PB = (from x in db.ProductBalances where x.ProductID == ProductId && x.WarehouseID == pWarehouse select x).FirstOrDefault();
                    OnHandQty = PB.OnHandQuantity;
                    conversionFactor = Convert.ToDouble(P.ConversionFactor);
                    piecesToCase = (pCases / conversionFactor);
                    piecesToCase = Math.Round(piecesToCase, 2);
                    StockBalance = OnHandQty;
                    return new JsonResult { Data = new { StockBalance = StockBalance, piecesToCase = piecesToCase }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                else
                {
                    var P = (from x in db.Products where x.ProductID == ProductId select x).FirstOrDefault();
                    conversionFactor = Convert.ToDouble(P.ConversionFactor);
                    piecesToCase = (pCases / conversionFactor);
                    return new JsonResult { Data = new { StockQuantity = 0, piecesToCase = piecesToCase }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product Id not found .. Pls Check..!!" });
                //throw ex;
            }
        }
        public JsonResult GetProdInfo(int idProduct)
        {
            //  decimal rate1, rate2, rate3;

            try
            {

                var d = (from x in db.Products where x.ProductID == idProduct && x.Status == "A" select x).FirstOrDefault();


                var partialResult = (from c in db.Products
                                     join o in db.StockBatches

                                     on c.ProductID equals o.ProductID
                                     where c.ProductID == idProduct
                                     select new
                                     {
                                         c.ProductID,
                                         c.ProductDescription,
                                         o.PlantLineNo,
                                         o.WarehouseID,
                                         o.ExpiryDate,
                                         o.ManufacturDate,
                                         o.BatchNo
                                     }).FirstOrDefault();// OrderBy(m => m.ManufacturDate.Month).ThenBy(y => y.ManufacturDate.Year)
                                                         //var finalResult = from c in db.Products
                                                         //                  orderby c.ProductDescription
                                                         //                  select new
                                                         //                  {
                                                         //                      pid = c.ProductID,
                                                         //                      pname = c.ProductDescription,
                                                         //                      pbatch = c.
                                                         //                      list = (from r in partialResult where c.ProductDescription == r.ProductDescription select r.ManufacturDate).ToList()

                //                  };

                //foreach (var item in finalResult)
                //{

                //    Console.WriteLine(item.name);
                //    if (item.list.Count == 0)
                //    {
                //        Console.WriteLine("No orders");
                //    }
                //    else
                //    {
                //        foreach (var i in item.list)
                //        {
                //            Console.WriteLine(i);
                //        }
                //    }
                //}
                // var a = partialResult;

                return new JsonResult { Data = partialResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }

        [HttpGet]
        public JsonResult ProductToLine(int idProduct, int idWarehouse)
        {


            //var data = db.StockBatches.Where(x => x.ProductID == idProduct).Select(x => new { date = x.ManufacturDate, name = x.BatchNo + "[" + x.ManufacturDate + "]" }).ToList();
            var data = db.StockBatches.Where(x => x.ProductID == idProduct && x.WarehouseID == idWarehouse).Select(x => new { id = x.BatchNo, name = x.BatchRefNo }).ToList();



            // return Json(data, "data", JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult PartialChecking(int inWarehouseID, int inCustomerID, int inSalesOrderNo)
        {
            //decimal totOrder = 0;

            // int remOrderCase = 0;         
            // var obs = db.OrderBankSummaries.Where(x => x.WarehouseID == inWarehouseID && x.CustomerID == inCustomerID && x.SalesOrderNO== inSalesOrderNo).FirstOrDefault();
            //totOrder = (decimal)obs.OrderCase;
            var remcs = db.spGetRemainingCsQtyForSalesOrderNo(inWarehouseID, inCustomerID, inSalesOrderNo);
            // remOrderCase = 
            return new JsonResult { Data = remcs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CheckStockDataProductWise(int warehouseID, int prID, int allCSqty)
        {
            bool result = false;
            int totalQty;
            string Message = "";
            //db.Configuration.ProxyCreationEnabled = false;
            var V = (from x in db.ProductBalances where x.WarehouseID == warehouseID && x.ProductID == prID select x).FirstOrDefault();
            var W = (from x in db.Products where x.ProductID == prID select x).FirstOrDefault();
            //var V = (from x in db.ProductBalances where x.WarehouseID == warehouseID && prID.Contains(x.ProductID) select x).FirstOrDefault();
            totalQty = (allCSqty * W.ConversionFactor);
            if (totalQty <= V.OnHandQuantity)
            {
                result = true;
            }
            else
            {
                result = false;
                Message = "Product ID " + V.ProductID + "-" + W.ProductDescription + " Stock Qty is : " + V.OnHandQuantity / W.ConversionFactor + " Cases,Please Check stock..!!";

            }
            return Json(Message, JsonRequestBehavior.AllowGet);
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
    }
}