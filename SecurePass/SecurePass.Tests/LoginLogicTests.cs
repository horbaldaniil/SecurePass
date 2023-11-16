using SecurePass.BLL;

namespace SecurePass.Tests
{
    [TestFixture]
    public class LoginLogicTests
    {
        [Test]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            var loginLogic = new LoginLogic();
            string validEmail = "test@example.com";

            // Act
            bool isValid = loginLogic.IsValidEmail(validEmail);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var loginLogic = new LoginLogic();
            string invalidEmail = "invalid-email";

            // Act
            bool isValid = loginLogic.IsValidEmail(invalidEmail);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void VerifyPasswordAsync_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var loginLogic = new LoginLogic();
            string password = "password";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Act
            bool isVerified = loginLogic.VerifyPasswordAsync(password, hashedPassword).Result;

            // Assert
            Assert.IsTrue(isVerified);
        }

        [Test]
        public void VerifyPasswordAsync_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var loginLogic = new LoginLogic();
            string correctPassword = "correct-password";
            string incorrectPassword = "incorrect-password";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

            // Act
            bool isVerified = loginLogic.VerifyPasswordAsync(incorrectPassword, hashedPassword).Result;

            // Assert
            Assert.IsFalse(isVerified);
        }

        // Add more tests as needed for the VerifyUser method
    }
}