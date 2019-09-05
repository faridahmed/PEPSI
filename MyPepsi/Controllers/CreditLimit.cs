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
    public class CreditLimitController : Controller
    {
        // GET: CreditLimit
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


            return View();
        }
        [HttpPost]
        public JsonResult SaveCLData(CustomerCreditLimitVM A)
        {
            bool status = false;
            string mes = "";
            string v = A.CLNo;
            int clNo;
            clNo = (from x in db.CustomerCreditLimits select x).Max(p => p.CLRefNo) + 1;
            var c = db.Customers.Where(t => t.CustomerID == A.CustomerID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {
                        CustomerCreditLimit clm = new CustomerCreditLimit();
                        {
                            clm.CustomerID = A.CustomerID;
                            clm.CLRefNo = clNo;
                            clm.CLNo = A.CLNo;
                            clm.ApproveBy = A.ApproveBy;
                            clm.ApproveDate = A.ApproveDate;
                            clm.ActivateDate = A.ActivateDate;
                            clm.IssueDate = A.IssueDate;
                            clm.ExpiryDate = A.ExpiryDate;
                            clm.CLAmount = A.CLAmount;
                            clm.Status = "A";
                            clm.CreateBy = User.Identity.Name;
                            clm.CreateDate = System.DateTime.Now;

                        }
                        dc.CustomerCreditLimits.Add(clm);
                        CustomerLedgerDetail cl = new CustomerLedgerDetail();
                        {
                            cl.WarehouseID = (int)c.WarehouseID;
                            cl.TransactionNo = "CL" + clNo;
                            cl.TransactionTypeID = 17;
                            cl.CustomerID = A.CustomerID;
                            cl.DebitCredit = "C";
                            cl.CrAmount = A.CLAmount;
                            cl.DrAmount = 0;
                            cl.Narration = "Credit limit amount#" + A.CLAmount + "Against CLRef#" + A.CLNo;
                            cl.RefNumber1 = A.CLNo;
                            cl.TransactionDate = System.DateTime.Today;
                            cl.CreateBy = User.Identity.Name;
                            cl.CreateDate = System.DateTime.Now;
                        }
                        dc.CustomerLedgerDetails.Add(cl);
                        // Update Customer Curr balance
                        var d = db.spCustomerBalanceUpdate(A.CustomerID, "L", A.CLAmount);
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