
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Core.Entities
{
    public class Media
    {
        public Media()
        {
            Title = "";
            IsArchived = false;
        }
        public int MediaID { get; set; }
        public int MediaTypeID { get; set; }
        public MediaType? MediaType { get; set; } = new MediaType();
        public string Title { get; set; }
        public bool IsArchived { get; set; }

        public List<CheckoutLog> CheckoutLogs { get; set; } = [];
    }
}
