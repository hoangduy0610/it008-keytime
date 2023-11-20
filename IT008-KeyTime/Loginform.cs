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
    public partial class Loginform : Form
    {
        public Loginform()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{
            
        //}

        private void materialButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            materialButton1.Enabled = false;
            var username = textBox1.Text;
            var password = textBox2.Text;
            var statement = "SELECT * FROM tbl_users WHERE username ='" + username + "'";
            var user = PostgresHelper.QueryFirst<User>(statement);
            if (user.password == password)
            {
                MessageBox.Show("Login success");
            }
            else
            {
                MessageBox.Show("Login failed");
            }
            this.Cursor = Cursors.Default;
            materialButton1.Enabled = true;
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {

        }

        private void Loginform_Load(object sender, EventArgs e)
        {

        }
    }
}
