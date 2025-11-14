namespace CartonCaps.Infrastructure.Data
{
    /// <summary>
    /// A model that simulates the application's Referral Visit table to monitor 
    /// each click on the link and prevent abuse.
    /// </summary>
    public class ReferralVisit
    {
        //Unique identifier.
        public Guid Id { get; set; }

        //Date on which the link is Visited.
        public DateTime VisitedAt { get; set; }

        //IP address from where the link was opened.
        public string? IpAddress { get; set; }

        //Relation to the Referral table
        public Guid ReferralId { get; set; }
        
        //Property to directly access the Referral table.
        public Referral Referral { get; set; } = null!;
    }
}
