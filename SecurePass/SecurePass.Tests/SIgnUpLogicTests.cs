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
            var signUpLogic = new SignUpLogic();
            string validEmail = "test@example.com";

            // Act
            bool isValid = signUpLogic.IsValidEmail(validEmail);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var signUpLogic = new SignUpLogic();
            string invalidEmail = "invalid-email";

            // Act
            bool isValid = signUpLogic.IsValidEmail(invalidEmail);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            // Arrange
            var signUpLogic = new SignUpLogic();
            string validPassword = "test1test!testA";

            // Act
            bool isValid = signUpLogic.IsValidPassword(validPassword);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidPassword_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var signUpLogic = new SignUpLogic();
            string invalidPassword = "weakpassword";

            // Act
            bool isValid = signUpLogic.IsValidPassword(invalidPassword);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public async Task UserRegistration_NewUser_ReturnsNull()
        {
            // Arrange
            var signUpLogic = new SignUpLogic();
            string email = "newuser@example.com";
            string password = "Test123!";

            // Act
            string result = await signUpLogic.UserRegistration(email, password);

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
            var signUpLogic = new SignUpLogic();
            string existingEmail = "existinguser@example.com";
            string password = "Test123!";

            // Act
            string result = await signUpLogic.UserRegistration(existingEmail, password);

            // Assert
            Assert.AreEqual("InUseEmail", result);
        }

        [Test]
        public void HashPassword_ValidPassword_ReturnsHashedPassword()
        {
            // Arrange
            var signUpLogic = new SignUpLogic();
            string password = "Test123!";

            // Act
            string hashedPassword = signUpLogic.HashPassword(password);

            // Assert
            Assert.IsNotNull(hashedPassword);
            Assert.AreNotEqual(password, hashedPassword);
        }
    }
}
