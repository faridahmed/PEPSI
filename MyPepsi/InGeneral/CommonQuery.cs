using MyPepsi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.InGeneral
{
    public class CommonQuery : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();

        public JsonResult GetProductData(int idProduct)
        {
            string PName;

            try
            {
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
                throw ex;
            }
        }
    }
}