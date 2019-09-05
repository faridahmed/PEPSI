using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPepsi.InGeneral;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        // GET: Stock
        private PEPSIEntities db = new PEPSIEntities();
        private SqlConnection con;
        private int TrNo;
        private int TrID;
        public ActionResult ProductTransaction()
        {

            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes.Where(x => x.UsedType == "P" && x.Status == "A"), "TransactionTypeID", "TransactionTypeName");
            // ViewBag.FWarehouse = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.TWarehouse = new SelectList(db.Warehouses.Where(x => x.WarehouseID != wn.WarehouseID && x.WarehouseID != 100), "WarehouseID", "WarehouseDescription");
            ViewBag.Vehicle = new SelectList(db.Vehicles.Where(x => x.WarehouseID == wn.WarehouseID), "VehicleID", "VehicleRegistrationNo", "0");
            ViewBag.Driver = new SelectList(db.Drivers.Where(x => x.WorkStation == wn.WarehouseID), "DriverID", "DriverName", "0");
            ViewBag.PlantLineNo = new SelectList(db.FixedDataTables.Where(y => y.Value == 100 && y.Status == "A"), "TypeCodeID", "TypeDescription");
            ViewBag.TrBatchNo = new SelectList(db.FixedDataTables.Where(y => y.Value == 100 && y.Status == "A"), "TypeCodeID", "TypeDescription");
            ViewBag.TransportAgencyList = new SelectList(db.TransportAgencySetups.Where(x => x.EligibleForCarringGoods == 1), "TAID", "TAName");
            ViewBag.StorageLocationCode = new SelectList(db.StorageLocations.Where(c => c.WarehouseID == wn.WarehouseID), "WarehouseLocationCode", "ShortDescription");
            //return View();
            return View(new StockVM());

        }
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            //string constr = ConfigurationManager.ConnectionStrings["PEPSIEntities"].ToString();
            con = new SqlConnection(constr);

        }

        [HttpPost]
        public ActionResult SaveData(StockVM S)
        {

            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();

            //  var PTID = (from x in db.OrderBankSummaries select x.SalesOrderNO).DefaultIfEmpty(610000000).Max();
            //  PIDMax = (PTID + 1);
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "0000000");
            string s3 = string.Concat(s1 + Convert.ToString(DateTime.Now.Year) + "000");
            string plantfl = wn.PlantFlag;
            int batchno = Convert.ToInt32(s3);
            int Trno = Convert.ToInt32(s2);
            bool status = true;
            string mes = "";
            int PIDMax;
            var PTID = (from x in db.ProductTransactions where x.WarehouseID == wn.WarehouseID select x.TransactionNo).DefaultIfEmpty(Trno).Max();
            PIDMax = (PTID + 1);
            int v = PIDMax;
            if (ModelState.IsValid)
            {
                try
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {
                        CommonChk ck = new CommonChk();
                        ProductTransaction dm = new ProductTransaction();
                        {
                            // Here Id   Primary Key of DB table auto increase
                            //var PTID = db.ProductTransactions.DefaultIfEmpty(100).Max(p => p.TransactionNo);
                            dm.TransactionTypeID = S.TransactionTypeID;
                            dm.TransactionNo = PIDMax;//
                            dm.WarehouseID = S.WarehouseID;
                            dm.FromWarehouse = S.WarehouseID;
                            dm.ToWarehouse = S.ToWarehouse;
                            dm.TransactionDate = DateTime.Today;
                            dm.ReferenceNo = S.ReferenceNo;
                            dm.Remarks = S.Remarks;
                            if (S.TransactionTypeID != 1)
                            {
                                if (S.TransactionTypeID == 1)
                                {
                                    dm.Status = "T";
                                }
                                if (S.TransactionTypeID == 6)
                                {
                                    dm.Status = "D";
                                }
                                if (S.TransactionTypeID == 10)
                                {
                                    dm.Status = "B";
                                }
                                if (S.TransactionTypeID == 11)
                                {
                                    dm.Status = "W";
                                }
                            }
                            else
                            {
                                dm.Status = "A";
                            }
                            //cuser = User.Identity.Name
                            dm.CreatedBy = User.Identity.Name;
                            dm.CreatedDate = DateTime.Now;
                            dm.StoreLocation = S.StoreLocation;
                        }
                        dc.ProductTransactions.Add(dm);
                        // Transfer Note
                        if (S.TransactionTypeID == 2)
                        {
                            ProductTransferNote ptn = new ProductTransferNote();
                            {

                                var PTNoteID = (from x in db.ProductTransferNotes where x.WarehouseID == w.WorkStationID select x.SLNo).DefaultIfEmpty(1).Max();
                                int TrNoteId = Convert.ToInt32((PTNoteID)) + 1;
                                ptn.WarehouseID = S.WarehouseID;
                                ptn.TransactionNo = PIDMax;
                                ptn.VehicleID = S.VehicleID;
                                if (S.VehicleID == 1)
                                {
                                    ptn.DriverID = 0;
                                }
                                else
                                {
                                    ptn.DriverID = S.DriverID;
                                }
                                ptn.Remarks = S.Remarks;
                                ptn.SLNo = TrNoteId;
                                ptn.CreateBy = User.Identity.Name;
                                ptn.CreateDate = DateTime.Now;
                            }
                            dc.ProductTransferNotes.Add(ptn);
                        }
                        // Agency Fare Amount
                        if (S.VehicleID == 1 && S.TransactionTypeID == 2)
                        {
                            TransportAgencyandFareSetup tf = new TransportAgencyandFareSetup();


                            {
                                tf.WarehouseID = S.WarehouseID;
                                tf.TAID = (int)S.TAID;
                                tf.ChallanNumber = PIDMax;
                                tf.ChallanDate = DateTime.Today;
                                tf.CustomerName = ck.GetCustomerName((int)S.ToWarehouse, "N");
                                tf.Address = ck.GetCustomerName((int)S.ToWarehouse, "N");
                                tf.VechileNo =S.VehicleID.ToString();
                                tf.FareAmnt = (decimal)S.FareAmnt;
                                tf.Remarks = "Hired Truck";
                                tf.TotalCases = S.TotCase;
                                tf.Status = "No";
                                tf.EnteredBy = User.Identity.Name;
                                tf.EnteredDate = System.DateTime.Now;
                            }
                            dc.TransportAgencyandFareSetups.Add(tf);
                        }

                        if (S.TransactionTypeID == 2)
                        {
                            foreach (var j in S.ProdTrDtl)
                            {
                                //checking  stock balace                  
                                if (ck.stockchkcs(j.ProductID, S.WarehouseID, (int)j.Quantity) == true)
                                {
                                    //status = false;
                                }
                                else
                                {
                                    status = false;
                                };
                            };
                        }
                        if (status == true)
                        {
                            foreach (var i in S.ProdTrDtl)
                            {

                                var CF = (from x in dc.Products where x.ProductID == i.ProductID select x).FirstOrDefault();
                                var batch = (from x in dc.StockBatches where x.ProductID == i.ProductID && x.WarehouseID == S.WarehouseID select x.BatchNo).DefaultIfEmpty(batchno).Max() + 1;
                                ProductTransactionDetail dmd = new ProductTransactionDetail();
                                dmd.ProductID = i.ProductID; // Not Null
                                dmd.WarehouseID = S.WarehouseID;
                                dmd.TransactionNo = PIDMax;
                                dmd.LineNoPlant = i.LineNoPlant;
                                if (S.TransactionTypeID != 1)
                                {
                                    dmd.BatchNo = i.BatchNo;
                                }
                                else
                                {
                                    dmd.BatchNo = batch;
                                }

                                if (plantfl == "Y")
                                {
                                    int plant = (int)wn.PlantNo;
                                    dmd.PlantNo = plant;
                                }
                                dmd.ManufactureDate = i.ManufactureDate;
                                //DateTime d = Convert.ToDateTime(i.ManufactureDate);
                                if (CF.Expirydays == null)
                                {
                                    dmd.ExpiryDate = i.ManufactureDate;
                                }
                                else
                                {
                                    dmd.ExpiryDate = Convert.ToDateTime(i.ManufactureDate).AddDays((int)CF.Expirydays);// + 10;//  (int)CF.Expirydays;  //i.ExpiryDate;
                                }
                                dmd.Quantity = (i.Quantity);
                                dmd.EmptyBottleQuantity = (i.EmptyBottleQuantity);
                                dmd.PlasticBoxQuantity = i.PlasticBoxQuantity;
                                dmd.BurstBottleQuantity = (i.BurstBottleQuantity);
                                dmd.BreakageBottleQuantity = (i.BreakageBottleQuantity);
                                //TCases = TCases + i.Quantity;
                                dc.ProductTransactionDetails.Add(dmd);
                                // For Batch                      
                                StockBatch sb = new StockBatch();
                                sb.ProductID = i.ProductID;
                                sb.WarehouseID = S.WarehouseID;
                                //sb.BatchNo = batch;

                                if (S.TransactionTypeID != 1)
                                {
                                    var t = db.StockBatches.Where(x => x.WarehouseID == S.WarehouseID && x.ProductID == i.ProductID && x.BatchNo == i.BatchNo).FirstOrDefault();
                                    sb.BatchRefNo = t.BatchRefNo;
                                    sb.ManufacturDate = t.ManufacturDate;
                                    sb.ExpiryDate = t.ExpiryDate;
                                    sb.PlantLineNo = t.PlantLineNo;
                                    sb.BatchNo = (int)i.BatchNo; // Add
                                    sb.PlantNo = t.PlantNo;
                                    if (S.TransactionTypeID == 2)
                                    {
                                        sb.Status = "T";
                                    }
                                    if (S.TransactionTypeID == 6)
                                    {
                                        sb.Status = "D";
                                    }
                                    if (S.TransactionTypeID == 10)
                                    {
                                        sb.Status = "B";
                                    }
                                    if (S.TransactionTypeID == 11)
                                    {
                                        sb.Status = "W";
                                    }
                                }
                                else
                                {
                                    sb.ManufacturDate = (DateTime)i.ManufactureDate;
                                    sb.ExpiryDate = Convert.ToDateTime(i.ManufactureDate).AddDays((int)CF.Expirydays); //(DateTime)i.ExpiryDate;
                                    sb.PlantLineNo = (int)i.LineNoPlant;
                                    sb.PlantNo = i.PlantNo;
                                    sb.BatchRefNo = i.BatchNo;
                                    sb.BatchNo = batch;  // Add
                                    sb.WarehouseLocationCode = S.StoreLocation;
                                    sb.Status = "A";
                                }
                                sb.IssueQuantity = (decimal)i.Quantity;
                                sb.ReceivedQty = i.Quantity;
                                dc.StockBatches.Add(sb);
                                // Add stock batch detail for transaction history
                                StockBatchDetail sbd = new StockBatchDetail();
                                sbd.WarehouseID = S.WarehouseID;
                                sbd.ProductID = i.ProductID;
                                sbd.TransactionNo = PIDMax;
                                sbd.Quantity = i.Quantity;
                                sbd.BurstBottleQuantity = i.BurstBottleQuantity;
                                sbd.BreakageBottleQuantity = i.BreakageBottleQuantity;
                                sbd.PlasticBoxQuantity = (int)i.PlasticBoxQuantity;
                                sbd.TransactionDate = DateTime.Today;
                                sbd.CreateBy = User.Identity.Name;
                                sbd.CreateDate = DateTime.Now;
                                if (S.TransactionTypeID == 2)
                                {
                                    sbd.BatchNo = i.BatchNo;
                                    sbd.TransactionType = "Product Transfer";
                                }
                                else if (S.TransactionTypeID == 10)
                                {
                                    sbd.BatchNo = i.BatchNo;
                                    sbd.TransactionType = "Warehouse Burst";

                                }
                                else if (S.TransactionTypeID == 11)
                                {
                                    sbd.BatchNo = i.BatchNo;
                                    sbd.TransactionType = "Write Off";

                                }
                                else if (S.TransactionTypeID == 6)
                                {
                                    sbd.BatchNo = i.BatchNo;
                                    sbd.TransactionType = "Adjustment";

                                }
                                else
                                {
                                    sbd.TransactionType = "Product Received";
                                    sbd.BatchNo = batch;
                                }
                                dc.StockBatchDetails.Add(sbd);

                            }
                        }

                        //db.SaveChanges();
                        // dc.SaveChanges();
                        foreach (var i in S.ProdTrDtl)
                        {
                            var CF = (from x in dc.Products where x.ProductID == i.ProductID select x).FirstOrDefault();
                            try
                            {
                                if (S.TransactionTypeID == 1)
                                {

                                    var productupdate = dc.spProductBalanceUpdate(i.ProductID,
                                                                                S.WarehouseID,
                                                                                (int)i.Quantity, // change
                                                                                i.BurstBottleQuantity,
                                                                                i.BreakageBottleQuantity,
                                                                                (int)i.PlasticBoxQuantity, //change
                                                                                i.EmptyBottleQuantity);

                                }

                                if (S.TransactionTypeID !=1 && status == true)
                                {
                                    //Deduct Stock
                                    var productupdate = dc.spProductBalanceDecrease(i.ProductID,
                                                                                  S.WarehouseID,
                                                                                  (int)i.Quantity, //change
                                                                                  i.BurstBottleQuantity,
                                                                                  i.BreakageBottleQuantity,
                                                                                  (int)i.PlasticBoxQuantity,
                                                                                  i.EmptyBottleQuantity);



                                }

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                        }
                        if (status == true)
                        {
                            dc.SaveChanges();
                            status = true;
                            dc.Dispose();
                        }
                    }
                    // return new JsonResult { Data = new { status = status, mes = mes } };
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
            return new JsonResult { Data = new { status = status, mes = mes, v = v } };//, v = v } };
        }

        public JsonResult GetProductData(int idProduct)
        {
            string PName;

            try
            {
                // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Products where x.ProductID == idProduct select x).FirstOrDefault();
                    PName = V.ProductDescription;

                }

                return new JsonResult { Data = PName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        [HttpGet]
        public JsonResult GetProdInfo(int idProduct, int iWarehouse)
        {
            //  decimal rate1, rate2, rate3;

            try
            {

                var d = (from x in db.Products where x.ProductID == idProduct && x.Status == "A" select x).FirstOrDefault();


                var partialResult = (from c in db.Products
                                     join o in db.StockBatches
                                     on c.ProductID equals o.ProductID
                                     where c.ProductID == idProduct
                                     && o.WarehouseID == iWarehouse
                                     select new
                                     {
                                         c.ProductID,
                                         c.ProductDescription,
                                         o.PlantLineNo,
                                         o.WarehouseID,
                                         o.ExpiryDate,
                                         o.ManufacturDate,
                                         o.BatchNo

                                     }).FirstOrDefault();


                return new JsonResult { Data = partialResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        [HttpGet]
        public JsonResult ProductBatch(int idProduct, int idWarehouse)
        {

            var data = db.StockBatches.Where(x => x.ProductID == idProduct && x.WarehouseID == idWarehouse && x.Status == "A").Select(x => new { id = x.BatchNo, name = "Batch->" + x.BatchRefNo + " Mgf-> " + x.ManufacturDate.ToString() }).ToList();


            return Json(data, "data", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetProducExpiryDays(int idProduct)
        {

            int pexpirydays;

            try
            {
                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Products where x.ProductID == idProduct select x).FirstOrDefault();
                    pexpirydays = (int)V.Expirydays;
                    //  Pdate = pManudate + pexpirydays;

                }

                return new JsonResult { Data = pexpirydays, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        public JsonResult GetAgencyFareAmount(int fWarehouse, int tWarehouse)
        {
            //  decimal rate1, rate2, rate3;

            try
            {

                var a = (from x in db.TransportFareSetups where x.CustomerID == tWarehouse && x.WarehouseID == fWarehouse && x.IsActive == 1 select x).FirstOrDefault();
                return new JsonResult { Data = a, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        public JsonResult GetCustomerOutstanding(int pCustomerID)
        {

            return new JsonResult { Data = 1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult CheckStock(int idProduct, int fWarehouse, int convertCases)
        {
            int StockQuantity, HandQty;
            double ConversionFact, psToCs;

            try
            {
                if (fWarehouse != 0)
                {
                    var A = (from x in db.Products where x.ProductID == idProduct select x).FirstOrDefault();
                    var V = (from x in db.ProductBalances where x.ProductID == idProduct && x.WarehouseID == fWarehouse select x).FirstOrDefault();
                    HandQty = V.OnHandQuantity;
                    ConversionFact = Convert.ToDouble(A.ConversionFactor);
                    psToCs = (convertCases / ConversionFact);
                    psToCs = Math.Round(psToCs, 2);
                    StockQuantity = HandQty;

                    return new JsonResult { Data = new { StockQuantity = StockQuantity, psToCs = psToCs }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var A = (from x in db.Products where x.ProductID == idProduct select x).FirstOrDefault();
                    ConversionFact = Convert.ToDouble(A.ConversionFactor);
                    psToCs = (convertCases / ConversionFact);
                    return new JsonResult { Data = new { StockQuantity = 0, psToCs = psToCs }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        public JsonResult StockinPcs(int idProduct, int fWarehouse, double convertpcs)
        {
            int StockQuantity, HandQty;
            double ConversionFact, CsToPc;

            try
            {
                if (fWarehouse != 0)
                {
                    var A = (from x in db.Products where x.ProductID == idProduct select x).FirstOrDefault();
                    var V = (from x in db.ProductBalances where x.ProductID == idProduct && x.WarehouseID == fWarehouse select x).FirstOrDefault();
                    HandQty = V.OnHandQuantity;
                    ConversionFact = Convert.ToDouble(A.ConversionFactor);
                    CsToPc = (convertpcs * ConversionFact);
                    CsToPc = Math.Round(CsToPc, 2);
                    StockQuantity = HandQty;

                    return new JsonResult { Data = new { StockQuantity = StockQuantity, CsToPc = CsToPc }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    var A = (from x in db.Products where x.ProductID == idProduct select x).FirstOrDefault();
                    ConversionFact = Convert.ToDouble(A.ConversionFactor);
                    CsToPc = (convertpcs * ConversionFact);
                    return new JsonResult { Data = new { StockQuantity = 0, CsToPc = CsToPc }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }

        // Stock Recieve
        public ActionResult StockReceive()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            ViewBag.TransactionNo = new SelectList(db.ProductTransactions.Where(x => x.ToWarehouse == wn.WarehouseID && x.Status == "T" && x.UpdatedBy == null && x.UpdatedDate == null), "TransactionNo", "TransactionNo");
            ViewBag.StorageLocationCode = new SelectList(db.StorageLocations.Where(c => c.WarehouseID == wn.WarehouseID), "WarehouseLocationCode", "ShortDescription");
            return View("StockReceive");

        }

        [HttpGet]
        public JsonResult GetProductDetail(int pTWarehouse, int pTransactionNo)
        {
            // DateTime d;

            try
            {
                var ProdInfo = db.spGetDBProductTransactionProduct(pTWarehouse, pTransactionNo);
                //   var V = (from x in db.spGetDBProductTransactionProduct(pTWarehouse, pTransactionNo) where x.TransactionNo == pTransactionNo && x.WarehouseID == pTWarehouse select x).FirstOrDefault();
                //  d = (DateTime)V.ExpiryDate;

                return new JsonResult { Data = ProdInfo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Transaction No  Not Found" });
                //  throw ex;
            }
        }
        [HttpPost]
        public ActionResult SaveProductReceived(StockReceivedVM S)
        {

            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "0000000");
            int Trno = Convert.ToInt32(s2);
            bool status = false;
            string mes = "";
            int v;
            int PIDMax;
            // var PTID = db.ProductTransactions.Max(p => p.TransactionNo);
            var PTID = (from x in db.ProductTransactions where x.WarehouseID == wn.WarehouseID select x.TransactionNo).DefaultIfEmpty(Trno).Max();
            PIDMax = (PTID + 1);
            v = PIDMax;

            try
            {
                if (ModelState.IsValid)
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {
                        ProductTransaction dm = new ProductTransaction();
                        {
                            // Here Id   Primary Key of DB table auto increase
                            dm.TransactionTypeID = 3;
                            dm.TransactionNo = PIDMax;
                            dm.WarehouseID = S.WarehouseID;
                            dm.FromWarehouse = S.FromWarehouse;
                            //dm.ToWarehouse = S.WarehouseID;
                            dm.TransactionDate = DateTime.Today;
                            dm.ReferenceNo = S.ReferenceNo;
                            dm.Status = "A";
                            //cuser = User.Identity.Name
                            dm.CreatedBy = User.Identity.Name;
                            dm.CreatedDate = DateTime.Now;
                            dm.StoreLocation = S.StoreLocation;
                        }
                        dc.ProductTransactions.Add(dm);
                        foreach (var i in S.ProdTrDtl)
                        {
                            ProductTransactionDetail dmd = new ProductTransactionDetail();
                            dmd.ProductID = i.ProductID; // Not Null
                            dmd.WarehouseID = S.WarehouseID;
                            //dmd.WarehouseID = (int)S.FromWarehouse;
                            dmd.TransactionNo = PIDMax;
                            // dmd.PlantNo =
                            dmd.LineNoPlant = i.LineNoPlant;
                            dmd.BatchNo = i.BatchNo;
                            dmd.ExpiryDate = i.ExpiryDate;
                            dmd.ManufactureDate = i.ManufactureDate;
                            dmd.Quantity = (i.Quantity);
                            dmd.EmptyBottleQuantity = (i.EmptyBottleQuantity);
                            dmd.PlasticBoxQuantity = i.PlasticBoxQuantity;
                            dmd.BurstBottleQuantity = (i.BurstBottleQuantity);
                            dmd.BreakageBottleQuantity = (i.BreakageBottleQuantity);
                            //TCases = TCases + i.Quantity;
                            dc.ProductTransactionDetails.Add(dmd);
                            // For Batch 
                            StockBatch sb = new StockBatch();
                            sb.ProductID = i.ProductID;
                            sb.WarehouseID = (int)S.WarehouseID;
                            sb.BatchNo = (int)i.BatchNo;
                            sb.ManufacturDate = (DateTime)i.ManufactureDate;
                            sb.ExpiryDate = (DateTime)i.ExpiryDate;
                            sb.PlantLineNo = (int)i.LineNoPlant;
                            sb.PlantNo = i.PlantNo;
                            sb.IssueQuantity = i.Quantity;
                            sb.ReceivedQty = i.Quantity;
                            sb.Status = "A";
                            sb.WarehouseLocationCode = S.StoreLocation;
                            dc.StockBatches.Add(sb);
                            // Add stock batch detail for transaction history
                            StockBatchDetail sbd = new StockBatchDetail();
                            sbd.WarehouseID = S.WarehouseID;
                            sbd.BatchNo = (int)i.BatchNo;
                            sbd.ProductID = i.ProductID;
                            sbd.TransactionNo = PIDMax;
                            sbd.Quantity = i.Quantity;
                            sbd.BurstBottleQuantity = i.BurstBottleQuantity;
                            sbd.BreakageBottleQuantity = i.BreakageBottleQuantity;
                            sbd.PlasticBoxQuantity = (int)i.PlasticBoxQuantity;
                            sbd.TransactionDate = DateTime.Today;
                            sbd.TransactionType = "Transfer Received";
                            sbd.CreateBy = User.Identity.Name;
                            sbd.CreateDate = DateTime.Now;
                            dc.StockBatchDetails.Add(sbd);
                        }
                        foreach (var i in S.ProdTrDtl)
                        {
                            var CF = (from x in dc.Products where x.ProductID == i.ProductID select x).FirstOrDefault();
                            try
                            {


                                var V = (from x in db.ProductBalances where x.ProductID == i.ProductID && x.WarehouseID == S.ToWarehouse select x).FirstOrDefault();
                                //Add Stock

                                var productupdate = dc.spProductBalanceUpdate(i.ProductID,
                                S.WarehouseID,
                                (int)i.Quantity, //change
                                i.BurstBottleQuantity,
                                i.BreakageBottleQuantity,
                                (int)i.PlasticBoxQuantity,
                                i.EmptyBottleQuantity);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                        }
                        var pt = dc.ProductTransactions.Where(x => x.WarehouseID == S.FromWarehouse && x.TransactionNo == S.TransactionNo && x.TransactionTypeID == S.TransactionTypeID).FirstOrDefault();

                        pt.UpdatedBy = User.Identity.Name;
                        pt.UpdatedDate = DateTime.Now;
                        dc.SaveChanges();
                        status = true;
                        dc.Dispose();
                    }

                }
                else
                {
                    status = false;
                }
                return new JsonResult { Data = new { status = status, mes = mes, v = v } };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Transaction No  Not Found" });
                //  throw ex;
            }

        }

        public JsonResult ProductDetail(int inWarehouseID, int inTrNo)
        {


            var pData = db.spGetDBProductTransactionProduct(inWarehouseID, inTrNo);


            return new JsonResult { Data = pData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
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