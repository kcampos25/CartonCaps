namespace CartonCaps.Application.DTOs
{
    public class CreateReferralInviteRequest
    {
        //ID of the user who sends the referral
        public Guid ReferrerUserId { get; set; }
    }
}
