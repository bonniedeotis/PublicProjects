using LibraryManagement.Core.Entities;

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
