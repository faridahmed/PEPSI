using MyPepsi.InGeneral;
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
    public class ReplacementController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: Replacement
        public ActionResult Index()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            var cust = db.Customers.Where(x => x.WarehouseID == wn.WarehouseID && x.ActiveStatus == "A").Select(x => new { Text = x.CustomerName + " , " + x.CustomerID, Value = x.CustomerID }).ToList();
            ViewBag.CustomerID = new SelectList(cust, "Value", "Text");
            ViewBag.ReturnMethod = ViewBag.ClusterName = new SelectList(db.FixedDataTables.Where(x => x.Value == 999), "TypeCodeID", "TypeDescription");
            return View("ReplaceProcess");
        }
        [HttpGet]
        public JsonResult GetDataForReplace(int inWarehouse, int inCustomer, string inFromdate, string inTodate)
        {
            DateTime date1 = Convert.ToDateTime(inFromdate);
            DateTime date2 = Convert.ToDateTime(inTodate);
            try
            {

                var rtnInfo = db.spGetReturnProduct(inWarehouse, inCustomer, date1, date2);



                return new JsonResult { Data = rtnInfo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "SalesOrder No Not Found" });
                throw ex;
            }
        }

        public JsonResult GetCustomerSalesAmount(int inCustomer, string inFromdate, string inTodate)
        {
            DateTime date1 = Convert.ToDateTime(inFromdate);
            DateTime date2 = Convert.ToDateTime(inTodate);
            try
            {

                var salesamt = db.spGetCustomerSalesAmount(inCustomer, date1, date2, "B");



                return new JsonResult { Data = salesamt, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "SalesOrder No Not Found" });
                throw ex;
            }
        }
        public ActionResult ReplaceSave(ReplacementMasterVM D)
        {
            bool status = false;
            string mes = "";
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "2000000");
            int returnNo = Convert.ToInt32(s2);
            var maxreturnNO = (from n in db.ReplaceMasters where n.WarehouseID == w.WorkStationID select n.ReplaceNo).DefaultIfEmpty(returnNo).Max();
            var maxrNo = maxreturnNO + 1;
            int v = maxrNo;
            if (ModelState.IsValid)
            {
                using (PEPSIEntities dc = new PEPSIEntities())
                {

                    ReplaceMaster dbo = new ReplaceMaster();
                    {
                        dbo.ReplaceNo = maxrNo;
                        dbo.WarehouseID = D.WarehouseID;
                        dbo.CustomerID = D.CustomerID;
                        dbo.TransactionDate = DateTime.Today;
                        dbo.ProcessfromDate = D.ProcessfromDate;
                        dbo.ProcessToDate = D.ProcessToDate;
                        dbo.ReferenceNo = D.ReferenceNo;
                        dbo.TotReturnAmt = D.TotReturnAmt;
                        dbo.TotSalesAmt = D.TotSalesAmt;
                        dbo.TotPayableAmt = D.TotPayableAmt;
                        dbo.CalculationID = D.CalculationID;
                        dbo.Remarks = D.Remarks;
                        dbo.Status = "P";
                        dbo.CreateDate = DateTime.Now;
                        dbo.CreateBy = User.Identity.Name;

                    };
                    dc.ReplaceMasters.Add(dbo);
                    CustomerLedgerDetail cld = new CustomerLedgerDetail();
                    {
                        cld.WarehouseID = D.WarehouseID;
                        cld.CustomerID = D.CustomerID;
                        cld.TransactionNo = "PAY" + maxrNo.ToString();
                        cld.TransactionTypeID = 12;
                        cld.RefNumber1 = D.ReferenceNo.ToString();
                        cld.TransactionDate = DateTime.Today;
                        cld.CrAmount = D.TotPayableAmt;
                        cld.DrAmount = 0;
                        cld.DebitCredit = "C";
                        cld.Narration = "ReplacePayable Creation Amount#" + D.TotPayableAmt + " to Customer" + cld.CustomerID;
                        cld.CreateBy = User.Identity.Name;
                        cld.CreateDate = System.DateTime.Now;
                    }
                    dc.CustomerLedgerDetails.Add(cld);
                    foreach (var i in D.replacedetail)
                    {
                        ReplaceDetail obd = new ReplaceDetail();
                        obd.WarehouseID = D.WarehouseID;
                        obd.ReplacementNo = maxrNo;
                        obd.ReturnID = i.ReturnID;
                        obd.ProductID = i.ProductID;
                        obd.AlternateQuantity = i.AlternateQuantity;
                        obd.PlasticBoxQuantity = i.PlasticBoxQuantity;
                        obd.SubTotAmount = i.SubTotAmount;
                        dc.ReplaceDetails.Add(obd);
                        var updatereturn = db.spRelaceDataUpdate(i.ReturnID, D.WarehouseID, "", D.CustomerID, "R", User.Identity.Name, DateTime.Now);
                    }
                    var custReplacePayable = db.spCustomerBalanceUpdate(D.CustomerID, "T", D.TotPayableAmt);

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
        public ActionResult SalesReplace()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            var cust = db.Customers.Where(x => x.WarehouseID == wn.WarehouseID && x.ActiveStatus == "A").Select(x => new { Text = x.CustomerName + " , " + x.CustomerID, Value = x.CustomerID }).ToList();
            ViewBag.CustomerID = new SelectList(cust, "Value", "Text");
            ViewBag.ReturnMethod = ViewBag.ClusterName = new SelectList(db.FixedDataTables.Where(x => x.Value == 999), "TypeCodeID", "TypeDescription");
            return View("SalesReplace");
        }
        public ActionResult SalesReplaceSave(ReplacSalesVM D)
        {
            bool status = false;
            string mes = "";
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "2000000");
            int returnNo = Convert.ToInt32(s2);
            var maxreturnNO = (from n in db.ReplaceMasters where n.WarehouseID == w.WorkStationID select n.ReplaceNo).DefaultIfEmpty(returnNo).Max();
            var maxrNo = maxreturnNO + 1;
            int v = maxrNo;
            if (ModelState.IsValid)
            {
                using (PEPSIEntities dc = new PEPSIEntities())
                {

                    ReplaceMaster dbo = new ReplaceMaster();
                    {
                        dbo.ReplaceNo = maxrNo;
                        dbo.WarehouseID = D.WarehouseID;
                        dbo.CustomerID = D.CustomerID;
                        dbo.TransactionDate = DateTime.Today;
                        dbo.ProcessfromDate = D.ProcessfromDate;
                        dbo.ProcessToDate = D.ProcessToDate;
                        dbo.ReferenceNo = D.ReferenceNo;
                        dbo.TotReturnAmt = D.TotReturnAmt;
                        dbo.TotSalesAmt = D.TotSalesAmt;
                        dbo.TotPayableAmt = D.TotPayableAmt;
                        dbo.CalculationID = D.CalculationID;
                        dbo.Remarks = D.Remarks;
                        dbo.Status = "P";
                        dbo.CreateDate = DateTime.Now;
                        dbo.CreateBy = User.Identity.Name;

                    };
                    dc.ReplaceMasters.Add(dbo);
                    ReplaceDetail rd = new ReplaceDetail();
                    rd.WarehouseID = D.WarehouseID;
                    rd.ReturnID = D.CustomerID;
                    rd.ReplacementNo = maxrNo;
                    rd.ProductID = 0;
                    rd.SubTotAmount = 0;
                    rd.AlternateQuantity = 0;
                    rd.PlasticBoxQuantity = 0;
                    dc.ReplaceDetails.Add(rd);
                    CustomerLedgerDetail cld = new CustomerLedgerDetail();
                    {
                        cld.WarehouseID = D.WarehouseID;
                        cld.CustomerID = D.CustomerID;
                        cld.TransactionNo = "PAY" + maxrNo.ToString();
                        cld.TransactionTypeID = 12;
                        cld.RefNumber1 = D.ReferenceNo.ToString();
                        cld.TransactionDate = DateTime.Today;
                        cld.CrAmount = D.TotPayableAmt;
                        cld.DrAmount = 0;
                        cld.DebitCredit = "C";
                        cld.Narration = "ReplacePayable Creation Amount#" + D.TotPayableAmt + " to Customer" + cld.CustomerID;
                        cld.CreateBy = User.Identity.Name;
                        cld.CreateDate = System.DateTime.Now;
                    }
                    dc.CustomerLedgerDetails.Add(cld);
                    //foreach (var i in D.replacedetail)
                    //{
                    //    ReplaceDetail obd = new ReplaceDetail();
                    //    obd.WarehouseID = D.WarehouseID;
                    //    obd.ReplacementNo = maxrNo;
                    //    obd.ReturnID = i.ReturnID;
                    //    obd.ProductID = i.ProductID;
                    //    obd.AlternateQuantity = i.AlternateQuantity;
                    //    obd.PlasticBoxQuantity = i.PlasticBoxQuantity;
                    //    obd.SubTotAmount = i.SubTotAmount;
                    //    dc.ReplaceDetails.Add(obd);
                    //    var updatereturn = db.spRelaceDataUpdate(i.ReturnID, D.WarehouseID, "", D.CustomerID, "R", User.Identity.Name, DateTime.Now);
                    //}
                    var custReplacePayable = db.spCustomerBalanceUpdate(D.CustomerID, "T", D.TotPayableAmt);
                    var updatereplace = db.spRelaceDataUpdate(0, D.WarehouseID, "P", D.CustomerID, "M", User.Identity.Name, D.ProcessfromDate);

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
        public ActionResult ReplaceInvoice()
        {
            var w = (from yt in db.UserLogins
                     where yt.UserID.ToString() == User.Identity.Name
                     select new { yt.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(t => t.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            //ViewBag.ReplaceRefNo = (from x in db.ReplaceMasters where x.WarehouseID == w.WorkStationID && x.Status=="P" orderby x.ReplaceNo descending select x);
            var cust = db.Customers.Where(x => x.WarehouseID == wn.WarehouseID && x.ActiveStatus == "A").Select(x => new { Text = x.CustomerName + " , " + x.CustomerID, Value = x.CustomerID }).OrderBy(i => i.Text).ToList();
            ViewBag.CustomerID = new SelectList(cust, "Value", "Text");
            ViewBag.ReplaceRefNo = new SelectList(db.ReplaceMasters.Where(x => x.WarehouseID == w.WorkStationID && x.Status == "P").ToList(), "ReplaceNo", "CustomerID");
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "4000000");
            int ReplaceDisburseID = Convert.ToInt32(s2);
            var replaceDisburseID = (from n in db.ReplaceDisburseMasters where n.WarehouseID == w.WorkStationID select n.ReplaceDisburseID).DefaultIfEmpty(ReplaceDisburseID).Max();
            ViewBag.ReplaceDisburseID = replaceDisburseID + 1;
            return View("ReplaceInvoice");
        }
        [HttpGet]
        public JsonResult getReplaceRefNo(int inCustomer)
        {
            var data = db.ReplaceMasters.Where(c => c.CustomerID == inCustomer && c.Status == "P").Select(c => new { id = c.ReplaceNo, name = c.ReplaceNo + " , " + c.CustomerID }).ToList();
            return Json(data, "data", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult mgetReplaceRefNo(int inCustomer)
        {
            var data = db.ReplaceManuals.Where(c => c.CustomerID == inCustomer && c.Status == "P").Select(c => new { id = c.ReplaceID, name = c.ReplaceID + " , " + c.CustomerID }).ToList();
            return Json(data, "data", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getProductBatch(int inProduct, int inWarehouse)
        {

            var data = db.StockBatches.Where(x => x.ProductID == inProduct && x.WarehouseID == inWarehouse && x.Status == "A").Select(x => new { id = x.BatchNo, name = "Batch->" + x.BatchRefNo + " Mgf-> " + x.ManufacturDate.ToString() }).ToList();
            return Json(data, "data", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getPayableAmt(int inCustomer)
        {
            decimal payamt = 0;
            try
            {
                var clm = db.CustomerLedgerMasters.Where(x => x.CustomerID == inCustomer).FirstOrDefault();
                payamt = (decimal)clm.ReplacementPayable;
                return new JsonResult { Data = payamt, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Customer ID No Not Found" });
                throw ex;
            }
        }
        public JsonResult getProductDesc(int inProduct)
        {
            string PName;
            try
            {

                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Products where x.ProductID == inProduct && x.Status == "A" select x).FirstOrDefault();
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
        public JsonResult getPlasticData(int inProduct)
        {
            int PB;
            try
            {

                var V = (from x in db.Products where x.ProductID == inProduct && x.Status == "A" select x).FirstOrDefault();

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
        public JsonResult getOrderValue(int custID, int idProduct, decimal tOrder, int totalPB)
        {
            decimal OAmount = 0;
            try
            {
                var V = (from x in db.Customers where x.CustomerID == custID select x).FirstOrDefault();
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

            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                // throw ex;
            }
        }
        public ActionResult DisburseSave(ReplaceDisburseVM D)
        {
            bool status = false;
            string mes = "";
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "4000000");
            int replaceDisNo = Convert.ToInt32(s2);
            var maxreplaceNO = (from n in db.ReplaceDisburseMasters where n.WarehouseID == w.WorkStationID select n.ReplaceDisburseID).DefaultIfEmpty(replaceDisNo).Max();
            var maxrNo = maxreplaceNO + 1;
            int v = maxrNo;
            var maxprodtr = (from j in db.ProductTransactions where j.WarehouseID == w.WorkStationID select j.TransactionNo).Max() + 1;

            var customerData = (from i in db.Customers where (i.CustomerID == D.CustomerID) select i).FirstOrDefault();
            if (ModelState.IsValid)
            {
                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    CommonChk cm = new CommonChk();
                    ReplaceDisburseMaster dbo = new ReplaceDisburseMaster();
                    {
                        dbo.ReplaceDisburseID = maxrNo;
                        dbo.WarehouseID = D.WarehouseID;
                        dbo.CustomerID = D.CustomerID;
                        dbo.TransactionDate = DateTime.Today;
                        dbo.DriverID = D.DriverID;
                        dbo.VehicleID = D.VehicleID;
                        dbo.ReplaceRefNo = D.ReplaceRefNo;
                        dbo.TotDisburseAmt = D.TotDisburseAmt;
                        dbo.RemainingAmt = D.RemainingAmt;
                        dbo.LoadSheetNo = D.LoadSheetNo;
                        dbo.TotCase = D.TotCase;
                        dbo.TotBox = D.TotBox;
                        dbo.VehicleID = D.VehicleID;
                        dbo.Remarks = D.Remarks +"," + D.VehicleID;
                        dbo.CreateDate = DateTime.Now;
                        dbo.CreateBy = User.Identity.Name;

                    };
                    dc.ReplaceDisburseMasters.Add(dbo);
                    CustomerLedgerDetail cld = new CustomerLedgerDetail();
                    {
                        cld.WarehouseID = D.WarehouseID;
                        cld.CustomerID = D.CustomerID;
                        cld.TransactionNo = "DIS" + maxrNo.ToString();
                        cld.TransactionTypeID = 19;
                        cld.RefNumber1 = D.ReplaceDisburseID.ToString();
                        cld.TransactionDate = DateTime.Today;
                        cld.CrAmount = 0;
                        cld.DrAmount = D.TotDisburseAmt;
                        cld.DebitCredit = "D";
                        cld.Narration = "ReplaceDisburse Amount#" + D.TotDisburseAmt + " to Customer" + cld.CustomerID;
                        cld.CreateBy = User.Identity.Name;
                        cld.CreateDate = System.DateTime.Now;
                    }
                    dc.CustomerLedgerDetails.Add(cld);
                    ProductTransaction pt = new ProductTransaction();
                    {
                        pt.WarehouseID = D.WarehouseID;
                        pt.TransactionNo = maxprodtr;
                        pt.TransactionDate = DateTime.Today;
                        pt.TransactionTypeID = 4;
                        pt.FromWarehouse = D.WarehouseID;
                        pt.ReferenceNo = maxrNo.ToString();
                        //pt.Remarks = 
                        pt.Status = "D";
                        pt.CreatedBy = User.Identity.Name;
                        pt.CreatedDate = DateTime.Now;
                    }
                    dc.ProductTransactions.Add(pt);
                    foreach (var i in D.disbursedetail)
                    {
                        var iBatch = (from s in db.StockBatches
                                      where s.WarehouseID == D.WarehouseID
                                      && s.ProductID == i.ProductID
                                      && s.Status == "A"
                                      select s).FirstOrDefault();
                        int qty = cm.CasetoPcs(i.ProductID, (decimal)i.CaseQty);
                        ReplaceDisburseDetail obd = new ReplaceDisburseDetail();
                        {
                            var vr = db.ProductRateDetails.Where(x => x.ProductID == i.ProductID && x.ProductRateID == customerData.ProductRateID).FirstOrDefault();
                            obd.WarehouseID = D.WarehouseID;
                            obd.ReplaceDisburseID = maxrNo;
                            obd.CustomerID = D.CustomerID;
                            obd.ProductID = i.ProductID;
                            obd.CaseQty = i.CaseQty;
                            obd.BoxQty = i.BoxQty;
                            obd.BatchNo = i.BatchNo;
                            obd.ManufactureDate = iBatch.ManufacturDate;
                            obd.UnitPrice = vr.UnitPrice;
                            obd.AlternateUnitPrice = vr.AlternateUnitPrice;
                            obd.TotAmount = i.TotAmount;
                           
                            dc.ReplaceDisburseDetails.Add(obd);
                        }
                        StockBatchDetail sbd = new StockBatchDetail()
                        {
                            WarehouseID = D.WarehouseID,
                            TransactionNo = maxprodtr,
                            TransactionDate = System.DateTime.Today,
                            ProductID = i.ProductID,
                            Quantity = (int)i.CaseQty,
                            PlasticBoxQuantity = (int)i.BoxQty,
                            TransactionType = "Replace Disburse",
                            BatchNo = i.BatchNo,
                            CreateBy = User.Identity.Name,
                            CreateDate = System.DateTime.Now,


                        };
                        dc.StockBatchDetails.Add(sbd);

                        ProductTransactionDetail ptd = new ProductTransactionDetail();
                        {
                            ptd.TransactionNo = maxprodtr;
                            ptd.WarehouseID = D.WarehouseID;
                            ptd.ProductID = i.ProductID;
                            ptd.Quantity = qty;
                            ptd.PlasticBoxQuantity = (int)i.BoxQty;
                            ptd.EmptyBottleQuantity = 0;
                            ptd.BurstBottleQuantity = 0;
                            ptd.BreakageBottleQuantity = 0;
                            dc.ProductTransactionDetails.Add(ptd);

                        };
                        //int allProductsQty;

                        // var Pr = (from x in db.Products where x.ProductID == i.ProductID select x).FirstOrDefault();
                        //  allProductsQty = (int)(i.Quantity + ((z.AlternateQuantity + z.AlternateRebateQuantity) * Pr.ConversionFactor) + z.RebateQuantity);

                        var decreaseprodbal = db.spProductBalanceDecrease(i.ProductID,
                            D.WarehouseID,
                            qty,
                            0,
                            0,
                            (int)i.BoxQty,
                            0);
                    }

                    //{ 
                    //var replace = (from r in db.ReplaceMasters where r.WarehouseID == D.WarehouseID && r.ReplaceNo == D.ReplaceRefNo select r).FirstOrDefault();
                    //replace.Status = "R";
                    //}
                    var custReplacePayable = db.spCustomerBalanceUpdate(D.CustomerID, "A", D.TotDisburseAmt);
                    var updatereplace = db.spRelaceDataUpdate(D.ReplaceRefNo, D.WarehouseID, "C", D.CustomerID, "M", User.Identity.Name, DateTime.Now);
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
        public JsonResult CalculatePayableAmt(int inMethod, decimal inAmount)
        {
            decimal payamt = 0;
            decimal t;
            try
            {
                if (inMethod == 100002)
                {
                    t = (decimal)0.007;
                    payamt = Math.Round(inAmount * t, 2);
                }
                else if (inMethod == 100001)
                {
                    t = (decimal)0.01;
                    payamt = Math.Round(inAmount * t, 2);

                }
                return new JsonResult { Data = payamt, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Customer ID No Not Found" });
                throw ex;
            }
        }
        [HttpGet]
        public JsonResult getReplaceRefNoRemaks(int inCustomer, int inReplace)
        {

            var rep = db.ReplaceManuals.Where(c => c.CustomerID == inCustomer && c.ReplaceID == inReplace && c.Status == "P").FirstOrDefault();
            var data = rep.Remarks;
            return Json(data, "data", JsonRequestBehavior.AllowGet);
        }

    }
}