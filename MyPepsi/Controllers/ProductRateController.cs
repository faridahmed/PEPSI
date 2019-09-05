using MyPepsi;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrimarySalesSystem.Controllers
{
    [Authorize]
    public class ProductRateController : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();
        // GET: ProductRate
        public ActionResult Index()
        {
            var Prate = db.ProductRates.Select(s => new { Text = s.ProductRateID + "-" + s.ProductRateDescription, Value = s.ProductRateID }).ToList();

            ViewBag.PRate = new SelectList(Prate, "Value", "Text");
            // ViewBag.SchemeLists = new SelectList(db.RebateSchemes.OrderBy(x => x.SchemeDescription), "SchemeID", "SchemeDescription");
            return View(new ProductRateVM());
        }
        public JsonResult ShowRateDetails(int idPRate)
        {
            try
            {
                // List<ProductRateDetailVM> prDetail
                db.Configuration.ProxyCreationEnabled = false;
                //var scheme = db.RebateSchemes;
                var v = from productRateDetail in db.ProductRateDetails.Where(x => x.ProductRateID == idPRate)
                        join product in db.Products on productRateDetail.ProductID equals product.ProductID
                        select new
                        {
                            productRateDetail.ProductID,
                            product.ProductDescription,
                            productRateDetail.UnitPrice,
                            productRateDetail.AlternateUnitPrice,
                            productRateDetail.AgencyCommission,
                            productRateDetail.AlternateAgencyCommission,
                            productRateDetail.SecurityDeposit,
                            productRateDetail.AlternateSecurityDeposit,
                            productRateDetail.PlasticBoxSecurity,
                            productRateDetail.MRPRate
                        };
                var genericResult = new { Result = v }; //{ Result =v, ID = scheme };
                // var v   = db.ProductRateDetails.Where(x=>x.ProductRateID == idPRate);
                return new JsonResult { Data = genericResult, MaxJsonLength = 100000, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //            ViewBag.DetailsProductRate = v;
                //return PartialView("showRateDetail");
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Not Found" });

            }
        }

        //public JsonResult changeAmount(int idPRate, int PrID)
        //{
        //    try
        //    {
        //        // List<ProductRateDetailVM> prDetail
        //        db.Configuration.ProxyCreationEnabled = false;
        //        var result = (from x in db.ProductRateDetails where x.ProductRateID == idPRate && x.ProductID == PrID select x).FirstOrDefault();
        //        var genericResult = result.AgencyCommission;
        //        // var v   = db.ProductRateDetails.Where(x=>x.ProductRateID == idPRate);
        //        return new JsonResult { Data = genericResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { status = "error", message = "Not Found" });

        //    }
        //}

        [HttpPost]
        public ActionResult UpdateDetail(ProductRateVM prvm)
        {
            bool status = false;
            string mes = "";
            try
            {

                foreach (var i in prvm.PRDVM)
                {
                    var result = (from x in db.ProductRateDetails where x.ProductRateID == prvm.ProductRateID && x.ProductID == i.ProductID select x).FirstOrDefault();
                    result.UnitPrice = i.UnitPrice;
                    result.AlternateUnitPrice = i.AlternateUnitPrice;
                    result.AgencyCommission = i.AgencyCommission;
                    result.AlternateAgencyCommission = i.AlternateAgencyCommission;
                    result.SecurityDeposit = i.SecurityDeposit;
                    result.AlternateSecurityDeposit = i.AlternateSecurityDeposit;
                    result.PlasticBoxSecurity = i.PlasticBoxSecurity;
                    result.MRPRate = i.MRPRate;
                    result.Status = "A";
                    result.UpdatedBy = User.Identity.Name;
                    result.UpdatedDate = DateTime.Now;
                    //string sid = i.SchemeDescription;
                }
                db.SaveChanges();
                status = true;
                //return Json(status= status, mes = mes, JsonRequestBehavior.AllowGet);
                return Json(new { status = status, mes = mes, MaxJsonLength = 10000000, JsonRequestBehavior.AllowGet });
            }

            catch (Exception ex)
            {

                return Json(new { status = "error", message = "Product ID Not Found" });
            }
        }
    }
}



