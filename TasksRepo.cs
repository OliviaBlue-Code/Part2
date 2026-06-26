using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace demo
{
    public class TasksRepo
    {
        // data source or what connects the database to the project
        public readonly string connection =
           @"Data Source= (localdb)\MSSQLLocalDB;
             Initial Catalog=prog_tasks;
             Integrated Security=True";

        public TasksRepo() { }

        // XAML.cs calls: repo.AddTask(currentUser, title, date)
        public void AddTask(string username, string title, DateTime? reminderDate)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                // Map to your DB: demo_tasks(task_name, task_description, task_dueDate, task_status)
                string query = @"INSERT INTO demo_tasks(username,task_name, task_description, task_dueDate, task_status) 
                                 VALUES(@User,@Title, @Description, @DueDate, 'pending')";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@User", username);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", title);
                command.Parameters.AddWithValue(
                    "@DueDate",
                    reminderDate.HasValue ? (object)reminderDate.Value: DBNull.Value
                );
                command.ExecuteNonQuery();
            }
        }

        // XAML.cs calls: repo.GetTasks(currentUser) -> List<CyberTask>
        public List<CyberTask> GetTasks(string username)
        {
            List<CyberTask> tasks = new List<CyberTask>();
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string query = "SELECT task_id, task_name, task_description, task_dueDate, task_status FROM demo_tasks WHERE username = ORDER BY task_id DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@User", username);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new CyberTask
                    {
                        TaskId = Convert.ToInt32(reader["task_id"]),
                        Title = reader["task_name"].ToString(),
                        Description = reader["task_description"].ToString(),
                        Username = reader["username"].ToString(), //adding username to database
                        ReminderDate = reader["task_dueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["task_dueDate"]),
                        IsCompleted = reader["task_status"].ToString().ToLower() == "done"
                    });
                }
                }return tasks;
            }
            

        // XAML.cs calls: repo.DeleteTask(taskId)
        public void DeleteTask(int taskId, string username)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM demo_tasks WHERE task_id = @TaskId AND username = @User", conn);
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                cmd.Parameters.AddWithValue("@User", username);
                cmd.ExecuteNonQuery();
            }
        }

        // Keep for your old Reminders tab
        public void load_tasks(ListView view_tasks, string username)
        {
            view_tasks.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT task_id, task_name, task_dueDate, task_status FROM demo_tasks WHERE username = @User ORDER BY task_dueDate", conn);
                cmd.Parameters.AddWithValue("@User", username);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string status = rdr["task_status"].ToString().ToUpper();
                    string date = rdr["task_dueDate"] == DBNull.Value ? "No date" : Convert.ToDateTime(rdr["task_dueDate"].ToString()).ToString("dd MMM HH:mm");
                    view_tasks.Items.Add($"{rdr["task_id"]}: {rdr["task_name"]} | {date} | {status}");
                }
            }
        }

        public void update_taskStatus(int taskId, string username)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE demo_tasks SET task_status='done' WHERE task_id=@TaskId AND username = @User", conn);
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                cmd.Parameters.AddWithValue ("username", username);
                cmd.ExecuteNonQuery();
            }
        }
    }
}