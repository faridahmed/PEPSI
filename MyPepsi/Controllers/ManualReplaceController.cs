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
    public class ManualReplaceController : Controller
    {
        // GET: ManualReplace
        private PEPSIEntities db = new PEPSIEntities();
        public ActionResult Index()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            ViewBag.CustomerID = new SelectList(db.Customers.Where(x => x.WarehouseID == wn.WarehouseID && x.ActiveStatus == "A").OrderBy(x => x.CustomerName), "CustomerID", "CustomerName");
            ViewBag.MonthName = new SelectList(db.FixedDataTables.Where(x => x.Value == 12 && x.Status == "A"), "TypeCodeID", "TypeDescription");
            ViewBag.GivenMethod = new SelectList(db.FixedDataTables.Where(x => x.Value == 999 && x.Status == "A"), "TypeCodeID", "TypeDescription");
            ViewBag.NoLevel = 0;
            ViewBag.NoCrown = 0;
            ViewBag.NoCan = 0;
            ViewBag.TotalAmount = 0;
            ViewBag.PayableAmount = 0;
            return View();
        }
        public ActionResult DataSave(ReplaceManualVM D)
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
            string s2 = string.Concat(s1 + "710000");
            int RepNo = Convert.ToInt32(s2);
            var maxRepNo = (from n in db.ReplaceManuals select n.ReplaceID).DefaultIfEmpty(RepNo).Max();
            var maxsalno = maxRepNo + 1;
            int v = maxsalno;
            if (ModelState.IsValid)
            {
                ReplaceManual dbo = new ReplaceManual();
                {
                    dbo.CustomerID = D.CustomerID;
                    dbo.ReplaceID = maxsalno;
                    dbo.MonthName = D.MonthName;
                    dbo.GivenMethod = D.GivenMethod;
                    dbo.NoLevel = D.NoLevel;
                    dbo.NoCrown = D.NoCrown;
                    dbo.NoCan = D.NoCan;
                    dbo.TotalAmount = D.TotalAmount;
                    dbo.PayableAmount = D.PayableAmount;
                    dbo.Remarks = D.Remarks;
                    dbo.Status = "P";
                    dbo.CreateDate = DateTime.Now;
                    dbo.CreateBy = User.Identity.Name;

                };
                db.ReplaceManuals.Add(dbo);
                var Payblecreation = db.spCustomerBalanceUpdate(D.CustomerID, "T", D.PayableAmount);
                db.SaveChanges();
                status = true;
                db.Dispose();
                ModelState.Clear();
            }
            else

            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes, v = v } };
        }
    }
}