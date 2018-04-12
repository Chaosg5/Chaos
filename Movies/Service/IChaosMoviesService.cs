//-----------------------------------------------------------------------
// <copyright file="IChaosMoviesService.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Service
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;

    /// <summary>Interface for <see cref="ChaosMoviesService"/>.</summary>
    [ServiceContract]
    public interface IChaosMoviesService
    {
        #region Generic

        #region Character	

        #region Character 

        /// <summary>Saves the <paramref name="character"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="character">The <see cref="CharacterDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task CharacterSaveAsync(UserSessionDto session, CharacterDto character);

        /// <summary>Gets the <see cref="CharacterDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<CharacterDto>> CharacterGetAsync(UserSessionDto session, IEnumerable<int> idList);

        #endregion

        #region Department 

        /// <summary>Saves the <paramref name="department"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="department">The <see cref="DepartmentDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task DepartmentSaveAsync(UserSessionDto session, DepartmentDto department);

        /// <summary>Gets the <see cref="DepartmentDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<DepartmentDto>> DepartmentGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="DepartmentDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<DepartmentDto>> DepartmentGetAllAsync(UserSessionDto session);

        #endregion

        #region Error	

        /// <summary>Saves the <paramref name="error"/>.</summary>
        /// <param name="session">The session.</param>
        /// <param name="error">The <see cref="ErrorDto"/> to save.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [OperationContract]
        Task ErrorSaveAsync(UserSessionDto session, ErrorDto error);
        
        #endregion

        #region ExternalSource 

        /// <summary>Saves the <paramref name="externalSource"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="externalSource">The <see cref="ExternalSourceDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task ExternalSourceSaveAsync(UserSessionDto session, ExternalSourceDto externalSource);

        /// <summary>Gets the <see cref="ExternalSourceDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<ExternalSourceDto>> ExternalSourceGetAsync(UserSessionDto session, IEnumerable<int> idList);

        #endregion

        #region Genre 

        /// <summary>Saves the <paramref name="genre"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="genre">The <see cref="GenreDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task GenreSaveAsync(UserSessionDto session, GenreDto genre);

        /// <summary>Gets the <see cref="GenreDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<GenreDto>> GenreGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="GenreDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<GenreDto>> GenreGetAllAsync(UserSessionDto session);

        #endregion

        #region Icon 

        /// <summary>Saves the <paramref name="icon"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="icon">The <see cref="IconDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task IconSaveAsync(UserSessionDto session, IconDto icon);

        /// <summary>Gets the <see cref="IconDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<IconDto>> IconGetAsync(UserSessionDto session, IEnumerable<int> idList);

        #endregion

        #region IconType 

        /// <summary>Saves the <paramref name="iconType"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="iconType">The <see cref="IconTypeDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task IconTypeSaveAsync(UserSessionDto session, IconTypeDto iconType);

        /// <summary>Gets the <see cref="IconTypeDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<IconTypeDto>> IconTypeGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="IconTypeDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<IconTypeDto>> IconTypeGetAllAsync(UserSessionDto session);

        #endregion

        #region Movie 

        /// <summary>Saves the <paramref name="movie"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="movie">The <see cref="MovieDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task MovieSaveAsync(UserSessionDto session, MovieDto movie);

        /// <summary>Gets the <see cref="MovieDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<MovieDto>> MovieGetAsync(UserSessionDto session, IEnumerable<int> idList);

        #endregion

        #region MovieSeries 

        /// <summary>Saves the <paramref name="movieSeries"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="movieSeries">The <see cref="MovieSeriesDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task MovieSeriesSaveAsync(UserSessionDto session, MovieSeriesDto movieSeries);

        /// <summary>Gets the <see cref="MovieSeriesDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<MovieSeriesDto>> MovieSeriesGetAsync(UserSessionDto session, IEnumerable<int> idList);

        #endregion

        #region MovieSeriesType 

        /// <summary>Saves the <paramref name="movieSeriesType"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="movieSeriesType">The <see cref="MovieSeriesTypeDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task MovieSeriesTypeSaveAsync(UserSessionDto session, MovieSeriesTypeDto movieSeriesType);

        /// <summary>Gets the <see cref="MovieSeriesTypeDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<MovieSeriesTypeDto>> MovieSeriesTypeGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="MovieSeriesTypeDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<MovieSeriesTypeDto>> MovieSeriesTypeGetAllAsync(UserSessionDto session);

        #endregion

        #region MovieType 

        /// <summary>Saves the <paramref name="movieType"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="movieType">The <see cref="MovieTypeDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task MovieTypeSaveAsync(UserSessionDto session, MovieTypeDto movieType);

        /// <summary>Gets the <see cref="MovieTypeDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<MovieTypeDto>> MovieTypeGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="MovieTypeDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<MovieTypeDto>> MovieTypeGetAllAsync(UserSessionDto session);

        #endregion

        #region Person 

        /// <summary>Saves the <paramref name="person"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="person">The <see cref="PersonDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task PersonSaveAsync(UserSessionDto session, PersonDto person);

        /// <summary>Gets the <see cref="PersonDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<PersonDto>> PersonGetAsync(UserSessionDto session, IEnumerable<int> idList);

        #endregion

        #region RatingSystem 

        /// <summary>Saves the <paramref name="ratingSystem"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="ratingSystem">The <see cref="RatingSystemDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task RatingSystemSaveAsync(UserSessionDto session, RatingSystemDto ratingSystem);

        /// <summary>Gets the <see cref="RatingSystemDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<RatingSystemDto>> RatingSystemGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="RatingSystemDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<RatingSystemDto>> RatingSystemGetAllAsync(UserSessionDto session);

        #endregion

        #region RatingType 

        /// <summary>Saves the <paramref name="ratingType"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="ratingType">The <see cref="RatingTypeDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task RatingTypeSaveAsync(UserSessionDto session, RatingTypeDto ratingType);

        /// <summary>Gets the <see cref="RatingTypeDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<RatingTypeDto>> RatingTypeGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="RatingTypeDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<RatingTypeDto>> RatingTypeGetAllAsync(UserSessionDto session);

        #endregion

        #region Role 

        /// <summary>Saves the <paramref name="role"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="role">The <see cref="RoleDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task RoleSaveAsync(UserSessionDto session, RoleDto role);

        /// <summary>Gets the <see cref="RoleDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<RoleDto>> RoleGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="RoleDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<RoleDto>> RoleGetAllAsync(UserSessionDto session);

        #endregion

        #region User 

        /// <summary>Saves the <paramref name="user"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="user">The <see cref="UserDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task UserSaveAsync(UserSessionDto session, UserDto user);

        /// <summary>Gets the <see cref="UserDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<UserDto>> UserGetAsync(UserSessionDto session, IEnumerable<int> idList);

        #endregion

        #region UserSession

        /// <summary>Saves the <paramref name="userSession"/>.</summary>
        /// <param name="session">The session.</param>
        /// <param name="userSession">The <see cref="UserSessionDto"/> to save.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [OperationContract]
        Task UserSessionSaveAsync(UserSessionDto session, UserSessionDto userSession);
        
        #endregion

        #region WatchType 

        /// <summary>Saves the <paramref name="watchType"/>.</summary> 
        /// <param name="session">The session.</param> 
        /// <param name="watchType">The <see cref="WatchTypeDto"/> to save.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task WatchTypeSaveAsync(UserSessionDto session, WatchTypeDto watchType);

        /// <summary>Gets the <see cref="WatchTypeDto"/> with the specified <paramref name="idList"/>.</summary> 
        /// <param name="session">The session.</param>
        /// <param name="idList">The id list.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<WatchTypeDto>> WatchTypeGetAsync(UserSessionDto session, IEnumerable<int> idList);

        /// <summary>Gets all <see cref="WatchTypeDto"/>s.</summary> 
        /// <param name="session">The session.</param> 
        /// <returns>The <see cref="Task"/>.</returns> 
        [OperationContract]
        Task<IEnumerable<WatchTypeDto>> WatchTypeGetAllAsync(UserSessionDto session);

        #endregion

        #endregion

        #endregion

        #region Custom

        /// <summary>Creates a new <see cref="UserSessionDto"/> from the <paramref name="userLogin"/>.</summary>
        /// <param name="userLogin">The user login data.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [OperationContract]
        Task<UserSessionDto> CreateUserSessionAsync(UserLogin userLogin);

        #endregion
    }
}