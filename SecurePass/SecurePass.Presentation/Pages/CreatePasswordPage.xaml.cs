using SecurePass.BLL;
using SecurePass.DAL.Model;
using SecurePass.Presentation.ViewModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SecurePass.Presentation.Pages;

/// <summary>
/// Interaction logic for CreatePasswordPage.xaml
/// </summary>
public partial class CreatePasswordPage : Page
{
    public event EventHandler OnPasswordCreated;
    private UserModel currentUser = CurrentUserManager.CurrentUser;
    public CreatePasswordPage()
    {
        InitializeComponent();
        this.Loaded += CreatePasswordPage_Loaded;
    }

    private bool isEditMode;

    public bool IsEditMode
    {
        get { return isEditMode; }
        set
        {
            isEditMode = value;
        }
    }
    private void CreatePasswordPage_Loaded(object sender, RoutedEventArgs e)
    {
        LoadFolders();

        IsEditMode = DataContext != null;

        if (IsEditMode)
        {
            var passwordViewModel = (PasswordViewModel)DataContext;

            PasswordTitleTextBox.Text = passwordViewModel.Password.Title;
            EmailOrUsernameTextBox.Text = passwordViewModel.Password.Email_Username;
            PasswordTextBox.Text = passwordViewModel.Password.Password;

            foreach (ComboBoxItem item in FoldersComboBox.Items)
            {
                if (item.Tag is int folderId && folderId == passwordViewModel.Password.FolderId)
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
        CreatePasswordPageTitle.SetResourceReference(ContentProperty, "ChangePassword");
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string passwordTitle = PasswordTitleTextBox.Text;
        string emailOrUsername = EmailOrUsernameTextBox.Text;
        string password = PasswordTextBox.Text;
        

        using (var db = new SecurePassDbContext())
        {
            if (IsEditMode)
            {
                var passwordViewModel = (PasswordViewModel)PasswordTitleTextBox.DataContext;

                if (passwordViewModel != null)
                {
                    var existingPassword = db.Passwords.Find(passwordViewModel.Password.Id);

                    if (existingPassword != null)
                    {
                        if (!string.IsNullOrEmpty(passwordTitle) && !string.IsNullOrEmpty(password))
                        {
                            existingPassword.Title = passwordTitle;
                            existingPassword.Email_Username = emailOrUsername;
                            existingPassword.Password = password;
                            existingPassword.LastUpdated = DateTime.UtcNow;

                            if (FoldersComboBox.SelectedItem is ComboBoxItem selectedFolderItem)
                            {
                                if (selectedFolderItem.Tag != null)
                                {
                                    int folderId = (int)selectedFolderItem.Tag;
                                    existingPassword.FolderId = folderId;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(passwordTitle) && !string.IsNullOrEmpty(password))
                {
                    var newPassword = new PasswordModel
                    {
                        Title = passwordTitle,
                        Password = password,
                        UserId = currentUser.Id,
                        Email_Username = string.IsNullOrEmpty(emailOrUsername) ? null : emailOrUsername,
                        LastUpdated = DateTime.UtcNow,
                    };

                    if (FoldersComboBox.SelectedItem is ComboBoxItem selectedFolderItem)
                    {
                        if (selectedFolderItem.Tag != null)
                        {
                            int folderId = (int)selectedFolderItem.Tag;
                            newPassword.FolderId = folderId;
                        }
                    }

                    db.Passwords.Add(newPassword);
                }
            }
            db.SaveChanges();
        }

        OnPasswordCreated?.Invoke(this, EventArgs.Empty);
        this.NavigationService.GoBack();
    }

    private void LoadFolders()
    {
        using (var db = new SecurePassDbContext())
        {
            var userFolders = db.Folders.Where(f => f.UserId == currentUser.Id).ToList();

            ComboBoxItem defaultItem = new ComboBoxItem();
            defaultItem.Content = "Без папки";
            FoldersComboBox.Items.Add(defaultItem);

            foreach (var folder in userFolders)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = folder.Title;
                item.Tag = folder.Id;
                FoldersComboBox.Items.Add(item);
            }

            FoldersComboBox.SelectedIndex = 0;
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.NavigationService.GoBack();
    }

    private void GeneratePassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        GenaratePassword.Visibility = Visibility.Visible;
        GenaratePasswordBackground.Visibility = Visibility.Visible;
    }

    private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!GenaratePassword.IsMouseOver)
        {
            GenaratePassword.Visibility = Visibility.Collapsed;
            GenaratePasswordBackground.Visibility = Visibility.Collapsed;
        }
    }
}
