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

            if (Store._currentEditing != null)
            {
                // Update User
                Store._currentEditing.username = username;
                Store._currentEditing.password = password;
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
    }
}
