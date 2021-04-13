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
    public class HubService
    {
        private readonly HubConnection hubConnection;
        private readonly HubHelper hubHelper;
        private SendRequest sendRequest;

        public HubService()
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

            hubConnection.On<string>("SendInvite", (userName) =>
            {
                hubHelper.SendInvite(userName);
            });

            hubConnection.On<bool>("AcceptInvite", (accept) =>
            {
                sendRequest.AcceptInvite(accept);
            });
        }

        private bool isBusy;
        public bool IsBusy
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

        private bool isConnected;
        public bool IsConnected
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

        public async Task Connect()
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
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.StopAsync();
            IsConnected = false;
        }

        public async Task SendInvite(string userName, SendRequest form)
        {
            sendRequest = form;
            await hubConnection.InvokeAsync("SendInvite", userName);
        }

        public async Task AcceptInvite(string userName, bool accept)
        {
            await hubConnection.InvokeAsync("AcceptInvite", userName, accept);
        }

        public async void SendRequest()
        {
            await hubConnection.InvokeAsync("Send", UserProfile.Login, "Подключение пользователя");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(new object(), new PropertyChangedEventArgs(prop));
        }
    }
}
