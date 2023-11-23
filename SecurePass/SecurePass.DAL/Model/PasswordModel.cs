// <copyright file="PasswordModel.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.DAL.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The <c>PasswordModel</c> class represents a password entity in the application.
    /// </summary>
    [Table("passwords")]
    public class PasswordModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the password.
        /// </summary>
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the password. This property is required.
        /// </summary>
        [Column("title")]
        required public string Title { get; set; }

        /// <summary>
        /// Gets or sets the email or username associated with the password. It can be null.
        /// </summary>
        [Column("email_username")]
        public string? Email_Username { get; set; }

        /// <summary>
        /// Gets or sets the password. This property is required.
        /// </summary>
        [Column("password")]
        required public string Password { get; set; }

        /// <summary>
        /// Gets or sets the folder ID associated with the password. It can be null.
        /// </summary>
        [Column("folder_id")]
        public int? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the password. This property is required.
        /// </summary>
        [Column("user_id")]
        required public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the password was last updated.
        /// </summary>
        [Column("last_updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a flag indicating whether the password is marked as deleted.
        /// </summary>
        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}