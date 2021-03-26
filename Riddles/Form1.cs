using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles.DAL;
using Riddles.Services;

namespace Riddles
{
    public partial class Form1 : Form
    {
        private const string loginLabelText = "I have an account";
        private const string signupLabelText = "Create a new account";
        private const string enterName = "Enter your Nickname";
        private const string enterPassword = "Enter your Password";
        private bool isLogin = true;

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserService userService = new UserService();
            var ewbjkfdew = userService.GetData();
            MessageBox.Show(string.Join("&", ewbjkfdew));
            //var name = textBox1.Text;
            //if (checkBox1.Checked)
            //{
            //    if (string.IsNullOrEmpty(name) || name.Equals(enterName))
            //    {
            //        MessageBox.Show("Be sure to enter a name!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else
            //    {
            //        var availableUsers = _context.Users.ToList();
            //        if (checkBox2.Checked && availableUsers.Any(u => u.Name.Equals(name)))
            //        {
            //            MessageBox.Show("Please enter unique name");
            //        }
            //        else if(checkBox2.Checked)
            //        {
            //            User user = new User() { Name = name };
            //            _context.Users.Add(user);
            //            _context.SaveChanges();
            //            Menu menu = new Menu(user);
            //            menu.Show();
            //            this.Hide();
            //        }
            //        else if (!checkBox2.Checked)
            //        {
            //            var user = availableUsers.FirstOrDefault(u => u.Name.Equals(name));
            //            if (user == null)
            //            {
            //                MessageBox.Show("Such a user does not exist.Please check the correctness of the entered data");
            //            }
            //            else
            //            {
            //                Menu menu = new Menu(user);
            //                menu.Show();
            //                this.Hide();
            //            }
            //        }
            //    }
            //}
            //else 
            //{ 
            //    MessageBox.Show("To continue, you must accept the following agreement!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            if (isLogin)
            {
                isLogin = false;
                label2.Text = loginLabelText;
                button1.Text = "Sign up";
            }
            else
            {
                isLogin = true;
                label2.Text = signupLabelText;
                button1.Text = "Log in";
            }
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }

        private void TextBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
        }
    }
}
