using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPepsi.Models;


namespace MyPepsi.Controllers
{
    [Authorize]
    public class LegacyPageRouteController : Controller
    {
        public ActionResult Index()
        {
            ReportViewerViewModel model = new ReportViewerViewModel();
            string content = Url.Content("~/Reports/CrystalViewer/LegacyReportViewer.aspx");
            model.ReportPath = content;
            return View("ReportViewer", model);
        }
        public ActionResult Index500()
        {
            ReportViewerViewModel model = new ReportViewerViewModel();
            string content = Url.Content("~/Reports/CrystalViewer/LegacyReportViewer500.aspx");
            model.ReportPath = content;
            return View("ReportViewer500", model);
        }
    }

    public class ReportViewerViewModel
    {
        public string ReportPath { get; set; }
    }
}