using System.ComponentModel.DataAnnotations;

namespace MiniBank.Models
{
    public class CreditApplication
    {
        public int ApplicationID { get; set; }
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please select a company structure.")]
        [Display(Name = "Company Structure")]
        public string CompanyStructure { get; set; }

        [Required(ErrorMessage = "Please enter the date.")]
        [Display(Name = "In Business Since")]
        [DataType(DataType.Date)]
        public DateTime? BusinessSince { get; set; } // Make nullable for validation

        [Required(ErrorMessage = "Please enter a phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a street address.")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "The State field is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter a Zip Code.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "Please select a bank.")]
        public string? BankName { get; set; }

        [Display(Name = "Account Number")]
        [Required(ErrorMessage = "Please enter your account number.")]
        [StringLength(18, ErrorMessage = "Account number cannot exceed 18 digits.")]
        public string? AccountNumber { get; set; }

        [Display(Name = "IFSC Code")]
        [Required(ErrorMessage = "Please select IFSC code.")]
        public string? IFSCCode { get; set; }

        [Display(Name = "Bank Branch")]
        [Required(ErrorMessage = "Please select a branch.")]
        public string? BankBranch { get; set; }

        [Display(Name = "Bank State")]
        [Required(ErrorMessage = "Please select bank state.")]
        public string? BankState { get; set; }

        [Display(Name = "Bank City")]
        [Required(ErrorMessage = "Please select bank city.")]
        public string? BankCity { get; set; }

        [Display(Name = "Bank Zip Code")]
        [Required(ErrorMessage = "Please enter bank zip code.")]
        public string? BankZipCode { get; set; }

        public string? LogoImagePath { get; set; } // No [Required] for logoFile
        public string? Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}