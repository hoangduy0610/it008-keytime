using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using IT008_KeyTime.Commons;
using IT008_KeyTime.Models;
using IT008_KeyTime.Views;
using MaterialSkin.Controls;
using System.Text.RegularExpressions;



namespace IT008_KeyTime.Views.Forgotpassword
{
    public partial class Resetpassword : Form
    {
        public Resetpassword()
        {
            InitializeComponent();
            button1.Enabled = false;        
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckLoginButtonEnable();       
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CheckLoginButtonEnable();      
        }
        private void CheckLoginButtonEnable()
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                button1.Enabled = true;
                return;
            }
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string USERNAME = textBox1.Text;
            var statement = "SELECT * FROM tbl_users WHERE username ='" + USERNAME + "'";

            Console.WriteLine(statement);
            Console.WriteLine(USERNAME);

            var user = PostgresHelper.QueryFirst<User>(statement);
            string EMAIL = user.email;
            if (user is null)
            {
                MessageBox.Show("Username isn't valid.");
                return;
            }
            if (user.username != USERNAME)
            {
                MessageBox.Show("Wrong username.");
                this.Hide();
                this.Close();
                return;
            }

            string newPassword = GenerateRandomPassword(8, 14);
            string subject = "Reset password (IT008 - KeyTime)";
            string body = "New password: " + newPassword;

            string hashedPassword = IT008_KeyTime.Commons.Bcrypt.CreateMD5(newPassword); 
            user.password = hashedPassword;        
            PostgresHelper.Update(user);
            
            Send(EMAIL, subject, body);
            MessageBox.Show("Reset password successfully. Please check your email");

            this.Close();
        }
        private string GenerateRandomPassword(int minLength, int maxLength)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+";

            using (var rng = new RNGCryptoServiceProvider())
            {
                int length = new Random().Next(minLength, maxLength + 1);

                char[] password = new char[length];
                byte[] randomData = new byte[length];

                rng.GetBytes(randomData);

                for (int i = 0; i < length; i++)
                {
                    password[i] = validChars[randomData[i] % validChars.Length];
                }

                return new string(password);
            }
        }

        public SmtpClient client = new SmtpClient();
        public MailMessage msg = new MailMessage();
        public System.Net.NetworkCredential smtpCreds = new System.Net.NetworkCredential("hoangduy06104@gmail.com", "zdzqvzxnxknlgkus");
        public void Send(string sendTo, string subject, string body)
        {
            try
            {
                //setup SMTP Host Here
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = smtpCreds;
                client.EnableSsl = true;

                //converte string to MailAdress
                MailAddress to = new MailAddress(sendTo);
                MailAddress from = new MailAddress("hoangduy06104@gmail.com", "IT008 KeyTime");

                //set up message settings
                msg.Subject = subject;
                msg.Body = body;
                msg.From = from;
                msg.To.Add(to);

                // Enviar E-mail
                client.Send(msg);

            }
            catch (Exception error)
            {
                MessageBox.Show("Unexpected Error: " + error);
            }
        }
    }
}
