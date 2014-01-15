using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ClientAsp.Models;

namespace ClientAsp.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {
        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            ViewData["LogOnError"] = null;
            if (ModelState.IsValid)
            {
                ServAcc.Account acc = new ServAcc.Account();
                ServAcc.ResultatOfstringAccountDataGmrGjgq6 result = acc.Login(model.Email, model.Password);
                if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    HttpCookie auth = FormsAuthentication.GetAuthCookie(model.Email, true);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(auth.Value);
                    FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, result._val1);
                    auth.Value = FormsAuthentication.Encrypt(newticket);
                    Response.Cookies.Add(auth);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewData["LogOnError"] = result._error.ToString();
            }

            // Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("INDEX", "HOME");
            ViewData["RegisterError"] = null;
            if (ModelState.IsValid)
            {
                // Tentative d'inscription de l'utilisateur
                ServAcc.Account acc = new ServAcc.Account();
                ServAcc.Resultat result = acc.Register(model.Email, model.UserName, model.Password);
                if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
                {
                    ViewData["LogOnError"] = null;
                    ServAcc.ResultatOfstringAccountDataGmrGjgq6 login = acc.Login(model.Email, model.Password);
                    if (login._error == ServAcc.ResultatErrorCode.SUCCESS)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        HttpCookie auth = FormsAuthentication.GetAuthCookie(model.Email, true);
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(auth.Value);
                        FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, login._val1);
                        auth.Value = FormsAuthentication.Encrypt(newticket);
                        Response.Cookies.Add(auth);
                        return RedirectToAction("Index", "Home");
                    }
                    ViewData["LogOnError"] = login._error.ToString();
                    return RedirectToAction("LOGON", "Account");
                }
                else
                    ViewData["RegisterError"] = result._error.ToString();
            }
            // Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************
        public ActionResult Delete()
        {
            return Delete(0);
        }

        [HttpPost]
        public ActionResult Delete(int a)
        {
            ServAcc.Account acc = new ServAcc.Account();
            FormsIdentity id = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = id.Ticket;
            ServAcc.Resultat result = acc.DeleteAccount(ticket.UserData);
            ViewData["DeleteError"] = null;
            if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
                FormsAuthentication.SignOut();
            ViewData["DeleteError"] = result._error.ToString();
            return RedirectToAction("Index", "Home");
        }
       
        [Authorize]
        public ActionResult ChangePassword()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                ServAcc.Account acc = new ServAcc.Account();
                FormsIdentity id = User.Identity as FormsIdentity;
                FormsAuthenticationTicket ticket = id.Ticket;

                ServAcc.Resultat result = acc.UpdatePassword(ticket.UserData, model.NewPassword);
                if (result._error == ServAcc.ResultatErrorCode.SUCCESS)
                {
                    return RedirectToAction("ChangePasswordSuccess", "Account");
                }
                else
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
