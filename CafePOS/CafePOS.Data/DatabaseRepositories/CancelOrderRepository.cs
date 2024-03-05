using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace CafePOS.Data.Repositories
{
    public class CancelOrderRepository : ICancelOrderRepository
    {
        private readonly CafeContext _dbContext;

        public CancelOrderRepository(string connectionString)
        {
            _dbContext = new CafeContext(connectionString);
        }

        // CANCEL ORDER
        public List<CafeOrder> GetOpenOrders()
        {
            return _dbContext.CafeOrder
                .Include(co => co.Server)
                .Where(co => co.PaymentTypeID == null)
                .ToList();
        }

        public void CancelOrder(int orderId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.OrderItem.Where(oi => oi.OrderID == orderId).ExecuteDelete();

                _dbContext.CafeOrder.Where(co => co.OrderID == orderId).ExecuteDelete();

                transaction.Commit();
            }
        }
    }
}
