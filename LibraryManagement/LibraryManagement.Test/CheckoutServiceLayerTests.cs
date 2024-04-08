using LibraryManagement.Application.Services;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Data.Repositories.Test_Repositories;
using NUnit.Framework;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class CheckoutServiceLayerTests
    {
        private ICheckoutService _service;
        List<CheckoutLog> checkedOutItemList = new List<CheckoutLog> {
            new CheckoutLog
            {
                CheckoutLogID = 1, MediaID = 1, BorrowerID = 1,
                CheckoutDate = new DateTime(2024,1,3),
                DueDate = new DateTime(2024,1,10),
                ReturnDate = null,
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 1, Email = "john@email.com"}
            },
            new CheckoutLog
            {
                CheckoutLogID = 4, MediaID = 4, BorrowerID = 3,
                CheckoutDate = new DateTime(2024,1,31),
                DueDate = new DateTime(2024,2,6),
                ReturnDate = null,
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 3, Email = "sam@email.com"}
            },
                 new CheckoutLog
            {
                CheckoutLogID = 5, MediaID = 5, BorrowerID = 3,
                CheckoutDate = new DateTime(2024,1,31),
                DueDate = new DateTime(2024,2,6),
                ReturnDate = null,
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 3, Email = "sam@email.com"}
            },
              new CheckoutLog
            {
                CheckoutLogID = 6, MediaID = 6, BorrowerID = 3,
                CheckoutDate = new DateTime(2024,1,31),
                DueDate = new DateTime(2024,2,6),
                ReturnDate = null,
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 3, Email = "sam@email.com"}
            }
        };

        List<CheckoutLog> samCheckedOutItems = new List<CheckoutLog> {
                new CheckoutLog
            {
                CheckoutLogID = 4, MediaID = 4, BorrowerID = 3,
                CheckoutDate = new DateTime(2024,1,31),
                DueDate = new DateTime(2024,2,6),
                ReturnDate = null,
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 3, Email = "sam@email.com"}
            },
                new CheckoutLog
            {
                CheckoutLogID = 5, MediaID = 5, BorrowerID = 3,
                CheckoutDate = new DateTime(2024,1,31),
                DueDate = new DateTime(2024,2,6),
                ReturnDate = null,
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 3, Email = "sam@email.com"}
            },
                new CheckoutLog
            {
                CheckoutLogID = 6, MediaID = 6, BorrowerID = 3,
                CheckoutDate = new DateTime(2024,1,31),
                DueDate = new DateTime(2024,2,6),
                ReturnDate = null,
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 3, Email = "sam@email.com"}
            }
        };

        [SetUp]
        public void Init()
        {
            _service = new CheckoutService(new TestCheckoutRepository());
        }

        [Test]
        public void GetCheckedOutItems_Success()
        {
            var test = _service.GetAllCheckedOutMedia();

            var expected = ResultFactory.Success(checkedOutItemList);

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Data, Is.EqualTo(test.Data));
        }

        [Test]
        public void IsBorrowerAtLimit_true()
        {
            var test = _service.IsBorrowerAtLimit("sam@email.com");

            var expected = ResultFactory.Fail<bool>("Borrower already has 3 items checked out. Limit is 3 at a time.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }

        [Test]
        public void IsBorrowerAtLimit_false()
        {
            var test = _service.IsBorrowerAtLimit("john@email.com");

            var expected = ResultFactory.Success(false);

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Data, Is.EqualTo(test.Data));
        }

        [Test]
        public void GetBorrowerStatus_HasOverdueItems_True()
        {
            var test = _service.GetBorrowerStatus("john@email.com");

            var expected = ResultFactory.Fail("Borrower has overdue items that must be returned prior to checking out new material.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }

        [Test]
        public void GetBorrowerStatus_HasOverdueItems_False()
        {
            var test = _service.GetBorrowerStatus("jane@email.com");

            var expected = ResultFactory.Success();

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
        }

        [Test]
        public void GetBorrowerCheckedOutItems_Success()
        {
            Borrower b = new Borrower { BorrowerID = 3, Email = "sam@email.com" };
            var test = _service.GetBorrowerCheckedOutItems(b);

            var expected = ResultFactory.Success(samCheckedOutItems);

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Data, Is.EqualTo(test.Data));
        }

        [Test]
        public void GetBorrowerCheckedOutItems_Fail()
        {
            Borrower b = new Borrower { BorrowerID = 2, Email = "jane@email.com" };
            var test = _service.GetBorrowerCheckedOutItems(b);

            var expected = ResultFactory.Fail<List<CheckoutLog>>($"{b.Email} does not have any items checked out currently.");

            Assert.That(expected.Ok, Is.EqualTo(test.Ok));
            Assert.That(expected.Message, Is.EqualTo(test.Message));
        }
    }
}
