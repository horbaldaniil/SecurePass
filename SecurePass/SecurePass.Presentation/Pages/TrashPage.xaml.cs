using SecurePass.BLL;
using SecurePass.DAL.Model;
using SecurePass.Presentation.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace SecurePass.Presentation.Pages;

/// <summary>
/// Interaction logic for TrashPage.xaml
/// </summary>
public partial class TrashPage : Page
{

    private UserModel currentUser = CurrentUserManager.CurrentUser;
    private ObservableCollection<PasswordViewModel> passwordViewModels;
    public TrashPage()
    {
        InitializeComponent();
        GetData();
    }

    public void GetData()
    {
        using (var db = new SecurePassDbContext())
        {
            var passwordItems = db.Passwords.Where(f => f.UserId == currentUser.Id && f.Deleted == true).ToList();
            passwordViewModels = new ObservableCollection<PasswordViewModel>(
                passwordItems.Select(password => new PasswordViewModel { Password = password, IsPasswordVisible = false })
            );
            if( passwordItems.Any() )
            {
                TrashEmpty.Visibility = Visibility.Collapsed;
            }
            else
            {
                TrashEmpty.Visibility = Visibility.Visible;
            }
            DataBinding.ItemsSource = passwordViewModels;
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

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        Button deleteButton = (Button)sender;
        PasswordViewModel passwordViewModel = (PasswordViewModel)deleteButton.DataContext;

        if (passwordViewModel != null)
        {
            using (var db = new SecurePassDbContext())
            {
                db.Passwords.Remove(passwordViewModel.Password);
                db.SaveChanges();
            }

            GetData();
        }
    }
    private void RestoreButton_Click(object sender, RoutedEventArgs e)
    {
        Button deleteButton = (Button)sender;
        PasswordViewModel passwordViewModel = (PasswordViewModel)deleteButton.DataContext;

        if (passwordViewModel != null)
        {
            using (var db = new SecurePassDbContext())
            {
                var existingPassword = db.Passwords.Find(passwordViewModel.Password.Id);
                existingPassword.Deleted = false;
                db.SaveChanges();
            }

            GetData();
        }
    }
}
