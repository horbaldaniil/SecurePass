using SecurePass.BLL;
using SecurePass.DAL.Model;
using SecurePass.Presentation.ViewModel;
using System;
using System.Collections.Generic;
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
    private readonly PasswordManager passwordManager;
    private readonly FolderManager folderManager;

    private bool test = false;
    public CreatePasswordPage()
    {
        InitializeComponent();
        this.Loaded += CreatePasswordPage_Loaded;

        UserModel currentUser = CurrentUserManager.CurrentUser;
        passwordManager = new PasswordManager(currentUser);
        folderManager = new FolderManager(currentUser);
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
                PasswordViewModel passwordViewModel = (PasswordViewModel)DataContext;
                passwordViewModel.UpdatePasswordModel(passwordTitle, emailUsername, password, folderId);

                passwordManager?.ChangePassword(passwordViewModel.Password);
            }
            else
            {
                passwordManager?.SavePassword(passwordTitle, password,
                                                    emailUsername, folderId);
            }

            OnPasswordCreated?.Invoke(this, EventArgs.Empty);
            this.NavigationService.GoBack();
        }
        else
        {
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

        ComboBoxItem defaultItem = new ComboBoxItem();
        defaultItem.Content = "Без папки";
        FoldersComboBox.Items.Add(defaultItem);

        foreach (var folder in userFolders)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = folder.Title;
            item.Tag = folder?.Id;
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
