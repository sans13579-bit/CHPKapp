using System.Linq;
using System.Windows;

namespace CHPKapp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new CHPKHostelEntities1())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    MainWindow mainWindow = new MainWindow(user.Role);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    PasswordBox.Clear(); 
                }
            }
        }
    }
}