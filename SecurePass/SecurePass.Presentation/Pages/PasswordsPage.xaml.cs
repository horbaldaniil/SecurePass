using SecurePass.DAL.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SecurePass.Presentation.Pages;

/// <summary>
/// Interaction logic for PasswordsPage.xaml
/// </summary>
public partial class PasswordsPage : Page
{
    private Frame _mainFrame;
    public int loggedInUserId { get; set; }

    public PasswordsPage()
    {
        InitializeComponent();
    }

    private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox != null)
        {
            textBox.Focus();
            textBox.SelectAll();
            Clipboard.SetText(textBox.Text);
            e.Handled = false;
        }
    }
    public PasswordsPage(Frame mainFrame, int Id)
    {
        _mainFrame = mainFrame;
        InitializeComponent();
        GetData(Id);
    }


    public void GetData(int Id)
    {
        loggedInUserId = Id;
        using (var db = new SecurePassDbContext())
        {
            var passwordItems = db.Passwords.Where(f => f.UserId == loggedInUserId).ToList();

            DataBinding.ItemsSource = passwordItems;
        }
    }

    private void AddNewButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var CreatePasswordPage = new CreatePasswordPage(loggedInUserId);
        _mainFrame.Navigate(CreatePasswordPage);
    }
}
