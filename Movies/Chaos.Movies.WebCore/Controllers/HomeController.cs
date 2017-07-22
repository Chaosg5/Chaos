using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Chaos.Movies.WebCore.Controllers
{
    using Chaos.Movies.Model;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

        public IActionResult Movie()
        {
            var movie = new Movie();
            movie.Characters.AddCharacter(new CharacterInMovie(new Person("Paula Patton"), new Character("Garona"), 0));
            movie.Characters.AddCharacter(new CharacterInMovie(new Person("Travis Fimmel"), new Character("Anduin Lothar"), 0));
            movie.Characters.AddCharacter(new CharacterInMovie(new Person("Ben Foster"), new Character("Medivh"), 0));
            return this.View(movie);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
