using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagement.Data.Repositories.EF_Repositories
{
    // created to work with ET but could swap out for a different implementation that uses ADO or Dapper
    // could also swap for one with a different data source like a file or API
    // data layer does not do validation/calculations - just assumes the data is good
    // application logic belongs in the application layer

    public class EFBorrowerRepository : IBorrowerRepository
    {
        private LibraryContext _dbContext;

        public EFBorrowerRepository(string connectionString)
        {
            _dbContext = new LibraryContext(connectionString);
        }
        public int Add(Borrower borrower)
        {
            _dbContext.Borrower.Add(borrower);
            _dbContext.SaveChanges();
            return borrower.BorrowerID;
        }

        public List<Borrower> GetAll()
        {
            return _dbContext.Borrower.ToList();
        }

        public Borrower? GetByEmail(string email)
        {
            return _dbContext.Borrower.FirstOrDefault(b => b.Email == email);
        }

        public Borrower? GetById(int id)
        {
            return _dbContext.Borrower.FirstOrDefault(b => b.BorrowerID == id); //if not found, return null
        }

        public void Delete(Borrower borrower)
        {
            var history = _dbContext.CheckoutLog.Where(cl => cl.BorrowerID == borrower.BorrowerID);

            var b = _dbContext.Borrower.FirstOrDefault(b => b.BorrowerID == borrower.BorrowerID);

            if (b != null)
            {
                _dbContext.CheckoutLog.RemoveRange(history);
                _dbContext.Borrower.Remove(b);

                _dbContext.SaveChanges();
            }
        }

        public void Edit(Borrower borrower)
        {
            var b = _dbContext.Borrower.FirstOrDefault(b => b.BorrowerID == borrower.BorrowerID);

            if (b != null)
            {
                b.Email = borrower.Email;
                b.FirstName = borrower.FirstName;
                b.LastName = borrower.LastName;

                _dbContext.SaveChanges();
            }
        }

        public List<CheckoutLog> GetBorrowedItems(string email)
        {
            return _dbContext.CheckoutLog
                .Where(cl => cl.Borrower.Email == email && cl.ReturnDate == null)
                .Include(cl => cl.Media)
                .ToList();
        }
    }
}
