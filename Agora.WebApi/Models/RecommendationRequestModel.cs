namespace Agora.Models
{
    public class RecommendationRequestModel
    {
        public List<string> Queries { get; set; } = new();
        public List<int> BasketIds { get; set; } = new();
    }
}
