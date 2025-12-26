using System.Data;
using Dapper;
using Types.Types;

namespace Database.Handlers;

public class GuidCheckedTypeHandler : SqlMapper.TypeHandler<GuidChecked>
{
    public override void SetValue(IDbDataParameter parameter, GuidChecked value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value.Value;
    }

    public override GuidChecked Parse(object value)
    {
        return new GuidChecked(Guid.Parse((string)value));
    }
}