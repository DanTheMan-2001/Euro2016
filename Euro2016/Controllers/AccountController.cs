using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Euro2016.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            var model = new Euro2016.Models.LoginViewModel();
            model.ReturnUrl = Request["ReturnUrl"];
            return View(model);
        }


        [HttpPost]
        public ActionResult Login(Euro2016.Models.LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Add("Message", "Please enter username and password.");
                return View(model);
            }

            if (model.Username == "DanTheMan" && model.Password == "letmein")
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return new RedirectResult(model.ReturnUrl);
                
            }

            TempData.Add("Message", "Invalid username and password.");
            return View(model);
        }
    }
}