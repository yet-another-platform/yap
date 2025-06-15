using Microsoft.AspNetCore.Mvc;

namespace Types.Types.Option;

public class Option<T>
{
    private readonly T? _value;
    public T Value => Ok ? _value! : throw new MemberAccessException();

    private readonly Error? _error;
    public Error Error => Ok ? throw new MemberAccessException() : _error!;

    public bool Ok { get; init; }

    public Option(T value)
    {
        _value = value;
        _error = null;
        Ok = true;
    }

    public Option(Error error)
    {
        _error = error;
        _value = default;
        Ok = false;
    }
    
    public static implicit operator T(Option<T> option) => option.Value;
    public static implicit operator Option<T>(T value) => new (value);
    public static implicit operator Option<T>(Error error) => new(error);
}
