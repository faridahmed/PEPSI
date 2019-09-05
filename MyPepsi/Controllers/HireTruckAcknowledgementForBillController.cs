using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class HireTruckAcknowledgementForBillController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: HireTruckAcknowledgementForBill
        public ActionResult Index()
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

            ViewBag.InvoicePrTransaction = new SelectList(db.TransportAgencyandFareSetups.Where(x => x.Status=="No" && x.WarehouseID==wn.WarehouseID), "Sl", "ChallanNumber");
            ViewBag.SelectAgency = new SelectList(db.TransportAgencySetups.Where(x => x.EligibleForCarringGoods == 1).ToList(), "TAName", "TAName", "0");
            //ViewBag.TrAgency = new SelectList(db.TransportAgencySetups.Where(x => x.EligibleForCarringGoods == 1 && x.WarehouseID == wn.WarehouseID), "TAID", "TAName");
            return View();
        }
        public JsonResult GetChallanInfo(int challanNumber)
        {

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var result = db.spGetTransportFareSetupChallanInfo(challanNumber).ToList();
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Challan Details not found" });

            }

        }
        [HttpPost]
        public JsonResult AcknoledgementData(TransportAgencyandFareSetupVM trAcknowledge)
        {

            //int GRNo;
            //var a = db.POSMReceives.Max(p => p.GRNO);
            //GRNo = Convert.ToInt32(a + 1);
            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    TransportAgencyandFareSetup v = db.TransportAgencyandFareSetups.SingleOrDefault(x => x.ChallanNumber == trAcknowledge.ChallanNumber);
                    v.VechileNo = trAcknowledge.VechileNo;
                    v.AcknowledgementDate = trAcknowledge.AcknowledgementDate;
                    v.Status = "Yes";
                    v.ActualAcknowledgementDate = System.DateTime.Now;
                    v.UpdatedBy = User.Identity.Name;
                    v.UpdatedDate = System.DateTime.Now;
                    v.EmptyReturn = trAcknowledge.EmptyReturn;
                    db.Entry(v).State = EntityState.Modified;
                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "Not Updated" });
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };//, v = v } };
        }
        [HttpGet]
        public JsonResult GetinvoiceHT(string sItem)
        {

            try
            {
                var wa = (from y in db.UserLogins
                          where y.UserID.ToString() == User.Identity.Name
                          select new { y.WorkStationID }).FirstOrDefault();
                db.Configuration.ProxyCreationEnabled = false;
                var invNo= (from yt in db.TransportAgencyandFareSetups
                            where yt.Status.ToString() == "No" && yt.WarehouseID==wa.WorkStationID
                            select new { yt.ChallanNumber }).ToList();
                return new JsonResult { Data = invNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Invoice not found" });

            }

        }

    }
}