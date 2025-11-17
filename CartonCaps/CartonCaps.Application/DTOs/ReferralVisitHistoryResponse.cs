namespace CartonCaps.Application.DTOs
{
    public class ReferralVisitHistoryResponse
    {
        //Date of click on the link
        public DateTime VisitedAt { get; set; }

        //IP address from where the link was clicked
        public string? IpAddress { get; set; }

        //User who sends the referral
        public string ReferrerUser { get; set; } = null!;
    }
}
