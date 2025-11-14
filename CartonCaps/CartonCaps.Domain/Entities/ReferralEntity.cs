namespace CartonCaps.Domain.Entities
{
    public class ReferralEntity
    {
        public Guid Id { get; set; }

        //Invitation link.
        public string LinkGenerated { get; set; } = null!;

        //Referral status (Pending / Completed).
        public string Status { get; set; } = "Pending";

        //Name of the invited person.
        public string? InviteName { get; set; }

        //Referral code generated for each invitation made via Email, SMS or Share
        public string InvitationCode { get; set; } = null!;

        //record creation date
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Relation to the User table
        public Guid ReferrerUserId { get; set; }
    }
}
