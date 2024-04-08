using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;

namespace LibraryManagement.ConsoleUI.IO
{
    public class CheckoutWorkflows
    {
        public static void CheckoutMenuChoice(ICheckoutService service)
        {
            int choice = 0;

            while (choice != 4)
            {
                choice = Menus.CheckoutManagementMenu();

                switch (choice)
                {
                    case 1:
                        Checkout(service);
                        break;
                    case 2:
                        Return(service);
                        break;
                    case 3:
                        ViewCheckoutLog(service);
                        break;
                    case 4: //go back
                        return;
                }
            }
        }

        public static void GetAvailableMedia(ICheckoutService service)
        {
            var result = service.GetAvailableMedia();

            Console.Clear();
            Console.WriteLine("Available Media List");
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Type",-12}");
            Console.WriteLine(new string('=', 85));

            if (result.Ok)
            {

                foreach (var m in result.Data)
                {
                    Console.WriteLine($"{m.MediaID,-5} {m.Title,-32} {m.MediaType.MediaTypeName,-12}");
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }

        public static void ViewCheckoutLog(ICheckoutService service)
        {
            var result = service.GetAllCheckedOutMedia();

            Console.Clear();
            Console.WriteLine("Checked Out Media List");
            Console.WriteLine();
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Checkout Date",-15} {"Due Date",-15} {"Borrower Name",-32} {"Borrower Email",-32}");
            Console.WriteLine(new string('=', 140));

            if (result.Ok)
            {

                foreach (var cl in result.Data)
                {
                    Console.Write($"{cl.MediaID,-5} {cl.Media.Title,-32} {cl.CheckoutDate,-15:d}");
                    if (cl.DueDate < DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($" {cl.DueDate,-15:d} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($" {cl.DueDate,-15:d} ");
                    }
                    Console.Write($"{cl.Borrower.FirstName + " " + cl.Borrower.LastName,-32} {cl.Borrower.Email,-32}\n");
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();

        }

        public static void Checkout(ICheckoutService service)
        {
            Console.Clear();

            var email = Utilities.GetRequiredString("Enter borrower email: ");
            var borrowerStatus = service.GetBorrowerStatus(email);

            if (borrowerStatus.Ok)
                do
                {
                    GetAvailableMedia(service);

                    var id = Utilities.GetPositiveInteger("\nEnter item ID: ");
                    var checkoutResult = service.Checkout(id, email);

                    if (checkoutResult.Ok)
                    {
                        Console.WriteLine($"Item ID {id} is checked out with due date of {checkoutResult.Data.Date:d}.");
                        if (Utilities.Continue("\nDo you want to checkout another item? Y/N: "))
                        {
                            var atLimit = service.IsBorrowerAtLimit(email);
                            if (atLimit.Ok)
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine(atLimit.Message);
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine(checkoutResult.Message);
                        break;
                    }
                } while (true);
            else
            {
                Console.WriteLine(borrowerStatus.Message);
            }

            Utilities.AnyKey();
        }

        public static void Return(ICheckoutService service)
        {
            do
            {
                Console.Clear();

                var email = Utilities.GetRequiredString("Enter borrower email: ");
                var borrowerResult = service.GetBorrowerByEmail(email);

                if (borrowerResult.Ok)
                {
                    do
                    {
                        var borrowedItems = service.GetBorrowerCheckedOutItems(borrowerResult.Data);
                        if (borrowedItems.Ok)
                        {
                            DisplayItemsCheckedOutToBorrower(borrowedItems.Data);

                            var itemID = Utilities.GetPositiveInteger("\nSelect the item ID being returned: ");

                            var returnResult = service.Return(itemID);
                            if (returnResult.Ok)
                            {
                                Console.WriteLine("Item has been returned. Thank you!");
                                if (Utilities.Continue("\nDo you want to return another item? Y/N: "))
                                {
                                    continue;
                                }
                                return;
                            }
                            else
                            {
                                Console.WriteLine(returnResult.Message);
                            }
                        }
                        else
                        {
                            Console.Write(borrowedItems.Message);
                            Utilities.AnyKey();
                            return;
                        }

                    } while (true);
                }
                else
                {
                    Console.WriteLine(borrowerResult.Message);
                }
                Utilities.AnyKey();
            } while (true);
        }

        public static void DisplayItemsCheckedOutToBorrower(List<CheckoutLog> borrowerItems)
        {
            Console.Clear();
            Console.WriteLine($"Items Checked Out to Borrower: ");
            Console.WriteLine();
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Media Type",-15} {"Checkout Date",-15} {"Due Date",-15}");
            Console.WriteLine(new string('=', 100));

            foreach (var item in borrowerItems)
            {
                Console.Write($"{item.MediaID,-5} {item.Media.Title,-32} {item.Media.MediaType.MediaTypeName,-15} {item.CheckoutDate,-15:d} ");
                if (item.DueDate < DateTime.Today)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{item.DueDate,-15:d}\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($"{item.DueDate,-15:d}\n");
                }
            }
        }
    }
}
