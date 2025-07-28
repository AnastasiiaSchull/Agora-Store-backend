using Agora.BLL.DTO;
using Agora.BLL.Storages;
using Microsoft.AspNetCore.SignalR;

namespace Agora.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string sender, string message)
        {
            var newMessage = new Message
            {
                Sender = sender,
                MessageText = message,
                SentAt = DateTime.UtcNow
            };

            ChatStorage.Messages.Add(newMessage);

            if (sender == "client" && !ChatStorage.HasClientStarted)
            {
                ChatStorage.HasClientStarted = true;
                await Clients.All.SendAsync("ChatStatusChanged", true);
            }

            await Clients.All.SendAsync("ReceiveMessage", newMessage.Sender, newMessage.MessageText, newMessage.SentAt);
        }

        public override async Task OnConnectedAsync()
        {
            foreach (var msg in ChatStorage.Messages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", msg.Sender, msg.MessageText, msg.SentAt);
            }

            await base.OnConnectedAsync();
        }

        public async Task ClearChat()
        {
            ChatStorage.Messages.Clear();
            await Clients.All.SendAsync("ChatCleared");
        }
    }
}
