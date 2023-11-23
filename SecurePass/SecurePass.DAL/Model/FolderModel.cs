// <copyright file="FolderModel.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.DAL.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The <c>FolderModel</c> class represents a folder entity in the application.
    /// </summary>
    [Table("folders")]
    public class FolderModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the folder.
        /// </summary>
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the folder. This property is required.
        /// </summary>
        [Column("title")]
        required public string Title { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the folder. This property is required.
        /// </summary>
        [Column("user_id")]
        required public int UserId { get; set; }
    }
}