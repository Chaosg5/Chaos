//-----------------------------------------------------------------------
// <copyright file="IChaosMoviesService.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Service
{
    using System.ServiceModel;
    using System.Threading.Tasks;

    using Chaos.Movies.Contract;
    using Chaos.Movies.Model;

    /// <summary>Interface for <see cref="ChaosMoviesService"/>.</summary>
    [ServiceContract]
    public interface IChaosMoviesService
    {
        [OperationContract]
        Task<UserSessionDto> CreateUserSessionAsync(UserSessionDto session, UserLoginDto userLogin);
        
        [OperationContract]
        Task CharacterSaveAsync(UserSessionDto session, CharacterDto character);
    }
}
