// <copyright file="PasswordScannerPage.xaml.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation.Pages
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Navigation;
    using log4net;
    using SecurePass.BLL;
    using SecurePass.DAL.Model;

    /// <summary>
    /// Interaction logic for PasswordScannerPage.xaml.
    /// </summary>
    public partial class PasswordScannerPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly PasswordManager passwordManager;
        private readonly UserModel currentUser = CurrentUserManager.CurrentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordScannerPage"/> class.
        /// </summary>
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

            log.Info("Received information about weak/reused/old passwords count.");
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
