using Types.Types;

namespace Types.Extensions;

public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? guid)
    {
        return guid == null || guid == Guid.Empty;
    }

    public static bool IsNotNullOrEmpty(this Guid? guid) => !IsNullOrEmpty(guid);
    
    public static bool IsEmpty(this Guid guid)
    {
        return guid == Guid.Empty;
    }

    public static bool IsNotEmpty(this Guid guid) => !IsEmpty(guid);
    
    public static List<GuidChecked> ToCheckedList(this IEnumerable<Guid> guids) => guids.Select(g => new GuidChecked(g)).ToList();
}