namespace SSO.Client.VAMS.Models
{
    /// <summary>
    /// Represents a row returned by the database view <c>vw_PersonnelDivisionDetails</c>.
    /// This model is read-only as it maps to a database view.
    /// </summary>
    public class PersonnelDivisionDetail
    {
        /// <summary>
        /// Employee identifier. The type is string here because many HR systems use alphanumeric employee IDs.
        /// Change to int if your database uses numeric IDs.
        /// </summary>
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>
        /// Surname / family name.
        /// </summary>
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Given / first name.
        /// </summary>
        public string GivenName { get; set; } = string.Empty;

        /// <summary>
        /// Name of the division the employee belongs to.
        /// </summary>
        public string DivisionName { get; set; } = string.Empty;
    }
}