using System.Collections.Generic;

namespace SSO.Client.VAMS.Models
{
    // Represents data to display on the dashboard
    public class DashboardViewModel
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Division { get; set; }
        public List<ClaimViewModel> Claims { get; set; }
    }
}
