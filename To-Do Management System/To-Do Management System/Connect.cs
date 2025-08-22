using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace To_Do_Management_System
{
    public partial class Todo_App : Form
    {
        public Todo_App()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.panel1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool connected = DbManager.SetConstr(textBox8.Text, textBox7.Text, textBox6.Text);
            this.panel1.Visible = connected;
        }

        private void loginbtn_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals(""))
            {
                MessageBox.Show("Please provide all the required information");
                return;
            }

            List<List<string>> userData;

            userData = DbManager.Login(textBox1.Text, textBox2.Text);

            if (userData == null)
                return;

            this.Hide();
            new Tasks(userData, textBox1.Text).Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals("") || textBox4.Text.Equals("") || textBox5.Text.Equals(""))
            {
                MessageBox.Show("Please provide all the required information");
                return;
            }

            DbManager.Register(textBox4.Text, textBox3.Text, textBox5.Text);
        }
    }

}
