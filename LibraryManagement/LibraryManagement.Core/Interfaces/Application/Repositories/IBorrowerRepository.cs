using LibraryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces.Application.Repositories
{
    public interface IBorrowerRepository
    {
        int Add(Borrower borrower);
        void Edit(Borrower borrower);
        List<Borrower> GetAll();
        Borrower? GetById(int id);
        Borrower? GetByEmail(string email);
        void Delete(Borrower borrower);
        List<CheckoutLog> GetBorrowedItems(string email);
        
    }
}
