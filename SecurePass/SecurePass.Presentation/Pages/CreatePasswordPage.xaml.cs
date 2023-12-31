﻿// <copyright file="CreatePasswordPage.xaml.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using log4net;
    using SecurePass.BLL;
    using SecurePass.DAL.Model;
    using SecurePass.Presentation.ViewModel;

    /// <summary>
    /// Interaction logic for CreatePasswordPage.xaml.
    /// </summary>
    public partial class CreatePasswordPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly PasswordManager passwordManager;

        private readonly FolderManager folderManager;

        public bool IsEditMode { get; set; }

        public event EventHandler OnPasswordCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePasswordPage"/> class.
        /// </summary>
        public CreatePasswordPage()
        {
            InitializeComponent();
            this.Loaded += CreatePasswordPage_Loaded;

            UserModel currentUser = CurrentUserManager.CurrentUser;
            passwordManager = new PasswordManager(currentUser);
            folderManager = new FolderManager(currentUser);
        }

        private void CreatePasswordPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFolders();

            IsEditMode = DataContext != null;

            if (IsEditMode)
            {
                var passwordViewModel = (PasswordViewModel)DataContext;

                PasswordTitleTextBox.Text = passwordViewModel?.Password.Title;
                EmailOrUsernameTextBox.Text = passwordViewModel.Password.Email_Username;
                PasswordTextBox.Text = passwordViewModel.Password.Password;

                foreach (ComboBoxItem item in FoldersComboBox.Items)
                {
                    if (item?.Tag is int folderId && folderId == passwordViewModel?.Password.FolderId)
                    {
                        FoldersComboBox.SelectedItem = item;
                        break;
                    }
                }

                UpdateUIBasedOnMode();
            }
        }

        private void UpdateUIBasedOnMode()
        {
            CreatePasswordPageTitle?.SetResourceReference(ContentProperty, "ChangePassword");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string passwordTitle = PasswordTitleTextBox.Text;
            string emailUsername = EmailOrUsernameTextBox.Text;
            string password = PasswordTextBox.Text;
            int? folderId = null;

            if (FoldersComboBox?.SelectedItem is ComboBoxItem selectedFolderItem && selectedFolderItem?.Tag != null)
            {
                folderId = (int)selectedFolderItem.Tag;
            }

            if (IsValidPassword(passwordTitle, password))
            {
                if (IsEditMode)
                {
                    log.Info($"Saving changes to existing password. User : {CurrentUserManager.CurrentUser.Email}");

                    PasswordViewModel passwordViewModel = (PasswordViewModel)DataContext;
                    passwordViewModel.UpdatePasswordModel(passwordTitle, emailUsername, password, folderId);

                    passwordManager.ChangePassword(passwordViewModel.Password);
                }
                else
                {
                    log.Info($"Creating a new password. User : {CurrentUserManager.CurrentUser.Email}");
                    passwordManager.SavePassword(passwordTitle, password,
                                                        emailUsername, folderId);
                }

                OnPasswordCreated?.Invoke(this, EventArgs.Empty);
                this.NavigationService.GoBack();
            }
            else
            {
                log.Warn($"Invalid password. Showing error message. User : {CurrentUserManager.CurrentUser.Email}");
                ShowError();
            }
        }

        private void ShowError()
        {
            PasswordTitleTextBoxError.Visibility = string.IsNullOrWhiteSpace(PasswordTitleTextBox?.Text) ? Visibility.Visible : Visibility.Collapsed;
            PasswordTextBoxError.Visibility = string.IsNullOrWhiteSpace(PasswordTextBox?.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool IsValidPassword(string title, string password)
        {
            return !string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(password);
        }

        private void LoadFolders()
        {
            List<FolderModel> userFolders = folderManager.GetUserFolders();

            ComboBoxItem defaultItem = new ();
            defaultItem.SetResourceReference(ContentProperty, "NoFolder");
            FoldersComboBox.Items.Add(defaultItem);

            foreach (var folder in userFolders)
            {
                ComboBoxItem item = new ()
                {
                    Content = folder.Title,
                    Tag = folder?.Id
                };
                FoldersComboBox?.Items.Add(item);
            }

            FoldersComboBox.SelectedIndex = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void GeneratePassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GenaratePassword.Visibility = Visibility.Visible;
            GenaratePasswordBackground.Visibility = Visibility.Visible;

            GeneratePasswordAndUpdateLabel();
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!GenaratePassword.IsMouseOver)
            {
                GenaratePassword.Visibility = Visibility.Collapsed;
                GenaratePasswordBackground.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordParameterChanged(object sender, RoutedEventArgs e)
        {
            GeneratePasswordAndUpdateLabel();
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            GeneratePasswordAndUpdateLabel();
        }

        private void PasswordLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GeneratePasswordAndUpdateLabel();
        }

        private void GeneratePasswordAndUpdateLabel()
        {
            int passwordLength = (int)PasswordLengthSlider.Value;

            bool passwordCapitalLetters = PasswordCapitalLetters?.IsChecked == true;
            bool passwordDigits = PasswordDigits?.IsChecked == true;
            bool passwordSymbols = PasswordSymbols?.IsChecked == true;
            if (PasswordLengthTxt != null)
            {
                PasswordLengthTxt.Content = passwordLength;
            }

            string generatedPassword = PasswordGenerator.GeneratePassword(passwordLength, passwordCapitalLetters, passwordDigits, passwordSymbols);
            GeneratedPassword.Text = generatedPassword;
        }

        private void FillPassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = GeneratedPassword?.Text.ToString();

            GenaratePassword.Visibility = Visibility.Collapsed;
            GenaratePasswordBackground.Visibility = Visibility.Collapsed;

            PasswordLengthSlider.Value = 10;
            PasswordCapitalLetters.IsChecked = false;
            PasswordDigits.IsChecked = false;
            PasswordSymbols.IsChecked = false;
        }
    }
}