using LibraryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IBorrowerService
    {
        Result<List<Borrower>> GetAllBorrowers();
        Result<Borrower> GetBorrower(string email);
        Result<int> AddBorrower(Borrower newBorrower);
        Result RemoveBorrower(Borrower borrower);
        Result EditBorrower(Borrower borrower, string email);
        Result<List<CheckoutLog>> GetBorrowedItems(string email);
    }
}
