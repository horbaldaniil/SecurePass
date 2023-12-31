﻿// <copyright file="TrashPage.xaml.cs" company="SecurePass">
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
    using log4net;
    using SecurePass.BLL;
    using SecurePass.DAL.Model;
    using SecurePass.Presentation.ViewModel;

    /// <summary>
    /// Interaction logic for TrashPage.xaml.
    /// </summary>
    public partial class TrashPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly PasswordManager passwordManager;
        private readonly UserModel? currentUser = CurrentUserManager.CurrentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrashPage"/> class.
        /// </summary>
        public TrashPage()
        {
            InitializeComponent();
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

        /// <summary>
        /// Retrieves data from the database and updates it on the user page.
        /// </summary>
        public void GetData()
        {
            var passwordItems = passwordManager.GetPasswords().FindAll(f => f.Deleted == true);

            var passwordViewModels = new ObservableCollection<PasswordViewModel>(
                                    passwordItems.Select(PasswordViewModel.CreateFromPassword));

            TrashEmpty.Visibility = passwordItems.Any() ? Visibility.Collapsed : Visibility.Visible;
            TrashLabel.Visibility = passwordItems.Any() ? Visibility.Visible : Visibility.Collapsed;

            log.Info($"Received a list of deleted passwords from the database. User : {CurrentUserManager.CurrentUser.Email}.");

            DataBinding.ItemsSource = passwordViewModels;
        }

        private void EmptyTrash_Click(object sender, RoutedEventArgs e)
        {
            var passwordItems = passwordManager.GetPasswords().FindAll(f => f.Deleted == true);
            foreach (var password in passwordItems)
            {
                passwordManager.DeletePassword(password);
            }

            log.Info($"The trash is cleaned. User : {CurrentUserManager.CurrentUser.Email}.");

            GetData();
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            Button showButton = (Button)sender;
            PasswordViewModel passwordViewModel = (PasswordViewModel)showButton.DataContext;

            if (passwordViewModel != null)
            {
                log.Info($"The password is shown. Password : {passwordViewModel.Password.Id}. User : {CurrentUserManager.CurrentUser.Email}.");

                passwordViewModel.IsPasswordVisible = !passwordViewModel.IsPasswordVisible;

                DataBinding.Items.Refresh();
            }
            else
            {
                log.Error($"The password that the user wanted to be shown does not exist! User : {CurrentUserManager.CurrentUser.Email}.");
            }
        }

        private void HandleDeleteOrRestoreButtonClick(Button button, bool isDelete)
        {
            PasswordViewModel passwordViewModel = (PasswordViewModel)button.DataContext;

            if (passwordViewModel != null)
            {
                if (isDelete)
                {
                    log.Info($"Password deleted. Password : {passwordViewModel.Password.Id}. User : {CurrentUserManager.CurrentUser.Email}.");

                    passwordManager.DeletePassword(passwordViewModel.Password);
                }
                else
                {
                    log.Info($"Password restored. Password : {passwordViewModel.Password.Id}. User : {CurrentUserManager.CurrentUser.Email}.");

                    passwordManager.RestorePassword(passwordViewModel.Password);
                }

                GetData();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDeleteOrRestoreButtonClick((Button)sender, isDelete: true);
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            HandleDeleteOrRestoreButtonClick((Button)sender, isDelete: false);
        }
    }
}
