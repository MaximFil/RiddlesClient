using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using Riddles.Entities;
using Riddles.Services;

namespace Riddles
{
    public partial class Playground : Form
    {
        private readonly GameSessionUseHintHistoryService hintHistoryService;
        private readonly AnswerHistoryService answerHistoryService;
        private readonly UserService userService;
        private readonly GameSessionService gameSessionService;
        private GameSession gameSession { get; set; }
        private List<Riddle> riddles { get; set; }
        private Riddle currentRiddle { get; set; }
        private RiddleService riddleService { get; set; }
        public delegate Task<Tuple<string, string>> usingHint(HintType hintType);
        public static event usingHint Notify;
        public delegate void surrenderRequest(string rivalName);
        public static event surrenderRequest SurrenderNotify;
        public delegate void rivalFinishedGame();
        public static event rivalFinishedGame rivalFinishedNotify;
        public delegate void rivalExitedGame();
        public static event rivalExitedGame rivalExitedNotify;
        public int HintPoints { get; set; } = 10;
        private List<TextBoxModel> textBoxModelsList { get; set; }
        private bool dispose { get; set; }
        private DateTime timeTimer { get; set; }
        private DateTime totalTime { get; set; }
        private int pointsForOneRiddle { get; set; }
        private int totalUserPoints { get; set; }      

        public Playground(GameSession gameSession)
        {
            InitializeComponent();
            this.gameSession = gameSession;
            UserProfile.GamaSessionId = gameSession.Id;
            this.riddleService = new RiddleService();
            this.userService = new UserService();
            this.hintHistoryService = new GameSessionUseHintHistoryService();
            this.answerHistoryService = new AnswerHistoryService();
            this.gameSessionService = new GameSessionService();
            this.textBoxModelsList = InitTextBoxModels();
            Notify += FunctionOfUsingHint;
            SurrenderNotify += SurrenderAction;
            rivalFinishedNotify += RivalFinishedAction;
            rivalExitedNotify += RivalExitedAction;
            this.dispose = true;
            this.timeTimer = new DateTime(1, 1, 1, 1, 0, 0).AddSeconds(UserProfile.Level.LevelTime);
            this.totalTime = new DateTime(1, 1, 1, 1, 0, 0);
            this.pointsForOneRiddle = 10;
            this.totalUserPoints = 0;
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
        private async Task<Tuple<string, string>> FunctionOfUsingHint(HintType hintType)
        {
            var res = String.Empty;
            var message = string.Empty;
            if ((this.HintPoints - (int)hintType) >= 0)
            {
                this.HintPoints -= Hints.DictionaryHints[hintType.ToString()].Cost;
                this.totalUserPoints -= Hints.DictionaryHints[hintType.ToString()].Cost;
                label5.Text = String.Format("Очки подсказок: {0}", this.HintPoints);
                label4.Text = string.Format("Очки пользователя: {0}", totalUserPoints);
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

                await hintHistoryService.CreateHistory(gameSession.Id, UserProfile.Id, currentRiddle.Id, hintType.ToString(), oldValue, newValue);
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

        public static Task<Tuple<string, string>> UseHint(HintType hintType)
        {
            return Notify.Invoke(hintType);
        }

        private void CleanTextBox()
        {
            textBoxModelsList.ForEach(t => t.TextBox.Text = string.Empty);
            textBoxModelsList.ForEach(t => t.Correct = false);
        }

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

        private async void button1_Click(object sender, EventArgs e)
        {
            dispose = false;
            var userAnswer = string.Join("", textBoxModelsList.Where(t => t.TextBox.Visible).Select(t => t.TextBox.Text));
            var correct = string.Equals(userAnswer, currentRiddle.Answer, StringComparison.InvariantCultureIgnoreCase);
            await answerHistoryService.CreateHistory(gameSession.Id, UserProfile.Id, currentRiddle.Id, userAnswer, correct);
            if(correct)
            {
                currentRiddle = GetNextRiddle();
                if(currentRiddle == null)
                {
                    dispose = false;
                    timer1.Stop();
                    await gameSessionService.CompleteGameSessionForUser(gameSession.Id, UserProfile.Id, totalTime.ToString("m:s"), totalUserPoints);
                    var rivalGameSessionUser = gameSessionService.GetGameSessionUser(gameSession.Id, UserProfile.RivalName);
                    if (!rivalGameSessionUser.Finished)
                    {
                        MessageBox.Show("Пожалуйста подождите пока ваш соперник не закончит игру.\nВы автоматичски будете переброшенны на страницу результатов!", "Игра окончена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox3.Image = Image.FromFile(@"Resources/99px_ru_animacii_20594_kot_krutitsja_kak_kolesiko_zagruzki.gif");
                        pictureBox3.Dock = DockStyle.Fill;
                        pictureBox3.Visible = true;
                    }
                    else
                    {
                        await gameSessionService.CompleteGameSession(gameSession.Id);
                        HubService.RivalFinishedRequest(UserProfile.RivalName);
                        ResultForm resultForm = new ResultForm(totalTime.ToString("m:s"), totalUserPoints, rivalGameSessionUser.TotalTime, rivalGameSessionUser.Points);
                        resultForm.Show();
                        this.Close();
                    }
                }
                else
                {
                    HideAllTextBox();
                    label1.Text = currentRiddle.Text;
                    VisibleNeededTextBox(currentRiddle.Answer);
                    CleanTextBox();
                    totalUserPoints += pointsForOneRiddle;
                    label4.Text = string.Format("Очки пользователя: {0}", totalUserPoints);
                }
                
            }
            else
            {
                MessageBox.Show("Ответ неверный!\nПопробуйте ещё раз.", "Ответ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            label1.MaximumSize = new Size(this.Size.Width, this.Size.Height);
            riddles = riddleService.GetRiddlesByGameSessionId(gameSession.Id);
            if(riddles == null || !riddles.Any())
            {
                MessageBox.Show("Не удалось получить загадки с сервера!", "Ошибка загрузки загадок", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            await userService.ChangeIsPlayingOfUser(UserProfile.Id, true);
            HideAllTextBox();
            currentRiddle = GetNextRiddle();
            label1.Text = currentRiddle.Text;
            VisibleNeededTextBox(currentRiddle.Answer);
            label5.Text = String.Format("Очки подсказок: {0}", this.HintPoints);
            label3.Text = timeTimer.ToString("m:ss");
            label4.Text = "Очки пользователя: 0";
            timer1.Start();
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


        private async void Playground_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dispose)
            {
                var dialogResult = MessageBox.Show("Вы действительно хотите закончить текущую игровую сессию и перейти в меню?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    await gameSessionService.ExitGameSessionUser(gameSession.Id, UserProfile.Id);
                    var menu = new Menu();
                    menu.Show();
                    this.Close();
                }
                else
                {
                    e.Cancel = true;
                }
            }

            await userService.ChangeIsPlayingOfUser(UserProfile.Id, false);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeTimer = timeTimer.AddSeconds(-1);
            totalTime = totalTime.AddSeconds(1);
            label3.Text = timeTimer.ToString("m:ss");
            if(timeTimer.Minute == 0 && timeTimer.Second == 0)
            {
                timer1.Stop();
                MessageBox.Show("Время вышло");
            }
        }        
        
        private class TextBoxModel
        {
            public TextBox TextBox { get; set; }

            public bool Correct { get; set; } = false;

            public int Index { get; set; }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await gameSessionService.SurrenderGameSessionUser(gameSession.Id, UserProfile.Login);
            HubService.Surrender(UserProfile.Login, UserProfile.RivalName);
            Menu mainMenu = new Menu();
            mainMenu.Show();
            this.Close();
        }

        public static void SurrenderRival(string rivalName)
        {
            MessageBox.Show($"Ваш соперник {rivalName} сдался!\nВы будете перенаправлены на форму результатов!", "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SurrenderNotify.Invoke(rivalName);
        }

        private async void SurrenderAction(string rivalName)
        {
            await gameSessionService.CompleteGameSession(gameSession.Id);
            var gameSessionUser = gameSessionService.GetGameSessionUser(gameSession.Id, rivalName);
            var resultForm = new ResultForm(totalTime.ToString("m:s"), totalUserPoints, gameSessionUser.TotalTime, gameSessionUser.Points, surrender: true);
            dispose = false;
            resultForm.Show();
            this.Close();
        }

        public static void RivalFinishedGame()
        {
            rivalFinishedNotify.Invoke();
        }

        private async void RivalFinishedAction()
        {
            await gameSessionService.CompleteGameSession(gameSession.Id);
            var gameSessionUser = gameSessionService.GetGameSessionUser(gameSession.Id, UserProfile.RivalName);
            var resultForm = new ResultForm(totalTime.ToString("m:s"), totalUserPoints, gameSessionUser.TotalTime, gameSessionUser.Points);
            dispose = false;
            resultForm.Show();
            this.Close();
        }

        public static void RivalExitedGame()
        {
            MessageBox.Show($"Ваш соперник {UserProfile.RivalName} вышел из игры!\nВы будете перенаправлены на форму результатов!", "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            rivalExitedNotify.Invoke();
        }

        private async void RivalExitedAction()
        {
            await gameSessionService.CompleteGameSession(gameSession.Id);
            var gameSessionUser = gameSessionService.GetGameSessionUser(gameSession.Id, UserProfile.RivalName);
            var resultForm = new ResultForm(totalTime.ToString("m:s"), totalUserPoints, gameSessionUser.TotalTime, gameSessionUser.Points, exited: true);
            dispose = false;
            resultForm.Show();
            this.Close();
        }

        private void Playground_Resize(object sender, EventArgs e)
        {
            label1.MaximumSize = new Size(this.Size.Width, this.Size.Height); 
        }
    }
}
