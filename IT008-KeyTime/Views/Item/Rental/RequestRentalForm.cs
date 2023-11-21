using IT008_KeyTime.Commons;
using IT008_KeyTime.Enums;
using IT008_KeyTime.Models;
using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IT008_KeyTime
{
    public partial class RequestRentalForm : Form
    {
        public object RentalStatus { get; private set; }

        public RequestRentalForm()
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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            // check if id and name is empty
            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            // insert new rental item
            var rentalItem = new RentalItem();
            rentalItem.user_id = Store._user.id;
            rentalItem.item_id = int.Parse(textBox3.Text);
            rentalItem.rental_start = dateTimePicker1.Value;
            rentalItem.expect_return = dateTimePicker2.Value;
            rentalItem.status = (int)RentalStatusEnum.REQUESTED;

            PostgresHelper.Insert(rentalItem);
            MessageBox.Show("Request success");
            materialButton1.Enabled = true;
            Cursor.Current = Cursors.Default;
            clearForm();
        }

        private void clearForm()
        {
            textBox3.Text = "";
            textBox4.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var data = PostgresHelper.GetAll<Item>();
            UpdateDataGridViewSource(data);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get selected row, fill to id and name textbox
            var row = dataGridView1.Rows[e.RowIndex];
            var id = row.Cells["id"].Value?.ToString();
            var name = row.Cells["name"].Value?.ToString();
            textBox3.Text = id;
            textBox4.Text = name;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            // check if last date is before start date
            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                MessageBox.Show("Last date must be after start date");
                dateTimePicker2.Value = dateTimePicker1.Value;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // check if start date is before today
            if (dateTimePicker1.Value < DateTime.Now)
            {
                MessageBox.Show("Start date must be after now");
                dateTimePicker1.Value = DateTime.Now;
                return;
            }
            dateTimePicker2.Value = dateTimePicker1.Value;
        }
    }
}
