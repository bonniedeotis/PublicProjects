using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;

namespace LibraryManagement.Data.Repositories.Test_Repositories
{
    public class TestCheckoutRepository : ICheckoutRepository
    {
        List<CheckoutLog> checkoutLogs = new List<CheckoutLog> {
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
                CheckoutLogID = 2, MediaID = 2, BorrowerID = 1,
                CheckoutDate = new DateTime(2024,1,3),
                DueDate = new DateTime(2024,1,10),
                ReturnDate = new DateTime(2024,1,9),
                Media = new Media{ IsArchived = true},
                Borrower = new Borrower{BorrowerID = 1, Email = "john@email.com"}
            },
            new CheckoutLog
            {
                CheckoutLogID = 3, MediaID = 3, BorrowerID = 2,
                CheckoutDate = new DateTime(2024,1,3),
                DueDate = new DateTime(2024,1,10),
                ReturnDate = new DateTime(2024,1,9),
                Media = new Media{ IsArchived = false},
                Borrower = new Borrower{BorrowerID = 2, Email = "jane@email.com"}
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

        public List<CheckoutLog> AllCheckedOutItems()
        {
            var media = checkoutLogs
                .Where(m => m.ReturnDate == null)
                .ToList();

            return media;
        }

        public DateTime? Checkout(int itemID, Borrower borrower)
        {
            return DateTime.Today.AddDays(7);
        }

        public List<Media> GetAvailableMedia()
        {
            return checkoutLogs.Where(cl => cl.Media.IsArchived == false && cl.ReturnDate != null).Select(m => m.Media).ToList();
        }

        public Borrower? GetBorrowerByEmail(string email)
        {
            return checkoutLogs.FirstOrDefault(cl => cl.Borrower.Email == email).Borrower;
        }

        public List<CheckoutLog> GetBorrowerCheckoutLog(Borrower borrower)
        {
            var cl = checkoutLogs.Where(cl => cl.BorrowerID == borrower.BorrowerID && cl.ReturnDate == null).ToList();
            return cl;
        }

        public CheckoutLog? GetCheckedOutItem(int itemID)
        {
            return checkoutLogs.FirstOrDefault(cl => cl.MediaID == itemID && cl.ReturnDate == null);
        }

        public bool HasOverdueItems(Borrower borrower)
        {
            var a = checkoutLogs.FirstOrDefault(cl => cl.BorrowerID == borrower.BorrowerID && cl.DueDate < DateTime.Today && cl.ReturnDate == null);
            var b = a != null ? true : false;
            return b;
        }

        public bool IsAtMaxItems(Borrower borrower)
        {
            return checkoutLogs.Where(cl => cl.BorrowerID == borrower.BorrowerID).Count() >= 3 ? true : false;
        }

        public bool IsItemCheckedOut(int itemID)
        {
            return checkoutLogs.FirstOrDefault(cl => cl.MediaID == itemID && cl.ReturnDate == null) != null ? true : false;
        }

        public void Return(int ItemID)
        {
            throw new NotImplementedException();
        }
    }
}
