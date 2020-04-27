using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chaos.Wedding.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }
        // GET: Game
        public ActionResult Help()
        {
            return View();
        }
        // GET: Game
        public ActionResult Score()
        {
            return View();
        }
    }
}