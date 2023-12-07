// <copyright file="FoldersPage.xaml.cs" company="SecurePass">
// Copyright (c) SecurePass. All rights reserved.
// </copyright>

namespace SecurePass.Presentation.Pages
{
    using System.Threading.Tasks;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using System.Windows.Navigation;
    using SecurePass.BLL;
    using SecurePass.DAL.Model;
    using log4net;
    using SecurePass.Presentation.ViewModel;

    /// <summary>
    /// Interaction logic for FoldersPage.xaml.
    /// </summary>
    public partial class FoldersPage : Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly UserModel? currentUser = CurrentUserManager.CurrentUser;
        private readonly FolderManager folderManager;
        private int? currentEditingFolderId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersPage"/> class.
        /// </summary>
        public FoldersPage()
        {
            InitializeComponent();
            folderManager = new FolderManager(currentUser);
            LoadFolders();
        }

        private async void LoadingAnimation()
        {
            var items = DataBinding.Items;

            foreach (var item in items)
            {
                var container = DataBinding.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (container != null)
                {
                    container.Opacity = 0;
                }
            }

            foreach (var item in items)
            {
                var container = DataBinding.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (container != null)
                {
                    var animation = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    };

                    container.BeginAnimation(UIElement.OpacityProperty, animation);
                }

                await Task.Delay(100);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingAnimation();
        }

        private void LoadFolders()
        {
            var folders = folderManager.GetUserFolders();
            DataBinding.ItemsSource = folders;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newFolderName = NewFolderTextBox.Text;

                if (!string.IsNullOrWhiteSpace(newFolderName))
                {
                    if (currentEditingFolderId.HasValue)
                    {
                        log.Info($"Editing existing folder. User : {CurrentUserManager.CurrentUser.Email}.");
                        bool folderUpdated = folderManager.ChangeFolder(currentEditingFolderId.Value, newFolderName);

                        if (folderUpdated)
                        {
                            log.Info($"Folder changed. Folder : {currentEditingFolderId}. User : {CurrentUserManager.CurrentUser.Email}.");
                            LoadFolders();
                            SetVisibility(false);
                            ClearNewFolderTextBox();
                            currentEditingFolderId = null;
                        }
                        else
                        {
                            log.Warn($"Folder with this name already exists. User : {CurrentUserManager.CurrentUser.Email}.");

                            FolderError("FolderNameExist");
                        }
                    }
                    else
                    {
                        log.Info($"Adding new folder. User : {CurrentUserManager.CurrentUser.Email}.");
                        bool folderAdded = folderManager.AddNewFolder(newFolderName);

                        if (folderAdded)
                        {
                            log.Info($"Folder added. Folder : {newFolderName}. User : {CurrentUserManager.CurrentUser.Email}.");

                            LoadFolders();
                            SetVisibility(false);
                            ClearNewFolderTextBox();
                        }
                        else
                        {
                            log.Warn($"Folder with this name already exists. User : {CurrentUserManager.CurrentUser.Email}.");

                            FolderError("FolderNameExist");
                        }
                    }
                }
                else
                {
                    log.Warn($"Folder name is empty. User : {CurrentUserManager.CurrentUser.Email}.");

                    FolderError("FolderNameEmpty");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in folder save func: {ex}");
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

            log.Info($"Folder deleted. Folder : {folder.Id}. User : {CurrentUserManager.CurrentUser.Email}.");

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

            navigationService?.Navigate(new PasswordsPage(folder.Id, folder.Title));
        }
    }
}