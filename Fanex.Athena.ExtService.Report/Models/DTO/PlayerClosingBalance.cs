namespace Fanex.Athena.ExtService.Report.Models.DTO
{
    using Converters;
    using Newtonsoft.Json;

    public class PlayerClosingBalance
    {
        [JsonProperty(PropertyName = "Username")]
        public string Nickname { get; set; }

        [JsonIgnore]
        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        [JsonProperty(PropertyName = "SystemId")]
        public string Username { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal OpeningBalance { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Deposit { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Withdrawal { get; set; }

        [JsonProperty(PropertyName = "ExtraPayout")]
        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Promotion { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Turnover { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal DrawGame { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal NetTurnover { get; set; }

        [JsonProperty(PropertyName = "PendingFunds")]
        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Outstanding { get; set; }

        [JsonProperty(PropertyName = "CustomerWL")]
        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal WinLoss { get; set; }

        [JsonProperty(PropertyName = "CustomerWLTotal")]
        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal WinLossTotal { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal ClosingBalance { get; set; }
    }
}