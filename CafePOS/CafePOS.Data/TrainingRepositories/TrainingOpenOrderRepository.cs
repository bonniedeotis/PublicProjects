using CafePOS.Core.DTOs;
using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;

namespace CafePOS.Data.TrainingRepositories
{
    public class TrainingOpenOrderRepository : IOpenOrderRepository
    {
        public void AddItemsToOrder(List<ItemToAdd> orderItems)
        {
            foreach (var oi in orderItems)
            {
                var eachPrice = FakeDb.itemPrices.FirstOrDefault(ip => ip.ItemPriceID == oi.ItemPriceId).Price;

                OrderItem newItem = new OrderItem
                {
                    OrderID = oi.OrderId,
                    Quantity = (byte)oi.ItemQty,
                    ItemPriceID = oi.ItemPriceId,
                    ExtendedPrice = eachPrice * oi.ItemQty
                };
                FakeDb.orderItems.Add(newItem);
            }
        }

        public List<ItemPrice> GetAvailableItemsByCategory(int categoryId, int timeOfDay)
        {

            var itemList = FakeDb.items.Where(i => i.CategoryID == categoryId).ToList();
            var itemPriceList = FakeDb.itemPrices;

            var itemPrice = new List<ItemPrice>();

            foreach (var price in itemPriceList)
            {
                foreach (var item in itemList)
                {
                    if (price.ItemID == item.ItemID)
                    {
                        price.Item = item;
                        itemPrice.Add(price);
                    }
                }
            }

            return itemPrice;
        }

        public List<Category> GetCategories(int timeOfDay)
        {
            return FakeDb.categories.ToList();
        }

        public int GetItemPriceID(int categoryId, int itemId, int timeOfDay)
        {
            var item = FakeDb.items.Where(i => i.CategoryID == categoryId).ToList();
            var itemPrice = FakeDb.itemPrices.Where(ip => ip.ItemID == itemId).ToList();

            if (item.Exists(i => i.ItemID == itemId))
            {
                return itemPrice.FirstOrDefault(ip => itemId == itemId).ItemPriceID;
            }
            else
            {
                return 0;
            }

        }

        public List<CafeOrder> GetOpenOrders()
        {
            var orders = FakeDb.cafeOrders.Where(co => co.PaymentTypeID == null).ToList();
            var servers = FakeDb.servers;

            var openOrders = new List<CafeOrder>();

            foreach (var order in orders)
            {
                foreach (var server in servers)
                {
                    if (order.ServerID == server.ServerID)
                    {
                        Server name = new Server { FirstName = server.FirstName, LastName = server.LastName };

                        order.Server = name;

                        openOrders.Add(order);
                    }
                }
            }
            return openOrders;
        }

        public List<OrderItem> GetOrderDetails(int orderId)
        {
            var orderItemList = FakeDb.orderItems.Where(oi => oi.OrderID == orderId).ToList();
            var itemList = FakeDb.items;
            var orderDetails = new List<OrderItem>();

            foreach (var order in orderItemList)
            {
                var itemPriceId = order.ItemPriceID;
                var itemId = FakeDb.itemPrices.FirstOrDefault(ip => ip.ItemPriceID == itemPriceId).ItemID;
                var item = FakeDb.items.FirstOrDefault(i => i.ItemID == itemId);

                if (itemPriceId != null && itemId != null && item != null)
                {
                    var name = new ItemPrice { Item = new Item { ItemName = item.ItemName } };
                    order.ItemPrice = name;
                    orderDetails.Add(order);
                }
            }

            return orderDetails;
        }

        public bool IsOrderOpen(int orderId)
        {
            return FakeDb.cafeOrders.FirstOrDefault(co => co.OrderID == orderId && co.PaymentTypeID == null) != null ? true : false;
        }

        public bool IsValidOrderNumber(int orderId)
        {
            return FakeDb.cafeOrders.FirstOrDefault(co => co.OrderID == orderId) != null ? true : false;
        }
    }
}
