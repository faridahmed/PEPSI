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
    public class ApprovedClaimController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: ApprovedClaim
        public ActionResult Index()
        {
            var clients = db.Customers
    .Select(s => new
    {
        Text = s.CustomerName + " , " + s.CustomerAddress1,
        Value = s.CustomerID
    })
    .ToList();

            ViewBag.CustomerLists = new SelectList(clients, "Value", "Text");

            ViewBag.SchemeName = new SelectList(db.RebateSchemes.OrderBy(x => x.SchemeID), "SchemeID", "SchemeDescription");
            var V = db.ClaimApprovements.Max(p => p.ApprovedID);
            ViewBag.AppID = (V + 1);
            return View(new ApprovedClaimVM());
        }

        public JsonResult GetProductData(int idProduct)
        {
            string PName;

            try
            {
                // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.

                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Products where x.ProductID == idProduct select x).FirstOrDefault();
                    PName = V.ProductDescription;
                }

                return new JsonResult { Data = PName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }

        public JsonResult GetProductValue(int idProduct, int customer, int cases)
        {
            decimal pamount;

            try
            {
                var C = (from x in db.Customers where x.CustomerID == customer select x).FirstOrDefault();
                var A = (from x in db.Products where x.ProductID == idProduct select x).FirstOrDefault();
                var V = (from x in db.ProductRateDetails where x.ProductRateID == C.ProductRateID && x.ProductID == idProduct select x).FirstOrDefault();

                pamount = (((cases * V.UnitPrice) - (cases * V.AgencyCommission)));
                // StockQuantity = Math.Round(StockQuantity, 2);

                //StockQuantity =  Convert.ToDouble(V.OnHandQuantity/A.ConversionFactor);
                //}

                //return new JsonResult { Data = new { ProductAmount = pamount }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return new JsonResult { Data = pamount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Product ID Not Found" });
                //throw ex;
            }
        }

        [HttpPost]
        public JsonResult SaveApprovedClaimData(ApprovedClaimVM approvedClaimnew)
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
                    foreach (var v in approvedClaimnew.AppClaimDetails)
                    {
                        ClaimApprovement pi = new ClaimApprovement();
                        {

                            pi.ApprovedID = v.ApprovedID;
                            pi.ApproveDate = DateTime.Now;
                            pi.SchemeID = v.SchemeID;
                            pi.CustomerID = v.CustomerID;
                            pi.ProductID = v.ProductID;
                            pi.ApproveQuantity = v.ApproveQuantity;
                            pi.ApproveAmount = v.ApproveAmount;
                            pi.EneteredBy = User.Identity.Name;
                            pi.EntryDate = DateTime.Now;
                            db.ClaimApprovements.Add(pi);
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


    }
}