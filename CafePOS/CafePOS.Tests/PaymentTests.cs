using CafePOS.Application.Services;
using CafePOS.Tests.Mocks.PaymentRepo;
using NUnit.Framework;

namespace CafePOS.Tests
{
    [TestFixture]
    public class PaymentTests
    {
        [Test]
        public void OrderIsUnder15Items_Fail_Has20Items()
        {
            var service = new PaymentService(new Over15Items());

            var result = service.OrderIsUnder15items(5000);

            Assert.That(result.Ok, Is.False);
        }

        [Test]
        public void OrderHasItems_Fail_Has0Items()
        {
            var service = new PaymentService(new OrderHasNoItems());

            var result = service.OrderHasItems(5000);

            Assert.That(result.Ok, Is.False);
        }

        [Test]
        public void OrderHasItems_Pass()
        {
            var service = new PaymentService(new OrderHas5Items());

            var result = service.OrderHasItems(5000);

            Assert.That(result.Ok, Is.True);
        }

        [Test]
        public void OrderIsUnder15Items_Pass()
        {
            var service = new PaymentService(new OrderHas5Items());

            var result = service.OrderIsUnder15items(5000);

            Assert.That(result.Ok, Is.True);
        }
    }
}
