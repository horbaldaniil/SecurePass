using SecurePass.BLL;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SecurePass.Presentation
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private LoginLogic loginLogic;
        private bool loading;

        public LogInWindow()
        {
            InitializeComponent();
            SetLang(Properties.Settings.Default.lang);
            loginLogic = new LoginLogic();

            EmailTextBox.Text = "gorbaldaniil@gmail.com";
            PasswordTextBox.Text = "Gorbal1234!";
        }

        private void Lang_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetLang(((Image)sender).Tag.ToString());
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

            switch (lang)
            {
                case "en-US":
                    LangImage.Source = new BitmapImage(new Uri($"/Images/uk-UA.png", UriKind.Relative));
                    LangImage.Tag = "uk-UA";
                    break;
                case "uk-UA":
                    LangImage.Source = new BitmapImage(new Uri($"/Images/en-US.png", UriKind.Relative));
                    LangImage.Tag = "en-US";
                    break;
                default:
                    break;
            }

            Properties.Settings.Default.lang = lang;
            Properties.Settings.Default.Save();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!loading)
            {
                loading = true;
                LoginButton.Style = (Style)FindResource("LoginButtonLoaded");
                try
                {
                    ValidErrorLabel.Content = "";
                    EmailErrorLabel.Content = "";

                    string email = EmailTextBox.Text;
                    string password = PasswordTextBox.Text;

                    if (!loginLogic.IsValidEmail(email))
                    {
                        EmailErrorLabel.SetResourceReference(ContentProperty, "InvalidFormatEmail");
                        return;
                    }

                    var loginResult = await loginLogic.VerifyUser(email, password);

                    if (loginResult != null)
                    {
                        ValidErrorLabel.SetResourceReference(ContentProperty, loginResult);
                    }
                    else
                    {
                        var window = new MainWindow();
                        window.Show();
                        Close();
                    }
                }
                finally { LoginButton.Style = (Style)FindResource("LoginButton"); loading = false; }
            }
            
        }

        private void SignUpLabel_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow window = new SignUpWindow();
            window.Show();
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
