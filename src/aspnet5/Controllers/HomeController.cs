using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using aspnet5.Models;
using aspnet5.Services;

namespace aspnet5.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        [FromServices]
        public TestService TestService { get; set; }

        public HomeController(ApplicationDbContext dataContext)
        {
            db = dataContext;
        }

        public IActionResult Index()
        {
            var test = TestService.TestName;

            return View();
        }

        public IActionResult Test(string answer)
        {
            if (!User.Identity.IsAuthenticated) return View();
            var userId = User.GetUserId();

            ApplicationDbContext db = new ApplicationDbContext();
            var stat = db.UserStats.SingleOrDefault(u => u.Token == userId);

            QuestTask task;
            if (stat == null)
            {
                task = db.QuestTasks.FirstOrDefault(t => t.Number == 0);
                db.UserStats.Add(new UserStat { TaskNumber = 0, Token = userId });
                db.SaveChanges();
            }
            else
            {
                if (answer != null)
                {
                    var userAnswer = new UserAnswer { Answer = answer, TaskNumber = stat.TaskNumber, UserId = userId };
                    db.UserAnswers.Add(userAnswer);
                    db.SaveChanges();
                }

                task = db.QuestTasks.FirstOrDefault(t => t.Number == stat.TaskNumber);
                if (task == null)
                {
                    ViewBag.Finished = true;
                    return View();
                }

                if (task.Number == 10 || (!string.IsNullOrEmpty(answer) && task.Answer == answer.ToLower()))
                {
                    task = db.QuestTasks.FirstOrDefault(t => t.Number == stat.TaskNumber + 1);
                    stat.TaskNumber++;
                    db.SaveChanges();
                }
            }

            if (stat != null && stat.TaskNumber == 11) ViewBag.Finished = true;

            return View(task);
        }

        [HttpGet]
        public IActionResult Messages()
        {
            var model = db.Messages.OrderByDescending(_ => _.Time).Take(50);

            return View(model);
        }

        [HttpPost]
        public IActionResult Messages(string messageText)
        {
            if (!User.Identity.IsAuthenticated) return View();

            var date = DateTime.Now;
            ApplicationDbContext db = new ApplicationDbContext();
            //var lastMessage = db.Messages.LastOrDefault(u => u.Author == User.Identity.Name);
            //if (lastMessage != null)
            //{
            //    var diff = date - DateTime.Parse(lastMessage.Time);
            //    if (diff.TotalSeconds < 2)
            //    {
            //        return Messages();
            //    }
            //}

            int count = db.Messages.Count(m => m.Author == User.Identity.Name);
            if (count > 100) return Messages();

            var msg = new Message()
            {
                Author = User.Identity.Name,
                //Date = date.ToShortDateString(),
                Text = messageText,
                Time = date.ToString()
            };

            db.Messages.Add(msg);
            db.SaveChanges();

            return Messages();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
