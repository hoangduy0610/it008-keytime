using IT008_KeyTime.Commons;
using IT008_KeyTime.Models;
using MaterialSkin.Controls;
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
    public partial class ManageItem : Form
    {
        protected string _searchKeyword;
        public ManageItem()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }

        private void clearForm()
        {
            materialTextBox1.Text = "";
            materialTextBox2.Text = "";
            materialTextBox3.Text = "";
            materialTextBox4.Text = "";
            materialComboBox1.SelectedIndex = 0;
            materialMultiLineTextBox1.Text = "";
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            // search based on keyword input in materialMaskedTextbox2
            var keyword = materialMaskedTextBox2.Text;
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please input keyword to search.");
                materialMaskedTextBox2.Focus();
                return;
            }
            _searchKeyword = keyword;
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            // Reload data
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            // Add new item
            var name = materialTextBox2.Text;
            var room = materialTextBox3.Text;
            var description = materialTextBox4.Text;
            var status = materialComboBox1.SelectedIndex;
            var note = materialMultiLineTextBox1.Text;
            var item = new Item();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please input item's name.");
                materialTextBox2.Focus();
                materialButton1.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(room))
            {
                MessageBox.Show("Please input item's room.");
                materialTextBox3.Focus();
                materialButton1.Enabled = true;
                return;
            }
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please input item's description.");
                materialTextBox3.Focus();
                materialButton1.Enabled = true;
                return;
            }

            item.name = name;
            item.room = room;
            item.description = description;
            item.status = status;
            item.note = note;

            // save to database
            materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            PostgresHelper.Insert(item);
            Cursor.Current = Cursors.Default;
            materialButton1.Enabled = true;
            MessageBox.Show("Add new item successfully.");
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();

            // clear form
            clearForm();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            // Update item
            var id = materialTextBox1.Text;
            var name = materialTextBox2.Text;
            var room = materialTextBox3.Text;
            var description = materialTextBox4.Text;
            var status = materialComboBox1.SelectedIndex;
            var note = materialMultiLineTextBox1.Text;
            var item = new Item();
            item.id = int.Parse(id);
            item.name = name;
            item.room = room;
            item.description = description;
            item.status = status;
            item.note = note;
            materialButton4.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            PostgresHelper.Update(item);
            Cursor.Current = Cursors.Default;
            materialButton4.Enabled = true;
            MessageBox.Show("Update item successfully.");
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
            clearForm();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // Delete items
            materialButton3.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            ShowLoading();
            // Delete selecting row in dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // delete all selected rows
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    var item = row.DataBoundItem as Item;
                    if (item != null)
                    {
                        PostgresHelper.Delete(item);
                    }
                }
                MessageBox.Show("Delete item successfully.");
                Cursor.Current = Cursors.Default;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please select at least one row");
                HideLoading();
            }
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
                    var items = (List<Item>)data;
                    List<MapItem> mapItem = new List<MapItem>();


                    foreach (var item in items)
                    {
                        mapItem.Add(new MapItem(item));
                    }

                    this.dataGridView1.DataSource = mapItem;
                    //this.dataGridView1.Columns["id"].Visible = false;
                    //this.dataGridView1.Columns["user_id"].Visible = false;
                    //this.dataGridView1.Columns["item_id"].Visible = false;
                    //this.dataGridView1.Columns["rental_start"].Visible = false;
                    //this.dataGridView1.Columns["expect_return"].Visible = false;
                    //this.dataGridView1.Columns["actual_return"].Visible = false;
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
            // Search
            if (!string.IsNullOrEmpty(_searchKeyword))
            {
                // build string statement (name, room, description, note)
                var statement = "SELECT * FROM tbl_items WHERE name LIKE '%" + _searchKeyword + "%' OR room LIKE '%" + _searchKeyword + "%' OR description LIKE '%" + _searchKeyword + "%' OR note LIKE '%" + _searchKeyword + "%'";
                var items_search = PostgresHelper.Query<Item>(statement);
                UpdateDataGridViewSource(items_search);
                return;
            }

            // Get all items and fill to datagridview
            var items = PostgresHelper.GetAll<Item>();
            UpdateDataGridViewSource(items);
        }

        private void Item_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            materialButton3.Enabled = false;
            materialButton4.Enabled = false;
            clearForm();
            // check if datagridview no row selected, disable edit and delete button, clear form
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    // fill data from datagridview to form
                    var row = dataGridView1.SelectedRows[0];
                    materialTextBox1.Text = row.Cells["id"].Value?.ToString();
                    materialTextBox2.Text = row.Cells["name"].Value?.ToString();
                    materialTextBox3.Text = row.Cells["room"].Value?.ToString();
                    materialTextBox4.Text = row.Cells["description"].Value?.ToString();
                    materialComboBox1.SelectedIndex = int.Parse(row.Cells["status"].Value.ToString());
                    materialMultiLineTextBox1.Text = row.Cells["note"].Value?.ToString();

                    // enable edit and delete button
                    materialButton4.Enabled = true;
                }
                materialButton3.Enabled = true;
            }
        }

        private void materialComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
