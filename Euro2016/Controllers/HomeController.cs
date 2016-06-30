using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro2016.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var model = new Euro2016.Models.HomeViewModel();
            using (var dbContext = new Euro2016Entities())
            {
                model.Groups = dbContext.Groups
                    .Include("Teams")
                    .ToList();
                    
            }

                return View(model);
        }
    }
}