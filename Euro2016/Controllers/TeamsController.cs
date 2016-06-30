using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Euro2016.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Teams
        public ActionResult Index()
        {
            var model = new Euro2016.Models.TeamViewModel();


            using (var dbContext = new Euro2016Entities())
            {
                model.Teams = dbContext.Teams
                    .Include("Group")
                    .ToList();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var model = new Euro2016.Models.TeamViewModel();

            using (var dbContext = new Euro2016Entities())
            {
                model.Groups = dbContext.Groups.ToList();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new Euro2016.Models.TeamViewModel();
            using (var dbContext = new Euro2016Entities())
            {
                var team = dbContext.Teams.Where(x => x.Id == id).FirstOrDefault();
                model.Name = team.Name;
                model.Flag = team.Flag;
                model.GroupId = team.GroupId;
                model.Groups = dbContext.Groups.ToList();
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Add(Euro2016.Models.TeamViewModel model)
        {
            using (var dbContext = new Euro2016Entities())
            {
                model.Groups = dbContext.Groups.ToList();
            }

            if (!ModelState.IsValid)
            {         
                return View(model);
            }

            using (var dbContext = new Euro2016Entities())
            {
                var group = dbContext.Groups.Where(x => x.Id == model.GroupId).FirstOrDefault();
                var teamCount = group.Teams.Count();
                if (teamCount >= 4)
                {
                    TempData.Add("Message", string.Format("{0} already has 4 teams, please choose another group.",group.Name));
                    return View(model);
                }
            }

                var team = new Team();
            team.Name = model.Name;
            team.Flag = model.Flag;
            team.GroupId = model.GroupId;
            team.CreationDate = DateTime.Now;
            team.CreationSource = "Dan";

            using (var dbContext = new Euro2016Entities())
            {
                dbContext.Teams.Add(team);
                dbContext.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Euro2016.Models.TeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var dbContext = new Euro2016Entities())
            {
                var team = dbContext.Teams.Where(x => x.Id == model.Id).FirstOrDefault();
                team.MaintenanceDate = DateTime.Now;
                team.MaintenanceSource = "Dan";
                team.Name = model.Name;
                team.GroupId = model.GroupId;
                team.Flag = model.Flag;

                dbContext.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var dbContext = new Euro2016Entities())
            {
                var team = dbContext.Teams.Where(x => x.Id == id).FirstOrDefault();

                if (team != null)
                {
                    if (team.HomeFixtures.Any() || team.AwayFixtures.Any())
                    {
                        TempData.Add("Message", "Cannot delete team as it has fixtures, please delete the fixtures first.");
                        return RedirectToAction("Index");
                    }

                    dbContext.Teams.Remove(team);

                    dbContext.SaveChanges();
                }


            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Fixtures(int id)
        {

            var model = new Euro2016.Models.TeamFixturesViewModel();
            model.Fixtures = new List<Fixture>();

            using (var dbContext = new Euro2016Entities())
            {
                var team = dbContext.Teams
                    .Include(x=>x.HomeFixtures.Select(y=>y.HomeTeam))
                    .Include(x => x.HomeFixtures.Select(y => y.AwayTeam))
                    .Include(x => x.HomeFixtures.Select(y => y.Venue))
                    .Include(x => x.AwayFixtures.Select(y => y.HomeTeam))
                    .Include(x => x.AwayFixtures.Select(y => y.AwayTeam))
                    .Include(x => x.AwayFixtures.Select(y => y.Venue))

                    .Where(x => x.Id == id).FirstOrDefault();

                model.Fixtures.AddRange(team.HomeFixtures.ToList());
                model.Fixtures.AddRange(team.AwayFixtures.ToList());

                model.Team = team;
                
            }



                return View(model);
        }
    }
}