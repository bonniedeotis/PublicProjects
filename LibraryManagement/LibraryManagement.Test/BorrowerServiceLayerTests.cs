using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Data.Repositories.TestRepositories;
using NUnit.Framework;


namespace LibraryManagement.Test
{
    [TestFixture]

    public class BorrowerServiceLayerTests
    {
        private IBorrowerService _service = new BorrowerService(new TestBorrowerRepository());

        Borrower testBorrower1 = new Borrower { FirstName = "Sam", LastName = "Johnson", Email = "sam@email.com", Phone = "3334445555" };
        Borrower testBorrower2 = new Borrower { FirstName = "John", LastName = "Doe", Email = "john@email.com", Phone = "3334445555" };
        Borrower testBorrower3 = new Borrower { FirstName = "Jon", LastName = "Joe", Email = "jon@email.com", Phone = "3334445555" };
        Borrower testBorrower4 = new Borrower { FirstName = "Jane", LastName = "Johnson", Email = "jane@email.com", Phone = "3334445555" };

        [Test]
        public void AddBorrower_Success()
        {
            var test = _service.AddBorrower(testBorrower1);

            var expected = ResultFactory.Success();

            Assert.That(3, Is.EqualTo(test.Data));
            Assert.That(expected.Ok, Is.EqualTo(test.Ok));

        }

        [Test]
        public void AddBorrower_Fail()
        {
            var test = _service.AddBorrower(testBorrower2);

            var expected = ResultFactory.Fail<int>("");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void EditBorrower_Success()
        {
            var test = _service.EditBorrower(testBorrower3, "john@email.com");

            var expected = ResultFactory.Success();

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void EditBorrower_Fail()
        {
            var test = _service.EditBorrower(testBorrower4, "john@email.com");

            var expected = ResultFactory.Fail("");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void GetBorrowerByEmail_Success()
        {
            var test = _service.GetBorrower("john@email.com");

            var expectedResult = ResultFactory.Success();
            var expectedData = new Borrower { Email = "john@email.com" };

            Assert.That(expectedResult.Ok, Is.EqualTo(test.Ok));
            Assert.That(expectedData.Email, Is.EqualTo(test.Data.Email));
        }

        [Test]
        public void GetBorrowerByEmail_Fail()
        {
            var test = _service.GetBorrower("chris@email.com");

            var expected = ResultFactory.Fail<Borrower>("");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void GetBorroweredItems_Success()
        {
            var test = _service.GetBorrowedItems("john@email.com");

            var expected = ResultFactory.Success();

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void GetBorroweredItems_Fail()
        {
            var test = _service.GetBorrowedItems("sam@email.com");

            var expected = ResultFactory.Fail("");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }
    }
}
