﻿using System;
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

namespace Riddles
{
    public partial class TableRecords : Form, ICloseble
    {

        //private readonly Context _context;
        
        //public Riddles.DAL.Record Record { get; set; }
        public TableRecords()
        {
            InitializeComponent();
            UserProfile.CurrentForm = this;
           // this.Record = record;
           // _context = new Context();
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = GetRecordsByLevel(this.Record.Level.LevelName);

        }

        private void middleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = GetRecordsByLevel(this.Record.Level.LevelName);
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = GetRecordsByLevel(this.Record.Level.LevelName);
        }
        private List<Record> GetRecordsByLevel(string level)
        {
            //var records = _context.Records.Include(r => r.Level).Include(r => r.User).Where(r => r.Level.LevelName == level).OrderBy(r => r.TotalTime).ToList();
            var res = new List<Record>();
            //foreach (var record in records)
            //{
            //    res.Add(new Riddles.Record { Date = record.Date.ToString("dd.MM.yyyy"), Minutes = record.Minutes, Name = record.User.Name, Seconds = record.Seconds });
            //}
            return res;
        }
        private void TableRecords_Load(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = GetRecordsByLevel(this.Record.Level.LevelName);
        }

        public void CloseForm()
        {
            this?.Close();
        }
    }

}
