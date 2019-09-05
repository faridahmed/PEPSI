using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyPepsi.Models;
using System.Web.Mvc;

namespace MyPepsi.Reports.CrystalViewer
{
    [Authorize]
    public partial class LegacyReportViewer500 : System.Web.UI.Page
    {
        //public static ReportDocument reportDocument;
        private PEPSIEntities db = new PEPSIEntities();
        ReportDocument reportDocument = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            DateTime date = DateTime.UtcNow;
            BeginingDate.Text = date.ToString();
            EndDate.Text = date.ToString();
            LoadList();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
               LoadReport();
            }
            if (!IsPostBack)
            {

            }
        }


        //ReportDocument CrystalRpt;
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            if (this.crViewer != null)
            { 
                this.crViewer.Dispose();
            this.crViewer = null;
            //reportDocument.Close();
            //reportDocument.Dispose();
            GC.Collect();
        }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {

            LoadReport();
        }
        private void LoadReport()
        {
            DateTime frst = Convert.ToDateTime(BeginingDate.Text); // DateTime.UtcNow.AddDays(-10);
            DateTime snd = Convert.ToDateTime(EndDate.Text);// DateTime.UtcNow.AddDays(0);

            int wareID =Convert.ToInt32( WarehouseList.SelectedItem.Value);
            int custID = Convert.ToInt32(CustomerList.SelectedItem.Value);
 
            reportDocument = new ReportDocument();
            //Report path
             string reportPath = Server.MapPath("~/Reports/Crystal/CustomerLedgerDetail500.rpt");  
            //string reportPath = Server.MapPath("~/Reports/Crystal/CrystalReport1.rpt");

            reportDocument.Load(reportPath);
            reportDocument.SetDatabaseLogon("sa", "tbl@pss", "10.168.90.69", "PEPSI", true);

            reportDocument.SetParameterValue("BeginingDate", frst.Date);
            reportDocument.SetParameterValue("EndingDate", snd.Date);
            reportDocument.SetParameterValue("WarehouseID", wareID);
            reportDocument.SetParameterValue("CustomerID", custID);
            reportDocument.SetParameterValue("CustomerType", 10);

            reportDocument.SetParameterValue("BeginingDate", frst.Date, reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("EndingDate", snd.Date, reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("WarehouseID", wareID, reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("CustomerID", custID, reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("CustomerType", 10, reportDocument.Subreports[0].Name.ToString());

            reportDocument.SetParameterValue("BeginingDate", frst.Date, reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("EndingDate", snd.Date, reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("WarehouseID", wareID, reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("CustomerID", custID, reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("CustomerType", 10, reportDocument.Subreports[1].Name.ToString());

            crViewer.ReportSource = reportDocument;
            crViewer.DataBind();

            //reportDocument.Close();
            //reportDocument.Dispose();


        }

        private void LoadList()
        {
            var w = (from yt in db.UserLogins
                     where yt.UserID.ToString() == User.Identity.Name
                     select new { yt.WorkStationID }).FirstOrDefault();
            var wn = db.Warehouses.Where(t => t.WarehouseID == w.WorkStationID).FirstOrDefault();

            WarehouseList.Items.Clear();
            CustomerList.Items.Clear();

            WarehouseList.DataSource = db.Warehouses.ToList();
            WarehouseList.DataTextField = "WarehouseDescription";
            WarehouseList.DataValueField = "WarehouseID";
            WarehouseList.DataBind();

            //CustomerList.DataSource = db.Customers.OrderBy(x => x.CustomerName).ToList();


            int wID = wn.WarehouseID;//Convert.ToInt32(WarehouseList.SelectedItem.Value);
            CustomerList.DataSource = db.Customers.Where(a => a.WarehouseID == wID).OrderBy(x=>x.CustomerName).ToList();CustomerList.DataTextField = "CustomerName";
            CustomerList.DataValueField = "CustomerID";
            CustomerList.DataBind();
        }

        //protected void WarehouseList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //int wID = Convert.ToInt32(WarehouseList.SelectedItem.Value);
        //    //CustomerList.DataSource = db.Customers.Where(a => a.WarehouseID == wID).OrderBy(x=>x.CustomerName).ToList();

        //    CustomerList.DataSource = db.Customers.OrderBy(x => x.CustomerName).ToList();
        //    CustomerList.DataTextField = "CustomerName";
        //    CustomerList.DataValueField = "CustomerID";
        //    CustomerList.DataBind();
        //}
    }
}