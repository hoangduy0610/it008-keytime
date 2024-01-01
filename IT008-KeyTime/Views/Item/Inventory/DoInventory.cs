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

        public void UpdateDataGridViewSource(object data, object data2)
        {
            if (data != null)
            {
                if (this.dataGridView1.InvokeRequired)
                {
                    this.dataGridView1.Invoke(new Action(() => UpdateDataGridViewSource(data, data2)));
                }
                else
                {
                  
                    var inventoryItems = (List<Models.Item>)data;
                    var inventoryItems2 = (List<Models.InventoryItem>)data2;
                    List<MapItem> mapInventoryItems = new List<MapItem>();

                    foreach (var inventoryItem in inventoryItems)
                    {
                        // find inventory item with item id
                        var inventoryItem2 = inventoryItems2.Find(x => x.item_id == inventoryItem.id);
                        if (inventoryItem2 != null)
                        {
                            mapInventoryItems.Add(new MapItem(inventoryItem, inventoryItem2, true));
                        }
                    }

                    this.dataGridView1.DataSource = mapInventoryItems;
                    this.dataGridView1.Columns["status"].Visible = false;
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
            Console.WriteLine(Store._currentInventoryPlan.id);
            var statement = $"SELECT tbl_items.* FROM tbl_items JOIN rls_inventory_items ON tbl_items.id = rls_inventory_items.item_id WHERE rls_inventory_items.inventory_plan_id = {Store._currentInventoryPlan.id}";
            var data = PostgresHelper.Query<Models.Item>(statement);
            var statement2 = $"SELECT * FROM rls_inventory_items WHERE inventory_plan_id = {Store._currentInventoryPlan.id}";
            var data2 = PostgresHelper.Query<Models.InventoryItem>(statement2);
            UpdateDataGridViewSource(data, data2);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            // show dialog ReportItem to report item, pass selected item to ReportItem
            ShowLoading();
            this.materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Console.WriteLine(Store._currentInventoryPlan.id);
            var statement = $"SELECT * FROM rls_inventory_items WHERE inventory_plan_id = {Store._currentInventoryPlan.id} AND item_id = {(dataGridView1.SelectedRows[0].DataBoundItem as MapItem).id}";
            var data = PostgresHelper.Query<InventoryItem>(statement).FirstOrDefault();
            //Console.WriteLine(data.id);
            Console.WriteLine(statement);
            Store._currentInventoryItem = data;
            var reportItem = new ReportItem();
            this.Hide();
            reportItem.ShowDialog();
            backgroundWorker1.RunWorkerAsync();
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
            IT008_KeyTime.Commons.MenuStripUtils.ExitCurForm(this);
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.ChangePassword();
            this.Show();
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IT008_KeyTime.Commons.MenuStripUtils.Help();
        }
    }
}
