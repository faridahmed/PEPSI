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
    public class POSMItemController : Controller
    {
        // GET: POSMItem
        private PEPSIEntities db = new PEPSIEntities();
        public ActionResult Index()
        {
            ViewBag.PosmItemLists = new SelectList(db.POSMItems.Where(s=>s.PosmName!="NA").OrderBy(x => x.PosmName), "PosmID", "PosmName");
            //ViewBag.PTransactionType = new SelectList(db.PaymentTransactionTypes, "PaymentTransactionType1", "PaymentTransactionName");
            return View();
        }
        public JsonResult GetPOSMItemID()
        {

            try
            {
                int pID;
                var a = db.POSMItems.Max(p => p.PosmID);
                pID = a + 1;

                return new JsonResult { Data = pID, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });

            }

        }
        public JsonResult GetPOSMItemIDName(int pID)
        {

            try
            {
                string pName;
                var a = (from x in db.POSMItems where x.PosmID == pID select x).FirstOrDefault();
                pName = a.PosmName;

                return new JsonResult { Data = pName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });

            }

        }

        public JsonResult SavePOSMItem(POSMItemVM posmnew)
        {
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    POSMItem pi = new POSMItem();
                    {

                        //var V = db.UnionPayments.Max(p => p.MoneyReceiptNo);
                        pi.PosmID = posmnew.PosmID;
                        pi.PosmName = posmnew.PosmName;
                        db.POSMItems.Add(pi);
                    }
                    var WarehouseNumber = db.Warehouses;
                    foreach (var v in WarehouseNumber)
                    {
                        POSMStock pStock = new POSMStock();
                        {
                       
                            pStock.WarehouseId = v.WarehouseID;
                            pStock.POSMItemID = posmnew.PosmID;
                            pStock.OnHandQty = 0;
                            pStock.ReceivedQty =0;
                            pStock.IssuedQty = 0;
                            pStock.ReturnedFreshQty =0;
                            pStock.ReturnedDefectedQty = 0;
                            pStock.DestroyedQty = 0;
                            db.POSMStocks.Add(pStock);
                        }
                    }
                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Product ID Not Found" });
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };//, v = v } };
        }
        public JsonResult UpdatePOSMItem(POSMItemVM updatePO)
        {
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var result = db.POSMItems.SingleOrDefault(x => x.PosmID == updatePO.PosmID);
                    if (result != null)
                    {
                        // CashSettlement uc = new CashSettlement();
                        result.PosmName = updatePO.PosmName;
                    }
                    //db.POSMItems.Add(pi);
                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Product ID Not Found" });
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