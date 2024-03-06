
namespace LibraryManagement.Core.Entities
{
    public class CheckoutLog
    {
        public int CheckoutLogID { get; set; }
        public int BorrowerID { get; set; }
        public Borrower? Borrower { get; set; } = new Borrower();
        public int MediaID { get; set; }
        public Media? Media { get; set; } = new Media();
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
