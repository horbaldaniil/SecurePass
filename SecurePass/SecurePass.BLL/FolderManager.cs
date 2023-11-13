using Microsoft.EntityFrameworkCore;
using SecurePass.DAL.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurePass.BLL
{
    public class FolderManager
    {
        private readonly UserModel currentUser;
        public FolderManager(UserModel currentUser)
        {
            this.currentUser = currentUser ?? throw new System.ArgumentNullException(nameof(currentUser));
        }
        public List<FolderModel> GetUserFolders()
        {
            using (var db = new SecurePassDbContext())
            {
                return db.Folders.Where(f => f.UserId == currentUser.Id).ToList();
            }
        }
        public bool AddNewFolder(string folderName)
        {
            using (var db = new SecurePassDbContext())
            {
                var existingFolder = db.Folders.FirstOrDefault(f => f.UserId == currentUser.Id && f.Title == folderName);

                if (existingFolder == null)
                {
                    var newFolder = new FolderModel
                    {
                        Title = folderName,
                        UserId = currentUser.Id
                    };

                    db.Folders.Add(newFolder);
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public bool UpdateFolder(int folderId, string newFolderName)
        {
            using (var db = new SecurePassDbContext())
            {
                var folderToUpdate = db.Folders.FirstOrDefault(f => f.Id == folderId);

                if (folderToUpdate != null)
                {
                    folderToUpdate.Title = newFolderName;

                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }
    }
}
