using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DoorWebsite.Controllers
{
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Models;

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DoorManager _doorManager;

        public HomeController(IConfiguration configuration, DoorManager doorManager)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _doorManager = doorManager ?? throw new ArgumentNullException(nameof(doorManager));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection formCollection)
        {
            Validate();
            if (ModelState.IsValid)
            {
                _doorManager.Open();
            }

            return View();
        }

        private void Validate()
        {
            var start = _configuration.GetSection("Door").GetValue<int>("StartHour");
            var end = _configuration.GetSection("Door").GetValue<int>("EndHour");

            if (DateTime.Now.IsWeekend())
            {
                ModelState.AddModelError("", "The door can only be opened on week days");
            }
            else if (DateTime.Now.IsDutchHoliday())
            {
                ModelState.AddModelError("", "The door cannot be opened on holidays");
            }
            else if (DateTime.Now.Hour < start)
            {
                ModelState.AddModelError("", $"The door can only be opened after {start}:00");
            }
            else if (DateTime.Now.Hour > end)
            {
                ModelState.AddModelError("", $"The door can only be opened before {end}:00");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
