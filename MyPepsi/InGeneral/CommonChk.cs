using MyPepsi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.InGeneral
{
    public class CommonChk : Controller
    {
        private PEPSIEntities db = new PEPSIEntities();

        public bool chkCustomer(int custid)
        {
            bool t = false;
            int f = db.Customers.Where(x => (x.CustomerID == custid)).Count();

            if (f == 0)
            {
                t = true;
            }
            return t;
        }
        public bool stockchk(int proId, int pWarehouse, int pCases)
        {
            // bool t = false;
            int tflag = 0;
            int StockBalance = 0;
            int OnHandQty = 0;
            decimal conversionFactor = 0;
            decimal CaseToPcs = 0;
            var P = (from x in db.Products where x.ProductID == proId select x).FirstOrDefault();
            var productbalance = (from x in db.ProductBalances where x.ProductID == proId && x.WarehouseID == pWarehouse select x).FirstOrDefault();
            OnHandQty = productbalance.OnHandQuantity;
            conversionFactor = Convert.ToDecimal(P.ConversionFactor);
            CaseToPcs = (pCases * conversionFactor);
            CaseToPcs = Math.Round(CaseToPcs);
            StockBalance = OnHandQty;
            if (CaseToPcs > StockBalance)
            {
                tflag = tflag + 1;
            }
            else
            {
                tflag = tflag + 0;
            }
            if (tflag == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool stockchkcs(int proId, int pWarehouse, int pQtypcs)
        {
            // bool t = false;
            int tflag = 0;
            int StockBalance = 0;
            int OnHandQty = 0;
            // decimal conversionFactor = 0;
            int CaseToPcs = 0;
            // var P = (from x in db.Products where x.ProductID == proId select x).FirstOrDefault();
            var productbalance = (from x in db.ProductBalances where x.ProductID == proId && x.WarehouseID == pWarehouse select x).FirstOrDefault();
            OnHandQty = productbalance.OnHandQuantity;
            //conversionFactor = Convert.ToDecimal(P.ConversionFactor);
            //CaseToPcs = (pQtypcs * conversionFactor);
            CaseToPcs = pQtypcs;
            StockBalance = OnHandQty;
            if (CaseToPcs > StockBalance)
            {
                tflag = tflag + 1;
            }
            else
            {
                tflag = tflag + 0;
            }
            if (tflag == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool chkDriver(int driver)
        {
            bool t = false;
            int f = db.Drivers.Where(x => (x.DriverID == driver)).Count();

            if (f == 0)
            {
                t = true;
            }
            return t;
        }
        //public IEnumerable<SelectListItem> getProductGrp()
        //{
        //  //  IEnumerable<SelectListItem> pro = new SelectList(db.ProductGroups.Where(x => x.Status == "Y"), "ProductGroupID", "ProductGroupDescription");
        //  //  return pro;
        //}

        public int CasetoPcs(int inProdID, int pCases)
        {
            decimal CaseToPcsqty = 0;
            decimal conversionFactor = 0;
            var P = (from x in db.Products where x.ProductID == inProdID select x).FirstOrDefault();
            conversionFactor = Convert.ToDecimal(P.ConversionFactor);
            CaseToPcsqty = (pCases * conversionFactor);
            int pcsqty = Convert.ToInt32(CaseToPcsqty);

            return pcsqty;// new JsonResult { Data = CaseToPcs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public int CasetoPcs(int inProdID, decimal pCases)
        {
            decimal CaseToPcsqty = 0;
            decimal conversionFactor = 0;
            var P = (from x in db.Products where x.ProductID == inProdID select x).FirstOrDefault();
            conversionFactor = Convert.ToDecimal(P.ConversionFactor);
            CaseToPcsqty = (pCases * conversionFactor);
            int pcsqty = Convert.ToInt32(CaseToPcsqty);

            return pcsqty;// new JsonResult { Data = CaseToPcs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                throw ex;
            }
        }

        public string GetCustomerName(int inCustID, string inWC)
        {
            string CustName = "N/A";

            try
            {
                // Here "MyDatabaseEntities " is dbContext, which is created at time of model creation.
                // var allOrder = db.GetDBOrderDetail(salesorderno);
                //   IList<OrderBankDetail> d = new List<OrderBankDetail>();
                using (PEPSIEntities dc = new PEPSIEntities())
                {
                    var V = (from x in dc.Customers where x.CustomerID == inCustID select x).FirstOrDefault();
                    var W = (from c in dc.Warehouses where c.WarehouseID == inCustID select c).FirstOrDefault();
                    if (inWC == "C")
                    {
                        CustName = V.CustomerName;
                    }
                    else if (inWC == "N")
                    {
                        CustName = W.WarehouseDescription;
                    }
                }
                return CustName;


            }
            catch (Exception)
            {
                return CustName;
                //throw ex;
            }
        }
        public decimal GetSecurityAmount(int inProductID, decimal inCsQty, int inPlastic)
        {
            decimal SAmount = 0;

            try
            {
                {
                    var V = (from x in db.Products where x.ProductID == inProductID && x.BottleReturnable == 1 select x).FirstOrDefault();
                    SAmount = ((V.AlternateSecurityDeposit * inCsQty) + (V.PlasticBoxSecurity * inPlastic));
                }
                return SAmount;


            }
            catch (Exception)
            {
                return SAmount;
            }
        }
        public bool chkDuplicateChallan(int orderNo)
        {
            bool t = false;
            int f = db.CashSettlements.Where(x => (x.OrderID == orderNo)).Count();

            if (f == 0)
            {
                t = true;
            }
            return t;
        }

    }
}