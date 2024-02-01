using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Union
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseHelper databaseHelper;
        public MainWindow()
        {
            InitializeComponent();
            databaseHelper = new DatabaseHelper();
            RefreshData();
            RefreshDataSanatorium();
            RefreshDataPayment();
        }

        private void RefreshData()
        {
            dataGrid.ItemsSource = databaseHelper.GetStudents();
        }
        private void RefreshDataSanatorium()
        {
            dataGridSanatoria.ItemsSource = databaseHelper.GetSanatoria();
        }
        private void RefreshDataPayment()
        {
            dataGridPayment.ItemsSource = databaseHelper.GetPayments();
        }

        private void AddStudentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var newStudent = new Student();

            var addStudentWindow = new AddEditStudentWindow(newStudent);
            addStudentWindow.ShowDialog();
            RefreshData();
        }

        private void AddSanatoriumMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Логика обработки нажатия на "Добавить в санаторий"
        }

        private void AddPaymentMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPaymentPeriodMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddFacultyMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Получаем выделенного студента
            Student selectedStudent = (Student)dataGrid.SelectedItem;

            if (selectedStudent != null)
            {
                // Создаем окно редактирования студента
                AddEditStudentWindow editStudentWindow = new AddEditStudentWindow(selectedStudent);

                // Открываем окно в модальном режиме для редактирования студента
                editStudentWindow.ShowDialog();

                // Обновляем данные в DataGrid после закрытия окна редактирования
                RefreshData();
            }
        }
    }
}
