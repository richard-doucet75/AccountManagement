using AccountServices.UseCases.ValueTypes;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AccountServicesApi.ValueTypeConverters
{
    public class PasswordTypeConverter : JsonConverter<Password>
    {
        public override Password Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString() ?? throw new Exception("Null password not expected");
        }

        public override void Write(Utf8JsonWriter writer, Password value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
