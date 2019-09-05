using MyPepsi;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrimarySalesSystem.Controllers
{
    [Authorize]
    public class DeliveryControlController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: DeliveryControl
        public ActionResult Index()
        {
            ViewBag.CustomerLists = new SelectList(db.Customers.Where(x => x.ActiveStatus == "A").OrderBy(x => x.CustomerName), "CustomerID", "CustomerName");
            return View(new DeliveryControlVM());
        }

        public JsonResult EditLimit(int custID)
        {

            try
            {
                var result = db.DeliveryControls.SingleOrDefault(x => x.CustomerID == custID && x.IsActive == true);
                //DateTime dt = result.IsRcvlEDate;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });

            }

        }
        [HttpPost]
        public JsonResult UpdateLimit(DeliveryControlVM DCM)
        {
            bool status = false;
            string mes = "";
            try
            {
                DeliveryControl v = db.DeliveryControls.SingleOrDefault(x => x.CustomerID == DCM.CustomerID && x.IsActive == true);
                v.RcvlAmnt = DCM.RcvlAmnt;
                v.IsRcvlEDate = DCM.IsRcvlEDate.Date;
                v.Remarks = DCM.Remarks;
                v.IsBGurated = DCM.IsBGurated;


                //var v = db.DeliveryControls.SingleOrDefault(x => x.CustomerID == DCM.CustomerID && x.IsActive == true);
                //v.RcvlAmnt = DCM.RcvlAmnt;
                //v.IsRcvlEDate = DCM.IsRcvlEDate;
                //v.Remarks = DCM.Remarks;
                //v.IsBGurated = DCM.IsBGurated;
                //v.IsActive = DCM.IsActive;


                //db.DeliveryControls.Attach(v);
                db.Entry(v).State = EntityState.Modified;
                db.SaveChanges();
                status = true;
                return new JsonResult { Data = new { status = status, mes = mes } };
        }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Not Found" });
                //throw ex;
            }

        }
    }
}