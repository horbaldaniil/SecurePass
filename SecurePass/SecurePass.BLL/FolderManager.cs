// <copyright file="FolderManager.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.BLL
{
    using System.Collections.Generic;
    using System.Linq;
    using log4net;
    using SecurePass.DAL.Model;

    /// <summary>
    /// Provides control of user folders.
    /// </summary>
    public class FolderManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly UserModel currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderManager"/> class.
        /// </summary>
        /// <param name="currentUser">Current logged in user.</param>
        public FolderManager(UserModel currentUser)
        {
            this.currentUser = currentUser;
        }

        /// <summary>
        /// Returns a list of the user's folders from database.
        /// </summary>
        /// <returns>A <see cref="List{FolderModel}"/> that contains list of user folders.</returns>
        public List<FolderModel> GetUserFolders()
        {
            using var db = new SecurePassDbContext();
            return db.Folders.Where(f => f.UserId == this.currentUser.Id).ToList();
        }

        /// <summary>
        /// Adds a new folder with the specified title for the current user.
        /// </summary>
        /// <param name="folderTitle">The title of the new folder.</param>
        /// <returns>
        /// True if the folder was successfully added; otherwise, false if a folder with the same title already exists
        /// for the current user.
        /// </returns>
        public bool AddNewFolder(string folderTitle)
        {
            using var db = new SecurePassDbContext();
            var existingFolder = db.Folders.FirstOrDefault(f => f.UserId == this.currentUser.Id && f.Title == folderTitle);

            if (existingFolder == null)
            {
                var newFolder = new FolderModel
                {
                    Title = folderTitle,
                    UserId = this.currentUser.Id,
                };
                db.Folders.Add(newFolder);
                db.SaveChanges();

                log.Info($"New folder '{folderTitle}' added for user {this.currentUser.Email}.");

                return true;
            }

            return false;
        }

        /// <summary>
        /// Changes the title of a folder with the specified ID for the current user.
        /// </summary>
        /// <param name="folderId">The ID of the folder to be updated.</param>
        /// <param name="newFolderTitle">The new title for the folder.</param>
        /// <returns>
        /// True if the folder title was successfully updated;
        /// otherwise, false if a folder with the new title already exists for the current user.
        /// </returns>
        public bool ChangeFolder(int folderId, string newFolderTitle)
        {
            using var db = new SecurePassDbContext();
            var existingFolder = db.Folders.FirstOrDefault(f => f.UserId == this.currentUser.Id && f.Title == newFolderTitle);
            if (existingFolder == null || (existingFolder != null && existingFolder.Id == folderId))
            {
                var folderToUpdate = db.Folders.FirstOrDefault(f => f.Id == folderId);

                if (folderToUpdate != null)
                {
                    folderToUpdate.Title = newFolderTitle;

                    db.SaveChanges();

                    log.Info($"Folder ID {folderId} title updated to '{newFolderTitle}' by user {this.currentUser.Email}.");

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Deletes a folder and removes associated passwords from the database.
        /// </summary>
        /// <param name="folder">The folder to be deleted.</param>
        /// <returns>True if the folder was successfully deleted; otherwise, false.</returns>
        public bool DeleteFolder(FolderModel folder)
        {
            using var db = new SecurePassDbContext();
            var passwordsInFolder = db.Passwords.Where(p => p.FolderId == folder.Id).ToList();
            foreach (var password in passwordsInFolder)
            {
                password.FolderId = null;
            }

            db.SaveChanges();
            db.Folders.Remove(folder);

            db.SaveChanges();

            log.Info($"Folder '{folder.Title}' and associated passwords deleted by user {this.currentUser.Email}.");

            return true;
        }
    }
}
