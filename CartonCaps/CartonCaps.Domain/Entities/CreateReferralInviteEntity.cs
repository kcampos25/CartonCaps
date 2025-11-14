namespace CartonCaps.Domain.Entities
{
    public class CreateReferralInviteEntity
    {
        //Link generated with referral code
        public string LinkGenerated { get; set; } = null!;
        //The referral code is handled separately so that the app can use it for other functionality.
        public string ReferralCode { get; set; } = null!;
    }
}
