using CafePOS.Core.DTOs;
using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;
using CafePOS.Core.Interfaces.Services;


namespace CafePOS.Application.Services
{
    public class CancelOrderService : ICancelOrderService
    {
        private ICancelOrderRepository _cancelOrderRepository;
        private IOpenOrderRepository _openOrderRepository;

        public CancelOrderService(ICancelOrderRepository cancelOrderRepository, IOpenOrderRepository openOrderRepository)
        {
            _cancelOrderRepository = cancelOrderRepository;
            _openOrderRepository = openOrderRepository;
        }

        public Result<List<CafeOrder>> GetOpenOrders()
        {
            try
            {
                var orders = _cancelOrderRepository.GetOpenOrders();
                if (orders.Count == 0)
                {
                    return ResultFactory.Fail<List<CafeOrder>>("There are no orders open currently.");
                }
                return ResultFactory.Success(orders);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<CafeOrder>>(ex.Message);
            }
        }

        public Result<bool> IsValidOpenOrder(int orderId)
        {
            try
            {
                var isValid = _openOrderRepository.IsValidOrderNumber(orderId);
                var isOpen = _openOrderRepository.IsOrderOpen(orderId);

                if (!isValid)
                {
                    return ResultFactory.Fail<bool>("That is not a valid order number.");
                }

                if (!isOpen)
                {
                    return ResultFactory.Fail<bool>("That order is not open currently.");
                }
                else
                {
                    return ResultFactory.Success(isOpen);
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<bool>(ex.Message);
            }
        }

        public Result CancelOrder(int orderId)
        {
            try
            {
                _cancelOrderRepository.CancelOrder(orderId);
                return ResultFactory.Success();
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail(ex.Message);
            }
        }
    }
}
