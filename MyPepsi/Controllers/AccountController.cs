using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyPepsi.Models;
using MyPepsi.ViewModel;
using System.Collections.Generic;
using System.Web.Security;
using System.Net;

namespace MyPepsi.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private PEPSIEntities databaseManager = new PEPSIEntities();
        // GET: /Account/Login    

        [AllowAnonymous]
        public ActionResult UserLogin(string returnUrl)
        {
            try
            {
                // Verification.    
                if (this.Request.IsAuthenticated)
                {
                    // Info.    
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            // Info.    
            return this.View();
        }

        // POST: /Account/Login    
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(LoginViewModel model, string returnUrl)
        {
            try
            {
                // Verification.    
                if (ModelState.IsValid)
                {
                    // Initialization.   

                    var loginInfo = this.databaseManager.spLoginUser(model.UserID, model.Password).ToList();
                    // var loginInfo = this.databaseManager.LoginUserPassword(model.UserCode, model.Password).ToList();
                    // Verification.
                        
                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        // Initialization.    
                        var logindetails = loginInfo.First();
                        // Login In.
                        // var sid = logindetails.UserName;
                        var sid = logindetails.userId;


                        this.SignInUser(sid.ToString(), false);
                      // Info.    
                      // return this.RedirectToLocal(returnUrl);

                        // for menu
                        bool isExist = false;
                        using (PEPSIEntities dc = new PEPSIEntities())
                        {
                            isExist = dc.UserLogins.Where(x => x.UserID == model.UserID).Any();
                            if (isExist)
                            {
                                LoginModels loginCredentials = dc.UserLogins.Where(x => x.UserID == model.UserID).Select(x => new LoginModels
                                {

                                    UserName = x.UserName,
                                    UserRoleId = x.RoleId,
                                    UserID = x.UserID
                                }).FirstOrDefault();
                                List<MenuModels> menus = dc.MenuSubs.Where(x => x.RoleId == loginCredentials.UserRoleId && x.UserID==loginCredentials.UserID).Select(x => new MenuModels
                                {
                                    MainMenuId = x.MenuMain.Id,
                                    MainMenuName = x.MenuMain.MainMenu,
                                    SubMenuId = x.MenuCode,
                                    SubMenuName = x.SubMenu,
                                    ControllerName = x.Controller,
                                    ActionName = x.Action,
                                    RoleId = x.RoleId,
                                    UserID =x.UserID,
                                    RoleName = x.MenuRole.Roles
                                }).ToList();
                                FormsAuthentication.SetAuthCookie(loginCredentials.UserID.ToString(), false);
                                Session["LoginCredentials"] = loginCredentials;
                                Session["MenuMaster"] = menus;
                                Session["Name"] = loginCredentials.UserID;
                                return this.RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Please enter the valid credentials!...";
                                return View("Index");
                            }
                        }


                    }
                    else
                    {
                        // Setting.    
                        ModelState.AddModelError(string.Empty, "Invalid UserName or Password..Please check");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            return this.View(model);
        }

        /// POST: /Account/LogOff 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                // Setting.    
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign Out.    
                //authenticationManager.SignOut();
                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            // return this.RedirectToAction("Login", "Account");
            return RedirectToAction("UserLogin", "Account");

        }

        private void SignInUser(string username, bool isPersistent)
        {
            // Initialization.    
            var claims = new List<Claim>();
            
            try
            {
                // Setting                   
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.    
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.    
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.    
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public JsonResult getUserName(int userid)
        {
            try
            {

                string uName = " ";
                //decimal price = 0;
                var userlog = (from x in databaseManager.UserLogins where x.UserID == userid select x).FirstOrDefault();
                uName = userlog.UserName;
                //price = prod.AlternateUnitPrice;

                // return new JsonResult { Data = orderValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                return new JsonResult { Data =  uName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Not Found" });

            }
        }
       
    }
}