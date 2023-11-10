using SecurePass.BLL;
using SecurePass.DAL.Model;
using System;
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
                    UserId = currentUser.Id
                };

                db.Passwords.Add(newPassword);
                db.SaveChanges();
                
            }
        }
        OnPasswordCreated?.Invoke(this, EventArgs.Empty);
        this.NavigationService.GoBack();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.NavigationService.GoBack();
    }

    public void RemoveText(object sender, EventArgs e)
    {
        TextBox instance = (TextBox)sender;
        instance.Foreground = Brushes.Black;
        if (instance.Text == instance.Tag.ToString())
            instance.Text = "";
    }

    public void AddText(object sender, EventArgs e)
    {
        TextBox instance = (TextBox)sender;
        Color color = (Color)ColorConverter.ConvertFromString("#A9B1B8");

        if (string.IsNullOrWhiteSpace(instance.Text))
        {
            instance.Text = instance.Tag.ToString();
            instance.Foreground = new SolidColorBrush(color);
        }
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
