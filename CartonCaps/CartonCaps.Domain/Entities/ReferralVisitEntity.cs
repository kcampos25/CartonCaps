namespace CartonCaps.Domain.Entities
{
    public class ReferralVisitEntity
    {
        public Guid Id { get; set; }

        //Date of click on the link
        public DateTime VisitedAt { get; set; }

        //IP address from where the link was clicked
        public string? IpAddress { get; set; }

        //Relationship with the referrals associated.
        public Guid ReferralId { get; set; }
    }
}
