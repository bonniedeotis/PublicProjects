using CafePOS.Core.DTOs;
using CafePOS.Core.Entities;
using CafePOS.Core.Interfaces.Repositories;
using CafePOS.Core.Interfaces.Services;

namespace CafePOS.Application.Services
{
    public class CreateOrderService : ICreateOrderService
    {
        private ICreateOrderRepository _orderRepository;

        public CreateOrderService(ICreateOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Result<List<Server>> GetServerList()
        {
            try
            {
                var servers = _orderRepository.GetAvailableServers();

                if (servers.Count() == 0)
                {
                    return ResultFactory.Fail<List<Server>>("No servers are available currently.");
                }
                else
                {
                    return ResultFactory.Success(servers);
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<List<Server>>(ex.Message);
            }

        }

        public Result<Server> GetServer(int ID)
        {
            try
            {
                var server = _orderRepository.GetServer(ID);

                if (server == null)
                {
                    return ResultFactory.Fail<Server>($"Server ID {ID} is not available currently.");
                }
                else
                {
                    return ResultFactory.Success(server);
                }
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<Server>(ex.Message);
            }
        }

        public Result<int> CreateNewOrder(Server server)
        {
            try
            {
                var orderId = _orderRepository.NewOrder(server);

                return ResultFactory.Success(orderId);
            }
            catch (Exception ex)
            {
                return ResultFactory.Fail<int>(ex.Message);
            }

        }
    }
}
