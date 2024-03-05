using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;

namespace CafePOS.Tests.Mocks.PaymentRepo
{
    public class Over15Items : IPaymentRepository
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
            return false;
        }

        public bool IsValidPaymentType(int paymentType)
        {
            throw new NotImplementedException();
        }

        public bool OrderHasItems(int orderId)
        {
            throw new NotImplementedException();
        }

        public void ProcessPayment(int orderId, int paymentType)
        {
            throw new NotImplementedException();
        }
    }
}
