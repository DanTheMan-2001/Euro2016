using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Euro2016.Controllers
{
    public class VenuesController : Controller
    {
        // GET: Venues
        public ActionResult Index()
        {
            var model = new Euro2016.Models.VenuesViewModel(); //this is creating a new instance of an object


            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Venues = dbContext.Venues.ToList();
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
            var model = new Euro2016.Models.VenuesViewModel(); //this is creating a new instance of an object
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var venue = dbContext.Venues.Where(x => x.Id == id).FirstOrDefault();
                model.Name = venue.Name;
            }

                return View(model);
        }

        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Add(Euro2016.Models.VenuesViewModel model)
        {
            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {
                return View(model);
            }
            var venue = new Venue(); //this is creating a new instance of an object
            venue.Name = model.Name;
            venue.CreationDate = DateTime.Now;
            venue.CreationSource = "Dan";

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                dbContext.Venues.Add(venue);
                dbContext.SaveChanges();
            }
                return RedirectToAction("Index");
        }

        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Edit(Euro2016.Models.VenuesViewModel model)
        {
            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {
                return View(model);
            }
          

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var venue = dbContext.Venues.Where(x => x.Id == model.Id).FirstOrDefault();
                venue.Name = model.Name;
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
                var venue = dbContext.Venues.Where(x => x.Id == id).FirstOrDefault();

                if (venue != null)
                {
                    if (venue.Fixtures.Any())
                    {
                        TempData.Add("Message", "Cannot delete venue as it has fixtures, please delete the fixtures first.");
                        return RedirectToAction("Index");
                    }

                    dbContext.Venues.Remove(venue);

                    dbContext.SaveChanges();
                }


            }
            return RedirectToAction("Index");
        }

        [HttpGet] //this requests data from a specified resource
        public ActionResult Fixtures(int id)
        {

            var model = new Euro2016.Models.VenueFixturesViewModel(); //this is creating a new instance of an object
            model.Fixtures = new List<Fixture>();

            //This is getting the requested venue from the database and including the venue fixtures
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var venue = dbContext.Venues
                    .Include(x => x.Fixtures.Select(y => y.HomeTeam))
                    .Include(x => x.Fixtures.Select(y => y.AwayTeam))                    
                    
                    .Where(x => x.Id == id).FirstOrDefault();

                model.Fixtures.AddRange(venue.Fixtures.ToList());

                model.Venue = venue;

            }


            return View(model);
        }
    }
}