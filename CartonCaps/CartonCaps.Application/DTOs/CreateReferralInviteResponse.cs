namespace CartonCaps.Application.DTOs
{
    public class CreateReferralInviteResponse
    {
        //Link generated with referral code
        public string LinkGenerated { get; set; } = null!;

        //The referral code is sent separately so that the app can use it for other functionality.
        public string ReferralCode { get; set; } = null!;
    }
}
