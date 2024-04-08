namespace LibraryManagement.ConsoleUI.IO
{
    public class Utilities
    {
        public static void AnyKey()
        {
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static int GetPositiveInteger(string prompt)
        {
            int result;

            do
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out result))
                {
                    if (result > 0)
                    {
                        return result;
                    }
                }

                Console.WriteLine("Invalid input, must be a positive integer!");
                AnyKey();
            } while (true);
        }

        public static string GetRequiredString(string prompt)
        {
            string? input;

            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Input is required.");
                AnyKey();
            } while (true);
        }

        public static bool DeleteConfirmed()
        {
            string? input;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nProceed with deletion of borrower account? Y/N: ");
            Console.ResetColor();
            input = Console.ReadLine().ToUpper();
            if (!string.IsNullOrWhiteSpace(input) && input == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DisplayTypeOptions()
        {
            Console.WriteLine("\nMedia Types");
            Console.WriteLine("--------------");
            Console.WriteLine("1. Book");
            Console.WriteLine("2. DVD");
            Console.WriteLine("3. Digital Media");
            Console.WriteLine();
        }

        public static bool Continue(string prompt)
        {
            var answer = GetRequiredString(prompt).ToUpper();
            if (answer == "Y")
            {
                return true;
            }
            return false;
        }
    }
}
