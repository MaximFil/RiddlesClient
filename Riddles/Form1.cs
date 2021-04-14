using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles.Entities;
using Riddles.Services;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Principal;

namespace Riddles
{
    public partial class Form1 : Form
    {
        private readonly UserService userService;
        //private readonly HubService hubService;
        private const string loginLabelText = "У меня уже есть аккаунт";
        private const string signupLabelText = "Создать новый аккаунт";
        private const string enterName = "Введите логин";
        private const string enterPassword = "Введите пароль";
        private bool isLogin = true;
        private HashSet<string> usedUserNames;
        private User User;

        public Form1()
        {
            InitializeComponent();
            this.userService = new UserService();
            this.usedUserNames = new HashSet<string>();
            //this.hubService = new HubService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var login = textBox1.Text;
                var password = textBox2.Text;
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Логин или пароль должен содержать валидные символы!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var loginRegex = new Regex(@"[^\u0021-\u007E]+");
                var loginAdditionalRegex = new Regex("\"|\'");
                if (loginRegex.IsMatch(login) || loginAdditionalRegex.IsMatch(login))
                {
                    MessageBox.Show("Пожалуйста введите корректный логин, который соответствует правилам:" +
                        "\n-можно использовать латинские символы;" +
                        "\n-можно использовать цифры;" +
                        "\n-можно использовать спецсимволы за исключением \' и \".",
                        "Валидация логина",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var passwordRegex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_.]).{8,}$");
                if (!passwordRegex.IsMatch(password))
                {
                    MessageBox.Show("Пожалуйста введите корректный пароль, который соответствует правилам:" +
                        "\n-по крайней мере одна строчная английская буква;" +
                        "\n-по крайней мере одна заглавная английская буква;" +
                        "\n-по крайней мере одна цифра;" +
                        "\n-по крайней мере один спец символ;" +
                        "\n-минимум 8 символов в длину.",
                        "Валидация пароля",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (isLogin)
                {
                    User = userService.LogIn(login, password);
                }
                else
                {
                    User = userService.SignUp(login, password);
                }
                userService.ChangeActivityOfUser(User.Id, true);

                UserProfile.Id = User.Id;
                UserProfile.Login = User.Name;
                UserProfile.Password = User.Password;

                HubService.SendRequest();

                Menu menu = new Menu(/*hubService*/);
                menu.Show();
                this.Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                button1.Text = "Заригестрироваться";
            }
            else
            {
                isLogin = true;
                label2.Text = signupLabelText;
                button1.Text = "Войти";
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                usedUserNames = await userService.GetUsedUserNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isLogin)
            {
                if (usedUserNames.Contains(textBox1.Text))
                {
                    label3.Visible = true;
                }
                else
                {
                    label3.Visible = false;
                }
            } 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(User != null)
            {
                userService.ChangeActivityOfUser(User.Id, false);
            }
        }
    }
}
