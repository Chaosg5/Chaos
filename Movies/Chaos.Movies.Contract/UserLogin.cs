//-----------------------------------------------------------------------
// <copyright file="UserLogin.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Movies.Contract
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>A login request for a specific <see cref="UserDto"/>.</summary>
    [DataContract]
    public class UserLogin
    {
        /// <summary>Initializes a new instance of the <see cref="UserLogin"/> class.</summary>
        /// <param name="username">The <see cref="Username"/>.</param>
        /// <param name="password">The unencrypted <see cref="Password"/>, the password will immediately be encrypted.</param>
        /// <param name="clientIp">The <see cref="ClientIp"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="username"/>, <paramref name="password"/> or <paramref name="clientIp"/> is <see langword="null"/></exception>
        public UserLogin(string username, string password, string clientIp)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrEmpty(clientIp))
            {
                throw new ArgumentNullException(nameof(clientIp));
            }

            this.Username = username;
            this.Password = GetSha256Password(password);
            this.ClientIp = clientIp;
        }

        /// <summary>Gets the username of the user logging in.</summary>
        [DataMember]
        public string Username { get; }

        /// <summary>Gets the IP-address of the client device the user is using.</summary>
        [DataMember]
        public string ClientIp { get; }

        /// <summary>Gets the encrypted password.</summary>
        [DataMember]
        public string Password { get; }
        
        /// <summary>Encrypts the <paramref name="password"/>.</summary>
        /// <param name="password">The password to encrypt.</param>
        /// <returns>The encrypted <paramref name="password"/>.</returns>
        private static string GetSha256Password(string password)
        {
            var sha256 = new SHA256Managed();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }
    }
}