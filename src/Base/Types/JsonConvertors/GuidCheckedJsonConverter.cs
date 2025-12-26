using System.Text.Json;
using System.Text.Json.Serialization;
using Types.Types;

namespace Types.JsonConvertors;

public class GuidCheckedJsonConverter : JsonConverter<GuidChecked>
{
    public override GuidChecked Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Unexpected token parsing GuidChecked. Expected String, got {reader.TokenType}.");
        }

        string str = reader.GetString()!;
        return !GuidChecked.TryParse(str, out var result) ? throw new JsonException($"Invalid GuidChecked value: {str}") : result;
    }

    public override void Write(Utf8JsonWriter writer, GuidChecked value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}