namespace LibraryManagement.ConsoleUI.IO
{
    public class Menus
    {
        public static int MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Library Manager Main Menu");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. Borrower Managememt");
            Console.WriteLine("2. Media Management");
            Console.WriteLine("3. Checkout Management");
            Console.WriteLine("");
            Console.WriteLine("0. Quit");

            int choice;

            do
            {
                Console.Write("Enter your choice (0-3): ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice >= 0 && choice <= 3)
                    {
                        return choice;
                    }
                }

                Console.WriteLine("Invalid choice!");
            } while (true);
        }

        public static int BorrowerManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("Borrower Management");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. List All Borrowers");
            Console.WriteLine("2. View a Borrower");
            Console.WriteLine("3. Edit a Borrower");
            Console.WriteLine("4. Add a Borrower");
            Console.WriteLine("5. Delete a Borrower");
            Console.WriteLine("6. Go Back");

            Console.WriteLine("");

            int choice;

            do
            {
                Console.Write("Enter your choice (1-6): ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice >= 1 && choice <= 6)
                    {
                        return choice;
                    }
                }

                Console.WriteLine("Invalid choice!");
            } while (true);
        }

        public static int MediaManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("Media Management");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. List Media");
            Console.WriteLine("2. Add Media");
            Console.WriteLine("3. Edit Media");
            Console.WriteLine("4. Archive Media");
            Console.WriteLine("5. View Archive");
            Console.WriteLine("6. Most Popular Media Report");
            Console.WriteLine("7. Go Back");

            Console.WriteLine("");

            int choice;

            do
            {
                Console.Write("Enter your choice (1-7): ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice >= 1 && choice <= 7)
                    {
                        return choice;
                    }
                }

                Console.WriteLine("Invalid choice!");
            } while (true);
        }

        public static int CheckoutManagementMenu()
        {
            Console.Clear();
            Console.WriteLine("Checkout Management");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. Checkout");
            Console.WriteLine("2. Return");
            Console.WriteLine("3. Checkout Log");
            Console.WriteLine("4. Go Back");

            Console.WriteLine("");

            int choice;

            do
            {
                Console.Write("Enter your choice (1-4): ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice >= 1 && choice <= 4)
                    {
                        return choice;
                    }
                }

                Console.WriteLine("Invalid choice!");
            } while (true);
        }
    }
}
