﻿//-----------------------------------------------------------------------
// <copyright file="ChaosMoviesService.svc.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        #region Generic

        #region Character 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task CharacterSaveAsync(UserSessionDto session, CharacterDto character)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Character.Static.FromContract(character).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }
        
        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<CharacterDto>> CharacterGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Character.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region Department 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task DepartmentSaveAsync(UserSessionDto session, DepartmentDto department)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Department.Static.FromContract(department).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<DepartmentDto>> DepartmentGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Department.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<DepartmentDto>> DepartmentGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Department.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region Error

        /// <inheritdoc />
        /// <exception cref="FaultException">An exception occurred.</exception>
        public async Task ErrorSaveAsync(UserSessionDto session, ErrorDto error)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Error.Static.FromContract(error).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region ExternalSource 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task ExternalSourceSaveAsync(UserSessionDto session, ExternalSourceDto externalSource)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await ExternalSource.Static.FromContract(externalSource).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<ExternalSourceDto>> ExternalSourceGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await ExternalSource.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region Genre 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task GenreSaveAsync(UserSessionDto session, GenreDto genre)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Genre.Static.FromContract(genre).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<GenreDto>> GenreGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Genre.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<GenreDto>> GenreGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Genre.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region Icon 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task IconSaveAsync(UserSessionDto session, IconDto icon)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Icon.Static.FromContract(icon).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<IconDto>> IconGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Icon.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region IconType 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task IconTypeSaveAsync(UserSessionDto session, IconTypeDto iconType)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await IconType.Static.FromContract(iconType).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<IconTypeDto>> IconTypeGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await IconType.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<IconTypeDto>> IconTypeGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await IconType.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region Movie 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task MovieSaveAsync(UserSessionDto session, MovieDto movie)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Movie.Static.FromContract(movie).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<MovieDto>> MovieGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Movie.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region MovieSeries 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task MovieSeriesSaveAsync(UserSessionDto session, MovieSeriesDto movieSeries)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await MovieSeries.Static.FromContract(movieSeries).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<MovieSeriesDto>> MovieSeriesGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await MovieSeries.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region MovieSeriesType 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task MovieSeriesTypeSaveAsync(UserSessionDto session, MovieSeriesTypeDto movieSeriesType)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await MovieSeriesType.Static.FromContract(movieSeriesType).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<MovieSeriesTypeDto>> MovieSeriesTypeGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await MovieSeriesType.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<MovieSeriesTypeDto>> MovieSeriesTypeGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await MovieSeriesType.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region MovieType 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task MovieTypeSaveAsync(UserSessionDto session, MovieTypeDto movieType)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await MovieType.Static.FromContract(movieType).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<MovieTypeDto>> MovieTypeGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await MovieType.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<MovieTypeDto>> MovieTypeGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await MovieType.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region Person 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task PersonSaveAsync(UserSessionDto session, PersonDto person)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Person.Static.FromContract(person).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<PersonDto>> PersonGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Person.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region RatingSystem 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task RatingSystemSaveAsync(UserSessionDto session, RatingSystemDto ratingSystem)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await RatingSystem.Static.FromContract(ratingSystem).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<RatingSystemDto>> RatingSystemGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await RatingSystem.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<RatingSystemDto>> RatingSystemGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await RatingSystem.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region RatingType 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task RatingTypeSaveAsync(UserSessionDto session, RatingTypeDto ratingType)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await RatingType.Static.FromContract(ratingType).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<RatingTypeDto>> RatingTypeGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await RatingType.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<RatingTypeDto>> RatingTypeGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await RatingType.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region Role 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task RoleSaveAsync(UserSessionDto session, RoleDto role)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await Role.Static.FromContract(role).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<RoleDto>> RoleGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Role.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<RoleDto>> RoleGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await Role.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region User 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task UserSaveAsync(UserSessionDto session, UserDto user)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await User.Static.FromContract(user).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<UserDto>> UserGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await User.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region UserSession

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception>
        public async Task UserSessionSaveAsync(UserSessionDto session, UserSessionDto userSession)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await UserSession.Static.FromContract(userSession).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #region WatchType 

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task WatchTypeSaveAsync(UserSessionDto session, WatchTypeDto watchType)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                await WatchType.Static.FromContract(watchType).SaveAsync(UserSession.Static.FromContract(session));
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc /> 
        /// <exception cref="FaultException">An exception occurred.</exception> 
        public async Task<IEnumerable<WatchTypeDto>> WatchTypeGetAsync(UserSessionDto session, IEnumerable<int> idList)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await WatchType.Static.GetAsync(UserSession.Static.FromContract(session), idList)).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        /// <inheritdoc />
        /// <exception cref="FaultException">An exception occurred.</exception>
        public async Task<IEnumerable<WatchTypeDto>> WatchTypeGetAllAsync(UserSessionDto session)
        {
            try
            {
                await this.ValidateStateAndSessionAsync(session);
                return (await WatchType.Static.GetAllAsync(UserSession.Static.FromContract(session))).Select(c => c.ToContract());
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        #endregion

        #endregion

        public async Task CreateUserAsync(UserSessionDto session, User user, UserLogin userLogin)
        {
        }

        /// <inheritdoc/>
        /// <exception cref="FaultException">An exception occurred.</exception>
        public async Task<UserSessionDto> CreateUserSessionAsync(UserLogin userLogin)
        {
            try
            {
                return (await UserSession.Static.CreateSessionAsync(userLogin)).ToContract();
            }
            catch (Exception exception)
            {
                throw new FaultException(exception.Message);
            }
        }

        public void MovieSearch(string searchText)
        {
        }

        public void MovieSearchNewUpdates(string searchText)
        {
            // var s = new TMDbLib.Client.TMDbClient()
            // TMDbLib.Objects.People.Person
        }

        public void LoadMovie(string movieIdentifier)
        {
            ////var s = new TMDbLib.Client.TMDbClient(Properties.Settings.Default.TmdbApiKey);

            // s.GetMovieAsync()
        }

        /// <summary>The validate state and session async.</summary>
        /// <param name="session">The session.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ServiceRequiredException">Configuration is not valid.</exception>
        private async Task ValidateStateAndSessionAsync(UserSessionDto session)
        {
            if (Persistent.UseService)
            {
                throw new ServiceRequiredException($"The {nameof(Persistent.UseService)} setting has to be set to false.");
            }

            await UserSession.Static.FromContract(session).ValidateSessionAsync();
        }
    }
}