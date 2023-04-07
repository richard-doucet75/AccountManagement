using AccountServices.UseCases.ValueTypes;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AccountServicesApi.ValueTypeConverters
{
    public class EmailAddressTypeConverter : JsonConverter<EmailAddress>
    {
        public override EmailAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString() ?? throw new Exception("Null email address not expected");
        }

        public override void Write(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
