using IT008_KeyTime.Commons;
using IT008_KeyTime.Models;
using IT008_KeyTime.Views;
using IT008_KeyTime.Views.Forgotpassword;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT008_KeyTime
{
    public partial class Loginform : Form
    {
        public Loginform()
        {
            InitializeComponent();
            materialButton1.Enabled = false; 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckLoginButtonEnable();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CheckLoginButtonEnable();
            textBox2.PasswordChar = '*';
        }
        private void CheckLoginButtonEnable()
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                materialButton1.Enabled = true;
                return;
            }
            materialButton1.Enabled = false;

        }
        private void materialButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            materialButton1.Enabled = false;
            var username = textBox1.Text;
            var password = textBox2.Text;

            string hashedPassword = IT008_KeyTime.Commons.Bcrypt.CreateMD5(password);
            var statement = "SELECT * FROM tbl_users WHERE username ='" + username + "'";
            var user = PostgresHelper.QueryFirst<User>(statement);
            if (user.password == hashedPassword)
            {
                MessageBox.Show("Login success");
                Store._user = user;
                DashboardForm form = new DashboardForm();
                this.Hide();
                form.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong password");
            }
            this.Cursor = Cursors.Default;
            materialButton1.Enabled = true;
        }

        private void Loginform_Load(object sender, EventArgs e)
        {

        }
        private void Loginform_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Gọi sự kiện click của nút đăng nhập
                materialButton1_Click(sender, e);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Gọi sự kiện click của nút đăng nhập
                materialButton1_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
            {
                button2.BringToFront();
                textBox2.PasswordChar = '\0';
            }
            else
            {
                button1.BringToFront();
                textBox2.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '*')
            {
                button2.BringToFront();
                textBox2.PasswordChar = '\0';
            }
            else
            {
                button1.BringToFront();
                textBox2.PasswordChar = '*';
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Resetpassword resetpassword = new Resetpassword();
            resetpassword.Show();
        }
    }
}
