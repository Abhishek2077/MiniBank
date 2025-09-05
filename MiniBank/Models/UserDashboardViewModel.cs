using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MiniBank.Models
{
    public class UserDashboardViewModel
    {
        // This property is for the new application form
        public CreditApplication NewApplication { get; set; }

        // This property is for the list of submitted applications
        [BindNever]
        public List<CreditApplication> SubmittedApplications { get; set; } = new List<CreditApplication>();
    }
}