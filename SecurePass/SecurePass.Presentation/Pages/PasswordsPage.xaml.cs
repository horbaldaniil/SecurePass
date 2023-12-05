// <copyright file="PasswordsPage.xaml.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation.Pages
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Navigation;
    using MaterialDesignThemes.Wpf;
    using SecurePass.BLL;
    using SecurePass.DAL.Model;
    using SecurePass.Presentation.ViewModel;

    /// <summary>
    /// Interaction logic for PasswordsPage.xaml.
    /// </summary>
    public partial class PasswordsPage : Page
    {
        private readonly PasswordManager passwordManager;
        private readonly int? folderId;
        private readonly UserModel currentUser = CurrentUserManager.CurrentUser;
        private readonly SnackbarMessageQueue snackbarMessageQueue = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordsPage"/> class.
        /// </summary>
        public PasswordsPage()
        {
            InitializeComponent();
            Snackbar.MessageQueue = snackbarMessageQueue;
            passwordManager = new PasswordManager(currentUser);
            GetData();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordsPage"/> class.
        /// </summary>
        /// <param name="folderId">ID of the folder.</param>
        /// <param name="folderTitle">Title of the folder.</param>
        public PasswordsPage(int folderId, string folderTitle)
        {
            InitializeComponent();
            PasswordsPageLabel.Content = $"📁 {folderTitle}";
            Snackbar.MessageQueue = snackbarMessageQueue;
            passwordManager = new PasswordManager(currentUser);
            this.folderId = folderId;
            GetData(folderId);
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

        /// <summary>
        /// Retrieves data from the database and updates it on the user page.
        /// </summary>
        /// <param name="folderId">ID of the folder.</param>
        public void GetData(int? folderId = null)
        {
            ObservableCollection<PasswordViewModel> passwordViewModels;
            if (folderId == null)
            {
                var passwords = passwordManager.GetPasswords()
                    .Where(p => p.Deleted == false)
                    .OrderByDescending(p => p.LastUpdated)
                    .ToList();
                passwordViewModels = new ObservableCollection<PasswordViewModel>(
                passwords.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false }));
            }
            else
            {
                var passwords = passwordManager.GetPasswords()
                    .Where(p => p.Deleted == false && p.FolderId == folderId)
                    .OrderByDescending(p => p.LastUpdated)
                    .ToList();

                passwordViewModels = new ObservableCollection<PasswordViewModel>(
                passwords.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false }));
            }

            DataBinding.ItemsSource = passwordViewModels;
        }

        private void AddNewPassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            if (navigationService != null)
            {
                var createPasswordPage = new CreatePasswordPage();
                if (folderId == null)
                {
                    createPasswordPage.OnPasswordCreated += PasswordCreatedHandler;
                }
                else
                {
                    createPasswordPage.OnPasswordCreated += PasswordCreatedFolderHandler;
                }

                navigationService.Navigate(createPasswordPage);
            }
        }

        private void PasswordCreatedHandler(object? sender, EventArgs e)
        {
            GetData();
        }

        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            PasswordViewModel passwordViewModel = (PasswordViewModel)deleteButton.DataContext;

            if (passwordViewModel != null)
            {
                passwordManager.SendPasswordToTrash(passwordViewModel.Password);
                string passwordToTrash = (string)Application.Current.FindResource("PasswordToTrash");
                ShowSnackbar(passwordToTrash);
                GetData();
            }
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

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Button copyButton = (Button)sender;
            PasswordViewModel passwordViewModel = (PasswordViewModel)copyButton.DataContext;

            if (passwordViewModel != null)
            {
                Clipboard.SetText(passwordViewModel.Password.Password);
                string passwordCopied = (string)Application.Current.FindResource("PasswordCopied");
                ShowSnackbar(passwordCopied);
            }
        }

        private void ShowSnackbar(string message)
        {
            Snackbar.MessageQueue.Enqueue(message);
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

                if (folderId == null)
                {
                    createPasswordPage.OnPasswordCreated += PasswordCreatedHandler;
                }
                else
                {
                    createPasswordPage.OnPasswordCreated += PasswordCreatedFolderHandler;
                }

                NavigationService.Navigate(createPasswordPage);
            }
        }

        private void PasswordCreatedFolderHandler(object? sender, EventArgs e)
        {
            GetData(folderId);
        }
    }
}