using Microsoft.EntityFrameworkCore;
using SecurePass.BLL;
using SecurePass.DAL.Model;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SecurePass.Presentation
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private LoginLogic loginLogic;

        public LogInWindow()
        {
            InitializeComponent();
            SetLang(Properties.Settings.Default.lang);
            loginLogic = new LoginLogic();
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
            LoginButton.IsEnabled = false;

            try
            {
                PasswordErrorLabel.Content = "";
                EmailErrorLabel.Content = "";

                string email = EmailTextBox.Text;
                string password = PasswordTextBox.Text;

                if (!loginLogic.IsValidEmail(email))
                {
                    EmailErrorLabel.SetResourceReference(ContentProperty, "InvalidFormatEmail");
                    return;
                }

                using (SecurePassDbContext db = new SecurePassDbContext())
                {
                    var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email);

                    if (user != null)
                    {
                        if (await loginLogic.VerifyPasswordAsync(password, user.Password))
                        {
                            CurrentUserManager.SetCurrentUser(user);
                            MainWindow window = new MainWindow();
                            window.Show();
                            Close();
                        }
                        else
                        {
                            PasswordErrorLabel.SetResourceReference(ContentProperty, "InvalidPassword");
                        }
                    }
                    else
                    {
                        EmailErrorLabel.SetResourceReference(ContentProperty, "NotFoundEmail");
                    }
                }
            }
            finally { LoginButton.IsEnabled = true; }
        }

        private void SignUpLabel_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow window = new SignUpWindow();
            window.Show();
            Close();
        }
        public void RemoveText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            instance.Foreground = Brushes.Black;
            if (instance.Text == instance.Tag.ToString())
                instance.Text = "";
        }

        public void AddText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            Color color = (Color)ColorConverter.ConvertFromString("#A9B1B8");

            if (string.IsNullOrWhiteSpace(instance.Text))
            {
                instance.Text = instance.Tag.ToString();
                instance.Foreground = new SolidColorBrush(color);
            }
        }
    }
}
