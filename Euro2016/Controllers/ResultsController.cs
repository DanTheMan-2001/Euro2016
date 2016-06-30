using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro2016.Controllers
{
    public class ResultsController : Controller
    {
        // GET: Results
        public ActionResult Index()
        {
            var model = new Euro2016.Models.ResultsViewModel();


            using (var dbContext = new Euro2016Entities())
            {
                model.Results = dbContext.Results.ToList();
            }
            return View(model);
        }

        public ActionResult GenerateResults()
        {
            return RedirectToAction("Index");
        }

   
    }
}