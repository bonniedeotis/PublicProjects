using CafePOS.Core.DTOs;
using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;

namespace CafePOS.Tests.Mocks.OpenOrderRepo
{
    public class OpenValidOrder : IOpenOrderRepository
    {
        public void AddItemsToOrder(List<ItemToAdd> orderItems)
        {
            throw new NotImplementedException();
        }

        public List<ItemPrice> GetAvailableItemsByCategory(int categoryId, int timeOfDay)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategories(int timeOfDay)
        {
            throw new NotImplementedException();
        }

        public int GetItemPriceID(int categoryId, int itemId, int timeOfDay)
        {
            throw new NotImplementedException();
        }

        public List<CafeOrder> GetOpenOrders()
        {
            throw new NotImplementedException();
        }

        public List<OrderItem> GetOrderDetails(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool IsOrderOpen(int orderId)
        {
            return true;
        }

        public bool IsValidOrderNumber(int orderId)
        {
            return true;
        }
    }
}
