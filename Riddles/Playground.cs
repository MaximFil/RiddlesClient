using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Data.Entity;

namespace Riddles
{
    public enum HintType
    {
        HalfWord = 2,
        FullWord = 3
    }
    public partial class Playground : Form
    {
        private List<TextBox> textBoxList;
        private List<int> passRiddlesNumber;
        private int number;
        private int count;
        private StreamReader readerRiddle;
        private readonly Random random;
        private StreamReader readerAnswer;
        public static int typeHint = 0;
        public int initialValue = 10;
        private const int maxCountRiddle = 5;
        private int counter = 0;
        public delegate string usingHint(HintType hintType);
        public static event usingHint Notify;
        public string RiddlesAnswerPath { get; set; }
        public string RiddlesTextPath { get; set; }
        public string RiddleText { get; set; }
        public string RiddleAnswer { get; set; }
        public Level Level { get; set; }
        private Stopwatch stopwatch;

        public Playground()
        {
            InitializeComponent();
        }
        public Playground(Level level)
        {
            InitializeComponent();
            var pos = this.PointToScreen(pictureBox2.Location);
            pos = pictureBox1.PointToClient(pos);
            pictureBox2.Parent = pictureBox1;
            pictureBox2.Location = pos;
            pictureBox2.BackColor = Color.Transparent;
            this.Level = level;
            this.RiddlesTextPath = String.Format("{0}{1}.txt", ConfigurationManager.AppSettings["path"], this.Level);
            this.RiddlesAnswerPath = String.Format("{0}{1}Answer.txt", ConfigurationManager.AppSettings["path"], this.Level);
            this.count = File.ReadAllLines(this.RiddlesTextPath).Length;
            this.random = new Random();
            this.passRiddlesNumber = new List<int>();
            //this.InitializeRiddle();
            this.textBoxList = new List<TextBox> { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14 };
            Notify += function;
            stopwatch = new Stopwatch();
        }
        private string function(HintType hintType)
        {
            var res = String.Empty;
            if ((this.initialValue - (int)hintType) >= 0)
            {
                this.initialValue -= (int)hintType;
                label2.Text = String.Format("Points for tips: {0}", this.initialValue);
                var list = this.GetHintTextBoxes(hintType);
                var chars = this.GetHintChars(hintType);
                PutCharsInTextBoxes(list, chars);
                res = "success";
            }
            else
            {
                res = "error";
            }
            return res;
        }

        private List<TextBox> GetHintTextBoxes(HintType hintType)
        {
            var res = new List<TextBox>();
            if (hintType.Equals(HintType.HalfWord))
            {
                for (int i = 1; i < this.textBoxList.Where(t => t.Visible == true).Count(); i += 2)
                {
                    res.Add(textBoxList[i]);
                }
            }
            else
            {
                res.AddRange(this.textBoxList.Where(t => t.Visible == true));
            }
            return res;
        }
        private List<char> GetHintChars(HintType hintType)
        {
            //var answerChars = this.Riddle.Answer.ToCharArray();
            var res = new List<char>();
            //if (hintType.Equals(HintType.HalfWord))
            //{
            //    for (int i = 1; i < answerChars.Length; i += 2)
            //    {
            //        res.Add(answerChars[i]);
            //    }
            //}
            //else
            //{
            //    res.AddRange(answerChars);
            //}
            return res;
        }

        private void PutCharsInTextBoxes(List<TextBox> textBoxes, List<char> chars)
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].Text = chars[i].ToString();
            }
        }

        public static string UseHint(HintType hintType)
        {
            return Notify.Invoke(hintType);
        }
        //private Riddle InitializeRiddle()
        //{
        //    var availableRiddles = _context.Riddles.Include(r => r.Level).Include(r => r.Users).Where(r => r.Level.LevelName.Equals(this.Level.ToString()) && !r.Users.Any(u => u.Id == this.User.Id)).ToList();
        //    var availbaleRiddleIds = availableRiddles.Select(r => r.Id).ToList();
        //    var riddle = new Riddle();
        //    while (true)
        //    {
        //        this.number = this.random.Next(availbaleRiddleIds.Min(), availbaleRiddleIds.Max());
        //        riddle = availableRiddles.FirstOrDefault(r => r.Id == this.number);
        //        if (riddle != null)
        //        {
        //            break;
        //        }
        //    }
        //    return riddle;

        //}
        private string GetRiddleText(int number)
        {
            string line;
            int counter = 0;
            while ((line = this.readerRiddle.ReadLine()) != null)
            {
                if (counter == number)
                {
                    break;
                }
                counter++;
            }
            return line;
        }
        private string GetRiddleAnswer(int number)
        {
            string line;
            int counter = 0;
            while ((line = this.readerAnswer.ReadLine()) != null)
            {
                if (counter == number)
                {
                    break;
                }
                counter++;
            }
            return line;
        }
        private void ActionAfterResponse()
        {
            HideAllTextBox();
            //this.Riddle = this.InitializeRiddle();
            //label1.Text = this.Riddle.Text;
            //VisibleNeededTextBox(this.Riddle.Answer);
            CleanTextBox();
        }

        private void CleanTextBox()
        {
            textBoxList.ForEach(t => t.Text = string.Empty);
        }
     
        private void VisibleNeededTextBox(string answer)
        {
            textBoxList.Take(answer.Length).ToList().ForEach(t => t.Visible = true);
        }

        private void HideAllTextBox()
        {
            textBoxList.ForEach(t => t.Visible = false);
        }
        private TextBox GetEmptyTextBox()
        {
            return textBoxList.FirstOrDefault(t => t.Text.Length == 0 && t.Visible == true);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var answer = string.Join("", textBoxList.Where(t => t.Visible == true).Select(t => t.Text));
            //if (this.Riddle.Answer.Equals(answer, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    counter++;
            //    var level = _context.Levels.FirstOrDefault(l => l.LevelName == this.Level.ToString());
            //    var user = _context.Users.FirstOrDefault(u => u.Id == this.User.Id);
            //    if (counter == maxCountRiddle)
            //    {
            //        stopwatch.Stop();
            //        if (MessageBox.Show(String.Format("Congratulations! You won this difficult game and guessed all the riddles!\nYou: {0} \nSelected Level: {1} \nSpent time: {2} minutes and {3} seconds", this.User.Name, this.Level.ToString(), stopwatch.Elapsed.Minutes.ToString(), stopwatch.Elapsed.Seconds.ToString()), "Congratulations") == DialogResult.OK)
            //        {

            //            var record = new Riddles.DAL.Record { User = user, Level = level, Minutes = stopwatch.Elapsed.Minutes, Seconds = stopwatch.Elapsed.Seconds, TotalTime = stopwatch.Elapsed.Minutes * 60 + stopwatch.Elapsed.Seconds, Date = DateTimeOffset.Now };
            //            _context.Records.Add(record);
            //            this.Riddle.Users.Add(user);
            //            _context.SaveChanges();
            //            TableRecords records = new TableRecords(record);
            //            records.Show();
            //            this.Close();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("YOU did it!!!!!!!!");
            //        this.Riddle.Users.Add(user);
            //        _context.SaveChanges();
            //        ActionAfterResponse();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("TRY again(");
            //}

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            HideAllTextBox();
            //label1.Text = this.Riddle.Text;
            //VisibleNeededTextBox(this.Riddle.Answer);
            //label2.Text = String.Format("Hint glasses: {0}", this.initialValue);
            stopwatch.Start();
        }
        private void pictureBox2_MouseEnter_1(object sender, EventArgs e)
        {
            if (!this.Focused)
            {
                Hint hint = new Hint();
                hint.ShowDialog();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GetEmptyTextBox()?.Focus();
        }
        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
            if (e.KeyCode == Keys.Left) GetPreviousTextBox(GetFocusedTextBox())?.Focus();
            if (e.KeyCode == Keys.Right) GetNextTextBox(GetFocusedTextBox())?.Focus();
        }
        private TextBox GetFocusedTextBox()
        {
            return textBoxList.FirstOrDefault(t => t.Focused == true);
        }

        private TextBox GetPreviousTextBox(TextBox textBox)
        {
            var linkedList = new LinkedList<TextBox>(textBoxList);
            var listNode = linkedList.Find(textBox);
            return listNode?.Previous?.Value;
        }

        private TextBox GetNextTextBox(TextBox textBox)
        {
            var linkedList = new LinkedList<TextBox>(textBoxList);
            var listNode = linkedList.Find(textBox);
            return listNode?.Next?.Value?.Visible == true ? listNode?.Next?.Value : textBox;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
