namespace SSO.Client.VAMS.Models
{
    /// <summary>
    /// DTO representing a claim returned by the SSO API (/api/auth/me).
    /// Each claim has a type and a value.
    /// </summary>
    public class UserClaimsDto
    {
        /// <summary>
        /// Claim type (e.g., role, email).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Claim value.
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
