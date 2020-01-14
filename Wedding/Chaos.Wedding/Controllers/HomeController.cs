using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chaos.Wedding.Controllers
{
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;
    using Chaos.Wedding.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Inbjudan(string id)
        {
            ViewBag.Message = "Inbjudan";

            if (id != null)
            {
                var address = await Address.Static.GetAsync(await SessionHandler.GetSessionAsync(), new Guid(id));
                return View(address);
            }

            return View();
        }

        public async Task<ActionResult> GetLookupShort(string lookupShort)
        {
            try
            {
                var addresses = await Address.Static.SearchAsync(
                    new SearchParametersDto { SearchText = lookupShort },
                    await SessionHandler.GetSessionAsync());
                return this.Content(addresses.First().LookupId.ToString("D"));
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> SetReceptionStatus(int guestId, int invitationStatus)
        {
            try
            {
                var session = await SessionHandler.GetSessionAsync();
                var guest = await Guest.Static.GetAsync(session, guestId);
                if (!Enum.IsDefined(typeof(InvitationStatus), invitationStatus))
                {
                    throw new InvalidSaveCandidateException(string.Format("The invitation status {0} is not valid.", invitationStatus));
                }

                guest.Reception = (InvitationStatus)invitationStatus;
                await guest.SaveAsync(session);
                return this.Content(string.Empty);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> SetDinnerStatus(int guestId, int invitationStatus)
        {
            try
            {
                var session = await SessionHandler.GetSessionAsync();
                var guest = await Guest.Static.GetAsync(session, guestId);
                if (!Enum.IsDefined(typeof(InvitationStatus), invitationStatus))
                {
                    throw new InvalidSaveCandidateException(string.Format("The invitation status {0} is not valid.", invitationStatus));
                }

                guest.Dinner = (InvitationStatus)invitationStatus;
                await guest.SaveAsync(session);
                return this.Content(string.Empty);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> SaveGuestInformation(int guestId, string information)
        {
            try
            {
                var session = await SessionHandler.GetSessionAsync();
                var guest = await Guest.Static.GetAsync(session, guestId);
                guest.Information = information;
                await guest.SaveAsync(session);
                return this.Content(string.Empty);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> SaveNewGuest(string name, int addressId)
        {
            try
            {
                var session = await SessionHandler.GetSessionAsync();
                var guest = new Guest(name, addressId);
                await guest.SaveAsync(session);
                return this.Content(string.Empty);
            }
            catch (Exception exception)
            {
                throw;
            }
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

        public ActionResult Schema()
        {
            return View();
        }

        public ActionResult Stadsjakt()
        {
            return View();
        }
    }
}