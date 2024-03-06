using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Data.Repositories.TestRepositories
{
    public class TestBorrowerRepository : IBorrowerRepository
    {
        List<Borrower> borrowers = new List<Borrower>
        {
            new Borrower {
                BorrowerID = 1,
                Email = "john@email.com",
                FirstName = "John",
                LastName = "Doe",
                Phone = "11122223333",
                CheckoutLogs = new List<CheckoutLog>{
                    new CheckoutLog {
                        MediaID = 1,
                        BorrowerID = 1,
                        CheckoutDate = DateTime.Today
                    } }
                },
            new Borrower { BorrowerID = 2, Email = "jane@email.com", FirstName = "Jane", LastName = "Smith", Phone = "2223334444" }
        };

        public int Add(Borrower borrower)
        {
            borrower.BorrowerID = borrowers.OrderBy(b => b.BorrowerID).Last().BorrowerID + 1;
            borrowers.Add(borrower);
            return borrower.BorrowerID;
        }

        public void Delete(Borrower borrower)
        {
            throw new NotImplementedException();
        }

        public void Edit(Borrower borrower)
        {
            var b = borrowers.FirstOrDefault(b => b.BorrowerID == borrower.BorrowerID);

            if (b != null)
            {
                b.Email = borrower.Email;
                b.FirstName = borrower.FirstName;
                b.LastName = borrower.LastName;
            }
        }

        public List<Borrower> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<CheckoutLog> GetBorrowedItems(string email)
        {
            return borrowers.FirstOrDefault(b => b.Email == email).CheckoutLogs;
        }

        public Borrower? GetByEmail(string email)
        {
            return borrowers.FirstOrDefault(b => b.Email == email);
        }

        public Borrower? GetById(int id)
        {
            return borrowers.FirstOrDefault(b => b.BorrowerID == id);
        }
    }
}
