using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;

namespace CafePOS.Data.TrainingRepositories
{
    public class TrainingPaymentRepository : IPaymentRepository
    {
        public void AddTipToOrder(int orderId, decimal tip)
        {
            FakeDb.cafeOrders.FirstOrDefault(co => co.OrderID == orderId).Tip = tip;
        }

        public decimal GetFinalTotal(int orderId)
        {
            return (decimal)FakeDb.cafeOrders.FirstOrDefault(co => co.OrderID == orderId).AmountDue;
        }

        public CafeOrder GetOrderSubtotals(int orderId)
        {
            var order = FakeDb.cafeOrders.FirstOrDefault(co => co.OrderID == orderId && co.PaymentTypeID == null);
            var orderIndex = FakeDb.cafeOrders.IndexOf(order);

            var orderItems = FakeDb.orderItems.Where(ci => ci.OrderID == orderId).ToList();

            if (orderItems.Count > 0)
            {
                order.SubTotal = orderItems.Sum(oi => oi.ExtendedPrice);
                order.Tax = order.SubTotal * .05M;
                order.AmountDue = order.SubTotal + order.Tax;
            }

            FakeDb.cafeOrders[orderIndex] = order;

            return order;
        }

        public List<PaymentType> GetPaymentTypes()
        {
            return FakeDb.paymentTypes.ToList();
        }

        public bool IsOrderUnder15Items(int orderId)
        {
            return FakeDb.orderItems.Where(oi => oi.OrderID == orderId).Sum(oi => oi.Quantity) < 15 ? true : false;
        }

        public bool IsValidPaymentType(int paymentType)
        {
            return FakeDb.paymentTypes.FirstOrDefault(pt => pt.PaymentTypeID == paymentType) != null ? true : false;
        }

        public bool OrderHasItems(int orderId)
        {
            return FakeDb.orderItems.Any(oi => oi.OrderID == orderId) ? true : false;
        }

        public void ProcessPayment(int orderId, int paymentType)
        {
            var order = FakeDb.cafeOrders.FirstOrDefault(co => co.OrderID == orderId);
            var orderIndex = FakeDb.cafeOrders.IndexOf(order);

            order.PaymentTypeID = paymentType;
            order.AmountDue = order.SubTotal + order.Tax + order.Tip;

            FakeDb.cafeOrders[orderIndex] = order;

            var verificaiton = FakeDb.cafeOrders.FirstOrDefault(co => co.OrderID == orderId);
        }
    }
}
