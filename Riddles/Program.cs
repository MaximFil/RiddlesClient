using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles.Services;

namespace Riddles
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            UserService userService = new UserService();
            userService.ChangeActivityOfUser(UserProfile.Id, false);
            userService.ChangeIsPlayingOfUser(UserProfile.Id, false);
        }
    }
}
