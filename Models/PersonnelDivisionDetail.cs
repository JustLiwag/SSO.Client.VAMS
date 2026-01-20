namespace SSO.Client.VAMS.Models
{
    /// <summary>
    /// Represents a row returned by the database view <c>vw_PersonnelDivisionDetails</c>.
    /// This model is read-only as it maps to a database view.
    /// </summary>
    public class PersonnelDivisionDetail
    {
        public int hris_id { get; set; }
        public string employee_id { get; set; } = null!;
        public string surname { get; set; } = string.Empty;
        public string given_name { get; set; } = string.Empty;
        public DateTime? separation_date { get; set; }
        public string division_name { get; set; } = string.Empty;
    }
}