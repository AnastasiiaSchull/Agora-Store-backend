namespace Agora.BLL.DTO
{
    public class ImageMessage
    {
        public string Base64Image { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Sender { get; set; }
        public string Timestamp { get; set; }
    }
}
