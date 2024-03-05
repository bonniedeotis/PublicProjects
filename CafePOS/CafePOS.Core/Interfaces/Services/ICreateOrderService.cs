using CafePOS.Core.DTOs;
using CafePOS.Core.Entities;

namespace CafePOS.Core.Interfaces.Services
{
    public interface ICreateOrderService
    {
        Result<List<Server>> GetServerList();
        Result<Server> GetServer(int ID);
        Result<int> CreateNewOrder(Server server);
    }
}
