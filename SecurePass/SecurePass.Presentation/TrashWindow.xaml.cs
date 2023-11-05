using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SecurePass.Presentation
{
    /// <summary>
    /// Interaction logic for TrashWindow.xaml
    /// </summary>
    public partial class TrashWindow : Window
    {
        public TrashWindow()
        {
            InitializeComponent();
            SetLang(Properties.Settings.Default.lang);

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
