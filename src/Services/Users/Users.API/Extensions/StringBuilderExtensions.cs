using System.Text;
using Dapper;
using Users.API.Constants.Database;
using Users.API.Helpers;
using Users.Domain.Enums;

namespace Users.API.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendValidUserStateFilter(this StringBuilder builder, DynamicParameters parameters,
        bool includeInvalid, bool useWhere = false)
    {
        if (includeInvalid)
        {
            return builder;
        }

        string keyword = useWhere ? "WHERE" : "AND";
        builder.AppendLine($"\n{keyword} {UsersTable.State} != ANY(@states)");
        parameters.Add("states", UserStateHelper.GetInvalidStatesAsInts());
        return builder;
    }
}