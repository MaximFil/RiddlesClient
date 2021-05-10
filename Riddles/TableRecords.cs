using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
//using Riddles.DAL;
using System.Data.Entity;
using Riddles.Services;

namespace Riddles
{
    public partial class TableRecords : Form, ICloseble
    {

        private readonly RecordService recordService;

        private List<ToolStripMenuItem> ToolStripMenuItems;

        public TableRecords()
        {
            InitializeComponent();
            UserProfile.CurrentForm = this;
            this.recordService = new RecordService();
            this.ToolStripMenuItems = new List<ToolStripMenuItem> { easyToolStripMenuItem, middleToolStripMenuItem, hardToolStripMenuItem };
        }

        private async void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = await recordService.GetRecordsByLevel(Level.Easy.ToString());
            easyToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
            middleToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.Control);
            hardToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        private async void middleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = await recordService.GetRecordsByLevel(Level.Middle.ToString());
            middleToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
            easyToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.Control);
            hardToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        private  async void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = await recordService .GetRecordsByLevel(Level.Hard.ToString());
            hardToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
            middleToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.Control);
            easyToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        private async void TableRecords_Load(object sender, EventArgs e)
        {
            var defaultLevelName = UserProfile.Level?.LevelName ?? Level.Easy.ToString();

            if (string.Equals(defaultLevelName, Level.Easy.ToString()))
            {
                easyToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
            }

            if (string.Equals(defaultLevelName, Level.Middle.ToString()))
            {
                middleToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
            }

            if (string.Equals(defaultLevelName, Level.Hard.ToString()))
            {
                hardToolStripMenuItem.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
            }

            dataGridView1.DataSource = await recordService.GetRecordsByLevel(defaultLevelName);
        }

        public void CloseForm()
        {
            this?.Close();
        }

        private void TableRecords_FormClosed(object sender, FormClosedEventArgs e)
        {
            var menu = new Menu();
            menu.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
