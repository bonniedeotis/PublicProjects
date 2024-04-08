using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface ICheckoutService
    {
        Result<List<Media>> GetAvailableMedia();
        Result<List<CheckoutLog>> GetAllCheckedOutMedia();
        Result<Borrower> GetBorrowerByEmail(string email);
        Result<DateTime> Checkout(int itemID, string email);
        Result Return(int itemID);
        Result<List<CheckoutLog>> GetBorrowerCheckedOutItems(Borrower borrower);
        Result GetBorrowerStatus(string email);
        Result<bool> IsBorrowerAtLimit(string email);
    }
}
