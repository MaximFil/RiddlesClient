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
using Riddles.Entities;
using Riddles.Services;

namespace Riddles
{
    public partial class Playground : Form
    {
        private readonly GameSessionUseHintHistoryService hintHistoryService;
        private GameSession gameSession;
        private List<Riddle> riddles;
        private Riddle currentRiddle;
        private RiddleService riddleService;
        private Stopwatch stopwatch;
        public delegate Tuple<string, string> usingHint(HintType hintType);
        public static event usingHint Notify;
        public int HintPoints { get; set; } = 10;
        private List<TextBoxModel> textBoxModelsList;
        private readonly UserService userService;
        private bool dispose;
        private readonly AnswerHistoryService answerHistoryService;


        private List<int> passRiddlesNumber;
        private int number;
        private int count;
        private readonly Random random;
        public static int typeHint = 0;
        private const int maxCountRiddle = 5;
        private int counter = 0;
        public string RiddlesAnswerPath { get; set; }
        public string RiddlesTextPath { get; set; }
        public string RiddleText { get; set; }
        public string RiddleAnswer { get; set; }
        public Level Level { get; set; }
        

        public Playground(GameSession gameSession)
        {
            InitializeComponent();
            this.gameSession = gameSession;
            this.riddleService = new RiddleService();
            this.userService = new UserService();
            hintHistoryService = new GameSessionUseHintHistoryService();
            this.answerHistoryService = new AnswerHistoryService();
            textBoxModelsList = InitTextBoxModels();
            stopwatch = new Stopwatch();
            Notify += FunctionOfUsingHint;
            dispose = true;

            //var pos = this.PointToScreen(pictureBox2.Location);
            //pos = pictureBox1.PointToClient(pos);
            //pictureBox2.Parent = pictureBox1;
            //pictureBox2.Location = pos;
            //pictureBox2.BackColor = Color.Transparent;
            //this.Level = level;
            //this.RiddlesTextPath = String.Format("{0}{1}.txt", ConfigurationManager.AppSettings["path"], this.Level);
            //this.RiddlesAnswerPath = String.Format("{0}{1}Answer.txt", ConfigurationManager.AppSettings["path"], this.Level);
            //this.count = File.ReadAllLines(this.RiddlesTextPath).Length;
            //this.random = new Random();
            //this.passRiddlesNumber = new List<int>();
            //
            //
        }

        private List<TextBoxModel> InitTextBoxModels()
        {
            return new List<TextBoxModel>
            {
                new TextBoxModel{TextBox = textBox1, Index = 0},
                new TextBoxModel{TextBox = textBox2, Index = 1},
                new TextBoxModel{TextBox = textBox3, Index = 2},
                new TextBoxModel{TextBox = textBox4, Index = 3},
                new TextBoxModel{TextBox = textBox5, Index = 4},
                new TextBoxModel{TextBox = textBox6, Index = 5},
                new TextBoxModel{TextBox = textBox7, Index = 6},
                new TextBoxModel{TextBox = textBox8, Index = 7},
                new TextBoxModel{TextBox = textBox9, Index = 8},
                new TextBoxModel{TextBox = textBox10, Index = 9},
                new TextBoxModel{TextBox = textBox11, Index = 10},
                new TextBoxModel{TextBox = textBox12, Index = 11},
                new TextBoxModel{TextBox = textBox13, Index = 12},
                new TextBoxModel{TextBox = textBox14, Index = 13}
            };
        }
        private Tuple<string, string> FunctionOfUsingHint(HintType hintType)
        {
            var res = String.Empty;
            var message = string.Empty;
            if ((this.HintPoints - (int)hintType) >= 0)
            {
                this.HintPoints -= (int)hintType;
                label2.Text = String.Format("Очки подсказок: {0}", this.HintPoints);
                int countTextBoxesForOpen = 1;
                if(hintType == HintType.OneChar)
                {
                    countTextBoxesForOpen = 1;
                }
                else if(hintType == HintType.HalfWord)
                {
                    countTextBoxesForOpen = currentRiddle.Answer.Length / 2;
                }
                else
                {
                    countTextBoxesForOpen = currentRiddle.Answer.Length;
                }

                ClearAllInCorrectTextBoxes();
                var oldValue = GetValueFromVisibleTextBoxes();
                this.UseHintForTextBoxes(countTextBoxesForOpen);
                var newValue = GetValueFromVisibleTextBoxes();

                hintHistoryService.CreateHistory(gameSession.Id, UserProfile.Id, currentRiddle.Id, hintType.ToString(), oldValue, newValue);
                res = "success";
            }
            else
            {
                res = "error";
                message = "Недостаточно очков подсказок!";
            }
            return new Tuple<string, string>(res, message);
        }

        private void ClearAllInCorrectTextBoxes()
        {
            textBoxModelsList.Where(t => !t.Correct).ToList().ForEach(t => t.TextBox.Text = string.Empty);
        }

        private string GetValueFromVisibleTextBoxes()
        {
            var visibleTextBoxes = textBoxModelsList.Where(t => t.TextBox.Visible).ToList();

            return string.Join("", visibleTextBoxes);
        }

        private void UseHintForTextBoxes(int count)
        {
            var allCorrectTextBoxes = textBoxModelsList
                .Where(t => !t.Correct);

            var necessaryCorrectTestBoxes = allCorrectTextBoxes.Take(Math.Min(count, allCorrectTextBoxes.Count())).ToList();

            foreach(var textBox in necessaryCorrectTestBoxes)
            {
                textBox.TextBox.Text = currentRiddle.Answer[textBox.Index].ToString();
            }
        }

        public static Tuple<string, string> UseHint(HintType hintType)
        {
            return Notify.Invoke(hintType);
        }


        //private void ActionAfterResponse()
        //{
        //    HideAllTextBox();
        //    //this.Riddle = this.InitializeRiddle();
        //    //label1.Text = this.Riddle.Text;
        //    //VisibleNeededTextBox(this.Riddle.Answer);
        //    CleanTextBox();
        //}

        //private void CleanTextBox()
        //{
        //    textBoxList.ForEach(t => t.Text = string.Empty);
        //}

        private void VisibleNeededTextBox(string answer)
        {
            textBoxModelsList.Take(answer.Length).ToList().ForEach(t => t.TextBox.Visible = true);
        }

        private void HideAllTextBox()
        {
            textBoxModelsList.ForEach(t => t.TextBox.Visible = false);
        }

        private TextBoxModel GetEmptyTextBox()
        {
            return textBoxModelsList.FirstOrDefault(t => t.TextBox.Text.Length == 0 && t.TextBox.Visible == true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dispose = false;
            var userAnswer = string.Join("", textBoxModelsList.Where(t => t.TextBox.Visible).Select(t => t.TextBox.Text));
            var correct = string.Equals(userAnswer, currentRiddle.Answer, StringComparison.InvariantCultureIgnoreCase);
            answerHistoryService.CreateHistory(gameSession.Id, UserProfile.Id, currentRiddle.Id, userAnswer, correct);
            if(correct)
            {
                currentRiddle = GetNextRiddle();
                if(currentRiddle == null)
                {
                    MessageBox.Show("You are winner");
                }

                HideAllTextBox();
                label1.Text = currentRiddle.Text;
                VisibleNeededTextBox(currentRiddle.Answer);
            }
            else
            {
                MessageBox.Show("Ответ неверный!\nПопробуйте ещё раз.", "Ответ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            riddles = riddleService.GetRiddlesByGameSessionId(gameSession.Id);
            if(riddles == null || !riddles.Any())
            {
                MessageBox.Show("Не удалось получить загадки с сервера!", "Ошибка загрузки загадок", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            userService.ChangeIsPlayingOfUser(UserProfile.Id, true);
            HideAllTextBox();
            currentRiddle = GetNextRiddle();
            label1.Text = currentRiddle.Text;
            VisibleNeededTextBox(currentRiddle.Answer);
            label2.Text = String.Format("Очки подсказок: {0}", this.HintPoints);
            stopwatch.Start();
        }

        private Riddle GetNextRiddle()
        {
            if(currentRiddle == null)
            {
                return riddles?.FirstOrDefault();
            }
            else
            {
                return riddles.Where(r => r.Id > currentRiddle.Id)?.FirstOrDefault();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GetEmptyTextBox()?.TextBox.Focus();
        }
        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
            if (e.KeyCode == Keys.Left) GetPreviousTextBox(GetFocusedTextBox())?.TextBox.Focus();
            if (e.KeyCode == Keys.Right) GetNextTextBox(GetFocusedTextBox())?.TextBox.Focus();
        }
        private TextBoxModel GetFocusedTextBox()
        {
            return textBoxModelsList.FirstOrDefault(t => t.TextBox.Focused == true);
        }

        private TextBoxModel GetPreviousTextBox(TextBoxModel textBox)
        {
            var linkedList = new LinkedList<TextBoxModel>(textBoxModelsList);
            var listNode = linkedList.Find(textBox);
            return listNode?.Previous?.Value;
        }

        private TextBoxModel GetNextTextBox(TextBoxModel textBox)
        {
            var linkedList = new LinkedList<TextBoxModel>(textBoxModelsList);
            var listNode = linkedList.Find(textBox);
            return listNode?.Next?.Value?.TextBox.Visible == true ? listNode?.Next?.Value : textBox;
        }
        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var visibleCount = textBoxModelsList.Where(t => t.TextBox.Visible == true).Count();
            var correctCount = textBoxModelsList.Where(t => t.Correct).Count();

            if(HintPoints < 1)
            {
                MessageBox.Show("У вас нет очков подсказоок!", "Лимит исчерпан", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if(correctCount >= visibleCount)
            {
                MessageBox.Show("Введённое слово корректно!\nНе требуется использование подсказки.", "Подсказки", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                UseHint useHint = new UseHint();
                useHint.Show();
            }
        }
        private class TextBoxModel
        {
            public TextBox TextBox { get; set; }

            public string Char { get; set; } = string.Empty;

            public bool Correct { get; set; } = false;

            public int Index { get; set; }
        }

        private void Playground_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dispose)
            {
                Application.Exit();
            }
        }
    }
}
