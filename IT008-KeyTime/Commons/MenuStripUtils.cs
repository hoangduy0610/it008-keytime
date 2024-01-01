using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
