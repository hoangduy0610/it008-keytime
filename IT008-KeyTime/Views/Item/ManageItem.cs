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
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

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

            // Ask for confirmation
            DialogResult result = MessageBox.Show("Update this item?", "Confirmation", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                PostgresHelper.Update(item);
                MessageBox.Show("Update item successfully.");
                ShowLoading();
                backgroundWorker1.RunWorkerAsync();
                clearForm();
            }

            Cursor.Current = Cursors.Default;
            materialButton4.Enabled = true;
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // Delete items
            materialButton3.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            // Delete selecting row in dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Ask for confirmation
                DialogResult result = MessageBox.Show("Delete this item?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    // Delete all selected rows
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        var item = row.DataBoundItem as Item;
                        if (item != null)
                        {
                            PostgresHelper.Delete(item);
                        }
                    }
                    MessageBox.Show("Delete item successfully.");
                    ShowLoading();
                    Cursor.Current = Cursors.Default;
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    HideLoading();
                }
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
                    this.dataGridView1.Columns["status"].Visible = false;
                }
            }
            else
            {
                MessageBox.Show("No data");
            }
            HideLoading();
        }

        public void ReloadAfterImport()
        {
            if (this.dataGridView1.InvokeRequired)
            {
                this.dataGridView1.Invoke(new Action(() => ReloadAfterImport()));
            }
            else
            {
                backgroundWorker1.RunWorkerAsync();
                clearForm();
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
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            // Lấy tham số truyền vào từ RunWorkerAsync
            string filePath = e.Argument as string;

            // Thực hiện công việc với filePath ở đây
            List<Item> items = ReadExcelFile(filePath);

            foreach (var item in items)
            {
                Console.WriteLine(item.name);
                PostgresHelper.Insert(item);
            }

            // Gọi hàm cập nhật UI nếu cần thiết
            ReloadAfterImport();
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

        private void materialButton6_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                ShowLoading();
                materialButton6.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
                backgroundWorker2.RunWorkerAsync(filePath);
            }
        }
        private List<Item> ReadExcelFile(string filePath)
        {
            List<Item> items = new List<Item>();

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.Worksheets[1];

            bool flag = false;
            foreach (Excel.Range row in worksheet.UsedRange.Rows)
            {
                if (flag == false)
                {
                    flag = true;        
                    continue;
                }

                Item item = new Item();
                item.name = row.Cells[2].Value.ToString();
                item.room = row.Cells[3].Value.ToString();
                item.description = row.Cells[4].Value.ToString();
                string Status = row.Cells[5].Value.ToString();
                // switch case status from string to int
                switch (Status)
                {
                    case "IDLE":
                        item.status = 0;
                        break;
                    case "IN USE":
                        item.status = 1;
                        break;
                    case "BROKEN":
                        item.status = 2;
                        break;
                    case "LOST":
                        item.status = 3;
                        break;
                    default:
                        item.status = 0;
                        break;
                }

                Console.WriteLine(Status, item.status);
                item.note = row.Cells[6].Value.ToString();
                items.Add(item);
            }

            workbook.Close();
            excelApp.Quit();
            return items;
        }
        private void logoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            IT008_KeyTime.Commons.MenuStripUtils.LogOut();
            this.Show();
        }
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            IT008_KeyTime.Commons.MenuStripUtils.ExitCurForm(this);
        }
        private void changePasswordToolStripMenuItem_Click_1(object sender, EventArgs e)
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
