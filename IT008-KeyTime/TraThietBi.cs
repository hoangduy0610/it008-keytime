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
    public partial class TraThietBi : Form
    {
        public string status;
        public TraThietBi()
        {
            InitializeComponent();

            // Tạo DataTable và định nghĩa cấu trúc
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Tên Thiết Bị", typeof(string));
            dataTable.Columns.Add("Trạng Thái", typeof(string));
            dataTable.Columns.Add("Ngày Trả Dự Kiến", typeof(string));

            // Thêm dữ liệu vào DataTable
            dataTable.Rows.Add(1, "Thiết Bị 1", "Đã thuê", "25/10/2023");
            dataTable.Rows.Add(2, "Thiết Bị 2", "Chưa thuê", "-");
            dataTable.Rows.Add(3, "Thiết Bị 3", "Đã thuê", "7/11/2023");

            // Liên kết DataGridView với DataTable
            dataGridView1.DataSource = dataTable;

            // Gắn sự kiện CellContentClick
            dataGridView1.CellClick += dataGridView1_CellContentClick;

            dataGridView1.RowTemplate.Height = 40; // Chiều cao hàng
            dataGridView1.Columns["ID"].Width = 80; // Chiều rộng cột "ID"
            dataGridView1.Columns["Tên Thiết Bị"].Width = 60; // Chiều rộng cột "Tên Thiết Bị"
            dataGridView1.Columns["Trạng Thái"].Width = 60; // Chiều rộng cột "Trạng Thái"

            dataGridView1.AllowUserToAddRows = false; // Tắt tính năng cho phép người dùng thêm dòng mới

            dataGridView1.AllowUserToDeleteRows = false;

            dataGridView1.ReadOnly = true;

            materialTextBox21.ReadOnly = true;
            materialTextBox22.ReadOnly = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Lấy giá trị từ các ô trong dòng
                string cellValue1 = selectedRow.Cells[0].Value.ToString(); // Cột 1 (ID)
                string cellValue2 = selectedRow.Cells[1].Value.ToString(); // Cột 2 (Tên Thiết Bị)
                string cellValue3 = selectedRow.Cells[2].Value.ToString(); // Cột 3 (Trạng Thái)

                materialTextBox21.Text = cellValue1;
                materialTextBox22.Text = cellValue2;
                status = cellValue3;
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (status == "Đã thuê")
        //    {
        //        if (dataGridView1.SelectedRows.Count > 0)
        //        {
        //            // Lấy dòng được chọn
        //            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

        //            // Đặt giá trị mới cho cột "Trạng Thái"
        //            selectedRow.Cells["Trạng Thái"].Value = "Chưa thuê";
        //            selectedRow.Cells["Ngày Trả Dự Kiến"].Value = "-";
        //        }
        //    }
        //    else MessageBox.Show("Thiết bị này chưa được thuê");
        //}

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (status == "Đã thuê")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Lấy dòng được chọn
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    // Đặt giá trị mới cho cột "Trạng Thái"
                    selectedRow.Cells["Trạng Thái"].Value = "Chưa thuê";
                    selectedRow.Cells["Ngày Trả Dự Kiến"].Value = "-";
                }
            }
            else MessageBox.Show("Thiết bị này chưa được thuê");
        }

        private void materialTextBox21_Click(object sender, EventArgs e)
        {

        }

        private void materialTextBox22_Click(object sender, EventArgs e)
        {

        }
    }
}
