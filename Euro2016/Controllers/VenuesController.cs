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
            var model = new Euro2016.Models.VenuesViewModel();


            using (var dbContext = new Euro2016Entities())
            {
                model.Venues = dbContext.Venues.ToList();
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
            var model = new Euro2016.Models.VenuesViewModel();
            using (var dbContext = new Euro2016Entities())
            {
                var venue = dbContext.Venues.Where(x => x.Id == id).FirstOrDefault();
                model.Name = venue.Name;
            }

                return View(model);
        }

        [HttpPost]
        public ActionResult Add(Euro2016.Models.VenuesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var venue = new Venue();
            venue.Name = model.Name;
            venue.CreationDate = DateTime.Now;
            venue.CreationSource = "Dan";

            using (var dbContext = new Euro2016Entities())
            {
                dbContext.Venues.Add(venue);
                dbContext.SaveChanges();
            }
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Euro2016.Models.VenuesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
          

            using (var dbContext = new Euro2016Entities())
            {
                var venue = dbContext.Venues.Where(x => x.Id == model.Id).FirstOrDefault();
                venue.Name = model.Name;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var dbContext = new Euro2016Entities())
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

        [HttpGet]
        public ActionResult Fixtures(int id)
        {

            var model = new Euro2016.Models.VenueFixturesViewModel();
            model.Fixtures = new List<Fixture>();

            //This is getting the requested venue from the database and including the venue fixtures
            using (var dbContext = new Euro2016Entities())
            {
                var venue = dbContext.Venues
                    .Include(x => x.Fixtures.Select(y => y.HomeTeam))
                    .Include(x => x.Fixtures.Select(y => y.AwayTeam))                    
                    
                    .Where(x => x.Id == id).FirstOrDefault();

                model.Fixtures.AddRange(venue.Fixtures.ToList());

                model.Venue = venue;

            }


            //This 
            return View(model);
        }
    }
}