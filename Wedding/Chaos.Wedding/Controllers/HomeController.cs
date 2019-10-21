using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chaos.Wedding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inbjudan()
        {
            ViewBag.Message = "Inbjudan";

            return View();
        }

        public ActionResult Info()
        {
            ViewBag.Message = "Information";

            return View();
        }

        public ActionResult Presenter()
        {
            ViewBag.Message = "Information";

            return View();
        }
    }
}