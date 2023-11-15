using SecurePass.DAL.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

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
                var encryptedPasswords = db.Passwords.Where(f => f.UserId == currentUser.Id).ToList();

                // Розшифрування кожного пароля
                var decryptedPasswords = new List<PasswordModel>();
                foreach (var encryptedPassword in encryptedPasswords)
                {
                    // Перетворення рядка у масив байтів
                    byte[] encryptedPasswordBytes = Convert.FromBase64String(encryptedPassword.Password);

                    // Розшифрування
                    byte[] decryptedPasswordBytes = ProtectedData.Unprotect(encryptedPasswordBytes, null, DataProtectionScope.CurrentUser);
                    string decryptedPassword = Encoding.UTF8.GetString(decryptedPasswordBytes);

                    var decryptedPasswordModel = new PasswordModel
                    {
                        Id = encryptedPassword.Id,
                        Title = encryptedPassword.Title,
                        Password = decryptedPassword,
                        UserId = encryptedPassword.UserId,
                        Email_Username = encryptedPassword.Email_Username,
                        FolderId = encryptedPassword.FolderId,
                        LastUpdated = encryptedPassword.LastUpdated,
                        Deleted = encryptedPassword.Deleted,
                    };

                    decryptedPasswords.Add(decryptedPasswordModel);
                }

                return decryptedPasswords;
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

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password.Password);

                byte[] encryptedPassword = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);

                existingPassword.Title = password.Title;
                existingPassword.Email_Username = password.Email_Username;
                existingPassword.Password = Convert.ToBase64String(encryptedPassword);
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

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] encryptedPassword = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);

                var newPassword = new PasswordModel
                {
                    Title = passwordTitle,
                    Password = Convert.ToBase64String(encryptedPassword),
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
