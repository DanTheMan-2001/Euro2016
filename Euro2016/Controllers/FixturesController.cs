using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Euro2016.Controllers
{
    public class FixturesController : Controller
    {
        // GET: Fixtures
        public ActionResult Index()
        {
            var model = new Euro2016.Models.FixturesViewModel(); //this is creating a new instance of an object

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Fixtures = dbContext.Fixtures
                    .Include("HomeTeam")
                    .Include("AwayTeam")
                    .Include("Venue")
                    .ToList();
            }
            return View(model);
        }

        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Add()
        {
            var model = new Euro2016.Models.FixturesViewModel(); //this is creating a new instance of an object //this is creating a new instance of an object //this is creating a new instance of an object

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Teams = dbContext.Teams.OrderBy(x => x.Name).ToList();
                model.Venues = dbContext.Venues.ToList();
            }
            return View(model);
        }


        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Edit(int id)
        {
            var model = new Euro2016.Models.FixturesViewModel(); //this is creating a new instance of an object
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var fixture = dbContext.Fixtures.Where(x => x.Id == id).FirstOrDefault();
                model.HomeTeamId = fixture.HomeTeamId;
                model.AwayTeamId = fixture.AwayTeamId;
                model.VenueId = fixture.VenueId;
                model.FixtureDate = fixture.Date;
                model.Teams = dbContext.Teams.OrderBy(x => x.Name).ToList();
                model.Venues = dbContext.Venues.ToList();
            }
            return View(model);
        }
        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Add(Euro2016.Models.FixturesViewModel model)
        {
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Teams = dbContext.Teams.OrderBy(x => x.Name).ToList();
                model.Venues = dbContext.Venues.ToList();
            }

            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {
                return View(model);
            }

            if (model.HomeTeamId == model.AwayTeamId)
            {
                TempData.Add("Message", "Please select two different teams.");
                return View(model);
            }

            var fixture = new Fixture(); //this is creating a new instance of an object
            fixture.HomeTeamId = model.HomeTeamId;
            fixture.AwayTeamId = model.AwayTeamId;
            fixture.VenueId = model.VenueId;
            fixture.Date = model.FixtureDate.GetValueOrDefault();
            fixture.CreationDate = DateTime.Now;
            fixture.CreationSource = "Dan";


            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                dbContext.Fixtures.Add(fixture);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Edit(Euro2016.Models.FixturesViewModel model)
        {
            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {
                return View(model);
            }

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var fixture = dbContext.Fixtures.Where(x => x.Id == model.Id).FirstOrDefault();
                fixture.MaintenanceDate = DateTime.Now;
                fixture.MaintenanceSource = "Dan";
                fixture.HomeTeamId = model.HomeTeamId;
                fixture.AwayTeamId = model.AwayTeamId;
                fixture.VenueId = model.VenueId;
                fixture.Date = model.FixtureDate.GetValueOrDefault();

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
                var fixture = dbContext.Fixtures.Where(x => x.Id == id).FirstOrDefault();

                if (fixture != null)
                {
                    if (fixture.Results.Any())
                    {
                        TempData.Add("Message", "Cannot delete fixture as it contains results.");
                        return RedirectToAction("Index");
                    }

                    dbContext.Fixtures.Remove(fixture);

                    dbContext.SaveChanges();
                }


            }
            return RedirectToAction("Index");
        }
    }
}