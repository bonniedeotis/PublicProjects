using CafePOS.Core.Entities;

namespace CafePOS.Core.Interfaces.Repositories
{
    public interface ICancelOrderRepository
    {
        List<CafeOrder> GetOpenOrders();
        void CancelOrder(int orderId);
    }
}
