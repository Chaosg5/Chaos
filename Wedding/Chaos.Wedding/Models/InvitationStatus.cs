//-----------------------------------------------------------------------
// <copyright file="InvitationStatus.cs">
//     Copyright (c) Erik Bunnstad. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Chaos.Wedding.Models
{
    /// <summary>The invitation status.</summary>
    public enum InvitationStatus
    {
        /// <summary>Not invited.</summary>
        None = 0,

        /// <summary>Has been invited.</summary>
        Invited = 1,

        /// <summary>Invitation accepted.</summary>
        Accepted = 1
    }
}