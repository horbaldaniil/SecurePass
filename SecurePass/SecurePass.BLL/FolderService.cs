using Microsoft.EntityFrameworkCore;
using SecurePass.DAL.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurePass.BLL
{
    public class FolderService
    {
        public static async Task<List<FolderModel>> GetFoldersByUserIdAsync(int userId)
        {
            using (var db = new SecurePassDbContext())
            {
                return await db.Folders.Where(f => f.UserId == userId).ToListAsync();
            }
        }

        public static async Task<bool> AddNewFolderAsync(string folderName, int userId)
        {
            using (var db = new SecurePassDbContext())
            {
                var existingFolder = await db.Folders.FirstOrDefaultAsync(f => f.UserId == userId && f.Title == folderName);

                if (existingFolder == null)
                {
                    var newFolder = new FolderModel
                    {
                        Title = folderName,
                        UserId = userId
                    };

                    db.Folders.Add(newFolder);
                    await db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }

        public static async Task<bool> UpdateFolderAsync(int folderId, string newFolderName)
        {
            using (var db = new SecurePassDbContext())
            {
                var folderToUpdate = await db.Folders.FirstOrDefaultAsync(f => f.Id == folderId);

                if (folderToUpdate != null)
                {
                    folderToUpdate.Title = newFolderName;

                    await db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }
    }
}
