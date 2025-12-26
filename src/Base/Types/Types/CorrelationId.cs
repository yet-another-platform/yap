using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Types.JsonConvertors;

namespace Types.Types;

[JsonConverter(typeof(CorrelationIdJsonConverter))]
public record CorrelationId : IParsable<CorrelationId>
{
    public GuidChecked Value { get; }

    public CorrelationId()
    {
        Value = new GuidChecked(Guid.NewGuid());
    }

    public CorrelationId(GuidChecked value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();

    public static CorrelationId Parse(string s) => Parse(s, null);

    public static CorrelationId Parse(string s, IFormatProvider? provider)
    {
        return new CorrelationId(GuidChecked.Parse(s));
    }

    public static bool TryParse([NotNullWhen(true)] string? s, out CorrelationId result) => TryParse(s, null, out result);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out CorrelationId result)
    {
        if (!GuidChecked.TryParse(s, out var guid))
        {
            result = null!;
            return false;
        }

        result = new CorrelationId(guid);
        return true;
    }
}