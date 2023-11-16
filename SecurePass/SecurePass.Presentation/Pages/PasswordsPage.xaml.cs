using MaterialDesignThemes.Wpf;
using SecurePass.BLL;
using SecurePass.DAL.Model;
using SecurePass.Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SecurePass.Presentation.Pages;

/// <summary>
/// Interaction logic for PasswordsPage.xaml
/// </summary>
public partial class PasswordsPage : Page
{
    private UserModel currentUser = CurrentUserManager.CurrentUser;
    private ObservableCollection<PasswordViewModel> passwordViewModels;
    private SnackbarMessageQueue snackbarMessageQueue = new SnackbarMessageQueue();
    private readonly PasswordManager passwordManager;

    public PasswordsPage()
    {
        InitializeComponent();
        Snackbar.MessageQueue = snackbarMessageQueue;
        passwordManager = new PasswordManager(currentUser);
        GetData();
    }

    public PasswordsPage(int folderId, string folderTitle)
    {
        InitializeComponent();
        AddPasswordButton.Visibility = Visibility.Collapsed;
        PasswordsPageLabel.Content = $"📁 {folderTitle}";
        Snackbar.MessageQueue = snackbarMessageQueue;
        passwordManager = new PasswordManager(currentUser);
        GetData(folderId);
    }

    public void GetData(int? folderId = null)
    {
        
        if (folderId == null)
        {
            var passwords = passwordManager.GetPasswords()
                .Where(p => p.Deleted == false)
                .OrderByDescending(p => p.LastUpdated)
                .ToList();
            passwordViewModels = new ObservableCollection<PasswordViewModel>(
            passwords.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false })
            );
        }
        else
        {
            var passwords = passwordManager.GetPasswords()
                .Where(p => p.Deleted == false && p.FolderId == folderId)
                .OrderByDescending(p => p.LastUpdated)
                .ToList();
                
            passwordViewModels = new ObservableCollection<PasswordViewModel>(
            passwords.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false })
            );
        }
            
        DataBinding.ItemsSource = passwordViewModels;
        
    }

    private void AddNewPassword_Click(object sender, RoutedEventArgs e)
    {
        NavigationService navigationService = NavigationService.GetNavigationService(this);

        if (navigationService != null)
        {
            var CreatePasswordPage = new CreatePasswordPage();
            CreatePasswordPage.OnPasswordCreated += PasswordCreatedHandler;
            navigationService.Navigate(CreatePasswordPage);
        }
    }
    private void PasswordCreatedHandler(object sender, EventArgs e)
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

            createPasswordPage.OnPasswordCreated += PasswordCreatedHandler;

            NavigationService.Navigate(createPasswordPage);
        }
    }

}
