namespace Fanex.Athena.ExtService.Report.Converters
{
    using System;
    using Constant;
    using Newtonsoft.Json;

    public class DecimalJsonConverter : JsonConverter<decimal>
    {
        public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Convert.ToDecimal(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value.ToString(ReportOptions.NumberFormatPattern));
        }
    }
}