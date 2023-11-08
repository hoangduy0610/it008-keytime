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
    public partial class GiaHan : Form
    {
        public GiaHan()
        {
            InitializeComponent();

            // Tạo DataTable và định nghĩa cấu trúc
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Tên Thiết Bị", typeof(string));
            dataTable.Columns.Add("Trạng Thái", typeof(string));
            dataTable.Columns.Add("Ngày Trả Dự Kiến", typeof(string));

            // Thêm dữ liệu vào DataTable
            dataTable.Rows.Add(1, "Thiết Bị 1", "Đã thuê", "10/25/2023");
            dataTable.Rows.Add(2, "Thiết Bị 2", "Chưa thuê", "-");
            dataTable.Rows.Add(3, "Thiết Bị 3", "Đã thuê", "11/7/2023");

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

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy dòng đã chọn
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Lấy giá trị mới từ DateTimePicker
                DateTime newDate = dateTimePicker1.Value;

                // Cập nhật cột 4 của dòng đã chọn với giá trị mới
                selectedRow.Cells[3].Value = newDate;
            }
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

                textBox1.Text = cellValue1;
                textBox2.Text = cellValue2;

                string cellValue4 = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

                if (DateTime.TryParse(cellValue4, out DateTime trarDate))
                {
                    dateTimePicker1.Value = trarDate;
                }
                else
                {
                    MessageBox.Show("Dữ liệu ngày không hợp lệ");
                }
                //status = cellValue3;
            }
        }
    }
}
