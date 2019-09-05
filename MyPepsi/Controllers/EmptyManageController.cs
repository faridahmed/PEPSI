using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPepsi.ViewModel;
using System.Data.SqlClient;
namespace MyPepsi.Controllers
{
    [Authorize]
    public class EmptyManageController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        
        // GET: EmptyManage
        public ActionResult Index()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            ViewBag.TransactionTypeID = new SelectList(db.TransactionTypes.Where(x => x.UsedType == "E" && x.Status == "A"), "TransactionTypeID", "TransactionTypeName");
            ViewBag.Vehicle = new SelectList(db.Vehicles.Where(x => x.WarehouseID == w.WorkStationID), "VehicleID", "VehicleRegistrationNo");
            ViewBag.Driver = new SelectList(db.Drivers.Where(x => x.WorkStation == w.WorkStationID && x.Status == "A").ToList(), "DriverID", "DriverName");
            ViewBag.StorageLocationCode = new SelectList(db.StorageLocations.Where(c => c.WarehouseID == wn.WarehouseID && c.Status == "A"), "WarehouseLocationCode", "ShortDescription");
            ViewBag.BinID = new SelectList(db.FixedDataTables.Where(c => c.Value == 20000 && c.Status == "A"), "TypeCodeID", "TypeDescription");
            ViewBag.ToWarehouse = new SelectList(db.Warehouses.Where(x => x.WarehouseID != w.WorkStationID && x.PlantFlag=="Y").ToList(), "WarehouseID", "WarehouseDescription");
            return View();
        }
        public JsonResult GetProductDesc(int idProduct)
        {
            string PName;

            try
            {

                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Products where x.ProductID == idProduct && x.Status == "A" select x).FirstOrDefault();
                    PName = V.ProductDescription;
                }

                return new JsonResult { Data = PName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product Description Not Found" });
                // throw ex;
            }
        }
        public JsonResult GetPlasticData(int idProduct)
        {
            int PB;
            try
            {

                var V = (from x in db.Products where x.ProductID == idProduct && x.Status == "A" select x).FirstOrDefault();

                PB = V.BottleReturnable;
                return new JsonResult { Data = PB, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                // throw ex;
            }
        }
        [HttpPost]
        public ActionResult SaveEmptyMove(EmptyMoveVM D)
        {
            bool status = false;
            string mes = "";
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "100000");
            int TrNo = Convert.ToInt32(s2);
            var maxNO = (from n in db.EmptyMoveMasters where n.WarehouseID == w.WorkStationID select n.TransactionNo).DefaultIfEmpty(TrNo).Max();
            var maxTrNo = maxNO + 1;
            int v = maxTrNo;
            if (ModelState.IsValid)
            {
                if (D.TransactionType == 28)
                    {
                        D.Status = "A";
                    }
                    else if (D.TransactionType == 29)
                    {
                        D.Status = "T";

                    }
                    else if (D.TransactionType == 30)
                    {
                        D.Status = "C";

                    }
                    else if (D.TransactionType == 31)
                    {
                        D.Status = "P";

                    }
                    else if (D.TransactionType == 32)
                    {
                        D.Status = "D";

                    }
                else if (D.TransactionType == 34)
                {
                    D.Status = "O";

                }
                else
                    {
                        D.Status = "A";
                    }
            
                if (D.DriverID == null)
                {
                    D.DriverID = 0;
                }
                if (D.LocationID == null)
                {
                    D.LocationID = "TBL000";
                }
                if (D.BinNo == null)
                {
                    D.BinNo = "20000";
                }
                EmptyMoveMaster dbo = new EmptyMoveMaster
                {
                        TransactionNo = maxTrNo,
                        WarehouseID = D.WarehouseID,
                        ReferenceNo = D.ReferenceNo,
                        ToWarehouse = D.ToWarehouse,
                        FromWarehouse = D.WarehouseID,
                        DriverID = D.DriverID,
                        VehicleNo = D.VehicleNo,
                        TotalEmpty = D.TotalEmpty,
                        TotalBox = D.TotalBox,
                        TransactionType = D.TransactionType,
                        LocationID = D.LocationID,
                        BinNo = D.BinNo,
                        Status = D.Status,
                        Remarks = D.Remarks,
                        TransactionDate = DateTime.Today,                    
                        CreateDate = DateTime.Now,
                        CreateBy = User.Identity.Name
                    };
                    db.EmptyMoveMasters.Add(dbo);
                    foreach (var i in D.emptydetail)
                    {
                        EmptyMoveDetail obd = new EmptyMoveDetail();
                        obd.WarehouseID = D.WarehouseID;
                        obd.ProductID = i.ProductID;
                        obd.TransactionNo = maxTrNo;
                        obd.Boxes = i.Boxes;
                        obd.QuantityCase = i.QuantityCase;
                        obd.CreateBy = User.Identity.Name;
                        obd.CreateDate = DateTime.Now;
                        db.EmptyMoveDetails.Add(obd);
                    }
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
        public ActionResult EmptyTransfferReceive()
        {
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            ViewBag.WarehouseID = wn.WarehouseID;
            ViewBag.WarehouseIDLogin = wn.WarehouseDescription;
            ViewBag.TransactionNo = new SelectList(db.EmptyMoveMasters.Where(x => x.ToWarehouse == wn.WarehouseID && x.Status == "T" && x.TransactionType == 29 && x.UpdatedBy == null && x.UpdatedBy == null), "TransactionNo", "TransactionNo");
            ViewBag.StorageLocationCode = new SelectList(db.StorageLocations.Where(c => c.WarehouseID == wn.WarehouseID), "WarehouseLocationCode", "ShortDescription");
            ViewBag.StorageBin = new SelectList(db.FixedDataTables.Where(c => c.Value == 20000 && c.Status=="A"), "TypeCodeID", "TypeDescription");
            return View("EmptyTransfferReceive");

        }
        [HttpGet]
      public JsonResult EmptyDetailInfo(int inWarehouseID, int inTrNo)
        {


            var pData = db.spEmptyTransferDetail(inWarehouseID, inTrNo);


            return new JsonResult { Data = pData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult TransfferData( EmptyTransferVM S )
        {
            bool status = false;
            string mes = "";
            var w = (from y in db.UserLogins
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(x => x.WarehouseID == w.WorkStationID).FirstOrDefault();
            string s1 = w.WorkStationID.ToString();
            string s2 = string.Concat(s1 + "100000");
            int TrNo = Convert.ToInt32(s2);
            var maxNO = (from n in db.EmptyMoveMasters where n.WarehouseID == w.WorkStationID select n.TransactionNo).DefaultIfEmpty(TrNo).Max();
            var maxTrNo = maxNO + 1;
            int v = maxTrNo;

            try
            {
             if (ModelState.IsValid)
                {
                 EmptyMoveMaster dm = new EmptyMoveMaster();
                   {                          
                            dm.TransactionType = 33;
                            dm.TransactionNo = maxTrNo;
                            dm.WarehouseID = S.WarehouseID;
                            dm.FromWarehouse = S.FromWarehouse;
                            dm.ToWarehouse = S.WarehouseID;
                            dm.TransactionDate = DateTime.Today;
                            dm.TotalEmpty = S.TotalEmpty;
                            dm.TotalBox = S.TotalBox;
                            dm.BinNo = S.BinNo;
                            dm.LocationID = S.LocationID;
                            dm.DriverID = S.DriverID;
                            dm.VehicleNo = "0";
                            dm.ReferenceNo = S.ReferenceNo;
                            dm.Remarks = S.Remarks;
                            dm.Status = "A";
                            dm.CreateBy = User.Identity.Name;
                            dm.CreateDate = DateTime.Now;
                        }
                        db.EmptyMoveMasters.Add(dm);
                        foreach (var i in S.emptydetail)
                        {
                            EmptyMoveDetail dmd = new EmptyMoveDetail();
                            dmd.ProductID = i.ProductID; 
                            dmd.WarehouseID = S.WarehouseID;
                            dmd.TransactionNo = maxTrNo;
                            dmd.QuantityCase = i.QuantityCase;
                            dmd.Boxes = i.Boxes;
                            dmd.CreateBy = User.Identity.Name;
                            dmd.CreateDate = DateTime.Now;
                            db.EmptyMoveDetails.Add(dmd);                        
                        }
                    var pt = db.spEmptyTransferInformation(S.FromWarehouse,S.TransactionNo,"U", User.Identity.Name);
                    db.SaveChanges();
                    status = true;
                    db.Dispose();
                }
                else
                {
                    status = false;
                }
                return new JsonResult { Data = new { status = status, mes = mes, v = v } };
            }
            catch (Exception ex)
            {
                string mess = ex.Message;            
                return Json(new { status = "error", message = "Transaction No  Not Found" });              
                
            }

        }

    }
}