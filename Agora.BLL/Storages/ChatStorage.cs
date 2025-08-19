using Agora.BLL.DTO;

namespace Agora.BLL.Storages
{
    public static class ChatStorage
    {
        public static List<Message> Messages = new();
        public static bool HasClientStarted { get; set; } = false;
    }
}
