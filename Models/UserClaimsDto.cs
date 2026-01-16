namespace SSO.Client.VAMS.Models
{
    /// Represents user claims from /api/auth/me
    public class UserClaimsDto
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
