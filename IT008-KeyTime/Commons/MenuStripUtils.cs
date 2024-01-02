using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IT008_KeyTime.Commons;
using IT008_KeyTime.Views.Changepassword;

namespace IT008_KeyTime.Commons
{
    internal class MenuStripUtils
    {
        public static void ChangePassword()
        {
            Changepassword form = new Changepassword();
            form.ShowDialog();      
        }
        public static void LogOut() 
        {
            Loginform form = new Loginform();
            form.ShowDialog();
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "Loginform")
                    Application.OpenForms[i].Close();
            }
        }
        public static void ExitCurForm(Form curForm)
        {
            DialogResult dialog = new DialogResult();

            dialog = MessageBox.Show("Do you want to close?", "Alert!", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.Yes)
            {
                System.Environment.Exit(1);
            }
        }
        public static void Help()
        {
            System.Diagnostics.Process.Start("https://github.com/hoangduy0610/it008-keytime");
        }
    }
}
