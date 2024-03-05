using CafePOS.Core.DTOs;
using CafePOS.Core.Entities;

namespace CafePOS.Core.Interfaces.Services
{
    public interface ICancelOrderService
    {
        Result<List<CafeOrder>> GetOpenOrders();
        Result<bool> IsValidOpenOrder(int orderId);
        Result CancelOrder(int orderId);
    }
}
