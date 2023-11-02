using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SecurePass.Presentation
{
    /// <summary>
    /// Interaction logic for PasswordsWindow.xaml
    /// </summary>
    public partial class PasswordsWindow : Window
    {
        public PasswordsWindow()
        {
            InitializeComponent();
        }

        private void Folders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FoldersWindow foldersWindow = new FoldersWindow();
            foldersWindow.Show();
            Close();
        }

        private void Trash_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            TrashWindow trashWindow = new TrashWindow();
            trashWindow.Show();
            Close();
        }

        private void Passwords_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            PasswordsWindow passwordsWindow = new PasswordsWindow();
            passwordsWindow.Show();
            Close();
        }

        private void AddNewButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CreatePasswordWindow createPasswordWindow = new CreatePasswordWindow();
            createPasswordWindow.Show();
            Close();
        }

        private void UserImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserInfoPanel.Visibility = Visibility.Visible;
        }

        private void SettingsImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SettingsPanel.Visibility = Visibility.Visible;
        }


        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!UserInfoPanel.IsMouseOver)
            {
                UserInfoPanel.Visibility = Visibility.Collapsed;
            }
            if (!SettingsPanel.IsMouseOver)
            {
                SettingsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            Close();
        }
    }
}
