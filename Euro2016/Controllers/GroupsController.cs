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
            var model = new Euro2016.Models.GroupViewModel(); //this is creating a new instance of an object


            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Groups = dbContext.Groups.ToList();
            }
            return View(model);
        }




        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Add()
        {
            return View();
        }




        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Edit(int id)
        {
            var model = new Euro2016.Models.GroupViewModel(); //this is creating a new instance of an object
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var group = dbContext.Groups.Where(x => x.Id == id).FirstOrDefault();
                model.Name = group.Name;
            }

            return View(model);
        }

        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Add(Euro2016.Models.GroupViewModel model)
        {
            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {
                return View(model);
            }
            var group = new Group(); //this is creating a new instance of an object
            group.Name = model.Name;
            group.CreationDate = DateTime.Now;
            group.CreationSource = "Dan";

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                dbContext.Groups.Add(group);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Edit(Euro2016.Models.GroupViewModel model)
        {
            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {
                return View(model);
            }
      
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var group = dbContext.Groups.Where(x => x.Id == model.Id).FirstOrDefault();
                group.MaintenanceDate = DateTime.Now;
                group.MaintenanceSource = "Dan";
                group.Name = model.Name;

                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");

            
        }

        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Delete(int id)
        {
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
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