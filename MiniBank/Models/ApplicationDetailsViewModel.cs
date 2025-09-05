namespace MiniBank.Models
{
    public class ApplicationDetailsViewModel
    {
        public int ApplicationID { get; set; }
        public string ApplicantFullName { get; set; }
        public string ApplicantEmail { get; set; }
        public string CompanyStructure { get; set; }
        public DateTime BusinessSince { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
        public string? BankBranch { get; set; }
        public string? BankState { get; set; }
        public string? BankCity { get; set; }
        public string? BankZipCode { get; set; }
        public string? LogoImagePath { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}