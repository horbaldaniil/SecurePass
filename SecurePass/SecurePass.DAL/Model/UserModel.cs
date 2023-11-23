// <copyright file="UserModel.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.DAL.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The <c>UserModel</c> class represents a user entity in the application.
    /// </summary>
    [Table("users")]
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user. This property is required.
        /// </summary>
        [Column("email")]
        required public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user. This property is required.
        /// </summary>
        [Column("password")]
        required public string Password { get; set; }
    }
}
