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

namespace IT008_KeyTime.Views.Item.Inventory
{
    public partial class DoInventory : Form
    {
        public DoInventory()
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
            // load data from database based on store current inventory plan id
            var statement = $"SELECT * FROM tbl_items JOIN rls_inventory_items ON tbl_items.id = rls_inventory_items.item_id WHERE rls_inventory_items.inventory_plan_id = {Store._currentInventoryPlan.id}";
            var data = PostgresHelper.Query<InventoryItemWithItem>(statement);
            UpdateDataGridViewSource(data);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            // show dialog ReportItem to report item, pass selected item to ReportItem
            ShowLoading();
            this.materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            var statement = $"SELECT * FROM rls_inventory_items WHERE inventory_plan_id = {Store._currentInventoryPlan.id} AND item_id = {(dataGridView1.SelectedRows[0].DataBoundItem as Models.InventoryItemWithItem).item_id}";
            var data = PostgresHelper.Query<InventoryItem>(statement).FirstOrDefault();
            Store._currentInventoryItem = data;
            var reportItem = new ReportItem();
            this.Hide();
            reportItem.ShowDialog();
            HideLoading();
            this.materialButton1.Enabled = true;
            Cursor.Current = Cursors.Default;
            this.Show();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // update status of inventory plan to 1 (done)
            ShowLoading();
            this.materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            var inventoryPlan = PostgresHelper.GetById<InventoryPlan>(Store._currentInventoryPlan.id);
            inventoryPlan.status = (int) InventoryPlanStatus.DONE;
            var result = PostgresHelper.Update<InventoryPlan>(inventoryPlan);
            if (result)
            {
                MessageBox.Show("Done");
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed");
                HideLoading();
                this.materialButton1.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                materialButton1.Enabled = true;
                materialButton2.Enabled = true;
            }
            else
            {
                materialButton1.Enabled = false;
                materialButton2.Enabled = false;
            }
        }
        private void materialButton2_Click(object sender, EventArgs e)
        {
            // go to QRScan form
            var qrScan = new QRScan();
            this.Hide();
            qrScan.ShowDialog();
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
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
