using SecurePass.DAL.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace SecurePass.Presentation.Pages;

/// <summary>
/// Interaction logic for CreatePasswordPage.xaml
/// </summary>
public partial class CreatePasswordPage : Page
{
    public int loggedInUserId { get; set; }
    public CreatePasswordPage(int Id)
    {
        loggedInUserId = Id;
        InitializeComponent();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string passwordTitle = PasswordTitleTextBox.Text;
        string emailOrUsername = EmailOrUsernameTextBox.Text;
        string password = PasswordTextBox.Text;

        if (!string.IsNullOrEmpty(passwordTitle) && !string.IsNullOrEmpty(password))
        {
            using (var db = new SecurePassDbContext())
            {
                var newPassword = new PasswordModel
                {
                    Title = passwordTitle,
                    Password = password,
                    UserId = loggedInUserId
                };

                db.Passwords.Add(newPassword);
                db.SaveChanges();
            }
        }
        this.NavigationService.GoBack();
    }

    
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.NavigationService.GoBack();
    }
    public void RemoveText(object sender, EventArgs e)
    {
        ((App)Application.Current).RemoveText(sender, e);
    }

    public void AddText(object sender, EventArgs e)
    {
        ((App)Application.Current).AddText(sender, e);
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
