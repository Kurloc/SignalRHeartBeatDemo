using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRSharedModels;

namespace SignalRServer.Controllers
{
    public class ChatHub : Hub
    {
        private IHeartBeatDbModelService _heartBeatDbModelService;
        
        public ChatHub(IHeartBeatDbModelService heartBeatDbModelService)
        {
            _heartBeatDbModelService = heartBeatDbModelService;
        }

        public async Task SendMessage(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("newMessage", "anonymousGroupName", message);
        }
        public async Task SendHeartBeatMessage(HeartBeatDbModel data)
        {
            Console.WriteLine($"guid: {data.guid}, name: {data.randomInfo}");
            await Clients.All.SendAsync("newMessage", "anonymousGroupName", data);
            _heartBeatDbModelService.SaveMessage(data);
        }
    }
}