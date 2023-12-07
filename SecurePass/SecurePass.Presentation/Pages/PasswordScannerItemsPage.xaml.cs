// <copyright file="PasswordScannerItemsPage.xaml.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Navigation;
    using log4net;
    using SecurePass.BLL;
    using SecurePass.DAL.Model;
    using SecurePass.Presentation.ViewModel;

    /// <summary>
    /// Interaction logic for PasswordScannerItemsPage.xaml.
    /// </summary>
    public partial class PasswordScannerItemsPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        private async void LoadingAnimation()
        {
            var items = DataBinding.Items;

            foreach (var item in items)
            {
                var container = DataBinding.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (container != null)
                {
                    container.Opacity = 0;
                }
            }

            foreach (var item in items)
            {
                var container = DataBinding.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (container != null)
                {
                    var animation = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    };

                    container.BeginAnimation(UIElement.OpacityProperty, animation);
                }

                await Task.Delay(100);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingAnimation();
        }

        private void GetData()
        {
            var passwords = new List<PasswordModel>();
            switch (passwordCategory)
            {
                case PasswordManager.PasswordCategory.Weak:
                    passwords = passwordManager.GetWeakPasswords();
                    PasswordScannerItemsTitle.SetResourceReference(ContentProperty, "WeakPasswordsStr");
                    log.Info("Received information about weak passwords.");
                    break;
                case PasswordManager.PasswordCategory.Reused:
                    passwords = passwordManager.GetDuplicatePasswords();
                    PasswordScannerItemsTitle.SetResourceReference(ContentProperty, "ReusedPasswordsStr");
                    log.Info("Received information about reused passwords.");
                    break;
                case PasswordManager.PasswordCategory.Old:
                    passwords = passwordManager.GetOldPasswords();
                    PasswordScannerItemsTitle.SetResourceReference(ContentProperty, "OldPasswordsStr");
                    log.Info("Received information about old passwords.");
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
