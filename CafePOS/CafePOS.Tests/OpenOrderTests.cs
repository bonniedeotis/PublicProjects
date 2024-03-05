using CafePOS.Application.Mocks;
using CafePOS.Application.Services;
using CafePOS.Tests.Mocks.OpenOrderRepo;
using NUnit.Framework;

namespace CafePOS.Tests
{
    [TestFixture]
    public class OpenOrderTests
    {
        [Test]
        public void IsValidOpenOrder_Fail_OrderIsClosed()
        {
            var service = new OpenOrderService(new ClosedOutOrder(), new Breakfast());

            var result = service.IsValidOpenOrder(5000);

            Assert.That(result.Ok, Is.False);
        }

        [Test]
        public void IsValidOpenOrder_Fail_InvalidOrderNumber()
        {
            var service = new OpenOrderService(new InvalidOrderNumber(), new Breakfast());

            var result = service.IsValidOpenOrder(5000);

            Assert.That(result.Ok, Is.False);
        }

        [Test]
        public void IsValidOpenOrder_Pass()
        {
            var service = new OpenOrderService(new OpenValidOrder(), new Breakfast());

            var result = service.IsValidOpenOrder(5000);

            Assert.That(result.Ok, Is.True);
        }
    }
}
