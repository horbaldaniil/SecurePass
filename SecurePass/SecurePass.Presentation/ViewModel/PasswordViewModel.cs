using SecurePass.DAL.Model;

namespace SecurePass.Presentation.ViewModel
{
    public class PasswordViewModel
    {
        public PasswordModel Password { get; set; }
        public bool IsPasswordVisible { get; set; }

        public static PasswordViewModel CreateFromPassword(PasswordModel password)
        {
            return new PasswordViewModel { Password = password, IsPasswordVisible = false };
        }

        public void UpdatePasswordModel(string title, string emailOrUsername, string password, int? folderId)
        {
            this.Password.Title = title;
            this.Password.Email_Username = emailOrUsername;
            this.Password.Password = password;
            this.Password.FolderId = folderId;
        }
    }
}
