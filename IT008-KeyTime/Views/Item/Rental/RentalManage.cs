using DocumentFormat.OpenXml.Office2010.CustomUI;
using IT008_KeyTime.Commons;
using IT008_KeyTime.Enums;
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

namespace IT008_KeyTime.Views.Item.Rental
{
    public partial class RentalManage : Form
    {
        public RentalManage()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync(); 
        }

        public void UpdateDataGridViewSource(object data)
        {
            if (data != null)
            {
                if (this.dataGridView1.InvokeRequired)
                {
                    this.dataGridView1.Invoke(new Action(() => UpdateDataGridViewSource(data)));
                }
                else
                {
                    this.dataGridView1.DataSource = data;
                    this.dataGridView1.Columns["status"].Visible = false;
                    this.dataGridView1.Columns["item_id"].Visible = false;
                    this.dataGridView1.Columns["user_id"].Visible = false;
                }
            }
            else
            {
                MessageBox.Show("No data");
            }
            HideLoading();
        }

        public void HideLoading()
        {
            if (this.pictureBox1.InvokeRequired)
            {
                this.pictureBox1.Invoke(new Action(() => HideLoading()));
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }

        public void ShowLoading()
        {
            if (this.pictureBox1.InvokeRequired)
            {
                this.pictureBox1.Invoke(new Action(() => ShowLoading()));
            }
            else
            {
                pictureBox1.Visible = true;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Load data from database
            var data = PostgresHelper.GetAll<RentalItem>();
            var items = PostgresHelper.GetAll<Models.Item>();
            var users = PostgresHelper.GetAll<User>();
            // create MapRentalItem list with item name and username
            List<MapRentalItem> mapRentalItems = new List<MapRentalItem>();
            foreach (var rental in data)
            {
                var item = items.Find(x => x.id == rental.item_id);
                var user = users.Find(x => x.id == rental.user_id);
                mapRentalItems.Add(new MapRentalItem(rental, item.name, user.name));
            }
            UpdateDataGridViewSource(mapRentalItems);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            User currentUser = IT008_KeyTime.Commons.Store._user;
            if (currentUser.role == 4)
            {
                materialButton2.Enabled = false;
                materialButton4.Enabled = false;
            }

            materialButton2.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            // update status of rental item to REJECTED
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (currentUser.role == 4)
                {
                    materialButton2.Enabled = false;
                    materialButton4.Enabled = false;
                }

                var selectedRows = dataGridView1.SelectedRows;
                foreach (DataGridViewRow row in selectedRows)
                {
                    var rental_item = row.DataBoundItem as RentalItem;
                    if (rental_item != null)
                    {
                        rental_item.status = (int) RentalStatusEnum.CANCELED;
                        PostgresHelper.Update(rental_item);

                        int itemID = rental_item.item_id;
                        var UpdateItemStatement = "SELECT * FROM tbl_items WHERE id ='" + itemID + "'";
                        var itemInDB = PostgresHelper.QueryFirst<IT008_KeyTime.Models.Item>(UpdateItemStatement);

                        itemInDB.status = 0;
                        PostgresHelper.Update(itemInDB);
                    }
                }
                MessageBox.Show("Rejected rental item request.");
                materialButton4.Enabled = false;
                Cursor.Current = Cursors.Default;
                ShowLoading();
                backgroundWorker1.RunWorkerAsync();

            }
            else
            {
                MessageBox.Show("Please select at least one row");
                HideLoading();
            }


            
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            User currentUser = IT008_KeyTime.Commons.Store._user;
            if (currentUser.role == 4)
            {
                materialButton2.Enabled = false;
                materialButton4.Enabled = false;
            }

            materialButton4.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            // update status of rental item to APPROVED
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (currentUser.role == 4)
                {
                    materialButton2.Enabled = false;
                    materialButton4.Enabled = false;
                }

                var selectedRows = dataGridView1.SelectedRows;
                foreach (DataGridViewRow row in selectedRows)
                {
                    var rental_item = row.DataBoundItem as RentalItem;
                    if (rental_item != null)
                    {
                        rental_item.status = (int) RentalStatusEnum.APPROVED;
                        PostgresHelper.Update(rental_item);
                    }
                }
                MessageBox.Show("Approved rental item request.");
                Cursor.Current = Cursors.Default;
                materialButton2.Enabled = false;
                ShowLoading();
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please select at least one row");
                HideLoading();
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            RequestRentalForm form = new RequestRentalForm();
            this.Hide();
            form.ShowDialog();
            this.Show();
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            materialButton2.Enabled = false;
            materialButton3.Enabled = false;
            materialButton4.Enabled = false;
            materialButton5.Enabled = false;

            if (dataGridView1.SelectedRows.Count == 1)
            {
                User currentUser = IT008_KeyTime.Commons.Store._user;
                if (currentUser.role == 4)
                {
                    materialButton2.Enabled = false;
                    materialButton4.Enabled = false;

                    return;
                }

                var selectedRow = dataGridView1.SelectedRows[0];
                var rental_request = selectedRow.DataBoundItem as RentalItem;

                if (rental_request.status == (int)RentalStatusEnum.REQUESTED)
                {
                    materialButton2.Enabled = true;
                    materialButton4.Enabled = true;
                }
                if (rental_request.status == (int)RentalStatusEnum.APPROVED)
                {
                    materialButton3.Enabled = true;
                    if (rental_request != null)
                        if (rental_request.expect_return <= DateTime.Now)
                            materialButton5.Enabled = true;
                }
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // prompt user to return item, if yes, update status of rental item to RETURNED and actual_return to current timestamp
            materialButton3.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                var rental_request = selectedRow.DataBoundItem as RentalItem;
                if (rental_request != null)
                {
                    var result = MessageBox.Show("Do you want to return this item?", "Return item", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        rental_request.status = (int) RentalStatusEnum.RETURNED;
                        rental_request.actual_return = DateTime.Now;
                        PostgresHelper.Update(rental_request);
                        MessageBox.Show("Return item successfully.");
                        materialButton5.Enabled = false;
                        materialButton3.Enabled = false;
                        ShowLoading();
                        backgroundWorker1.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Return item canceled.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select one row");
            }
            materialButton3.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            // go to GiaHan form
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                var rental_request = selectedRow.DataBoundItem as RentalItem;
                if (rental_request != null)
                {
                    Store._currentRentalItem = rental_request;
                    ExtendForm form = new ExtendForm();
                    this.Hide();
                    form.ShowDialog();
                    this.Show();
                    materialButton5.Enabled = false;
                    materialButton3.Enabled = false;
                    ShowLoading();
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            else
            {
                MessageBox.Show("Please select one row");
            }
        }

        private void logoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.LogOut();
            this.Show();
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void changePasswordToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.ChangePassword();
            this.Show();
        }
    }
}
