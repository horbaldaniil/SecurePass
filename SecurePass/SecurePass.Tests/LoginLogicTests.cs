using SecurePass.BLL;
using System.Diagnostics;

namespace SecurePass.Tests
{
    [TestFixture]
    public class LoginLogicTests
    {
        [Test]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string validEmail = "test@example.com";

            // Act
            bool isValid = LoginLogic.IsValidEmail(validEmail);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            string invalidEmail = "invalid-email";

            // Act
            bool isValid = LoginLogic.IsValidEmail(invalidEmail);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string password = "password";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Act
            bool isVerified = LoginLogic.VerifyPassword(password, hashedPassword);

            // Assert
            Assert.IsTrue(isVerified);
        }

        [Test]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            string correctPassword = "correct-password";
            string incorrectPassword = "incorrect-password";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

            // Act
            bool isVerified = LoginLogic.VerifyPassword(incorrectPassword, hashedPassword);

            // Assert
            Assert.IsFalse(isVerified);
        }

        [Test]
        public async Task VerifyUserAsync_ReturnTrue()
        {
            // Arrange
            string email = "Test@gmail.com";
            string password = "Test123456!";

            // Act
            var result = await LoginLogic.VerifyUserAsync(email, password);

            // Assert
            Assert.IsTrue(result == null);
        }

        [Test]
        public async Task VerifyUser_ReturnFalse()
        {
            // Arrange
            string email = "Test@gmail.com";
            string password = "Test123456!!";

            // Act
            var result = await LoginLogic.VerifyUserAsync(email, password);

            // Assert
            Assert.IsFalse(result == null);
        }
    }
}