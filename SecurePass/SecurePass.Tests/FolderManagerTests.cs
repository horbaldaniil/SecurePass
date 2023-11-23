using SecurePass.BLL;
using SecurePass.DAL.Model;

namespace SecurePass.Tests
{
    [TestFixture]
    public class FolderManagerTests
    {
        private UserModel CreateUser() => new UserModel { Id = 86, Email = "Test@gmail.com", Password = "Test123456!" };

        [Test]
        public void GetUserFolders_ReturnsUserFolders()
        {
            // Arrange
            var user = CreateUser();
            var folderManager = new FolderManager(user);

            // Act
            var folders = folderManager.GetUserFolders();

            // Assert
            Assert.That(folders.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddNewFolder_NewFolder_SuccessfullyAdded()
        {
            // Arrange
            var user = CreateUser();
            var folderManager = new FolderManager(user);
            var folderName = "NewFolder";

            // Act
            var result = folderManager.AddNewFolder(folderName);

            // Assert
            Assert.IsTrue(result);

            // Additional check to verify the folder was actually added
            var folders = folderManager.GetUserFolders();
            Assert.That(folders.Count, Is.EqualTo(1));
            Assert.That(folders[0].Title, Is.EqualTo(folderName));

            // cleanup
            folderManager.DeleteFolder(folders[0]);
        }

        [Test]
        public void ChangeFolder_ExistingFolderName_ChangesSuccessfully()
        {
            // Arrange
            var user = CreateUser();
            var folderManager = new FolderManager(user);

            // Add a folder to change its name
            var originalFolderName = "bobik";
            folderManager.AddNewFolder(originalFolderName);

            var folderId = folderManager.GetUserFolders().Where(f => f.Title ==  originalFolderName).Single().Id;
            var newFolderName = "NewFolderName";

            // Act
            var result = folderManager.ChangeFolder(folderId, newFolderName);

            // Assert
            Assert.IsTrue(result);

            // Additional check to verify the folder name was actually changed
            var folders = folderManager.GetUserFolders();
            Assert.That(folders[0].Title, Is.EqualTo(newFolderName));

            // cleanup
            folderManager.DeleteFolder(folders[0]);
        }

        [Test]
        public void DeleteFolder_ExistingFolder_SuccessfullyDeleted()
        {
            // Arrange
            var user = CreateUser();
            var folderManager = new FolderManager(user);

            // Add a folder to delete
            var folderNameToDelete = "FolderToDelete";
            folderManager.AddNewFolder(folderNameToDelete);

            var folderToDelete = folderManager.GetUserFolders().First();

            // Act
            var result = folderManager.DeleteFolder(folderToDelete);

            // Assert
            Assert.IsTrue(result);

            // Additional check to verify the folder was actually deleted
            var folders = folderManager.GetUserFolders();
            Assert.That(folders.Count, Is.EqualTo(0));
        }
    }
}
