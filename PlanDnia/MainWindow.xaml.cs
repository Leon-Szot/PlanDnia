using System;
using System.Collections.Generic;
using System.Windows;

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

        // Obsługuje zmianę wyboru w ListBox
        private void TasksListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedTask = (Task)TasksListBox.SelectedItem;
            if (selectedTask != null)
            {
                // Wyświetlanie szczegółów wybranego zadania
                TaskDetailsTextBlock.Text = selectedTask.Title;
                TaskDescriptionTextBlock.Text = $"Description: {selectedTask.Description}";
                TaskDateTextBlock.Text = $"Date: {selectedTask.Date}";
                TaskCompletedCheckBox.IsChecked = selectedTask.IsCompleted;
            }
            else
            {
                // Jeśli nic nie jest wybrane, wyczyść szczegóły
                TaskDetailsTextBlock.Text = string.Empty;
                TaskDescriptionTextBlock.Text = string.Empty;
                TaskDateTextBlock.Text = string.Empty;
                TaskCompletedCheckBox.IsChecked = false;
            }
        }
    }
}
