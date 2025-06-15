using Types.Interfaces.Database;
using Users.API.Models;

namespace Users.API.DatabaseServices.Interfaces;

public interface IUserDatabaseService : ICrud<User>;