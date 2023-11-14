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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private SignUpLogic signUpLogic;
        public SignUpWindow()
        {
            InitializeComponent();
            SetLang(Properties.Settings.Default.lang);
            signUpLogic = new SignUpLogic();
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
        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            EmailErrorLabel.Content = "";
            PasswordErrorLabel.Content = "";

            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            if (!signUpLogic.IsValidEmail(email))
            {
                EmailErrorLabel.SetResourceReference(ContentProperty, "InvalidFormatEmail");
                return;
            }

            if (!signUpLogic.IsValidPassword(password))
            {
                PasswordErrorLabel.SetResourceReference(ContentProperty, "InvalidFormatPassword");
                return;
            }

            var signUpResult = await signUpLogic.UserRegistration(email, password);

            if (signUpResult != null)
            {
                EmailErrorLabel.SetResourceReference(ContentProperty, signUpResult);
            }
            else
            {
                MessageBox.Show("Registration successful. You can now log in.");

                LogInWindow loginWindow = new LogInWindow();
                loginWindow.Show();
                Close();
            }
        }

        private void LoginLabel_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow window = new LogInWindow();
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
