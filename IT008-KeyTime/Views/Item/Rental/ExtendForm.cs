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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IT008_KeyTime
{
    public partial class ExtendForm : Form
    {
        public ExtendForm()
        {
            InitializeComponent();
            // get data from store and set to form
            var item = Store._currentRentalItem;
            textBox1.Text = item.id.ToString();
            dateTimePicker1.Value = item.expect_return;
        }
        private void materialButton1_Click(object sender, EventArgs e)
        {
            materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            // save new expect_return for rental item
            var id = int.Parse(textBox1.Text);
            var expect_return = dateTimePicker1.Value;
            var rentalItem = Store._currentRentalItem;
            rentalItem.expect_return = expect_return;
            var result = PostgresHelper.Update<RentalItem>(rentalItem);
            if (result)
            {
                MessageBox.Show("Extend successfully");
                this.Close();
            }
            else
            {
                MessageBox.Show("Extend failed");
                this.Close();
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.LogOut();
            this.Show();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.ChangePassword();
            this.Show();
        }
    }
}
