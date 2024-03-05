using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;

namespace CafePOS.Tests.Mocks.PaymentRepo
{
    public class OrderHasNoItems : IPaymentRepository
    {
        public void AddTipToOrder(int orderId, decimal tip)
        {
            throw new NotImplementedException();
        }

        public decimal GetFinalTotal(int orderId)
        {
            throw new NotImplementedException();
        }

        public CafeOrder GetOrderSubtotals(int orderId)
        {
            throw new NotImplementedException();
        }

        public List<PaymentType> GetPaymentTypes()
        {
            throw new NotImplementedException();
        }

        public bool IsOrderUnder15Items(int orderId)
        {
            throw new NotImplementedException();
        }

        public bool IsValidPaymentType(int paymentType)
        {
            throw new NotImplementedException();
        }

        public bool OrderHasItems(int orderId)
        {
            return false;
        }

        public void ProcessPayment(int orderId, int paymentType)
        {
            throw new NotImplementedException();
        }
    }
}
