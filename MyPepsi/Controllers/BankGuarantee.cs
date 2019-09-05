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
    public class BankGuaranteeController : Controller
    {
        // GET: BankGuarantee
        private PEPSIEntities db = new PEPSIEntities();
        // GET: UserManger
        public ActionResult Index()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            int userwarehouse = wn.WarehouseID;
            if (userwarehouse == 100)
            {
                ViewBag.WarehouseID = new SelectList(db.Warehouses.Where(x => x.WarehouseID != 100), "WarehouseID", "WarehouseDescription");
            }
            else
            {
                ViewBag.WarehouseID = new SelectList(db.Warehouses.Where(x => x.WarehouseID == wn.WarehouseID), "WarehouseID", "WarehouseDescription");
            }
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            if (userwarehouse == 100)
            {

                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName");
            }
            else
            {
                ViewBag.CustomerID = new SelectList(db.Customers.Where(x => x.WarehouseID == userwarehouse), "CustomerID", "CustomerName");

            }
            //ViewBag.BankNameList = new SelectList(db.BankNames, "BankName", "BankName");
            ViewBag.BankBrName = new SelectList(db.Branches, "BranchName", "BranchName");

            return View();
        }
        [HttpPost]
        public JsonResult SaveBGData(CustomerBGInfoVM A)
        {
            bool status = false;
            string mes = "";
            string v = A.BGRefNo;
            int bgNo;
            bgNo = (from x in db.CustomerBGInfoes select x).Max(p => p.BankGuaranteeNo) + 1;
            var c = db.Customers.Where(t => t.CustomerID == A.CustomerID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {
                        CustomerBGInfo bg = new CustomerBGInfo();
                        {
                            bg.CustomerID = A.CustomerID;
                            bg.BankGuaranteeNo = bgNo;
                            bg.BankName = A.BankName;
                            bg.BGRefNo = A.BGRefNo;
                            bg.ActualBGAmt = A.ActualBGAmt;
                            bg.BGAmount = A.BGAmount;
                            bg.IssueDate = A.IssueDate;
                            bg.ExpiryDate = A.ExpiryDate;
                            bg.BankBrName = A.BankBrName;
                            bg.Status = "A";
                            bg.CreateBy = User.Identity.Name;
                            bg.CreateDate = System.DateTime.Now;

                        }
                        dc.CustomerBGInfoes.Add(bg);
                        CustomerLedgerDetail cl = new CustomerLedgerDetail();
                        {
                            cl.WarehouseID = (int)c.WarehouseID;
                            cl.TransactionNo = "BG" + bgNo;
                            cl.TransactionTypeID = 18;
                            cl.CustomerID = A.CustomerID;
                            cl.DebitCredit = "C";
                            cl.CrAmount = A.BGAmount;
                            cl.DrAmount = 0;
                            cl.Narration = "Customer BG amount#" + A.BGAmount + "Against BG NO#" + A.BGRefNo;
                            cl.RefNumber1 = A.BGRefNo;
                            cl.TransactionDate = System.DateTime.Today;
                            cl.CreateBy = User.Identity.Name;
                            cl.CreateDate = System.DateTime.Now;
                        }
                        dc.CustomerLedgerDetails.Add(cl);
                        // Update Customer Curr balance
                        var d = db.spCustomerBalanceUpdate(A.CustomerID, "G", A.BGAmount);
                        // Save All Data
                        dc.SaveChanges();
                        status = true;
                        dc.Dispose();
                    }
                }
                catch (Exception ex)
                {

                    return new JsonResult { Data = new { status = status, mes = mes, v = v } };
                    //throw ex;
                }

            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = status, mes = mes, v = v } };
        }
    }
}