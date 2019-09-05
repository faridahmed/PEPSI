using MyPepsi.Models;
using MyPepsi.ViewModel;
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
    public class POSMIssueController : Controller
    {
        // GET: POSMIssue
        private PEPSIEntities db = new PEPSIEntities();
        public ActionResult Index()
        {
            var wa = (from y in db.UserLogins
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();


            var clients = db.Customers.Where(x => x.WarehouseID == wn.WarehouseID && x.ActiveStatus == "A")
    .Select(s => new
    {
        Text = s.CustomerName + " , " + s.CustomerAddress1,
        Value = s.CustomerID
    })
    .ToList();

            ViewBag.CustomerLists = new SelectList(clients, "Value", "Text");

            ViewBag.PosmIssueType = new SelectList(db.POSMIssueTypes.OrderBy(x => x.IssueTypeID), "IssueTypeID", "TypeDescription");
            ViewBag.WarehouseName = new SelectList(db.Warehouses.OrderBy(x => x.WarehouseID), "WarehouseID", "WarehouseDescription", "0");
            ViewBag.VehicleName = new SelectList(db.Vehicles.OrderBy(x => x.VehicleID), "VehicleID", "VehicleRegistrationNo");
            ViewBag.DriverName = new SelectList(db.Drivers.OrderBy(x => x.DriverID), "DriverID", "DriverName");
            ViewBag.POItem = new SelectList(db.POSMItems.OrderBy(x => x.PosmID), "PosmID", "PosmName");
            ViewBag.POScheme = new SelectList(db.POSMSchemes.OrderBy(x => x.SchemeID), "SchemeID", "SchemeDescription");
            ViewBag.fWarehouse = wn.WarehouseID;
            //ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            //ViewBag.TransactionNo = new SelectList(db.POSMIssues.Where(x => x.Status =="Pending" && x.WareHousID == wn.WarehouseID).Select(x=>x.POSMChallanNo).Distinct());
            ////var V = db.POSMIssues.Max(p => p.POSMChallanNo);
            //if (V != null)
            //{
            //    int chNumber = Convert.ToInt32(V + 1);
            //    ViewBag.ChallanNumber = chNumber;
            //}
            //else
            //{
            //    int chNumber = 1000000;
            //    ViewBag.ChallanNumber = chNumber;
            //}
            int chNumber;
            //var w = db.POSMIssues.Where(x => x.CustomerID == paymentsystem.CustomerID).FirstOrDefault();
            string s1 = wn.WarehouseID.ToString();
            string s2 = string.Concat(s1 + "4000000");
            int CHNo = Convert.ToInt32(s2);
            var maxChNO = (from n in db.POSMIssues where n.FromWarehouse == wn.WarehouseID select n.POSMChallanNo).DefaultIfEmpty(CHNo).Max();

            //var V = db.Payments.Where(p => p.WarehouseID == w.WarehouseID).Select(p => p.MoneyReceiptNo).DefaultIfEmpty(100000).Max();
            chNumber = Convert.ToInt32(maxChNO + 1);
            ViewBag.ChallanNumber = chNumber;

            return View();

        }

        [HttpPost]
        public JsonResult SavePOSMIssue(POSMIssueVM posmnew)
        {
            //int chNumber;
            //var wa = (from y in db.UserLogins
            //          where y.UserID.ToString() == User.Identity.Name
            //          select new { y.WorkStationID }).FirstOrDefault();
            //var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

            //string s1 = wn.WarehouseID.ToString();
            //string s2 = string.Concat(s1 + "300000");
            //int CHNo = Convert.ToInt32(s2);
            //var maxCHNo = (from n in db.POSMIssues where n.FromWarehouse == wn.WarehouseID select n.GRNO).DefaultIfEmpty(CHNo).Max();

            ////var V = db.Payments.Where(p => p.WarehouseID == w.WarehouseID).Select(p => p.MoneyReceiptNo).DefaultIfEmpty(100000).Max();
            //chNumber = Convert.ToInt32(maxCHNo + 1);

            bool status = false;
            string mes = "";
            //if (ModelState.IsValid)
            //{
            try
            {
                foreach (var v in posmnew.PODetails)
                {
                    POSMIssue pi = new POSMIssue();
                    {

                        pi.POSMChallanNo = v.POSMChallanNo;
                        pi.FromWarehouse = v.FromWarehouse;
                        pi.IssuingDate = v.IssuingDate;//System.DateTime.Now;
                        pi.IssueTypeID = v.IssueTypeID;
                        if (v.DistributorID == null)
                        {
                            pi.DistributorID = 0;
                        }
                        else
                        {
                            pi.DistributorID = v.DistributorID;
                        }
                        pi.RefDChallan = v.RefDChallan;

                        if (v.WareHousID == null)
                        {
                            pi.WareHousID = 0;
                        }
                        else
                        {
                            pi.WareHousID = v.WareHousID;
                        }
                        pi.TransferNote = v.TransferNote;

                        if (v.VehicleID == null)
                        {
                            pi.VehicleID = 0;
                        }
                        else
                        {
                            pi.VehicleID = v.VehicleID;
                        }

                        if (v.DriverID == null)
                        {
                            pi.DriverID = 0;
                        }
                        else
                        {
                            pi.DriverID = v.DriverID;
                        }

                        pi.POSItemID = v.POSItemID;
                        if (v.SchemeID == null)
                        {
                            pi.SchemeID = 0;
                        }
                        else
                        {
                            pi.SchemeID = v.SchemeID;
                        }

                        pi.SchemeID = v.SchemeID;
                        pi.IssuedQty = v.IssuedQty;
                        pi.IssuingUnitPrice = 0;//v.IssuingUnitPrice;
                                                //Status
                        if (v.IssueTypeID != 3)
                        {
                            pi.Status = "DrIssued";
                        }
                        else
                        {
                            pi.Status = "Pending";
                        }

                        pi.IssuedBy = User.Identity.Name;
                        pi.EntryDate = System.DateTime.Now;
                        //pi.IssueReceivedDate =;
                        db.POSMIssues.Add(pi);
                    }
                    POSMRecIss posmreceiveissueTable = new POSMRecIss();
                    {
                        posmreceiveissueTable.GRChNo = v.POSMChallanNo;
                        posmreceiveissueTable.SupplierID = 0;
                        posmreceiveissueTable.WarehouseID = v.FromWarehouse;
                        posmreceiveissueTable.Date = v.IssuingDate;
                        posmreceiveissueTable.ItemID = v.POSItemID;
                        posmreceiveissueTable.SchemeID = v.SchemeID;
                        posmreceiveissueTable.ReceivedQty = 0;
                        posmreceiveissueTable.RcvRate = 0;
                        posmreceiveissueTable.IssuedQty = v.IssuedQty;
                        posmreceiveissueTable.IssuRate = 0;
                        posmreceiveissueTable.FreshProductReturn = 0;
                        posmreceiveissueTable.DefectiveProductReturn = 0;
                        posmreceiveissueTable.DestroyedProduct = 0;
                        posmreceiveissueTable.EnteredBy = User.Identity.Name;
                        posmreceiveissueTable.EntryDate = System.DateTime.Now;
                        db.POSMRecIsses.Add(posmreceiveissueTable);
                    }
                    POSMStock p = db.POSMStocks.SingleOrDefault(x => x.POSMItemID == v.POSItemID && x.WarehouseId == v.FromWarehouse);
                    p.OnHandQty = p.OnHandQty - v.IssuedQty;
                    p.IssuedQty = p.IssuedQty + v.IssuedQty;
                    db.Entry(p).State = EntityState.Modified;
                }
                //Deduct from stock

                //DeliveryControl v = db.DeliveryControls.SingleOrDefault(x => x.CustomerID == DCM.CustomerID && x.IsActive == true);
                //v.RcvlAmnt = DCM.RcvlAmnt;
                //v.IsRcvlEDate = DCM.IsRcvlEDate.Date;
                //v.Remarks = DCM.Remarks;
                //v.IsBGurated = DCM.IsBGurated;

                //db.Entry(v).State = EntityState.Modified;



                db.SaveChanges();
                status = true;
                return new JsonResult { Data = new { status = status, mes = mes } };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
       // }
        //else
        //{
        //    status = false;
        //}
        // return new JsonResult { Data = new { status = status, mes = mes } };//, v = v } };
    }

        public JsonResult GetCustomerInvoiceNumber(int cId)
        {
            using (PEPSIEntities dc = new PEPSIEntities())
            {

                try
                {
                    var invoiceLists = db.Orders.Where(x => x.CustomerID == cId).OrderByDescending(x=>x.ChallanNumber);

                    return new JsonResult { Data = invoiceLists, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "invoiceLists Not Found" });
                    // throw ex;
                }
            }
        }
        public JsonResult GetStockQty(int pId)
        {
            int stockq;

            try
            {
                // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var wa = (from y in db.UserLogins
                              where y.UserID.ToString() == User.Identity.Name
                              select new { y.WorkStationID }).FirstOrDefault();
                    var wn = db.Warehouses.Where(x => x.WarehouseID == wa.WorkStationID).FirstOrDefault();

                    var V = (from x in dc.POSMStocks where x.POSMItemID == pId && x.WarehouseId == wn.WarehouseID select x).FirstOrDefault();
                    stockq = Convert.ToInt32(V.OnHandQty);
                }

                return new JsonResult { Data = stockq, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product Description Not Found" });
                // throw ex;
            }
        }

    }
}