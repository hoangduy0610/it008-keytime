using System;
using IT008_KeyTime.Commons;
using IT008_KeyTime.Models;
using IT008_KeyTime.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms.Helpers.Transitions;

namespace IT008_KeyTime.Views.Changepassword
{
    public partial class Changepassword : Form
    {
        public Changepassword()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string OLDPASSWORD = textBox1.Text;
            string NEWPASSWORD = textBox2.Text;
            string CONFIRM = textBox3.Text;
            User currentUser = IT008_KeyTime.Commons.Store._user;

            if (OLDPASSWORD == null)
            {
                MessageBox.Show("Please enter old password");
                textBox1.Focus();       
                return;
            }
            if (NEWPASSWORD == null)
            {
                MessageBox.Show("Please enter new password");
                textBox2.Focus();
                return;
            }
            if (CONFIRM == null)
            {
                MessageBox.Show("Please enter old password");
                textBox3.Focus();
                return;
            }

            string oldHashed = IT008_KeyTime.Commons.Bcrypt.CreateMD5(OLDPASSWORD);
            string curUserPassword = (currentUser.password);

            if (oldHashed != curUserPassword) 
            {
                MessageBox.Show("Old password is incorrect");
                textBox1.Focus();   
                return;
            }
            if (NEWPASSWORD != CONFIRM)
            {
                MessageBox.Show("Wrong comfirm password! Please check.");
                textBox3.Focus();       
                return;
            }

            var statement = "SELECT * FROM tbl_users WHERE username ='" + currentUser.username + "'";
            var userInDB = PostgresHelper.QueryFirst<User>(statement);
            Console.WriteLine(userInDB);        
            userInDB.password = IT008_KeyTime.Commons.Bcrypt.CreateMD5(NEWPASSWORD); ;
            PostgresHelper.Update(userInDB);

            MessageBox.Show("Change password successfully.");

            this.Close();
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { 
                button1_Click(sender, e);       
            }
        }
    }
}
