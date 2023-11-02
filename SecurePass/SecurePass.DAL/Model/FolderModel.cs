using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePass.DAL.Model
{
    public class FolderModel
    {
        public int Id { get; set; }
        public required string Title {  get; set; }
        public required int UserId { get; set; }
    }
}
