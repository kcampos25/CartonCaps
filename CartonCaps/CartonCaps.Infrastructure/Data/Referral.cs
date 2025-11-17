namespace CartonCaps.Infrastructure.Data
{
    /// <summary>
    /// A model that simulates the Referral table, which is responsible for recording each invitation 
    /// </summary>
    public class Referral
    {
        //Unique identifier.
        public Guid Id { get; set; }

        //Invitation link.
        public string LinkGenerated { get; set; } = null!;

        //Referral status (Pending / Completed).
        public string Status { get; set; } = "Pending";

        //Name of the invited person.
        public string? InviteName { get; set; }

        //Referral code generated for each invitation made via Email, SMS or Share
        public string ReferralCode { get; set; } = null!;

        //record creation date
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Relation to the User table
        public Guid ReferrerUserId { get; set; }

        //Property to directly access the User table.
        public User ReferrerUser { get; set; } = null!;

        //Referral Visit Associates
        public ICollection<ReferralVisit> Visits { get; set; } = new List<ReferralVisit>();
    }
}
