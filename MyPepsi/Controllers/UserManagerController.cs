using MyPepsi.Models;
using MyPepsi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MyPepsi.Controllers
{
    [Authorize]

    public class UserManagerController : Controller
    {
       
        private PEPSIEntities db = new PEPSIEntities();
        // GET: UserManger
        public ActionResult Index()
        {
            int uid;
            uid = (from x in db.UserLogins select x).Max(p => p.UserID);
            ViewBag.UserID = uid + 1;
            ViewBag.WorkStationID = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            ViewBag.RoleId = new SelectList(db.MenuRoles, "RoleID", "Roles");

            return View();
        }
        [HttpPost]
        public JsonResult CreateUser(UserCreateVM A)
        {
            bool status = false;
            string mes = "";
            int v = A.UserID;
            if (ModelState.IsValid)
            {
                try
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {
                        UserLogin ulog = new UserLogin();
        
                        {

                            ulog.UserID = A.UserID;
                            ulog.UserName = A.UserName;
                            ulog.Password = A.Password;
                            ulog.MobileNo = A.MobileNo;
                            ulog.WorkStationID = A.WorkStationID;
                            ulog.RoleId = A.RoleId;
                            ulog.MachineIP = A.MachineIP;
                            ulog.Email = A.Email;
                            ulog.Status = "A";
                            ulog.CreateBy = User.Identity.Name;
                            ulog.CreateDate = System.DateTime.Now;
                        }

                            dc.UserLogins.Add(ulog);
                            dc.SaveChanges();
                            status = true;
                            dc.Dispose();
           
                        
                    }
                }
                catch (Exception ex)
                {
                   
                    return new JsonResult { Data = new { status = false, mes = mes, v = v } };
                    //throw ex;
                }

            }
            else
            {
                status = false;
            }
          
            return new JsonResult { Data = new { status = status, mes = mes, v = v } };
        }

        // Get : User Rigths
        public ActionResult MenuRights()
        {
            //int uid;
            // var uid = (from x in db.UserLogins select x).FirstOrDefault();
            ViewBag.UserID = new SelectList(db.UserLogins, "UserID", "UserName");
            //ViewBag.WorkStationID = new SelectList(db.Warehouses, "WarehouseID", "WarehouseDescription");
            //ViewBag.RoleId = new SelectList(db.MenuRoles, "RoleID", "Roles");
            // ViewBag.TypeCode = new SelectList(db.MenuRoles, "RoleID", "Roles");
            var tCode = new SelectList(
          new[]
          {
                new { ID = 2, Name = "Reports" },
                new { ID = 1, Name = "Forms" },
               },
          "ID",
          "Name"
      );
            ViewBag.TypeCode = tCode;

            return View("MenuRights");

        }
        [HttpPost]
        public JsonResult Rights(MenuSubVM A)
        {
            bool status = false;
            string mes = "";
            int v = (int)A.UserID;
            if (ModelState.IsValid)
            {
                try
                {
                    using (PEPSIEntities dc = new PEPSIEntities())
                    {
                        foreach (var i in A.menudetail)
                        {
                           
                            if (i.IsSelected == 1)
                            {
                                MenuSub ulog = new MenuSub
                                {
                                    UserID = (int)A.UserID,
                                    SubMenu = i.SubMenu,
                                    RoleId = A.RoleId,
                                    Controller = i.Controller,
                                    Action = i.Action,
                                    MainMenuId = i.MainMenuId,
                                    WarehouseID=A.WarehouseID,
                                    MenuCode =i.MenuCode,
                                    Status = "A",
                                    CreateBy = User.Identity.Name,
                                    CreateDate = System.DateTime.Now

                                };

                                dc.MenuSubs.Add(ulog);
                            }
                        }
                        
                        dc.SaveChanges();
                        status = true;
                        dc.Dispose();
                    }
                }
                catch (Exception ex)
                {

                    return new JsonResult { Data = new { status = false, mes = mes, v = v } };
                    //throw ex;
                }

            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = true, mes = mes, v = v } };
        }
        [HttpGet]
        public JsonResult GetMenuList(int inType)
        {

            try
            {

                var MenuData = db.MenuLists.Where(x => x.TypeCode == inType);
                return new JsonResult { Data = MenuData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Not Assign" });
                throw ex;
            }
        }
        [HttpGet]
        public JsonResult GetUserInfo(int pUserID)
        {

            try
            {

                var userinfo = db.spUserInformation(pUserID);
                return new JsonResult { Data = userinfo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "No data found for the user..!!" });
                throw ex;
            }
        }
        public JsonResult SendEmail(string receiver, string subject, string message)
        {
            bool status = true;
            string mes = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("farid.ahmed@tbl.transcombd.com", "Farid");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    //var Userid = "Your User ID  here";
                    var password = "tbl@Farid";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "mail.transcombd.com",
                        Port = 25,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    // return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                // ViewBag.Error = "Some Error";
            }
            //return View();
            return new JsonResult { Data = new { status = status, mes = mes } };
        }
        public JsonResult SendSMStoDB(string phoneno, string sms)
        {
            bool status = false;
            string mes = "";

            HttpWebRequest request;
            HttpWebResponse response = null;
            String url;
            String host;

            if (ModelState.IsValid)
            {
                try
                {
                    //  var customerphone = db.Customers.Where(x=>x.CustomerID == )
                    status = true;
                    host = "https://vas.banglalinkgsm.com/sendSMS/sendSMS?msisdn=" + phoneno + "&message=" + sms + "&userID=" + "tranbevltd" + "&passwd=" + "92b034a909c050209caac7d5ccc0bf64" + "&sender=" + "8801969900240" + "";
                    url = host;
                    request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "sms Not Send" });
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };
        }
    }
}




