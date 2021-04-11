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
        private static SendRequest sendRequest;

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
                Form1 form1 = new Form1();
            });

            hubConnection.On<string>("SendInvite", (userName) =>
            {
                hubHelper.SendInvite(userName);
            });

            hubConnection.On<bool>("AcceptInvite", (accept) =>
            {
                sendRequest.AcceptInvite(accept);
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

        public static async Task SendInvite(string userName, SendRequest form)
        {
            sendRequest = form;
            await hubConnection.InvokeAsync("SendInvite", userName);
        }

        public static async Task AcceptInvite(string userName, bool accept)
        {
            await hubConnection.InvokeAsync("AcceptInvite", userName, accept);
        }

        public static async void SendRequest()
        {
            await hubConnection.InvokeAsync("Send", "Болит очко");
        }

        public static event PropertyChangedEventHandler PropertyChanged;
        public static void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(new object(), new PropertyChangedEventArgs(prop));
        }
    }
}
