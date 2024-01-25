using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Union
{
    /// <summary>
    /// Логика взаимодействия для AddEditStudentWindow.xaml
    /// </summary>
    public partial class AddEditStudentWindow : Window
    {
        private Student studentToEdit;
        private DatabaseHelper databaseHelper;

        public AddEditStudentWindow(Student studentToEdit)
        {
            InitializeComponent();
            databaseHelper = new DatabaseHelper();
            Loaded += AddEditStudentWindow_Loaded;

            fioTextBox.Text = studentToEdit.Fio;
            dobDatePicker.SelectedDate = studentToEdit.DateOfBirth;
            educationTextBox.Text = studentToEdit.Education;
            addressTextBox.Text = studentToEdit.Address;
            ticketNumberTextBox.Text = studentToEdit.Nbileta.ToString();
            telephoneTextBox.Text = studentToEdit.Telephone;
            otherInfoTextBox.Text = studentToEdit.OtherInfo;

            facultyComboBox.SelectedValue = studentToEdit.Faculty?.name_faculty;
            groupComboBox.SelectedValue = studentToEdit.Group?.name_group;

            this.studentToEdit = studentToEdit;
        }

        private async void AddEditStudentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadFacultiesAsync();
            await LoadGroupsAsync();
        }

        private async Task LoadFacultiesAsync()
        {
            List<Faculty> faculties = await databaseHelper.GetFacultiesAsync();
            facultyComboBox.ItemsSource = faculties;
            facultyComboBox.DisplayMemberPath = "name_faculty";
            facultyComboBox.SelectedValuePath = "Id";
        }

        private async Task LoadGroupsAsync()
        {
            List<Group> groups = await databaseHelper.GetGroupsAsync();
            groupComboBox.ItemsSource = groups;
            groupComboBox.DisplayMemberPath = "name_group";
            groupComboBox.SelectedValuePath = "Id";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidInput())
                    return;

                Student student = CreateStudentObject();

                if (studentToEdit == null)
                {
                    databaseHelper.AddStudent(student);
                }
                else
                {
                    student.Id = studentToEdit.Id;
                    databaseHelper.UpdateStudent(student);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrEmpty(fioTextBox.Text) || dobDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: ФИО и Дата рождения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(ticketNumberTextBox.Text, out int ticketNumber) || ticketNumber < 0)
            {
                MessageBox.Show("Некорректный номер билета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private Student CreateStudentObject()
        {
            return new Student
            {
                Fio = fioTextBox.Text,
                DateOfBirth = dobDatePicker.SelectedDate ?? DateTime.MinValue,
                Education = educationTextBox.Text,
                Address = addressTextBox.Text,
                OtherInfo = otherInfoTextBox.Text,
                Group = groupComboBox.SelectedItem as Group,
                Faculty = facultyComboBox.SelectedItem as Faculty,
                Nbileta = int.Parse(ticketNumberTextBox.Text)
            };
        }
    }
}
