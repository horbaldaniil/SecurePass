using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePass.DAL.Model
{
    public class PasswordModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Email_Username {  get; set; }
        public required string Password {  get; set; }
        public int? FolderId { get; set; }
        public required int UserId { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Deleted {  get; set; }

    }
}
