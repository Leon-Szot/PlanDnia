using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlanDnia
{

    public partial class MainWindow : Window
    {
        private DataBase db;

        public MainWindow()
        {
            InitializeComponent();
            db = new DataBase("plan_dnia.db");
            db.CreateTable();
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = db.GetTasks();
            TasksListBox.ItemsSource = tasks;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new Task
            {
                Title = TaskTitleTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                Date = TaskDatePicker.SelectedDate ?? DateTime.Now,
                IsCompleted = false
            };
            db.AddTask(newTask);
            LoadTasks();
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = (Task)TasksListBox.SelectedItem;
            if (selectedTask != null)
            {
                db.DeleteTask(selectedTask.Id);
                LoadTasks();
            }
        }
    }
}
