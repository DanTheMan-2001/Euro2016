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
        public ActionResult Tables()
        {
            var model = new Euro2016.Models.HomeViewModel(); //this is creating a new instance of an object
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Groups = dbContext.Groups
                    .Include("Teams")
                    .ToList();
                    
            }

                return View(model);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}