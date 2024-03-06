using LibraryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
