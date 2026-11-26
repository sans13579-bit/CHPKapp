using System;
using System.Windows;

namespace CHPKapp
{
    public partial class AddRecordWindow : Window
    {
        public DateTime NewRecordDate { get; private set; }
        public string NewRecordFullName { get; private set; }
        public string NewRecordRoom { get; private set; }
        public string NewRecordIssueTime { get; private set; }
        public string NewRecordNote { get; private set; }

        public AddRecordWindow()
        {
            InitializeComponent();
            DatePickerRecord.SelectedDate = DateTime.Now;
            TxtIssueTime.Text = DateTime.Now.ToString("HH:mm");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFullName.Text) ||
                string.IsNullOrWhiteSpace(TxtRoom.Text) ||
                DatePickerRecord.SelectedDate == null)
            {
                MessageBox.Show("Заполните обязательные поля: ФИО и Комната!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewRecordDate = DatePickerRecord.SelectedDate.Value;
            NewRecordFullName = TxtFullName.Text;
            NewRecordRoom = TxtRoom.Text;
            NewRecordIssueTime = TxtIssueTime.Text;
            NewRecordNote = TxtNote.Text;

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}