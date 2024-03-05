using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;

namespace CafePOS.Data.TrainingRepositories
{
    public class TrainingCancelOrderRepository : ICancelOrderRepository
    {
        public void CancelOrder(int orderId)
        {
            var orderDetailList = FakeDb.orderItems;

            foreach (var detail in orderDetailList.ToList())
            {
                if (detail.OrderID == orderId)
                {
                    orderDetailList.Remove(detail);
                }
            }
            FakeDb.orderItems = orderDetailList;

            var order = FakeDb.cafeOrders.First(co => co.OrderID == orderId);
            FakeDb.cafeOrders.Remove(order);

            var o = FakeDb.cafeOrders;
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
