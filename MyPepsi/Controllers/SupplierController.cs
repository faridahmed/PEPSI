using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using static MyPepsi.ViewModel.POSMVM;

namespace PrimarySalesSystem.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        // GET: Supplier
        private PEPSIEntities db = new PEPSIEntities();
        public ActionResult Index()
        {
            ViewBag.SupplierLists = new SelectList(db.Suppliers.Where(s=>s.SupplierName!="NA").OrderBy(x => x.SupplierName), "SupplierID", "SupplierName");
            return View();
        }
        public JsonResult GetSupplierID()
        {

            try
            {
                int SID;
                var a = db.Suppliers.Max(p => p.SupplierID);
                SID = a + 1;

                return new JsonResult { Data = SID, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });

            }

        }
        public JsonResult GetAllInfoSupplier(int suppID)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var a = (from x in db.Suppliers where x.SupplierID == suppID select x).FirstOrDefault();

                return new JsonResult { Data = a, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Supplier ID Not Found" });

            }

        }


        public JsonResult SaveSupplier(SupplierVM supnew)
        {
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    Supplier pi = new Supplier();
                    {

                        //var V = db.UnionPayments.Max(p => p.MoneyReceiptNo);
                        pi.SupplierID = supnew.SupplierID;
                        pi.SupplierName = supnew.SupplierName;
                        pi.Address = supnew.Address;
                        pi.PhoneNo = supnew.PhoneNo;
                    }
                    db.Suppliers.Add(pi);
                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "supplier Not Found" });
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };//, v = v } };
        }
        public JsonResult UpdatePOSMSupplier(SupplierVM updateSupplier)
        {
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var result = db.Suppliers.SingleOrDefault(x => x.SupplierID == updateSupplier.SupplierID);

                    if (result != null)
                    {

                        result.SupplierName = updateSupplier.SupplierName;
                        result.Address = updateSupplier.Address;
                        result.PhoneNo = updateSupplier.PhoneNo;
                    }

                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Supplier ID Not Found" });

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

