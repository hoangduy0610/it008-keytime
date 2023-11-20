using IT008_KeyTime.Commons;
using IT008_KeyTime.Models;
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
    public partial class Registrationform : Form
    {
        public Registrationform()
        {
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            var username = materialTextBox1.Text;
            var password = materialTextBox2.Text;
            var name = materialTextBox4.Text;
            var email = materialTextBox3.Text;
            var phone = materialTextBox6.Text;
            var address = materialTextBox5.Text;
            var role = materialComboBox1.SelectedIndex + 1;
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please input your username.");
                materialTextBox1.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please input your password.");
                materialTextBox2.Focus();
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please input your name.");
                materialTextBox4.Focus();
                return;
            }
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please input your email.");
                materialTextBox3.Focus();
                return;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Please input your phone.");
                materialTextBox6.Focus();
                return;
            }
            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please input your address.");
                materialTextBox5.Focus();
                return;
            }
            var newUser = new User();
            newUser.username = username;
            newUser.password = password;
            newUser.name = name;
            newUser.email = email;
            newUser.phone = phone;
            newUser.address = address;
            newUser.role = role;

            PostgresHelper.Insert(newUser);

            MessageBox.Show("Registration success");
            // clear data
            materialComboBox1.SelectedIndex = 0;
            materialTextBox1.Text = "";
            materialTextBox2.Text = "";
            materialTextBox3.Text = "";
            materialTextBox4.Text = "";
            materialTextBox5.Text = "";
            materialTextBox6.Text = "";
            materialTextBox1.Focus();
            materialButton1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //this.Close();
        }
    }
}
