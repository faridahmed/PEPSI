using MyPepsi.InGeneral;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class ReturnmentController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        private SqlConnection con;
        private int csID;
        // GET: Returnment
        public ActionResult Index()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            var cust = db.Customers.Where(x => x.WarehouseID == wn.WarehouseID && x.ActiveStatus == "A").Select(x => new { Text = x.CustomerName + " , " + x.CustomerID, Value = x.CustomerID }).OrderBy(e => e.Text).ToList();
            ViewBag.CustomerID = new SelectList(cust, "Value", "Text");
            ViewBag.ReturnTypeID = new SelectList(db.TransactionTypes.Where(x => x.UsedType == "R" && x.Status == "A").OrderBy(x => x.TransactionTypeName), "TransactionTypeID", "TransactionTypeName");
            ViewBag.TransportAgencyList = new SelectList(db.TransportAgencySetups.Where(x => x.EligibleForCarringGoods == 1), "TAID", "TAName", "0");
            //var prod = db.Products.Where(x => x.BottleReturnable == 1).Select(x => new { Text = x.ProductID + " , " + x.ProductDescription, Value = x.ProductID }).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult SaveReturn(ReturnmentVM R)
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "3000000");
            string s3 = string.Concat(s1 + "5000000");
            int vRID = Convert.ToInt32(s2);
            int MaxCashID = Convert.ToInt32(s3);
            bool status = false;
            string mes = "";
            //int  = db.Returnments.Max(p=>p.ReturnmentID)+1;
            var V = (from x in db.Customers where x.CustomerID == R.CustomerID select x).FirstOrDefault();

            var MvrID = (from n in db.Returnments where n.WarehouseID == w.WorkStationID select n.ReturnmentID).DefaultIfEmpty(vRID).Max();
            int vrID = MvrID + 1;
            int v = vrID;
            var PTID = (from x in db.ProductTransactions where x.WarehouseID == w.WorkStationID select x.TransactionNo).Max();
            int TrID = (PTID + 1);
            var maxCasettleId = (from n in db.CashSettlements where n.WarehouseID == w.WorkStationID select n.CashSettlementID).DefaultIfEmpty(MaxCashID).Max();
            csID = Convert.ToInt32(maxCasettleId + 1);
            decimal securityAmt = 0;
            decimal securitywithRebate = 0;
            decimal diffamt = 0;
            decimal negload = 0;
            if (ModelState.IsValid)
            {
                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    CommonChk cm = new CommonChk();

                    Returnment dbr = new Returnment
                    {

                        WarehouseID = R.WarehouseID,
                        ReturnmentID = vrID,
                        CustomerID = R.CustomerID,
                        Reasons = R.Reasons,
                        RefNumber = R.RefNumber,
                        ReturnDate = DateTime.Today,//R.ReturnDate,
                        ReturnTypeID = R.ReturnTypeID,
                        ReturnGoodsQty = R.ReturnGoodsQty,
                        InAmount = R.InAmount,
                        AdjustedAmt = R.AdjustedAmt,
                        CreateDate = DateTime.Now,
                        CreateBy = User.Identity.Name

                    };
                    dc.Returnments.Add(dbr);
                    if (R.FareAmt > 0)
                    {
                        var CustName = (from x in db.Customers where x.CustomerID == R.CustomerID select x).FirstOrDefault();
                        TransportAgencyandFareSetup tf = new TransportAgencyandFareSetup();
                        {
                            tf.WarehouseID = R.WarehouseID;
                            tf.CustomerName = CustName.CustomerName.ToString();
                            tf.Address = CustName.CustomerAddress1;
                            tf.TAID = (int)R.TAID;
                            tf.ChallanNumber = vrID;
                            tf.FareAmnt = (decimal)R.FareAmt;
                            tf.VechileNo = R.VechileNo;
                            tf.EnteredBy = User.Identity.Name;
                            tf.EnteredDate = DateTime.Now;
                            tf.ChallanDate = DateTime.Today;
                            tf.TotalCases = R.ReturnGoodsQty;
                            tf.Status = "No";
                            tf.Remarks = "Returned Hired Truck";
                            tf.EmptyReturn = "Yes";
                        }
                        dc.TransportAgencyandFareSetups.Add(tf);
                    }
                    if (R.ReturnTypeID == 14)
                    {
                        foreach (var i in R.listreturndetail)

                        {
                            decimal countQtywithRebate = 0;
                            int countQtywithRebatePlastic = 0;
                            countQtywithRebate = (decimal)i.ReturnQty + (decimal)i.RebateQty;
                            countQtywithRebatePlastic = (int)i.Packsize + (int)i.RebatePlastic;
                            decimal xxx = cm.GetSecurityAmount((int)i.ProductID, (decimal)i.ReturnQty, (int)i.Packsize);
                            decimal xx = cm.GetSecurityAmount((int)i.ProductID, countQtywithRebate, countQtywithRebatePlastic);
                            securityAmt = securityAmt + xxx;
                            securitywithRebate = securitywithRebate + xx;
                            diffamt = securitywithRebate - securityAmt;


                        }
                    }

                    decimal cramt = 0;
                    decimal drinksamount = (decimal)R.InAmount - securityAmt;
                    decimal cramtsub = (decimal)R.InAmount - (decimal)R.AdjustedAmt; // Unloadcharge is negetive
                    decimal cramtadd = (decimal)R.InAmount + (decimal)R.AdjustedAmt;
                    decimal totsecurityamount = securityAmt + diffamt;
                    if (R.ReturnTypeID == 13)
                    {
                        cramt = cramtadd;
                        securityAmt = (decimal)R.InAmount;
                    }
                    else
                    {
                        cramt = cramtsub;
                    }
                    if (R.ReturnTypeID != 13) // Not Empty Return
                    {
                        ProductTransaction dm = new ProductTransaction();
                        {

                            dm.TransactionTypeID = R.ReturnTypeID;
                            dm.TransactionNo = TrID;//
                            dm.WarehouseID = R.WarehouseID;
                            dm.FromWarehouse = R.WarehouseID;
                            //dm.ToWarehouse = S.ToWarehouse;
                            dm.TransactionDate = DateTime.Today;
                            dm.ReferenceNo = vrID.ToString();
                            if (R.ReturnTypeID != 13)
                            {
                                dm.Status = "R";
                            }
                            //else
                            //{
                            //    dm.Status = "A";
                            //}
                            //cuser = User.Identity.Name
                            dm.CreatedBy = User.Identity.Name;
                            dm.CreatedDate = DateTime.Now;
                        }
                        dc.ProductTransactions.Add(dm);
                    }
                    // Customer Ledger
                    if (R.ReturnTypeID != 12)
                    {
                        CustomerLedgerDetail cld = new CustomerLedgerDetail();
                        {
                            cld.WarehouseID = R.WarehouseID;
                            cld.CustomerID = (int)R.CustomerID;
                            cld.TransactionNo = "RTN" + vrID.ToString();
                            cld.TransactionTypeID = R.ReturnTypeID;
                            cld.RefNumber1 = R.RefNumber.ToString();
                            cld.TransactionDate = DateTime.Today;
                            cld.CrAmount = cramt;//R.InAmount;
                            cld.DrAmount = 0;
                            cld.DebitCredit = "C";
                            cld.Narration = "Returnment Amount#" + cramt /*R.InAmount*/ + " to Customer" + cld.CustomerID;
                            cld.CreateBy = User.Identity.Name;
                            cld.CreateDate = System.DateTime.Now;
                        }
                        dc.CustomerLedgerDetails.Add(cld);
                        var m = (from x in db.Customers where x.CustomerID == R.CustomerID select x).FirstOrDefault();
                        CashSettlement cs = new CashSettlement();
                        cs.WarehouseID = R.WarehouseID;
                        cs.CustomerID = (int)R.CustomerID;
                        cs.CashSettlementID = csID;
                        cs.CashSettlementDate = DateTime.Today;
                        cs.DeliveryChallanDate = DateTime.Today;
                        cs.OrderID = vrID;// R.ReturnmentID;
                        cs.DeliveryChallanNumbers = 0;
                        cs.TransactionNo = vrID;
                        cs.TotalAmount = cramt;
                        cs.ReceivedAmount = cramt;
                        cs.CustomerExecutiveID = m.CustomerExecutiveID;
                        if (R.ReturnTypeID == 13)
                        {
                            cs.EmptyReturnLoadCharge = R.AdjustedAmt;
                            cs.SecurityAmount = -R.InAmount;
                        }
                        else
                        {
                            // cs.FinishGoodsUnloadCharge = R.AdjustedAmt;
                            cs.ReturnFGCharge = R.AdjustedAmt;
                            cs.SecurityAmount = -totsecurityamount;
                        }

                        cs.CreateBy = User.Identity.Name;
                        cs.CraeteDate = DateTime.Now;
                        dc.CashSettlements.Add(cs);

                        // var cb = db.spCustomerBalanceUpdate(R.CustomerID, "R", cramt/*R.InAmount*/);

                        decimal vInvAmt = cramt - securityAmt;
                        decimal vBookAmt = 0;
                        decimal vCurBal = cramt + diffamt;
                        if (R.ReturnTypeID == 14)
                        {
                            negload = -(decimal)R.AdjustedAmt;
                        }
                        else
                        {
                            negload = (decimal)R.AdjustedAmt;
                        }
                        decimal vChargeAmount = negload;
                        decimal vSecurityAmt = securityAmt + diffamt;// securitywithRebate;
                        decimal vRebate = 0;
                        decimal vFare = (decimal)R.FareAmt;
                        decimal VactuaInvAmount = vInvAmt - negload;// (decimal)R.AdjustedAmt; // without loaduloadcharge


                        db.spUpdateDBsInvoiceValue(
                            R.CustomerID,
                            "S",
                            vCurBal,
                            vCurBal,
                            VactuaInvAmount,
                            vBookAmt,
                            vSecurityAmt,
                            vChargeAmount,
                            vFare,
                            vRebate,
                            vrID
                            );

                    }
                    foreach (var i in R.listreturndetail)
                    {
                        var q = (from x in db.Products where x.ProductID == i.ProductID select x).FirstOrDefault();
                        int conf = q.ConversionFactor;
                        decimal totpcqty = (decimal)i.ReturnQty * conf;
                        int csqty = (Convert.ToInt32(Math.Round(totpcqty)) / conf);
                        int pcqty = (Convert.ToInt32(Math.Round(totpcqty)) % conf);
                        decimal totrebpcqty = (decimal)i.RebateQty * conf;
                        int rebcsqty = (Convert.ToInt32(Math.Round(totrebpcqty)) / conf);
                        int rebpcqty = (Convert.ToInt32(Math.Round(totrebpcqty)) % conf);

                        if (R.ReturnTypeID != 13)
                        {
                            ProductTransactionDetail ptd = new ProductTransactionDetail();
                            decimal qty = (decimal)i.ReturnQty + (decimal)i.RebateQty;
                            int s = q.ConversionFactor;
                            // decimal qt = Math.Round((decimal)(i.ReturnQty) * s, 2);
                            decimal qt = Math.Round((decimal)(qty) * s, 2);
                            int pcquantity = (Convert.ToInt32(Math.Round(qt)) % s);
                            int casesqty = (Convert.ToInt32(Math.Round(qt)) / s);
                            ptd.WarehouseID = R.WarehouseID;
                            ptd.TransactionNo = TrID;
                            ptd.ProductID = (int)i.ProductID;
                            ptd.Quantity = (int)qt;// (int)i.ReturnQty;
                            ptd.PlasticBoxQuantity = (int)i.Packsize;
                            ptd.BatchNo = i.BatchNo;
                            dc.ProductTransactionDetails.Add(ptd);

                            StockBatchDetail sbd = new StockBatchDetail();
                            sbd.WarehouseID = R.WarehouseID;
                            sbd.TransactionNo = vrID;
                            sbd.TransactionDate = System.DateTime.Today;
                            sbd.ProductID = (int)i.ProductID;
                            sbd.Quantity = (int)qt;// (int)i.ReturnQty;
                            sbd.PlasticBoxQuantity = i.Packsize;
                            sbd.TransactionType = "Return Product";
                            sbd.BatchNo = (int)i.BatchNo;
                            sbd.CreateBy = User.Identity.Name;
                            sbd.CreateDate = System.DateTime.Now;
                            dc.StockBatchDetails.Add(sbd);
                            if (R.ReturnTypeID == 14)
                            {
                                var peb = db.spProductBalanceUpdate(i.ProductID, R.WarehouseID, (int)qt, 0, 0, 0, 0);// (int)i.ReturnQty);

                            };
                        };
                        ReturnmentDetail rd = new ReturnmentDetail();
                        rd.WarehouseID = R.WarehouseID;
                        rd.ReturnmentID = vrID;// R.ReturnmentID;
                        rd.ProductID = i.ProductID;
                        rd.ReturnQty = i.ReturnQty;
                        rd.Amount = i.Amount;
                        rd.Packsize = i.Packsize;
                        rd.BatchNo = i.BatchNo;
                        rd.RebateQty = i.RebateQty;
                        rd.RebatePlastic = i.RebatePlastic;
                        rd.SchemeID = i.SchemeID;
                        dc.ReturnmentDetails.Add(rd);

                        if (R.ReturnTypeID != 12)
                        {
                            CashSettlementDetail cd = new CashSettlementDetail();
                            var ratedata = db.ProductRateDetails.Where(x => x.ProductRateID == V.ProductRateID && x.ProductID == i.ProductID).FirstOrDefault();
                            // double xva = .2;
                            cd.WarehouseID = R.WarehouseID;
                            cd.CashSettlementID = csID;
                            cd.AlternateReturnedFilledQuantity = (int)i.RebateQty;
                            cd.ProductID = (int)i.ProductID;
                            cd.UnitPrice = ratedata.UnitPrice;
                            cd.AlternateAgencyCommission = ratedata.AlternateAgencyCommission;
                            cd.AlternateUnitPrice = ratedata.AlternateUnitPrice;
                            cd.MRPRate = ratedata.MRPRate;
                            cd.AlternateRebateAmount = 0;
                            cd.AlternateGrossSalesAmount = 0;
                            cd.VATAmount = 0;
                            cd.SchemeID = (int)i.SchemeID;
                            cd.ReturnedPlasticBoxSecurity = 0;// (decimal)i.Amount* (decimal)xva;                                                                                  
                            //cd.UnitPrice = 
                            if (R.ReturnTypeID == 13)
                            {
                                //cd.ReturnedAlternateSecurityDeposit = (decimal)i.Amount;
                                cd.ReturnedQuantity = pcqty;
                                cd.AlternateReturnedQuantity = csqty;
                                cd.ReturnedPlasticBoxQuantity = (int)i.Packsize;
                                cd.ReturnedSecurityDeposit = q.SecurityDeposit;
                                cd.ReturnedAlternateSecurityDeposit = q.AlternateSecurityDeposit;
                                cd.ReturnedPlasticBoxSecurity = q.PlasticBoxSecurity;
                            }
                            else
                            {
                                cd.AlternateReturnedFilledQuantity = csqty;// (int)i.ReturnQty;
                                cd.ReturnedFilledQuantity = pcqty;
                                cd.AlternateReturnedRebateQuantity = rebcsqty;// (int)i.RebateQty;
                                cd.ReturnedRebateQuantity = rebpcqty;
                                cd.ReturnedPlasticBoxQuantity = (int)i.Packsize+(int)i.RebatePlastic;
                                cd.ReturnedSecurityDeposit = q.SecurityDeposit;
                                cd.ReturnedAlternateSecurityDeposit = q.AlternateSecurityDeposit;
                                cd.ReturnedPlasticBoxSecurity = q.PlasticBoxSecurity;

                            }

                            dc.CashSettlementDetails.Add(cd);

                        }


                        //if (R.ReturnTypeID == 14)
                        //{
                        //    var pb = db.spProductBalanceUpdate(i.ProductID, R.WarehouseID, (int)i.ReturnQty, 0, 0, 0, 0);

                        //}

                    };
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
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }

        public JsonResult GetProductDesc(int idProduct)
        {
            string PName;

            try
            {
                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Products where x.ProductID == idProduct select x).FirstOrDefault();
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
        public JsonResult GetReturnAmt(int inCustID, int inProduct, decimal inCase, int inPlastic, int inTrType)
        {
            decimal returnAmount = 0;

            try
            {


                var V = (from x in db.Customers where x.CustomerID == inCustID select x).FirstOrDefault();
                var data = db.ProductRateDetails.Where(x => x.ProductRateID == V.ProductRateID && x.ProductID == inProduct);

                if (data != null)
                {
                    var x = data.ToList();
                    foreach (var W in x)
                    {
                        if (inTrType != 13)
                        {
                            returnAmount = ((inCase * W.AlternateUnitPrice) - (inCase * W.AlternateAgencyCommission)) + ((inCase * W.AlternateSecurityDeposit) + (inPlastic * W.PlasticBoxSecurity));
                        }
                        else
                        {
                            returnAmount = ((inCase * W.AlternateSecurityDeposit) + (inPlastic * W.PlasticBoxSecurity));
                        }

                    }
                }
                else
                {
                    returnAmount = 0;
                }
                return new JsonResult { Data = returnAmount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                // throw ex;
            }
        }
        public JsonResult GetPlastic(int inProductID)
        {
            int plasticbox;
            try
            {

                var V = (from x in db.Products where x.ProductID == inProductID select x).FirstOrDefault();

                plasticbox = V.BottleReturnable;
                return new JsonResult { Data = plasticbox, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                // throw ex;
            }
        }
        [HttpGet]
        public JsonResult GetAgencyFareAmount(int custID)
        {

            try
            {

                var a = (from x in db.EmptyFareSetups where x.CustomerID == custID && x.Status == "A" select x).FirstOrDefault();
                return new JsonResult { Data = a, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        [HttpGet]
        public JsonResult GetAgencyFareAmountEmpty(int custID)
        {

            try
            {

                var a = (from x in db.EmptyFareSetups where x.CustomerID == 100001 && x.Status == "A" select x).FirstOrDefault();
                return new JsonResult { Data = a, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }
        [HttpGet]
        public JsonResult getCustomerChallan(int inChalan, int inCustomer)
        {
            var data = db.Orders.Where(c => c.CustomerID == inCustomer && c.Delivered == "Y").Select(c => new { id = c.ChallanNumber, name = c.ChallanNumber + " ( " + c.OrderDate + " ) " + c.CustomerID }).ToList();
            //var data = db.StockBatches.Where(x => x.ProductID == idProduct && x.WarehouseID == idWarehouse).Select(x => new { id = x.BatchNo, name = x.BatchRefNo + "[" + x.ManufacturDate + "]" }).ToList();
            return Json(data, "data", JsonRequestBehavior.AllowGet);
        }
    }
}