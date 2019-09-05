using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class DBTargetController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: DBTarget
        public ActionResult Index()
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();


            var clients = db.Customers.Where(x => x.WarehouseID == wn.WarehouseID)
    .Select(s => new
    {
        Text = s.CustomerName + " , " + s.CustomerAddress1,
        Value = s.CustomerID
    })
    .ToList();

            ViewBag.CustomerLists = new SelectList(clients, "Value", "Text");
            ViewBag.targetType = new SelectList(db.DBTargetTypes, "TargetTypeID", "TargetType","1");
            string YearMonth;
            YearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString();
            ViewBag.TargetID = YearMonth;
            return View();
        }
    }
}