using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurePass.DAL.Model
{
    [Table("users")]
    public class UserModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("email")]
        public required string Email { get; set; }
        [Column("password")]
        public required string Password { get; set; }
    }
}
