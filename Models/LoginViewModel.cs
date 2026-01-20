namespace SSO.Client.VAMS.Models
{
    /// <summary>
    /// View model for the login form.
    /// Contains only the credentials required by the SSO API.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Username/login id.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Clear-text password. This is posted to the SSO API over HTTPS.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
