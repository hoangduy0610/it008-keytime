using IT008_KeyTime.Commons;
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
    public partial class AssignItem : Form
    {
        public AssignItem()
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

        public void fillDataDropdown(object items)
        {
            if (this.materialComboBox1.InvokeRequired)
            {
                this.materialComboBox1.Invoke(
                    new Action(() => fillDataDropdown(items))
                );
            }
            else
            {
                this.materialComboBox1.ValueMember = "id";
                this.materialComboBox1.DisplayMember = "name";
                this.materialComboBox1.DataSource = items;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var allItems = PostgresHelper.GetAll<Models.Item>();
            // get all assigned item from InventoryItem by join table with items and query where by inventoryPlanId
            int inventoryPlanId = Store._currentInventoryPlan.id;
            // console print inventoryPlanId
            Console.WriteLine(inventoryPlanId);
            string statement = $"SELECT tbl_items.* FROM tbl_items JOIN rls_inventory_items ON rls_inventory_items.item_id = tbl_items.id WHERE rls_inventory_items.inventory_plan_id = {inventoryPlanId}";
            var assignedItems = PostgresHelper.Query<Models.Item>(statement);
            assignedItems.ForEach(i => Console.WriteLine($"id={i.id}, name={i.name}"));
            // items = allItems filter by assignedItems
            var items = allItems.Where(item => assignedItems.All(assignedItem => assignedItem.id != item.id)).ToList();
            items.ForEach(i => Console.WriteLine($"id={i.id}, name={i.name}"));
            //var items = allItems.Where(item => !assignedItems.Any(assignedItem => assignedItem.id == item.id)).ToList();
            if (items == null && assignedItems == null)
            {
                MessageBox.Show("No data");
                HideLoading();
                return;
            }
            fillDataDropdown(items);

            // fill all assignedItems to dataGridView1
            UpdateDataGridViewSource(assignedItems);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            ShowLoading();
            this.materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            // add new InventoryItem
            var item = (Models.Item)materialComboBox1.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Please select an item");
                this.materialButton1.Enabled = true;
                Cursor.Current = Cursors.Default;
                HideLoading();
                return;
            }
            var inventoryItem = new Models.InventoryItem()
            {
                inventory_plan_id = Store._currentInventoryPlan.id,
                item_id = item.id,
                status = 0
            };
            var result = PostgresHelper.Insert(inventoryItem);
            this.materialButton1.Enabled = true;
            Cursor.Current = Cursors.Default;
            if (result != 0)
            {
                MessageBox.Show("Add item successfully");
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Add item failed");
                HideLoading();
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            // remove InventoryItem from dataGridView1 selected rows
            materialButton2.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            ShowLoading();
            // Delete selecting row in dataGridView1
            if (dataGridView1.SelectedRows.Count == 1)
            {
                // delete InventoryItem by item_id and inventory_plan_id
                var selectedRow = dataGridView1.SelectedRows[0];
                var item = selectedRow.DataBoundItem as Models.Item;
                var inventoryItem = PostgresHelper.Query<Models.InventoryItem>($"SELECT * FROM rls_inventory_items WHERE item_id = {item.id} AND inventory_plan_id = {Store._currentInventoryPlan.id}").FirstOrDefault();
                var result = PostgresHelper.Delete(inventoryItem);
                if (result == false)
                {
                    MessageBox.Show("Remove item failed");
                    this.materialButton2.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    HideLoading();
                    return;
                }
                this.materialButton2.Enabled = true;
                MessageBox.Show("Remove item successfully.");
                Cursor.Current = Cursors.Default;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please select one row");
                Cursor.Current = Cursors.Default;
                HideLoading();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // enable button if select at least one row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                materialButton2.Enabled = true;
            }
            else
            {
                materialButton2.Enabled = false;
            }
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
            this.Close();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IT008_KeyTime.Commons.MenuStripUtils.ExitCurForm(this);
        }
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            IT008_KeyTime.Commons.MenuStripUtils.ExitCurForm(this);
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IT008_KeyTime.Commons.MenuStripUtils.Help();        
        }
    }
}
