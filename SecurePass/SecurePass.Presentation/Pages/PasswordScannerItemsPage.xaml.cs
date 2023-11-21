using SecurePass.BLL;
using SecurePass.DAL.Model;
using SecurePass.Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace SecurePass.Presentation.Pages
{
    /// <summary>
    /// Interaction logic for PasswordScannerItemsPage.xaml
    /// </summary>
    public partial class PasswordScannerItemsPage : Page
    {

        private readonly PasswordManager.PasswordCategory PasswordCategory;
        private readonly PasswordManager passwordManager;
        private readonly UserModel currentUser = CurrentUserManager.CurrentUser;


        public PasswordScannerItemsPage(PasswordManager.PasswordCategory pCategory)
        {
            InitializeComponent();
            PasswordCategory = pCategory;
            passwordManager = new PasswordManager(currentUser);
            GetData();
        }

        private void GetData()
        {
            var passwords = new List<PasswordModel>();
            switch (PasswordCategory)
            {
                case PasswordManager.PasswordCategory.Weak:
                    passwords = passwordManager.GetWeakPasswords();
                    PasswordScannerItemsTitle.SetResourceReference(ContentProperty, "WeakPasswordsStr");
                    break;
                case PasswordManager.PasswordCategory.Reused:
                    passwords = passwordManager.GetDuplicatePasswords();
                    PasswordScannerItemsTitle.SetResourceReference(ContentProperty, "ReusedPasswordsStr");
                    break;
                case PasswordManager.PasswordCategory.Old:
                    passwords = passwordManager.GetOldPasswords();
                    PasswordScannerItemsTitle.SetResourceReference(ContentProperty, "OldPasswordsStr");
                    break;
            }
            var passwordViewModels = new ObservableCollection<PasswordViewModel>(
            passwords.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false })
            );
            DataBinding.ItemsSource = passwordViewModels;
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            Button showButton = (Button)sender;
            PasswordViewModel passwordViewModel = (PasswordViewModel)showButton.DataContext;

            if (passwordViewModel != null)
            {
                passwordViewModel.IsPasswordVisible = !passwordViewModel.IsPasswordVisible;

                DataBinding.Items.Refresh();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            Button changeButton = (Button)sender;
            PasswordViewModel passwordViewModel = (PasswordViewModel)changeButton.DataContext;

            if (passwordViewModel != null)
            {
                var createPasswordPage = new CreatePasswordPage
                {
                    IsEditMode = true,
                    DataContext = passwordViewModel,
                };

                createPasswordPage.OnPasswordCreated += onUpdate;

                NavigationService.Navigate(createPasswordPage);
            }
        }

        private void onUpdate(object? sender, EventArgs e)
        {
            GetData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
