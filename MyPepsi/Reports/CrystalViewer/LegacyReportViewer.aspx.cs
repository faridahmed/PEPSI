using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyPepsi.Models;

namespace MyPepsi.Reports.CrystalViewer
{
    public partial class LegacyReportViewer : System.Web.UI.Page
    {
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
        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadReport();
        }
        private void LoadReport()
        {
            DateTime frst = Convert.ToDateTime(BeginingDate.Text); // DateTime.UtcNow.AddDays(-10);
            DateTime snd = Convert.ToDateTime(EndDate.Text);// DateTime.UtcNow.AddDays(0);

            int wareID =Convert.ToInt32( WarehouseList.SelectedItem.Value);
            //if (!int.TryParse(WarehouseList.SelectedItem, out int wareID))
            //{
            //    var wareID = 0;
            //}

            reportDocument = new ReportDocument();
            //Report path
             string reportPath = Server.MapPath("~/Reports/Crystal/SalesSummary76.rpt");  
            //string reportPath = Server.MapPath("~/Reports/Crystal/CrystalReport1.rpt");

            reportDocument.Load(reportPath);
            reportDocument.SetDatabaseLogon("sa", "tbl@pss", "10.168.90.69", "PEPSI", true);

            reportDocument.SetParameterValue("BeginingDate", frst.Date);
            reportDocument.SetParameterValue("EndingDate", snd.Date);
            reportDocument.SetParameterValue("WarehouseID", wareID);
            reportDocument.SetParameterValue("CustomerID", 0);
            reportDocument.SetParameterValue("CustomerType", 10);

            reportDocument.SetParameterValue("BeginingDate", frst.Date,reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("EndingDate", snd.Date, reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("WarehouseID", wareID, reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("CustomerID", 0, reportDocument.Subreports[0].Name.ToString());
            reportDocument.SetParameterValue("CustomerType", 10, reportDocument.Subreports[0].Name.ToString());

            reportDocument.SetParameterValue("BeginingDate", frst.Date, reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("EndingDate", snd.Date,reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("WarehouseID", wareID, reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("CustomerID", 0, reportDocument.Subreports[1].Name.ToString());
            reportDocument.SetParameterValue("CustomerType", 10, reportDocument.Subreports[1].Name.ToString());

            reportDocument.SetParameterValue("BeginingDate", frst.Date, reportDocument.Subreports[2].Name.ToString());
            reportDocument.SetParameterValue("EndingDate", snd.Date, reportDocument.Subreports[2].Name.ToString());
            reportDocument.SetParameterValue("WarehouseID", wareID, reportDocument.Subreports[2].Name.ToString());
            reportDocument.SetParameterValue("CustomerID", 0, reportDocument.Subreports[2].Name.ToString());
            reportDocument.SetParameterValue("CustomerType", 10, reportDocument.Subreports[2].Name.ToString());

           

            crViewer.ReportSource = reportDocument;
            crViewer.DataBind();

            //reportDocument.Close();
            //reportDocument.Dispose();
        }

        private void LoadList()
        {
            WarehouseList.Items.Clear();
            CustomerList.Items.Clear();

            WarehouseList.DataSource = db.Warehouses.ToList();
            WarehouseList.DataTextField = "WarehouseDescription";
            WarehouseList.DataValueField = "WarehouseID";
            WarehouseList.DataBind();

           CustomerList.DataSource = db.Customers.ToList();
            CustomerList.DataTextField = "CustomerName";
            CustomerList.DataValueField = "CustomerID";
            CustomerList.DataBind();
        }
    }
}