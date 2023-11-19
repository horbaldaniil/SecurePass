using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using SecurePass.BLL;
using SecurePass.DAL.Model;

namespace SecurePass.Presentation.Pages
{
    /// <summary>
    /// Interaction logic for PasswordScannerPage.xaml
    /// </summary>

    public partial class PasswordScannerPage : Page
    {
        private readonly PasswordManager passwordManager;
        private UserModel currentUser = CurrentUserManager.CurrentUser;
        public PasswordScannerPage()
        {
            InitializeComponent();
            passwordManager = new PasswordManager(currentUser);
            GetData();
        }

        private void GetData()
        {
            WeakPasswordCount.Content = passwordManager.GetWeakPasswords().Count;
            ReusedPasswordCount.Content = passwordManager.GetDuplicatePasswords().Count;
            OldPasswordCount.Content = passwordManager.GetOldPasswords().Count;
        }

        private void NavigateToScannerItemsPage(PasswordManager.PasswordCategory category)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            var passwordScannerItemsPage = new PasswordScannerItemsPage(category);
            navigationService.Navigate(passwordScannerItemsPage);
        }

        private void WeakPasswords_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigateToScannerItemsPage(PasswordManager.PasswordCategory.Weak);
        }

        private void ReusedPasswords_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigateToScannerItemsPage(PasswordManager.PasswordCategory.Reused);
        }

        private void OldPasswords_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigateToScannerItemsPage(PasswordManager.PasswordCategory.Old);
        }
    }
}
