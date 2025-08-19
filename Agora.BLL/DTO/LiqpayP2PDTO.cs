using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Agora.BLL.DTO
{
    public class LiqpayP2PDTO
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("public_key")]
        public string PublicKey { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }   

        [JsonProperty("result_url")]
        public string ResultUrl { get; set; }

        [JsonProperty("sandbox")]
        public int Sandbox { get; set; }

        [JsonProperty("receiver_card")]
        public string ReceiverCard { get; set; }

        [JsonProperty("receiver_last_name")]
        public string ReceiverLastname { get; set; }

        [JsonProperty("receiver_first_name")]
        public string ReceiverFirstName { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }



    }
}
