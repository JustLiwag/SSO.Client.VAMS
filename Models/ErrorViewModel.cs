namespace SSO.Client.VAMS.Models
{
    /// <summary>
    /// View model used for the error page to display request id and optionally aid diagnostics.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// The current request identifier (if available).
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Helper property used by the Error view to determine whether to show the request id.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
