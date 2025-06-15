using Users.Domain.Enums;

namespace Users.API.Helpers;

public static class UserStateHelper
{
    public static UserState[] GetInvalidStates()
    {
        return [UserState.Unknown, UserState.Deleted, UserState.Banned];
    }
    
    public static int[] GetInvalidStatesAsInts() => GetInvalidStates().Cast<int>().ToArray();
}