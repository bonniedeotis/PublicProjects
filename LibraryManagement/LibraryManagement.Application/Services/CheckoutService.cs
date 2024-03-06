using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Application.Repositories;
using LibraryManagement.Core.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlTypes;

namespace LibraryManagement.Application.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IMediaRespository _mediaRepository;

        public CheckoutService(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public Result<DateTime> Checkout(int itemID, string email)
        {
            try
            {
                var borrower = _checkoutRepository.GetBorrowerByEmail(email);

                var media = _mediaRepository.GetItem(itemID);
                
                if (media == null)
                {
                    return ResultFactory.Fail<DateTime>($"No item with ID {itemID} exists.");
                }

                if (_checkoutRepository.IsItemCheckedOut(itemID))
                {
                    return ResultFactory.Fail<DateTime>("That item is not available for checkout at this time.");
                }
                
                var log = _checkoutRepository.Checkout(itemID, borrower);
                
                if (log == null)
                {
                    return ResultFactory.Fail<DateTime>("Error during checkout.");
                }
                return ResultFactory.Success<DateTime>(log.Value);

            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<DateTime>(ex.Message);
            }
        }

        public Result<List<Media>> GetAvailableMedia()
        {
            try
            {
                var availableMedia = _checkoutRepository.GetAvailableMedia();
                if (availableMedia.Count == 0)
                {
                    return ResultFactory.Fail<List<Media>>("No available media was found.");
                }
                return ResultFactory.Success(availableMedia);
            }
            catch(Exception ex)
            {
                return ResultFactory.Fail<List<Media>>(ex.Message);
            }
        }

        public Result<Borrower> GetBorrowerByEmail(string email)
        {
            try
            {
                var borrower = _checkoutRepository.GetBorrowerByEmail(email);
                if (borrower == null)
                {
                    return ResultFactory.Fail<Borrower>($"Borrower with email {email} was not found!");
                }
                return ResultFactory.Success(borrower);
            }
            catch(Exception ex)
            {
                return ResultFactory.Fail<Borrower>(ex.Message);
            }
        }

        

        public Result<List<CheckoutLog>> GetAllCheckedOutMedia()
        {
            try
            {
                var checkedOutMedia = _checkoutRepository.AllCheckedOutItems();
                if (checkedOutMedia.Count() == 0)
                {
                    return ResultFactory.Fail<List<CheckoutLog>>("No checked out media was found.");
                }
                return ResultFactory.Success(checkedOutMedia);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
            }
        }

        public Result Return(int itemID)
        {
            try
            {
                var returnItem = _checkoutRepository.GetCheckedOutItem(itemID);
                
                if (returnItem == null) 
                {
                    return ResultFactory.Fail($"Item ID {itemID} is not currently checked out. Try again.");
                }
                
                _checkoutRepository.Return(itemID);

                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }

        public Result<List<CheckoutLog>> GetBorrowerCheckedOutItems(Borrower borrower)
        {
            try
            {
                var logs = _checkoutRepository.GetBorrowerCheckoutLog(borrower);
                if (logs.Count == 0)
                {
                    return ResultFactory.Fail<List<CheckoutLog>>($"{borrower.Email} does not have any items checked out currently.");
                }
                return ResultFactory.Success(logs);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<CheckoutLog>>(ex.Message);
            }
        }

        public Result<bool> IsBorrowerAtLimit(string email)
        {
            try
            {
                var borrower = _checkoutRepository.GetBorrowerByEmail(email);
                var isAtMaxItems = _checkoutRepository.IsAtMaxItems(borrower);

                if (isAtMaxItems)
                {
                    return ResultFactory.Fail<bool>("Borrower already has 3 items checked out. Limit is 3 at a time.");
                }
                return ResultFactory.Success(false);
            }
            catch(Exception ex)
            {
                return ResultFactory.Fail<bool>(ex.Message);
            }
        }

        public Result GetBorrowerStatus(string email)
        {
            try
            {
                var borrower = GetBorrowerByEmail(email);
                if (borrower == null)
                {
                    return ResultFactory.Fail(borrower.Message);
                }

                var overdue = _checkoutRepository.HasOverdueItems(borrower.Data);
                if (overdue)
                {
                    return ResultFactory.Fail("Borrower has overdue items that must be returned prior to checking out new material.");
                }

                var maxItems = _checkoutRepository.IsAtMaxItems(borrower.Data);
                if (maxItems)
                {
                    return ResultFactory.Fail("Borrower already has 3 items checked out. Limit is 3 at a time.");
                }

                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }
    }
}
