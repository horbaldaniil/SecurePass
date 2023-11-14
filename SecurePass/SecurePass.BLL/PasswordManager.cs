using SecurePass.DAL.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SecurePass.BLL
{
    public class PasswordManager
    {
        private readonly UserModel currentUser;

        public PasswordManager(UserModel currentUser)
        {
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public List<PasswordModel> GetPasswords()
        {
            using (var db = new SecurePassDbContext())
            {
                return db.Passwords.Where(f => f.UserId == currentUser.Id).ToList();
            }
        }

        public void SendPasswordToTrash(PasswordModel password)
        {
            using (var db = new SecurePassDbContext())
            {
                var existingPassword = db.Passwords.Find(password.Id);
                existingPassword.Deleted = true;
                
                db.SaveChanges();
            }
        }

        public void RestorePassword(PasswordModel password)
        {
            using (var db = new SecurePassDbContext())
            {
                var existingPassword = db.Passwords.Find(password.Id);
                existingPassword.Deleted = false;

                db.SaveChanges();
            }
        }

        public void DeletePassword(PasswordModel password)
        {
            using (var db = new SecurePassDbContext())
            {
                db.Passwords.Remove(password);
                

                db.SaveChanges();
            }
        }

        public void ChangePassword(PasswordModel password)
        {
            using (var db = new SecurePassDbContext())
            {
                var existingPassword = db.Passwords.Find(password.Id);

                existingPassword.Title = password.Title;
                existingPassword.Email_Username = password.Email_Username;
                existingPassword.Password = password.Password;
                existingPassword.LastUpdated = DateTime.UtcNow;

                if (password.FolderId.HasValue)
                    existingPassword.FolderId = password.FolderId.Value;
                else
                    existingPassword.FolderId = null;
                
                
                db.SaveChanges();
            }
        }

        public void SavePassword(string passwordTitle, string password, string? emailOrUsername, int? folderId)
        {
            using (var db = new SecurePassDbContext())
            {
                var newPassword = new PasswordModel
                {
                    Title = passwordTitle,
                    Password = password,
                    UserId = currentUser.Id,
                    Email_Username = emailOrUsername,
                    FolderId = folderId,
                    LastUpdated = DateTime.UtcNow,
                };

                db.Passwords.Add(newPassword);

                db.SaveChanges();
            }
        }
    }
}
