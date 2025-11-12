using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlanDnia
{
    public partial class MainWindow : Window
    {
        private DataBase db;
        Task selectedTask;
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
            var pendingTasks = new List<Task>();
            var completedTasks = new List<Task>();

            foreach (var task in tasks)
            {
                if (task.IsCompleted)
                    completedTasks.Add(task);
                else
                    pendingTasks.Add(task);
            }

            TasksListBox.ItemsSource = pendingTasks;
            CompletedTasksListBox.ItemsSource = completedTasks;
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

            if (selectedTask != null)
            {
                db.DeleteTask(selectedTask.Id);
                LoadTasks();
            }
        }
        private void TasksListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedTask = (Task)TasksListBox.SelectedItem;
            if (selectedTask != null)
            {
                TaskDetailsTextBlock.Text = selectedTask.Title;
                TaskDescriptionTextBlock.Text = $"Description: {selectedTask.Description}";
                TaskDateTextBlock.Text = $"Date: {selectedTask.Date}";
            }
            else
            {
                TaskDetailsTextBlock.Text = string.Empty;
                TaskDescriptionTextBlock.Text = string.Empty;
                TaskDateTextBlock.Text = string.Empty;
            }
        }

        private void CompletedTasksListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedTask = (Task)CompletedTasksListBox.SelectedItem;
            if (selectedTask != null)
            {
                TaskDetailsTextBlock.Text = selectedTask.Title;
                TaskDescriptionTextBlock.Text = $"Description: {selectedTask.Description}";
                TaskDateTextBlock.Text = $"Date: {selectedTask.Date}";
            }
            else
            {
                TaskDetailsTextBlock.Text = string.Empty;
                TaskDescriptionTextBlock.Text = string.Empty;
                TaskDateTextBlock.Text = string.Empty;
            }
        }
        private void MoveToCompletedButton_Click(object sender, RoutedEventArgs e)
        {
            var task = (Task)((Button)sender).DataContext;
            task.IsCompleted = true;
            db.UpdateTask(task);
            LoadTasks();
        }

        private void MoveToPendingButton_Click(object sender, RoutedEventArgs e)
        {
            var task = (Task)((Button)sender).DataContext;
            task.IsCompleted = false;
            db.UpdateTask(task);
            LoadTasks();
        }
    }
}
