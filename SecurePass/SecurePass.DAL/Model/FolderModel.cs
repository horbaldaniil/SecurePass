using System.ComponentModel.DataAnnotations.Schema;

namespace SecurePass.DAL.Model;

[Table("folders")]
public class FolderModel
{
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    public required string Title { get; set; }
    [Column("user_id")]
    public required int UserId { get; set; }
}
