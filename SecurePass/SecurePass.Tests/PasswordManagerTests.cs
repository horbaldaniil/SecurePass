using SecurePass.BLL;
using SecurePass.DAL.Model;
using System.Diagnostics;

namespace SecurePass.Tests
{
    [TestFixture]
    public class PasswordManagerTests
    {
        private UserModel CreateUser() => new UserModel { Id = 86, Email = "Test@gmail.com", Password = "Test123456!" };

        [Test]
        public void GetPasswords_ReturnsListOfPasswords()
        {
            // Arrange
            var user = CreateUser();
            var passwordManager = new PasswordManager(user);

            // Act
            List<PasswordModel> passwords = passwordManager.GetPasswords();

            // Assert
            Assert.That(passwords.Count, Is.EqualTo(0));
        }

        [Test]
        public void SendPasswordToTrash_MarksPasswordAsDeleted()
        {
            // Arrange
            var db = new SecurePassDbContext();
            var user = CreateUser();
            var passwordManager = new PasswordManager(user);
            passwordManager.SavePassword("Password", "Password1234!", null, null);
            var password = db.Passwords.Where(p => p.Title == "Password").First();

            // Act
            passwordManager.SendPasswordToTrash(password);
            // Assert
            // Verify that the password is marked as deleted
            var db1 = new SecurePassDbContext();
            var updatedPassword = db1.Passwords.Where(p => p.Title == "Password").First();

            Assert.IsNotNull(updatedPassword);
            Assert.That(updatedPassword.Deleted, Is.EqualTo(true));

            // cleanup
            passwordManager.DeletePassword(updatedPassword);
        }

        [Test]
        public void RestorePassword_RestoresPassword()
        {
            // Arrange
            var user = CreateUser();
            var passwordManager = new PasswordManager(user);
            passwordManager.SavePassword("Password", "Password1234!", null, null);
            var database = new SecurePassDbContext();
            var password = database.Passwords.Where(p => p.Title == "Password").First();
            passwordManager.SendPasswordToTrash(password);

            // Act
            passwordManager.RestorePassword(password);

            // Assert
            // Verify that the password is restored (Deleted is set to false)
            using (var db = new SecurePassDbContext())
            {
                var updatedPassword = db.Passwords.Find(password.Id);

                Assert.IsNotNull(updatedPassword);
                Assert.IsFalse(updatedPassword.Deleted);
            }

            // cleanup
            passwordManager.DeletePassword(password);
        }

        [Test]
        public void ChangePassword_UpdatesPasswordProperties()
        {
            // Arrange
            var user = CreateUser();
            var passwordManager = new PasswordManager(user);
            var password = new PasswordModel
            {
                UserId = user.Id,
                Title = "OldTitle",
                Email_Username = "OldEmail",
                Password = "OldPassword",
                LastUpdated = DateTime.UtcNow
            };

            // Save the original password to the database
            passwordManager.SavePassword(password.Title, password.Password, password.Email_Username, null);

            // Retrieve the password with the assigned Id
            var newPassword = passwordManager.GetPasswords().FirstOrDefault(p => p.Title == password.Title);

            // Update the properties of newPassword
            newPassword.Title = "NewTitle";
            newPassword.Email_Username = "NewEmail";
            newPassword.Password = "NewPassword";
            newPassword.LastUpdated = DateTime.UtcNow;

            // Act
            passwordManager.ChangePassword(newPassword);

            // Assert
            // Verify that the password properties are updated
            List<PasswordModel> passwords = passwordManager.GetPasswords();
            PasswordModel updatedPassword = passwords.FirstOrDefault(p => p.Id == newPassword.Id);

            Assert.IsNotNull(updatedPassword);
            Assert.AreEqual(newPassword.Title, updatedPassword.Title);
            Assert.AreEqual(newPassword.Email_Username, updatedPassword.Email_Username);
            Assert.AreEqual(newPassword.Password, updatedPassword.Password);

            // cleanup
            passwordManager.DeletePassword(newPassword);
        }

        [Test]
        public void SavePassword_AddsNewPasswordToDatabase()
        {
            // Arrange
            var user = CreateUser();
            var passwordManager = new PasswordManager(user);

            string passwordTitle = "NewPasswordTitle";
            string passwordValue = "NewPasswordValue";
            string emailOrUsername = "NewEmailOrUsername";
            int? folderId = null;

            // Act
            passwordManager.SavePassword(passwordTitle, passwordValue, emailOrUsername, folderId);

            // Assert
            // Verify that the new password is added to the database
            List<PasswordModel> passwords = passwordManager.GetPasswords();
            PasswordModel newPassword = passwords.FirstOrDefault(p => p.Title == passwordTitle);

            Assert.IsNotNull(newPassword);
            Assert.AreEqual(passwordTitle, newPassword.Title);
            Assert.AreEqual(passwordValue, newPassword.Password);
            Assert.AreEqual(emailOrUsername, newPassword.Email_Username);
            Assert.IsNull(newPassword.FolderId);

            // Act + cleanup
            passwordManager.SendPasswordToTrash(newPassword);
            passwordManager.DeletePassword(newPassword);

            // Assert
            // Verify that the password is removed from the database (e.g., check using GetPasswords)
            List<PasswordModel> newpasswords = passwordManager.GetPasswords();
            Assert.IsFalse(newpasswords.Any(p => p.Id == newPassword.Id));
        }
    }
}
