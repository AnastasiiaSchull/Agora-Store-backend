using Newtonsoft.Json;

namespace Agora.BLL.DTO
{
    public class LiqpayDTO
    {
        // Required parameters
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

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("result_url")]
        public string ResultUrl { get; set; }

        /// <summary>
        /// Test mode: 1 - test, 0 - production (in test mode money isn't withdrawn)
        /// </summary>
        [JsonProperty("sandbox")]
        public int Sandbox { get; set; }

        // Additional information
        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("product_category")]
        public string ProductCategory { get; set; }

        [JsonProperty("product_description")]
        public string ProductDescription { get; set; }

        [JsonProperty("product_url")]
        public string ProductUrl { get; set; }
    }
}
