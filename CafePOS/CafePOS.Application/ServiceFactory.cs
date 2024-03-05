using CafePOS.Application.Mocks;
using CafePOS.Application.Services;
using CafePOS.Application.TimeOfDaySettings;
using CafePOS.Core.DTOs;
using CafePOS.Core.Interfaces.Application;
using CafePOS.Core.Interfaces.Services;
using CafePOS.Core.TimeOfDaySettings;
using CafePOS.Data.Repositories;
using CafePOS.Data.TrainingRepositories;


namespace CafePOS.Application
{
    public class ServiceFactory
    {
        private IAppConfiguration _config;
        private ITimeOfDaySetting _timeOfDaySetting;
        private TrainingMode _trainingMode;

        public ServiceFactory(IAppConfiguration config)
        {
            _config = config;
            _timeOfDaySetting = GetTimeOfDay();
            _trainingMode = _config.GetTrainingModeSetting();
        }

        public ITimeOfDaySetting GetTimeOfDay()
        {
            switch (_config.GetTimeOfDayMode())
            {
                case TimeOfDayMode.RealTime:
                    return new RealTime();
                case TimeOfDayMode.Breakfast:
                    return new Breakfast();
                case TimeOfDayMode.Lunch:
                    return new Lunch();
                case TimeOfDayMode.Dinner:
                    return new Dinner();
                default:
                case TimeOfDayMode.HappyHour:
                    return new HappyHour();
            }
        }
        public ICreateOrderService CreateOrderService()
        {
            if (_trainingMode == TrainingMode.Enabled)
            {
                return new CreateOrderService(new TrainingCreateOrderRepository());
            }
            else
            {
                return new CreateOrderService(
                    new CreateOrderRepository(_config.GetConnectionString()));
            }
        }

        public IOpenOrderService CreateOpenOrderService()
        {
            if (_trainingMode == TrainingMode.Enabled)
            {
                return new OpenOrderService(new TrainingOpenOrderRepository(), _timeOfDaySetting);
            }
            else
            {
                return new OpenOrderService(
                new OpenOrderRepository(_config.GetConnectionString()), _timeOfDaySetting);
            }
        }

        public ICancelOrderService CreateCancelOrderService()
        {
            if (_trainingMode == TrainingMode.Enabled)
            {
                return new CancelOrderService(new TrainingCancelOrderRepository(), new TrainingOpenOrderRepository());
            }
            else
            {
                return new CancelOrderService(
                new CancelOrderRepository(_config.GetConnectionString()), new OpenOrderRepository(_config.GetConnectionString()));
            }
        }

        public IPaymentService CreatePaymentService()
        {
            if (_trainingMode == TrainingMode.Enabled)
            {
                return new PaymentService(new TrainingPaymentRepository());
            }
            else
            {
                return new PaymentService(
                new PaymentRepository(_config.GetConnectionString()));
            }
        }

        public IReportService CreateReportService()
        {
            if (_trainingMode == TrainingMode.Enabled)
            {
                return new ReportService(new TrainingReportRepository());
            }
            else
            {
                return new ReportService(
                new ReportRepository(_config.GetConnectionString()));
            }
        }
    }
}
