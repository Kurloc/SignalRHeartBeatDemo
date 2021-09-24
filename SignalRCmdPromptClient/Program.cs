using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SignalRSharedModels;

namespace SignalRDemo
{
    class Program
    {
        private static HubConnection _connection;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var running = true;
            
            await StartConnection();
            while (running)
            {
                await SendMessage();
                await Task.Delay(1000);
            }
        }

        static async Task StartConnection()
        {
            Console.WriteLine("Let's build our Connection");
            
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/ChatHub")
                .Build();
            await _connection.StartAsync();
            
            Console.WriteLine("Con Built");
        }

        static async Task SendMessage()
        {
            await _connection.InvokeAsync("SendHeartBeatMessage", new HeartBeatDbModel(){ guid = Guid.NewGuid(), randomInfo = "Test" });
            Console.WriteLine("HeartBeat Sent at " + DateTime.Now);
        }
    }
}