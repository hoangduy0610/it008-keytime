using IT008_KeyTime.Commons;
using IT008_KeyTime.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;


namespace IT008_KeyTime
{
    public partial class Registrationform : Form
    {
        public Registrationform()
        {
            InitializeComponent();
            if (Store._currentEditing != null)
            {
                materialButton1.Text = "Update";
                materialTextBox1.Text = Store._currentEditing.username;
                materialTextBox2.Text = Store._currentEditing.password;
                materialTextBox3.Text = Store._currentEditing.email;
                materialTextBox4.Text = Store._currentEditing.name;
                materialTextBox5.Text = Store._currentEditing.address;
                materialTextBox6.Text = Store._currentEditing.phone;
                materialComboBox1.SelectedIndex = Store._currentEditing.role - 1;

                this.Text = "Update User";
            }
        }
        static bool IsEmailValid(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(\.[a-zA-Z]{2,})?$";
            // Kiểm tra sự khớp giữa địa chỉ email và biểu thức chính quy
            return Regex.IsMatch(email, pattern);
        }
        static bool IsPhoneNumberValid(string phoneNumber)
        {
            // Biểu thức chính quy để kiểm tra số điện thoại
            string pattern = @"^(03[2-9]|05[2-9]|07[0-9]|08[1-9]|09[0-9]|01[2-9])[0-9]{7}$";
            // Kiểm tra sự khớp giữa số điện thoại và biểu thức chính quy
            return Regex.IsMatch(phoneNumber, pattern);
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
                materialButton1.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please input your password.");
                materialTextBox2.Focus();
                materialButton1.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please input your name.");
                materialTextBox4.Focus();
                materialButton1.Enabled = true;
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please input your email.");
                materialTextBox3.Focus();
                materialButton1.Enabled = true;
                return;
            }
            if (!IsEmailValid(email))
            {
                MessageBox.Show("Email is invalid.");
                materialTextBox3.Focus();
                materialButton1.Enabled = true;
                return; 
            }


            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Please input your phone.");
                materialTextBox6.Focus();
                materialButton1.Enabled = true;
                return;
            }
            if (!IsPhoneNumberValid(phone))
            {
                MessageBox.Show("Phone number is invalid.");
                materialTextBox6.Focus();
                materialButton1.Enabled = true;
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please input your address.");
                materialTextBox5.Focus();
                materialButton1.Enabled = true;
                return;
            }

            if (Store._currentEditing != null)
            {
                // Update User
                Store._currentEditing.username = username;

                string hashedPassword = IT008_KeyTime.Commons.Bcrypt.CreateMD5(password);
                Store._currentEditing.password = hashedPassword;

                Store._currentEditing.name = name;
                Store._currentEditing.email = email;
                Store._currentEditing.phone = phone;
                Store._currentEditing.address = address;
                Store._currentEditing.role = role;
                PostgresHelper.Update(Store._currentEditing);
                MessageBox.Show("Update success");
            }
            else
            {
                var newUser = new User();
                newUser.username = username;
                
                newUser.name = name;
                newUser.email = email;
                newUser.phone = phone;
                newUser.address = address;
                newUser.role = role;

                string hashedPassword = IT008_KeyTime.Commons.Bcrypt.CreateMD5(password);
                newUser.password = hashedPassword;

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
            }
            materialTextBox1.Focus();
            materialButton1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }
        private void Registrationform_FormClosing(object sender, FormClosingEventArgs e)
        {
            // clear editing user
            Store._currentEditing = null;
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.ChangePassword();
            this.Show();
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.LogOut();
            this.Show();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IT008_KeyTime.Commons.MenuStripUtils.ExitCurForm(this);
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IT008_KeyTime.Commons.MenuStripUtils.Help();
        }
    }
}
