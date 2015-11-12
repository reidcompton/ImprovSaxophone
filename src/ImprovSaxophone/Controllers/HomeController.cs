using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ImprovSaxophone.Models;

namespace ImprovSaxophone.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string[] letters = { "a", "b", "c", "d", "e", "f", "g" };
            int[] nums = { 1, 2 };
            Random rand = new Random();
            Random rand2 = new Random();
            int randLetter = rand.Next(letters.Count());
            int randNumber = rand2.Next(nums.Count());
            string name = string.Format("{0}_{1}", letters[randLetter], nums[randNumber]);
            ViewBag.name = name;
            return View();
        }

        public string RandomNote(Random r1, Random r2)
        {
            string[] letters = { "a", "b", "c", "d", "e", "f", "g" };
            int[] nums = { 1, 2 };
            int randLetter = r1.Next(letters.Count());
            int randNumber = r2.Next(nums.Count());
            string name = string.Format("{0}_{1}", letters[randLetter], nums[randNumber]);
            return name;
        }

        public JsonResult Measure(DateTime start, int bpm, int tsN, int tsD, double measureFraction = 1.0, string root = "", string quality = "", string auxiliary = "")
        {
            Measure measure = new Measure() {
                Start = start
            };
            List<Note> notes = new List<Note>();
            Random r1 = new Random();
            Random r2 = new Random();
            for (int i = 0; i < 20; i++)
            {
                notes.Add(new Note
                {
                    Duration = (r1.Next(0, 1000)),
                    Value = RandomNote(r1, r2)
                });
            }
            measure.Notes = notes.ToArray();
            return Json(measure);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
