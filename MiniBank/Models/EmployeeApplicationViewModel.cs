using System;
using System.ComponentModel.DataAnnotations;

namespace MiniBank.Models
{
    public class EmployeeApplicationViewModel
    {
        public int ApplicationID { get; set; }

        [Display(Name = "Applicant Name")]
        public string? ApplicantFullName { get; set; }

        [Display(Name = "Applicant Email")]
        public string? ApplicantEmail { get; set; }

        [Display(Name = "Company Structure")]
        public string? CompanyStructure { get; set; }

        public string? Status { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime SubmittedAt { get; set; }
    }
}