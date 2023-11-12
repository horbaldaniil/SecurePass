using MaterialDesignThemes.Wpf;
using SecurePass.BLL;
using SecurePass.DAL.Model;
using SecurePass.Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

    public PasswordsPage()
    {
        InitializeComponent();
        Snackbar.MessageQueue = snackbarMessageQueue;
        GetData();
    }

    public PasswordsPage(int folderId, string folderTitle)
    {
        InitializeComponent();
        PasswordsPageLabel.Content = $"📁 {folderTitle}";
        Snackbar.MessageQueue = snackbarMessageQueue;
        GetData(folderId);
    }


    public void GetData(int folderId = 0)
    {
        using (var db = new SecurePassDbContext())
        {
            if (folderId == 0)
            {
                var passwordItems = db.Passwords.Where(f => f.UserId == currentUser.Id && f.Deleted == false).ToList();

                passwordViewModels = new ObservableCollection<PasswordViewModel>(
                passwordItems.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false })
                );
            }
            else
            {
                var passwordItems = db.Passwords.Where(f => f.UserId == currentUser.Id && f.Deleted == false && f.FolderId == folderId).ToList();

                passwordViewModels = new ObservableCollection<PasswordViewModel>(
                passwordItems.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false })
                );
            }
            
            DataBinding.ItemsSource = passwordViewModels;
        }
    }

    private void AddNewButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        Button deleteButton = (Button)sender;
        PasswordViewModel passwordViewModel = (PasswordViewModel)deleteButton.DataContext;

        if (passwordViewModel != null)
        {
            using (var db = new SecurePassDbContext())
            {
                var existingPassword = db.Passwords.Find(passwordViewModel.Password.Id);
                existingPassword.Deleted = true;
                ShowSnackbar("Password moved to trash!");
                db.SaveChanges();
            }

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
            ShowSnackbar("Password copied!");
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
