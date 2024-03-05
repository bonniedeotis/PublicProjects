using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;
using System.Data;

namespace CafePOS.Data.Repositories
{
    public class CreateOrderRepository : ICreateOrderRepository
    {
        private readonly CafeContext _dbContext;

        public CreateOrderRepository(string connectionString)
        {
            _dbContext = new CafeContext(connectionString);
        }

        public List<Server> GetAvailableServers()
        {
            return _dbContext.Server.Where(s => s.HireDate < DateTime.Today && s.TermDate == null).ToList();
        }

        public Server? GetServer(int serverId)
        {
            return _dbContext.Server
                .FirstOrDefault(s => s.ServerID == serverId && s.HireDate < DateTime.Today && s.TermDate == null);
        }

        public int NewOrder(Server server)
        {
            var order = new CafeOrder();
            order.Server = server;
            order.OrderDate = DateTime.Now;
            _dbContext.CafeOrder.Add(order);
            _dbContext.SaveChanges();

            return order.OrderID;
        }
    }
}
