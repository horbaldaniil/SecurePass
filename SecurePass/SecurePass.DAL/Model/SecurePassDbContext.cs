// <copyright file="SecurePassDbContext.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.DAL.Model
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The <c>SecurePassDbContext</c> class represents the database context for the SecurePass application.
    /// </summary>
    public class SecurePassDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet for the User entities.
        /// </summary>
        public DbSet<UserModel> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for the Folder entities.
        /// </summary>
        public DbSet<FolderModel> Folders { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for the Password entities.
        /// </summary>
        public DbSet<PasswordModel> Passwords { get; set; }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=1234;Database=securepass");
    }
}
