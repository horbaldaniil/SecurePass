using SecurePass.BLL;
using SecurePass.DAL.Model;
using System;
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

    public PasswordsPage()
    {
        InitializeComponent();
        GetData();
    }

    public void GetData()
    {
        using (var db = new SecurePassDbContext())
        {
            var passwordItems = db.Passwords.Where(f => f.UserId == currentUser.Id).ToList();
            DataBinding.ItemsSource = passwordItems;
        }
    }

    private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox != null)
        {
            textBox.Focus();
            textBox.SelectAll();
            Clipboard.SetText(textBox.Text);
            e.Handled = false;
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
}
