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
            var model = new Euro2016.Models.FixturesViewModel();

            using (var dbContext = new Euro2016Entities())
            {
                model.Fixtures = dbContext.Fixtures
                    .Include("HomeTeam")
                    .Include("AwayTeam")
                    .Include("Venue")
                    .ToList();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var model = new Euro2016.Models.FixturesViewModel();

     using (var dbContext = new Euro2016Entities())
            {
                model.Teams = dbContext.Teams.OrderBy(x => x.Name).ToList();
                model.Venues = dbContext.Venues.ToList();
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new Euro2016.Models.FixturesViewModel();
            using (var dbContext = new Euro2016Entities())
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
        [HttpPost]
        public ActionResult Add(Euro2016.Models.FixturesViewModel model)
        {
            using (var dbContext = new Euro2016Entities())
            {
                model.Teams = dbContext.Teams.OrderBy(x => x.Name).ToList();
                model.Venues = dbContext.Venues.ToList();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.HomeTeamId == model.AwayTeamId)
            {
                TempData.Add("Message", "Please select two different teams.");
                return View(model);
            }

            var fixture = new Fixture();
            fixture.HomeTeamId = model.HomeTeamId;
            fixture.AwayTeamId = model.AwayTeamId;
            fixture.VenueId = model.VenueId;
            fixture.Date = model.FixtureDate.GetValueOrDefault();
            fixture.CreationDate = DateTime.Now;
            fixture.CreationSource = "Dan";


            using (var dbContext = new Euro2016Entities())
            {
                dbContext.Fixtures.Add(fixture);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Euro2016.Models.FixturesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var dbContext = new Euro2016Entities())
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

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var dbContext = new Euro2016Entities())
            {
                var fixture = dbContext.Fixtures.Where(x => x.Id == id).FirstOrDefault();

                if (fixture != null)
                {
                    dbContext.Fixtures.Remove(fixture);

                    dbContext.SaveChanges();
                }


            }
            return RedirectToAction("Index");
        }
    }
}