using System;
using System.Linq;
using System.Windows;

namespace CHPKapp
{
    public partial class MainWindow : Window
    {
        private string _currentRole;

        public MainWindow(string role)
        {
            InitializeComponent();
            _currentRole = role;

            ConfigureRole();
            LoadDataFromDb();
        }

        private void ConfigureRole()
        {
            if (_currentRole != "Admin")
            {
                BtnUsers.Visibility = Visibility.Collapsed;
            }
            BtnJournal_Click(null, null);
        }

        private void LoadDataFromDb()
        {
            using (var db = new CHPKHostelEntities1()) 
            {
                var records = db.KeyRecords
                                .OrderByDescending(r => r.RecordDate)
                                .ToList();

                MainDataGrid.ItemsSource = records;
            }
        }

        private void BtnJournal_Click(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Журнал учёта ключей";
            MainDataGrid.Visibility = Visibility.Visible;
            BtnAddRecord.Visibility = Visibility.Visible;
            BtnReturnKey.Visibility = Visibility.Visible;
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Управление пользователями (Админ)";
            MainDataGrid.Visibility = Visibility.Collapsed;
            BtnAddRecord.Visibility = Visibility.Collapsed;
            BtnReturnKey.Visibility = Visibility.Collapsed;
            MessageBox.Show("Здесь будет таблица со списком пользователей");
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            PageTitle.Text = "Настройки";
            MainDataGrid.Visibility = Visibility.Collapsed;
            BtnAddRecord.Visibility = Visibility.Collapsed;
            BtnReturnKey.Visibility = Visibility.Collapsed;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void BtnAddRecord_Click(object sender, RoutedEventArgs e)
        {
            AddRecordWindow addWindow = new AddRecordWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                using (var db = new CHPKHostelEntities1())
                {
                    var newRecord = new KeyRecords
                    {
                        RecordDate = addWindow.NewRecordDate,
                        FullName = addWindow.NewRecordFullName,
                        RoomNumber = addWindow.NewRecordRoom,
                        IssueTime = addWindow.NewRecordIssueTime,
                        ReturnTime = null,
                        Note = addWindow.NewRecordNote
                    };

                    db.KeyRecords.Add(newRecord);
                    db.SaveChanges(); 
                }

                LoadDataFromDb(); 
            }
        }

        private void BtnReturnKey_Click(object sender, RoutedEventArgs e)
        {
            var selectedRecord = MainDataGrid.SelectedItem as KeyRecords;

            if (selectedRecord == null)
            {
                MessageBox.Show("Выберите запись в таблице, чтобы сдать ключ.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(selectedRecord.ReturnTime))
            {
                MessageBox.Show($"Ключ уже сдан в {selectedRecord.ReturnTime}!",
                                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string currentTime = DateTime.Now.ToString("HH:mm");

            var result = MessageBox.Show($"Сдать ключ для {selectedRecord.FullName} в {currentTime}?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            using (var db = new CHPKHostelEntities1())
            {
                var recordToUpdate = db.KeyRecords.FirstOrDefault(r => r.Id == selectedRecord.Id);

                if (recordToUpdate != null)
                {
                    recordToUpdate.ReturnTime = currentTime;

                    if (string.IsNullOrWhiteSpace(recordToUpdate.Note))
                    {
                        recordToUpdate.Note = "Ключ сдан";
                    }

                    db.SaveChanges();
                }
            }

            LoadDataFromDb();
        }
    }
}