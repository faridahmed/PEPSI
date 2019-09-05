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
    public class CustomerController : Controller
    {
        // GET: Customer
        private PEPSIEntities db = new PEPSIEntities();
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
                ViewBag.WarehouseID = new SelectList(db.Warehouses.Where(x => x.WarehouseID == userwarehouse), "WarehouseID", "WarehouseDescription");
            }
            ViewBag.CustomerTypeID = new SelectList(db.FixedDataTables.Where(x => x.Value == 200), "TypeCodeID", "TypeDescription");
            ViewBag.CustomerExecutiveID = new SelectList(db.CustomerExecutives, "CustomerExecutiveID", "CustomerExecutiveName");
            ViewBag.SalesPersonID = new SelectList(db.SalesPersons, "SalesPersonID", "SalesPersonName");
            ViewBag.RegionID = new SelectList(db.Regions, "RegionID", "RegionDescription");
            ViewBag.ProductRateID = new SelectList(db.ProductRates, "ProductRateID", "ProductRateDescription");
            ViewBag.ClusterName = new SelectList(db.FixedDataTables.Where(x => x.Value == 111), "TypeCodeID", "TypeDescription");
            return View();
        }

        [HttpPost]
        public JsonResult SaveCustomer(CustomerVM A)
        {
            bool status = false;
            string mes = "";
            int maxCus;
            maxCus = (from x in db.Customers where x.WarehouseID == A.WarehouseID select x).Max(p => p.CustomerID) + 1;
            //var c = db.Customers.Where(t => t.CustomerID == A.CustomerID).FirstOrDefault();
            int v = maxCus;
            if (ModelState.IsValid)
            {
                try
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {
                        Customer c = new Customer();
                        {
                            c.CustomerID = maxCus;
                            c.CustomerName = A.CustomerName;
                            c.ProprietorsName = A.ProprietorsName;
                            //c.CustomerShortName = A.ProprietorsName;
                            c.ContactPersonsName = A.ProprietorsName;
                            c.WarehouseID = A.WarehouseID;
                            c.CustomerTypeID = A.CustomerTypeID;
                            c.CustomerAddress1 = A.CustomerAddress1;
                            c.CustomerAddress2 = A.CustomerAddress1;
                            c.CustomerPhone = A.CustomerPhone;
                            c.CustomerMobilePhone = A.CustomerPhone;
                            c.CustomerExecutiveID = A.CustomerExecutiveID;
                            c.SalesPersonID = A.SalesPersonID;
                            c.RegionID = A.RegionID;
                            c.ClusterName = A.ClusterName;
                            c.ProductRateID = A.ProductRateID;
                            c.CustomerStartingDate = DateTime.Today;
                            c.VATRegistrationNumber = A.VATRegistrationNumber;
                            c.ActiveStatus = "A";
                            c.CreateBy = User.Identity.Name;
                            c.CreateDate = System.DateTime.Now;

                        }
                        dc.Customers.Add(c);
                        CustomerLedgerMaster cl = new CustomerLedgerMaster();
                        {
                            cl.CustomerID = maxCus;
                            cl.CurrentBalance = 0;
                            cl.BankGuranteeAmt = 0;
                            cl.BlockedAmt = 0;
                            cl.BookedAmt = 0;
                            cl.CheckingFlag = "Y";
                            cl.CreditAmt = 0;
                            cl.OpeningBalance = 0;
                            cl.PaymentAmt = 0;
                            cl.ReplacementPayable = 0;
                            cl.ReturnAmt = 0;
                            cl.TotalSequrityAmt = 0;
                            cl.CustomerStatus = "A";
                            cl.SequrityReceive = 0;
                            cl.SequrityRefund = 0;
                            cl.TotalSequrityAmt = 0;
                            cl.LoadUnloadCharge = 0;
                            cl.OutstandingAmt = 0;
                            cl.InvoiceAmt = 0;
                            cl.CreateBy = User.Identity.Name;
                            cl.WarehouseID = A.WarehouseID;
                            cl.CreateDate = System.DateTime.Now;
                        }
                        dc.CustomerLedgerMasters.Add(cl);
                        TransportFareSetup tf = new TransportFareSetup();
                        tf.CustomerID =  maxCus;
                        tf.CustomerName = A.CustomerName;
                        tf.WarehouseID = A.WarehouseID;
                        tf.Address = A.CustomerAddress1;
                        tf.IsActive = 1;
                        tf.Rate1 = 3000;
                        tf.Rate2 = 4000;
                        tf.Rate3 = 6000;
                        tf.Date = DateTime.Today;
                        dc.TransportFareSetups.Add(tf);
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