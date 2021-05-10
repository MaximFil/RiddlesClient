using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Configuration;
using Riddles;
using System.ComponentModel;
using Riddles.Helpers;
using System.Windows.Forms;

namespace Riddles.Services
{
    public static class HubService
    {
        private static readonly HubConnection hubConnection;
        private static readonly HubHelper hubHelper;
        private static IAcceptInite formAcceptInvite;

        static HubService()
        {
            hubHelper = new HubHelper();
            hubConnection = new HubConnectionBuilder()
                .WithUrl(ConfigurationManager.AppSettings["hostAddress"] + "riddleshub")
                .Build();
            hubConnection.StartAsync();
            hubConnection.ServerTimeout = TimeSpan.FromMinutes(10);
            hubConnection.On<string>("Recieve", (message) =>
            {
                Console.WriteLine("User connected");
            });

            hubConnection.On<string, string, string>("SendInvite", (userName, levelName, message) =>
            {
                hubHelper.SendInvite(userName, levelName, message);
            });

            hubConnection.On<bool>("AcceptInvite", (accept) =>
            {
                formAcceptInvite.AcceptInvite(accept);
            });

            hubConnection.On<int>("StartGame", (gameSessionId) =>
            {
                hubHelper.StartGame(gameSessionId);
            });

            hubConnection.On<string>("Surrender", (userName) =>
            {
                hubHelper.Surrender(userName);
            });

            hubConnection.On("RivalFinished", () =>
            {
                hubHelper.RivalFinished();
            });

            hubConnection.On("RivalExitedGame", () =>
            {
                hubHelper.RivalExitedGame();
            });
        }

        private static bool isBusy;
        public static bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        private static bool isConnected;
        public static bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }

        public static async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();

                IsConnected = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Отключение от чата
        public static async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.StopAsync();
            IsConnected = false;
        }

        public static async Task SendInvite(string userName, string levelName, string message, IAcceptInite form)
        {
            formAcceptInvite = form;
            await hubConnection.InvokeAsync("SendInvite", userName, levelName);
        }

        public static async Task AcceptInvite(string userName, bool accept)
        {
            await hubConnection.InvokeAsync("AcceptInvite", userName, accept);
        }

        public static async Task StartGame(string userName, int gameSessionId)
        {
            await hubConnection.InvokeAsync("StartGame", userName, gameSessionId);
        }

        public static async void SendRequest()
        {
            await hubConnection.InvokeAsync("Send", UserProfile.Login, "Подключение пользователя");
        }

        //сдаваться
        public static async void Surrender(string userName, string rivalName)
        {
            await hubConnection.InvokeAsync("Surrender", userName, rivalName);
        }

        //соперник закончил раньше тебя и ждёт твоих действий
        public static async void RivalFinishedRequest(string rivalName)
        {
            await hubConnection.InvokeAsync("RivalFinished", rivalName);
        }

        //игрок вышел из игры не закончив игру
        public static async void RivalExitedGameResuest(string rivalName)
        {
            await hubConnection.InvokeAsync("RivalExited", rivalName);
        }

        public static event PropertyChangedEventHandler PropertyChanged;
        public static void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(new object(), new PropertyChangedEventArgs(prop));
        }
    }
}
