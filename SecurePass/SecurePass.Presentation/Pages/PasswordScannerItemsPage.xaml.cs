// <copyright file="PasswordScannerItemsPage.xaml.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using SecurePass.BLL;
    using SecurePass.DAL.Model;
    using SecurePass.Presentation.ViewModel;

    /// <summary>
    /// Interaction logic for PasswordScannerItemsPage.xaml.
    /// </summary>
    public partial class PasswordScannerItemsPage : Page
    {
        private readonly PasswordManager.PasswordCategory passwordCategory;
        private readonly PasswordManager passwordManager;
        private readonly UserModel currentUser = CurrentUserManager.CurrentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordScannerItemsPage"/> class.
        /// </summary>
        /// <param name="pCategory">Category of the passwords.</param>
        public PasswordScannerItemsPage(PasswordManager.PasswordCategory pCategory)
        {
            InitializeComponent();
            passwordCategory = pCategory;
            passwordManager = new PasswordManager(currentUser);
            GetData();
        }

        private void GetData()
        {
            var passwords = new List<PasswordModel>();
            switch (passwordCategory)
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
            passwords.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false }));
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

                createPasswordPage.OnPasswordCreated += OnUpdate;

                NavigationService.Navigate(createPasswordPage);
            }
        }

        private void OnUpdate(object? sender, EventArgs e)
        {
            GetData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
