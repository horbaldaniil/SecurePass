using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace SecurePass.DAL.Model;

[Table("passwords")]
public class PasswordModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public required string Title { get; set; }

    [Column("email_username")]
    public string? Email_Username { get; set; }

    [Column("password")]
    public required string Password { get; set; }

    [Column("folder_id")]
    public int? FolderId { get; set; }

    [Column("user_id")]
    public required int UserId { get; set; }

    [Column("last_updated")]
    public DateTime LastUpdated { get; set; }

    [Column("deleted")]
    public bool Deleted { get; set; }


}
