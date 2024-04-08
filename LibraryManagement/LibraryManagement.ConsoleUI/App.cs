using LibraryManagement.Application;
using LibraryManagement.ConsoleUI.IO;
using LibraryManagement.Core.Interfaces.Application;

namespace LibraryManagement.ConsoleUI
{
    public class App
    {
        readonly IAppConfiguration _config;
        readonly ServiceFactory _serviceFactory;

        public App()
        {
            _config = new AppConfiguration();
            _serviceFactory = new ServiceFactory(_config);
        }

        public void Run()
        {
            do
            {
                int choice = Menus.MainMenu();

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        BorrowerWorkflows.BorrowerMenuChoice(_serviceFactory.CreateBorrowerService());
                        break;
                    case 2:
                        MediaWorkflows.MediaMenuChoice(_serviceFactory.CreateMediaService());
                        break;
                    case 3:
                        CheckoutWorkflows.CheckoutMenuChoice(_serviceFactory.CreateCheckoutService());
                        break;
                }
            } while (true);
        }
    }
}
