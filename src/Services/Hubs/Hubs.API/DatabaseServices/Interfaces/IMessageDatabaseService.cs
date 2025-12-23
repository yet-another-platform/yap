using Hubs.API.Models;
using Types.Interfaces.Database;

namespace Hubs.API.DatabaseServices.Interfaces;

public interface IMessageDatabaseService : ICrud<long, Message>
{
    
}