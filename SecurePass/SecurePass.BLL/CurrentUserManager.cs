// <copyright file="CurrentUserManager.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.BLL
{
    using SecurePass.DAL.Model;

    /// <summary>
    /// The <c>CurrentUserManager</c> class provides a centralized and static management
    /// of the currently logged-in user within the application.
    /// </summary>
    public class CurrentUserManager
    {
        /// <summary>
        /// Gets or represents the currently logged-in user.
        /// </summary>
        public static UserModel CurrentUser { get; private set; }

        /// <summary>
        /// Sets the currently logged-in user.
        /// </summary>
        /// <param name="user">The user to set as the current user.</param>
        public static void SetCurrentUser(UserModel user)
        {
            CurrentUser = user;
        }
    }
}