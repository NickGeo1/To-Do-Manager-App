using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace To_Do_Management_System
{
    public partial class TaskDetails : Form
    {
        private string taskTitle;
        private string taskDesc;
        private decimal taskPriority;
        private string taskDeadline;

        private string modId;
        private string userName;
        private List<List<string>> userData;

        public TaskDetails(string modId, string userName, List<List<string>> userData)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            dateTimePicker1.MinDate = DateTime.Today;

            this.modId = modId;
            this.userName = userName;
            this.userData = userData;

            if (!modId.Equals(""))
            {
                int modRow = userData.IndexOf(userData.Where(li => li[0].Equals(modId)).First());
                textBox1.Text = userData[modRow][2];
                richTextBox1.Text = userData[modRow][3];
                numericUpDown1.Value = decimal.Parse(userData[modRow][4]);
                DateTime date = DateTime.ParseExact(userData[modRow][5], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime today = DateTime.ParseExact(DateTime.Today.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                dateTimePicker1.Value = date < today ?  today : date;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            taskTitle = textBox1.Text;
            taskDesc = richTextBox1.Text;
            taskPriority = numericUpDown1.Value;
            taskDeadline = dateTimePicker1.Value.ToString("MM/dd/yyyy");

            if(!modId.Equals(""))
            {
                DbManager.ModifyTaskDb(modId, taskTitle, taskDesc, taskPriority.ToString(), taskDeadline);
                int modrow = userData.IndexOf(userData.Where(li => li[0].Equals(modId)).First());
                userData[modrow] = new List<string> {userData[modrow][0], userData[modrow][1], taskTitle, taskDesc, taskPriority.ToString(), taskDeadline };
            }
            else
            {
                string id = DbManager.InsertTaskDb(userName, "Pending", taskTitle, taskDesc, taskPriority.ToString(), taskDeadline);

                if(id == null)
                {
                    MessageBox.Show("Could not add new task to the table");
                    return;
                }

                userData.Add(new List<string> {id, "Pending", taskTitle, taskDesc, taskPriority.ToString(), taskDeadline });
            }
            
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Tasks(userData, userName).Show();
        }
    }
}
