using System.Text.Json;
using System.Text.Json.Serialization;
using Users.Domain.DataTransferObjects;

namespace Client.Net;

[JsonSerializable(typeof(LoginResultDto))]
[JsonSerializable(typeof(LoginDto))]
[JsonSerializable(typeof(RegisterDto))]
public partial class JsonContext : JsonSerializerContext;