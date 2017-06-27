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

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;
    
    /// <summary>Internal service for Chaos Movies.</summary>
    public class ChaosMoviesService : IChaosMoviesService
    {
        public async Task CreateUserAsync(UserSession session, User user, UserLogin userLogin)
        {
            
        }

        public async Task<UserSession> CreateUserSessionAsync(UserSession session, UserLogin userLogin)
        {
            return null;
        }
        
        public void MovieSave(UserSession session, Movie movie)
        {
            try
            {
                //var s = new TMDbLib.Client.TMDbClient()
            }
            catch (Exception exception)
            {
                //Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        public async Task RatingSaveAsync(UserSession session, Rating rating)
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
                //Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        public async Task RatingSaveAllAsync(UserSession session, Rating rating)
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
                //Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        #region Character

        public async Task CharacterSaveAsync(UserSession session, ICharacter character)
        {
            
        }

        /// <summary></summary>
        /// <param name="characterToKeep"></param>
        /// <param name="characterIdToMerge"></param>
        public void MergeCharacters(UserSession session, Character characterToKeep, int characterIdToMerge)
        {

        }

        #endregion
        }
}
