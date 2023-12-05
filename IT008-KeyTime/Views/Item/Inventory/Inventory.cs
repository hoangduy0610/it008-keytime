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
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            // fetch data from database and assign to dataGridView1.DataSource
            var data = PostgresHelper.GetAll<InventoryPlan>();
            UpdateDataGridViewSource(data);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            // go to CreatePlan form
            var createPlan = new CreatePlan();
            this.Hide();
            createPlan.ShowDialog();
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
            this.Show();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            // go  to CreatePlan form
            var createPlan = new CreatePlan();
            Store._currentInventoryPlan = (InventoryPlan)dataGridView1.CurrentRow.DataBoundItem;
            this.Hide();
            createPlan.ShowDialog();
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
            this.Show();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // go to AssignItem
            var assignItem = new AssignItem();
            Store._currentInventoryPlan = (InventoryPlan)dataGridView1.CurrentRow.DataBoundItem;
            this.Hide();
            assignItem.ShowDialog();
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
            this.Show();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            // change status of InventoryPlan
            this.materialButton4.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            ShowLoading();
            var inventoryPlan = (InventoryPlan)dataGridView1.CurrentRow.DataBoundItem;
            if (inventoryPlan != null)
            {
                inventoryPlan.status = (int) InventoryPlanStatus.INPROGRESS;
                var result = PostgresHelper.Update(inventoryPlan);
                if (result)
                {
                    MessageBox.Show("Change status successfully");
                    Cursor.Current = Cursors.Default;
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Change status failed");
                    Cursor.Current = Cursors.Default;
                    HideLoading();
                }
            }
            else
            {
                MessageBox.Show("Please select an inventory plan");
                Cursor.Current = Cursors.Default;
                HideLoading();
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            // go to DoInventory form
            var doInventory = new DoInventory();
            Store._currentInventoryPlan = (InventoryPlan)dataGridView1.CurrentRow.DataBoundItem;
            this.Hide();
            doInventory.ShowDialog();
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
            this.Show();
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            // delete InventoryPlan
            this.materialButton6.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            ShowLoading();
            var inventoryPlan = (InventoryPlan)dataGridView1.CurrentRow.DataBoundItem;
            if (inventoryPlan != null)
            {
                var result = PostgresHelper.Delete(inventoryPlan);
                if (result)
                {
                    MessageBox.Show("Delete inventory plan successfully");
                    Cursor.Current = Cursors.Default;
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Delete inventory plan failed");
                    Cursor.Current = Cursors.Default;
                    HideLoading();
                }
            }
            else
            {
                MessageBox.Show("Please select an inventory plan");
                Cursor.Current = Cursors.Default;
                HideLoading();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // enable or disable buttons if choose an InventoryPlan
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var inventoryPlan = (InventoryPlan)dataGridView1.CurrentRow.DataBoundItem;
                if (inventoryPlan != null)
                {
                    materialButton2.Enabled = true;
                    materialButton6.Enabled = true;
                    if (inventoryPlan.status == (int) InventoryPlanStatus.INPROGRESS)
                    {
                        materialButton3.Enabled = false;
                        materialButton4.Enabled = false;
                        materialButton5.Enabled = true;
                    }
                    else if (inventoryPlan.status == (int) InventoryPlanStatus.DONE)
                    {
                        materialButton3.Enabled = false;
                        materialButton4.Enabled = false;
                        materialButton5.Enabled = false;
                    }
                    else
                    {
                        materialButton3.Enabled = true;
                        materialButton4.Enabled = true;
                        materialButton5.Enabled = false;
                    }
                }
            } else {
                materialButton2.Enabled = false;
                materialButton3.Enabled = false;
                materialButton4.Enabled = false;
                materialButton5.Enabled = false;
                materialButton6.Enabled = false;
            }
        }
    }
}
