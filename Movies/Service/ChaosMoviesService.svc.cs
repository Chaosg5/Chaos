//-----------------------------------------------------------------------
// <copyright file="ChaosMoviesService.svc.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;

    /// <summary>Internal service for Chaos Movies.</summary>
    public class ChaosMoviesService : IChaosMoviesService
    {
        private static string TmdbApiKey = "edd081789d1682057b56406a750f9e01";
        // eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJlZGQwODE3ODlkMTY4MjA1N2I1NjQwNmE3NTBmOWUwMSIsInN1YiI6IjUwNGJhODNhMTljMjk1NzM5OTAwMTZiNCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.E4BK8hZVN1I6a7YLKYh2Ia8PVtL4-Sxag8qfSOlAY5Y
        // https://api.themoviedb.org/3/movie/550?api_key=edd081789d1682057b56406a750f9e01

        public async Task CreateUserAsync(UserSessionDto session, User user, UserLogin userLogin)
        {
        }

        public async Task<UserSessionDto> CreateUserSessionAsync(UserSessionDto session, UserLoginDto userLogin)
        {
            return null;
        }

        public void MovieSearch(string searchText)
        {
            
        }

        public void MovieSearchNewUpdates(string searchText)
        {
            // var s = new TMDbLib.Client.TMDbClient()
            //TMDbLib.Objects.People.Person
        }

        public void LoadMovie(string movieIdentifier)
        {
            var s = new TMDbLib.Client.TMDbClient(Properties.Settings.Default.TmdbApiKey);
            //s.GetMovieAsync()
        }

        public void MovieSave(UserSessionDto session, Movie movie)
        {
            try
            {
            }
            catch (Exception exception)
            {
                // Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        public async Task RatingSaveAsync(UserSessionDto session, UserRating userRating)
        {
            try
            {
                if (userRating == null)
                {
                    throw new ArgumentNullException(nameof(userRating));
                }
                
                ////userRating.Save();
            }
            catch (Exception exception)
            {
                // Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        #region Character

        public async Task CharacterSaveAsync(UserSessionDto session, CharacterDto character)
        {
            this.ValidateStateAndSession(session);
            await Character.Static.SaveAsync(new UserSession(session));
        }

        public async Task<IEnumerable<CharacterDto>> CharacterGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            this.ValidateStateAndSession(session);
            return (await Character.Static.GetAsync(new UserSession(session), idList)).Select(c => c.ToContract());
        }

        /// <summary></summary>
        /// <param name="characterToKeep"></param>
        /// <param name="characterIdToMerge"></param>
        public void MergeCharacters(UserSessionDto session, Character characterToKeep, int characterIdToMerge)
        {
        }

        #endregion

        private void ValidateStateAndSession(UserSessionDto session)
        {
            if (Persistent.UseService)
            {
                throw new ServiceRequiredException($"The {nameof(Persistent.UseService)} setting has to be set to false.");
            }
        }
    }
}