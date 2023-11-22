using SecurePass.BLL;
using SecurePass.DAL.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SecurePass.Presentation.Pages;

/// <summary>
/// Interaction logic for FoldersPage.xaml
/// </summary>
public partial class FoldersPage : Page
{
    private readonly UserModel? currentUser = CurrentUserManager.CurrentUser;
    private readonly FolderManager folderManager;
    private int? currentEditingFolderId;
    public FoldersPage()
    {
        InitializeComponent();
        folderManager = new FolderManager(currentUser);
        LoadFolders();
    }

    private void LoadFolders()
    {
        var folders = folderManager.GetUserFolders();
        DataBinding.ItemsSource = folders;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string newFolderName = NewFolderTextBox.Text;

        if (!string.IsNullOrWhiteSpace(newFolderName))
        {
            if (currentEditingFolderId.HasValue)
            {
                bool folderUpdated = folderManager.ChangeFolder(currentEditingFolderId.Value, newFolderName);

                if (folderUpdated)
                {
                    LoadFolders();
                    SetVisibility(false);
                    ClearNewFolderTextBox();
                    currentEditingFolderId = null;
                }
                else
                {
                    FolderError("FolderNameExist");
                }
            }
            else
            {
                bool folderAdded = folderManager.AddNewFolder(newFolderName);

                if (folderAdded)
                {
                    LoadFolders();
                    SetVisibility(false);
                    ClearNewFolderTextBox();
                }
                else
                {
                    FolderError("FolderNameExist");
                }
            }
        }
        else
        {
            FolderError("FolderNameEmpty");
        }
    }

    private void AddNewFolder_Click(object sender, RoutedEventArgs e)
    {
        SetVisibility(true);
    }

    private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!NewFolderPanel.IsMouseOver)
        {
            SetVisibility(false);
            NewFolderTextBox.Clear();
        }
    }

    private void SetVisibility(bool isVisible)
    {
        NewFolderPanel.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        GenaratePasswordBackground.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        FolderNameError.Visibility = IsVisible ? Visibility.Collapsed : Visibility.Collapsed;
    }

    private void FolderError(string error)
    {
        FolderNameError.SetResourceReference(ContentProperty, error);
        FolderNameError.Visibility = Visibility.Visible;
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        Button deleteButton = sender as Button;
        FolderModel folder = (FolderModel)deleteButton.DataContext;

        folderManager.DeleteFolder(folder);
        LoadFolders();
 
    }

    private void ChangeButton_Click(object sender, RoutedEventArgs e)
    {
        Button changeButton = sender as Button;
        FolderModel folder = (FolderModel)changeButton.DataContext;

        NewFolderTextBox.Text = folder.Title;
        currentEditingFolderId = folder.Id;

        SetVisibility(true);
        
    }

    private void ClearNewFolderTextBox()
    {
        NewFolderTextBox.Text = "";
    }

    private void OpenButton_Click(object sender, RoutedEventArgs e)
    {
        Button showPasswords = sender as Button;
        FolderModel folder = (FolderModel)showPasswords.DataContext;

        NavigationService navigationService = NavigationService.GetNavigationService(this);

        if (navigationService != null)
        {
            navigationService.Navigate(new PasswordsPage(folder.Id, folder.Title));
        }
    }
}
