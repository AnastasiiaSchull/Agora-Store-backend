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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ChatStorage.Messages.Clear();
            ChatStorage.HasClientStarted = false;

            await Clients.All.SendAsync("ChatCleared");
            await Clients.All.SendAsync("ChatStatusChanged", false);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task UserLeftChat()
        {
            ChatStorage.Messages.Clear();
            ChatStorage.HasClientStarted = false;

            await Clients.All.SendAsync("ChatCleared");
            await Clients.All.SendAsync("ChatStatusChanged", false);
        }

        public async Task ClearChat()
        {
            ChatStorage.Messages.Clear();
            await Clients.All.SendAsync("ChatCleared");
        }

        public async Task SendImage(ImageMessage message)
        {
            Console.WriteLine($"Image received: {message.FileName}, size: {message.Base64Image.Length} chars");
            Console.WriteLine($"===> ReceiveImage called. Filename: {message.FileName}, Length: {message.Base64Image?.Length}");
            await Clients.All.SendAsync("ReceiveImage", message);
        }
    }
}
