using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Euro2016.Controllers
{
    public class ResultsController : Controller
    {
        // GET: Results
        public ActionResult Index()
        {
            var model = new Euro2016.Models.ResultsViewModel(); //this is creating a new instance of an object


            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                model.Results = dbContext.Results
                    .Include(x => x.Fixture.HomeTeam)
                    .Include(x => x.Fixture.AwayTeam)
                    .Include(x => x.Fixture.Venue)
                    .ToList();
            }
            return View(model);
        }

        public ActionResult GenerateResults()
        {
            var rnd = new Random(); //this is creating a new instance of an object
            var maxScore = 5;


            using (var dbContext = new Euro2016Entities()) //this gets a database connection until the curly bracket is closed
            {
                //Clear results
                var results = dbContext.Results.ToList();
                foreach (var result in results)
                {
                    dbContext.Results.Remove(result);
                }

                //Reset Teams

                var teams = dbContext.Teams.ToList();
                foreach (var team in teams)
                {
                    team.Played = 0;
                    team.Won = 0;
                    team.Drawn = 0;
                    team.Lost = 0;
                    team.Scored = 0;
                    team.Conceded = 0;
                    team.Points = 0;
                }

                //dbContext.SaveChanges();


                var fixtures = dbContext.Fixtures.ToList();
                foreach (var fixture in fixtures)
                {
                    var homeScore = rnd.Next(maxScore);
                    var awayScore = rnd.Next(maxScore);
                    var homeTeam = fixture.HomeTeam;
                    var awayTeam = fixture.AwayTeam;
                    homeTeam.Played = homeTeam.Played + 1;
                    awayTeam.Played = awayTeam.Played + 1;
                    homeTeam.Scored = homeTeam.Scored + homeScore;
                    awayTeam.Scored = awayTeam.Scored + awayScore;
                    homeTeam.Conceded = homeTeam.Conceded + awayScore;
                    awayTeam.Conceded = awayTeam.Conceded + homeScore;

                    if (homeScore > awayScore)
                    {
                        homeTeam.Won = homeTeam.Won + 1;
                        homeTeam.Points = homeTeam.Points + 3;
                        awayTeam.Lost = awayTeam.Lost + 1;
                    }

                    if (awayScore > homeScore)
                    {
                        awayTeam.Won = awayTeam.Won + 1;
                        awayTeam.Points = awayTeam.Points + 3;
                        homeTeam.Lost = homeTeam.Lost + 1;
                    }

                    if (homeScore == awayScore)
                    {
                        homeTeam.Drawn = homeTeam.Drawn + 1;
                        awayTeam.Drawn = awayTeam.Drawn + 1;
                        homeTeam.Points = homeTeam.Points + 1;
                        awayTeam.Points = awayTeam.Points + 1;
                    }

       

                    var result = new Result(); //this is creating a new instance of an object
                    result.CreationDate = DateTime.Now;
                    result.CreationSource = "Dan";
                    result.HomeScore = homeScore;
                    result.AwayScore = awayScore;
                    result.FixtureId = fixture.Id;

                    dbContext.Results.Add(result);

                }

                dbContext.SaveChanges();

               

            }
            return RedirectToAction("Tables", "Home");


        }

   
    }
}