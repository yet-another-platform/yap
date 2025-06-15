namespace Types.Types;

public readonly struct GuidChecked : IEquatable<GuidChecked>, IComparable<GuidChecked>, IEquatable<Guid>,
    IComparable<Guid>
{
    public Guid Value { get; }

    public GuidChecked(Guid value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Guid is not valid!", nameof(value));
        }

        Value = value;
    }

    public static bool IsValid(Guid value) => value != Guid.Empty;

    public static implicit operator GuidChecked(Guid value) => new(value);
    public static implicit operator Guid(GuidChecked value) => value.Value;

    public override string ToString() => Value.ToString();
    public override int GetHashCode() => Value.GetHashCode();

    public bool Equals(GuidChecked other)
    {
        return Value.Equals(other.Value);
    }

    public int CompareTo(GuidChecked other)
    {
        return Value.CompareTo(other.Value);
    }

    public bool Equals(Guid other)
    {
        return Value.Equals(Value);
    }

    public int CompareTo(Guid other)
    {
        return Value.CompareTo(other);
    }

    public override bool Equals(object? obj) => obj is GuidChecked value && Equals(value);
}