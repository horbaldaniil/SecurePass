using SecurePass.BLL;
using SecurePass.DAL.Model;
using SecurePass.Presentation.Pages;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SecurePass.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserModel currentUser = CurrentUserManager.CurrentUser;
        public MainWindow()
        {
            InitializeComponent();
            SetLang(Properties.Settings.Default.lang);
            Main.Navigate(new PasswordsPage());

            using (var db = new SecurePassDbContext())
            {
                var User = db.Users.FirstOrDefault(u => u.Id == currentUser.Id);
                UserEmail.Text = User.Email; 
            }
        }
        private void SwitchMenuStyle()
        {
            FoldersMenuLabel.Style = (Style)FindResource("MenuLabel");
            PasswordsMenuLabel.Style = (Style)FindResource("MenuLabel");
            TrashMenuLabel.Style = (Style)FindResource("MenuLabel");
        }


        private void Folders_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SwitchMenuStyle();
            FoldersMenuLabel.Style = (Style)FindResource("ActiveMenu");

            Main.Navigate(new FoldersPage());
        }

        private void Trash_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            SwitchMenuStyle();
            TrashMenuLabel.Style = (Style)FindResource("ActiveMenu");

            Main.Navigate(new TrashPage());
        }

        private void Passwords_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            SwitchMenuStyle();
            PasswordsMenuLabel.Style = (Style)FindResource("ActiveMenu");

            Main.Navigate(new PasswordsPage());
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

        private void Lang_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetLang(((Label)sender).Tag.ToString());
        }

        private void SetLang(string lang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

            Application.Current.Resources.MergedDictionaries.Clear();
            ResourceDictionary resdict = new ResourceDictionary()
            {
                Source = new Uri($"/Languages/Dictionary-{lang}.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(resdict);

            Properties.Settings.Default.lang = lang;
            Properties.Settings.Default.Save();

            switch (lang)
            {
                case "en-US":
                    EnLang.Style = (Style)FindResource("SettingItemLabelActive");
                    UaLang.Style = (Style)FindResource("SettingItemLabel");
                    break;
                case "uk-UA":
                    UaLang.Style = (Style)FindResource("SettingItemLabelActive");
                    EnLang.Style = (Style)FindResource("SettingItemLabel");
                    break;
                default:
                    break;
            }
        }
    }
}
