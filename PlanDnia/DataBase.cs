using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class DataBase
{
    private readonly string _connectionString;

    public DataBase(string dbFilePath)
    {
        _connectionString = $"Data Source={dbFilePath}";
    }

    public void CreateTable()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT,
                    Description TEXT,
                    Date DATETIME,
                    IsCompleted INTEGER
                );";
            command.ExecuteNonQuery();
        }
    }

    public void AddTask(Task task)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Tasks (Title, Description, Date, IsCompleted)
                VALUES (@Title, @Description, @Date, @IsCompleted);";
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Date", task.Date);
            command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
            command.ExecuteNonQuery();
        }
    }

    public List<Task> GetTasks()
    {
        var tasks = new List<Task>();
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tasks";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tasks.Add(new Task
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        Date = reader.GetDateTime(3),
                        IsCompleted = reader.GetInt32(4) == 1
                    });
                }
            }
        }
        return tasks;
    }

    public void UpdateTask(Task task)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Tasks
                SET Title = @Title, Description = @Description, Date = @Date, IsCompleted = @IsCompleted
                WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Date", task.Date);
            command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("@Id", task.Id);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteTask(int taskId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Tasks WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", taskId);
            command.ExecuteNonQuery();
        }
    }
}