namespace SSO.Client.VAMS.Models
{
    /// <summary>
    /// DTO representing the login response returned by the SSO API.
    /// Contains the access token and basic user details returned by the SSO system.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Bearer token to be used in API calls that require authentication.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Employee number/identifier (used by the local DB view for lookup).
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// Full display name of the user provided by the SSO API.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Division name provided by the SSO API (may differ from local DB).
        /// </summary>
        public string Division { get; set; } = string.Empty;
    }
}
