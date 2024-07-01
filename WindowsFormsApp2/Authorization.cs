using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }
        public static bool IsAdminAuthorized = false; //Переменная определяющая являетесь ли вы администратором
        private void login_Click(object sender, EventArgs e)
        {
            unhide.Focus();
            string login = loginbox.Text;
            string password = passwordbox.Text;
            string validUsername = "admin";
            string validPassword = "121244"; ;
            if (login.Equals(validUsername, StringComparison.OrdinalIgnoreCase) && password.Equals(validPassword))
            {
                Authorization.IsAdminAuthorized = true;
                AdminPanel openform = new AdminPanel();
                openform.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Введён неверный логин или пароль.");
            }
        }

        private void unhide_Click(object sender, EventArgs e)
        {
            passwordbox.UseSystemPasswordChar = false;
        }

        private void hide_Click(object sender, EventArgs e)
        {
            passwordbox.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainPage openform = new MainPage();
            openform.Show();
            this.Hide();
        }

        private void Authorization_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
