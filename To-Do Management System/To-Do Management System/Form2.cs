using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace To_Do_Management_System
{
    public partial class Form2 : Form
    {
        private List<List<string>> userData;
        private List<List<string>> sortedUserData;
        private string userName;
        private bool allowClose = true;

        public Form2(List<List<string>> userData, string userName)
        {
            this.userData = userData;
            this.sortedUserData = userData.Select(innerList => new List<string>(innerList)).ToList();

            this.userName = userName;

            InitializeComponent();

            panel1.AutoScroll = true;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            label2.Text = userName + "'s to-do tasks";
            label2.Location = new Point(this.Width / 2 - label2.Width / 2, label2.Location.Y);

            InitTable();
        }

        private void InitTable()
        {
            if (userData.Count == 0)
            {
                tableLayoutPanel1.Visible = false;
                label1.Visible = true;
            }

            //Initial row and columns
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnCount = 7;

            //Styles
            // Set each column to equal percent, apply autosize and set border style
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / tableLayoutPanel1.ColumnCount));
            }
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // Create titles
            Label status = new Label
            {
                Text = "Status\n(Click to set)",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            Label title = new Label
            {
                Text = "Title",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            Label desc = new Label
            {
                Text = "Description",MinimumSize = new Size(0, 30),
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,

            };

            Label prioritry = new Label
            {
                Text = "Priority",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label deadline = new Label
            {
                Text = "Deadline\n(M/D/Y)",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label modify = new Label
            {
                Text = "Modify",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label delete = new Label
            {
                Text = "Delete",
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            tableLayoutPanel1.Controls.Add(status, 0, 0);
            tableLayoutPanel1.Controls.Add(title, 1, 0);
            tableLayoutPanel1.Controls.Add(desc, 2, 0);
            tableLayoutPanel1.Controls.Add(prioritry, 3, 0);
            tableLayoutPanel1.Controls.Add(deadline, 4, 0);
            tableLayoutPanel1.Controls.Add(modify, 5, 0);
            tableLayoutPanel1.Controls.Add(delete, 6, 0);

            // Add tasks as rows
            foreach(List<string> taskData in userData)
            {
                AddTask(taskData[1], taskData[2], taskData[3], taskData[4], taskData[5]);
            }
        }

        private void AddTask(string pending, string title, string desc, string priority, string deadline)
        {
            //Make table visible and get style data
            label1.Visible = false;
            tableLayoutPanel1.RowCount += 1;
            tableLayoutPanel1.Visible = true;
            int rows = tableLayoutPanel1.RowCount;
            int colWidth = tableLayoutPanel1.GetColumnWidths()[0];
            int rowHeight = 60;

            // Create a status label with click event
            Label pendingLabel = new Label
            {
                Text = pending,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                MinimumSize = new Size(0, rowHeight),
                ForeColor = pending.Equals("Pending") ? Color.DarkOrange : Color.Green
        };

            pendingLabel.Click += StatusChange;

            // Create a task name label
            Label titleLabel = new Label
            {
                Text = title,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                MaximumSize = new Size(colWidth - 5, 0),
            };

            // Create a task description label
            Label descLabel = new Label 
            {
                Text = desc,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                MaximumSize = new Size(colWidth - 5, 0)

            };  

            // Create a task label indicating priority
            Label prioritryLabel = new Label
            {
                Text = priority,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = ReturnPriorityColor(int.Parse(priority))
            };

            // Create a task label for the deadline
            Label deadlineLabel = new Label
            {
                Text = deadline,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            //Create the icons for delete/modify
            PictureBox delete = new PictureBox
            {
                Image = Properties.Resources.delete,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = colWidth - 8,
                Height = rowHeight - 8,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.None

            };

            delete.Click += DeleteTaskEvent;

            PictureBox modify = new PictureBox
            {
                Image = Properties.Resources.modify,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = colWidth - 8,
                Height = rowHeight - 8,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.None
            };

            modify.Click += ModifyTaskEvent;

            //Add the controls
            tableLayoutPanel1.Controls.Add(pendingLabel, 0, rows - 1);
            tableLayoutPanel1.Controls.Add(titleLabel, 1, rows - 1);
            tableLayoutPanel1.Controls.Add(descLabel, 2, rows - 1);
            tableLayoutPanel1.Controls.Add(prioritryLabel, 3, rows - 1);
            tableLayoutPanel1.Controls.Add(deadlineLabel, 4, rows - 1);
            tableLayoutPanel1.Controls.Add(modify, 5, rows - 1);
            tableLayoutPanel1.Controls.Add(delete, 6, rows - 1);

        }

        private Color ReturnPriorityColor(int priority)
        {
            Color color;

            if (priority >= 1 && priority <= 4) 
            {
                color = Color.Green;
            }
            else if(priority >= 5 && priority <= 8)
            {
                color = Color.DarkOrange;
            }
            else
            {
                color = Color.Red;
            }

            return color;
        }

        private void StatusChange(object sender, EventArgs e)
        {
            string text = ((Label)sender).Text;
            string newText = text.Equals("Pending") ? "Completed" : "Pending";
            Color newColor = newText.Equals("Pending") ? Color.DarkOrange : Color.Green;
            ((Label)sender).Text = newText;
            ((Label)sender).ForeColor = newColor;

            int sortedRow = tableLayoutPanel1.GetRow((Label)sender);
            string id = sortedUserData[sortedRow - 1][0];
            int defaultRow = userData.IndexOf(userData.Where(li => li[0].Equals(id)).First());

            sortedUserData[sortedRow - 1][1] = newText;
            userData[defaultRow][1] = newText;

            DbManager.ModifyStatusDb(id, newText);
        }

        private void DeleteTaskEvent(object sender, EventArgs e)
        {
            this.Hide();

            int taskRow = tableLayoutPanel1.GetRow((PictureBox)sender);
            string taskId = sortedUserData[taskRow - 1][0];

            // 1. Remove all controls in the row
            for (int i = 0; i <= tableLayoutPanel1.ColumnCount - 1; i++)
            {
                var control = tableLayoutPanel1.GetControlFromPosition(i, taskRow);
                tableLayoutPanel1.Controls.Remove(control);
                control.Dispose();

            }

            // 2. Move all rows below up by one
            for (int row = taskRow + 1; row <= tableLayoutPanel1.RowCount - 1; row++)
            {
                for (int col = 0; col <= tableLayoutPanel1.ColumnCount - 1; col++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(col, row);
                    tableLayoutPanel1.SetRow(control, row - 1);
                }
            }

            // 3. Decrease RowCount (also removes last remaining row)
            tableLayoutPanel1.RowCount -= 1;

            if (tableLayoutPanel1.RowCount == 1)
            {
                tableLayoutPanel1.Visible = false;
                label1.Visible = true;
            }

            DbManager.DeleteTaskDb(taskId);

            sortedUserData.RemoveAt(taskRow -1);
            int defTaskRow = userData.IndexOf(userData.Where(li => li[0].Equals(taskId)).First());
            userData.RemoveAt(defTaskRow);

            this.Show();
        }

        private void ModifyTaskEvent(object sender, EventArgs e)
        {
            allowClose = false;
            int modRow = tableLayoutPanel1.GetRow((PictureBox)sender);
            string modId = sortedUserData[modRow - 1][0];
            this.Close();
            new Form3(modId, userName, userData).Show();
        }


        private void addTodoTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allowClose = false;
            this.Close();
            new Form3("", userName, userData).Show();    
        }

        private void prioriyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            TableLayoutPanel dataTable = tableLayoutPanel1;
            string sortBy = ((ToolStripMenuItem)sender).Text;

            List<List<string>> data = null;

            this.Hide();

            // Apply chosen sort
            if (sortBy.Equals("Priority") && ((ToolStripMenuItem)sender).Checked)
            {
                dateToolStripMenuItem.Checked = false;
                sortedUserData = sortedUserData.OrderBy(li => int.Parse(li[4])).ToList();
                data = sortedUserData;
            }
            else if(sortBy.Equals("Priority") && !((ToolStripMenuItem)sender).Checked)
            {
                sortedUserData = userData.Select(innerList => new List<string>(innerList)).ToList();
                data = userData;
            }
            else if(sortBy.Equals("Date") && ((ToolStripMenuItem)sender).Checked)
            {
                prioriyToolStripMenuItem.Checked = false;
                sortedUserData = sortedUserData.OrderBy(li => DateTime.ParseExact(li[5], "MM/dd/yyyy", CultureInfo.InvariantCulture)).ToList();
                data = sortedUserData;
            }
            else if(sortBy.Equals("Date") && !((ToolStripMenuItem)sender).Checked)
            {
                sortedUserData = userData.Select(innerList => new List<string>(innerList)).ToList();
                data = userData;
            }
          
            //Remove old and add new sorted text with correct format
            for (int row = 1; row <= dataTable.RowCount - 1; row++)
            {
                for (int col = 0; col <= dataTable.ColumnCount - 3; col++)
                {
                    Control ctrl = dataTable.GetControlFromPosition(col, row);
                    ctrl.Text = data[row - 1][col + 1];                 
                    if (col == 0)
                        ctrl.ForeColor = ctrl.Text.Equals("Pending") ? Color.DarkOrange : Color.Green;
                    if (col == 3)
                        ctrl.ForeColor = ReturnPriorityColor(int.Parse(data[row - 1][4]));
                }
            }

            this.Show();
        }


        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(allowClose)
                Application.OpenForms[0].Show();
        }
    }
}
