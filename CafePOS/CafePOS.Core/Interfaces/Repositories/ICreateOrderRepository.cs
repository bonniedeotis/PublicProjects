using CafePOS.Core.Entities;

namespace CafePOS.Core.Interfaces.Repositories
{
    public interface ICreateOrderRepository
    {
        List<Server> GetAvailableServers();
        Server? GetServer(int serverId);
        int NewOrder(Server server);
    }
}
