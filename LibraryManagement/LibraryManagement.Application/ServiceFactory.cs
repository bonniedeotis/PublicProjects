using LibraryManagement.Application.Services;
using LibraryManagement.Core.Interfaces.Application;
using LibraryManagement.Core.Interfaces.Services;
using LibraryManagement.Data.Repositories;
using LibraryManagement.Data.Repositories.DapperAndADO;
using LibraryManagement.Data.Repositories.EF_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application
{
    public class ServiceFactory
    {
        private readonly IAppConfiguration _config;

        public ServiceFactory(IAppConfiguration config)
        {
            _config = config;
        }

        public IBorrowerService CreateBorrowerService()
        {
            var mode = _config.GetDatabaseMode();

            if (mode == DatabaseMode.ORM)
            {
                return new BorrowerService(
                    new EFBorrowerRepository(_config.GetConnectionString()));
            }
            else
            {
                return new BorrowerService(
                    new DapperBorrowerRepository(_config.GetConnectionString()));
            }
        }

        public IMediaService CreateMediaService()
        {
            var mode = _config.GetDatabaseMode();

            if (mode == DatabaseMode.ORM)
            {
                return new MediaService(
                    new EFMediaRepository(_config.GetConnectionString()));
            }
            else
            {
                return new MediaService(
                    new DapperMediaRepository(_config.GetConnectionString()));
            }
        }

        public ICheckoutService CreateCheckoutService()
        {
            var mode = _config.GetDatabaseMode();

            if (mode == DatabaseMode.ORM)
            {
                return new CheckoutService(
                    new EFCheckoutRepository(_config.GetConnectionString()), new EFMediaRepository(_config.GetConnectionString()));
            }
            else
            {
                return new CheckoutService(
                    new DapperCheckoutRepository(_config.GetConnectionString()), new DapperMediaRepository(_config.GetConnectionString()));
            }
        }
    }
}
