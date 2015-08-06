using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
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

        [HttpGet]
        public IActionResult Messages()
        {
            var model = db.Messages.OrderByDescending(_ => _.Time).Take(50);

            return View(model);
        }

        [HttpPost]
        public IActionResult Messages(string messageText)
        {
            if (!User.Identity.IsAuthenticated) return null;

            var date = DateTime.Now;
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
