using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace To_Do_Management_System
{
    public static class DbManager
    {
        private static string connectionString;

        public static bool SetConstr(string server, string user, string password)
        {
            connectionString = "server="+ server + ";user="+user+";password="+ password + ";database=tododb;";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong during database connection. Please check your information and try again");
                return false;
            }
        }

        public static List<List<string>> Login(string userName, string password)
        {
            List<List<string>> userData = new List<List<string>>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {

                    conn.Open();

                    string query = "SELECT username from users where username = @un and password = @pw";

                    MySqlCommand cmd1 = new MySqlCommand(query, conn);

                    cmd1.Parameters.AddWithValue("@un", userName);
                    cmd1.Parameters.AddWithValue("@pw", password);

                    MySqlDataReader reader1 = cmd1.ExecuteReader();

                    // If user does not exist
                    if (!reader1.HasRows)
                    {
                        MessageBox.Show("Wrong user information, please try again.");
                        conn.Close();
                        reader1.Close();
                        return null;
                    }

                    reader1.Close();

                    //if user exists retrieve data

                    string query2 = "SELECT * from tasks where username = @un";

                    MySqlCommand cmd2 = new MySqlCommand(query2, conn);
                    cmd2.Parameters.AddWithValue("@un", userName);

                    MySqlDataReader reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        userData.Add(new List<string> {reader2[0].ToString(), reader2[2].ToString(), reader2[3].ToString(), reader2[4].ToString(), reader2[5].ToString(), ((DateTime)reader2[6]).ToString("MM/dd/yyyy") });
                    }

                    reader2.Close();
                }

                return userData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error During login process: " + ex.Message);
                return null;
            }
        }

        public static void Register(string userName, string password1, string password2)
        {
            if (!password1.Equals(password2))
            {
                MessageBox.Show("Password missmatch, please try again");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {

                    conn.Open();

                    //Check if username already exists
                    string checkquery = "SELECT username FROM users WHERE username = @un";

                    MySqlCommand cmd1 = new MySqlCommand(checkquery, conn);
                    cmd1.Parameters.AddWithValue("@un", userName);

                    MySqlDataReader reader = cmd1.ExecuteReader();

                    if (reader.Read())
                    {
                        MessageBox.Show("This username already exists try another one.");
                        reader.Close();
                        conn.Close();

                        return;
                    }

                    reader.Close();

                    //If not add user to db
                    string query = "INSERT INTO users (username, password) VALUES (@un, @pw)";

                    MySqlCommand cmd2 = new MySqlCommand(query, conn);

                    cmd2.Parameters.AddWithValue("@un", userName);
                    cmd2.Parameters.AddWithValue("@pw", password1);

                    cmd2.ExecuteNonQuery();

                }

                MessageBox.Show("User registration was successful!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error During register process: " + ex.Message);
            }
        }

        public static string InsertTaskDb(string userName, string pending, string title, string desc, string priority, string deadline)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // insert new task to db
                    string query = "INSERT INTO tasks (`username`, `status`, `title`, `descr`, `priority`, `deadline`) VALUES (@un, @stat, @title, @desc, @priority, @dl)";

                    MySqlCommand cmd1 = new MySqlCommand(query, conn);

                    cmd1.Parameters.AddWithValue("@un", userName);
                    cmd1.Parameters.AddWithValue("@stat", pending);
                    cmd1.Parameters.AddWithValue("@title", title);
                    cmd1.Parameters.AddWithValue("@desc", desc);
                    cmd1.Parameters.AddWithValue("@priority", priority);
                    cmd1.Parameters.AddWithValue("@dl", DateTime.ParseExact(deadline, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));

                    cmd1.ExecuteNonQuery();

                    //get the new generated id and return it
                    return cmd1.LastInsertedId.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not add task info into database: " + ex.Message);
                return null;
            }
        }

        public static void ModifyTaskDb(string id, string newTitle, string newDesc, string newPriority, string newDl)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // delete task with given id
                    string query = "UPDATE tasks SET title=@newtitle, descr=@newdesc, priority=@newpr, deadline=@newdl WHERE id = @id";

                    MySqlCommand cmd1 = new MySqlCommand(query, conn);

                    cmd1.Parameters.AddWithValue("@newtitle", newTitle);
                    cmd1.Parameters.AddWithValue("@newdesc", newDesc);
                    cmd1.Parameters.AddWithValue("@newpr", newPriority);
                    cmd1.Parameters.AddWithValue("@newdl", DateTime.ParseExact(newDl, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
                    cmd1.Parameters.AddWithValue("@id", id);

                    cmd1.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update task info from database: " + ex.Message);
            }
        }

        public static void ModifyStatusDb(string id, string newStatus)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // delete task with given id
                    string query = "UPDATE tasks SET status=@newstatus WHERE id = @id";

                    MySqlCommand cmd1 = new MySqlCommand(query, conn);

                    cmd1.Parameters.AddWithValue("@newstatus", newStatus);
                    cmd1.Parameters.AddWithValue("@id", id);

                    cmd1.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update task status from database: " + ex.Message);
            }
        }

        public static void DeleteTaskDb(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // delete task with given id
                    string query = "DELETE FROM tasks WHERE id = @id";

                    MySqlCommand cmd1 = new MySqlCommand(query, conn);

                    cmd1.Parameters.AddWithValue("@id", id);
                    cmd1.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete task info from database: " + ex.Message);
            }
        }
    }
}
