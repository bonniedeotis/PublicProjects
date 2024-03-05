using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;

namespace CafePOS.Data.TrainingRepositories
{
    public class TrainingCreateOrderRepository : ICreateOrderRepository
    {
        public List<Server> GetAvailableServers()
        {
            return FakeDb.servers.ToList();
        }

        public Server? GetServer(int serverId)
        {
            return FakeDb.servers
               .FirstOrDefault(s => s.ServerID == serverId);
        }

        public int NewOrder(Server server)
        {
            var order = new CafeOrder();
            order.OrderID = FakeDb.cafeOrders.Count() + 1;
            order.ServerID = server.ServerID;
            order.OrderDate = DateTime.Now;

            FakeDb.cafeOrders.Add(order);

            return order.OrderID;
        }
    }
}
