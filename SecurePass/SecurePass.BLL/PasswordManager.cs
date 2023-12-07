// <copyright file="PasswordManager.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using log4net;
    using Microsoft.EntityFrameworkCore;
    using SecurePass.DAL.Model;

    /// <summary>
    /// The <c>PasswordManager</c> class provides methods for managing passwords associated with a user,
    /// including retrieval, analysis, and modification based on various criteria.
    /// </summary>
    public class PasswordManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly UserModel currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordManager"/> class.
        /// </summary>
        /// <param name="currentUser">The current user for whom passwords are managed.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="currentUser"/> is null.</exception>
        public PasswordManager(UserModel currentUser)
        {
            this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        /// <summary>
        /// Enumeration representing different categories of passwords.
        /// </summary>
        public enum PasswordCategory
        {
            /// <summary>
            /// Weak password.
            /// </summary>
            Weak,

            /// <summary>
            /// Reused password.
            /// </summary>
            Reused,

            /// <summary>
            /// Old password.
            /// </summary>
            Old,
        }

        /// <summary>
        /// Retrieves decrypted passwords associated with the current user from the database.
        /// </summary>
        /// <returns>A list of decrypted passwords.</returns>
        public List<PasswordModel> GetPasswords()
        {
            using var db = new SecurePassDbContext();
            var encryptedPasswords = db.Passwords.Where(f => f.UserId == this.currentUser.Id).ToList();

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

        /// <summary>
        /// Retrieves weak passwords (not meeting strength criteria) associated with the current user.
        /// </summary>
        /// <returns>A list of weak passwords.</returns>
        public List<PasswordModel> GetWeakPasswords()
        {
            List<PasswordModel> passwords = this.GetPasswords();
            List<PasswordModel> weakPasswords = new ();

            foreach (var password in passwords)
            {
                if (!this.IsStrongPassword(password) && !password.Deleted)
                {
                    weakPasswords.Add(password);
                }
            }

            return weakPasswords;
        }

        /// <summary>
        /// Retrieves duplicate passwords associated with the current user.
        /// </summary>
        /// <returns>A list of duplicate passwords.</returns>
        public List<PasswordModel> GetDuplicatePasswords()
        {
            List<PasswordModel> passwords = this.GetPasswords();

            var duplicatePasswords = passwords
                .Where(p => !p.Deleted)
                .GroupBy(p => p.Password)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            return duplicatePasswords;
        }

        /// <summary>
        /// Retrieves old passwords (not updated within the last 90 days) associated with the current user.
        /// </summary>
        /// <returns>A list of old passwords.</returns>
        public List<PasswordModel> GetOldPasswords()
        {
            List<PasswordModel> passwords = this.GetPasswords();

            var oldPasswords = passwords
                .Where(p => (DateTime.UtcNow - p.LastUpdated).TotalDays > 90)
                .ToList();

            return oldPasswords;
        }

        /// <summary>
        /// Determines if a password is considered strong based on specific criteria.
        /// </summary>
        /// <param name="password">The password to evaluate.</param>
        /// <returns>True if the password is considered strong; otherwise, false.</returns>
        public bool IsStrongPassword(PasswordModel password)
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

        /// <summary>
        /// Moves a password to the trash by marking it as deleted in the database.
        /// </summary>
        /// <param name="password">The password to send to the trash.</param>
        public void SendPasswordToTrash(PasswordModel password)
        {
            using var db = new SecurePassDbContext();
            var existingPassword = db.Passwords.Single(p => p.Id == password.Id);
            existingPassword.Deleted = true;
            db.SaveChanges();

            log.Info($"Password with ID {password.Id} sent to trash by user {this.currentUser.Email}.");
        }

        /// <summary>
        /// Restores a previously deleted password by updating its deleted status in the database.
        /// </summary>
        /// <param name="password">The password to restore.</param>
        public void RestorePassword(PasswordModel password)
        {
            using var db = new SecurePassDbContext();
            var existingPassword = db.Passwords.Single(p => p.Id == password.Id);
            existingPassword.Deleted = false;

            db.SaveChanges();

            log.Info($"Password with ID {password.Id} restored by user {this.currentUser.Email}.");
        }

        /// <summary>
        /// Deletes a password from the database.
        /// </summary>
        /// <param name="password">The password to delete.</param>
        public void DeletePassword(PasswordModel password)
        {
            using var db = new SecurePassDbContext();
            db.Passwords.Remove(password);
            db.SaveChanges();

            log.Info($"Password with ID {password.Id} permanently deleted by user {this.currentUser.Email}.");
        }

        /// <summary>
        /// Updates the information of an existing password in the database.
        /// </summary>
        /// <param name="password">The updated password information.</param>
        public void ChangePassword(PasswordModel password)
        {
            using var db = new SecurePassDbContext();
            var existingPassword = db.Passwords.Single(p => p.Id == password.Id);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password.Password);

            byte[] encryptedPassword = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);

            existingPassword.Title = password.Title;
            existingPassword.Email_Username = string.IsNullOrWhiteSpace(password.Email_Username) ? null : password.Email_Username;
            existingPassword.Password = Convert.ToBase64String(encryptedPassword);
            existingPassword.LastUpdated = DateTime.UtcNow;

            if (password.FolderId.HasValue)
            {
                existingPassword.FolderId = password.FolderId.Value;
            }
            else
            {
                existingPassword.FolderId = null;
            }

            db.SaveChanges();

            log.Info($"Password with ID {password.Id} updated by user {this.currentUser.Email}.");
        }

        /// <summary>
        /// Saves a new password to the database.
        /// </summary>
        /// <param name="passwordTitle">The title of the new password.</param>
        /// <param name="password">The actual password to be saved.</param>
        /// <param name="emailOrUsername">The associated email or username (optional).</param>
        /// <param name="folderId">The folder ID where the password belongs (optional).</param>
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
                    UserId = this.currentUser.Id,
                    Email_Username = string.IsNullOrWhiteSpace(emailOrUsername) ? null : emailOrUsername,
                    FolderId = folderId,
                    LastUpdated = DateTime.UtcNow,
                };

                db.Passwords.Add(newPassword);

                db.SaveChanges();

                log.Info($"New password added for user {this.currentUser.Email}.");
            }
        }
    }
}
