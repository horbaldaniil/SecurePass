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

    private readonly UserModel? currentUser = CurrentUserManager.CurrentUser;
    private ObservableCollection<PasswordViewModel> passwordViewModels;
    private readonly PasswordManager passwordManager;

    public TrashPage()
    {
        InitializeComponent();
        passwordManager = new PasswordManager(currentUser);
        GetData();
    }

    public void GetData()
    {
        using (var db = new SecurePassDbContext())
        {
            var passwordItems = db.Passwords.Where(f => f.UserId == currentUser.Id && f.Deleted == true).ToList();
            passwordViewModels = new ObservableCollection<PasswordViewModel>(
                                 passwordItems.Select(PasswordViewModel.CreateFromPassword));

            TrashEmpty.Visibility = passwordItems.Any() ? Visibility.Collapsed : Visibility.Visible;
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

    private void HandleDeleteOrRestoreButtonClick(Button button, bool isDelete)
    {
        PasswordViewModel passwordViewModel = (PasswordViewModel)button.DataContext;

        if (passwordViewModel != null)
        {
            if (isDelete)
            {
                passwordManager.DeletePassword(passwordViewModel.Password);
            }
            else
            {
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
