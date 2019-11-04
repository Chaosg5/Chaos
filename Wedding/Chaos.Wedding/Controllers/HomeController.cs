using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chaos.Wedding.Controllers
{
    using System.Threading.Tasks;

    using Chaos.Wedding.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Inbjudan(string addressLookupId)
        {
            ViewBag.Message = "Inbjudan";

            if (addressLookupId != null)
            {
                var address = await Address.Static.GetAsync(await SessionHandler.GetSessionAsync(), new Guid(addressLookupId));
            }

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