

using Agora.Enums;

namespace Agora.Models
{
    public class DhlPayload
    {
        public ShippingStatus Status { get; set; }
        public string TrackingNumber { get; set; }

    }
}
