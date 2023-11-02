using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurePass.DAL.Model
{
    public class SecurePassDbContext : DbContext
    {
        
        public DbSet<UserModel> Users { get; set; }
        public DbSet<FolderModel> Folders { get; set; }
        public DbSet<PasswordModel> Passwords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=1234;Database=securepass");

    }
}
