// <copyright file="PasswordViewModel.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation.ViewModel
{
    using SecurePass.DAL.Model;

    /// <summary>
    /// The <c>PasswordViewModel</c> class represents a view model for displaying password information in the user interface.
    /// </summary>
    public class PasswordViewModel
    {
        /// <summary>
        /// Gets or sets the underlying password model.
        /// </summary>
        required public PasswordModel Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the password is visible.
        /// </summary>
        public bool IsPasswordVisible { get; set; }

        /// <summary>
        /// Creates a new <c>PasswordViewModel</c> instance from a given <c>PasswordModel</c>.
        /// </summary>
        /// <param name="password">The password model to create the view model from.</param>
        /// <returns>A new instance of <c>PasswordViewModel</c>.</returns>
        public static PasswordViewModel CreateFromPassword(PasswordModel password)
        {
            return new PasswordViewModel { Password = password, IsPasswordVisible = false };
        }

        /// <summary>
        /// Updates the underlying <c>PasswordModel</c> with new information.
        /// </summary>
        /// <param name="title">The new title for the password.</param>
        /// <param name="emailOrUsername">The new email or username for the password.</param>
        /// <param name="password">The new password.</param>
        /// <param name="folderId">The new folder ID for the password, can be null.</param>
        public void UpdatePasswordModel(string title, string emailOrUsername, string password, int? folderId)
        {
            Password.Title = title;
            Password.Email_Username = emailOrUsername;
            Password.Password = password;
            Password.FolderId = folderId;
        }
    }
}
