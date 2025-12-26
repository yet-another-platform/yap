using System.Text.Json;
using System.Text.Json.Serialization;
using Types.Types;

namespace Types.JsonConvertors;

public class CorrelationIdJsonConverter : JsonConverter<CorrelationId>
{
    public override CorrelationId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Unexpected token parsing CorrelationId. Expected String, got {reader.TokenType}.");
        }

        string str = reader.GetString()!;
        return !CorrelationId.TryParse(str, out var result) ? throw new JsonException($"Invalid CorrelationId value: {str}") : result;
    }

    public override void Write(Utf8JsonWriter writer, CorrelationId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.Value.ToString());
    }
}