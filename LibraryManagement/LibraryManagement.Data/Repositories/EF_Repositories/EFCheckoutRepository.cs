using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Data.Repositories.EF_Repositories
{
    public class EFCheckoutRepository : ICheckoutRepository
    {
        private LibraryContext _dbContext;

        public EFCheckoutRepository(string connectionString)
        {
            _dbContext = new LibraryContext(connectionString);
        }

        public List<CheckoutLog> AllCheckedOutItems()
        {
            //var media = _dbContext.Media
            //    .Where(m => m.CheckoutLogs.Count(cl => cl.ReturnDate == null) > 0)
            //    .Include(m => m.CheckoutLogs)
            //        .ThenInclude(cl => cl.Borrower)
            //    .Include(m => m.MediaType)
            //    .ToList();

            var m = _dbContext.CheckoutLog
                .Where(m => m.ReturnDate == null)
                .Include(m => m.Borrower)
                .Include(m => m.Media)
                .ToList();

            return m;
        }

        public DateTime? Checkout(int itemID, Borrower borrower)
        {
            CheckoutLog log = new CheckoutLog();

            log.Borrower = borrower;
            log.BorrowerID = borrower.BorrowerID;
            log.MediaID = itemID;
            log.CheckoutDate = DateTime.Today;
            log.DueDate = DateTime.Today.AddDays(7);

            _dbContext.Add(log);
            _dbContext.SaveChanges();

            return log.DueDate;
        }

        public List<Media> GetAvailableMedia()
        {
            return _dbContext.Media.Where(m => m.IsArchived == false &&
            !m.CheckoutLogs.Any(cl => cl.ReturnDate == null))
                .Include(m => m.MediaType)
                .ToList();
        }

        public Borrower GetBorrowerByEmail(string email)
        {
            return _dbContext.Borrower.FirstOrDefault(b => b.Email == email);
        }

        public CheckoutLog? GetCheckedOutItem(int itemID)
        {
            return _dbContext.CheckoutLog
                .Include(cl => cl.Media)
                .FirstOrDefault(cl => cl.MediaID == itemID && cl.ReturnDate == null);
        }

        public List<CheckoutLog> GetBorrowerCheckoutLog(Borrower borrower)
        {
            return _dbContext.CheckoutLog
                .Where(cl => cl.BorrowerID == borrower.BorrowerID && cl.ReturnDate == null)
                .Include(cl => cl.Media)
                .ThenInclude(m => m.MediaType)
                .ToList();
        }

        public void Return(int itemID)
        {
            var item = _dbContext.CheckoutLog.FirstOrDefault(cl => cl.MediaID == itemID && cl.ReturnDate == null);

            item.ReturnDate = DateTime.Today;

            _dbContext.SaveChanges();
        }

        public bool HasOverdueItems(Borrower borrower)
        {
            var overdue = _dbContext.CheckoutLog.Where(cl => cl.BorrowerID == borrower.BorrowerID && cl.ReturnDate == null && cl.DueDate < DateTime.Today).Count();

            return overdue > 0 ? true : false;
        }

        public bool IsAtMaxItems(Borrower borrower)
        {
            var numOfCheckedOutItems = _dbContext.CheckoutLog.Where(cl => cl.BorrowerID == borrower.BorrowerID && cl.ReturnDate == null).Count();

            return numOfCheckedOutItems >= 3 ? true : false;
        }

        public bool IsItemCheckedOut(int itemID)
        {
            var item = _dbContext.CheckoutLog.FirstOrDefault(cl => cl.MediaID == itemID && cl.ReturnDate == null);

            return item == null ? false : true;
        }
    }
}
