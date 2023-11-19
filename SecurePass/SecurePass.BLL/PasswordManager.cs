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

        public enum PasswordCategory
        {
            Weak,
            Reused,
            Old,
        }

        public PasswordManager(UserModel currentUser)
        {
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public List<PasswordModel> GetPasswords()
        {
            using (var db = new SecurePassDbContext())
            {
                var encryptedPasswords = db.Passwords.Where(f => f.UserId == currentUser.Id).ToList();

                var decryptedPasswords = new List<PasswordModel>();
                foreach (var encryptedPassword in encryptedPasswords)
                {
                    byte[] encryptedPasswordBytes = Convert.FromBase64String(encryptedPassword.Password);

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

        public List<PasswordModel> GetWeakPasswords()
        {
            List<PasswordModel> passwords = GetPasswords();
            List<PasswordModel> weakPasswords = new List<PasswordModel>();

            foreach (var password in passwords)
            {
                if (!IsStrongPassword(password) && !password.Deleted)
                {
                    weakPasswords.Add(password);
                }
            }

            return weakPasswords;
        }

        public List<PasswordModel> GetDuplicatePasswords()
        {
            List<PasswordModel> passwords = GetPasswords();

            var duplicatePasswords = passwords
                .Where(p => !p.Deleted)
                .GroupBy(p => p.Password)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            return duplicatePasswords;
        }

        public List<PasswordModel> GetOldPasswords()
        {
            List<PasswordModel> passwords = GetPasswords();

            var oldPasswords = passwords
                .Where(p => (DateTime.UtcNow - p.LastUpdated).TotalDays > 90)
                .ToList();

            return oldPasswords;
        }

        public static bool IsStrongPassword(PasswordModel password)
        {
            if (password.Password.Length < 8)
            {
                return false;
            }

            if (!password.Password.Any(char.IsUpper))
            {
                return false;
            }

            if (!password.Password.Any(char.IsLower))
            {
                return false;
            }

            if (!password.Password.Any(char.IsDigit))
            {
                return false;
            }

            string specialCharacters = @"!@#$%^&*()-_=+[]{}|;:'<>,.?/";
            if (!password.Password.Any(c => specialCharacters.Contains(c)))
            {
                return false;
            }

            return true;
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
                existingPassword.Email_Username = string.IsNullOrWhiteSpace(password.Email_Username) ? null : password.Email_Username;
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
                    Email_Username = string.IsNullOrWhiteSpace(emailOrUsername) ? null : emailOrUsername,
                    FolderId = folderId,
                    LastUpdated = DateTime.UtcNow,
                };

                db.Passwords.Add(newPassword);

                db.SaveChanges();
                
            }
        }
    }
}
