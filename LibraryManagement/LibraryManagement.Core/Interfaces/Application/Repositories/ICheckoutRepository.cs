using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Application.Repositories
{
    public interface ICheckoutRepository
    {
        List<Media> GetAvailableMedia();

        List<CheckoutLog> AllCheckedOutItems();

        Borrower? GetBorrowerByEmail(string email);
        DateTime? Checkout(int itemID, Borrower borrower);
        void Return(int ItemID);
        List<CheckoutLog> GetBorrowerCheckoutLog(Borrower borrower);
        CheckoutLog? GetCheckedOutItem(int itemID);
        bool HasOverdueItems(Borrower borrower);
        bool IsAtMaxItems(Borrower borrower);
        bool IsItemCheckedOut(int itemID);
    }
}
