using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MyPepsi.ViewModel.POSMVM;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class PosmSchemeController : Controller

    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: PosmScheme
        public ActionResult Index()
        {
            ViewBag.PosmSchemeLists = new SelectList(db.POSMSchemes.Where(s=>s.SchemeDescription!="NA").OrderBy(x => x.SchemeDescription), "SchemeID", "SchemeDescription");
            return View();
        }
        public JsonResult GetPOSMItemID()
        {

            try
            {
                int pID;
                var a = db.POSMSchemes.Max(p => p.SchemeID);
                pID = a + 1;

                return new JsonResult { Data = pID, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Scheme ID Not Found" });

            }

        }
        public JsonResult GetPOSMSchemeName(int SchemeID)
        {

            try
            {
                string pName;
                var a = (from x in db.POSMSchemes where x.SchemeID == SchemeID select x).FirstOrDefault();
                pName = a.SchemeDescription;

                return new JsonResult { Data = pName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Scheme ID Not Found" });

            }

        }
        public JsonResult SavePOSMItem(POSMScemeVM posmnew)
        {
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    POSMScheme pi = new POSMScheme();
                    {

                        //var V = db.UnionPayments.Max(p => p.MoneyReceiptNo);
                        pi.SchemeID = posmnew.SchemeID;
                        pi.SchemeDescription = posmnew.SchemeDescription;
                    }
                    db.POSMSchemes.Add(pi);
                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Scheme ID Not Found" });
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };//, v = v } };
        }
        public JsonResult UpdatePOSMItem(POSMScemeVM updatePO)
        {
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var result = db.POSMSchemes.SingleOrDefault(x => x.SchemeID == updatePO.SchemeID);
                    if (result != null)
                    {
                        // CashSettlement uc = new CashSettlement();
                        result.SchemeDescription = updatePO.SchemeDescription;
                    }
                    //db.POSMItems.Add(pi);
                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Scheme ID Not Found" });
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