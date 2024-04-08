using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Services;

namespace LibraryManagement.ConsoleUI.IO
{
    public class BorrowerWorkflows
    {

        public static void BorrowerMenuChoice(IBorrowerService service)
        {
            int choice = 0;

            while (choice != 6)
            {
                choice = Menus.BorrowerManagementMenu();

                switch (choice)
                {

                    case 1:
                        GetAllBorrowers(service);
                        break;
                    case 2:
                        GetBorrowerByEmail(service); //view borrower and see current items checked out
                        break;
                    case 3:
                        EditBorrower(service);
                        break;
                    case 4:
                        AddBorrower(service);
                        break;
                    case 5:
                        DeleteBorrower(service);
                        break;
                    case 6:
                        return;
                }
            }
        }

        public static void GetAllBorrowers(IBorrowerService service)
        {
            Console.Clear();
            Console.WriteLine("Borrower List");
            Console.WriteLine($"{"ID",-5} {"Name",-32} {"Email",-32}");
            Console.WriteLine(new string('=', 85));
            var result = service.GetAllBorrowers();

            if (result.Ok)
            {

                foreach (var b in result.Data)
                {
                    Console.WriteLine($"{b.BorrowerID,-5} {b.LastName + ", " + b.FirstName,-32} {b.Email,-32}");
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }

        public static void GetBorrowerByEmail(IBorrowerService service)
        {
            Console.Clear();
            var email = Utilities.GetRequiredString("\nEnter borrower email: ");
            var result = service.GetBorrower(email);

            if (result.Ok)
            {
                Console.WriteLine("\nBorrower Information");
                Console.WriteLine("====================");
                Console.WriteLine($"Id: {result.Data.BorrowerID}");
                Console.WriteLine($"Name: {result.Data.LastName}, {result.Data.FirstName}");
                Console.WriteLine($"Email: {result.Data.Email}");
                Console.WriteLine("\n\n");

                GetBorrowedItems(service, email);
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }

        public static void AddBorrower(IBorrowerService service)
        {
            Console.Clear();
            Console.WriteLine("Add New Borrower");
            Console.WriteLine("====================");

            Borrower newBorrower = new Borrower();

            newBorrower.FirstName = Utilities.GetRequiredString("First Name: ");
            newBorrower.LastName = Utilities.GetRequiredString("Last Name: ");
            newBorrower.Email = Utilities.GetRequiredString("Email: ");
            newBorrower.Phone = Utilities.GetRequiredString("Phone: ");

            var result = service.AddBorrower(newBorrower);

            if (result.Ok)
            {
                Console.WriteLine($"Borrower created with id: {result.Data}");
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }

        public static void DeleteBorrower(IBorrowerService service)
        {

            Console.Clear();
            var email = Utilities.GetRequiredString("\nEnter borrower email: ");
            var result = service.GetBorrower(email);

            if (result.Ok)
            {
                var confirmation = Utilities.DeleteConfirmed();
                if (confirmation)
                {
                    var removeResult = service.RemoveBorrower(result.Data);
                    if (removeResult.Ok)
                    {
                        Console.WriteLine($"Borrower with email {email} was removed.");
                    }
                    else
                    {
                        Console.WriteLine(removeResult.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Utilities.AnyKey();
        }

        public static void EditBorrower(IBorrowerService service)
        {
            Console.Clear();
            var email = Utilities.GetRequiredString("\nEnter borrower email: ");
            var borrower = service.GetBorrower(email);

            if (borrower.Ok)
            {
                Console.WriteLine("\nBorrower Information");
                Console.WriteLine("====================");
                Console.WriteLine($"Id: {borrower.Data.BorrowerID}");
                Console.WriteLine($"Name: {borrower.Data.LastName}, {borrower.Data.FirstName}");
                Console.WriteLine($"Email: {borrower.Data.Email}");

                Console.WriteLine();

                var input = Utilities.GetRequiredString("Which info do you want to edit? (F)irst Name, (L)ast Name, or (E)mail: ").ToUpper();

                switch (input)
                {
                    case "F":
                        borrower.Data.FirstName = Utilities.GetRequiredString("Enter new first name: ");
                        break;
                    case "L":
                        borrower.Data.LastName = Utilities.GetRequiredString("Enter new last name: ");
                        break;
                    case "E":
                        borrower.Data.Email = Utilities.GetRequiredString("Enter new email: ");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        return;
                }

                var editResult = service.EditBorrower(borrower.Data, email);

                if (editResult.Ok)
                {
                    Console.WriteLine($"\nRecord has been successfully updated.");
                }
                else
                {
                    Console.WriteLine(editResult.Message);
                }
            }
            else
            {
                Console.WriteLine(borrower.Message);
            }

            Utilities.AnyKey();
        }

        public static void GetBorrowedItems(IBorrowerService service, string email)
        {
            Console.WriteLine("Current Borrowed Items");
            Console.WriteLine($"{"ID",-5} {"Title",-32} {"Checkout Date",-15} {"Due Date",-15}");
            Console.WriteLine(new string('=', 80));

            var result = service.GetBorrowedItems(email);

            if (result.Ok)
            {
                foreach (var m in result.Data)
                {
                    Console.Write($"{m.MediaID,-5} {m.Media.Title,-32} {m.CheckoutDate,-15:d}");
                    if (m.DueDate < DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($" {m.DueDate,-15:d}\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($" {m.DueDate,-15:d}\n");
                    }
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }
    }
}
