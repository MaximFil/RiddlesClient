using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Riddles.Services;
//using Riddles.DAL;

namespace Riddles
{
    public partial class Menu : Form, ICloseble
    {
        private readonly LevelService levelService;
        private readonly HintService hintService;
        private readonly UserService userService;
        private bool dispose;
        public Menu(bool dispose = true)
        {
            InitializeComponent();
            UserProfile.CurrentForm = this;
            this.levelService = new LevelService();
            this.hintService = new HintService();
            this.userService = new UserService();
            this.dispose = dispose;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dispose = false;
            SendRequest sendRequest = new SendRequest();
            sendRequest.Show();
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dispose = false;
            About_The_Game about_The_Game = new About_The_Game();
            about_The_Game.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dispose = false;
            TableRecords records = new TableRecords();
            records.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dispose)
            {
                Application.Exit();
            }
        }
        public void CloseForm()
        {
            this?.Close();
        }

        private async void Menu_Load(object sender, EventArgs e)
        {
            var levels = await levelService.GetLevels();
            Levels.DictionaryLevels = levels.ToDictionary(l => l.LevelName);
            var hints = await hintService.GetHints();
            Hints.DictionaryHints = hints.ToDictionary(h => h.Name);
            await userService.ChangeIsPlayingOfUser(UserProfile.Id, false);
        }
    }
}
