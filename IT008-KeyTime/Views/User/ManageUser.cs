﻿using IT008_KeyTime.Commons;
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

namespace IT008_KeyTime.Views
{
    public partial class ManageUser : Form
    {
        public ManageUser()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Registrationform form = new Registrationform();
            this.Hide();
            form.ShowDialog();
            ShowLoading();
            backgroundWorker1.RunWorkerAsync();
            this.Show();
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
                    var users = (List<User>)data;
                    List<MapUser> mapUsers = new List<MapUser>();       

                    foreach (var user in users)
                    {
                        mapUsers.Add(new MapUser(user));        
                    }

                    this.dataGridView1.DataSource = mapUsers;
                    this.dataGridView1.Columns["password"].Visible = false;
                    this.dataGridView1.Columns["role"].Visible = false;
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
            var users = PostgresHelper.GetAll<User>();
            UpdateDataGridViewSource(users);
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Delete this user?", "Confirm Delete", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                try
                {
                    materialButton3.Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    ShowLoading();

                    // Delete selecting row in dataGridView1
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        var selectedRow = dataGridView1.SelectedRows[0];
                        var user = selectedRow.DataBoundItem as User;
                        if (user != null)
                        {
                            PostgresHelper.Delete(user);
                            backgroundWorker1.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select at least one row");
                    }
                }
                finally
                {
                    materialButton3.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    HideLoading();
                }
            }
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            // enable button delete when user select a row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                materialButton3.Enabled = true;
                materialButton2.Enabled = true;
            }
            else
            {
                materialButton3.Enabled = false;
                materialButton2.Enabled = false;
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            // fill data from selected row of dataGridView1 to Store._currentEditing
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                var mapuser = selectedRow.DataBoundItem as MapUser;
                var user = PostgresHelper.GetById<User>(mapuser.id);
                if (user != null)
                {
                    Store._currentEditing = user;
                    Registrationform form = new Registrationform();
                    this.Hide();
                    form.ShowDialog();
                    ShowLoading();
                    backgroundWorker1.RunWorkerAsync();
                    this.Show();
                }
            }
            else
            {
                MessageBox.Show("Please select one row");
            }
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
