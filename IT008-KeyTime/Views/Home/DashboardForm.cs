using IT008_KeyTime.Commons;
using IT008_KeyTime.Enums;
using IT008_KeyTime.Views.Item.Rental;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT008_KeyTime.Views
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            RentalManage form = new RentalManage();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            ManageItem form = new ManageItem();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loginform form = new Loginform();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            switch(Store._user.role)
            {
                case (int) Roles.Admin:
                    break;
                case (int) Roles.PropertyManager:
                    materialButton1.Enabled = false;
                    break;
                case (int) Roles.InventoryManager:
                    materialButton1.Enabled = false;
                    materialButton2.Enabled = false;
                    materialButton4.Enabled = false;
                    break;
                case (int) Roles.User:
                    materialButton1.Enabled = false;
                    materialButton2.Enabled = false;
                    materialButton3.Enabled = false;
                    break;
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            ManageUser form = new ManageUser();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }
    }
}
