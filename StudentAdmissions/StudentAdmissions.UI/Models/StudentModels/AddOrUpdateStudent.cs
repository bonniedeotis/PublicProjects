using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentAdmissions.UI.Models.StudentModels
{
    public class AddOrUpdateStudent
    {
        public int StudentID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Alias { get; set; }


        [DisplayName("Date of Birth")]
        //[ValidateDate("Date is not supported")]
        [Required]
        public DateOnly? DoB { get; set; }

    }

    //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValidateDateAttribute : ValidationAttribute
    {
        public ValidateDateAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object input, ValidationContext validationContext)
        {
            DateOnly date = (DateOnly)input;
            DateOnly priorToToday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            DateOnly earliestDate = DateOnly.FromDateTime(new DateTime(1900, 1, 1));

            if (date <= priorToToday)
            {
                if (date >= earliestDate)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("Date is not supported");
        }
    }
}
