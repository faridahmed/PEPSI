using MyPepsi;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrimarySalesSystem.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        private SqlConnection con;
        //decimal totalReceive, totalUnload, totalEmptyCharge, totalFGReturnCharge, totalPaid, actualOutstanding;
        // GET: Payment
        public ActionResult Index()
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();


            var clients = db.Customers.Where(x=>x.WarehouseID==wn.WarehouseID && x.ActiveStatus == "A")
    .Select(s => new
    {
        Text = s.CustomerName + " , " + s.CustomerAddress1,
        Value = s.CustomerID
    })
    .ToList();

            ViewBag.CustomerLists = new SelectList(clients, "Value", "Text");

            //ViewBag.CustomerLists = new SelectList(db.Customers.Where(x => x.CustomerTypeID == 10 && x.Suspended == false).OrderBy(x => x.CustomerName), "CustomerID", "CustomerName");
            ViewBag.PTransactionType = new SelectList(db.PaymentTransactionTypes, "PaymentTransactionType1", "PaymentTransactionName");
            ViewBag.PayMode = new SelectList(db.PaymentModes, "PaymentModeID", "PaymentModeDescription");
            ViewBag.DepositedBy = new SelectList(db.DepositedByPeopleNames, "DepositedByID", "DepositedByName");
            ViewBag.BankName = new SelectList(db.BankNames.OrderBy(x => x.BankName1), "BANKID", "BankName1");
            ViewBag.BranchName = new SelectList(db.Branches.OrderBy(x => x.BranchName), "BranchID", "BranchName");
            ViewBag.LocalBranchName = new SelectList(db.BankBranchSubZoneQueries.OrderBy(x => x.BranchName), "BranchID", "BranchName");
            //return View();
            return View(new PaymentTransactionVM());
        }

        public JsonResult fillLocalBranch(int idBank)
        {
            // string PName;

            try
            {
                // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

                var V = db.BankBranchSubZoneQueries.Where(c => c.BankID == idBank);
                //var V = 0;

                return new JsonResult { Data = V, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Bank ID Not Found" });
                //throw ex;
            }
        }
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            //string constr = ConfigurationManager.ConnectionStrings["PEPSIEntities"].ToString();
            con = new SqlConnection(constr);

        }

        //public JsonResult GetCustomerCurrentOuts(int custID)
        //{

        //    try
        //    {
        //        var a = db.spGetCustomerCurrentOutstanding(custID).ToList();
        //        foreach (var v in a)
        //        {
        //            totalReceive = Convert.ToDecimal(v.TotalAmount);
        //            totalUnload = Convert.ToDecimal(v.FinishGoodsUnloadCharge);
        //            totalEmptyCharge = Convert.ToDecimal(v.EmptyReturnLoadCharge);
        //            totalFGReturnCharge = Convert.ToDecimal(v.ReturnFGCharge);
        //            totalPaid = Convert.ToDecimal(v.PaymentReceived);
        //            actualOutstanding = (((totalReceive) - ((totalUnload + totalEmptyCharge) - totalFGReturnCharge)) - totalPaid);
        //            actualOutstanding = Math.Round(actualOutstanding, 2);
        //        }
        //        return new JsonResult { Data = actualOutstanding, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { status = "error", message = "Product ID Not Found" });

        //    }

        //}
        public JsonResult GetCustomerCurrentOuts(int custId)
        {
            using (PEPSIEntities dc = new PEPSIEntities())
            {
                // decimal TotalReceiveable = 0, TotalPayment = 0, TransitAmount = 0, TransitBottleAmount = 0, ActualOutstanding = 0, TotalCrLimit = 0;
                decimal amt = 0;
                try
                {
                    var ActualOutstanding = db.CustomerLedgerMasters.Where(x => x.CustomerID == custId).FirstOrDefault();
                    amt = (decimal)ActualOutstanding.CurrentBalance;                   

                    return new JsonResult { Data = amt, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Customer Outstanding" });
                    // throw ex;
                }
            }
        }
        public JsonResult CheckDuplicate(int bankBrnchID, string ckequeNo)
        {
            // string PName;

            try
            {
                // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

                var V = (from x in db.Payments where (x.BankBranchID == bankBrnchID && x.ChequeDDNo == ckequeNo) select x).FirstOrDefault();
                if (V.ChequeDDNo == ckequeNo)
                {
                    return new JsonResult { Data = V, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return Json(new { status = "error", message = "Duplicate Number Found", JsonRequestBehavior = JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Duplicate Number Found", JsonRequestBehavior = JsonRequestBehavior.AllowGet });
                //throw ex;
            }
        }
        public JsonResult AddPaymentData(PaymentTransactionVM paymentsystem)
        {
            int mrNumber;
            int v ;
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {

                    Payment pm = new Payment();
                    {


                        //db.Transactions.Where(w => w.AccountId == accountInstance.Id && w.IsCancelled == false && w.TransactionTypeId == 2 && w.Date.Year >= 2015)
                        //               .Select(t => t.Id)
                        //               .DefaultIfEmpty(-1)
                        //               .Max()
                        var wa = (from y in db.UserLogins
                                 where y.UserID.ToString() == User.Identity.Name
                                 select new { y.WorkStationID }).FirstOrDefault();
                        var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();


                        var w = db.Customers.Where(x => x.CustomerID == paymentsystem.CustomerID).FirstOrDefault();
                        string s1 = w.WarehouseID.ToString();
                        string s2 = string.Concat(s1 + "0000000");
                        int MRNo = Convert.ToInt32(s2);
                        var maxMrNO = (from n in db.Payments where n.WarehouseID == wn.WarehouseID select n.MoneyReceiptNo).DefaultIfEmpty(MRNo).Max();

                        //var V = db.Payments.Where(p => p.WarehouseID == w.WarehouseID).Select(p => p.MoneyReceiptNo).DefaultIfEmpty(100000).Max();
                        mrNumber = (maxMrNO + 1);
                        
                        pm.CustomerID = paymentsystem.CustomerID;
                        pm.PaymentTransactionType = paymentsystem.PaymentTransactionType;
                        pm.PaymentDate = DateTime.Now.Date;
                        pm.PaymentReceived = paymentsystem.PaymentReceived;
                        pm.PaymentModeID = paymentsystem.PaymentModeID;
                        pm.ActualPaymentEntryDate = DateTime.Now;
                        pm.PaymentNote = paymentsystem.PaymentNote;
                        pm.MoneyReceiptNo = mrNumber;
                        pm.BankBranchID = paymentsystem.BankBranchID;
                        pm.ChequeDDNo = paymentsystem.ChequeDDNo;
                        pm.ChequeDate = paymentsystem.ChequeDate;
                        pm.LocalBankBranchID = paymentsystem.LocalBankBranchID;
                        //pm.DepositedByID = (int)User.Identity.Name;// paymentsystem.DepositedByID;

                        pm.AdvanceAdjustment = false;
                        pm.CreateBy = User.Identity.Name;
                        pm.CreateDate = DateTime.Now;
                        pm.ChallanNo = paymentsystem.ChallanNo;
                        pm.BankID = paymentsystem.BankID;
                        pm.BranchID = paymentsystem.BankBranchID;
                        pm.LocalBranchID = paymentsystem.LocalBankBranchID;
                        pm.WarehouseID = (int)w.WarehouseID;
                    }
                    db.Payments.Add(pm);
                    CustomerLedgerDetail cld = new CustomerLedgerDetail();
                    {
                        var w = db.Customers.Where(x => x.CustomerID == paymentsystem.CustomerID).FirstOrDefault();
                        cld.WarehouseID = (int)w.WarehouseID;
                        cld.CustomerID = paymentsystem.CustomerID;
                        cld.TransactionTypeID = 16;
                        cld.TransactionDate = DateTime.Today;
                        cld.TransactionNo = "PMT" + mrNumber.ToString();
                        cld.CrAmount = paymentsystem.PaymentReceived;
                        cld.DrAmount = 0;
                        cld.DebitCredit = "C";
                        cld.Narration = "Payment Value#" + paymentsystem.PaymentReceived + " for Customer" + cld.CustomerID;
                        cld.CreateBy = User.Identity.Name;
                        cld.CreateDate = System.DateTime.Now;
                    }
                    db.CustomerLedgerDetails.Add(cld);
                    //var pmxPay = (from x in db.Customers where x.CustomerID == paymentsystem.CustomerID select x).FirstOrDefault();
                    //int cType = Convert.ToInt16(pmxPay.CustomerTypeID);
                    //if (cType == 10)
                    //{
                    PaymentDetail pd = new PaymentDetail();
                    {
                        var w = db.Customers.Where(x => x.CustomerID == paymentsystem.CustomerID).FirstOrDefault();
                        // pd.WarehouseID = 
                        pd.CashSettlementID = 0;
                        pd.MoneyReceiptNo = mrNumber;
                        pd.WarehouseID = w.WarehouseID;
                        pd.CashSettlementDate = DateTime.Now;
                    }
                    db.PaymentDetails.Add(pd);

                    // Customer Balance Update

                    var custbalance = db.spCustomerBalanceUpdate(paymentsystem.CustomerID,
                                                                "P",
                                                                 paymentsystem.PaymentReceived);

                    //{
                    //    connection();
                    //    con.Open();
                    //    // S.WarehouseID = (int)S.ToWarehouse;
                    //    SqlCommand com = new SqlCommand("spCustomerBalanceUpdate", con);
                    //    com.CommandType = CommandType.StoredProcedure;
                    //    com.Parameters.AddWithValue("@PCustomerID", paymentsystem.CustomerID);
                    //    com.Parameters.AddWithValue("@PDrCr", "D");
                    //    com.Parameters.AddWithValue("@PAmount", paymentsystem.PaymentReceived);
                    //    com.ExecuteNonQuery();
                    //    con.Close();
                    //}

                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes, v = mrNumber } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Error Found" });
                    //throw ex;
                }

            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };//, v = v } };
        }

    }

}