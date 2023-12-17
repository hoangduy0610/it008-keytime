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
    public partial class CreatePlan : Form
    {
        public CreatePlan()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }

        private void HideLoading()
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

        private void ShowLoading()
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

        private void CheckEditData()
        {
            if (Store._currentInventoryPlan != null)
            {
                var plan = Store._currentInventoryPlan;
                this.materialTextBox1.Text = plan.name;
                this.materialTextBox2.Text = plan.note;
                this.materialComboBox1.SelectedValue = plan.assignee_id;
                this.dateTimePicker1.Value = plan.deadline;
                this.Text = "Edit Plan";
                this.materialButton1.Text = "Update";
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // fetch all users and fill to materialComboBox1
            var users = PostgresHelper.GetAll<User>();
            if (users != null)
            {
                if (this.materialComboBox1.InvokeRequired)
                {
                    this.materialComboBox1.Invoke(
                        new Action(() => {
                            this.materialComboBox1.ValueMember = "id";
                            this.materialComboBox1.DisplayMember = "name";
                            this.materialComboBox1.DataSource = users;
                            CheckEditData();
                        })
                    );
                }
                else
                {
                    this.materialComboBox1.ValueMember = "id";
                    this.materialComboBox1.DisplayMember = "name";
                    this.materialComboBox1.DataSource = users;
                    CheckEditData();
                }
            }
            else
            {
                MessageBox.Show("No data");
            }
            HideLoading();
        }

        private void ResetForm()
        {
            materialTextBox1.Text = "";
            materialTextBox2.Text = "";
            materialComboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            var name = materialTextBox1.Text;
            if (name == "")
            {
                MessageBox.Show("Name is required");
                return;
            }
            var note = materialTextBox2.Text;
            var assignee = materialComboBox1.SelectedItem as User;
            var deadline = dateTimePicker1.Value;
            var status = (int) InventoryPlanStatus.PLANNING;
            ShowLoading();
            this.materialButton1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            // check if this is edit mode
            if (Store._currentInventoryPlan != null)
            {
                var plan_edit = PostgresHelper.GetById<InventoryPlan>(Store._currentInventoryPlan.id);
                plan_edit.name = name;
                plan_edit.note = note;
                plan_edit.assignee_id = assignee.id;
                plan_edit.deadline = deadline;
                plan_edit.status = status;
                var result_edit = PostgresHelper.Update<InventoryPlan>(plan_edit);
                if (result_edit)
                {
                    MessageBox.Show("Update plan successfully");
                }
                else
                {
                    MessageBox.Show("Update plan failed");
                }
                this.materialButton1.Enabled = true;
                Cursor.Current = Cursors.Default;
                HideLoading();
                return;
            }

            var plan = new InventoryPlan()
            {
                name = name,
                note = note,
                assignee_id = assignee.id,
                deadline = deadline,
                status = status
            };
            var result = PostgresHelper.Insert<InventoryPlan>(plan);
            if (result != 0)
            {
                MessageBox.Show("Create plan successfully");
                ResetForm();
            }
            else
            {
                MessageBox.Show("Create plan failed");
            }

            this.materialButton1.Enabled = true;
            Cursor.Current = Cursors.Default;
            HideLoading();
        }

        private void materialLabel2_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
