using Microsoft.EntityFrameworkCore;
using SecurePass.BLL;
using SecurePass.DAL.Model;

namespace SecurePass.Tests
{
    [TestFixture]
    public class SignUpLogicTests
    {
        [Test]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string validEmail = "test@example.com";

            // Act
            bool isValid = SignUpLogic.IsValidEmail(validEmail);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            string invalidEmail = "invalid-email";

            // Act
            bool isValid = SignUpLogic.IsValidEmail(invalidEmail);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            // Arrange
            string validPassword = "test1test!testA";

            // Act
            bool isValid = SignUpLogic.IsValidPassword(validPassword);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidPassword_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            string invalidPassword = "weakpassword";

            // Act
            bool isValid = SignUpLogic.IsValidPassword(invalidPassword);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public async Task UserRegistration_NewUser_ReturnsNull()
        {
            // Arrange
            string email = "newuser@example.com";
            string password = "Test123!";

            // Act
            string result = await SignUpLogic.UserRegistration(email, password);

            // Assert
            Assert.IsNull(result);

            // cleanup
            using (var db = new SecurePassDbContext())
            {
                var newUser = await db.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (newUser != null)
                {
                    db.Users.Remove(newUser);
                    await db.SaveChangesAsync();
                }
            }
        }

        [Test]
        public async Task UserRegistration_ExistingUser_ReturnsInUseEmail()
        {
            // Arrange
            string existingEmail = "existinguser@example.com";
            string password = "Test123!";

            // Act
            string result = await SignUpLogic.UserRegistration(existingEmail, password);

            // Assert
            Assert.AreEqual("InUseEmail", result);
        }

        [Test]
        public void HashPassword_ValidPassword_ReturnsHashedPassword()
        {
            // Arrange
            string password = "Test123!";

            // Act
            string hashedPassword = SignUpLogic.HashPassword(password);

            // Assert
            Assert.IsNotNull(hashedPassword);
            Assert.AreNotEqual(password, hashedPassword);
        }
    }
}
