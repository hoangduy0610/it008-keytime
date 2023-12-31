﻿using IT008_KeyTime.Commons;
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
    public partial class ReportItem : Form
    {
        public ReportItem()
        {
            InitializeComponent();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            // save status of invetory item
            Cursor.Current = Cursors.WaitCursor;
            var status = this.materialComboBox1.SelectedIndex;
            var inventoryItem = PostgresHelper.GetById<InventoryItem>(Store._currentInventoryItem.id);
            inventoryItem.status = status;
            inventoryItem.note = this.materialMultiLineTextBox1.Text;
            PostgresHelper.Update(inventoryItem);
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Update status successfully");
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
