using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Services
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IBorrowerRepository _borrowerRepository;

        public BorrowerService(IBorrowerRepository borrowerRepository) //pass in a borrower repo
        {
            _borrowerRepository = borrowerRepository;
        }

        public Result<int> AddBorrower(Borrower newBorrower)
        {
            try
            {
                var duplicate = _borrowerRepository.GetByEmail(newBorrower.Email);
                if (duplicate != null)
                {
                    return ResultFactory.Fail<int>($"Borrower with email: {newBorrower.Email} already exists!");
                }

                var id = _borrowerRepository.Add(newBorrower);
                return ResultFactory.Success(id);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<int>(ex.Message);
            }
        }

        public Result EditBorrower(Borrower borrower, string originalEmail)
        {
            try
            {
                var duplicate = _borrowerRepository.GetByEmail(borrower.Email);

                if (duplicate != null && duplicate.Email != originalEmail)
                {
                    return ResultFactory.Fail($"Borrower with email: {borrower.Email} already exists!");
                }

                _borrowerRepository.Edit(borrower);
                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }

        public Result<List<Borrower>> GetAllBorrowers()
        {
            try
            {
                var borrowers = _borrowerRepository.GetAll();
                return ResultFactory.Success(borrowers);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Borrower>>(ex.Message);
            }
        }

        public Result<List<CheckoutLog>> GetBorrowedItems(string email)
        {
            try
            {
                var items = _borrowerRepository.GetBorrowedItems(email);

                if (items.Count == 0)
                {
                    return ResultFactory.Fail<List<CheckoutLog>>("No items are currently checked out to this borrower.");
                }
                return ResultFactory.Success(items);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
            }

        }

        public Result<Borrower> GetBorrower(string email)
        {
            try
            {
                var borrower = _borrowerRepository.GetByEmail(email);

                return borrower is null ?
                    ResultFactory.Fail<Borrower>($"Borrower with email:{email} not found!") :
                    ResultFactory.Success(borrower);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Borrower>(ex.Message);
            }
        }

        public Result RemoveBorrower(Borrower borrower)
        {
            try
            {
                _borrowerRepository.Delete(borrower);
                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }
    }
}
