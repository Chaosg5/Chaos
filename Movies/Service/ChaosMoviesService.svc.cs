//-----------------------------------------------------------------------
// <copyright file="ChaosMoviesService.svc.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Service
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;
    
    using Chaos.Movies.Contract.Dto;
    using Chaos.Movies.Model;
    using Chaos.Movies.Model.Exceptions;
    using Chaos.Movies.Model.Sql;

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
        }

        public void LoadMovie(string movieIdentifier)
        {
            var s = new TMDbLib.Client.TMDbClient(TmdbApiKey);
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

        public async Task RatingSaveAsync(UserSessionDto session, Rating rating)
        {
            try
            {
                if (rating == null)
                {
                    throw new ArgumentNullException("rating");
                }

                rating.Save();
            }
            catch (Exception exception)
            {
                // Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        public async Task RatingSaveAllAsync(UserSessionDto session, Rating rating)
        {
            try
            {
                if (rating == null)
                {
                    throw new ArgumentNullException("rating");
                }

                await rating.SaveAllAsync();
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
            this.ValidateState();
            await new SqlCharacter(character).SaveAsync(new UserSession(session));
        }

        /// <summary></summary>
        /// <param name="characterToKeep"></param>
        /// <param name="characterIdToMerge"></param>
        public void MergeCharacters(UserSessionDto session, Character characterToKeep, int characterIdToMerge)
        {
        }

        #endregion

        private void ValidateState()
        {
            if (Model.Sql.Persistent.UseService)
            {
                throw new ServiceRequiredException($"The {nameof(Model.Sql.Persistent.UseService)} setting has to be set to false.");
            }
        }
    }
}