using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro2016.Controllers
{
    public class GroupsController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            var model = new Euro2016.Models.GroupViewModel();


            using (var dbContext = new Euro2016Entities())
            {
                model.Groups = dbContext.Groups.ToList();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new Euro2016.Models.GroupViewModel();
            using (var dbContext = new Euro2016Entities())
            {
                var group = dbContext.Groups.Where(x => x.Id == id).FirstOrDefault();
                model.Name = group.Name;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Euro2016.Models.GroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var group = new Group();
            group.Name = model.Name;
            group.CreationDate = DateTime.Now;
            group.CreationSource = "Dan";

            using (var dbContext = new Euro2016Entities())
            {
                dbContext.Groups.Add(group);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Edit(Euro2016.Models.GroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
      
            using (var dbContext = new Euro2016Entities())
            {
                var group = dbContext.Groups.Where(x => x.Id == model.Id).FirstOrDefault();
                group.MaintenanceDate = DateTime.Now;
                group.MaintenanceSource = "Dan";
                group.Name = model.Name;

                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");

            
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var dbContext = new Euro2016Entities())
            {
                var group = dbContext.Groups.Where(x => x.Id == id).FirstOrDefault();

                if(group != null)
                {
                    if(group.Teams.Any())
                    {
                        TempData.Add("Message", "Cannot delete group as it contains teams, please delete the teams first.");
                        return RedirectToAction("Index");
                    }

                    dbContext.Groups.Remove(group);

                    dbContext.SaveChanges();
                }


            }
            return RedirectToAction("Index");
        }
    }
}