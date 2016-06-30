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
            var model = new Euro2016.Models.TeamViewModel(); //this is creating a new instance of an object


            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Teams = dbContext.Teams
                    .Include("Group")
                    .ToList();
            }
            return View(model);
        }

        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Add()
        {
            var model = new Euro2016.Models.TeamViewModel(); //this is creating a new instance of an object

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Groups = dbContext.Groups.ToList();
            }

            return View(model);
        }

        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Edit(int id)
        {
            var model = new Euro2016.Models.TeamViewModel(); //this is creating a new instance of an object
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var team = dbContext.Teams.Where(x => x.Id == id).FirstOrDefault();
                model.Name = team.Name;
                model.Flag = team.Flag;
                model.GroupId = team.GroupId;
                model.Groups = dbContext.Groups.ToList();
            }
            return View(model);
        }


        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Add(Euro2016.Models.TeamViewModel model)
        {
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Groups = dbContext.Groups.ToList();
            }

            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {         
                return View(model);
            }

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                var group = dbContext.Groups.Where(x => x.Id == model.GroupId).FirstOrDefault();
                var teamCount = group.Teams.Count();
                if (teamCount >= 4)
                {
                    TempData.Add("Message", string.Format("{0} already has 4 teams, please choose another group.",group.Name));
                    return View(model);
                }
            }

                var team = new Team(); //this is creating a new instance of an object
            team.Name = model.Name;
            team.Flag = model.Flag;
            team.GroupId = model.GroupId;
            team.CreationDate = DateTime.Now;
            team.CreationSource = "Dan";

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                dbContext.Teams.Add(team);
                dbContext.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        [HttpPost] //this submits data to be processed to a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Edit(Euro2016.Models.TeamViewModel model)
        {
            if (!ModelState.IsValid) //this checks if the model is valid, if it's not valid then we are returning the view and displaying an error. The model is invalid if a required property is not filled in for example.
            {
                return View(model);
            }

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
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

        [HttpGet] //this requests data from a specified resource
        [Authorize] //this means the user has to login to access the view
        public ActionResult Delete(int id)
        {
            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
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

        [HttpGet] //this requests data from a specified resource
        public ActionResult Fixtures(int id)
        {

            var model = new Euro2016.Models.TeamFixturesViewModel(); //this is creating a new instance of an object
            model.Fixtures = new List<Fixture>();

            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
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