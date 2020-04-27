//-----------------------------------------------------------------------
// <copyright file="HomeController.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model.Exceptions;
    using Chaos.Wedding.Models;

    using NLog;

    /// <inheritdoc />
    /// <summary>The home controller.</summary>
    public class HomeController : Controller
    {
        /// <summary>The logger.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>The index.</summary>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>The invitation.</summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> Inbjudan(string id)
        {
            try
            {
                ViewBag.Message = "Inbjudan";

                if (id != null)
                {
                    var address = await Address.Static.GetAsync(await SessionHandler.GetSessionAsync(), new Guid(id));
                    Logger.Info(CultureInfo.InvariantCulture, "Address viewed: {0}", address.LookupId);
                    return this.View(address);
                }

                return this.View();
            }
            catch (Exception exception)
            {
                Logger.Error(exception, CultureInfo.InvariantCulture, "Failed to get address with id {0}", id);
                throw;
            }
        }

        /// <summary>The get lookup short.</summary>
        /// <param name="lookupShort">The lookup short.</param>
        /// <returns>The <see cref="Task"/>.</returns>
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
                Logger.Error(exception);
                throw;
            }
        }

        /// <summary>The set reception status.</summary>
        /// <param name="guestId">The guest id.</param>
        /// <param name="invitationStatus">The invitation status.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> SetReceptionStatus(int guestId, int invitationStatus)
        {
            try
            {
                var session = await SessionHandler.GetSessionAsync();
                var guest = await Guest.Static.GetAsync(session, guestId);
                if (!Enum.IsDefined(typeof(InvitationStatus), invitationStatus))
                {
                    throw new InvalidSaveCandidateException(string.Format(CultureInfo.InvariantCulture, "The invitation status {0} is not valid.", invitationStatus));
                }

                guest.Reception = (InvitationStatus)invitationStatus;
                await guest.SaveAsync(session);
                return this.Content(string.Empty);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        /// <summary>The set reception status.</summary>
        /// <param name="giftId">The gift id.</param>
        /// <param name="bookedStatus">The booked status.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ActionResult> SetGiftBookedStatus(int giftId, int bookedStatus)
        {
            try
            {
                var session = await SessionHandler.GetSessionAsync();
                var gift = await Gift.Static.GetAsync(session, giftId);
                if (!Enum.IsDefined(typeof(InvitationStatus), bookedStatus))
                {
                    throw new InvalidSaveCandidateException(string.Format(CultureInfo.InvariantCulture, "The invitation status {0} is not valid.", bookedStatus));
                }

                gift.Booked = (InvitationStatus)bookedStatus;
                await gift.SaveAsync(session);
                return this.Content(string.Empty);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
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
                Logger.Error(exception);
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
                Logger.Error(exception);
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
                Logger.Error(exception);
                throw;
            }
        }

        public ActionResult Info()
        {
            ViewBag.Message = "Information";

            return View();
        }

        public async Task<ActionResult> Presenter()
        {
            var session = await SessionHandler.GetSessionAsync();
            var gifts = await Gift.Static.GetAllAsync(session);
            return this.View(gifts.OrderBy(g => g.Price));
        }

        public ActionResult Schema()
        {
            return View();
        }

        public ActionResult Stadsjakt()
        {
            return View();
        }

        public ActionResult Bilder()
        {
            return View();
        }

        public ActionResult Historia()
        {
            return View();
        }

        public ActionResult Svar()
        {
            return View();
        }
    }
}