using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MyPepsi.ViewModel.POSMVM;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class PosmReceiveController : Controller
    {
        // GET: PosmReceiveVM
        private PEPSIEntities db = new PEPSIEntities();
        public ActionResult Index()
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

            ViewBag.PosmSchemeLists = new SelectList(db.POSMSchemes.OrderBy(x => x.SchemeID), "SchemeID", "SchemeDescription");
            ViewBag.SupplierLists = new SelectList(db.Suppliers.Where(s => s.SupplierName != "NA").OrderBy(x => x.SupplierName), "SupplierID", "SupplierName");
            ViewBag.PosmItemLists = new SelectList(db.POSMItems.Where(s => s.PosmName != "NA").OrderBy(x => x.PosmName), "PosmID", "PosmName");
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            ViewBag.TransactionNo = new SelectList(db.POSMIssues.Where(x => x.Status == "Pending" && x.WareHousID == wn.WarehouseID).Select(x => x.POSMChallanNo).Distinct());

            if (wn.WarehouseID != 61)
            {
                return View("ExceptDhakaWarehouse");
                //return RedirectToAction("ExceptDhakaWarehouse", "POSMIssue");
            }
            else
            {
                return View();
            }

        }
        public JsonResult SavePOSMReceive(POSMReceiveVM posmnew)
        {
            int grNumber;
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

            string s1 = wn.WarehouseID.ToString();
            string s2 = string.Concat(s1 + "000000");
            int GRNo = Convert.ToInt32(s2);
            var maxGRNo = (from n in db.POSMReceives where n.WarehouseID == wn.WarehouseID select n.GRNO).DefaultIfEmpty(GRNo).Max();

            //var V = db.Payments.Where(p => p.WarehouseID == w.WarehouseID).Select(p => p.MoneyReceiptNo).DefaultIfEmpty(100000).Max();
            grNumber = Convert.ToInt32(maxGRNo + 1);


                bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var v in posmnew.PODetails)
                    {
                        POSMReceive pr = new POSMReceive();
                        {

                            pr.GRNO = grNumber;
                            pr.WarehouseID = wn.WarehouseID;
                            pr.ReceivingDate = System.DateTime.Now;
                            pr.SchemeID = v.SchemeID;
                            pr.SupplierID = v.SupplierID;
                            pr.SuppRefNo = v.SuppRefNo;
                            pr.SupplierRefDate = v.SupplierRefDate;
                            pr.POSItem = v.POSItem;
                            pr.Description = v.Description;
                            pr.ReceivedQty = v.ReceivedQty;
                            pr.UnitPrice = v.UnitPrice;
                            pr.WarrentyPeriod = v.WarrentyPeriod;
                            pr.WarrentyType = v.WarrentyType;
                            pr.RcvBy = User.Identity.Name;
                            pr.EntryDate = System.DateTime.Now;
                            db.POSMReceives.Add(pr);
                        }
                        POSMRecIss posmreceiveissueTable = new POSMRecIss();
                        {
                            posmreceiveissueTable.GRChNo = grNumber;
                            posmreceiveissueTable.SupplierID= v.SupplierID;
                            posmreceiveissueTable.WarehouseID = wn.WarehouseID;
                            posmreceiveissueTable.Date = System.DateTime.Now;
                            posmreceiveissueTable.ItemID = v.POSItem;
                                posmreceiveissueTable.SchemeID = v.SchemeID;
                            posmreceiveissueTable.ReceivedQty = v.ReceivedQty;
                            posmreceiveissueTable.RcvRate = v.UnitPrice;
                            posmreceiveissueTable.IssuedQty = 0;
                            posmreceiveissueTable.IssuRate = 0;
                            posmreceiveissueTable.FreshProductReturn = 0;
                            posmreceiveissueTable.DefectiveProductReturn = 0;
                            posmreceiveissueTable.DestroyedProduct = 0;
                           posmreceiveissueTable.EnteredBy = User.Identity.Name;
                            posmreceiveissueTable.EntryDate = System.DateTime.Now;
                                db.POSMRecIsses.Add(posmreceiveissueTable);
                        }
                        POSMStock p = db.POSMStocks.SingleOrDefault(x => x.WarehouseId == wn.WarehouseID && x.POSMItemID == v.POSItem);
                        p.OnHandQty = p.OnHandQty+v.ReceivedQty;
                        p.ReceivedQty = p.ReceivedQty + v.ReceivedQty;
                        db.Entry(p).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    status = true;
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message=ex.Message});
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };//, v = v } };
        }

        public JsonResult SavePOSMReceiveLater(POSMReceiveVM posmnew, int Chno)
        {
            int grNumber;
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

            string s1 = wn.WarehouseID.ToString();
            string s2 = string.Concat(s1 + "000000");
            int GRNo = Convert.ToInt32(s2);
            var maxGRNo = (from n in db.POSMReceives where n.WarehouseID == wn.WarehouseID select n.GRNO).DefaultIfEmpty(GRNo).Max();

            //var V = db.Payments.Where(p => p.WarehouseID == w.WarehouseID).Select(p => p.MoneyReceiptNo).DefaultIfEmpty(100000).Max();
            grNumber = Convert.ToInt32(maxGRNo + 1);



            bool status = false;
            string mes = "";
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var v in posmnew.PODetails)
                    {
                        POSMReceive pr = new POSMReceive();
                        {

                            pr.GRNO = grNumber;
                            pr.WarehouseID = wn.WarehouseID;
                            pr.ReceivingDate = System.DateTime.Now;
                            pr.SchemeID = v.SchemeID;
                            pr.SupplierID = v.SupplierID;
                            // pr.SuppRefNo = 0;
                            //pr.SupplierRefDate = v.SupplierRefDate;
                            pr.POSItem = v.POSItem;
                            pr.Description = "From Other";
                            pr.ReceivedQty = v.ReceivedQty;
                            pr.UnitPrice = 0;
                            // pr.WarrentyPeriod = v.WarrentyPeriod;
                            // pr.WarrentyType = v.WarrentyType;
                            pr.RcvBy = User.Identity.Name;
                            pr.EntryDate = System.DateTime.Now;
                            db.POSMReceives.Add(pr);
                        }
                        POSMRecIss posmreceiveissueTable = new POSMRecIss();
                        {
                            posmreceiveissueTable.GRChNo = grNumber;
                            posmreceiveissueTable.SupplierID = v.SupplierID;
                            posmreceiveissueTable.WarehouseID = wn.WarehouseID;
                            posmreceiveissueTable.Date = System.DateTime.Now;
                            posmreceiveissueTable.ItemID = v.POSItem;
                            posmreceiveissueTable.SchemeID = v.SchemeID;
                            posmreceiveissueTable.ReceivedQty = v.ReceivedQty;
                            //posmreceiveissueTable.RcvRate = v.UnitPrice;
                            posmreceiveissueTable.IssuedQty = 0;
                            posmreceiveissueTable.IssuRate = 0;
                            posmreceiveissueTable.FreshProductReturn = 0;
                            posmreceiveissueTable.DefectiveProductReturn = 0;
                            posmreceiveissueTable.DestroyedProduct = 0;
                            posmreceiveissueTable.EnteredBy = User.Identity.Name;
                            posmreceiveissueTable.EntryDate = System.DateTime.Now;
                            db.POSMRecIsses.Add(posmreceiveissueTable);
                        }
                        POSMStock p = db.POSMStocks.SingleOrDefault(x => x.WarehouseId == wn.WarehouseID && x.POSMItemID == v.POSItem);
                        p.OnHandQty = p.OnHandQty + v.ReceivedQty;
                        p.ReceivedQty = p.ReceivedQty + v.ReceivedQty;
                        db.Entry(p).State = EntityState.Modified;
                        POSMIssue q = db.POSMIssues.SingleOrDefault(x => x.POSMChallanNo == Chno && x.POSItemID == v.POSItem);
                        q.Status = "Received";
                        q.IssueReceivedDate = System.DateTime.Now;
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

        public JsonResult GetChallanDetail(int Chno)
        {


            var pData = db.spGetPOSMIssueChallan(Chno);


            return new JsonResult { Data = pData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}